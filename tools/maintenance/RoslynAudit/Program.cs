using System.Diagnostics;
using System.Text.Json;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

var root = GetOption(args, "--root") ?? Environment.CurrentDirectory;
var output = GetOption(args, "--output");
root = Path.GetFullPath(root);
Environment.CurrentDirectory = root;

var tracked = Git(root, "ls-files", "--", "*.cs")
    .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
var syntaxIssues = new List<object>();
foreach (var relative in tracked)
{
    var path = Path.Combine(root, relative);
    if (!File.Exists(path)) continue;
    var tree = Parse(File.ReadAllText(path), path);
    var errors = tree.GetDiagnostics()
        .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
        .Select(diagnostic => diagnostic.ToString())
        .ToArray();
    if (errors.Length > 0) syntaxIssues.Add(new { file = relative, errors });
}

var changed = Git(root, "diff", "--name-only", "--", "*.cs")
    .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
var tokenChanges = new List<object>();
var baselines = GitBatch(root, changed.Select(path => $"HEAD:{path}").ToArray());
foreach (var relative in changed)
{
    var currentPath = Path.Combine(root, relative);
    if (!File.Exists(currentPath)) continue;
    var oldTokens = CodeTokens(Parse(baselines[relative], relative));
    var newTokens = CodeTokens(Parse(File.ReadAllText(currentPath), currentPath));
    if (!oldTokens.SequenceEqual(newTokens, StringComparer.Ordinal))
        tokenChanges.Add(new { file = relative, old_tokens = oldTokens.Count, new_tokens = newTokens.Count });
}

var sdk = Run("dotnet", "--version");
var facts = new
{
    verified_on = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture),
    sdk,
    checked_tracked_cs_files = tracked.Length,
    syntax_errors = syntaxIssues,
    changed_cs_files = changed.Length,
    files_with_non_text_token_changes = tokenChanges,
    limits = "Syntax parsing and token comparison only; not a build, type check, runtime test, or proof that all string keys are preserved."
};
var json = JsonSerializer.Serialize(facts, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);
if (output is not null)
{
    var destination = Path.GetFullPath(output);
    Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
    File.WriteAllText(destination, json + Environment.NewLine);
}

static string? GetOption(string[] values, string name)
{
    var index = Array.IndexOf(values, name);
    return index >= 0 && index + 1 < values.Length ? values[index + 1] : null;
}

static string Git(string workingDirectory, params string[] arguments)
{
    var start = new ProcessStartInfo("git") { WorkingDirectory = workingDirectory, UseShellExecute = false };
    foreach (var argument in arguments) start.ArgumentList.Add(argument);
    start.ArgumentList.Insert(0, "-c");
    start.ArgumentList.Insert(1, "core.quotepath=false");
    start.RedirectStandardOutput = true;
    start.RedirectStandardError = true;
    using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not start git.");
    var stdoutTask = process.StandardOutput.ReadToEndAsync();
    var stderrTask = process.StandardError.ReadToEndAsync();
    Task.WaitAll(stdoutTask, stderrTask);
    var stdout = stdoutTask.Result;
    var stderr = stderrTask.Result;
    process.WaitForExit();
    if (process.ExitCode != 0) throw new InvalidOperationException($"git {string.Join(' ', arguments)} failed: {stderr}");
    return stdout.TrimEnd('\r', '\n');
}

static Dictionary<string, string> GitBatch(string workingDirectory, string[] objects)
{
    if (objects.Length == 0) return new Dictionary<string, string>(StringComparer.Ordinal);
    var start = new ProcessStartInfo("git") { WorkingDirectory = workingDirectory, UseShellExecute = false };
    start.ArgumentList.Add("-c");
    start.ArgumentList.Add("core.quotepath=false");
    start.ArgumentList.Add("cat-file");
    start.ArgumentList.Add("--batch");
    start.RedirectStandardInput = true;
    start.RedirectStandardOutput = true;
    start.RedirectStandardError = true;
    start.StandardInputEncoding = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    using var process = Process.Start(start) ?? throw new InvalidOperationException("Could not start git cat-file.");
    var stderrTask = process.StandardError.ReadToEndAsync();
    foreach (var objectName in objects) process.StandardInput.WriteLine(objectName);
    process.StandardInput.Close();
    var stream = process.StandardOutput.BaseStream;
    var result = new Dictionary<string, string>(StringComparer.Ordinal);
    for (var index = 0; index < objects.Length; index++)
    {
        var header = ReadLine(stream);
        var fields = header.Split(' ');
        if (fields.Length != 3 || fields[1] != "blob")
            throw new InvalidOperationException($"Invalid git cat-file response: {header}");
        var size = int.Parse(fields[2], System.Globalization.CultureInfo.InvariantCulture);
        var bytes = new byte[size];
        stream.ReadExactly(bytes);
        if (stream.ReadByte() != '\n') throw new InvalidOperationException("Invalid git cat-file content terminator.");
        var path = objects[index]["HEAD:".Length..];
        result[path] = System.Text.Encoding.UTF8.GetString(bytes);
    }
    process.WaitForExit();
    if (process.ExitCode != 0) throw new InvalidOperationException($"git cat-file failed: {stderrTask.Result}");
    return result;
}

static string ReadLine(Stream stream)
{
    var bytes = new List<byte>();
    int value;
    while ((value = stream.ReadByte()) >= 0 && value != '\n') bytes.Add((byte)value);
    if (value < 0) throw new EndOfStreamException("Unexpected end of git cat-file response.");
    return System.Text.Encoding.UTF8.GetString(bytes.ToArray());
}

static string Run(string fileName, params string[] arguments)
{
    var start = new ProcessStartInfo(fileName) { UseShellExecute = false };
    foreach (var argument in arguments) start.ArgumentList.Add(argument);
    start.RedirectStandardOutput = true;
    start.RedirectStandardError = true;
    using var process = Process.Start(start) ?? throw new InvalidOperationException($"Could not start {fileName}.");
    var stdoutTask = process.StandardOutput.ReadToEndAsync();
    var stderrTask = process.StandardError.ReadToEndAsync();
    Task.WaitAll(stdoutTask, stderrTask);
    process.WaitForExit();
    if (process.ExitCode != 0) throw new InvalidOperationException($"{fileName} failed: {stderrTask.Result}");
    return stdoutTask.Result.Trim();
}

static SyntaxTree Parse(string source, string path) =>
    CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), path);

static List<string> CodeTokens(SyntaxTree tree) => tree.GetRoot().DescendantTokens()
    .Select(token => token.RawKind switch
    {
        (int)SyntaxKind.StringLiteralToken or
        (int)SyntaxKind.InterpolatedStringTextToken or
        (int)SyntaxKind.SingleLineRawStringLiteralToken or
        (int)SyntaxKind.MultiLineRawStringLiteralToken => $"{token.RawKind}:<text>",
        _ => $"{token.RawKind}:{token.Text}"
    })
    .ToList();

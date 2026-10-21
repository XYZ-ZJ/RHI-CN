param([string]$OutputFile)
$ErrorActionPreference = 'Stop'
$projectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$auditProject = Join-Path $PSScriptRoot 'RoslynAudit\RoslynAudit.csproj'
$arguments = @('run', '--project', $auditProject, '--', '--root', $projectRoot)
if ($OutputFile) {
    $resolvedOutput = if ([IO.Path]::IsPathRooted($OutputFile)) {
        [IO.Path]::GetFullPath($OutputFile)
    } else {
        [IO.Path]::GetFullPath((Join-Path $projectRoot $OutputFile))
    }
    $arguments += @('--output', $resolvedOutput)
}
& dotnet @arguments
if ($LASTEXITCODE -ne 0) { throw "C# source audit failed with exit code $LASTEXITCODE." }

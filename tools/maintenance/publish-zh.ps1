param([string]$OutputName = ('zh-' + (Get-Date -Format 'yyyyMMdd-HHmmss')))
$ErrorActionPreference = 'Stop'
if ($OutputName -notmatch '^[a-zA-Z0-9][a-zA-Z0-9_-]{0,63}$') {
    throw 'OutputName must be a simple folder name, without paths.'
}
$projectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
$outputDir = Join-Path $projectRoot "artifacts\$OutputName"
if (Test-Path -LiteralPath $outputDir) { throw "Output already exists: $outputDir" }
$artifactRoot = Join-Path $projectRoot 'artifacts'
[IO.Directory]::CreateDirectory($artifactRoot) | Out-Null
$log = Join-Path $artifactRoot "$OutputName-build.log"
$previous = Get-Location
try {
    Set-Location -LiteralPath $projectRoot
    [IO.Directory]::CreateDirectory($outputDir) | Out-Null
    & dotnet publish RenoDXCommander/RenoDXCommander.csproj -c Release -r win-x64 --self-contained true -p:Platform=x64 -p:WindowsAppSDKSelfContained=true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -m:1 -o $outputDir *> $log
    if ($LASTEXITCODE -ne 0) { throw "Main publish failed; see $log" }
    $helperOutput = Join-Path $projectRoot 'RHI.DropHelper\bin\zh-publish'
    & dotnet publish RHI.DropHelper/RHI.DropHelper.csproj -c Release -r win-x64 --self-contained true -p:Platform=x64 -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -m:1 -o $helperOutput *>> $log
    if ($LASTEXITCODE -ne 0) { throw "Drop helper publish failed; see $log" }
    $helperExe = Join-Path $helperOutput 'RHI.DropHelper.exe'
    if (-not (Test-Path -LiteralPath $helperExe -PathType Leaf)) { throw 'Drop helper EXE missing.' }
    Copy-Item -LiteralPath $helperExe -Destination $outputDir
    if (-not (Test-Path -LiteralPath (Join-Path $outputDir 'RHI.exe') -PathType Leaf)) { throw 'Main RHI.exe missing after publish.' }
    [xml]$project = Get-Content -Raw -LiteralPath 'RenoDXCommander/RenoDXCommander.csproj'
    $assets = @($project.Project.ItemGroup.ChildNodes | Where-Object {
        $_.ExcludeFromSingleFile -eq 'true' -and $_.Include -and $_.Include -notmatch '\$\('
    } | ForEach-Object {$_.Include}) + @('reshade.rdr2.ini')
    foreach ($relative in $assets) {
        $source = Join-Path $projectRoot "RenoDXCommander\$relative"
        if (-not (Test-Path -LiteralPath $source -PathType Leaf)) { throw "Source asset missing: $source" }
        $target = Join-Path $outputDir $relative
        [IO.Directory]::CreateDirectory([IO.Path]::GetDirectoryName($target)) | Out-Null
        Copy-Item -LiteralPath $source -Destination $target
    }
    Copy-Item -LiteralPath (Join-Path $projectRoot 'LICENSE') -Destination $outputDir
    Get-ChildItem -LiteralPath $outputDir -Filter '*.pdb' -File -Recurse | Remove-Item -Force
    $missing = @($assets | Where-Object { -not (Test-Path -LiteralPath (Join-Path $outputDir $_) -PathType Leaf) })
    if ($missing.Count -gt 0) { throw "Release files missing after copy: $($missing -join ', ')" }
    if (-not (Test-Path -LiteralPath (Join-Path $outputDir 'RHI.DropHelper.exe') -PathType Leaf)) { throw 'Drop helper missing from release folder.' }
    if (-not (Test-Path -LiteralPath (Join-Path $outputDir 'LICENSE') -PathType Leaf)) { throw 'LICENSE missing from release folder.' }
    $sha256 = [Security.Cryptography.SHA256]::Create()
    try {
        $trackedDiff = (& git diff --binary) -join "`n"
        $untracked = @(& git ls-files --others --exclude-standard | Sort-Object)
        $untrackedFingerprints = foreach ($relative in $untracked) {
            $path = Join-Path $projectRoot $relative
            if (Test-Path -LiteralPath $path -PathType Leaf) {
                "$relative`t$((Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash)"
            }
        }
        $fingerprintInput = $trackedDiff + "`n--UNTRACKED--`n" + ($untrackedFingerprints -join "`n")
        $diffBytes = [Text.Encoding]::UTF8.GetBytes($fingerprintInput)
        $diffHash = [BitConverter]::ToString($sha256.ComputeHash($diffBytes)).Replace('-', '')
    } finally { $sha256.Dispose() }
    $fingerprint = [pscustomobject]@{
        built_at=(Get-Date).ToString('o'); sdk=(& dotnet --version).Trim()
        base_commit=(& git rev-parse HEAD).Trim(); origin=(& git remote get-url origin).Trim()
        runtime_validation='not performed'; localization='partial; see maintenance audit'
        working_tree_diff_sha256=$diffHash
        files=@(Get-ChildItem -LiteralPath $outputDir -Recurse -File | Where-Object {$_.Extension -ne '.pdb'} | ForEach-Object {
            [pscustomobject]@{path=$_.FullName.Substring($outputDir.Length).TrimStart('\'); bytes=$_.Length; sha256=(Get-FileHash -LiteralPath $_.FullName).Hash}
        })
    }
    [IO.File]::WriteAllText((Join-Path $outputDir 'release-manifest.json'), ($fingerprint | ConvertTo-Json -Depth 8) + "`n", [Text.UTF8Encoding]::new($false))
    Write-Output "Published to $outputDir. Runtime and localization acceptance remain required."
} finally { Set-Location $previous }

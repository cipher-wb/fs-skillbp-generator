param(
    [string]$AuditorPath = (Join-Path $PSScriptRoot "skill-json-auditor.ps1")
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $PSScriptRoot
$cases = @(
    @{ Path = (Join-Path $root "tests\\fixtures\\valid-minimal-skillgraph.json"); Expected = "PASS" },
    @{ Path = (Join-Path $root "tests\\fixtures\\invalid-bullet-zero.json"); Expected = "FAIL" },
    @{ Path = (Join-Path $root "tests\\fixtures\\invalid-missing-ai-range.json"); Expected = "FAIL" },
    @{ Path = (Join-Path $root "tests\\fixtures\\invalid-missing-template-path.json"); Expected = "FAIL" },
    @{ Path = (Join-Path $root "tests\\fixtures\\invalid-unwhitelisted-buff.json"); Expected = "FAIL" },
    @{ Path = (Join-Path $root "tests\\fixtures\\invalid-resource-edge-mismatch.json"); Expected = "FAIL" },
    @{ Path = (Join-Path $root "tests\\fixtures\\warn-fallback-only.json"); Expected = "WARN" }
)

foreach ($case in $cases) {
    $output = & powershell -ExecutionPolicy Bypass -File $AuditorPath -Path $case.Path 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Auditor command failed for $($case.Path): $output"
        exit 1
    }

    $result = $output | ConvertFrom-Json
    if ($result.status -ne $case.Expected) {
        Write-Error "Expected $($case.Expected) for $($case.Path), got $($result.status)"
        exit 1
    }
}

Write-Host "All auditor cases passed."

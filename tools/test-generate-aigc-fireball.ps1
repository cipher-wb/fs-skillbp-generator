param(
    [string]$GeneratorPath = (Join-Path $PSScriptRoot "generate-aigc-straight-projectile.ps1")
)

$ErrorActionPreference = "Stop"

function ConvertFrom-JsonCompat {
    param(
        [Parameter(Mandatory = $true)]
        [string]$Json,
        [int]$Depth = 64
    )

    $command = Get-Command ConvertFrom-Json
    if ($command.Parameters.ContainsKey("Depth")) {
        return $Json | ConvertFrom-Json -Depth $Depth
    }

    return $Json | ConvertFrom-Json
}

function Convert-CodePointsToString {
    param([int[]]$CodePoints)

    return -join ($CodePoints | ForEach-Object { [char]$_ })
}

$root = Split-Path -Parent $PSScriptRoot
$outputDir = Join-Path $root "tests\\output"
$outputPath = Join-Path $outputDir "generated-fireball.json"
$skillName = Convert-CodePointsToString @(0x706B, 0x7403, 0x672F)
$skillDesc = Convert-CodePointsToString @(0x524D, 0x6447, 0x31, 0x30, 0x5E27, 0xFF0C, 0x5411, 0x524D, 0x53D1, 0x5C04, 0x706B, 0x7403, 0xFF0C, 0x547D, 0x4E2D, 0x9644, 0x52A0, 0x707C, 0x70E7, 0x3002)

New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
if (Test-Path $outputPath) {
    Remove-Item -Force $outputPath
}

& pwsh -NoProfile -File $GeneratorPath -OutputPath $outputPath -SkillId 19009991

if (-not (Test-Path $outputPath)) {
    Write-Error "Generator did not create output file: $outputPath"
    exit 1
}

$graph = ConvertFrom-JsonCompat -Json (Get-Content -Raw $outputPath) -Depth 128
$skillRef = $graph.references.RefIds | Where-Object { $_.data.Config2ID -eq "SkillConfig_19009991" } | Select-Object -First 1
if (-not $skillRef) {
    Write-Error "Generated graph is missing SkillConfig_19009991."
    exit 1
}

$skillCfg = ConvertFrom-JsonCompat -Json ([string]$skillRef.data.ConfigJson) -Depth 64
if ($skillCfg.SkillNameEditor -ne $skillName) {
    Write-Error "Expected SkillNameEditor=$skillName, got $($skillCfg.SkillNameEditor)"
    exit 1
}

if ($skillCfg.SkillDescEditor -ne $skillDesc) {
    Write-Error "Expected SkillDescEditor=$skillDesc, got $($skillCfg.SkillDescEditor)"
    exit 1
}

$addBuffRef = $graph.references.RefIds | Where-Object { $_.type.class -eq "TSET_ADD_BUFF" -and $_.data.Config2ID -eq "SkillEffectConfig_19009993" } | Select-Object -First 1
if (-not $addBuffRef) {
    Write-Error "Generated graph is missing SkillEffectConfig_19009993."
    exit 1
}

$addBuffCfg = ConvertFrom-JsonCompat -Json ([string]$addBuffRef.data.ConfigJson) -Depth 64
if ([int]$addBuffCfg.Params[2].Value -ne 1080029) {
    Write-Error "Expected burn buff 1080029, got $($addBuffCfg.Params[2].Value)"
    exit 1
}

$auditOutput = & pwsh -NoProfile -File (Join-Path $PSScriptRoot "skill-json-auditor.ps1") -Path $outputPath
$audit = $auditOutput | ConvertFrom-Json
if ($audit.status -ne "PASS") {
    Write-Error "Expected auditor PASS, got $($audit.status)"
    exit 1
}

Write-Host "AIGC fireball generation test passed."

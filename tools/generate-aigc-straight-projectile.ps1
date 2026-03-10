param(
    [string]$OutputPath = "",
    [int]$SkillId = 19009991,
    [int]$CastFrame = 10,
    [int]$BaseDuration = 30,
    [int]$CdTime = 240,
    [int]$SkillRange = 1000,
    [int]$AISkillRange = 900,
    [string]$SkillName = "",
    [string]$SkillDesc = "",
    [string]$BuffConfig2Id = "BuffConfig_1080029"
)

$ErrorActionPreference = "Stop"
$scriptRoot = $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($scriptRoot)) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
}

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

function Read-Utf8Text {
    param([string]$FilePath)

    return [System.IO.File]::ReadAllText((Resolve-Path $FilePath), [System.Text.Encoding]::UTF8)
}

function Read-JsonFile {
    param(
        [string]$FilePath,
        [int]$Depth = 64
    )

    return ConvertFrom-JsonCompat -Json (Read-Utf8Text -FilePath $FilePath) -Depth $Depth
}

function Write-Utf8Json {
    param(
        [string]$FilePath,
        $Object
    )

    [System.IO.File]::WriteAllText($FilePath, ($Object | ConvertTo-Json -Depth 100), [System.Text.Encoding]::UTF8)
}

function Convert-CodePointsToString {
    param([int[]]$CodePoints)

    return -join ($CodePoints | ForEach-Object { [char]$_ })
}

function New-ParamObj {
    param(
        [int]$Value,
        [int]$ParamType,
        [int]$Factor = 0
    )

    return [pscustomobject]@{
        Value = $Value
        ParamType = $ParamType
        Factor = $Factor
    }
}

function Get-RefByConfig2Id {
    param(
        $Graph,
        [string]$Config2Id
    )

    return $Graph.references.RefIds | Where-Object { $_.data.Config2ID -eq $Config2Id } | Select-Object -First 1
}

function Update-ConfigJson {
    param(
        $Ref,
        $Config
    )

    $Ref.data.ConfigJson = $Config | ConvertTo-Json -Compress -Depth 64
}

function Get-TemplateEntry {
    param(
        $TemplateConfig,
        [string]$Key
    )

    $entry = $TemplateConfig.templates | Where-Object { $_.key -eq $Key } | Select-Object -First 1
    if (-not $entry) {
        throw "Template key not found in whitelist: $Key"
    }
    return $entry
}

function Get-PreferredResource {
    param(
        $ResourceConfig,
        [string]$Kind
    )

    $entry = $ResourceConfig.resources |
        Where-Object { $_.kind -eq $Kind -and -not $_.is_fallback } |
        Sort-Object priority |
        Select-Object -First 1

    if (-not $entry) {
        throw "Preferred resource not found for kind: $Kind"
    }

    return $entry
}

function Convert-WorkspacePathToAssetPath {
    param(
        [string]$WorkspaceRoot,
        [string]$FilePath
    )

    $absolute = [System.IO.Path]::GetFullPath($FilePath)
    $workspace = [System.IO.Path]::GetFullPath($WorkspaceRoot)
    if ($absolute.StartsWith($workspace, [System.StringComparison]::OrdinalIgnoreCase)) {
        $relative = $absolute.Substring($workspace.Length).TrimStart('\')
        if ($relative.StartsWith("NodeEditor\")) {
            return ("Assets/Thirds/" + ($relative -replace "\\", "/"))
        }
    }

    return ($absolute -replace "\\", "/")
}

function Get-NextRid {
    param($Graph)

    $maxRid = ($Graph.references.RefIds | Measure-Object -Property rid -Maximum).Maximum
    if ($null -eq $maxRid) {
        return 1000
    }

    return ([int]$maxRid + 1)
}

function Get-NextComputeOrder {
    param($Graph)

    $maxOrder = ($Graph.references.RefIds | Measure-Object -Property { [int]$_.data.computeOrder } -Maximum).Maximum
    if ($null -eq $maxOrder) {
        return 0
    }

    return ([int]$maxOrder + 1)
}

function Test-Config2IdUnused {
    param(
        [string]$SearchRoot,
        [string]$Config2Id,
        [string]$AllowedPath = ""
    )

    $hits = @()
    $rg = Get-Command rg -ErrorAction SilentlyContinue
    if ($rg) {
        $hits = @(& $rg.Source -l --glob "*.json" --fixed-strings $Config2Id $SearchRoot 2>$null)
    } else {
        $hits = Get-ChildItem $SearchRoot -Recurse -File -Filter *.json |
            Select-String -Pattern ([regex]::Escape($Config2Id)) |
            Select-Object -ExpandProperty Path -Unique
    }

    if ([string]::IsNullOrWhiteSpace($AllowedPath)) {
        return ($hits.Count -eq 0)
    }

    $allowed = [System.IO.Path]::GetFullPath($AllowedPath)
    $otherHits = @($hits | Where-Object { [System.IO.Path]::GetFullPath($_) -ne $allowed })
    return ($otherHits.Count -eq 0)
}

function Test-PathUnderRoot {
    param(
        [string]$RootPath,
        [string]$CandidatePath
    )

    $rootFull = [System.IO.Path]::GetFullPath($RootPath).TrimEnd('\')
    $candidateFull = [System.IO.Path]::GetFullPath($CandidatePath)
    return $candidateFull.StartsWith(($rootFull + "\"), [System.StringComparison]::OrdinalIgnoreCase) -or
        $candidateFull.Equals($rootFull, [System.StringComparison]::OrdinalIgnoreCase)
}

$workspaceRoot = Split-Path -Parent $scriptRoot
$skillJsonRoot = Join-Path $workspaceRoot "NodeEditor\\SkillEditor\\Saves\\Jsons"
$templateConfig = Read-JsonFile -FilePath (Join-Path $workspaceRoot "config\\template-whitelist.json") -Depth 32
$resourceConfig = Read-JsonFile -FilePath (Join-Path $workspaceRoot "config\\resource-whitelist.json") -Depth 32
$buffConfig = Read-JsonFile -FilePath (Join-Path $workspaceRoot "config\\buff-whitelist.json") -Depth 32
$configIdConfig = Read-JsonFile -FilePath (Join-Path $workspaceRoot "NodeEditor\\SkillEditor\\Saves\\ConfigID.json") -Depth 32

if ([string]::IsNullOrWhiteSpace($SkillName)) {
    $SkillName = Convert-CodePointsToString @(0x706B, 0x7403, 0x672F)
}

if ([string]::IsNullOrWhiteSpace($SkillDesc)) {
    $SkillDesc = Convert-CodePointsToString @(0x524D, 0x6447, 0x31, 0x30, 0x5E27, 0xFF0C, 0x5411, 0x524D, 0x53D1, 0x5C04, 0x706B, 0x7403, 0xFF0C, 0x547D, 0x4E2D, 0x9644, 0x52A0, 0x707C, 0x70E7, 0x3002)
}

if ($CastFrame -lt 0) {
    throw "CastFrame must be >= 0."
}

if ($BaseDuration -lt ($CastFrame + 20)) {
    throw "BaseDuration must be at least CastFrame + 20."
}

if ($CdTime -lt $BaseDuration) {
    throw "CdTime must be >= BaseDuration."
}

if ($AISkillRange -le 0 -or $SkillRange -le 0) {
    throw "SkillRange and AISkillRange must be > 0."
}

if ([string]::IsNullOrWhiteSpace($OutputPath)) {
    $OutputPath = Join-Path $workspaceRoot "NodeEditor\\SkillEditor\\Saves\\Jsons\\AIGC\\SkillGraph_$SkillId`_AIGC_Fireball.json"
} elseif (-not [System.IO.Path]::IsPathRooted($OutputPath)) {
    $OutputPath = Join-Path $workspaceRoot $OutputPath
}

$outputDir = Split-Path -Parent $OutputPath
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

$baseTemplate = Get-TemplateEntry -TemplateConfig $templateConfig -Key "system-straight-projectile"
$hitTemplate = Get-TemplateEntry -TemplateConfig $templateConfig -Key "template-projectile-hit"
$castFxResource = Get-PreferredResource -ResourceConfig $resourceConfig -Kind "cast_fx"
$projectileResource = Get-PreferredResource -ResourceConfig $resourceConfig -Kind "projectile_model"
$hitFxResource = Get-PreferredResource -ResourceConfig $resourceConfig -Kind "hit_fx"
$burnBuff = $buffConfig.buffs | Where-Object { $_.config2id -eq $BuffConfig2Id } | Select-Object -First 1
if (-not $burnBuff) {
    throw "Buff whitelist entry not found: $BuffConfig2Id"
}

$generatedSkillConfig2Id = "SkillConfig_$SkillId"
$generatedHitTemplateConfig2Id = "SkillEffectConfig_$($SkillId + 1)"
$generatedAddBuffConfig2Id = "SkillEffectConfig_$($SkillId + 2)"

foreach ($config2id in @($generatedSkillConfig2Id, $generatedHitTemplateConfig2Id, $generatedAddBuffConfig2Id)) {
    if ((Test-PathUnderRoot -RootPath $skillJsonRoot -CandidatePath $OutputPath) -and -not (Test-Config2IdUnused -SearchRoot $skillJsonRoot -Config2Id $config2id -AllowedPath $OutputPath)) {
        throw "Config2ID already exists in repository: $config2id"
    }
}

$baseGraphPath = Join-Path $workspaceRoot $baseTemplate.workspace_path
$hitTemplateGraphPath = Join-Path $workspaceRoot $hitTemplate.workspace_path
$buffSourceGraphPath = Join-Path $workspaceRoot $burnBuff.source_graph

$graph = Read-JsonFile -FilePath $baseGraphPath -Depth 128
$hitTemplateGraph = Read-JsonFile -FilePath $hitTemplateGraphPath -Depth 128
$buffSourceGraph = Read-JsonFile -FilePath $buffSourceGraphPath -Depth 128
$hitTemplateRoot = $hitTemplateGraph.references.RefIds | Where-Object { $_.data.IsTemplate -eq $true } | Select-Object -First 1
if (-not $hitTemplateRoot) {
    throw "template-projectile-hit root node not found."
}

$buffRef = $buffSourceGraph.references.RefIds | Where-Object { $_.data.Config2ID -eq $BuffConfig2Id } | Select-Object -First 1
if (-not $buffRef) {
    throw "Buff source node not found: $BuffConfig2Id"
}

$skillRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillConfig_1900215"
$castTemplateRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillEffectConfig_190012839"
$legacyProjectileRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillEffectConfig_190012840"
$projectileTemplateRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillEffectConfig_190012842"
$delayRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillEffectConfig_190012844"
$hitOrderRef = Get-RefByConfig2Id -Graph $graph -Config2Id "SkillEffectConfig_190012848"
$projectileNodeRef = Get-RefByConfig2Id -Graph $graph -Config2Id $projectileResource.config2id

if (-not $skillRef -or -not $castTemplateRef -or -not $legacyProjectileRef -or -not $projectileTemplateRef -or -not $delayRef -or -not $hitOrderRef -or -not $projectileNodeRef) {
    throw "Base graph is missing one or more expected nodes."
}

$skillConfig = ConvertFrom-JsonCompat -Json ([string]$skillRef.data.ConfigJson) -Depth 64
$castTemplateConfig = ConvertFrom-JsonCompat -Json ([string]$castTemplateRef.data.ConfigJson) -Depth 64
$legacyProjectileConfig = ConvertFrom-JsonCompat -Json ([string]$legacyProjectileRef.data.ConfigJson) -Depth 64
$projectileTemplateConfig = ConvertFrom-JsonCompat -Json ([string]$projectileTemplateRef.data.ConfigJson) -Depth 64
$delayConfig = ConvertFrom-JsonCompat -Json ([string]$delayRef.data.ConfigJson) -Depth 64
$hitOrderConfig = ConvertFrom-JsonCompat -Json ([string]$hitOrderRef.data.ConfigJson) -Depth 64

$skillRef.data.ID = $SkillId
$skillRef.data.Config2ID = $generatedSkillConfig2Id
$skillConfig.ID = $SkillId
$skillConfig.SkillCastFrame = $CastFrame
$skillConfig.SkillBufferStartFrame = $CastFrame
$skillConfig.SkillBufferFrame = $CastFrame
$skillConfig.SkillBaseDuration = $BaseDuration
$skillConfig.CdTime = $CdTime
$skillConfig.SkillRange = $SkillRange
$skillConfig.AISkillRange = $AISkillRange
$skillConfig.SkillIndicatorType = 3
$skillConfig.SkillIndicatorParam = @(300)
$skillConfig.SkillNameEditor = $SkillName
$skillConfig.SkillDescEditor = $SkillDesc
Update-ConfigJson -Ref $skillRef -Config $skillConfig

$castTemplateConfig.Params[3].Value = [int]$castFxResource.id
Update-ConfigJson -Ref $castTemplateRef -Config $castTemplateConfig

$legacyProjectileConfig.Params[3].Value = [int]$projectileResource.id
Update-ConfigJson -Ref $legacyProjectileRef -Config $legacyProjectileConfig

$projectileResourceEdges = @($graph.edges | Where-Object {
    [string]$_.outputNodeGUID -eq [string]$legacyProjectileRef.data.GUID -and
    [string]$_.outputFieldName -eq "PackedParamsOutput" -and
    [string]$_.outputPortIdentifier -eq "3"
})

if ($projectileResourceEdges.Count -ne 1) {
    throw "Expected exactly one projectile resource edge on port 3, got $($projectileResourceEdges.Count)."
}

$projectileResourceEdges[0].inputNodeGUID = [string]$projectileNodeRef.data.GUID

$projectileTemplateConfig.Params[8].Value = [int]$hitFxResource.id
Update-ConfigJson -Ref $projectileTemplateRef -Config $projectileTemplateConfig

$delayConfig.Params[0].Value = $CastFrame
Update-ConfigJson -Ref $delayRef -Config $delayConfig

$generatedHitTemplateId = $SkillId + 1
$generatedAddBuffId = $SkillId + 2
$hitOrderConfig.Params = @(
    (New-ParamObj -Value $generatedHitTemplateId -ParamType 0),
    $hitOrderConfig.Params[0]
)
Update-ConfigJson -Ref $hitOrderRef -Config $hitOrderConfig

$hitOrderGuid = [string]$hitOrderRef.data.GUID
foreach ($edge in @($graph.edges)) {
    if ([string]$edge.outputNodeGUID -eq $hitOrderGuid -and [string]$edge.outputPortIdentifier -eq "0") {
        $edge.outputPortIdentifier = "1"
    }
}

$nextRid = Get-NextRid -Graph $graph
$nextOrder = Get-NextComputeOrder -Graph $graph
$newHitTemplateRid = $nextRid
$newAddBuffRid = $nextRid + 1
$newBuffRid = $nextRid + 2
$newHitTemplateGuid = [guid]::NewGuid().ToString()
$newAddBuffGuid = [guid]::NewGuid().ToString()
$newBuffGuid = [guid]::NewGuid().ToString()

$newHitTemplateConfig = [pscustomobject]@{
    ID = $generatedHitTemplateId
    SkillEffectType = 118
    Params = @(
        (New-ParamObj 1 5),
        (New-ParamObj 2 5),
        (New-ParamObj 175000004 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj $generatedAddBuffId 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj -1 0),
        (New-ParamObj -1 0)
    )
}

$newAddBuffConfig = [pscustomobject]@{
    ID = $generatedAddBuffId
    SkillEffectType = 4
    Params = @(
        (New-ParamObj 2 5),
        (New-ParamObj 108 1),
        (New-ParamObj -Value ([int]$burnBuff.id) -ParamType 0),
        (New-ParamObj 0 0),
        (New-ParamObj 0 0),
        (New-ParamObj 1 0),
        (New-ParamObj 1 0)
    )
}

$buffTableHash = [string]$configIdConfig.tableName2TableTash.BuffConfigManager
$newRefs = @(
    [pscustomobject]@{
        rid = $newHitTemplateRid
        type = [pscustomobject]@{ class = "TSET_RUN_SKILL_EFFECT_TEMPLATE"; ns = "NodeEditor"; asm = "NodeEditor" }
        data = [pscustomobject]([ordered]@{
            GUID = $newHitTemplateGuid
            computeOrder = $nextOrder
            position = [pscustomobject]@{ serializedVersion = "2"; x = -7060.0; y = -3225.0; width = 410.0; height = 252.0 }
            expanded = $false
            debug = $false
            nodeLock = $false
            visible = $true
            hideChildNodes = $false
            hidePos = [pscustomobject]@{ x = 0.0; y = 0.0 }
            hideCounter = 0
            ID = $generatedHitTemplateId
            Desc = "SkillGraph_175_0001 template projectile hit"
            IsTemplate = $false
            TemplateFlags = 0
            TemplateParams = @()
            TemplateParamsDesc = @()
            TemplateParamsCustomAdd = $false
            TableTash = "0CFA05568A66FEA1DF3BA6FE40DB7080"
            ConfigJson = ($newHitTemplateConfig | ConvertTo-Json -Compress -Depth 64)
            Config2ID = $generatedHitTemplateConfig2Id
            SkillEffectType = 118
            TemplateData = [pscustomobject]([ordered]@{
                TemplateParams = $hitTemplateRoot.data.TemplateParams
                TemplatePath = [string]$hitTemplate.asset_path
            })
        })
    },
    [pscustomobject]@{
        rid = $newAddBuffRid
        type = [pscustomobject]@{ class = "TSET_ADD_BUFF"; ns = "NodeEditor"; asm = "NodeEditor" }
        data = [pscustomobject]([ordered]@{
            GUID = $newAddBuffGuid
            computeOrder = ($nextOrder + 1)
            position = [pscustomobject]@{ serializedVersion = "2"; x = -6605.0; y = -3225.0; width = 330.0; height = 191.0 }
            expanded = $false
            debug = $false
            nodeLock = $false
            visible = $true
            hideChildNodes = $false
            hidePos = [pscustomobject]@{ x = -6605.0; y = -3225.0 }
            hideCounter = 0
            ID = $generatedAddBuffId
            Desc = "AIGC burn on hit"
            IsTemplate = $false
            TemplateFlags = 0
            TemplateParams = @()
            TemplateParamsDesc = @()
            TemplateParamsCustomAdd = $false
            TableTash = "0CFA05568A66FEA1DF3BA6FE40DB7080"
            ConfigJson = ($newAddBuffConfig | ConvertTo-Json -Compress -Depth 64)
            Config2ID = $generatedAddBuffConfig2Id
            SkillEffectType = 4
        })
    },
    [pscustomobject]@{
        rid = $newBuffRid
        type = [pscustomobject]@{ class = "BuffConfigNode"; ns = "NodeEditor"; asm = "NodeEditor" }
        data = [pscustomobject]([ordered]@{
            GUID = $newBuffGuid
            computeOrder = ($nextOrder + 2)
            position = [pscustomobject]@{ serializedVersion = "2"; x = -6235.0; y = -3225.0; width = 281.0; height = 228.0 }
            expanded = $false
            debug = $false
            nodeLock = $false
            visible = $true
            hideChildNodes = $false
            hidePos = [pscustomobject]@{ x = 0.0; y = 0.0 }
            hideCounter = 0
            ID = [int]$buffRef.data.ID
            Desc = [string]$buffRef.data.Desc
            IsTemplate = $false
            TemplateFlags = 0
            TemplateParams = @()
            TemplateParamsDesc = @()
            TemplateParamsCustomAdd = $false
            TableTash = $buffTableHash
            ConfigJson = [string]$buffRef.data.ConfigJson
            Config2ID = [string]$buffRef.data.Config2ID
        })
    }
)

$graph.nodes += @(
    [pscustomobject]@{ rid = $newHitTemplateRid },
    [pscustomobject]@{ rid = $newAddBuffRid },
    [pscustomobject]@{ rid = $newBuffRid }
)
$graph.references.RefIds += $newRefs
$graph.edges += @(
    [pscustomobject]@{
        GUID = [guid]::NewGuid().ToString()
        inputNodeGUID = $newHitTemplateGuid
        outputNodeGUID = $hitOrderGuid
        inputFieldName = "ID"
        outputFieldName = "PackedParamsOutput"
        inputPortIdentifier = "0"
        outputPortIdentifier = "0"
        isVisible = $true
    },
    [pscustomobject]@{
        GUID = [guid]::NewGuid().ToString()
        inputNodeGUID = $newAddBuffGuid
        outputNodeGUID = $newHitTemplateGuid
        inputFieldName = "ID"
        outputFieldName = "PackedParamsOutput"
        inputPortIdentifier = "0"
        outputPortIdentifier = "6"
        isVisible = $true
    },
    [pscustomobject]@{
        GUID = [guid]::NewGuid().ToString()
        inputNodeGUID = $newBuffGuid
        outputNodeGUID = $newAddBuffGuid
        inputFieldName = "ID"
        outputFieldName = "PackedParamsOutput"
        inputPortIdentifier = "0"
        outputPortIdentifier = "2"
        isVisible = $true
    }
)

$graph.path = Convert-WorkspacePathToAssetPath -WorkspaceRoot $workspaceRoot -FilePath $OutputPath
Write-Utf8Json -FilePath $OutputPath -Object $graph
Write-Output $OutputPath

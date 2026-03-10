param(
    [Parameter(Mandatory = $true)]
    [Alias("GraphPath")]
    [string]$Path,
    [string]$ConfigIdPath = "",
    [string]$BuffWhitelistPath = "",
    [string]$ResourceWhitelistPath = "",
    [string]$TemplateWhitelistPath = ""
)

$ErrorActionPreference = "Stop"
$scriptRoot = $PSScriptRoot
if ([string]::IsNullOrWhiteSpace($scriptRoot)) {
    $scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
}

if ([string]::IsNullOrWhiteSpace($ConfigIdPath)) {
    $ConfigIdPath = Join-Path (Split-Path -Parent $scriptRoot) "NodeEditor\\SkillEditor\\Saves\\ConfigID.json"
}

if ([string]::IsNullOrWhiteSpace($BuffWhitelistPath)) {
    $BuffWhitelistPath = Join-Path (Split-Path -Parent $scriptRoot) "config\\buff-whitelist.json"
}

if ([string]::IsNullOrWhiteSpace($ResourceWhitelistPath)) {
    $ResourceWhitelistPath = Join-Path (Split-Path -Parent $scriptRoot) "config\\resource-whitelist.json"
}

if ([string]::IsNullOrWhiteSpace($TemplateWhitelistPath)) {
    $TemplateWhitelistPath = Join-Path (Split-Path -Parent $scriptRoot) "config\\template-whitelist.json"
}

function Add-Issue {
    param(
        [ref]$Bucket,
        [string]$Code,
        [string]$Message
    )

    $Bucket.Value += [pscustomobject]@{
        code = $Code
        message = $Message
    }
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

function Convert-AssetPathToWorkspacePath {
    param(
        [string]$AssetPath,
        [string]$WorkspaceRoot
    )

    if ([string]::IsNullOrWhiteSpace($AssetPath)) {
        return $null
    }

    $normalized = $AssetPath -replace "\\", "/"
    if ($normalized.StartsWith("Assets/Thirds/NodeEditor/")) {
        $relative = $normalized.Substring("Assets/Thirds/NodeEditor/".Length) -replace "/", "\\"
        return Join-Path $WorkspaceRoot ("NodeEditor\" + $relative)
    }

    return Join-Path $WorkspaceRoot ($normalized -replace "/", "\\")
}

function Convert-ConfigJson {
    param([string]$ConfigJson)

    if ([string]::IsNullOrWhiteSpace($ConfigJson)) {
        return $null
    }

    return ConvertFrom-JsonCompat -Json $ConfigJson -Depth 64
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

function Get-HashMap {
    param([string]$FilePath)

    if (-not (Test-Path $FilePath)) {
        return @{}
    }

    $json = Read-JsonFile -FilePath $FilePath -Depth 32
    $result = @{}
    foreach ($prop in $json.tableName2TableTash.PSObject.Properties) {
        $result[$prop.Name] = [string]$prop.Value
    }
    return $result
}

function Get-ResourceMap {
    param([string]$FilePath)

    if (-not (Test-Path $FilePath)) {
        return @{}
    }

    $json = Read-JsonFile -FilePath $FilePath -Depth 32
    $result = @{}
    foreach ($item in $json.resources) {
        $result[[string]$item.config2id] = $item
    }
    return $result
}

function Get-BuffMap {
    param([string]$FilePath)

    if (-not (Test-Path $FilePath)) {
        return @{}
    }

    $json = Read-JsonFile -FilePath $FilePath -Depth 32
    $result = @{}
    foreach ($item in $json.buffs) {
        $result[[string]$item.config2id] = $item
    }
    return $result
}

function Get-TemplateSet {
    param([string]$FilePath)

    if (-not (Test-Path $FilePath)) {
        return @{}
    }

    $json = Read-JsonFile -FilePath $FilePath -Depth 32
    $result = @{}
    foreach ($item in $json.templates) {
        $result[[string]$item.asset_path] = $true
    }
    return $result
}

if (-not (Test-Path $Path)) {
    throw "Graph file not found: $Path"
}

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$hashMap = Get-HashMap -FilePath $ConfigIdPath
$buffMap = Get-BuffMap -FilePath $BuffWhitelistPath
$resourceMap = Get-ResourceMap -FilePath $ResourceWhitelistPath
$templateSet = Get-TemplateSet -FilePath $TemplateWhitelistPath

$graph = Read-JsonFile -FilePath $Path -Depth 128
$errors = @()
$warnings = @()

if ($null -eq $graph.nodes -or $null -eq $graph.edges -or $null -eq $graph.references -or $null -eq $graph.references.RefIds) {
    Add-Issue ([ref]$errors) "GRAPH_TOP_LEVEL" "Missing top-level nodes / edges / references.RefIds structure."
}

$refIds = @()
if ($graph.references -and $graph.references.RefIds) {
    $refIds = @($graph.references.RefIds)
}

$ridMap = @{}
$guidMap = @{}
$config2Ids = @{}
$configMap = @{}

foreach ($ref in $refIds) {
    if ($null -ne $ref.rid) {
        $ridMap[[string]$ref.rid] = $ref
    }

    if ($null -ne $ref.data -and -not [string]::IsNullOrWhiteSpace([string]$ref.data.GUID)) {
        $guidMap[[string]$ref.data.GUID] = $ref
    }
}

foreach ($nodeRef in @($graph.nodes)) {
    if ($null -eq $nodeRef.rid) {
        Add-Issue ([ref]$errors) "NODE_RID_MISSING" "A node entry is missing rid."
        continue
    }

    if (-not $ridMap.ContainsKey([string]$nodeRef.rid)) {
        Add-Issue ([ref]$errors) "NODE_RID_BROKEN" "Node rid=$($nodeRef.rid) is not present in references.RefIds."
    }
}

foreach ($edge in @($graph.edges)) {
    if (-not $guidMap.ContainsKey([string]$edge.inputNodeGUID)) {
        Add-Issue ([ref]$errors) "EDGE_INPUT_GUID" "Edge input GUID not found: $($edge.inputNodeGUID)"
    }

    if (-not $guidMap.ContainsKey([string]$edge.outputNodeGUID)) {
        Add-Issue ([ref]$errors) "EDGE_OUTPUT_GUID" "Edge output GUID not found: $($edge.outputNodeGUID)"
    }

    if ([string]::IsNullOrWhiteSpace([string]$edge.inputFieldName) -or [string]::IsNullOrWhiteSpace([string]$edge.outputFieldName)) {
        Add-Issue ([ref]$errors) "EDGE_FIELD_NAME" "Edge is missing inputFieldName or outputFieldName."
    }
}

foreach ($ref in $refIds) {
    $data = $ref.data
    if ($null -eq $data) {
        continue
    }

    $config2Id = [string]$data.Config2ID
    if (-not [string]::IsNullOrWhiteSpace($config2Id)) {
        if ($config2Ids.ContainsKey($config2Id)) {
            Add-Issue ([ref]$errors) "DUPLICATE_CONFIG2ID" "Duplicate Config2ID: $config2Id"
        } else {
            $config2Ids[$config2Id] = $true
        }

        $configType = $config2Id.Split("_")[0]
        $managerName = "$($configType)Manager"
        $expectedHash = $hashMap[$managerName]
        if (-not [string]::IsNullOrWhiteSpace($expectedHash)) {
            if ([string]::IsNullOrWhiteSpace([string]$data.TableTash)) {
                Add-Issue ([ref]$errors) "TABLE_HASH_MISSING" "$config2Id is missing TableTash."
            } elseif ([string]$data.TableTash -ne $expectedHash) {
                Add-Issue ([ref]$errors) "TABLE_HASH_MISMATCH" "$config2Id TableTash does not match the current project."
            }
        }

        $config = $null
        try {
            $config = Convert-ConfigJson -ConfigJson ([string]$data.ConfigJson)
        } catch {
            Add-Issue ([ref]$errors) "CONFIG_JSON_PARSE" "$config2Id ConfigJson could not be parsed."
            continue
        }

        if (-not [string]::IsNullOrWhiteSpace([string]$data.GUID)) {
            $configMap[[string]$data.GUID] = $config
        }

        switch ($configType) {
            "SkillConfig" {
                if ([int]$config.SkillMainType -eq 0 -or [int]$config.SkillSubType -eq 0) {
                    Add-Issue ([ref]$errors) "SKILL_TYPE" "$config2Id is missing SkillMainType or SkillSubType."
                }

                $castFrame = [int]$config.SkillCastFrame
                $bufferFrame = [int]$config.SkillBufferFrame
                $duration = [int]$config.SkillBaseDuration
                $cdTime = [int]$config.CdTime
                if ($castFrame -gt $bufferFrame -or $bufferFrame -gt $duration -or ($duration -gt $cdTime -and $cdTime -gt 0)) {
                    Add-Issue ([ref]$errors) "SKILL_TIMING" "$config2Id violates cast<=buffer<=duration<=cd."
                }

                if ([int]$config.AISkillRange -le 0) {
                    Add-Issue ([ref]$errors) "SKILL_AI_RANGE" "$config2Id is missing AISkillRange."
                }
            }
            "SkillEffectConfig" {
                $effectType = [int]$config.SkillEffectType
                if ($effectType -eq 8) {
                    if ($null -eq $config.Params -or @($config.Params).Count -eq 0 -or [int]$config.Params[0].Value -eq 0) {
                        Add-Issue ([ref]$errors) "BULLET_ID_ZERO" "$config2Id bullet id must not be 0."
                    }
                }

                if ($null -ne $data.TemplateData) {
                    $templatePath = [string]$data.TemplateData.TemplatePath
                    if ([string]::IsNullOrWhiteSpace($templatePath)) {
                        Add-Issue ([ref]$errors) "TEMPLATE_PATH_EMPTY" "$config2Id is missing TemplatePath."
                    } else {
                        $workspaceTemplatePath = Convert-AssetPathToWorkspacePath -AssetPath $templatePath -WorkspaceRoot $workspaceRoot
                        if (-not (Test-Path $workspaceTemplatePath)) {
                            Add-Issue ([ref]$errors) "TEMPLATE_PATH_INVALID" "$config2Id template path does not exist: $templatePath"
                        } elseif (-not $templateSet.ContainsKey($templatePath)) {
                            Add-Issue ([ref]$warnings) "TEMPLATE_NOT_WHITELISTED" "$config2Id uses a template outside the whitelist: $templatePath"
                        }
                    }

                    if (@($config.Params).Count -gt 2 -and [int]$config.Params[2].Value -eq 0) {
                        Add-Issue ([ref]$errors) "TEMPLATE_ID_ZERO" "$config2Id template ref id is 0."
                    }
                }
            }
            "ModelConfig" {
                if ($resourceMap.ContainsKey($config2Id)) {
                    $resource = $resourceMap[$config2Id]
                    if ($resource.is_fallback) {
                        Add-Issue ([ref]$warnings) "FALLBACK_RESOURCE" "$config2Id uses a fallback resource."
                    }
                } else {
                    Add-Issue ([ref]$warnings) "RESOURCE_NOT_WHITELISTED" "$config2Id is not in the resource whitelist."
                }
            }
            "BuffConfig" {
                if (-not $buffMap.ContainsKey($config2Id)) {
                    Add-Issue ([ref]$errors) "BUFF_NOT_WHITELISTED" "$config2Id is not in the buff whitelist."
                }
            }
        }
    }
}

foreach ($edge in @($graph.edges)) {
    $outputGuid = [string]$edge.outputNodeGUID
    $inputGuid = [string]$edge.inputNodeGUID
    if (-not $guidMap.ContainsKey($outputGuid) -or -not $guidMap.ContainsKey($inputGuid)) {
        continue
    }

    if ([string]$edge.outputFieldName -ne "PackedParamsOutput") {
        continue
    }

    $portText = [string]$edge.outputPortIdentifier
    $portIndex = 0
    if (-not [int]::TryParse($portText, [ref]$portIndex)) {
        continue
    }

    $sourceRef = $guidMap[$outputGuid]
    $targetRef = $guidMap[$inputGuid]
    $sourceConfig2Id = [string]$sourceRef.data.Config2ID
    $targetConfig2Id = [string]$targetRef.data.Config2ID
    $targetType = ""
    if (-not [string]::IsNullOrWhiteSpace($targetConfig2Id)) {
        $targetType = $targetConfig2Id.Split("_")[0]
    }

    if ($targetType -ne "ModelConfig") {
        continue
    }

    if (-not $configMap.ContainsKey($outputGuid)) {
        continue
    }

    $sourceConfig = $configMap[$outputGuid]
    $params = @($sourceConfig.Params)
    if ($params.Count -le $portIndex) {
        continue
    }

    $expectedId = [int]$params[$portIndex].Value
    $actualId = [int]$targetRef.data.ID
    if ($expectedId -ne $actualId) {
        Add-Issue ([ref]$errors) "RESOURCE_EDGE_MISMATCH" "$sourceConfig2Id param[$portIndex] points to $expectedId but edge targets $targetConfig2Id."
    }
}

$status = "PASS"
if ($errors.Count -gt 0) {
    $status = "FAIL"
} elseif ($warnings.Count -gt 0) {
    $status = "WARN"
}

$result = [ordered]@{
    path = (Resolve-Path $Path).Path
    status = $status
    error_count = $errors.Count
    warning_count = $warnings.Count
    errors = $errors
    warnings = $warnings
}

$result | ConvertTo-Json -Depth 16

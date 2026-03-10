---
name: skill-graph-generator
description: Use when a whitelist-backed template plan in this repository must be turned into a concrete Unity SkillEditor graph without breaking node references, edges, or audit rules.
---

# Skill Graph Generator

## Overview
Generate by patching a proven graph, not by freehand authoring the whole structure.

## Method
1. Load the selected base graph from the template whitelist.
2. Keep `nodes`, `edges`, `references.RefIds`, `rid`, and `GUID` values intact unless copying an existing pattern already validated in this repo.
3. Limit edits to:
   - `data.ConfigJson`
   - `data.TemplateData.TemplatePath`
   - existing `Params[*].Value`
   - approved display fields such as skill name or description
4. Preserve `TableTash` values from `NodeEditor/SkillEditor/Saves/ConfigID.json`.
5. Save the new graph and run the local auditor.

## Minimum Checks While Editing
- `SkillConfig`: non-zero `SkillMainType` and `SkillSubType`, valid timing order, non-zero `AISkillRange`
- `SkillEffectConfig` with `SkillEffectType = 8`: bullet id in `Params[0].Value` must not be `0`
- Template references: `TemplatePath` must exist and remain whitelist-backed
- `ModelConfig`: only use approved resources or approved fallbacks
- `BuffConfig`: only use approved buff whitelist entries

## Stop Conditions
- The chosen template does not contain a safe slot for the requested behavior
- The requested effect depends on a buff, FX, or child template outside the whitelist
- Satisfying the request would require new manual wiring rather than patching proven topology

## Verification
Run:

```powershell
pwsh -NoProfile -File .\tools\skill-json-auditor.ps1 -Path <graph.json>
```

Only deliver the graph as complete when the auditor returns `PASS` or an explicitly accepted whitelist `WARN`.

---
name: skill-workflow-orchestrator
description: Use when turning a combat planner's natural-language skill request into a runnable Unity SkillEditor graph in this repository and the result must keep valid wiring, template usage, and auditable fallback resources.
---

# Skill Workflow Orchestrator

## Overview
Do not generate a full SkillGraph from scratch. Start from a whitelist-backed graph, patch a narrow set of fields, and run the local auditor before delivery.

## Workflow
1. Read [../skill-intent-parser/SKILL.md](../skill-intent-parser/SKILL.md) and convert the request into a compact intent object.
2. Read [../skill-template-selector/SKILL.md](../skill-template-selector/SKILL.md) and choose `template_key`, `sub_templates`, resource bindings, buff bindings, and fallback policy from [../../config/template-whitelist.json](../../config/template-whitelist.json), [../../config/resource-whitelist.json](../../config/resource-whitelist.json), and [../../config/buff-whitelist.json](../../config/buff-whitelist.json).
3. Read [../skill-graph-generator/SKILL.md](../skill-graph-generator/SKILL.md) and patch the chosen base graph. Prefer parameter edits over topology edits.
4. Save the resulting graph JSON.
5. Run one of these commands before claiming success:

```powershell
pwsh -NoProfile -File .\tools\skill-json-auditor.ps1 -Path <graph.json>
powershell -NoProfile -ExecutionPolicy Bypass -File .\tools\skill-json-auditor.ps1 -Path <graph.json>
```

6. Treat `FAIL` as blocked. Treat `WARN` as shippable only if the warning is an approved fallback from the whitelist.

## Hard Rules
- Never invent `rid`, `GUID`, or edge wiring unless cloning a proven pattern already present in this repository.
- Never emit an unverified `Config2ID` for templates, FX, bullets, or buffs. Use whitelist-backed entries or return `template_gap` or `resource_gap`.
- Preserve the main skill chain and template reference ports.
- If the request exceeds the current whitelist and template coverage, stop and report the gap instead of improvising a graph.

## Working Output
Work from a compact plan shaped like:

```json
{
  "skill_mode": "straight_projectile",
  "template_key": "system-straight-projectile",
  "sub_templates": [
    "template-cast-fx",
    "template-projectile-shape-circle",
    "template-projectile-hit"
  ],
  "replacements": {},
  "protected_nodes": [
    "main_skill_config",
    "main_skill_effect_chain",
    "template_reference_ports"
  ]
}
```

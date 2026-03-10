---
name: skill-template-selector
description: Use when a structured combat skill intent in this repository needs a safe base graph, sub-template set, and approved fallback resources before any SkillGraph editing.
---

# Skill Template Selector

## Overview
Always select from the local whitelist first. Template choice is the main control surface that keeps graphs connected and runnable.

## Required Inputs
- [../../config/template-whitelist.json](../../config/template-whitelist.json)
- [../../config/resource-whitelist.json](../../config/resource-whitelist.json)
- [../../config/buff-whitelist.json](../../config/buff-whitelist.json)

## Selection Rules
- `straight_projectile` -> base `system-straight-projectile`
- `ground_aoe` -> base `system-ground-aoe`
- If the request needs a cast FX or projectile child template, prefer:
  - `template-cast-fx`
  - `template-projectile-shape-circle`
  - `template-projectile-hit`
- Do not select any template outside the whitelist.

## Resource Rules
- Prefer formal resources first:
  - cast FX -> `ModelConfig_19000720`
  - projectile -> `ModelConfig_19000726`
  - hit FX -> `ModelConfig_19000725`
  - ground AOE -> `ModelConfig_19000701`
- Approved fallback chain:
  - cast FX: `ModelConfig_19000720 -> ModelConfig_19000706`
  - projectile: `ModelConfig_19000726 -> ModelConfig_19000741`
  - hit FX: `ModelConfig_19000725 -> ModelConfig_19000706`
  - ground AOE: `ModelConfig_19000701 -> ModelConfig_19000728`
- If a requested buff, FX, or projectile resource is not in the whitelist, return `resource_gap` instead of guessing.
- For generic burn-on-hit, prefer `BuffConfig_1080029`.

## Output Contract
Return a selection object:

```json
{
  "template_key": "system-straight-projectile",
  "sub_templates": [
    "template-cast-fx",
    "template-projectile-shape-circle",
    "template-projectile-hit"
  ],
  "resource_bindings": {
    "cast_fx_model": "ModelConfig_19000720",
    "projectile_model": "ModelConfig_19000726",
    "hit_fx_model": "ModelConfig_19000725"
  },
  "buff_bindings": {
    "burn_buff": "BuffConfig_1080029"
  },
  "protected_nodes": [
    "main_skill_config",
    "main_skill_effect_chain",
    "template_reference_ports"
  ]
}
```

## Guardrails
- Protect the main chain; only substitute inside the proven template slots.
- Never translate a vague request into a brand new base graph.
- If the user asks for a mode not represented in the whitelist, stop with `template_gap`.

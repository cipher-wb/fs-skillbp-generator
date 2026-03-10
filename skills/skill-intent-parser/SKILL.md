---
name: skill-intent-parser
description: Use when a natural-language combat skill request in this repository must be converted into a structured intent object before template selection or graph editing.
---

# Skill Intent Parser

## Overview
Produce a small intent object first. Do not jump directly to SkillGraph JSON.

## Parse Targets
- `skill_mode`: choose the nearest whitelist-backed mode such as `straight_projectile` or `ground_aoe`
- `theme`: fire, thunder, neutral, or other visual tone
- `timing`: cast frame, duration, cooldown
- `delivery`: projectile, ground blast, cast-only, delayed explosion
- `hit_behavior`: damage, buff, slow, destroy-on-hit, repeated tick
- `resource_policy`: `formal_first` or `fallback_allowed`

## Safe Defaults
- If the user gives cast frames only, set `SkillBufferStartFrame` and `SkillBufferFrame` to the same value.
- Default `SkillBaseDuration` to at least `cast_frame + 20`, and never below `30`.
- Default `CdTime` to at least `240`.
- Default `SkillRange` to `1000` and `AISkillRange` to `900` unless the request clearly implies a different range.
- For straight projectiles, default `bullet_speed` to `1500` and `bullet_lifetime` to `30`.
- Preserve requested on-hit behavior, but only as intent. Actual graph support is verified later by template selection and generation.

## Output Contract
Return a JSON-like object with explicit defaults filled in:

```json
{
  "skill_mode": "straight_projectile",
  "theme": "fire",
  "resource_policy": "fallback_allowed",
  "skill_config_patch": {
    "SkillCastFrame": 10,
    "SkillBufferStartFrame": 10,
    "SkillBufferFrame": 10,
    "SkillBaseDuration": 30,
    "CdTime": 240,
    "SkillRange": 1000,
    "AISkillRange": 900
  },
  "hit_behavior": [
    "play_hit_fx",
    "add_buff"
  ]
}
```

## Guardrails
- Do not output full graph JSON here.
- Do not invent specific `Config2ID` values unless they are already known from the local whitelist.
- If the request implies multi-phase combo logic or bespoke node topology outside existing templates, mark `template_gap` now.

# AIGC Skill Workflow Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** 为战斗策划落地一套“自然语言描述技能 -> 模板化生成 SkillGraph JSON -> 审核放行”的最小工业闭环。

**Architecture:** 使用一个入口 workflow skill 编排 3 个子 skill，并在工作区内提供白名单配置和本地审核器脚本。所有生成均以现有合法模板为骨架，禁止从零自由拼图。

**Tech Stack:** Markdown skills, PowerShell audit script, JSON/YAML whitelist configs, existing SkillEditor sample graphs

---

## Chunk 1: Curate Stable Inputs

### Task 1: Build template whitelist

**Files:**
- Create: `F:\AI\封神技能编辑器\config\template-whitelist.json`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\Jsons\系统功能`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\Jsons\技能模板`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\Jsons\通用技能`

- [ ] **Step 1: Write the failing validation case**

Create a small checklist doc that proves template selection is ambiguous today:

```md
需求: 直线飞行火球
当前失败模式: 可能误选宗门复杂技能而不是技能范例/模板
期望: 固定优先命中技能范例-直线飞行技能
```

- [ ] **Step 2: Verify the failure exists**

Run: manual inspection against current sample library  
Expected: no explicit machine-readable whitelist exists

- [ ] **Step 3: Create minimal whitelist**

Include per entry:

```json
{
  "path": "Assets/Thirds/NodeEditor/SkillEditor/Saves/Jsons/系统功能/SkillGraph_技能范例-直线飞行技能.json",
  "category": "projectile",
  "recommended": true,
  "allowed_replacements": ["cast_fx", "projectile_model", "hit_fx", "buff_id", "timing"],
  "protected_nodes": ["main_skill_chain", "template_reference_ports"]
}
```

- [ ] **Step 4: Review the whitelist against 10 representative skills**

Run: manual spot-check using `宗门技能` / `通用技能` / `系统功能` samples  
Expected: each common skill archetype maps to at least one stable template

- [ ] **Step 5: Commit**

```bash
git add config/template-whitelist.json
git commit -m "feat: add curated skill template whitelist"
```

### Task 2: Build resource whitelist

**Files:**
- Create: `F:\AI\封神技能编辑器\config\resource-whitelist.json`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\Jsons\系统功能\SkillGraph_190012590-参数调试白模效果.json`

- [ ] **Step 1: Write the failing validation case**

Document the current failure:

```md
需求: 找不到正式特效时自动使用安全占位
当前失败模式: AI 幻觉特效/模型 ID
期望: 命中正式资源，否则按优先级回退到白模占位
```

- [ ] **Step 2: Verify the failure exists**

Run: manual audit of previous AI outputs / known bad cases  
Expected: no centralized fallback policy exists

- [ ] **Step 3: Create minimal whitelist**

Include per entry:

```json
{
  "kind": "cast_fx",
  "config2id": "ModelConfig_19000720",
  "purpose": "safe_cast_placeholder",
  "is_fallback": true,
  "priority": 100
}
```

- [ ] **Step 4: Define fallback rules**

Add top-level fallback chains for:
- cast FX
- projectile model
- hit FX
- ground AOE
- buff placeholder

- [ ] **Step 5: Commit**

```bash
git add config/resource-whitelist.json
git commit -m "feat: add safe skill resource whitelist"
```

## Chunk 2: Create Workflow Skills

### Task 3: Create orchestrator skill

**Files:**
- Create: `F:\AI\封神技能编辑器\skills\skill-workflow-orchestrator\SKILL.md`
- Reference: `F:\AI\封神技能编辑器\config\template-whitelist.json`
- Reference: `F:\AI\封神技能编辑器\config\resource-whitelist.json`

- [ ] **Step 1: Write the failing baseline scenario**

Baseline prompt:

```md
我要一个直线飞行火球，命中加燃烧 buff
```

Expected baseline failure:
- no mandatory structured intake
- no forced template selection
- no audit gate

- [ ] **Step 2: Verify baseline failure**

Run: pressure scenario without the skill  
Expected: agent goes straight to generation or over-freeforms the design

- [ ] **Step 3: Write minimal orchestrator skill**

The skill must require this order:
1. parse intent
2. choose whitelist template
3. generate controlled graph mutation
4. run auditor
5. only then return result

- [ ] **Step 4: Re-run baseline scenario**

Run: same prompt with skill loaded  
Expected: output follows the enforced workflow and refuses unaudited delivery

- [ ] **Step 5: Commit**

```bash
git add skills/skill-workflow-orchestrator/SKILL.md
git commit -m "feat: add orchestrator skill for ai skill workflow"
```

### Task 4: Create parser and selector skills

**Files:**
- Create: `F:\AI\封神技能编辑器\skills\skill-intent-parser\SKILL.md`
- Create: `F:\AI\封神技能编辑器\skills\skill-template-selector\SKILL.md`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\TableAnnotation.json`
- Reference: `F:\AI\封神技能编辑器\config\template-whitelist.json`

- [ ] **Step 1: Write failing scenarios**

Examples:

```md
需求 1: 直线飞行火球，命中燃烧
需求 2: 地面 AOE，延迟爆炸并附加减速
```

Expected baseline failures:
- parser omits key timing fields
- selector picks non-stable template

- [ ] **Step 2: Verify failures**

Run: scenario review without dedicated skills  
Expected: inconsistent structure and inconsistent template choices

- [ ] **Step 3: Write minimal parser skill**

Force normalized output fields:
- `skill_mode`
- `cast_timing`
- `targeting`
- `damage_model`
- `bullet_needed`
- `fx_needed`
- `fallback_allowed`

- [ ] **Step 4: Write minimal selector skill**

Force selection precedence:
- `系统功能/技能范例`
- `技能模板`
- `通用技能`
- `宗门技能`

- [ ] **Step 5: Re-run both scenarios**

Expected:
- same prompt shape yields same structured output
- template selection is explainable and whitelist-bound

- [ ] **Step 6: Commit**

```bash
git add skills/skill-intent-parser/SKILL.md skills/skill-template-selector/SKILL.md
git commit -m "feat: add parser and template selector skills"
```

### Task 5: Create graph generator skill

**Files:**
- Create: `F:\AI\封神技能编辑器\skills\skill-graph-generator\SKILL.md`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\TableAnnotation.json`
- Reference: `F:\AI\封神技能编辑器\config\resource-whitelist.json`

- [ ] **Step 1: Write the failing scenario**

```md
在合法模板上替换施法特效、子弹模型、命中 buff，并保持模板主链不变
```

Expected baseline failure:
- agent modifies too many nodes
- agent invents missing resources

- [ ] **Step 2: Verify failure**

Run: sample generation review without generator skill  
Expected: no protected-node boundary

- [ ] **Step 3: Write minimal generator skill**

Explicitly allow only:
- parameter replacement
- whitelist resource replacement
- small local node additions

Explicitly forbid:
- whole-graph rewiring
- non-whitelist resource invention
- skipping template checks

- [ ] **Step 4: Re-run the scenario**

Expected: mutation scope stays local and explainable

- [ ] **Step 5: Commit**

```bash
git add skills/skill-graph-generator/SKILL.md
git commit -m "feat: add controlled graph generator skill"
```

## Chunk 3: Build and Validate the Auditor

### Task 6: Implement the local JSON auditor

**Files:**
- Create: `F:\AI\封神技能编辑器\tools\skill-json-auditor.ps1`
- Test: `F:\AI\封神技能编辑器\tests\auditor-cases.md`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\Base\ConfigEditor\Graphs\ConfigGraph.cs`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\Nodes\Base\ConfigBaseNode.cs`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\Nodes\BaseConfig\SkillConfigNode.Custom.cs`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\Nodes\SkillEffectConfig\TSET_CREATE_BULLET.Custom.cs`
- Reference: `F:\AI\封神技能编辑器\NodeEditor\Nodes\SkillEffectConfig\TSET_RUN_SKILL_EFFECT_TEMPLATE.Custom.cs`

- [ ] **Step 1: Write the failing test cases**

Create at least these cases:

```md
Case 1: projectile node with bullet id 0 -> FAIL
Case 2: skill node missing AI range -> FAIL
Case 3: missing template path -> FAIL
Case 4: missing formal FX but fallback exists -> WARN
Case 5: unique IDs + valid refs + valid whitelist resources -> PASS
```

- [ ] **Step 2: Run the tests manually against current non-audited flow**

Expected: no automated pass/warn/fail gate exists

- [ ] **Step 3: Implement minimal auditor**

Checks required:
- graph structure consistency
- `Config2ID` uniqueness
- `TableTash` presence and consistency
- required template path validity
- whitelist resource fallback
- targeted node rules

- [ ] **Step 4: Run the auditor on representative samples**

Run:

```bash
powershell -ExecutionPolicy Bypass -File tools/skill-json-auditor.ps1 "NodeEditor/SkillEditor/Saves/Jsons/系统功能/SkillGraph_技能范例-直线飞行技能.json"
```

Expected: valid sample returns `PASS`

- [ ] **Step 5: Run the auditor on intentionally broken fixtures**

Expected:
- broken projectile fixture -> `FAIL`
- missing formal FX with fallback -> `WARN`

- [ ] **Step 6: Commit**

```bash
git add tools/skill-json-auditor.ps1 tests/auditor-cases.md
git commit -m "feat: add local skill graph auditor"
```

### Task 7: Validate the end-to-end workflow

**Files:**
- Create: `F:\AI\封神技能编辑器\examples\workflow-smoke-tests.md`

- [ ] **Step 1: Write four representative smoke prompts**

Include:
- straight projectile
- ground AOE
- hit buff
- fallback FX

- [ ] **Step 2: Run the workflow manually end to end**

Expected for each:
- structured intent produced
- whitelist template selected
- candidate graph mutated locally
- auditor returns pass/warn/fail

- [ ] **Step 3: Record failures and tighten the rules**

Update:
- whitelist entries
- skill instructions
- auditor checks

- [ ] **Step 4: Final verification**

Run:

```bash
powershell -ExecutionPolicy Bypass -File tools/skill-json-auditor.ps1 "<generated-sample.json>"
```

Expected: no critical failures on the curated smoke set

- [ ] **Step 5: Commit**

```bash
git add examples/workflow-smoke-tests.md config/*.json skills/*/SKILL.md tools/skill-json-auditor.ps1
git commit -m "feat: validate ai skill workflow end to end"
```

Plan complete and saved to `docs/superpowers/plans/2026-03-10-aigc-skill-workflow.md`. Ready to execute?

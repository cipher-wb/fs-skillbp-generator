# 封神技能蓝图生成器

这个仓库用于把自然语言技能需求收敛为可落地的 `SkillGraph JSON`，并在落库前通过本地白名单和审核脚本做约束。

当前已经落地的能力包括：

- 基于白名单模板生成直线飞行技能
- 对资源、模板、BUFF 做白名单校验
- 对生成结果做本地 JSON 审核
- 提供一份可复现的火球术示例图

## 目录结构

`config/`
- 白名单配置
- `template-whitelist.json`
- `resource-whitelist.json`
- `buff-whitelist.json`

`tools/`
- 生成器与审核脚本
- `generate-aigc-straight-projectile.ps1`
- `skill-json-auditor.ps1`
- `test-generate-aigc-fireball.ps1`
- `test-skill-json-auditor.ps1`

`skills/`
- 工作流 skill 拆分
- `skill-workflow-orchestrator`
- `skill-intent-parser`
- `skill-template-selector`
- `skill-graph-generator`

`docs/superpowers/`
- 设计文档与执行计划

`tests/fixtures/`
- 审核器回归用例

`NodeEditor/SkillEditor/Saves/Jsons/AIGC/`
- AIGC 生成后的技能图输出目录

## 当前示例

已生成示例：

- [SkillGraph_19009991_AIGC_Fireball.json](F:\AI\封神技能编辑器\NodeEditor\SkillEditor\Saves\Jsons\AIGC\SkillGraph_19009991_AIGC_Fireball.json)

技能显示名：

- `火球术`

技能描述：

- `前摇10帧，向前发射火球，命中附加灼烧。`

## 常用命令

生成火球术：

```powershell
pwsh -NoProfile -File .\tools\generate-aigc-straight-projectile.ps1 `
  -OutputPath .\NodeEditor\SkillEditor\Saves\Jsons\AIGC\SkillGraph_19009991_AIGC_Fireball.json `
  -SkillId 19009991
```

审核单个技能图：

```powershell
pwsh -NoProfile -File .\tools\skill-json-auditor.ps1 `
  -Path .\NodeEditor\SkillEditor\Saves\Jsons\AIGC\SkillGraph_19009991_AIGC_Fireball.json
```

运行生成器回归：

```powershell
pwsh -NoProfile -File .\tools\test-generate-aigc-fireball.ps1
```

运行审核器回归：

```powershell
pwsh -NoProfile -File .\tools\test-skill-json-auditor.ps1
```

## 当前约束

- 不允许自然语言直接自由生成任意拓扑
- 优先从白名单模板派生
- 资源、BUFF、模板都必须有白名单背书
- 交付前必须经过本地审核器

## 后续建议

- 扩展到地面 AOE 和自身增益两类模板
- 把资源边一致性校验继续推广到更多节点类型
- 增加统一入口脚本，按需求自动选择模板

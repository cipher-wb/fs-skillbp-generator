# Skill Pressure Scenarios

## Baseline Prompt 1

用户输入：

```md
我要一个直线飞行火球，前摇 10 帧，命中附加燃烧 buff。
```

无本地 workflow skill 时，预期失败模式：

- 直接开始拼 JSON
- 不做模板选择
- 不声明资源回退策略
- 不要求审核放行

2026-03-10 实测基线输出补充：

- 会直接给出 `BuffConfig_10002` 和回退 `1080029`，但没有任何白名单校验
- 会假定命中挂 buff 链路天然存在，没有先确认模板是否具备该分支
- 会输出看似可执行的结构化方案，但没有要求跑 `tools/skill-json-auditor.ps1`
- 会把“找不到资源就自己补一个”当成允许自由扩展资源库，而不是受控回退

## Baseline Prompt 2

用户输入：

```md
我要一个地面范围技能，延迟爆炸并减速，找不到正式特效可以先占位。
```

无本地 workflow skill 时，预期失败模式：

- 目标形态和技能骨架不结构化
- 不优先命中 `技能范例-范围AOE`
- 资源占位策略不稳定

## Pass Criteria

有本地 workflow skill 后，期望：

- 先输出结构化技能意图
- 明确选择白名单模板
- 说明允许改动和受保护主链
- 在交付前要求跑本地审核器

2026-03-10 实测通过表现：

- 选择 `system-straight-projectile` 作为基底图，而不是从零拼图
- 资源绑定收敛到 `ModelConfig_19000720 / 19000726 / 19000725`
- 燃烧 buff 收敛到白名单 `BuffConfig_1080029`
- 明确限制“只改参数，不新建拓扑，不猜 rid/GUID/edges”
- 最后一行固定要求执行 `pwsh -NoProfile -File .\tools\skill-json-auditor.ps1 -Path <graph.json>`

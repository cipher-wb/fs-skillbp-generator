# AIGC Skill Workflow Design

**Topic:** 面向战斗策划的自然语言技能生成工作流  
**Date:** 2026-03-10

## Goal

为当前 `SkillEditor` 建立一套可直接给策划使用的受控工作流，使自然语言输入能够稳定产出：

- 可被 Unity 技能编辑器打开的 `SkillGraph` JSON
- 带明确风险分级的审核报告

重点不是“从零自由生图”，而是“基于模板库、白名单和审核器的受控生成”。

## Context

当前工程已经具备 4 个关键基础：

1. `TableAnnotation.json` 提供节点语义、参数注释和引用表信息。
2. `SkillGraph` 样例库、`宗门技能`、`通用技能`、`系统功能`、`技能模板` 提供可复用骨架。
3. `ConfigGraphWindow`、`ConfigGraph`、`ConfigBaseNode` 提供保存、同步、导表、备份与基础校验链。
4. 各节点存在额外 `OnSaveCheck()` 规则，说明运行可用性不能只靠 prompt。

## Architecture

推荐采用“多 skill + 本地审核器”的分层结构：

1. `workflow skill`
   - 统一入口，编排全流程
2. `intent-parser skill`
   - 自然语言 -> 结构化技能意图
3. `template-selector skill`
   - 意图 -> 合法模板骨架
4. `graph-generator skill`
   - 模板骨架 -> 受控改写后的候选 JSON
5. `skill-json-auditor`
   - 候选 JSON -> `PASS / WARN / FAIL`

## Data Flow

1. 策划按固定字段提交技能描述。
2. 解析 skill 产出结构化意图卡片。
3. 模板选择 skill 在白名单模板中匹配骨架。
4. 生成 skill 在骨架上补参数、替换资源、少量补节点。
5. 审核器执行静态检查：
   - 图结构一致性
   - `Config2ID`/ID 唯一性
   - `TableTash` 一致性
   - 重点节点自定义规则
   - 模板/资源/配置引用有效性
6. 交付 JSON 和审核报告。

## Constraints

- 不允许从零自由拼完整 SkillGraph。
- 不允许生成不在资源白名单内的幻想资源 ID。
- 不允许跳过审核器直接交付。
- 模板优先级固定为：
  - `系统功能/技能范例`
  - `技能模板`
  - `通用技能`
  - `宗门技能`

## Error Handling

审核器至少要区分三类结果：

- `PASS`
  - 可直接试跑
- `WARN`
  - 可打开，但存在占位资源或中风险项
- `FAIL`
  - 必须回到模板选择或参数修正阶段

找不到正式资源时，必须按白名单回退到安全占位；若连占位也不存在，则直接 `FAIL`。

## Testing

第一批验证样例应覆盖：

- 直线飞行
- 范围 AOE
- 命中加 Buff
- 模板引用技能
- 占位特效回退技能

成功标准：

- 编辑器可打开
- 审核器无 `FAIL`
- Unity 中可完成基础试跑
- 出错时可追溯到需求、模板、资源或引用链问题

## Recommendation

当前阶段主推：

**多 skill 协同工作流 + 本地 JSON 审核器 + 模板/资源白名单**

后续如需要进一步产品化，再升级为：

`自然语言 -> DSL -> Unity 导入器建图`

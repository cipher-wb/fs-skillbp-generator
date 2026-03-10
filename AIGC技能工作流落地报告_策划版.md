# AIGC 技能工作流落地报告（策划版）

## 1. 结论

这套 `SkillEditor` 逻辑具备明显的工业化潜力，但**不适合**走“自然语言 -> AI 从零自由编写完整 SkillGraph JSON”路线。  
推荐落地方案是：

**多 skill 协同工作流 + 本地 JSON 审核器 + 模板白名单 + 资源白名单**

原因很直接：

- `TableAnnotation.json` 解决了“节点枚举和参数槽位看不懂”的问题。
- 现有技能库和模板库解决了“骨架从哪来”的问题。
- 通用保存链和节点自定义 `OnSaveCheck()` 说明，这套系统存在大量**隐式工程约束**，不能只靠 prompt 保证正确。
- 你的历史痛点“能打开但游戏跑不起来”“连线错”“特效找不到”，本质上都属于**需要审核器兜底**的工程问题。

因此，是否以 `skills` 结构来做？答案是：

**要用 skill，但不能只用 skill。**

更准确地说，应当以 `workflow skill` 作为策划入口，再用子 skill 完成解析、选模板、受控改写，并由本地审核器决定是否放行。

---

## 2. 基于当前工程的关键判断

本报告基于以下真实工程结构和样本得出：

- `NodeEditor/SkillEditor/Saves/TableAnnotation.json`
- `NodeEditor/Base/ConfigEditor/Graphs/ConfigGraphWindow.cs`
- `NodeEditor/Base/ConfigEditor/Graphs/ConfigGraph.cs`
- `NodeEditor/Nodes/Base/ConfigBaseNode.cs`
- `NodeEditor/Nodes/BaseConfig/SkillConfigNode.Custom.cs`
- `NodeEditor/Nodes/SkillEffectConfig/TSET_CREATE_BULLET.Custom.cs`
- `NodeEditor/Nodes/SkillEffectConfig/TSET_RUN_SKILL_EFFECT_TEMPLATE.Custom.cs`
- `NodeEditor/SkillEditor/Saves/Jsons/宗门技能`
- `NodeEditor/SkillEditor/Saves/Jsons/通用技能`
- `NodeEditor/SkillEditor/Saves/Jsons/系统功能/SkillGraph_技能范例-直线飞行技能.json`

### 2.1 SkillGraph JSON 不是普通业务 JSON

样例图文件说明，技能图 JSON 不是“一个技能对象”，而是：

- 图结构：`nodes`、`edges`
- Unity 序列化引用池：`references.RefIds`
- 节点业务数据：`ConfigJson`、`Config2ID`、`TableTash`

也就是说，AI 改写的不是单张表，而是 Unity/GraphProcessor 的图存档。

### 2.2 `TableAnnotation.json` 只解决一半问题

它非常重要，因为它把下面这些黑盒信息白盒化了：

- 节点语义 -> `EnumValue`
- 节点实现名 -> `NodeName`
- 参数索引 -> `paramsAnn`
- 参数是否引用别的表/枚举 -> `RefTableFullName`

但它**不能**解决：

- edge 是否形成合法运行时引用
- 资源 ID 是否真实存在
- 模板路径和模板 ID 是否一致
- `TableTash` 是否和当前代码版本一致
- `SkillValueConfig`、`Buff`、`筛选` 等跨表关系是否完整

### 2.3 保存链和校验链说明“光会写 JSON 远远不够”

从 `ConfigGraphWindow.cs`、`ConfigGraph.cs`、`ConfigBaseNode.cs` 可以确认：

- 保存前会执行 `CheckConfigID`
- 保存时会执行每个节点的 `OnSaveCheck()`
- `Serialize()` 会重写 `ConfigJson` 和 `TableTash`
- `SyncConfigData()` 会把配置推回 `DesignTable`，甚至同步到战斗运行态
- `ExternalExportUtil.cs` 会在导表阶段检查重复 `Config2ID`

这意味着一个图即使能被编辑器打开，也不代表它能通过完整链路，更不代表它能进游戏正常跑。

### 2.4 模板机制是这套系统最适合 AI 工业化的切入点

`TSET_RUN_SKILL_EFFECT_TEMPLATE.Custom.cs` 和 `TemplateNodeData.cs` 已经说明：

- 项目本身就支持模板路径引用
- 模板节点有自己的参数扩展机制
- 模板 ID 和模板引用路径是强约束

这对 AIGC 非常重要，因为它意味着 AI 不需要从零拼整张图，而应该：

**优先选模板骨架，再做受控补参。**

---

## 3. 为什么以前其他 AI 容易失败

结合当前代码和样本，失败原因主要有 4 类。

### 3.1 只会填参数，不会维护图存档一致性

很多 AI 只会改 `ConfigJson`，却不知道：

- `nodes` 只是 `rid`
- 真正节点对象在 `references.RefIds`
- `edges` 通过 `GUID` 和端口名连线

所以会出现“图能开，但引用链不闭合”的情况。

### 3.2 不理解节点自定义保存规则

例如：

- `TSET_CREATE_BULLET` 要求子弹 ID 不允许为 0
- `SkillConfigNode` 要求技能主/子类型、时序、AI 距离、SkillValueConfig 对齐
- 模板节点要求模板 ID 必须和模板引用一致

这些规则不是 `TableAnnotation` 能看出来的。

### 3.3 不理解模板与白名单优先级

从零生成最容易造成：

- 连线顺序错
- 命中链和施法链耦合错
- 子弹/筛选/Buff 指向错
- 资源引用乱填

而工程里已经有大量 `【模板】`、`技能范例-*`、`通用技能` 可以复用。

### 3.4 把“找不到特效”当成可自由发挥问题

这在工业化里是错误方向。  
资源缺失不能让 AI 随便幻想一个 ID，而应该：

- 优先命中正式白名单资源
- 找不到则降级到调试白模/安全占位资源
- 再找不到就拒绝交付

---

## 4. 推荐落地方案

### 4.1 核心架构

推荐固定为 6 步：

1. 策划用规范自然语言描述技能。
2. `workflow skill` 调用 `intent-parser skill` 生成结构化意图。
3. `template-selector skill` 从白名单模板中选骨架。
4. `graph-generator skill` 在骨架上做受控改写。
5. `skill-json-auditor` 对结果做静态审核。
6. 输出 JSON 和审核报告，按 `PASS / WARN / FAIL` 分级。

### 4.2 为什么是“多 skill + 审核器”

单个大 skill 的问题是：

- 入口、理解、生成、校验混在一起
- 错误很难定位
- 复用成本高

拆分后，每一步都有明确输入输出，团队里不同策划也更容易照流程使用。

### 4.3 建议的 skill 角色

建议至少拆成 4 个：

- `skill-workflow-orchestrator`
  - 入口 skill，编排全流程
- `skill-intent-parser`
  - 把自然语言转结构化技能意图
- `skill-template-selector`
  - 从合法模板/范例里选骨架
- `skill-graph-generator`
  - 只在模板骨架上做受控改写

另外补一个**本地审核器**：

- `skill-json-auditor`
  - 不负责生成，只负责放行或拦截

### 4.4 这套方案最关键的边界

AI 允许做：

- 语义解析
- 模板匹配
- 参数补齐
- 合法资源替换
- 局部补节点

AI 不允许做：

- 从零自由设计整张 SkillGraph
- 任意重排核心主链
- 幻觉资源 ID
- 在未通过审核器前直接交付

---

## 5. 策划团队实际 SOP

### 5.1 入口描述格式

策划提交技能需求时，至少包含：

- 技能类型
- 表现形态
- 目标类型
- 前摇 / 持续 / CD
- 命中效果
- 是否允许占位资源

推荐格式：

```md
技能名：
技能类型：
表现形态：
目标类型：
前摇/持续/CD：
命中后效果：
额外约束：
找不到正式资源时，是否允许占位：
```

### 5.2 模板选择优先级

固定优先级建议：

1. `系统功能/技能范例-*`
2. `技能模板`
3. `通用技能`
4. `宗门技能`

原因：

- `技能范例` 和 `模板` 更适合当干净骨架
- `宗门技能` 更适合做高阶参考，不适合大刀阔斧修改

### 5.3 交付分级

- `PASS`
  - 可直接进 Unity 试跑
- `WARN`
  - 可打开，但存在占位资源或中风险项，策划确认后试跑
- `FAIL`
  - 拒绝交付，必须回到模板选择或参数修正阶段

---

## 6. 审核器必须拦截的错误

审核器至少要覆盖 6 组规则。

### 6.1 图结构规则

检查：

- `nodes`
- `edges`
- `references.RefIds`

三层是否一致，特别是 `GUID`、端口名、`rid` 索引是否闭合。

### 6.2 唯一性规则

检查：

- `Config2ID`
- 图内节点 ID
- 重复配置

这直接对应现有保存链和导表链的硬要求。

### 6.3 表结构版本规则

检查：

- `TableTash` 是否与当前代码版本一致

否则很容易出现“可读但不稳定”的问题。

### 6.4 节点自定义规则

重点覆盖：

- `SkillConfigNode`
- `TSET_CREATE_BULLET`
- `TSET_RUN_SKILL_EFFECT_TEMPLATE`

后续再逐步扩展常见节点规则。

### 6.5 资源存在性规则

检查：

- `ModelConfig`
- `BuffConfig`
- `SkillEffectConfig`
- `SkillSelectConfig`
- 模板路径

找不到正式资源时，按白名单回退到安全占位。

### 6.6 模板/引用链规则

检查：

- 模板路径是否存在
- 模板节点 ID 是否匹配
- 被引用配置是否能查到

---

## 7. 白名单策略

### 7.1 模板白名单

不要让 AI 在全量技能库里自由挑。  
先人工整理一批“稳定骨架”：

- 施法类模板
- 弹道类模板
- 命中类模板
- 筛选/目标类模板

每条模板白名单建议记录：

- 模板路径
- 用途
- 允许替换参数
- 禁止修改的核心节点
- 是否推荐

### 7.2 资源白名单

优先建立 6 类安全占位：

- 施法特效占位
- 飞行子弹占位
- 命中特效占位
- 地面范围占位
- 通用 Buff 占位
- 调试白模占位

每条资源白名单建议记录：

- 资源类型
- 资源 ID
- 用途
- 是否占位
- 回退优先级

### 7.3 回退策略

推荐固定：

- 正式资源命中 -> 直接用
- 正式资源未命中 -> 用安全占位
- 安全占位也未命中 -> `FAIL`

这样就不会再出现“AI 自己编了个特效 ID”的情况。

---

## 8. 工业化可行性判断

### 8.1 短期判断

如果目标是：

**让策划稳定得到一个能打开、且大概率能进游戏试跑的技能图**

则当前工程具备**高可行性**。

前提是：

- 不从零自由拼图
- 只在模板骨架上生成
- 引入审核器
- 约束资源和模板白名单

### 8.2 长期判断

如果目标是：

**自然语言 -> 100% 自动生成任意复杂 SkillGraph，且无需人工确认**

则当前阶段不建议作为第一目标。

更合理的长期升级路线是：

`自然语言 -> 中间 DSL -> Unity 导入器建图`

但这属于第二阶段产品化，不应阻塞你当前要落地的流程。

---

## 9. 建议的上线顺序

### 第一阶段

- 整理模板白名单
- 整理资源白名单
- 固化策划输入规范

### 第二阶段

- 做 `workflow skill`
- 做 `template-selector`
- 做 `graph-generator`
- 做 `skill-json-auditor`

### 第三阶段

挑 4 类代表性技能试跑：

- 直线飞行
- 范围 AOE
- 命中加 Buff
- 带模板引用和占位资源的技能

### 第四阶段

把试跑问题继续沉淀进：

- 审核器规则
- 模板白名单
- 资源白名单

而不是继续堆 prompt。

---

## 10. 最终建议

给策划团队直接落地的方案，最终推荐只有一句话：

**不要追求“AI 一步到位自由生成 SkillGraph”；要建设“受控工作流”。**

这套工作流的最小工业闭环就是：

**规范输入 -> 模板选骨架 -> AI 补参数 -> 审核器放行 -> Unity 试跑**

它比“只做 skill”更稳，比“先做大插件”更快，也最贴合你当前的实际需求。

---

## 11. 本报告对应的建议交付物

如果后续开始实施，建议落地成以下产物：

- 一个入口 `workflow skill`
- 三个子 skill
- 一个本地 JSON 审核器
- 一份模板白名单
- 一份资源白名单

这样既能复用给其他策划，也能在后续逐步升级成更强的 DSL/导入器体系。

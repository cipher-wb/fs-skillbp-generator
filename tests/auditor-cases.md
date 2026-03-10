# Skill JSON Auditor Cases

## Cases

1. `valid-minimal-skillgraph.json`
Expectation: `PASS`
Reason: top-level graph structure完整，`Config2ID` 唯一，`TableTash` 合法，技能节点具备基础时序和 AI 距离。

2. `invalid-bullet-zero.json`
Expectation: `FAIL`
Reason: `SkillEffectType = 8` 且 `Params[0].Value = 0`，违反子弹节点基本规则。

3. `invalid-missing-ai-range.json`
Expectation: `FAIL`
Reason: 主技能节点 `AISkillRange = 0`，违反可试跑技能的最低要求。

4. `invalid-missing-template-path.json`
Expectation: `FAIL`
Reason: 模板节点提供了不存在的 `TemplatePath`。

5. `invalid-unwhitelisted-buff.json`
Expectation: `FAIL`
Reason: 图内引用了未进入本地 buff 白名单的 `BuffConfig`。

6. `warn-fallback-only.json`
Expectation: `WARN`
Reason: 资源命中的是白名单中的安全占位资源，而不是正式资源。

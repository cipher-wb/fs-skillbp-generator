/////////////////////////////////////
// 注意！！此代码文件由工具自动生成！！ 
// 扩展方法请新建文件扩展partial类实现 
// 如:#CONFIGNAME#Node.Custom.cs
/////////////////////////////////////

using GraphProcessor;
using System;
using TableDR;

namespace NodeEditor
{
// TEMPLATE_CONTENT_BEGIN
    #region AITaskNodeConfig: 空
    [Serializable]
    public sealed partial class AI_TNT_NULL : AITaskNodeConfigNode
    {
		// 空
        public AI_TNT_NULL() : base(AITaskNodeType.AI_TNT_NULL) { }
    }
    #endregion AITaskNodeConfig: 空


    #region AITaskNodeConfig: 并行节点
    [Serializable]
	[NodeMenuItem("AI任务节点/并行节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_CONCURRENT : AITaskNodeConfigNode
    {
		// 并行节点
		// 参数0 : 失败条件-BattleAIConcurrentNodeFailOpt
		// 参数1 : 成功条件-BattleAIConcurrentNodeSuccessOpt
		// 参数2 : 子节点结束后是否继续-1:继续
		// 参数3 : 子节点-SkillEffectConfig
        public AI_TNT_CONCURRENT() : base(AITaskNodeType.AI_TNT_CONCURRENT) { }
    }
    #endregion AITaskNodeConfig: 并行节点


    #region AITaskNodeConfig: 顺序执行节点
    [Serializable]
	[NodeMenuItem("AI任务节点/顺序执行节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_SEQUENCE : AITaskNodeConfigNode
    {
		// 顺序执行节点
		// 参数0 : 失败条件-SkillConditionConfig
		// 参数1 : 成功条件-SkillConditionConfig
		// 参数2 : 子节点-SkillEffectConfig
        public AI_TNT_SEQUENCE() : base(AITaskNodeType.AI_TNT_SEQUENCE) { }
    }
    #endregion AITaskNodeConfig: 顺序执行节点


    #region AITaskNodeConfig: 循环节点
    [Serializable]
	[NodeMenuItem("AI任务节点/循环节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_LOOP : AITaskNodeConfigNode
    {
		// 循环节点
		// 参数0 : 中断条件-SkillConditionConfig
		// 参数1 : 循环次数（-1:无限次数）
		// 参数2 : 子节点-SkillEffectConfig
        public AI_TNT_LOOP() : base(AITaskNodeType.AI_TNT_LOOP) { }
    }
    #endregion AITaskNodeConfig: 循环节点


    #region AITaskNodeConfig: 条件执行节点
    [Serializable]
	[NodeMenuItem("AI任务节点/条件执行节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_CONDITION_ACT : AITaskNodeConfigNode
    {
		// 条件执行节点
		// 参数0 : 条件-SkillConditionConfig
		// 参数1 : 条件为真的行为-SkillEffectConfig
		// 参数2 : 条件为假的行为-SkillEffectConfig
        public AI_TNT_CONDITION_ACT() : base(AITaskNodeType.AI_TNT_CONDITION_ACT) { }
    }
    #endregion AITaskNodeConfig: 条件执行节点


    #region AITaskNodeConfig: 堵塞节点
    [Serializable]
	[NodeMenuItem("AI任务节点/堵塞节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_AWAITE : AITaskNodeConfigNode
    {
		// 堵塞节点
		// 参数0 : 成功的状态条件-SkillConditionConfig
		// 参数1 : 失败状态的条件-SkillConditionConfig
		// 参数2 : 超时返回成功状态(帧数)
		// 参数3 : 立即执行-SkillEffectConfig
        public AI_TNT_AWAITE() : base(AITaskNodeType.AI_TNT_AWAITE) { }
    }
    #endregion AITaskNodeConfig: 堵塞节点


    #region AITaskNodeConfig: 条件状态节点
    [Serializable]
	[NodeMenuItem("AI任务节点/条件状态节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_CONDITION_CONFIG : AITaskNodeConfigNode
    {
		// 条件状态节点
		// 参数0 : 条件配置-SkillConditionConfig
        public AI_TNT_CONDITION_CONFIG() : base(AITaskNodeType.AI_TNT_CONDITION_CONFIG) { }
    }
    #endregion AITaskNodeConfig: 条件状态节点


    #region AITaskNodeConfig: 延迟节点
    [Serializable]
	[NodeMenuItem("AI任务节点/延迟节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_DELAY : AITaskNodeConfigNode
    {
		// 延迟节点
		// 参数0 : 延迟时间(帧数)
		// 参数1 : 子节点-延迟后再执行-SkillEffectConfig
        public AI_TNT_DELAY() : base(AITaskNodeType.AI_TNT_DELAY) { }
    }
    #endregion AITaskNodeConfig: 延迟节点


    #region AITaskNodeConfig: AISwitch分支节点
    [Serializable]
	[NodeMenuItem("AI任务节点/AISwitch分支节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_SWITCH : AITaskNodeConfigNode
    {
		// AISwitch分支节点
		// 参数0 : Switch选择值
		// 参数1 : Default子节点(未匹配任意Case时执行当前子节点)-SkillEffectConfig
		// 参数2 : Case匹配值{0}
		// 参数3 : Case子节点{0}-SkillEffectConfig
        public AI_TNT_SWITCH() : base(AITaskNodeType.AI_TNT_SWITCH) { }
    }
    #endregion AITaskNodeConfig: AISwitch分支节点


    #region AITaskNodeConfig: 选择监测节点
    [Serializable]
	[NodeMenuItem("AI任务节点/选择监测节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_SELECTOR_MONITOR : AITaskNodeConfigNode
    {
		// 选择监测节点
		// 参数0 : 是否重置子节点（默认重置，暂不支持修改）
		// 参数1 : 子节点-条件状态节点-AITaskNodeConfig
        public AI_TNT_SELECTOR_MONITOR() : base(AITaskNodeType.AI_TNT_SELECTOR_MONITOR) { }
    }
    #endregion AITaskNodeConfig: 选择监测节点


    #region AITaskNodeConfig: 总是成功节点
    [Serializable]
	[NodeMenuItem("AI任务节点/总是成功节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_ALWAYS_SUCCESSRESULT : AITaskNodeConfigNode
    {
		// 总是成功节点
		// 参数0 : 子节点-AITaskNodeConfig
        public AI_TNT_ALWAYS_SUCCESSRESULT() : base(AITaskNodeType.AI_TNT_ALWAYS_SUCCESSRESULT) { }
    }
    #endregion AITaskNodeConfig: 总是成功节点


    #region AITaskNodeConfig: 总是失败节点
    [Serializable]
	[NodeMenuItem("AI任务节点/总是失败节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_ALWAYS_FAILEDRESULT : AITaskNodeConfigNode
    {
		// 总是失败节点
		// 参数0 : 子节点-AITaskNodeConfig
        public AI_TNT_ALWAYS_FAILEDRESULT() : base(AITaskNodeType.AI_TNT_ALWAYS_FAILEDRESULT) { }
    }
    #endregion AITaskNodeConfig: 总是失败节点


    #region AITaskNodeConfig: 总是运行节点
    [Serializable]
	[NodeMenuItem("AI任务节点/总是运行节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_ALWAYS_RUNNING : AITaskNodeConfigNode
    {
		// 总是运行节点
		// 参数0 : 子节点-AITaskNodeConfig
        public AI_TNT_ALWAYS_RUNNING() : base(AITaskNodeType.AI_TNT_ALWAYS_RUNNING) { }
    }
    #endregion AITaskNodeConfig: 总是运行节点


    #region AITaskNodeConfig: 监测分支节点
    [Serializable]
	[NodeMenuItem("AI任务节点/监测分支节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_MONITROE_BRANCH : AITaskNodeConfigNode
    {
		// 监测分支节点
		// 参数0 : 条件配置-SkillConditionConfig
		// 参数1 : 子节点-SkillEffectConfig
        public AI_TNT_MONITROE_BRANCH() : base(AITaskNodeType.AI_TNT_MONITROE_BRANCH) { }
    }
    #endregion AITaskNodeConfig: 监测分支节点


    #region AITaskNodeConfig: 选择执行节点
    [Serializable]
	[NodeMenuItem("AI任务节点/选择执行节点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class AI_TNT_SELECTOR : AITaskNodeConfigNode
    {
		// 选择执行节点
		// 参数0 : 子节点-SkillEffectConfig
        public AI_TNT_SELECTOR() : base(AITaskNodeType.AI_TNT_SELECTOR) { }
    }
    #endregion AITaskNodeConfig: 选择执行节点


// TEMPLATE_CONTENT_END
}

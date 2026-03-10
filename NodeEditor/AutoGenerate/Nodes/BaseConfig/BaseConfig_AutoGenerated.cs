/////////////////////////////////////
// 注意！！此代码文件由工具自动生成！！ 
// 扩展方法请新建文件扩展partial类实现 
// 如:#CONFIGNAME#.Custom.cs
/////////////////////////////////////

using GraphProcessor;
using System;
using TableDR;

namespace NodeEditor
{
// TEMPLATE_CONTENT_BEGIN
    #region AITaskNodeConfig
    [Serializable]
    public partial class AITaskNodeConfigNode : ConfigBaseNode<AITaskNodeConfig>
    {
        public AITaskNodeConfigNode() : base() { }
    }
    #endregion AITaskNodeConfig

    #region BattleAIConfig
    [Serializable]
	[NodeMenuItem("AI配置/BattleAIConfig", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BattleAIConfigNode : ConfigBaseNode<BattleAIConfig>
    {
        public BattleAIConfigNode() : base() { }
    }
    #endregion BattleAIConfig

    #region BattleCameraShakeConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BattleCameraShakeConfig_震屏配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleCameraShakeConfig_震屏配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleCameraShakeConfig_震屏配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BattleCameraShakeConfigNode : ConfigBaseNode<BattleCameraShakeConfig>
    {
        public BattleCameraShakeConfigNode() : base() { }
    }
    #endregion BattleCameraShakeConfig

    #region BattleCustomParamConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BattleCustomParamConfig_显示层配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleCustomParamConfig_显示层配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleCustomParamConfig_显示层配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BattleCustomParamConfigNode : ConfigBaseNode<BattleCustomParamConfig>
    {
        public BattleCustomParamConfigNode() : base() { }
    }
    #endregion BattleCustomParamConfig

    #region BattleFollowConfig
    [Serializable]
    public partial class BattleFollowConfigNode : ConfigBaseNode<BattleFollowConfig>
    {
        public BattleFollowConfigNode() : base() { }
    }
    #endregion BattleFollowConfig

    #region BattleHitFlyForceConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BattleHitFlyForceConfig_击飞力道值配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleHitFlyForceConfig_击飞力道值配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BattleHitFlyForceConfig_击飞力道值配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BattleHitFlyForceConfigNode : ConfigBaseNode<BattleHitFlyForceConfig>
    {
        public BattleHitFlyForceConfigNode() : base() { }
    }
    #endregion BattleHitFlyForceConfig

    #region BattleUnitConfig
    [Serializable]
    public partial class BattleUnitConfigNode : ConfigBaseNode<BattleUnitConfig>
    {
        public BattleUnitConfigNode() : base() { }
    }
    #endregion BattleUnitConfig

    #region BehaviorConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BehaviorConfig_任务行为配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BehaviorConfig_任务行为配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BehaviorConfig_任务行为配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BehaviorConfigNode : ConfigBaseNode<BehaviorConfig>
    {
        public BehaviorConfigNode() : base() { }
    }
    #endregion BehaviorConfig

    #region BuffConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BuffConfig_Buff配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BuffConfig_Buff配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BuffConfig_Buff配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BuffConfigNode : ConfigBaseNode<BuffConfig>
    {
        public BuffConfigNode() : base() { }
    }
    #endregion BuffConfig

    #region BulletConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/BulletConfig_子弹配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BulletConfig_子弹配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/BulletConfig_子弹配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class BulletConfigNode : ConfigBaseNode<BulletConfig>
    {
        public BulletConfigNode() : base() { }
    }
    #endregion BulletConfig

    #region FixtureConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/FixtureConfig_碰撞配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/FixtureConfig_碰撞配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/FixtureConfig_碰撞配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class FixtureConfigNode : ConfigBaseNode<FixtureConfig>
    {
        public FixtureConfigNode() : base() { }
    }
    #endregion FixtureConfig

    #region GuideConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/GuideConfig_引导配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/GuideConfig_引导配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/GuideConfig_引导配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class GuideConfigNode : ConfigBaseNode<GuideConfig>
    {
        public GuideConfigNode() : base() { }
    }
    #endregion GuideConfig

    #region MapAnimStateConfig
    [Serializable]
	[NodeMenuItem("MapAnimStateConfig", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public partial class MapAnimStateConfigNode : ConfigBaseNode<MapAnimStateConfig>
    {
        public MapAnimStateConfigNode() : base() { }
    }
    #endregion MapAnimStateConfig

    #region MapEventActorFormationConfig
    [Serializable]
	[NodeMenuItem("MapEventActorFormationConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventActorFormationConfigNode : ConfigBaseNode<MapEventActorFormationConfig>
    {
        public MapEventActorFormationConfigNode() : base() { }
    }
    #endregion MapEventActorFormationConfig

    #region MapEventConditionConfig
    [Serializable]
	[NodeMenuItem("MapEventConditionConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventConditionConfigNode : ConfigBaseNode<MapEventConditionConfig>
    {
        public MapEventConditionConfigNode() : base() { }
    }
    #endregion MapEventConditionConfig

    #region MapEventConditionGroupConfig
    [Serializable]
	[NodeMenuItem("MapEventConditionGroupConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventConditionGroupConfigNode : ConfigBaseNode<MapEventConditionGroupConfig>
    {
        public MapEventConditionGroupConfigNode() : base() { }
    }
    #endregion MapEventConditionGroupConfig

    #region MapEventFormulaConfig
    [Serializable]
	[NodeMenuItem("MapEventFormulaConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventFormulaConfigNode : ConfigBaseNode<MapEventFormulaConfig>
    {
        public MapEventFormulaConfigNode() : base() { }
    }
    #endregion MapEventFormulaConfig

    #region MapEventGeneralFuncConfig
    [Serializable]
	[NodeMenuItem("MapEventGeneralFuncConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventGeneralFuncConfigNode : ConfigBaseNode<MapEventGeneralFuncConfig>
    {
        public MapEventGeneralFuncConfigNode() : base() { }
    }
    #endregion MapEventGeneralFuncConfig

    #region MapEventGeneralFuncGroupConfig
    [Serializable]
	[NodeMenuItem("MapEventGeneralFuncGroupConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventGeneralFuncGroupConfigNode : ConfigBaseNode<MapEventGeneralFuncGroupConfig>
    {
        public MapEventGeneralFuncGroupConfigNode() : base() { }
    }
    #endregion MapEventGeneralFuncGroupConfig

    #region MapEventPerformanceConfig
    [Serializable]
	[NodeMenuItem("MapEventPerformanceConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventPerformanceConfigNode : ConfigBaseNode<MapEventPerformanceConfig>
    {
        public MapEventPerformanceConfigNode() : base() { }
    }
    #endregion MapEventPerformanceConfig

    #region MapEventPerformanceGroupConfig
    [Serializable]
	[NodeMenuItem("MapEventPerformanceGroupConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventPerformanceGroupConfigNode : ConfigBaseNode<MapEventPerformanceGroupConfig>
    {
        public MapEventPerformanceGroupConfigNode() : base() { }
    }
    #endregion MapEventPerformanceGroupConfig

    #region MapEventStoryConfig
    [Serializable]
	[NodeMenuItem("MapEventStoryConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class MapEventStoryConfigNode : ConfigBaseNode<MapEventStoryConfig>
    {
        public MapEventStoryConfigNode() : base() { }
    }
    #endregion MapEventStoryConfig

    #region ModelConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/ModelConfig_模型配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("ModelConfig_模型配置", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/ModelConfig_模型配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/ModelConfig_模型配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class ModelConfigNode : ConfigBaseNode<ModelConfig>
    {
        public ModelConfigNode() : base() { }
    }
    #endregion ModelConfig

    #region NpcEventActionConfig
    [Serializable]
	[NodeMenuItem("NpcEventActionConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventActionConfigNode : ConfigBaseNode<NpcEventActionConfig>
    {
        public NpcEventActionConfigNode() : base() { }
    }
    #endregion NpcEventActionConfig

    #region NpcEventActionGroupConfig
    [Serializable]
	[NodeMenuItem("NpcEventActionGroupConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventActionGroupConfigNode : ConfigBaseNode<NpcEventActionGroupConfig>
    {
        public NpcEventActionGroupConfigNode() : base() { }
    }
    #endregion NpcEventActionGroupConfig

    #region NpcEventActionGroupIndexConfig
    [Serializable]
	[NodeMenuItem("NpcEventActionGroupIndexConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventActionGroupIndexConfigNode : ConfigBaseNode<NpcEventActionGroupIndexConfig>
    {
        public NpcEventActionGroupIndexConfigNode() : base() { }
    }
    #endregion NpcEventActionGroupIndexConfig

    #region NpcEventConfig
    [Serializable]
	[NodeMenuItem("NpcEventConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventConfigNode : ConfigBaseNode<NpcEventConfig>
    {
        public NpcEventConfigNode() : base() { }
    }
    #endregion NpcEventConfig

    #region NpcEventLinkConfig
    [Serializable]
	[NodeMenuItem("NpcEventLinkConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventLinkConfigNode : ConfigBaseNode<NpcEventLinkConfig>
    {
        public NpcEventLinkConfigNode() : base() { }
    }
    #endregion NpcEventLinkConfig

    #region NpcEventModelConfig
    [Serializable]
	[NodeMenuItem("NpcEventModelConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcEventModelConfigNode : ConfigBaseNode<NpcEventModelConfig>
    {
        public NpcEventModelConfigNode() : base() { }
    }
    #endregion NpcEventModelConfig

    #region NpcTalkConfig
    [Serializable]
	[NodeMenuItem("NpcTalkConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcTalkConfigNode : ConfigBaseNode<NpcTalkConfig>
    {
        public NpcTalkConfigNode() : base() { }
    }
    #endregion NpcTalkConfig

    #region NpcTalkGroupConfig
    [Serializable]
	[NodeMenuItem("NpcTalkGroupConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcTalkGroupConfigNode : ConfigBaseNode<NpcTalkGroupConfig>
    {
        public NpcTalkGroupConfigNode() : base() { }
    }
    #endregion NpcTalkGroupConfig

    #region NpcTalkOptionConfig
    [Serializable]
	[NodeMenuItem("NpcTalkOptionConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcTalkOptionConfigNode : ConfigBaseNode<NpcTalkOptionConfig>
    {
        public NpcTalkOptionConfigNode() : base() { }
    }
    #endregion NpcTalkOptionConfig

    #region NpcTemplateRuleConfig
    [Serializable]
	[NodeMenuItem("NpcTemplateRuleConfig", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public partial class NpcTemplateRuleConfigNode : ConfigBaseNode<NpcTemplateRuleConfig>
    {
        public NpcTemplateRuleConfigNode() : base() { }
    }
    #endregion NpcTemplateRuleConfig

    #region ProductionAlchemyFurnaceConfig
    [Serializable]
    public partial class ProductionAlchemyFurnaceConfigNode : ConfigBaseNode<ProductionAlchemyFurnaceConfig>
    {
        public ProductionAlchemyFurnaceConfigNode() : base() { }
    }
    #endregion ProductionAlchemyFurnaceConfig

    #region SkillConditionConfig
    [Serializable]
    public partial class SkillConditionConfigNode : ConfigBaseNode<SkillConditionConfig>
    {
        public SkillConditionConfigNode() : base() { }
    }
    #endregion SkillConditionConfig

    #region SkillConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/SkillConfig_技能配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillConfig_技能配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillConfig_技能配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class SkillConfigNode : ConfigBaseNode<SkillConfig>
    {
        public SkillConfigNode() : base() { }
    }
    #endregion SkillConfig

    #region SkillEffectConfig
    [Serializable]
    public partial class SkillEffectConfigNode : ConfigBaseNode<SkillEffectConfig>
    {
        public SkillEffectConfigNode() : base() { }
    }
    #endregion SkillEffectConfig

    #region SkillEventConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/SkillEventConfig_技能事件配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillEventConfig_技能事件配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillEventConfig_技能事件配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class SkillEventConfigNode : ConfigBaseNode<SkillEventConfig>
    {
        public SkillEventConfigNode() : base() { }
    }
    #endregion SkillEventConfig

    #region SkillInterruptConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/SkillInterruptConfig_技能中断_打断配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillInterruptConfig_技能中断_打断配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillInterruptConfig_技能中断_打断配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class SkillInterruptConfigNode : ConfigBaseNode<SkillInterruptConfig>
    {
        public SkillInterruptConfigNode() : base() { }
    }
    #endregion SkillInterruptConfig

    #region SkillSelectConfig
    [Serializable]
    public partial class SkillSelectConfigNode : ConfigBaseNode<SkillSelectConfig>
    {
        public SkillSelectConfigNode() : base() { }
    }
    #endregion SkillSelectConfig

    #region SkillTagsConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/SkillTagsConfig_技能参数配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillTagsConfig_技能参数配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillTagsConfig_技能参数配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class SkillTagsConfigNode : ConfigBaseNode<SkillTagsConfig>
    {
        public SkillTagsConfigNode() : base() { }
    }
    #endregion SkillTagsConfig

    #region SkillTargetCondTemplateConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/SkillTargetCondTemplateConfig_智能施法目标条件配置表", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillTargetCondTemplateConfig_智能施法目标条件配置表", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/SkillTargetCondTemplateConfig_智能施法目标条件配置表", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class SkillTargetCondTemplateConfigNode : ConfigBaseNode<SkillTargetCondTemplateConfig>
    {
        public SkillTargetCondTemplateConfigNode() : base() { }
    }
    #endregion SkillTargetCondTemplateConfig

    #region TextConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/TextConfig_文本配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/TextConfig_文本配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/TextConfig_文本配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class TextConfigNode : ConfigBaseNode<TextConfig>
    {
        public TextConfigNode() : base() { }
    }
    #endregion TextConfig

    #region VoiceConfig
    [Serializable]
	[NodeMenuItem("基础表格节点/VoiceConfig_音效配置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/VoiceConfig_音效配置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/基础表格节点/VoiceConfig_音效配置", typeof(NodeEditor.AIEditor.AIGraph))]
    public partial class VoiceConfigNode : ConfigBaseNode<VoiceConfig>
    {
        public VoiceConfigNode() : base() { }
    }
    #endregion VoiceConfig

// TEMPLATE_CONTENT_END
}

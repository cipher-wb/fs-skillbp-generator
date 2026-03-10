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
    #region SkillConditionConfig: 条件与
    [Serializable]
	[NodeMenuItem("技能条件/条件与", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件与", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件与", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_AND : SkillConditionConfigNode
    {
		// 条件与
		// 参数0 : 条件ID-SkillConditionConfig
        public TSCT_AND() : base(TSkillConditionType.TSCT_AND) { }
    }
    #endregion SkillConditionConfig: 条件与


    #region SkillConditionConfig: 条件或
    [Serializable]
	[NodeMenuItem("技能条件/条件或", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件或", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件或", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_OR : SkillConditionConfigNode
    {
		// 条件或
		// 参数0 : 条件ID-SkillConditionConfig
        public TSCT_OR() : base(TSkillConditionType.TSCT_OR) { }
    }
    #endregion SkillConditionConfig: 条件或


    #region SkillConditionConfig: 条件非
    [Serializable]
	[NodeMenuItem("技能条件/条件非", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件非", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/条件非", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_NOT : SkillConditionConfigNode
    {
		// 条件非
		// 参数0 : 条件ID-SkillConditionConfig
        public TSCT_NOT() : base(TSkillConditionType.TSCT_NOT) { }
    }
    #endregion SkillConditionConfig: 条件非


    #region SkillConditionConfig: 阵营相等判断
    [Serializable]
	[NodeMenuItem("技能条件/阵营/阵营相等判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/阵营/阵营相等判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/阵营/阵营相等判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SAME_CAMP : SkillConditionConfigNode
    {
		// 阵营相等判断
        public TSCT_SAME_CAMP() : base(TSkillConditionType.TSCT_SAME_CAMP) { }
    }
    #endregion SkillConditionConfig: 阵营相等判断


    #region SkillConditionConfig: 单位类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_TYPE : SkillConditionConfigNode
    {
		// 单位类型判断
		// 参数0 : 对比单位
		// 参数1 : 单位类型-TEntityType
        public TSCT_ENTITY_TYPE() : base(TSkillConditionType.TSCT_ENTITY_TYPE) { }
    }
    #endregion SkillConditionConfig: 单位类型判断


    #region SkillConditionConfig: 单位状态判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位状态判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位状态判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位状态判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_STATE : SkillConditionConfigNode
    {
		// 单位状态判断
		// 参数0 : 单位实例ID
		// 参数1 : 单位状态-TEntityState
        public TSCT_ENTITY_STATE() : base(TSkillConditionType.TSCT_ENTITY_STATE) { }
    }
    #endregion SkillConditionConfig: 单位状态判断


    #region SkillConditionConfig: 数值判断
    [Serializable]
	[NodeMenuItem("技能条件/数值判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/数值判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/数值判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_VALUE_COMPARE : SkillConditionConfigNode
    {
		// 数值判断
		// 参数0 : 数值A
		// 参数1 : 比较类型-TConditionOperator
		// 参数2 : 数值B
        public TSCT_VALUE_COMPARE() : base(TSkillConditionType.TSCT_VALUE_COMPARE) { }
    }
    #endregion SkillConditionConfig: 数值判断


    #region SkillConditionConfig: 单位技能参数判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位技能参数判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位技能参数判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位技能参数判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILLTAGS : SkillConditionConfigNode
    {
		// 单位技能参数判断
		// 参数0 : 单位实例ID
		// 参数1 : 技能表格ID-SkillConfig
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 比较类型-TConditionOperator
		// 参数4 : 数值
		// 参数5 : 技能参数类型(默认-技能)-TSkillTagsType
		// 参数6 : 是否获取最终值（按照Tag表配置效果）
        public TSCT_SKILLTAGS() : base(TSkillConditionType.TSCT_SKILLTAGS) { }
    }
    #endregion SkillConditionConfig: 单位技能参数判断


    #region SkillConditionConfig: 恒真
    [Serializable]
	[NodeMenuItem("技能条件/恒真", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/恒真", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/恒真", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_TRUE : SkillConditionConfigNode
    {
		// 恒真
		// 参数0 : 技能效果ID-SkillEffectConfig
        public TSCT_TRUE() : base(TSkillConditionType.TSCT_TRUE) { }
    }
    #endregion SkillConditionConfig: 恒真


    #region SkillConditionConfig: 恒假
    [Serializable]
	[NodeMenuItem("技能条件/恒假", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/恒假", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/恒假", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_FALSE : SkillConditionConfigNode
    {
		// 恒假
		// 参数0 : 技能效果ID-SkillEffectConfig
        public TSCT_FALSE() : base(TSkillConditionType.TSCT_FALSE) { }
    }
    #endregion SkillConditionConfig: 恒假


    #region SkillConditionConfig: 是否五行克制
    [Serializable]
	[NodeMenuItem("技能条件/是否五行克制", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否五行克制", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否五行克制", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_ELEMENT_COUNTER : SkillConditionConfigNode
    {
		// 是否五行克制
		// 参数0 : 五行类型A-TElementsType
		// 参数1 : 五行类型B-TElementsType
        public TSCT_IS_ELEMENT_COUNTER() : base(TSkillConditionType.TSCT_IS_ELEMENT_COUNTER) { }
    }
    #endregion SkillConditionConfig: 是否五行克制


    #region SkillConditionConfig: 是否是主控单位
    [Serializable]
	[NodeMenuItem("技能条件/单位/是否是主控单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否是主控单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否是主控单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_MAIN_CONTROL_ENTITY : SkillConditionConfigNode
    {
		// 是否是主控单位
		// 参数0 : 单位实例ID
        public TSCT_IS_MAIN_CONTROL_ENTITY() : base(TSkillConditionType.TSCT_IS_MAIN_CONTROL_ENTITY) { }
    }
    #endregion SkillConditionConfig: 是否是主控单位


    #region SkillConditionConfig: 角色是否骑乘坐骑
    [Serializable]
	[NodeMenuItem("技能条件/单位/角色是否骑乘坐骑", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/角色是否骑乘坐骑", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/角色是否骑乘坐骑", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_RIDING_MOUNT : SkillConditionConfigNode
    {
		// 角色是否骑乘坐骑
		// 参数0 : 角色单位实例ID
        public TSCT_IS_RIDING_MOUNT() : base(TSkillConditionType.TSCT_IS_RIDING_MOUNT) { }
    }
    #endregion SkillConditionConfig: 角色是否骑乘坐骑


    #region SkillConditionConfig: 技能表数据比较
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能表数据比较", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能表数据比较", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能表数据比较", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_CONFIG_DATA_COMPARE : SkillConditionConfigNode
    {
		// 技能表数据比较
		// 参数0 : 技能ID
		// 参数1 : 技能表数据字段类型-TSkillConfigDataFieldType
		// 参数2 : 比较数值
		// 参数3 : 技能等级
		// 参数4 : 单位实例ID
        public TSCT_SKILL_CONFIG_DATA_COMPARE() : base(TSkillConditionType.TSCT_SKILL_CONFIG_DATA_COMPARE) { }
    }
    #endregion SkillConditionConfig: 技能表数据比较


    #region SkillConditionConfig: 是否拥有BUFF
    [Serializable]
	[NodeMenuItem("技能条件/单位/是否拥有BUFF", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否拥有BUFF", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否拥有BUFF", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_HAS_BUFF : SkillConditionConfigNode
    {
		// 是否拥有BUFF
		// 参数0 : 单位实例ID
		// 参数1 : BuffID-BuffConfig
		// 参数2 : Buff来源单位实例ID（0不区分）
        public TSCT_HAS_BUFF() : base(TSkillConditionType.TSCT_HAS_BUFF) { }
    }
    #endregion SkillConditionConfig: 是否拥有BUFF


    #region SkillConditionConfig: AI半径范围是否有敌人
    [Serializable]
	[NodeMenuItem("技能条件/AI半径范围是否有敌人", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/AI半径范围是否有敌人", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/AI半径范围是否有敌人", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_AI_HAS_ENEMY : SkillConditionConfigNode
    {
		// AI半径范围是否有敌人
		// 参数0 : 半径范围
        public TSCT_AI_HAS_ENEMY() : base(TSkillConditionType.TSCT_AI_HAS_ENEMY) { }
    }
    #endregion SkillConditionConfig: AI半径范围是否有敌人


    #region SkillConditionConfig: 数值是否有效
    [Serializable]
	[NodeMenuItem("技能条件/数值是否有效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/数值是否有效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/数值是否有效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_VALID_VALUE : SkillConditionConfigNode
    {
		// 数值是否有效
		// 参数0 : 数值
        public TSCT_IS_VALID_VALUE() : base(TSkillConditionType.TSCT_IS_VALID_VALUE) { }
    }
    #endregion SkillConditionConfig: 数值是否有效


    #region SkillConditionConfig: 模板条件
    [Serializable]
	[NodeMenuItem("技能条件/模板条件", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/模板条件", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/模板条件", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_GET_TEMPLATE_COND_RESULT : SkillConditionConfigNode
    {
		// 模板条件
		// 参数0 : 主体单位实例ID
		// 参数1 : 模板条件ID-SkillConditionConfig
        public TSCT_GET_TEMPLATE_COND_RESULT() : base(TSkillConditionType.TSCT_GET_TEMPLATE_COND_RESULT) { }
    }
    #endregion SkillConditionConfig: 模板条件


    #region SkillConditionConfig: 单位属性类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位属性类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位属性类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位属性类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_NATURE_TYPE : SkillConditionConfigNode
    {
		// 单位属性类型判断
		// 参数0 : 参数
		// 参数1 : 比较类型-TConditionOperator
		// 参数2 : 属性枚举类型-TBattleNatureEnum
        public TSCT_ENTITY_NATURE_TYPE() : base(TSkillConditionType.TSCT_ENTITY_NATURE_TYPE) { }
    }
    #endregion SkillConditionConfig: 单位属性类型判断


    #region SkillConditionConfig: 阵营不相等判断
    [Serializable]
	[NodeMenuItem("技能条件/阵营/阵营不相等判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/阵营/阵营不相等判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/阵营/阵营不相等判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_NOT_SAME_CAMP : SkillConditionConfigNode
    {
		// 阵营不相等判断
        public TSCT_NOT_SAME_CAMP() : base(TSkillConditionType.TSCT_NOT_SAME_CAMP) { }
    }
    #endregion SkillConditionConfig: 阵营不相等判断


    #region SkillConditionConfig: 单位AI标签判断
    [Serializable]
	[NodeMenuItem("效果/单位/单位AI标签判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_AITAG : SkillConditionConfigNode
    {
		// 单位AI标签判断
		// 参数0 : 单位实例ID
		// 参数1 : 比较类型-TConditionOperator
		// 参数2 : 比较值-BattleAITag
        public TSCT_ENTITY_AITAG() : base(TSkillConditionType.TSCT_ENTITY_AITAG) { }
    }
    #endregion SkillConditionConfig: 单位AI标签判断


    #region SkillConditionConfig: 单位ID判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位ID判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位ID判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位ID判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_ID_CHECK : SkillConditionConfigNode
    {
		// 单位ID判断
		// 参数0 : 单位实例ID
        public TSCT_ENTITY_ID_CHECK() : base(TSkillConditionType.TSCT_ENTITY_ID_CHECK) { }
    }
    #endregion SkillConditionConfig: 单位ID判断


    #region SkillConditionConfig: 单位是否在技能单位组
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位是否在技能单位组", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位是否在技能单位组", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位是否在技能单位组", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITYGROUP_HAS_ID : SkillConditionConfigNode
    {
		// 单位是否在技能单位组
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 被判断的单位实例ID
		// 参数2 : 单位组所属单位实例ID
		// 参数3 : 技能ID
        public TSCT_ENTITYGROUP_HAS_ID() : base(TSkillConditionType.TSCT_ENTITYGROUP_HAS_ID) { }
    }
    #endregion SkillConditionConfig: 单位是否在技能单位组


    #region SkillConditionConfig: 战斗单位类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/战斗单位类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/战斗单位类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/战斗单位类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BATTLE_UNIT_TYPE : SkillConditionConfigNode
    {
		// 战斗单位类型判断
		// 参数0 : 单位实例ID
		// 参数1 : 战斗单位类型-UnitType
        public TSCT_BATTLE_UNIT_TYPE() : base(TSkillConditionType.TSCT_BATTLE_UNIT_TYPE) { }
    }
    #endregion SkillConditionConfig: 战斗单位类型判断


    #region SkillConditionConfig: 技能是否CD可用
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能是否CD可用", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能是否CD可用", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能是否CD可用", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILLCD : SkillConditionConfigNode
    {
		// 技能是否CD可用
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 单位实例ID（0：默认主体单位）
        public TSCT_SKILLCD() : base(TSkillConditionType.TSCT_SKILLCD) { }
    }
    #endregion SkillConditionConfig: 技能是否CD可用


    #region SkillConditionConfig: 是否可使用技能
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否可使用技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否可使用技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否可使用技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CAN_USE_SKILL : SkillConditionConfigNode
    {
		// 是否可使用技能
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 跳过AI目标判断
		// 参数2 : 跳过距离判断
		// 参数3 : 跳过施法状态判断（通常BOSS不跳过）
        public TSCT_CAN_USE_SKILL() : base(TSkillConditionType.TSCT_CAN_USE_SKILL) { }
    }
    #endregion SkillConditionConfig: 是否可使用技能


    #region SkillConditionConfig: 目标单位是否存在
    [Serializable]
	[NodeMenuItem("技能条件/单位/目标单位是否存在", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/目标单位是否存在", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/目标单位是否存在", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_TARGET_ENTITY_EXIST : SkillConditionConfigNode
    {
		// 目标单位是否存在
		// 参数0 : 目标单位实例ID
        public TSCT_IS_TARGET_ENTITY_EXIST() : base(TSkillConditionType.TSCT_IS_TARGET_ENTITY_EXIST) { }
    }
    #endregion SkillConditionConfig: 目标单位是否存在


    #region SkillConditionConfig: 周围是否有子弹
    [Serializable]
	[NodeMenuItem("效果/单位/周围是否有子弹", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BULLET_AROUND : SkillConditionConfigNode
    {
		// 周围是否有子弹
		// 参数0 : 技能标签组-BattleSkillTagGroupConfig
		// 参数1 : 躲避技能范围
        public TSCT_BULLET_AROUND() : base(TSkillConditionType.TSCT_BULLET_AROUND) { }
    }
    #endregion SkillConditionConfig: 周围是否有子弹


    #region SkillConditionConfig: 是否存在指定功能标签的技能
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否存在指定功能标签的技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在指定功能标签的技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在指定功能标签的技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_EXIST_FEATURE_LABEL_SKILL : SkillConditionConfigNode
    {
		// 是否存在指定功能标签的技能
		// 参数0 : 单位实例ID
		// 参数1 : 指定功能标签-TSkillFeatureLabel
        public TSCT_IS_EXIST_FEATURE_LABEL_SKILL() : base(TSkillConditionType.TSCT_IS_EXIST_FEATURE_LABEL_SKILL) { }
    }
    #endregion SkillConditionConfig: 是否存在指定功能标签的技能


    #region SkillConditionConfig: 判断ai任务节点的状态
    [Serializable]
	[NodeMenuItem("技能条件/判断ai任务节点的状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/判断ai任务节点的状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/判断ai任务节点的状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_AI_TASKNODE_STATE : SkillConditionConfigNode
    {
		// 判断ai任务节点的状态
		// 参数0 : 单位实例ID
		// 参数1 : AI任务节点-AITaskNodeConfig
		// 参数2 : AI任务节点状态-TAiNodeState
        public TSCT_AI_TASKNODE_STATE() : base(TSkillConditionType.TSCT_AI_TASKNODE_STATE) { }
    }
    #endregion SkillConditionConfig: 判断ai任务节点的状态


    #region SkillConditionConfig: 伙伴的连携技能是否与主角的某个槽位绑定
    [Serializable]
	[NodeMenuItem("技能条件/伙伴的连携技能是否与主角的某个槽位绑定", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/伙伴的连携技能是否与主角的某个槽位绑定", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/伙伴的连携技能是否与主角的某个槽位绑定", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_CONNECTIVITY_ISBIND : SkillConditionConfigNode
    {
		// 伙伴的连携技能是否与主角的某个槽位绑定
		// 参数0 : 伙伴实体ID
		// 参数1 : 主角槽位ID-TSkillSlotType
        public TSCT_SKILL_CONNECTIVITY_ISBIND() : base(TSkillConditionType.TSCT_SKILL_CONNECTIVITY_ISBIND) { }
    }
    #endregion SkillConditionConfig: 伙伴的连携技能是否与主角的某个槽位绑定


    #region SkillConditionConfig: 指定技能是否满足Tips类型技能的前置条件
    [Serializable]
	[NodeMenuItem("技能条件/技能/指定技能是否满足Tips类型技能的前置条件", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/指定技能是否满足Tips类型技能的前置条件", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/指定技能是否满足Tips类型技能的前置条件", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_TIPS_PRE_CONDITION : SkillConditionConfigNode
    {
		// 指定技能是否满足Tips类型技能的前置条件
		// 参数0 : 单位实例ID
		// 参数1 : 被检测的技能ID
        public TSCT_SKILL_TIPS_PRE_CONDITION() : base(TSkillConditionType.TSCT_SKILL_TIPS_PRE_CONDITION) { }
    }
    #endregion SkillConditionConfig: 指定技能是否满足Tips类型技能的前置条件


    #region SkillConditionConfig: 骑乘的坐骑是否为剑
    [Serializable]
	[NodeMenuItem("技能条件/单位/骑乘的坐骑是否为剑", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/骑乘的坐骑是否为剑", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/骑乘的坐骑是否为剑", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_RIDE_SWORD_MOUNT : SkillConditionConfigNode
    {
		// 骑乘的坐骑是否为剑
		// 参数0 : 骑乘者单位实例ID
        public TSCT_IS_RIDE_SWORD_MOUNT() : base(TSkillConditionType.TSCT_IS_RIDE_SWORD_MOUNT) { }
    }
    #endregion SkillConditionConfig: 骑乘的坐骑是否为剑


    #region SkillConditionConfig: 战斗任务-判断战斗结束时达成的级别
    [Serializable]
	[NodeMenuItem("技能条件/战斗任务-判断战斗结束时达成的级别", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗任务-判断战斗结束时达成的级别", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗任务-判断战斗结束时达成的级别", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MISSION_COMPLETE_GRADE_TYPE : SkillConditionConfigNode
    {
		// 战斗任务-判断战斗结束时达成的级别
		// 参数0 : 是否达成某级别-TBattleMissionGradeType
        public TSCT_MISSION_COMPLETE_GRADE_TYPE() : base(TSkillConditionType.TSCT_MISSION_COMPLETE_GRADE_TYPE) { }
    }
    #endregion SkillConditionConfig: 战斗任务-判断战斗结束时达成的级别


    #region SkillConditionConfig: 战斗任务-判断最终任务是否为分级任务
    [Serializable]
	[NodeMenuItem("技能条件/战斗任务-判断最终任务是否为分级任务", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗任务-判断最终任务是否为分级任务", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗任务-判断最终任务是否为分级任务", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MISSION_IS_GRADE_MISSION : SkillConditionConfigNode
    {
		// 战斗任务-判断最终任务是否为分级任务
        public TSCT_MISSION_IS_GRADE_MISSION() : base(TSkillConditionType.TSCT_MISSION_IS_GRADE_MISSION) { }
    }
    #endregion SkillConditionConfig: 战斗任务-判断最终任务是否为分级任务


    #region SkillConditionConfig: 单位子类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位子类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位子类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位子类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_SUB_TYPE : SkillConditionConfigNode
    {
		// 单位子类型判断
		// 参数0 : 对比单位
		// 参数1 : 单位子类型-TEntitySubType
        public TSCT_ENTITY_SUB_TYPE() : base(TSkillConditionType.TSCT_ENTITY_SUB_TYPE) { }
    }
    #endregion SkillConditionConfig: 单位子类型判断


    #region SkillConditionConfig: 单位种族类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位种族类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位种族类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位种族类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_RACE_TYPE : SkillConditionConfigNode
    {
		// 单位种族类型判断
		// 参数0 : 单位实例ID
		// 参数1 : 单位种族类型-TRaceType-TRaceType
        public TSCT_ENTITY_RACE_TYPE() : base(TSkillConditionType.TSCT_ENTITY_RACE_TYPE) { }
    }
    #endregion SkillConditionConfig: 单位种族类型判断


    #region SkillConditionConfig: 单位种族子类型判断
    [Serializable]
	[NodeMenuItem("技能条件/单位/单位种族子类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位种族子类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/单位种族子类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_ENTITY_RACE_SUB_TYPE : SkillConditionConfigNode
    {
		// 单位种族子类型判断
		// 参数0 : 单位实例ID
		// 参数1 : 单位种族子类型-TRaceSubType-TRaceSubType
        public TSCT_ENTITY_RACE_SUB_TYPE() : base(TSkillConditionType.TSCT_ENTITY_RACE_SUB_TYPE) { }
    }
    #endregion SkillConditionConfig: 单位种族子类型判断


    #region SkillConditionConfig: 怪物等阶判断
    [Serializable]
    public sealed partial class TSCT_ENTITY_MONSTER_RANK : SkillConditionConfigNode
    {
		// 怪物等阶判断
		// 参数0 : 单位实例ID
		// 参数1 : 怪物等阶类型-TMonsterRankEnum-TMonsterRankEnum
        public TSCT_ENTITY_MONSTER_RANK() : base(TSkillConditionType.TSCT_ENTITY_MONSTER_RANK) { }
    }
    #endregion SkillConditionConfig: 怪物等阶判断


    #region SkillConditionConfig: 执行技能条件
    [Serializable]
	[NodeMenuItem("技能条件/执行技能条件", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/执行技能条件", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/执行技能条件", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_RUN_SKILL_CONDITION : SkillConditionConfigNode
    {
		// 执行技能条件
		// 参数0 : 执行主体单位实例ID
		// 参数1 : 技能条件表格ID-SkillConditionConfig
        public TSCT_RUN_SKILL_CONDITION() : base(TSkillConditionType.TSCT_RUN_SKILL_CONDITION) { }
    }
    #endregion SkillConditionConfig: 执行技能条件


    #region SkillConditionConfig: 是否为城市战场地图
    [Serializable]
	[NodeMenuItem("技能条件/是否为城市战场地图", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否为城市战场地图", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否为城市战场地图", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MAP_STYLE_IS_CITY_ONE : SkillConditionConfigNode
    {
		// 是否为城市战场地图
        public TSCT_MAP_STYLE_IS_CITY_ONE() : base(TSkillConditionType.TSCT_MAP_STYLE_IS_CITY_ONE) { }
    }
    #endregion SkillConditionConfig: 是否为城市战场地图


    #region SkillConditionConfig: 战斗类型判断
    [Serializable]
	[NodeMenuItem("技能条件/战斗类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BATTLE_TYPE_CONDITION : SkillConditionConfigNode
    {
		// 战斗类型判断
		// 参数0 : 战斗类型-BattleType
        public TSCT_BATTLE_TYPE_CONDITION() : base(TSkillConditionType.TSCT_BATTLE_TYPE_CONDITION) { }
    }
    #endregion SkillConditionConfig: 战斗类型判断


    #region SkillConditionConfig: 单位技能品质判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/单位技能品质判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/单位技能品质判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/单位技能品质判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_RANKTYPE_CONDITION : SkillConditionConfigNode
    {
		// 单位技能品质判断
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能品质类型-RankType
        public TSCT_SKILL_RANKTYPE_CONDITION() : base(TSkillConditionType.TSCT_SKILL_RANKTYPE_CONDITION) { }
    }
    #endregion SkillConditionConfig: 单位技能品质判断


    #region SkillConditionConfig: 论剑-是否有可购买的推进卡牌
    [Serializable]
	[NodeMenuItem("技能条件/论剑-是否有可购买的推进卡牌", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/论剑-是否有可购买的推进卡牌", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/论剑-是否有可购买的推进卡牌", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_WENJIAN_HASPURCHASABLESUGGESTCARD : SkillConditionConfigNode
    {
		// 论剑-是否有可购买的推进卡牌
        public TSCT_WENJIAN_HASPURCHASABLESUGGESTCARD() : base(TSkillConditionType.TSCT_WENJIAN_HASPURCHASABLESUGGESTCARD) { }
    }
    #endregion SkillConditionConfig: 论剑-是否有可购买的推进卡牌


    #region SkillConditionConfig: 千层塔-是否拥有神物
    [Serializable]
	[NodeMenuItem("技能条件/千层塔/千层塔-是否拥有神物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-是否拥有神物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-是否拥有神物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CHAPTER_HAS_SKILL : SkillConditionConfigNode
    {
		// 千层塔-是否拥有神物
		// 参数0 : 神物对应技能ID
        public TSCT_CHAPTER_HAS_SKILL() : base(TSkillConditionType.TSCT_CHAPTER_HAS_SKILL) { }
    }
    #endregion SkillConditionConfig: 千层塔-是否拥有神物


    #region SkillConditionConfig: Buff类型判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/Buff类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/Buff类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/Buff类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BUFF_TYPE : SkillConditionConfigNode
    {
		// Buff类型判断
		// 参数0 : BuffID
		// 参数1 : 比较类型-ConditionContainType
		// 参数2 : Buff类型-TBuffType
        public TSCT_BUFF_TYPE() : base(TSkillConditionType.TSCT_BUFF_TYPE) { }
    }
    #endregion SkillConditionConfig: Buff类型判断


    #region SkillConditionConfig: 判断技能是否包含指定功能标签
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能是否包含指定功能标签", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定功能标签", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定功能标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_CONTAIN_FEATURE_LABEL : SkillConditionConfigNode
    {
		// 判断技能是否包含指定功能标签
		// 参数0 : 技能ID
		// 参数1 : 指定功能标签-TSkillFeatureLabel
        public TSCT_IS_SKILL_CONTAIN_FEATURE_LABEL() : base(TSkillConditionType.TSCT_IS_SKILL_CONTAIN_FEATURE_LABEL) { }
    }
    #endregion SkillConditionConfig: 判断技能是否包含指定功能标签


    #region SkillConditionConfig: 千层塔-是否还有下一关
    [Serializable]
	[NodeMenuItem("技能条件/千层塔/千层塔-是否还有下一关", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-是否还有下一关", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-是否还有下一关", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CHAPTER_HAS_NEXT_LEVEL : SkillConditionConfigNode
    {
		// 千层塔-是否还有下一关
        public TSCT_CHAPTER_HAS_NEXT_LEVEL() : base(TSkillConditionType.TSCT_CHAPTER_HAS_NEXT_LEVEL) { }
    }
    #endregion SkillConditionConfig: 千层塔-是否还有下一关


    #region SkillConditionConfig: 是否存在五行弱点(被克制)
    [Serializable]
	[NodeMenuItem("技能条件/单位/是否存在五行弱点(被克制)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否存在五行弱点(被克制)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否存在五行弱点(被克制)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_ELEMENT_WEEKNESS : SkillConditionConfigNode
    {
		// 是否存在五行弱点(被克制)
		// 参数0 : 目标单位实例ID
		// 参数1 : 五行类型-TElementsType
        public TSCT_IS_ELEMENT_WEEKNESS() : base(TSkillConditionType.TSCT_IS_ELEMENT_WEEKNESS) { }
    }
    #endregion SkillConditionConfig: 是否存在五行弱点(被克制)


    #region SkillConditionConfig: 玩家是否为机器人
    [Serializable]
	[NodeMenuItem("技能条件/单位/玩家是否为机器人", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/玩家是否为机器人", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/玩家是否为机器人", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_PLAYER_ROBOT_AI : SkillConditionConfigNode
    {
		// 玩家是否为机器人
		// 参数0 : 单位实例ID
        public TSCT_IS_PLAYER_ROBOT_AI() : base(TSkillConditionType.TSCT_IS_PLAYER_ROBOT_AI) { }
    }
    #endregion SkillConditionConfig: 玩家是否为机器人


    #region SkillConditionConfig: 判断技能是否包含指定技能AI标签
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能是否包含指定技能AI标签", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定技能AI标签", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定技能AI标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG : SkillConditionConfigNode
    {
		// 判断技能是否包含指定技能AI标签
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽位-TSkillSlotType
		// 参数2 : 技能AI标签-TBattleSkillAITag
        public TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG() : base(TSkillConditionType.TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG) { }
    }
    #endregion SkillConditionConfig: 判断技能是否包含指定技能AI标签


    #region SkillConditionConfig: 是否可被抓捕
    [Serializable]
	[NodeMenuItem("技能条件/单位/是否可被抓捕", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否可被抓捕", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否可被抓捕", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_CAN_CAPTURED : SkillConditionConfigNode
    {
		// 是否可被抓捕
		// 参数0 : 抓捕单位实例ID
		// 参数1 : 被抓捕单位实例ID
        public TSCT_IS_CAN_CAPTURED() : base(TSkillConditionType.TSCT_IS_CAN_CAPTURED) { }
    }
    #endregion SkillConditionConfig: 是否可被抓捕


    #region SkillConditionConfig: 是否不在筛选冷却中
    [Serializable]
	[NodeMenuItem("技能条件/单位/是否不在筛选冷却中", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否不在筛选冷却中", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/是否不在筛选冷却中", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_CAN_SELECT_NOT_CD : SkillConditionConfigNode
    {
		// 是否不在筛选冷却中
		// 参数0 : 筛选单位ID
		// 参数1 : 被设置冷却单位ID
		// 参数2 : 筛选表格ID-SkillSelectConfig
		// 参数3 : 是否检查筛选单位存在（1:检查）
        public TSCT_IS_CAN_SELECT_NOT_CD() : base(TSkillConditionType.TSCT_IS_CAN_SELECT_NOT_CD) { }
    }
    #endregion SkillConditionConfig: 是否不在筛选冷却中


    #region SkillConditionConfig: 是否属于主五行
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否属于主五行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否属于主五行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否属于主五行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_MAIN_FIVE_ELEMENT : SkillConditionConfigNode
    {
		// 是否属于主五行
		// 参数0 : 五行类型-TElementsType
        public TSCT_IS_MAIN_FIVE_ELEMENT() : base(TSkillConditionType.TSCT_IS_MAIN_FIVE_ELEMENT) { }
    }
    #endregion SkillConditionConfig: 是否属于主五行


    #region SkillConditionConfig: 五行判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/五行判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/五行判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/五行判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_ELEMENT : SkillConditionConfigNode
    {
		// 五行判断
		// 参数0 : 五行类型A-TElementsType
		// 参数1 : 五行类型B-TElementsType
        public TSCT_IS_ELEMENT() : base(TSkillConditionType.TSCT_IS_ELEMENT) { }
    }
    #endregion SkillConditionConfig: 五行判断


    #region SkillConditionConfig: 千层塔-章节关卡类型判断
    [Serializable]
	[NodeMenuItem("技能条件/千层塔/千层塔-章节关卡类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-章节关卡类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/千层塔/千层塔-章节关卡类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CHAPTER_CHAPTER_TYPE : SkillConditionConfigNode
    {
		// 千层塔-章节关卡类型判断
		// 参数0 : 比较类型-TConditionOperator
		// 参数1 : 章节关卡类型-TBattleChapterType
        public TSCT_CHAPTER_CHAPTER_TYPE() : base(TSkillConditionType.TSCT_CHAPTER_CHAPTER_TYPE) { }
    }
    #endregion SkillConditionConfig: 千层塔-章节关卡类型判断


    #region SkillConditionConfig: 是否是联网战斗
    [Serializable]
	[NodeMenuItem("技能条件/是否是联网战斗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否是联网战斗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否是联网战斗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_ONLINE_BATTLE : SkillConditionConfigNode
    {
		// 是否是联网战斗
        public TSCT_IS_ONLINE_BATTLE() : base(TSkillConditionType.TSCT_IS_ONLINE_BATTLE) { }
    }
    #endregion SkillConditionConfig: 是否是联网战斗


    #region SkillConditionConfig: 射线检测
    [Serializable]
	[NodeMenuItem("技能条件/技能/射线检测", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/射线检测", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/射线检测", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_RAY_CAST : SkillConditionConfigNode
    {
		// 射线检测
		// 参数0 : 单位实例ID
		// 参数1 : 结束点_X
		// 参数2 : 结束点_Y
		// 参数3 : 碰撞层级
		// 参数4 : 碰撞个数检查
        public TSCT_RAY_CAST() : base(TSkillConditionType.TSCT_RAY_CAST) { }
    }
    #endregion SkillConditionConfig: 射线检测


    #region SkillConditionConfig: 是否在碰撞盒内
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否在碰撞盒内", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否在碰撞盒内", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否在碰撞盒内", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_IN_COLLISION : SkillConditionConfigNode
    {
		// 是否在碰撞盒内
		// 参数0 : 检测点_X
		// 参数1 : 检测点_Y
		// 参数2 : 检测点碰撞层级-TCollisionLayer
		// 参数3 : 碰撞盒碰撞层级
        public TSCT_IS_IN_COLLISION() : base(TSkillConditionType.TSCT_IS_IN_COLLISION) { }
    }
    #endregion SkillConditionConfig: 是否在碰撞盒内


    #region SkillConditionConfig: 塔科夫-判断单位中是否有道具
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-判断单位中是否有道具", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-判断单位中是否有道具", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-判断单位中是否有道具", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_TARKOV_HAS_ITEM : SkillConditionConfigNode
    {
		// 塔科夫-判断单位中是否有道具
		// 参数0 : 交互实例ID
        public TSCT_TARKOV_HAS_ITEM() : base(TSkillConditionType.TSCT_TARKOV_HAS_ITEM) { }
    }
    #endregion SkillConditionConfig: 塔科夫-判断单位中是否有道具


    #region SkillConditionConfig: 塔科夫-传送点是否可用
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-传送点是否可用", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-传送点是否可用", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-传送点是否可用", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_TARKOV_TELEPORT_ENABLE : SkillConditionConfigNode
    {
		// 塔科夫-传送点是否可用
        public TSCT_TARKOV_TELEPORT_ENABLE() : base(TSkillConditionType.TSCT_TARKOV_TELEPORT_ENABLE) { }
    }
    #endregion SkillConditionConfig: 塔科夫-传送点是否可用


    #region SkillConditionConfig: 技能是否勾选吟唱反制标签
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能是否勾选吟唱反制标签", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能是否勾选吟唱反制标签", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能是否勾选吟唱反制标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_CHANT_COUNTER_TAG : SkillConditionConfigNode
    {
		// 技能是否勾选吟唱反制标签
        public TSCT_IS_SKILL_CHANT_COUNTER_TAG() : base(TSkillConditionType.TSCT_IS_SKILL_CHANT_COUNTER_TAG) { }
    }
    #endregion SkillConditionConfig: 技能是否勾选吟唱反制标签


    #region SkillConditionConfig: 判断本次技能是否已标记反制成功
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断本次技能是否已标记反制成功", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断本次技能是否已标记反制成功", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断本次技能是否已标记反制成功", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_HURT_SKILL_CHANT_COUNTER_MARKED : SkillConditionConfigNode
    {
		// 判断本次技能是否已标记反制成功
		// 参数0 : 受击者单位实例ID
		// 参数1 : 攻击者实例ID
		// 参数2 : 伤害来源技能ID
		// 参数3 : 伤害来源技能使用开始时间(帧)
        public TSCT_IS_HURT_SKILL_CHANT_COUNTER_MARKED() : base(TSkillConditionType.TSCT_IS_HURT_SKILL_CHANT_COUNTER_MARKED) { }
    }
    #endregion SkillConditionConfig: 判断本次技能是否已标记反制成功


    #region SkillConditionConfig: 是否是单机战斗
    [Serializable]
	[NodeMenuItem("技能条件/是否是单机战斗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否是单机战斗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/是否是单机战斗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SINGLE_BATTLE : SkillConditionConfigNode
    {
		// 是否是单机战斗
        public TSCT_IS_SINGLE_BATTLE() : base(TSkillConditionType.TSCT_IS_SINGLE_BATTLE) { }
    }
    #endregion SkillConditionConfig: 是否是单机战斗


    #region SkillConditionConfig: 判断技能资源类型
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能资源类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能资源类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能资源类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CHECK_SKILL_SCHOOL_RES_TYPE : SkillConditionConfigNode
    {
		// 判断技能资源类型
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能资源类型-TSkillSchoolResType
        public TSCT_CHECK_SKILL_SCHOOL_RES_TYPE() : base(TSkillConditionType.TSCT_CHECK_SKILL_SCHOOL_RES_TYPE) { }
    }
    #endregion SkillConditionConfig: 判断技能资源类型


    #region SkillConditionConfig: 判断技能是否包含指定BD核心标签
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能是否包含指定BD核心标签", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定BD核心标签", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定BD核心标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_CONTAIN_BD_FIRST_LABEL : SkillConditionConfigNode
    {
		// 判断技能是否包含指定BD核心标签
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能BD核心标签-TSkillBDFirstLabelType
        public TSCT_IS_SKILL_CONTAIN_BD_FIRST_LABEL() : base(TSkillConditionType.TSCT_IS_SKILL_CONTAIN_BD_FIRST_LABEL) { }
    }
    #endregion SkillConditionConfig: 判断技能是否包含指定BD核心标签


    #region SkillConditionConfig: 判断技能是否包含指定BD二级标签
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能是否包含指定BD二级标签", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定BD二级标签", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否包含指定BD二级标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_CONTAIN_BD_SECOND_LABEL : SkillConditionConfigNode
    {
		// 判断技能是否包含指定BD二级标签
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : BD二级标签-TSkillBDSecondLabelType
		// 参数3 : 是否包含对应的基础标签判断
        public TSCT_IS_SKILL_CONTAIN_BD_SECOND_LABEL() : base(TSkillConditionType.TSCT_IS_SKILL_CONTAIN_BD_SECOND_LABEL) { }
    }
    #endregion SkillConditionConfig: 判断技能是否包含指定BD二级标签


    #region SkillConditionConfig: 技能槽位类型判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能槽位类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能槽位类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能槽位类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_CHECK_SKILL_SLOT_TYPE : SkillConditionConfigNode
    {
		// 技能槽位类型判断
		// 参数0 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数1 : 单位实例ID（按表格ID获取时需要配置）
		// 参数2 : 比较类型-TConditionOperator
		// 参数3 : 槽位类型-TSkillSlotType
        public TSCT_CHECK_SKILL_SLOT_TYPE() : base(TSkillConditionType.TSCT_CHECK_SKILL_SLOT_TYPE) { }
    }
    #endregion SkillConditionConfig: 技能槽位类型判断


    #region SkillConditionConfig: 是否UI槽位技能
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否UI槽位技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否UI槽位技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否UI槽位技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_UI_SKILL : SkillConditionConfigNode
    {
		// 是否UI槽位技能
		// 参数0 : 槽位类型-TSkillSlotType
        public TSCT_IS_UI_SKILL() : base(TSkillConditionType.TSCT_IS_UI_SKILL) { }
    }
    #endregion SkillConditionConfig: 是否UI槽位技能


    #region SkillConditionConfig: 是否存在包含指定BD核心标签的技能
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否存在包含指定BD核心标签的技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在包含指定BD核心标签的技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在包含指定BD核心标签的技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_EXIST_SKILL_BD_FIRST_LABEL : SkillConditionConfigNode
    {
		// 是否存在包含指定BD核心标签的技能
		// 参数0 : 单位实例ID
		// 参数1 : 技能BD核心标签-TSkillBDFirstLabelType
        public TSCT_IS_EXIST_SKILL_BD_FIRST_LABEL() : base(TSkillConditionType.TSCT_IS_EXIST_SKILL_BD_FIRST_LABEL) { }
    }
    #endregion SkillConditionConfig: 是否存在包含指定BD核心标签的技能


    #region SkillConditionConfig: 是否存在包含指定BD二级标签的技能
    [Serializable]
	[NodeMenuItem("技能条件/技能/是否存在包含指定BD二级标签的技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在包含指定BD二级标签的技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/是否存在包含指定BD二级标签的技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_EXIST_SKILL_BD_SECOND_LABEL : SkillConditionConfigNode
    {
		// 是否存在包含指定BD二级标签的技能
		// 参数0 : 单位实例ID
		// 参数1 : BD二级标签-TSkillBDSecondLabelType
		// 参数2 : 是否包含对应的基础标签判断
        public TSCT_IS_EXIST_SKILL_BD_SECOND_LABEL() : base(TSkillConditionType.TSCT_IS_EXIST_SKILL_BD_SECOND_LABEL) { }
    }
    #endregion SkillConditionConfig: 是否存在包含指定BD二级标签的技能


    #region SkillConditionConfig: 是否触发概率
    [Serializable]
	[NodeMenuItem("通用配置/随机/是否触发概率", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/是否触发概率", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/是否触发概率", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_TRIGGER_PROBABILITY : SkillConditionConfigNode
    {
		// 是否触发概率
		// 参数0 : 概率（万分比）
        public TSCT_IS_TRIGGER_PROBABILITY() : base(TSkillConditionType.TSCT_IS_TRIGGER_PROBABILITY) { }
    }
    #endregion SkillConditionConfig: 是否触发概率


    #region SkillConditionConfig: 判断技能是否有效
    [Serializable]
	[NodeMenuItem("技能条件/技能/判断技能是否有效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否有效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/判断技能是否有效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_SKILL_ENABLE : SkillConditionConfigNode
    {
		// 判断技能是否有效
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID
        public TSCT_IS_SKILL_ENABLE() : base(TSkillConditionType.TSCT_IS_SKILL_ENABLE) { }
    }
    #endregion SkillConditionConfig: 判断技能是否有效


    #region SkillConditionConfig: Marker-是否可被交互
    [Serializable]
	[NodeMenuItem("技能条件/单位/Marker-是否可被交互", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否可被交互", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否可被交互", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MARKER_IS_CAN_INTERACTIVE : SkillConditionConfigNode
    {
		// Marker-是否可被交互
		// 参数0 : Marker单位实例ID
		// 参数1 : 交互者实例ID
        public TSCT_MARKER_IS_CAN_INTERACTIVE() : base(TSkillConditionType.TSCT_MARKER_IS_CAN_INTERACTIVE) { }
    }
    #endregion SkillConditionConfig: Marker-是否可被交互


    #region SkillConditionConfig: Marker-是否在交互中
    [Serializable]
	[NodeMenuItem("技能条件/单位/Marker-是否在交互中", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否在交互中", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否在交互中", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MARKER_IS_IN_INTERACTIVE_ING : SkillConditionConfigNode
    {
		// Marker-是否在交互中
		// 参数0 : Marker单位实例ID
		// 参数1 : 交互者实例ID
		// 参数2 : 仅判断当前交互者是否在于Marker交互
        public TSCT_MARKER_IS_IN_INTERACTIVE_ING() : base(TSkillConditionType.TSCT_MARKER_IS_IN_INTERACTIVE_ING) { }
    }
    #endregion SkillConditionConfig: Marker-是否在交互中


    #region SkillConditionConfig: 判断玩家是否在线
    [Serializable]
	[NodeMenuItem("技能条件/单位/判断玩家是否在线", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/判断玩家是否在线", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/判断玩家是否在线", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_PLAYER_CONNECTED : SkillConditionConfigNode
    {
		// 判断玩家是否在线
		// 参数0 : 玩家索引PlayerIndex
        public TSCT_IS_PLAYER_CONNECTED() : base(TSkillConditionType.TSCT_IS_PLAYER_CONNECTED) { }
    }
    #endregion SkillConditionConfig: 判断玩家是否在线


    #region SkillConditionConfig: 大秘境-是否正在房间传送投票中
    [Serializable]
	[NodeMenuItem("技能条件/大秘境玩法/大秘境-是否正在房间传送投票中", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/大秘境玩法/大秘境-是否正在房间传送投票中", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/大秘境玩法/大秘境-是否正在房间传送投票中", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BIG_DUNGEON_IS_IN_ROOM_TELEPORT_VOTE : SkillConditionConfigNode
    {
		// 大秘境-是否正在房间传送投票中
        public TSCT_BIG_DUNGEON_IS_IN_ROOM_TELEPORT_VOTE() : base(TSkillConditionType.TSCT_BIG_DUNGEON_IS_IN_ROOM_TELEPORT_VOTE) { }
    }
    #endregion SkillConditionConfig: 大秘境-是否正在房间传送投票中


    #region SkillConditionConfig: 是否战斗结束
    [Serializable]
	[NodeMenuItem("技能条件/战斗/是否战斗结束", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗/是否战斗结束", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗/是否战斗结束", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_IS_GAME_OVER : SkillConditionConfigNode
    {
		// 是否战斗结束
        public TSCT_IS_GAME_OVER() : base(TSkillConditionType.TSCT_IS_GAME_OVER) { }
    }
    #endregion SkillConditionConfig: 是否战斗结束


    #region SkillConditionConfig: Marker-是否包含参数
    [Serializable]
	[NodeMenuItem("技能条件/单位/Marker-是否包含参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否包含参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/单位/Marker-是否包含参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_MARKER_IS_PARAM : SkillConditionConfigNode
    {
		// Marker-是否包含参数
		// 参数0 : Marker单位实例ID
		// 参数1 : 参数类型-TBMarkParamType
		// 参数2 : 参数值
        public TSCT_MARKER_IS_PARAM() : base(TSkillConditionType.TSCT_MARKER_IS_PARAM) { }
    }
    #endregion SkillConditionConfig: Marker-是否包含参数


    #region SkillConditionConfig: 野外明雷战斗-种族和组类型的怪物剩余数量判断
    [Serializable]
	[NodeMenuItem("技能条件/战斗/野外明雷战斗-种族和组类型的怪物剩余数量判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗/野外明雷战斗-种族和组类型的怪物剩余数量判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/战斗/野外明雷战斗-种族和组类型的怪物剩余数量判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_BATTLE_WORLD_MONSTER_GROUP_LEFT_COUNT : SkillConditionConfigNode
    {
		// 野外明雷战斗-种族和组类型的怪物剩余数量判断
		// 参数0 : SelfMarkerID
		// 参数1 : SelfFromTriggerID
		// 参数2 : 种族和组类型-TBattleWorldMonsterGroupType
		// 参数3 : 运算符-TConditionOperator
		// 参数4 : 数量
        public TSCT_BATTLE_WORLD_MONSTER_GROUP_LEFT_COUNT() : base(TSkillConditionType.TSCT_BATTLE_WORLD_MONSTER_GROUP_LEFT_COUNT) { }
    }
    #endregion SkillConditionConfig: 野外明雷战斗-种族和组类型的怪物剩余数量判断


    #region SkillConditionConfig: 技能主类型判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能主类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能主类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能主类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_MAIN_TYPE : SkillConditionConfigNode
    {
		// 技能主类型判断
		// 参数0 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数1 : 单位实例ID（按实例ID获取时需要配置）
		// 参数2 : 比较类型-TConditionOperator
		// 参数3 : 技能主类型-TBattleSkillMainType
        public TSCT_SKILL_MAIN_TYPE() : base(TSkillConditionType.TSCT_SKILL_MAIN_TYPE) { }
    }
    #endregion SkillConditionConfig: 技能主类型判断


    #region SkillConditionConfig: 技能子类型判断
    [Serializable]
	[NodeMenuItem("技能条件/技能/技能子类型判断", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能子类型判断", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/技能子类型判断", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSCT_SKILL_SUB_TYPE : SkillConditionConfigNode
    {
		// 技能子类型判断
		// 参数0 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数1 : 单位实例ID（按实例ID获取时需要配置）
		// 参数2 : 比较类型-TConditionOperator
		// 参数3 : 技能子类型-TBattleSkillSubType
        public TSCT_SKILL_SUB_TYPE() : base(TSkillConditionType.TSCT_SKILL_SUB_TYPE) { }
    }
    #endregion SkillConditionConfig: 技能子类型判断


// TEMPLATE_CONTENT_END
}

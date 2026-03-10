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
    #region SkillSelectConfig: 圆形范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/圆形范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/圆形范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/圆形范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_CIRCLE : SkillSelectConfigNode
    {
		// 圆形范围筛选
		// 参数0 : 筛选半径
		// 参数1 : 位置偏移_右侧
		// 参数2 : 位置偏移_面前
		// 参数3 : 【AI参数】筛选圆心-BattleAITag
		// 参数4 : 【AI参数】原单位筛选新圆心半径
		// 参数5 : 【AI参数】筛选标签组-BattleAITagGroupConfig
		// 参数6 : 是否筛选最近单位
		// 参数7 : 角度
        public TSKILLSELECT_CIRCLE() : base(TSkillSelectType.TSKILLSELECT_CIRCLE) { }
    }
    #endregion SkillSelectConfig: 圆形范围筛选


    #region SkillSelectConfig: 矩形范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/矩形范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/矩形范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/矩形范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_RECT : SkillSelectConfigNode
    {
		// 矩形范围筛选
		// 参数0 : 矩形Y长
		// 参数1 : 矩形X长
		// 参数2 : 位置偏移_角度方向右侧
		// 参数3 : 位置偏移_角度方向面前
		// 参数4 : 角度
        public TSKILLSELECT_RECT() : base(TSkillSelectType.TSKILLSELECT_RECT) { }
    }
    #endregion SkillSelectConfig: 矩形范围筛选


    #region SkillSelectConfig: 扇形范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/扇形范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/扇形范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/扇形范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_SECTOR : SkillSelectConfigNode
    {
		// 扇形范围筛选
		// 参数0 : 半径
		// 参数1 : 扇形角度
		// 参数2 : 朝向
		// 参数3 : 位置偏移_右侧
		// 参数4 : 位置偏移_面前
        public TSKILLSELECT_SECTOR() : base(TSkillSelectType.TSKILLSELECT_SECTOR) { }
    }
    #endregion SkillSelectConfig: 扇形范围筛选


    #region SkillSelectConfig: 射线段范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/射线段范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/射线段范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/射线段范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_RAYCAST : SkillSelectConfigNode
    {
		// 射线段范围筛选
        public TSKILLSELECT_RAYCAST() : base(TSkillSelectType.TSKILLSELECT_RAYCAST) { }
    }
    #endregion SkillSelectConfig: 射线段范围筛选


    #region SkillSelectConfig: 最近单位范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/最近单位范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/最近单位范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/最近单位范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_NEAREST : SkillSelectConfigNode
    {
		// 最近单位范围筛选
		// 参数0 : 筛选半径
        public TSKILLSELECT_NEAREST() : base(TSkillSelectType.TSKILLSELECT_NEAREST) { }
    }
    #endregion SkillSelectConfig: 最近单位范围筛选


    #region SkillSelectConfig: 椭圆范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/椭圆范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/椭圆范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/椭圆范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_ELLIPSE : SkillSelectConfigNode
    {
		// 椭圆范围筛选
		// 参数0 : 筛选矩形长
		// 参数1 : 筛选矩形宽
		// 参数2 : 位置偏移X
		// 参数3 : 位置偏移Y
        public TSKILLSELECT_ELLIPSE() : base(TSkillSelectType.TSKILLSELECT_ELLIPSE) { }
    }
    #endregion SkillSelectConfig: 椭圆范围筛选


    #region SkillSelectConfig: 特定对象筛选
    [Serializable]
	[NodeMenuItem("技能筛选/特定对象筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/特定对象筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/特定对象筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_ENTITY_ID : SkillSelectConfigNode
    {
		// 特定对象筛选
		// 参数0 : 单位实例ID
        public TSKILLSELECT_ENTITY_ID() : base(TSkillSelectType.TSKILLSELECT_ENTITY_ID) { }
    }
    #endregion SkillSelectConfig: 特定对象筛选


    #region SkillSelectConfig: 单位原型ID筛选
    [Serializable]
	[NodeMenuItem("技能筛选/单位原型ID筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/单位原型ID筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/单位原型ID筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_ENTITY_CONFIG_ID : SkillSelectConfigNode
    {
		// 单位原型ID筛选
		// 参数0 : 原型ID
		// 参数1 : 筛选半径
        public TSKILLSELECT_ENTITY_CONFIG_ID() : base(TSkillSelectType.TSKILLSELECT_ENTITY_CONFIG_ID) { }
    }
    #endregion SkillSelectConfig: 单位原型ID筛选


    #region SkillSelectConfig: 随机单位范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/随机单位范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/随机单位范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/随机单位范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_RANDOM_ENTITY : SkillSelectConfigNode
    {
		// 随机单位范围筛选
		// 参数0 : 最大筛选范围
		// 参数1 : 筛选个数
		// 参数2 : 目标不足时填补策略-TSkillSelectFillStrategyType
		// 参数3 : 扇形角度[填0为圆形范围]
		// 参数4 : 朝向角度
		// 参数5 : 最小筛选范围(环形)
        public TSKILLSELECT_RANDOM_ENTITY() : base(TSkillSelectType.TSKILLSELECT_RANDOM_ENTITY) { }
    }
    #endregion SkillSelectConfig: 随机单位范围筛选


    #region SkillSelectConfig: 模板筛选
    [Serializable]
	[NodeMenuItem("技能筛选/模板筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/模板筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/模板筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_CUSTOM : SkillSelectConfigNode
    {
		// 模板筛选
		// 参数0 : 主体单位实例ID
		// 参数1 : 技能筛选数据ID-SkillSelectConfig
        public TSKILLSELECT_CUSTOM() : base(TSkillSelectType.TSKILLSELECT_CUSTOM) { }
    }
    #endregion SkillSelectConfig: 模板筛选


    #region SkillSelectConfig: 条件筛选
    [Serializable]
	[NodeMenuItem("技能筛选/条件筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/条件筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/条件筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_CONDITION_SELECT : SkillSelectConfigNode
    {
		// 条件筛选
		// 参数0 : 条件ID-SkillConditionConfig
		// 参数1 : 条件满足执行筛选ID-SkillSelectConfig
		// 参数2 : 条件不满足执行筛选ID-SkillSelectConfig
        public TSKILLSELECT_CONDITION_SELECT() : base(TSkillSelectType.TSKILLSELECT_CONDITION_SELECT) { }
    }
    #endregion SkillSelectConfig: 条件筛选


    #region SkillSelectConfig: 并行筛选
    [Serializable]
	[NodeMenuItem("技能筛选/并行筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/并行筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/并行筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_PARALLEL_SELECT : SkillSelectConfigNode
    {
		// 并行筛选
		// 参数0 : 筛选ID-SkillSelectConfig
        public TSKILLSELECT_PARALLEL_SELECT() : base(TSkillSelectType.TSKILLSELECT_PARALLEL_SELECT) { }
    }
    #endregion SkillSelectConfig: 并行筛选


    #region SkillSelectConfig: 环形范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/环形范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/环形范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/环形范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_RING : SkillSelectConfigNode
    {
		// 环形范围筛选
		// 参数0 : 内环半径
		// 参数1 : 环宽度
		// 参数2 : 位置偏移_右侧
		// 参数3 : 位置偏移_面前
		// 参数4 : 角度
        public TSKILLSELECT_RING() : base(TSkillSelectType.TSKILLSELECT_RING) { }
    }
    #endregion SkillSelectConfig: 环形范围筛选


    #region SkillSelectConfig: 全地图范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/全地图范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/全地图范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/全地图范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_MAP : SkillSelectConfigNode
    {
		// 全地图范围筛选
		// 参数0 : 是否排除主单位
        public TSKILLSELECT_MAP() : base(TSkillSelectType.TSKILLSELECT_MAP) { }
    }
    #endregion SkillSelectConfig: 全地图范围筛选


    #region SkillSelectConfig: 仇恨值范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/仇恨值范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/仇恨值范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/仇恨值范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_HATEVALUE : SkillSelectConfigNode
    {
		// 仇恨值范围筛选
		// 参数0 : 筛选半径
		// 参数1 : 仇恨值相同随机
		// 参数2 : 仇恨值增加逻辑最大生效范围
        public TSKILLSELECT_HATEVALUE() : base(TSkillSelectType.TSKILLSELECT_HATEVALUE) { }
    }
    #endregion SkillSelectConfig: 仇恨值范围筛选


    #region SkillSelectConfig: 固定位置圆形范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/固定位置圆形范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/固定位置圆形范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/固定位置圆形范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_CIRCLE_ABSPOS : SkillSelectConfigNode
    {
		// 固定位置圆形范围筛选
		// 参数0 : 筛选半径
		// 参数1 : 绝对位置X
		// 参数2 : 绝对位置Y
		// 参数3 : 是否筛选最近单位
        public TSKILLSELECT_CIRCLE_ABSPOS() : base(TSkillSelectType.TSKILLSELECT_CIRCLE_ABSPOS) { }
    }
    #endregion SkillSelectConfig: 固定位置圆形范围筛选


    #region SkillSelectConfig: 执行技能筛选
    [Serializable]
	[NodeMenuItem("技能筛选/执行技能筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/执行技能筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/执行技能筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_RUN_SKILL_SELECT : SkillSelectConfigNode
    {
		// 执行技能筛选
		// 参数0 : 执行主体单位实例ID
		// 参数1 : 技能筛选表格ID-SkillSelectConfig
        public TSKILLSELECT_RUN_SKILL_SELECT() : base(TSkillSelectType.TSKILLSELECT_RUN_SKILL_SELECT) { }
    }
    #endregion SkillSelectConfig: 执行技能筛选


    #region SkillSelectConfig: 技能单位组_筛选
    [Serializable]
	[NodeMenuItem("技能筛选/技能单位组_筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/技能单位组_筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/技能单位组_筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_ENTITY_GROUP : SkillSelectConfigNode
    {
		// 技能单位组_筛选
		// 参数0 : 目标单位ID
		// 参数1 : 技能ID
		// 参数2 : 单位组类型-TSkillEntityGroupType
		// 参数3 : 是否整组选择(1:是；0:否<选血量最低>)
		// 参数4 : 最大筛选数量(0:不做限制)
		// 参数5 : 是否随机筛选单位(0:否,1:是)
        public TSKILLSELECT_ENTITY_GROUP() : base(TSkillSelectType.TSKILLSELECT_ENTITY_GROUP) { }
    }
    #endregion SkillSelectConfig: 技能单位组_筛选


    #region SkillSelectConfig: 以自身Fixture的筛选层范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/以自身Fixture的筛选层范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/以自身Fixture的筛选层范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/以自身Fixture的筛选层范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_SELF_FILTER_LAYER_RANGE : SkillSelectConfigNode
    {
		// 以自身Fixture的筛选层范围筛选
        public TSKILLSELECT_SELF_FILTER_LAYER_RANGE() : base(TSkillSelectType.TSKILLSELECT_SELF_FILTER_LAYER_RANGE) { }
    }
    #endregion SkillSelectConfig: 以自身Fixture的筛选层范围筛选


    #region SkillSelectConfig: 最远单位范围筛选
    [Serializable]
	[NodeMenuItem("技能筛选/最远单位范围筛选", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/最远单位范围筛选", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能筛选/最远单位范围筛选", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSKILLSELECT_FARTHEST : SkillSelectConfigNode
    {
		// 最远单位范围筛选
		// 参数0 : 筛选半径
        public TSKILLSELECT_FARTHEST() : base(TSkillSelectType.TSKILLSELECT_FARTHEST) { }
    }
    #endregion SkillSelectConfig: 最远单位范围筛选


// TEMPLATE_CONTENT_END
}

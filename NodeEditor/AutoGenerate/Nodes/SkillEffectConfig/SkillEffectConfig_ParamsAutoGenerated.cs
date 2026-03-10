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
    #region SkillEffectConfig: 顺序执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/顺序执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/顺序执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/顺序执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ORDER_EXECUTE : SkillEffectConfigNode
    {
		// 顺序执行
		// 参数0 : 技能效果ID-SkillEffectConfig
        public TSET_ORDER_EXECUTE() : base(TSkillEffectType.TSET_ORDER_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 顺序执行


    #region SkillEffectConfig: 延迟执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/延迟执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/延迟执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/延迟执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_DELAY_EXECUTE : SkillEffectConfigNode
    {
		// 延迟执行
		// 参数0 : 延迟帧数
		// 参数1 : 技能效果ID-SkillEffectConfig
		// 参数2 : 单位死亡后是否继续执行
		// 参数3 : 筛选ID_不填写则使用原始目标-SkillSelectConfig
		// 参数4 : 中断配置-SkillInterruptConfig
		// 参数5 : 占位参数
		// 参数6 : 是否受急速属性影响(减少延迟帧数)
        public TSET_DELAY_EXECUTE() : base(TSkillEffectType.TSET_DELAY_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 延迟执行


    #region SkillEffectConfig: 重复执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/重复执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/重复执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/重复执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REPEAT_EXECUTE : SkillEffectConfigNode
    {
		// 重复执行
		// 参数0 : 间隔帧数
		// 参数1 : 执行次数
		// 参数2 : 是否立刻执行
		// 参数3 : 技能效果ID-SkillEffectConfig
		// 参数4 : 筛选ID_不填写则使用原始目标-SkillSelectConfig
		// 参数5 : 中断配置-SkillInterruptConfig
		// 参数6 : 单位死亡后是否继续执行
		// 参数7 : 结束时技能效果-SkillEffectConfig
		// 参数8 : 战斗结束后是否继续执行
		// 参数9 : 是否受急速属性影响(减少间隔帧数)
        public TSET_REPEAT_EXECUTE() : base(TSkillEffectType.TSET_REPEAT_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 重复执行


    #region SkillEffectConfig: 添加buff
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/添加buff", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/添加buff", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/添加buff", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BUFF : SkillEffectConfigNode
    {
		// 添加buff
		// 参数0 : 挂载Buff目标单位
		// 参数1 : Buff来源单位
		// 参数2 : BuffID-BuffConfig
		// 参数3 : 持续时间(帧数)
		// 参数4 : 间隔时间(帧数)
		// 参数5 : 添加层数(填0=1层)
		// 参数6 : 是否受急速属性影响(减少间隔帧数)
        public TSET_ADD_BUFF() : base(TSkillEffectType.TSET_ADD_BUFF) { }
    }
    #endregion SkillEffectConfig: 添加buff


    #region SkillEffectConfig: 移除buff
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/移除buff", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/移除buff", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/移除buff", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REMOVE_BUFF : SkillEffectConfigNode
    {
		// 移除buff
		// 参数0 : 挂载Buff单位
		// 参数1 : 移除方式-TSkillEffectBuffRemoveType
		// 参数2 : 参数1
		// 参数3 : 参数2
		// 参数4 : 参数3
		// 参数5 : 参数4
        public TSET_REMOVE_BUFF() : base(TSkillEffectType.TSET_REMOVE_BUFF) { }
    }
    #endregion SkillEffectConfig: 移除buff


    #region SkillEffectConfig: 快速伤害结算
    [Serializable]
	[NodeMenuItem("技能效果/技能/快速伤害结算", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/快速伤害结算", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/快速伤害结算", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_QUICK_DAMAGE : SkillEffectConfigNode
    {
		// 快速伤害结算
		// 参数0 : 伤害值
		// 参数1 : 伤害五行类型-TElementsType
		// 参数2 : 是否暴击
		// 参数3 : 禁止伤害冒字显示
		// 参数4 : 是否化解
		// 参数5 : 伤害类型-TSkillDamageType
		// 参数6 : 是否闪避
		// 参数7 : 死亡击飞力道值配置-BattleHitFlyForceConfig
		// 参数8 : 是否无视护盾
		// 参数9 : 伤害子类型位组Flags-TSkillDamageSubType
        public TSET_QUICK_DAMAGE() : base(TSkillEffectType.TSET_QUICK_DAMAGE) { }
    }
    #endregion SkillEffectConfig: 快速伤害结算


    #region SkillEffectConfig: 标准伤害结算
    [Serializable]
	[NodeMenuItem("技能效果/技能/标准伤害结算", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标准伤害结算", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标准伤害结算", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_DAMAGE : SkillEffectConfigNode
    {
		// 标准伤害结算
		// 参数0 : 伤害来源单位实例ID
		// 参数1 : 伤害目标单位实例ID
		// 参数2 : 伤害值
		// 参数3 : 伤害五行类型-TElementsType
		// 参数4 : 是否暴击
		// 参数5 : 禁止伤害冒字显示
		// 参数6 : 是否化解
		// 参数7 : 伤害类型-TSkillDamageType
		// 参数8 : 是否闪避
		// 参数9 : 死亡击飞力道值配置-BattleHitFlyForceConfig
		// 参数10 : 是否无视护盾
		// 参数11 : 伤害子类型位组Flags-TSkillDamageSubType
		// 参数12 : 吟唱反制打断值
        public TSET_DAMAGE() : base(TSkillEffectType.TSET_DAMAGE) { }
    }
    #endregion SkillEffectConfig: 标准伤害结算


    #region SkillEffectConfig: 创建子弹
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/创建子弹", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建子弹", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建子弹", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_BULLET : SkillEffectConfigNode
    {
		// 创建子弹
		// 参数0 : 子弹ID-BulletConfig
		// 参数1 : 角度
		// 参数2 : 位置X
		// 参数3 : 位置Y
		// 参数4 : 创建者单位
		// 参数5 : 位置偏移_右侧
		// 参数6 : 位置偏移_面前
		// 参数7 : 是否子子弹（1:是|0:否）
		// 参数8 : 加入单位组：-TSkillEntityGroupType
		// 参数9 : 是否为射弹类子弹（1:是|0:否）
		// 参数10 : 初始技能实例ID(伤害参数)
		// 参数11 : 自定义模型ID-ModelConfig
		// 参数12 : 高度Z
		// 参数13 : 初始仰角
		// 参数14 : 是否受急速属性影响(加快飞行速度)
        public TSET_CREATE_BULLET() : base(TSkillEffectType.TSET_CREATE_BULLET) { }
    }
    #endregion SkillEffectConfig: 创建子弹


    #region SkillEffectConfig: 播放角色动作
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/播放角色动作", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放角色动作", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放角色动作", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PLAY_ROLE_ANIM : SkillEffectConfigNode
    {
		// 播放角色动作
		// 参数0 : 动作播放单位
		// 参数1 : 动作枚举-TRoleAnimType
		// 参数2 : 是否上半身动画（0_1）
		// 参数3 : 融合时间_毫秒
		// 参数4 : 动画速度百分比
		// 参数5 : 是否被移动打断
		// 参数6 : 是否受急速属性影响(加快动画速度)
		// 参数7 : 仅指定单位可见
		// 参数8 : 动画播放时间-帧（反推播放速度）
		// 参数9 : 动作绑定音效单位实例ID
        public TSET_PLAY_ROLE_ANIM() : base(TSkillEffectType.TSET_PLAY_ROLE_ANIM) { }
    }
    #endregion SkillEffectConfig: 播放角色动作


    #region SkillEffectConfig: 播放特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/播放特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_EFFECT : SkillEffectConfigNode
    {
		// 播放特效
		// 参数0 : 特效ID-ModelConfig
		// 参数1 : 角度
		// 参数2 : 位置X
		// 参数3 : 位置Y
		// 参数4 : 持续时间[帧数]
		// 参数5 : 特效跟随单位
		// 参数6 : 缩放百分比
		// 参数7 : 延迟销毁时间[毫秒][填0为默认延迟]
		// 参数8 : 位置偏移_右侧
		// 参数9 : 位置偏移_面前
		// 参数10 : 加入单位组：-TSkillEntityGroupType
		// 参数11 : 是否为纯特效[非单位化]
		// 参数12 : 高度Z
		// 参数13 : 特效类型-TViewEffectType
		// 参数14 : 播放速度-百分比
		// 参数15 : 出生后额外执行效果-SkillEffectConfig
		// 参数16 : 是否受急速属性影响(加快特效播放速度)
        public TSET_CREATE_EFFECT() : base(TSkillEffectType.TSET_CREATE_EFFECT) { }
    }
    #endregion SkillEffectConfig: 播放特效


    #region SkillEffectConfig: 修改单位属性
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位属性", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位属性", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位属性", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MODIFY_ENTITY_ATTR_VALUE : SkillEffectConfigNode
    {
		// 修改单位属性
		// 参数0 : 被修改属性单位
		// 参数1 : 属性枚举值-TBattleNatureEnum
		// 参数2 : 属性值
        public TSET_MODIFY_ENTITY_ATTR_VALUE() : base(TSkillEffectType.TSET_MODIFY_ENTITY_ATTR_VALUE) { }
    }
    #endregion SkillEffectConfig: 修改单位属性


    #region SkillEffectConfig: 预警圈
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/预警圈", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/预警圈", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/预警圈", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_WARNING_CIRCLE : SkillEffectConfigNode
    {
		// 预警圈
		// 参数0 : 模型ID-ModelConfig
		// 参数1 : z轴
		// 参数2 : X轴
		// 参数3 : 变化曲线id(无效)
		// 参数4 : 变化延迟时间(无效)
		// 参数5 : 变化总时间
		// 参数6 : 是否跟随目标朝向
		// 参数7 : 加入单位组：-TSkillEntityGroupType
		// 参数8 : 高度Z是否跟随
		// 参数9 : 位置偏移_右侧
		// 参数10 : 位置偏移_面前
        public TSET_CREATE_WARNING_CIRCLE() : base(TSkillEffectType.TSET_CREATE_WARNING_CIRCLE) { }
    }
    #endregion SkillEffectConfig: 预警圈


    #region SkillEffectConfig: 修改单位状态
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MODIFY_ENTITY_STATE : SkillEffectConfigNode
    {
		// 修改单位状态
		// 参数0 : 被修改状态单位
		// 参数1 : 状态枚举值-TEntityState
		// 参数2 : 目标状态值
        public TSET_MODIFY_ENTITY_STATE() : base(TSkillEffectType.TSET_MODIFY_ENTITY_STATE) { }
    }
    #endregion SkillEffectConfig: 修改单位状态


    #region SkillEffectConfig: 召唤角色
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/召唤角色", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤角色", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤角色", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_ROLE : SkillEffectConfigNode
    {
		// 召唤角色
		// 参数0 : 召唤的战斗单位ID-BattleUnitConfig
		// 参数1 : 角度
		// 参数2 : 位置X
		// 参数3 : 位置Y
		// 参数4 : 阵营(默认创建者所属阵营)-TEntityCamp
		// 参数5 : 玩家索引(默认创建者所属玩家索引)
		// 参数6 : 是否可控
		// 参数7 : 位置偏移X
		// 参数8 : 位置偏移Y
		// 参数9 : 召唤位置限制-TBattlePositionClampType
		// 参数10 : 存活基础帧数-自动死亡[填0:不自动死亡]
		// 参数11 : 加入单位组：-TSkillEntityGroupType
		// 参数12 : 是否标志为召唤物
		// 参数13 : 创建者单位实例ID（填0设置为召唤物自己）
		// 参数14 : 自定义模型ID-ModelConfig
		// 参数15 : 出生前额外执行效果-SkillEffectConfig
		// 参数16 : 出生后额外执行效果-SkillEffectConfig
		// 参数17 : 战斗结束后是否继续执行
		// 参数18 : 单位次要类型-TEntitySubType
        public TSET_CREATE_ROLE() : base(TSkillEffectType.TSET_CREATE_ROLE) { }
    }
    #endregion SkillEffectConfig: 召唤角色


    #region SkillEffectConfig: 镜头抖动
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头抖动", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头抖动", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头抖动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERA_SHAKE : SkillEffectConfigNode
    {
		// 镜头抖动
		// 参数0 : 镜头抖动执行单位
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 起效半径
		// 参数3 : 抖动坐标-X
		// 参数4 : 抖动坐标-Y
		// 参数5 : 镜头震动ID-BattleCameraShakeConfig
		// 参数6 : 优先级-TCameraPriority
		// 参数7 : 优先级插入方式-TPriorityInsertType
        public TSET_CAMERA_SHAKE() : base(TSkillEffectType.TSET_CAMERA_SHAKE) { }
    }
    #endregion SkillEffectConfig: 镜头抖动


    #region SkillEffectConfig: 镜头深度变化(废弃)
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头深度变化(废弃)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头深度变化(废弃)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头深度变化(废弃)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERA_DEPTH_CHANGE : SkillEffectConfigNode
    {
		// 镜头深度变化(废弃)
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 优先级-TCameraPriority
		// 参数3 : 优先级插入方式-TPriorityInsertType
		// 参数4 : 正交尺寸（cm）
		// 参数5 : 曲线ID
		// 参数6 : 移动时间（帧数）
		// 参数7 : 维持时间（帧数）
		// 参数8 : 是否切回
		// 参数9 : 切回时间（帧数）
		// 参数10 : 深度变化类型-TDepthChangeType
        public TSET_CAMERA_DEPTH_CHANGE() : base(TSkillEffectType.TSET_CAMERA_DEPTH_CHANGE) { }
    }
    #endregion SkillEffectConfig: 镜头深度变化(废弃)


    #region SkillEffectConfig: 游戏速度变化
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/游戏速度变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/游戏速度变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/游戏速度变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GAME_SPEED_CHANGE : SkillEffectConfigNode
    {
		// 游戏速度变化
		// 参数0 : 播放速度百分比
        public TSET_GAME_SPEED_CHANGE() : base(TSkillEffectType.TSET_GAME_SPEED_CHANGE) { }
    }
    #endregion SkillEffectConfig: 游戏速度变化


    #region SkillEffectConfig: 应用屏幕特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/应用屏幕特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/应用屏幕特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/应用屏幕特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_APPLY_SCREEN_EFFECT : SkillEffectConfigNode
    {
		// 应用屏幕特效
		// 参数0 : 屏幕特效应用单位
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 屏幕特效枚举-TScreenEffectType
		// 参数3 : 持续时间-帧
		// 参数4 : 效果开关类型-TEffectLifeType
		// 参数5 : 进入结算自动关闭
		// 参数6 : 额外参数1
		// 参数7 : 额外参数2
		// 参数8 : 额外参数3
		// 参数9 : 额外参数4
        public TSET_APPLY_SCREEN_EFFECT() : base(TSkillEffectType.TSET_APPLY_SCREEN_EFFECT) { }
    }
    #endregion SkillEffectConfig: 应用屏幕特效


    #region SkillEffectConfig: 设置单位碰撞启用状态
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置单位碰撞启用状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位碰撞启用状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位碰撞启用状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_ENTITY_COLLISION_ENABLE_STATE : SkillEffectConfigNode
    {
		// 设置单位碰撞启用状态
		// 参数0 : 设置碰撞的目标单位
		// 参数1 : 目标碰撞层-TCollisionLayer
		// 参数2 : 启用状态（1开启，0关闭）
		// 参数3 : 启用碰撞类型（0全部启用）-TCollisionType
        public TSET_SET_ENTITY_COLLISION_ENABLE_STATE() : base(TSkillEffectType.TSET_SET_ENTITY_COLLISION_ENABLE_STATE) { }
    }
    #endregion SkillEffectConfig: 设置单位碰撞启用状态


    #region SkillEffectConfig: 应用单位特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/应用单位特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/应用单位特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/应用单位特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_APPLY_ENTITY_EFFECT : SkillEffectConfigNode
    {
		// 应用单位特效
		// 参数0 : 应用特效目标单位
		// 参数1 : 单位特效枚举-TEntityEffectType
		// 参数2 : 是否开启效果
		// 参数3 : 优先级（默认不必设置）
		// 参数4 : 是否覆盖效果（关闭其他）
		// 参数5 : 持续帧数
		// 参数6 : 额外参数1
		// 参数7 : 额外参数2
		// 参数8 : 额外参数3
		// 参数9 : 额外参数4
		// 参数10 : 额外参数5
		// 参数11 : 额外参数6
        public TSET_APPLY_ENTITY_EFFECT() : base(TSkillEffectType.TSET_APPLY_ENTITY_EFFECT) { }
    }
    #endregion SkillEffectConfig: 应用单位特效


    #region SkillEffectConfig: 修改单位位置
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位位置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位位置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位位置", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_POSITION : SkillEffectConfigNode
    {
		// 修改单位位置
		// 参数0 : 修改位置目标单位
		// 参数1 : 位置X
		// 参数2 : 位置Y
		// 参数3 : 位置限制-TBattlePositionClampType
		// 参数4 : 自定义LocationConfig-LocationConfig
        public TSET_CHANGE_ENTITY_POSITION() : base(TSkillEffectType.TSET_CHANGE_ENTITY_POSITION) { }
    }
    #endregion SkillEffectConfig: 修改单位位置


    #region SkillEffectConfig: 更换主控单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/更换主控单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/更换主控单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/更换主控单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_MAIN_CONTROL_ENTITY : SkillEffectConfigNode
    {
		// 更换主控单位
		// 参数0 : 原始主控单位
		// 参数1 : 目标主控单位
        public TSET_CHANGE_MAIN_CONTROL_ENTITY() : base(TSkillEffectType.TSET_CHANGE_MAIN_CONTROL_ENTITY) { }
    }
    #endregion SkillEffectConfig: 更换主控单位


    #region SkillEffectConfig: 销毁单位
    [Serializable]
	[NodeMenuItem("技能效果/单位/销毁单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/销毁单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/销毁单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_DESTROY_ENTITY : SkillEffectConfigNode
    {
		// 销毁单位
		// 参数0 : 目标销毁单位
		// 参数1 : 是否执行死亡流程
		// 参数2 : 真正的击杀者实例ID
        public TSET_DESTROY_ENTITY() : base(TSkillEffectType.TSET_DESTROY_ENTITY) { }
    }
    #endregion SkillEffectConfig: 销毁单位


    #region SkillEffectConfig: 设置界面显示状态
    [Serializable]
	[NodeMenuItem("技能效果/UI/设置界面显示状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/设置界面显示状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/设置界面显示状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_UI_NODE_VISIBLE : SkillEffectConfigNode
    {
		// 设置界面显示状态
		// 参数0 : 界面位组
		// 参数1 : 是否显示
        public TSET_SET_UI_NODE_VISIBLE() : base(TSkillEffectType.TSET_SET_UI_NODE_VISIBLE) { }
    }
    #endregion SkillEffectConfig: 设置界面显示状态


    #region SkillEffectConfig: 概率执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/概率执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/概率执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/概率执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROBABILITY_EXECUTE : SkillEffectConfigNode
    {
		// 概率执行
		// 参数0 : 概率（万分比）
		// 参数1 : 成功执行技能效果ID-SkillEffectConfig
		// 参数2 : 失败执行技能效果ID-SkillEffectConfig
        public TSET_PROBABILITY_EXECUTE() : base(TSkillEffectType.TSET_PROBABILITY_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 概率执行


    #region SkillEffectConfig: 使用技能
    [Serializable]
	[NodeMenuItem("技能效果/技能/使用技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/使用技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/使用技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_USE_SKILL : SkillEffectConfigNode
    {
		// 使用技能
		// 参数0 : 技能ID-SkillConfig
		// 参数1 : 主体单位实例ID
		// 参数2 : 技能槽-TSkillSlotType
		// 参数3 : 技能等级（<=0默认1级）
		// 参数4 : 技能按钮阶段-TSkillButtonStage
        public TSET_USE_SKILL() : base(TSkillEffectType.TSET_USE_SKILL) { }
    }
    #endregion SkillEffectConfig: 使用技能


    #region SkillEffectConfig: 生成随机数
    [Serializable]
	[NodeMenuItem("通用配置/随机/生成随机数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/生成随机数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/生成随机数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_RANDOM_NUM : SkillEffectConfigNode
    {
		// 生成随机数
		// 参数0 : 最小值(包含)
		// 参数1 : 最大值(不包含)
        public TSET_CREATE_RANDOM_NUM() : base(TSkillEffectType.TSET_CREATE_RANDOM_NUM) { }
    }
    #endregion SkillEffectConfig: 生成随机数


    #region SkillEffectConfig: 设置跟随目标
    [Serializable]
	[NodeMenuItem("技能效果/技能/设置跟随目标", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置跟随目标", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置跟随目标", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_FOLLOW_ENTITY : SkillEffectConfigNode
    {
		// 设置跟随目标
		// 参数0 : 跟随者主体单位
		// 参数1 : 被跟随目标单位
		// 参数2 : 朝向是否跟随改变
		// 参数3 : 位置是否跟随改变
		// 参数4 : 特殊跟随配置-BattleFollowConfig
		// 参数5 : 特殊跟随初始角度
		// 参数6 : 特殊跟随绝对角度变化
		// 参数7 : 是否同跟随者保持一致
        public TSET_FOLLOW_ENTITY() : base(TSkillEffectType.TSET_FOLLOW_ENTITY) { }
    }
    #endregion SkillEffectConfig: 设置跟随目标


    #region SkillEffectConfig: 数值运算
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/常规运算/数值运算", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_NUM_CALCULATE : SkillEffectConfigNode
    {
		// 数值运算
		// 参数0 : 数值
		// 参数1 : 运算符-TNumOperators
		// 参数2 : 数值
        public TSET_NUM_CALCULATE() : base(TSkillEffectType.TSET_NUM_CALCULATE) { }
    }
    #endregion SkillEffectConfig: 数值运算


    #region SkillEffectConfig: 获取单位属性
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位属性", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位属性", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位属性", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ATTR_VALUE : SkillEffectConfigNode
    {
		// 获取单位属性
		// 参数0 : 被获取属性的单位
		// 参数1 : 属性枚举值-TBattleNatureEnum
        public TSET_GET_ENTITY_ATTR_VALUE() : base(TSkillEffectType.TSET_GET_ENTITY_ATTR_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取单位属性


    #region SkillEffectConfig: 添加物理外力
    [Serializable]
	[NodeMenuItem("技能效果/技能/添加物理外力", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/添加物理外力", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/添加物理外力", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_FORCE : SkillEffectConfigNode
    {
		// 添加物理外力
		// 参数0 : 被添加外力的单位
		// 参数1 : 水平初速度[厘米_秒]
		// 参数2 : 水平减速度[厘米_秒]
		// 参数3 : 是否开启碰撞反弹(0无_1开启)
		// 参数4 : 水平方向[角度]
		// 参数5 : 外力持续总时间（毫秒）
		// 参数6 : 击飞起始高度（厘米）
		// 参数7 : 击飞最高高度（厘米）
		// 参数8 : 击飞到落地时间（毫秒）
		// 参数9 : 是否忽略速度和高度受体型击退倍率影响
		// 参数10 : 是否无视不动如山
        public TSET_ADD_FORCE() : base(TSkillEffectType.TSET_ADD_FORCE) { }
    }
    #endregion SkillEffectConfig: 添加物理外力


    #region SkillEffectConfig: 获取单位间方向
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取单位间方向", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位间方向", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位间方向", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ANGLE_BETWEEN_ENTITY : SkillEffectConfigNode
    {
		// 获取单位间方向
		// 参数0 : 起点单位
		// 参数1 : 终点单位
        public TSET_GET_ANGLE_BETWEEN_ENTITY() : base(TSkillEffectType.TSET_GET_ANGLE_BETWEEN_ENTITY) { }
    }
    #endregion SkillEffectConfig: 获取单位间方向


    #region SkillEffectConfig: 调整技能目标点
    [Serializable]
	[NodeMenuItem("技能效果/技能/调整技能目标点", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/调整技能目标点", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/调整技能目标点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADJUST_SKILL_TARGET_POS : SkillEffectConfigNode
    {
		// 调整技能目标点
        public TSET_ADJUST_SKILL_TARGET_POS() : base(TSkillEffectType.TSET_ADJUST_SKILL_TARGET_POS) { }
    }
    #endregion SkillEffectConfig: 调整技能目标点


    #region SkillEffectConfig: 获取主体单位ID
    [Serializable]
    public sealed partial class TSET_GET_MAIN_ENTITY_ID : SkillEffectConfigNode
    {
		// 获取主体单位ID
        public TSET_GET_MAIN_ENTITY_ID() : base(TSkillEffectType.TSET_GET_MAIN_ENTITY_ID) { }
    }
    #endregion SkillEffectConfig: 获取主体单位ID


    #region SkillEffectConfig: 获取目标单位ID
    [Serializable]
    public sealed partial class TSET_GET_TARGET_ENTITY_ID : SkillEffectConfigNode
    {
		// 获取目标单位ID
        public TSET_GET_TARGET_ENTITY_ID() : base(TSkillEffectType.TSET_GET_TARGET_ENTITY_ID) { }
    }
    #endregion SkillEffectConfig: 获取目标单位ID


    #region SkillEffectConfig: 获取数值最大值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/常规运算/获取数值最大值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/获取数值最大值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/获取数值最大值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MAX_VALUE : SkillEffectConfigNode
    {
		// 获取数值最大值
		// 参数0 : 数值
        public TSET_GET_MAX_VALUE() : base(TSkillEffectType.TSET_GET_MAX_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取数值最大值


    #region SkillEffectConfig: 获取数值最小值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/常规运算/获取数值最小值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/获取数值最小值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/获取数值最小值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MIN_VALUE : SkillEffectConfigNode
    {
		// 获取数值最小值
		// 参数0 : 数值
        public TSET_GET_MIN_VALUE() : base(TSkillEffectType.TSET_GET_MIN_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取数值最小值


    #region SkillEffectConfig: 获取五行强化值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取五行强化值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行强化值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行强化值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_ATK : SkillEffectConfigNode
    {
		// 获取五行强化值
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_ATK() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_ATK) { }
    }
    #endregion SkillEffectConfig: 获取五行强化值


    #region SkillEffectConfig: 获取五行抵抗值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取五行抵抗值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行抵抗值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行抵抗值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_DEF : SkillEffectConfigNode
    {
		// 获取五行抵抗值
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_DEF() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_DEF) { }
    }
    #endregion SkillEffectConfig: 获取五行抵抗值


    #region SkillEffectConfig: 获取五行强化值万分比
    [Serializable]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_ENHANCE : SkillEffectConfigNode
    {
		// 获取五行强化值万分比
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_ENHANCE() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_ENHANCE) { }
    }
    #endregion SkillEffectConfig: 获取五行强化值万分比


    #region SkillEffectConfig: 获取五行抵抗值万分比
    [Serializable]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_ANTI : SkillEffectConfigNode
    {
		// 获取五行抵抗值万分比
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_ANTI() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_ANTI) { }
    }
    #endregion SkillEffectConfig: 获取五行抵抗值万分比


    #region SkillEffectConfig: 获取技能五行类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能五行类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能五行类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能五行类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_ELEMENTS_TYPE : SkillEffectConfigNode
    {
		// 获取技能五行类型
		// 参数0 : 技能ID-SkillConfig
        public TSET_GET_SKILL_ELEMENTS_TYPE() : base(TSkillEffectType.TSET_GET_SKILL_ELEMENTS_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取技能五行类型


    #region SkillEffectConfig: 获取初始技能ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取初始技能ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取初始技能ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取初始技能ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ORIGIN_SKILL_ID : SkillEffectConfigNode
    {
		// 获取初始技能ID
        public TSET_GET_ORIGIN_SKILL_ID() : base(TSkillEffectType.TSET_GET_ORIGIN_SKILL_ID) { }
    }
    #endregion SkillEffectConfig: 获取初始技能ID


    #region SkillEffectConfig: 修改技能参数值
    [Serializable]
	[NodeMenuItem("技能效果/技能/修改技能参数值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能参数值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能参数值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MODIFY_SKILL_TAG_VALUE : SkillEffectConfigNode
    {
		// 修改技能参数值
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 参数值
		// 参数4 : 技能参数类型-TSkillTagsType
        public TSET_MODIFY_SKILL_TAG_VALUE() : base(TSkillEffectType.TSET_MODIFY_SKILL_TAG_VALUE) { }
    }
    #endregion SkillEffectConfig: 修改技能参数值


    #region SkillEffectConfig: 条件执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/条件执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/条件执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/条件执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CONDITION_EXECUTE : SkillEffectConfigNode
    {
		// 条件执行
		// 参数0 : 条件ID-SkillConditionConfig
		// 参数1 : 成功执行技能效果ID-SkillEffectConfig
		// 参数2 : 失败执行技能效果ID-SkillEffectConfig
        public TSET_CONDITION_EXECUTE() : base(TSkillEffectType.TSET_CONDITION_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 条件执行


    #region SkillEffectConfig: 获取技能参数值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能参数值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能参数值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能参数值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_TAG_VALUE : SkillEffectConfigNode
    {
		// 获取技能参数值
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 技能参数类型-TSkillTagsType
		// 参数4 : 是否获取最终值（按照Tag表配置效果）
        public TSET_GET_SKILL_TAG_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_TAG_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能参数值


    #region SkillEffectConfig: 获取单位有效创建者
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取单位有效创建者", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位有效创建者", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位有效创建者", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_AVALIABLE_CREATOR_ENTITY : SkillEffectConfigNode
    {
		// 获取单位有效创建者
		// 参数0 : 当前主体单位
		// 参数1 : 单位类型-TEntityType
        public TSET_GET_AVALIABLE_CREATOR_ENTITY() : base(TSkillEffectType.TSET_GET_AVALIABLE_CREATOR_ENTITY) { }
    }
    #endregion SkillEffectConfig: 获取单位有效创建者


    #region SkillEffectConfig: 正弦函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/正弦函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/正弦函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/正弦函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_SIN : SkillEffectConfigNode
    {
		// 正弦函数求值
		// 参数0 : 角度
        public TSET_MATH_SIN() : base(TSkillEffectType.TSET_MATH_SIN) { }
    }
    #endregion SkillEffectConfig: 正弦函数求值


    #region SkillEffectConfig: 余弦函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/余弦函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/余弦函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/余弦函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_COS : SkillEffectConfigNode
    {
		// 余弦函数求值
		// 参数0 : 角度
        public TSET_MATH_COS() : base(TSkillEffectType.TSET_MATH_COS) { }
    }
    #endregion SkillEffectConfig: 余弦函数求值


    #region SkillEffectConfig: 正切函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/正切函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/正切函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/正切函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_TAN : SkillEffectConfigNode
    {
		// 正切函数求值
		// 参数0 : 角度
        public TSET_MATH_TAN() : base(TSkillEffectType.TSET_MATH_TAN) { }
    }
    #endregion SkillEffectConfig: 正切函数求值


    #region SkillEffectConfig: 余切函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/余切函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/余切函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/余切函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_COT : SkillEffectConfigNode
    {
		// 余切函数求值
		// 参数0 : 角度
        public TSET_MATH_COT() : base(TSkillEffectType.TSET_MATH_COT) { }
    }
    #endregion SkillEffectConfig: 余切函数求值


    #region SkillEffectConfig: 反正弦函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/反正弦函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反正弦函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反正弦函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_ARCSIN : SkillEffectConfigNode
    {
		// 反正弦函数求值
		// 参数0 : 值
        public TSET_MATH_ARCSIN() : base(TSkillEffectType.TSET_MATH_ARCSIN) { }
    }
    #endregion SkillEffectConfig: 反正弦函数求值


    #region SkillEffectConfig: 反余弦函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/反余弦函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反余弦函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反余弦函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_ARCCOS : SkillEffectConfigNode
    {
		// 反余弦函数求值
		// 参数0 : 值
        public TSET_MATH_ARCCOS() : base(TSkillEffectType.TSET_MATH_ARCCOS) { }
    }
    #endregion SkillEffectConfig: 反余弦函数求值


    #region SkillEffectConfig: 反正切函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/反正切函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反正切函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反正切函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_ARCTAN : SkillEffectConfigNode
    {
		// 反正切函数求值
		// 参数0 : 值
        public TSET_MATH_ARCTAN() : base(TSkillEffectType.TSET_MATH_ARCTAN) { }
    }
    #endregion SkillEffectConfig: 反正切函数求值


    #region SkillEffectConfig: 反余切函数求值
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/三角函数运算/反余切函数求值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反余切函数求值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/三角函数运算/反余切函数求值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_ARCCOT : SkillEffectConfigNode
    {
		// 反余切函数求值
		// 参数0 : 值
        public TSET_MATH_ARCCOT() : base(TSkillEffectType.TSET_MATH_ARCCOT) { }
    }
    #endregion SkillEffectConfig: 反余切函数求值


    #region SkillEffectConfig: 获取子弹发射高度
    [Serializable]
    public sealed partial class TSET_GET_BULLET_HEIGHT : SkillEffectConfigNode
    {
		// 获取子弹发射高度
		// 参数0 : 子弹表格ID-BulletConfig
        public TSET_GET_BULLET_HEIGHT() : base(TSkillEffectType.TSET_GET_BULLET_HEIGHT) { }
    }
    #endregion SkillEffectConfig: 获取子弹发射高度


    #region SkillEffectConfig: 获取两点间方向
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取两点间方向", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两点间方向", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两点间方向", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ANGLE_BETWEEN_POINT : SkillEffectConfigNode
    {
		// 获取两点间方向
		// 参数0 : 点A-x坐标
		// 参数1 : 点A-y坐标
		// 参数2 : 点B-x坐标
		// 参数3 : 点B-y坐标
        public TSET_GET_ANGLE_BETWEEN_POINT() : base(TSkillEffectType.TSET_GET_ANGLE_BETWEEN_POINT) { }
    }
    #endregion SkillEffectConfig: 获取两点间方向


    #region SkillEffectConfig: 获取技能效果目标数量
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能效果目标数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能效果目标数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能效果目标数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_EFFECT_TARGET_COUNT : SkillEffectConfigNode
    {
		// 获取技能效果目标数量
        public TSET_GET_SKILL_EFFECT_TARGET_COUNT() : base(TSkillEffectType.TSET_GET_SKILL_EFFECT_TARGET_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取技能效果目标数量


    #region SkillEffectConfig: 获取目标单位碰撞位置X
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取目标单位碰撞位置X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取目标单位碰撞位置X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取目标单位碰撞位置X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_COLLISION_POS_X : SkillEffectConfigNode
    {
		// 获取目标单位碰撞位置X
		// 参数0 : 目标碰撞单位
		// 参数1 : 基准单位
		// 参数2 : 筛选反应层-TCollisionLayer
		// 参数3 : 碰撞子类型-TCollisionSubType
		// 参数4 : 目标碰撞位置类型-TBattleTargetFixturePosType
        public TSET_GET_ENTITY_COLLISION_POS_X() : base(TSkillEffectType.TSET_GET_ENTITY_COLLISION_POS_X) { }
    }
    #endregion SkillEffectConfig: 获取目标单位碰撞位置X


    #region SkillEffectConfig: 获取目标单位碰撞位置Y
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取目标单位碰撞位置Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取目标单位碰撞位置Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取目标单位碰撞位置Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_COLLISION_POS_Y : SkillEffectConfigNode
    {
		// 获取目标单位碰撞位置Y
		// 参数0 : 目标碰撞单位
		// 参数1 : 基准单位
		// 参数2 : 筛选反应层-TCollisionLayer
		// 参数3 : 碰撞子类型-TCollisionSubType
		// 参数4 : 碰撞位置类型-TBattleTargetFixturePosType
        public TSET_GET_ENTITY_COLLISION_POS_Y() : base(TSkillEffectType.TSET_GET_ENTITY_COLLISION_POS_Y) { }
    }
    #endregion SkillEffectConfig: 获取目标单位碰撞位置Y


    #region SkillEffectConfig: 继承基础属性
    [Serializable]
	[NodeMenuItem("技能效果/单位/继承基础属性", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承基础属性", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承基础属性", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_BASE_ATTR : SkillEffectConfigNode
    {
		// 继承基础属性
		// 参数0 : 继承者单位实例ID
		// 参数1 : 传承者单位实例ID
		// 参数2 : 继承系数万分比
        public TSET_COPY_ENTITY_BASE_ATTR() : base(TSkillEffectType.TSET_COPY_ENTITY_BASE_ATTR) { }
    }
    #endregion SkillEffectConfig: 继承基础属性


    #region SkillEffectConfig: 继承技能
    [Serializable]
	[NodeMenuItem("技能效果/单位/继承技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_SKILL : SkillEffectConfigNode
    {
		// 继承技能
		// 参数0 : 继承者单位实例ID
		// 参数1 : 传承者单位实例ID
        public TSET_COPY_ENTITY_SKILL() : base(TSkillEffectType.TSET_COPY_ENTITY_SKILL) { }
    }
    #endregion SkillEffectConfig: 继承技能


    #region SkillEffectConfig: 设置单位技能
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置单位技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIND_ENTITY_SKILL : SkillEffectConfigNode
    {
		// 设置单位技能
		// 参数0 : 被设置技能单位
		// 参数1 : 技能槽类型-TSkillSlotType
		// 参数2 : 技能ID-SkillConfig
		// 参数3 : 技能等级（<=0默认1级）
		// 参数4 : 是否设为战斗带入的最初 技能
        public TSET_BIND_ENTITY_SKILL() : base(TSkillEffectType.TSET_BIND_ENTITY_SKILL) { }
    }
    #endregion SkillEffectConfig: 设置单位技能


    #region SkillEffectConfig: 筛选单位数量
    [Serializable]
	[NodeMenuItem("技能效果/单位/筛选单位数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/筛选单位数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/筛选单位数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SELECT_ENTITY_COUNT : SkillEffectConfigNode
    {
		// 筛选单位数量
		// 参数0 : 筛选ID-SkillSelectConfig
        public TSET_GET_SELECT_ENTITY_COUNT() : base(TSkillEffectType.TSET_GET_SELECT_ENTITY_COUNT) { }
    }
    #endregion SkillEffectConfig: 筛选单位数量


    #region SkillEffectConfig: 注册技能消息
    [Serializable]
	[NodeMenuItem("技能效果/技能/注册技能消息", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/注册技能消息", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/注册技能消息", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REGISTER_SKILL_EVENT : SkillEffectConfigNode
    {
		// 注册技能消息
		// 参数0 : 技能消息ID-SkillEventConfig
		// 参数1 : 技能效果ID-SkillEffectConfig
		// 参数2 : 绑定技能ID(不填为0通用)-SkillConfig
		// 参数3 : 筛选ID 不填写则使用原始目标-SkillSelectConfig
		// 参数4 : 条件ID-SkillConditionConfig
		// 参数5 : 触发次数（<=0表示不限次数）
		// 参数6 : 消息筛选-目标阵营-TSkillTargetCampType
		// 参数7 : 消息筛选-技能ID-SkillConfig
		// 参数8 : 消息筛选-技能子类型-TBattleSkillSubType
		// 参数9 : 事件子类型-TSkillEventSubType
		// 参数10 : 事件子类型的值
        public TSET_REGISTER_SKILL_EVENT() : base(TSkillEffectType.TSET_REGISTER_SKILL_EVENT) { }
    }
    #endregion SkillEffectConfig: 注册技能消息


    #region SkillEffectConfig: 反注册技能消息
    [Serializable]
	[NodeMenuItem("技能效果/技能/反注册技能消息", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/反注册技能消息", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/反注册技能消息", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_UNREGISTER_SKILL_EVENT : SkillEffectConfigNode
    {
		// 反注册技能消息
		// 参数0 : 技能消息ID-SkillEventConfig
		// 参数1 : 注册技能消息效果ID-SkillEffectConfig
		// 参数2 : 绑定技能ID(不填为0通用)-SkillConfig
		// 参数3 : 事件子类型-TSkillEventSubType
		// 参数4 : 事件子类型的值
        public TSET_UNREGISTER_SKILL_EVENT() : base(TSkillEffectType.TSET_UNREGISTER_SKILL_EVENT) { }
    }
    #endregion SkillEffectConfig: 反注册技能消息


    #region SkillEffectConfig: 发送技能消息
    [Serializable]
	[NodeMenuItem("技能效果/技能/发送技能消息", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/发送技能消息", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/发送技能消息", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_FIRE_SKILL_EVENT : SkillEffectConfigNode
    {
		// 发送技能消息
		// 参数0 : 发送者单位实例ID
		// 参数1 : 技能消息ID-SkillEventConfig
		// 参数2 : 参数1
		// 参数3 : 参数2
		// 参数4 : 参数3
		// 参数5 : 参数4
		// 参数6 : 参数5
		// 参数7 : 参数6
		// 参数8 : 参数7
		// 参数9 : 参数8
		// 参数10 : 参数9
		// 参数11 : 参数10
		// 参数12 : 参数11
		// 参数13 : 参数12
        public TSET_FIRE_SKILL_EVENT() : base(TSkillEffectType.TSET_FIRE_SKILL_EVENT) { }
    }
    #endregion SkillEffectConfig: 发送技能消息


    #region SkillEffectConfig: 增加单位属性
    [Serializable]
	[NodeMenuItem("技能效果/单位/增加单位属性", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/增加单位属性", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/增加单位属性", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ENTITY_ATTR_VALUE : SkillEffectConfigNode
    {
		// 增加单位属性
		// 参数0 : 被增加属性单位
		// 参数1 : 属性枚举值-TBattleNatureEnum
		// 参数2 : 属性变化值
        public TSET_ADD_ENTITY_ATTR_VALUE() : base(TSkillEffectType.TSET_ADD_ENTITY_ATTR_VALUE) { }
    }
    #endregion SkillEffectConfig: 增加单位属性


    #region SkillEffectConfig: 设置单位筛选冷却
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置单位筛选冷却", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位筛选冷却", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置单位筛选冷却", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_ENTITY_SELECT_CD : SkillEffectConfigNode
    {
		// 设置单位筛选冷却
		// 参数0 : 被设置冷却单位
		// 参数1 : 筛选ID-SkillSelectConfig
		// 参数2 : 冷却时间
		// 参数3 : 筛选主体单位ID
        public TSET_SET_ENTITY_SELECT_CD() : base(TSkillEffectType.TSET_SET_ENTITY_SELECT_CD) { }
    }
    #endregion SkillEffectConfig: 设置单位筛选冷却


    #region SkillEffectConfig: 角度距离计算位置X
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/角度距离计算位置/角度距离计算位置X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/角度距离计算位置X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/角度距离计算位置X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_POS_X_BY_ANGLE_DISTANCE : SkillEffectConfigNode
    {
		// 角度距离计算位置X
		// 参数0 : 基准点坐标X
		// 参数1 : 基准点坐标Y
		// 参数2 : 角度
		// 参数3 : 偏移距离_面前(基于角度方向)
		// 参数4 : 偏移距离_右侧(基于角度方向)
		// 参数5 : 是否超出边界强制回归
		// 参数6 : 强制回归-计算次数(360度分割扫描查找)
		// 参数7 : 强制回归-距离边界最小距离
        public TSET_GET_POS_X_BY_ANGLE_DISTANCE() : base(TSkillEffectType.TSET_GET_POS_X_BY_ANGLE_DISTANCE) { }
    }
    #endregion SkillEffectConfig: 角度距离计算位置X


    #region SkillEffectConfig: 角度距离计算位置Y
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/角度距离计算位置/角度距离计算位置Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/角度距离计算位置Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/角度距离计算位置Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_POS_Y_BY_ANGLE_DISTANCE : SkillEffectConfigNode
    {
		// 角度距离计算位置Y
		// 参数0 : 基准点坐标X
		// 参数1 : 基准点坐标Y
		// 参数2 : 角度
		// 参数3 : 偏移距离_面前(基于角度方向)
		// 参数4 : 偏移距离_右侧(基于角度方向)
		// 参数5 : 是否超出边界强制回归
		// 参数6 : 强制回归-计算次数(360度分割扫描查找)
		// 参数7 : 强制回归-距离边界最小距离
        public TSET_GET_POS_Y_BY_ANGLE_DISTANCE() : base(TSkillEffectType.TSET_GET_POS_Y_BY_ANGLE_DISTANCE) { }
    }
    #endregion SkillEffectConfig: 角度距离计算位置Y


    #region SkillEffectConfig: 应用单位升空效果
    [Serializable]
	[NodeMenuItem("技能效果/空间/应用单位升空效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/空间/应用单位升空效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/空间/应用单位升空效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_APPLY_ENTITY_BLAST_OFF_EFFECT : SkillEffectConfigNode
    {
		// 应用单位升空效果
		// 参数0 : 被升空单位
		// 参数1 : 持续时间
		// 参数2 : 贝塞尔曲线阶次
		// 参数3 : 曲线中间点坐标X
		// 参数4 : 曲线中间点坐标Y
		// 参数5 : 曲线中间点坐标Z
		// 参数6 : 是否切线方向（0_1）
        public TSET_APPLY_ENTITY_BLAST_OFF_EFFECT() : base(TSkillEffectType.TSET_APPLY_ENTITY_BLAST_OFF_EFFECT) { }
    }
    #endregion SkillEffectConfig: 应用单位升空效果


    #region SkillEffectConfig: 获取当前地图宽度
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取当前地图宽度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前地图宽度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前地图宽度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CUR_MAP_WIDTH : SkillEffectConfigNode
    {
		// 获取当前地图宽度
        public TSET_GET_CUR_MAP_WIDTH() : base(TSkillEffectType.TSET_GET_CUR_MAP_WIDTH) { }
    }
    #endregion SkillEffectConfig: 获取当前地图宽度


    #region SkillEffectConfig: 获取当前地图高度
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取当前地图高度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前地图高度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前地图高度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CUR_MAP_HEIGHT : SkillEffectConfigNode
    {
		// 获取当前地图高度
        public TSET_GET_CUR_MAP_HEIGHT() : base(TSkillEffectType.TSET_GET_CUR_MAP_HEIGHT) { }
    }
    #endregion SkillEffectConfig: 获取当前地图高度


    #region SkillEffectConfig: 骑乘坐骑
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/骑乘坐骑", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/骑乘坐骑", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/骑乘坐骑", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNT_RIDE : SkillEffectConfigNode
    {
		// 骑乘坐骑
		// 参数0 : 目标被骑乘单位
        public TSET_MOUNT_RIDE() : base(TSkillEffectType.TSET_MOUNT_RIDE) { }
    }
    #endregion SkillEffectConfig: 骑乘坐骑


    #region SkillEffectConfig: 卸下坐骑
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/卸下坐骑", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/卸下坐骑", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/卸下坐骑", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNT_DOWN : SkillEffectConfigNode
    {
		// 卸下坐骑
		// 参数0 : 该坐骑拥有者单位
        public TSET_MOUNT_DOWN() : base(TSkillEffectType.TSET_MOUNT_DOWN) { }
    }
    #endregion SkillEffectConfig: 卸下坐骑


    #region SkillEffectConfig: 获取单位静态表ID对应的单位ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取单位静态表ID对应的单位ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位静态表ID对应的单位ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位静态表ID对应的单位ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ID_BY_BATTLE_UNIT_CONFIG_ID : SkillEffectConfigNode
    {
		// 获取单位静态表ID对应的单位ID
		// 参数0 : 单位静态表ID
		// 参数1 : 单位子类型（默认战斗单位-默认）-TEntitySubType
        public TSET_GET_ENTITY_ID_BY_BATTLE_UNIT_CONFIG_ID() : base(TSkillEffectType.TSET_GET_ENTITY_ID_BY_BATTLE_UNIT_CONFIG_ID) { }
    }
    #endregion SkillEffectConfig: 获取单位静态表ID对应的单位ID


    #region SkillEffectConfig: 获取坐骑的坐骑表ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取坐骑的坐骑表ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑的坐骑表ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑的坐骑表ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_CONFIG_ID : SkillEffectConfigNode
    {
		// 获取坐骑的坐骑表ID
		// 参数0 : 所属单位
        public TSET_GET_MOUNT_CONFIG_ID() : base(TSkillEffectType.TSET_GET_MOUNT_CONFIG_ID) { }
    }
    #endregion SkillEffectConfig: 获取坐骑的坐骑表ID


    #region SkillEffectConfig: 设置角色坐骑ID
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置角色坐骑ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置角色坐骑ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置角色坐骑ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SETMOUNT_ID : SkillEffectConfigNode
    {
		// 设置角色坐骑ID
		// 参数0 : 被设置坐骑的单位
		// 参数1 : 坐骑ID
        public TSET_SETMOUNT_ID() : base(TSkillEffectType.TSET_SETMOUNT_ID) { }
    }
    #endregion SkillEffectConfig: 设置角色坐骑ID


    #region SkillEffectConfig: 获取两单位之间的距离
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取两单位之间的距离", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两单位之间的距离", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两单位之间的距离", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_DISTANCE : SkillEffectConfigNode
    {
		// 获取两单位之间的距离
		// 参数0 : 基准单位
		// 参数1 : 目标单位
		// 参数2 : 基准位置类型-TBattleBaseFixturePosType
		// 参数3 : 目标位置类型-TBattleTargetFixturePosType
        public TSET_GET_ENTITY_DISTANCE() : base(TSkillEffectType.TSET_GET_ENTITY_DISTANCE) { }
    }
    #endregion SkillEffectConfig: 获取两单位之间的距离


    #region SkillEffectConfig: 阵营获取玩家ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/阵营获取玩家ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/阵营获取玩家ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/阵营获取玩家ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PLAYER_INDEX_BY_CAMP : SkillEffectConfigNode
    {
		// 阵营获取玩家ID
		// 参数0 : 阵营-TEntityCamp
        public TSET_GET_PLAYER_INDEX_BY_CAMP() : base(TSkillEffectType.TSET_GET_PLAYER_INDEX_BY_CAMP) { }
    }
    #endregion SkillEffectConfig: 阵营获取玩家ID


    #region SkillEffectConfig: 战斗结束
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/战斗结束", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗结束", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗结束", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_END : SkillEffectConfigNode
    {
		// 战斗结束
		// 参数0 : 胜利阵营-TEntityCamp
		// 参数1 : 失败阵营
		// 参数2 : 胜利玩家
		// 参数3 : 失败玩家
        public TSET_BATTLE_END() : base(TSkillEffectType.TSET_BATTLE_END) { }
    }
    #endregion SkillEffectConfig: 战斗结束


    #region SkillEffectConfig: 权重随机
    [Serializable]
	[NodeMenuItem("通用配置/随机/权重随机", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/权重随机", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/随机/权重随机", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_WEIGHT_RANDOM : SkillEffectConfigNode
    {
		// 权重随机
		// 参数0 : 权重1
		// 参数1 : 权重2
		// 参数2 : 权重3
		// 参数3 : 权重4
		// 参数4 : 权重5
        public TSET_WEIGHT_RANDOM() : base(TSkillEffectType.TSET_WEIGHT_RANDOM) { }
    }
    #endregion SkillEffectConfig: 权重随机


    #region SkillEffectConfig: 获取数组元素
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取数组元素", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取数组元素", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取数组元素", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ARRAY_ELEMENT : SkillEffectConfigNode
    {
		// 获取数组元素
		// 参数0 : 索引
		// 参数1 : 数组元素
        public TSET_GET_ARRAY_ELEMENT() : base(TSkillEffectType.TSET_GET_ARRAY_ELEMENT) { }
    }
    #endregion SkillEffectConfig: 获取数组元素


    #region SkillEffectConfig: 更换单位模型
    [Serializable]
	[NodeMenuItem("技能效果/单位/更换单位模型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/更换单位模型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/更换单位模型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_MODEL_CONFIG : SkillEffectConfigNode
    {
		// 更换单位模型
		// 参数0 : 被更换模型单位
		// 参数1 : 模型表ID-ModelConfig
		// 参数2 : 是否重置模型缩放
		// 参数3 : 是否设为原始模型
        public TSET_CHANGE_ENTITY_MODEL_CONFIG() : base(TSkillEffectType.TSET_CHANGE_ENTITY_MODEL_CONFIG) { }
    }
    #endregion SkillEffectConfig: 更换单位模型


    #region SkillEffectConfig: 获取坐骑单位实例ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取坐骑单位实例ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑单位实例ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑单位实例ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_ENTITY_ID : SkillEffectConfigNode
    {
		// 获取坐骑单位实例ID
		// 参数0 : 坐骑所属单位
        public TSET_GET_MOUNT_ENTITY_ID() : base(TSkillEffectType.TSET_GET_MOUNT_ENTITY_ID) { }
    }
    #endregion SkillEffectConfig: 获取坐骑单位实例ID


    #region SkillEffectConfig: 快速治疗效果
    [Serializable]
	[NodeMenuItem("技能效果/技能/快速治疗效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/快速治疗效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/快速治疗效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_QUICK_CURE : SkillEffectConfigNode
    {
		// 快速治疗效果
		// 参数0 : 治疗值
		// 参数1 : 治疗五行类型-TElementsType
		// 参数2 : 是否暴击
		// 参数3 : 禁止显示层显示
        public TSET_QUICK_CURE() : base(TSkillEffectType.TSET_QUICK_CURE) { }
    }
    #endregion SkillEffectConfig: 快速治疗效果


    #region SkillEffectConfig: 标准治疗效果
    [Serializable]
	[NodeMenuItem("技能效果/技能/标准治疗效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标准治疗效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标准治疗效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CURE : SkillEffectConfigNode
    {
		// 标准治疗效果
		// 参数0 : 治疗来源单位
		// 参数1 : 治疗目标单位
		// 参数2 : 治疗值
		// 参数3 : 治疗五行类型-TElementsType
		// 参数4 : 是否暴击
		// 参数5 : 禁止显示层显示
        public TSET_CURE() : base(TSkillEffectType.TSET_CURE) { }
    }
    #endregion SkillEffectConfig: 标准治疗效果


    #region SkillEffectConfig: 修改单位X轴旋转
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位X轴旋转", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位X轴旋转", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位X轴旋转", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_X_AXIS_ANGLE : SkillEffectConfigNode
    {
		// 修改单位X轴旋转
		// 参数0 : 被修改单位
		// 参数1 : 角度
        public TSET_CHANGE_ENTITY_X_AXIS_ANGLE() : base(TSkillEffectType.TSET_CHANGE_ENTITY_X_AXIS_ANGLE) { }
    }
    #endregion SkillEffectConfig: 修改单位X轴旋转


    #region SkillEffectConfig: 修改镜头跟随单位
    [Serializable]
	[NodeMenuItem("技能效果/相机/修改镜头跟随单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头跟随单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头跟随单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_CAMERA_FOLLOW_ENTITY : SkillEffectConfigNode
    {
		// 修改镜头跟随单位
		// 参数0 : 被镜头跟随单位
		// 参数1 : 曲线ID
		// 参数2 : 过渡时间
		// 参数3 : 持续时间
		// 参数4 : 是否切回
		// 参数5 : 切回时间
		// 参数6 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数7 : 优先级-TCameraPriority
		// 参数8 : 优先级插入方式-TPriorityInsertType
		// 参数9 : 是否暂停逻辑帧(仅PVE)
        public TSET_CHANGE_CAMERA_FOLLOW_ENTITY() : base(TSkillEffectType.TSET_CHANGE_CAMERA_FOLLOW_ENTITY) { }
    }
    #endregion SkillEffectConfig: 修改镜头跟随单位


    #region SkillEffectConfig: 修改镜头跟随偏移
    [Serializable]
	[NodeMenuItem("技能效果/相机/修改镜头跟随偏移", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头跟随偏移", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头跟随偏移", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_CAMERA_OFFSET : SkillEffectConfigNode
    {
		// 修改镜头跟随偏移
		// 参数0 : 被镜头跟随单位
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 优先级-TCameraPriority
		// 参数3 : 优先级插入方式-TPriorityInsertType
		// 参数4 : 偏移X
		// 参数5 : 偏移Y
		// 参数6 : 偏移Z
		// 参数7 : 曲线ID
		// 参数8 : 过渡时间
		// 参数9 : 持续时间
		// 参数10 : 是否切回
		// 参数11 : 切回时间
		// 参数12 : 偏移方式(0:相对 1:绝对 2:基础相对)
        public TSET_CHANGE_CAMERA_OFFSET() : base(TSkillEffectType.TSET_CHANGE_CAMERA_OFFSET) { }
    }
    #endregion SkillEffectConfig: 修改镜头跟随偏移


    #region SkillEffectConfig: 增加技能参数值
    [Serializable]
	[NodeMenuItem("技能效果/技能/增加技能参数值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加技能参数值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加技能参数值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_SKILL_TAG_VALUE : SkillEffectConfigNode
    {
		// 增加技能参数值
		// 参数0 : 被增加技能参数单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 增加值
		// 参数4 : 技能参数类型-TSkillTagsType
        public TSET_ADD_SKILL_TAG_VALUE() : base(TSkillEffectType.TSET_ADD_SKILL_TAG_VALUE) { }
    }
    #endregion SkillEffectConfig: 增加技能参数值


    #region SkillEffectConfig: 获取玩家主控单位
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取玩家主控单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家主控单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家主控单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PLAYER_MAIN_CONTROL_ENTITY : SkillEffectConfigNode
    {
		// 获取玩家主控单位
		// 参数0 : 玩家PlayerIndex(不是阵营)
        public TSET_GET_PLAYER_MAIN_CONTROL_ENTITY() : base(TSkillEffectType.TSET_GET_PLAYER_MAIN_CONTROL_ENTITY) { }
    }
    #endregion SkillEffectConfig: 获取玩家主控单位


    #region SkillEffectConfig: 获取BUFF层数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取BUFF层数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取BUFF层数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取BUFF层数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BUFF_LAYER_COUNT : SkillEffectConfigNode
    {
		// 获取BUFF层数
		// 参数0 : 目标挂载单位
		// 参数1 : 来源单位实例ID（0：不指定来源）
		// 参数2 : Buff表格ID-BuffConfig
        public TSET_GET_BUFF_LAYER_COUNT() : base(TSkillEffectType.TSET_GET_BUFF_LAYER_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取BUFF层数


    #region SkillEffectConfig: 获取当前战斗ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取当前战斗ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前战斗ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取当前战斗ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CUR_BATTLE_CONFIG_ID : SkillEffectConfigNode
    {
		// 获取当前战斗ID
        public TSET_GET_CUR_BATTLE_CONFIG_ID() : base(TSkillEffectType.TSET_GET_CUR_BATTLE_CONFIG_ID) { }
    }
    #endregion SkillEffectConfig: 获取当前战斗ID


    #region SkillEffectConfig: 获取战斗类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取战斗类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取战斗类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取战斗类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_TYPE : SkillEffectConfigNode
    {
		// 获取战斗类型
		// 参数0 : 战斗ID（0：当前战斗）-BattleConfig
        public TSET_GET_BATTLE_TYPE() : base(TSkillEffectType.TSET_GET_BATTLE_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取战斗类型


    #region SkillEffectConfig: 获取技能表数据
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能表数据", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能表数据", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能表数据", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_CONFIG_DATA_FIELD_VALUE : SkillEffectConfigNode
    {
		// 获取技能表数据
		// 参数0 : 技能ID
		// 参数1 : 技能表数据字段类型-TSkillConfigDataFieldType
		// 参数2 : 技能等级
		// 参数3 : 单位实例ID
        public TSET_GET_SKILL_CONFIG_DATA_FIELD_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_CONFIG_DATA_FIELD_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能表数据


    #region SkillEffectConfig: 播放音效
    [Serializable]
	[NodeMenuItem("技能效果/音效/播放音效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/播放音效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/播放音效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PLAY_SOUND : SkillEffectConfigNode
    {
		// 播放音效
		// 参数0 : 音效来源单位
		// 参数1 : 音效表格ID-VoiceConfig
		// 参数2 : 音效类型-TBattleSoundType
		// 参数3 : 是否需要停止(StopOnDisable)
		// 参数4 : 淡出帧数（默认统一6帧）
		// 参数5 : 音效挂接单位
		// 参数6 : 音效播放坐标_X
		// 参数7 : 音效播放坐标_Y
		// 参数8 : 是否为纯音效[非单位化]
		// 参数9 : 加入单位组：-TSkillEntityGroupType
		// 参数10 : 音效持续帧数
        public TSET_PLAY_SOUND() : base(TSkillEffectType.TSET_PLAY_SOUND) { }
    }
    #endregion SkillEffectConfig: 播放音效


    #region SkillEffectConfig: 获取单位类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取单位类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_TYPE : SkillEffectConfigNode
    {
		// 获取单位类型
		// 参数0 : 被获取目标单位
        public TSET_GET_ENTITY_TYPE() : base(TSkillEffectType.TSET_GET_ENTITY_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取单位类型


    #region SkillEffectConfig: 召唤战斗布局单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/召唤战斗布局单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤战斗布局单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤战斗布局单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_BATTLE_UNIT_BY_LAYOUT : SkillEffectConfigNode
    {
		// 召唤战斗布局单位
		// 参数0 : 战斗布局ID-LayoutConfig
		// 参数1 : 阵营-TEntityCamp
		// 参数2 : 玩家ID
		// 参数3 : 是否可控(默认可控)
        public TSET_CREATE_BATTLE_UNIT_BY_LAYOUT() : base(TSkillEffectType.TSET_CREATE_BATTLE_UNIT_BY_LAYOUT) { }
    }
    #endregion SkillEffectConfig: 召唤战斗布局单位


    #region SkillEffectConfig: 增加子弹持续时间
    [Serializable]
	[NodeMenuItem("技能效果/技能/增加子弹持续时间", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加子弹持续时间", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加子弹持续时间", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BULLET_DURATION_TIME : SkillEffectConfigNode
    {
		// 增加子弹持续时间
		// 参数0 : 子弹单位实例ID
		// 参数1 : 持续时间
        public TSET_ADD_BULLET_DURATION_TIME() : base(TSkillEffectType.TSET_ADD_BULLET_DURATION_TIME) { }
    }
    #endregion SkillEffectConfig: 增加子弹持续时间


    #region SkillEffectConfig: 技能CD设置-进入CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能CD设置-进入CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-进入CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-进入CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_SKILLCD_ENTER : SkillEffectConfigNode
    {
		// 技能CD设置-进入CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽类型-TSkillSlotType
		// 参数2 : 剩余CD百分比(0-100)
		// 参数3 : 是否清空储能技能层数(0-1)
        public TSET_SET_SKILLCD_ENTER() : base(TSkillEffectType.TSET_SET_SKILLCD_ENTER) { }
    }
    #endregion SkillEffectConfig: 技能CD设置-进入CD


    #region SkillEffectConfig: 技能CD设置-刷新CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能CD设置-刷新CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-刷新CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-刷新CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_SKILLCD_REFRESH : SkillEffectConfigNode
    {
		// 技能CD设置-刷新CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽类型-TSkillSlotType
        public TSET_SET_SKILLCD_REFRESH() : base(TSkillEffectType.TSET_SET_SKILLCD_REFRESH) { }
    }
    #endregion SkillEffectConfig: 技能CD设置-刷新CD


    #region SkillEffectConfig: 技能CD设置-减少CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能CD设置-减少CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-减少CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能CD设置-减少CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_SKILLCD_REDUCE : SkillEffectConfigNode
    {
		// 技能CD设置-减少CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽类型-TSkillSlotType
		// 参数2 : 减少帧数
        public TSET_SET_SKILLCD_REDUCE() : base(TSkillEffectType.TSET_SET_SKILLCD_REDUCE) { }
    }
    #endregion SkillEffectConfig: 技能CD设置-减少CD


    #region SkillEffectConfig: 移除Buff-按Buff类型
    [Serializable]
    public sealed partial class TSET_REMOVE_BUFF_BY_TYPE : SkillEffectConfigNode
    {
		// 移除Buff-按Buff类型
		// 参数0 : 被移除Buff单位
		// 参数1 : Buff类型-TBuffType
        public TSET_REMOVE_BUFF_BY_TYPE() : base(TSkillEffectType.TSET_REMOVE_BUFF_BY_TYPE) { }
    }
    #endregion SkillEffectConfig: 移除Buff-按Buff类型


    #region SkillEffectConfig: 终止执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/终止执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/终止执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/终止执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_STOP_TIMER_TASK : SkillEffectConfigNode
    {
		// 终止执行
		// 参数0 : 执行任务ID
        public TSET_STOP_TIMER_TASK() : base(TSkillEffectType.TSET_STOP_TIMER_TASK) { }
    }
    #endregion SkillEffectConfig: 终止执行


    #region SkillEffectConfig: 显示冒泡对话
    [Serializable]
	[NodeMenuItem("技能效果/UI/显示冒泡对话", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示冒泡对话", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示冒泡对话", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_BUBBLE_DIALOGUE : SkillEffectConfigNode
    {
		// 显示冒泡对话
		// 参数0 : 目标挂载单位
		// 参数1 : 对话文本ID-TextConfig
		// 参数2 : 持续时间
		// 参数3 : 对谁生效(0表示都生效)-TViewEventTargetType
        public TSET_SHOW_BUBBLE_DIALOGUE() : base(TSkillEffectType.TSET_SHOW_BUBBLE_DIALOGUE) { }
    }
    #endregion SkillEffectConfig: 显示冒泡对话


    #region SkillEffectConfig: 显示次级对话
    [Serializable]
	[NodeMenuItem("技能效果/UI/显示次级对话", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示次级对话", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示次级对话", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_SECONDARY_DIALOGUE : SkillEffectConfigNode
    {
		// 显示次级对话
		// 参数0 : NPC表ID-NpcConfig
		// 参数1 : 对话文本ID-TextConfig
		// 参数2 : 持续时间-帧
        public TSET_SHOW_SECONDARY_DIALOGUE() : base(TSkillEffectType.TSET_SHOW_SECONDARY_DIALOGUE) { }
    }
    #endregion SkillEffectConfig: 显示次级对话


    #region SkillEffectConfig: 创建扇形预警圈
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/创建扇形预警圈", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/创建扇形预警圈", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/创建扇形预警圈", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_SECTOR_WARNING : SkillEffectConfigNode
    {
		// 创建扇形预警圈
		// 参数0 : 模型ID-ModelConfig
		// 参数1 : 位置X(无效)
		// 参数2 : 位置Y(无效)
		// 参数3 : 朝向角度
		// 参数4 : 扇形半径
		// 参数5 : 扇形角度
		// 参数6 : 变化时间
		// 参数7 : 是否跟随目标朝向
		// 参数8 : 加入单位组：-TSkillEntityGroupType
		// 参数9 : 内圈半径
		// 参数10 : 高度Z是否跟随
		// 参数11 : 结束前闪烁帧数
		// 参数12 : 初始进度百分比
		// 参数13 : 位置偏移_右侧
		// 参数14 : 位置偏移_面前
        public TSET_CREATE_SECTOR_WARNING() : base(TSkillEffectType.TSET_CREATE_SECTOR_WARNING) { }
    }
    #endregion SkillEffectConfig: 创建扇形预警圈


    #region SkillEffectConfig: 角色立绘对话
    [Serializable]
	[NodeMenuItem("技能效果/UI/角色立绘对话", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/角色立绘对话", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/角色立绘对话", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROLE_DIALOGUE : SkillEffectConfigNode
    {
		// 角色立绘对话
		// 参数0 : 行为表ID-BehaviorConfig
		// 参数1 : 是否暂停战斗
        public TSET_ROLE_DIALOGUE() : base(TSkillEffectType.TSET_ROLE_DIALOGUE) { }
    }
    #endregion SkillEffectConfig: 角色立绘对话


    #region SkillEffectConfig: 中断技能
    [Serializable]
	[NodeMenuItem("技能效果/技能/中断技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/中断技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/中断技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_INTERRUPT_SKILLEFFECT : SkillEffectConfigNode
    {
		// 中断技能
		// 参数0 : 目标中断单位
		// 参数1 : 中断方式-TSkillInterruptType
		// 参数2 : SkillTagID-SkillTagsConfig
		// 参数3 : 是否同时移除有中断标记的Buff
        public TSET_INTERRUPT_SKILLEFFECT() : base(TSkillEffectType.TSET_INTERRUPT_SKILLEFFECT) { }
    }
    #endregion SkillEffectConfig: 中断技能


    #region SkillEffectConfig: 模板技能效果
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/模板技能效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/模板技能效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/模板技能效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RUN_SKILL_EFFECT_TEMPLATE : SkillEffectConfigNode
    {
		// 模板技能效果
		// 参数0 : 主体单位（最近的主体单位）
		// 参数1 : 目标筛选单位（最近筛选的结果）
		// 参数2 : 技能效果ID-SkillEffectConfig
        public TSET_RUN_SKILL_EFFECT_TEMPLATE() : base(TSkillEffectType.TSET_RUN_SKILL_EFFECT_TEMPLATE) { }
    }
    #endregion SkillEffectConfig: 模板技能效果


    #region SkillEffectConfig: AI获取另一个单位的距离
    [Serializable]
    public sealed partial class TSET_AI_GETDISTANCE : SkillEffectConfigNode
    {
		// AI获取另一个单位的距离
        public TSET_AI_GETDISTANCE() : base(TSkillEffectType.TSET_AI_GETDISTANCE) { }
    }
    #endregion SkillEffectConfig: AI获取另一个单位的距离


    #region SkillEffectConfig: AI朝目标单位移动(寻路方向)
    [Serializable]
	[NodeMenuItem("效果/AI朝目标单位移动(寻路方向)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_MOVETO : SkillEffectConfigNode
    {
		// AI朝目标单位移动(寻路方向)
		// 参数0 : AI单位
		// 参数1 : 目标单位实例ID
        public TSET_AI_MOVETO() : base(TSkillEffectType.TSET_AI_MOVETO) { }
    }
    #endregion SkillEffectConfig: AI朝目标单位移动(寻路方向)


    #region SkillEffectConfig: AI获取技能施法距离
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/AI获取技能施法距离", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/AI获取技能施法距离", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/AI获取技能施法距离", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_GET_SKILL_RANGE : SkillEffectConfigNode
    {
		// AI获取技能施法距离
		// 参数0 : 技能槽索引-TSkillSlotType
		// 参数1 : 是否获取AI施法距离（0:施法距离1:AI施法距离）
        public TSET_AI_GET_SKILL_RANGE() : base(TSkillEffectType.TSET_AI_GET_SKILL_RANGE) { }
    }
    #endregion SkillEffectConfig: AI获取技能施法距离


    #region SkillEffectConfig: AI使用技能
    [Serializable]
	[NodeMenuItem("技能效果/技能/AI使用技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/AI使用技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/AI使用技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_USE_SKILL : SkillEffectConfigNode
    {
		// AI使用技能
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 技能目标单位
		// 参数2 : 距离超出不移动（0移动_1不移动）
		// 参数3 : 施法状态判定（0忽略_1施法状态则不释放_建议Boss）
        public TSET_AI_USE_SKILL() : base(TSkillEffectType.TSET_AI_USE_SKILL) { }
    }
    #endregion SkillEffectConfig: AI使用技能


    #region SkillEffectConfig: AI获取单位X坐标点
    [Serializable]
    public sealed partial class TSET_AI_GETPOSX : SkillEffectConfigNode
    {
		// AI获取单位X坐标点
		// 参数0 : 目标单位
        public TSET_AI_GETPOSX() : base(TSkillEffectType.TSET_AI_GETPOSX) { }
    }
    #endregion SkillEffectConfig: AI获取单位X坐标点


    #region SkillEffectConfig: AI获取单位Y坐标点
    [Serializable]
    public sealed partial class TSET_AI_GETPOSY : SkillEffectConfigNode
    {
		// AI获取单位Y坐标点
		// 参数0 : 目标单位
        public TSET_AI_GETPOSY() : base(TSkillEffectType.TSET_AI_GETPOSY) { }
    }
    #endregion SkillEffectConfig: AI获取单位Y坐标点


    #region SkillEffectConfig: AI静止不动
    [Serializable]
	[NodeMenuItem("效果/行为/AI静止不动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_STANDSKILL : SkillEffectConfigNode
    {
		// AI静止不动
        public TSET_AI_STANDSKILL() : base(TSkillEffectType.TSET_AI_STANDSKILL) { }
    }
    #endregion SkillEffectConfig: AI静止不动


    #region SkillEffectConfig: AI获取技能槽CD
    [Serializable]
	[NodeMenuItem("效果/数据参数获取/AI获取技能槽CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_GETSKILLCD : SkillEffectConfigNode
    {
		// AI获取技能槽CD
		// 参数0 : 技能槽索引-TSkillSlotType
        public TSET_AI_GETSKILLCD() : base(TSkillEffectType.TSET_AI_GETSKILLCD) { }
    }
    #endregion SkillEffectConfig: AI获取技能槽CD


    #region SkillEffectConfig: AI获取单位朝向
    [Serializable]
	[NodeMenuItem("效果/数据参数获取/AI获取单位朝向", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_GETFACE : SkillEffectConfigNode
    {
		// AI获取单位朝向
        public TSET_AI_GETFACE() : base(TSkillEffectType.TSET_AI_GETFACE) { }
    }
    #endregion SkillEffectConfig: AI获取单位朝向


    #region SkillEffectConfig: AI朝方向移动
    [Serializable]
	[NodeMenuItem("技能效果/AI/AI朝方向移动", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/AI/AI朝方向移动", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/AI/AI朝方向移动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_MOVETOANGLE : SkillEffectConfigNode
    {
		// AI朝方向移动
		// 参数0 : 角度值
		// 参数1 : 原地不动
        public TSET_AI_MOVETOANGLE() : base(TSkillEffectType.TSET_AI_MOVETOANGLE) { }
    }
    #endregion SkillEffectConfig: AI朝方向移动


    #region SkillEffectConfig: AI获取单位ID
    [Serializable]
    public sealed partial class TSET_AI_GETID : SkillEffectConfigNode
    {
		// AI获取单位ID
        public TSET_AI_GETID() : base(TSkillEffectType.TSET_AI_GETID) { }
    }
    #endregion SkillEffectConfig: AI获取单位ID


    #region SkillEffectConfig: AI被包围逃跑
    [Serializable]
	[NodeMenuItem("效果/行为/AI被包围逃跑", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_INSURROUND : SkillEffectConfigNode
    {
		// AI被包围逃跑
		// 参数0 : 逃跑距离
		// 参数1 : 疏散值
		// 参数2 : 筛选ID-SkillSelectConfig
        public TSET_AI_INSURROUND() : base(TSkillEffectType.TSET_AI_INSURROUND) { }
    }
    #endregion SkillEffectConfig: AI被包围逃跑


    #region SkillEffectConfig: AI自动移动
    [Serializable]
	[NodeMenuItem("效果/行为/AI自动移动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_AUTOMOVE : SkillEffectConfigNode
    {
		// AI自动移动
        public TSET_AI_AUTOMOVE() : base(TSkillEffectType.TSET_AI_AUTOMOVE) { }
    }
    #endregion SkillEffectConfig: AI自动移动


    #region SkillEffectConfig: AI设置界面显示状态
    [Serializable]
    public sealed partial class TSET_AI_SETUIVISIABLE : SkillEffectConfigNode
    {
		// AI设置界面显示状态
		// 参数0 : UILayer
		// 参数1 : 是否显示(0隐藏,1显示)
        public TSET_AI_SETUIVISIABLE() : base(TSkillEffectType.TSET_AI_SETUIVISIABLE) { }
    }
    #endregion SkillEffectConfig: AI设置界面显示状态


    #region SkillEffectConfig: AI修改单位属性
    [Serializable]
    public sealed partial class TSET_AI_MODIFYENTITYATTR : SkillEffectConfigNode
    {
		// AI修改单位属性
		// 参数0 : 目标修改属性单位
		// 参数1 : 属性类型
		// 参数2 : 属性值
        public TSET_AI_MODIFYENTITYATTR() : base(TSkillEffectType.TSET_AI_MODIFYENTITYATTR) { }
    }
    #endregion SkillEffectConfig: AI修改单位属性


    #region SkillEffectConfig: AI设置单位碰撞
    [Serializable]
    public sealed partial class TSET_AI_SETCOLLISIONSTATE : SkillEffectConfigNode
    {
		// AI设置单位碰撞
		// 参数0 : 目标设置碰撞单位
		// 参数1 : 未配置参数说明
        public TSET_AI_SETCOLLISIONSTATE() : base(TSkillEffectType.TSET_AI_SETCOLLISIONSTATE) { }
    }
    #endregion SkillEffectConfig: AI设置单位碰撞


    #region SkillEffectConfig: AI播放单位动作
    [Serializable]
    public sealed partial class TSET_AI_PLAYANIM : SkillEffectConfigNode
    {
		// AI播放单位动作
		// 参数0 : 目标播放单位
		// 参数1 : 动作插槽-TBattleNatureEnum
		// 参数2 : 动作类型-TRoleAnimType
        public TSET_AI_PLAYANIM() : base(TSkillEffectType.TSET_AI_PLAYANIM) { }
    }
    #endregion SkillEffectConfig: AI播放单位动作


    #region SkillEffectConfig: AI切换单位AI
    [Serializable]
	[NodeMenuItem("效果/单位/AI切换单位AI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_SWITCHAI : SkillEffectConfigNode
    {
		// AI切换单位AI
		// 参数0 : 目标切换单位
		// 参数1 : AI效果ID-SkillEffectConfig
        public TSET_AI_SWITCHAI() : base(TSkillEffectType.TSET_AI_SWITCHAI) { }
    }
    #endregion SkillEffectConfig: AI切换单位AI


    #region SkillEffectConfig: AI输出调试日志
    [Serializable]
	[NodeMenuItem("效果/UI/AI输出调试日志", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_DEBUGLOG : SkillEffectConfigNode
    {
		// AI输出调试日志
		// 参数0 : 未配置参数说明
        public TSET_AI_DEBUGLOG() : base(TSkillEffectType.TSET_AI_DEBUGLOG) { }
    }
    #endregion SkillEffectConfig: AI输出调试日志


    #region SkillEffectConfig: AI筛选单位
    [Serializable]
	[NodeMenuItem("效果/单位/AI筛选单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_GETSKILLSELECTRESULT : SkillEffectConfigNode
    {
		// AI筛选单位
		// 参数0 : 筛选ID-SkillSelectConfig
		// 参数1 : 筛选结果类型-TAITargetSelectType
        public TSET_AI_GETSKILLSELECTRESULT() : base(TSkillEffectType.TSET_AI_GETSKILLSELECTRESULT) { }
    }
    #endregion SkillEffectConfig: AI筛选单位


    #region SkillEffectConfig: AI获取单位属性
    [Serializable]
    public sealed partial class TSET_AI_GETENTITYATTR : SkillEffectConfigNode
    {
		// AI获取单位属性
		// 参数0 : 目标获取属性单位
		// 参数1 : 属性类型-TBattleNatureEnum
        public TSET_AI_GETENTITYATTR() : base(TSkillEffectType.TSET_AI_GETENTITYATTR) { }
    }
    #endregion SkillEffectConfig: AI获取单位属性


    #region SkillEffectConfig: 单位模型抖动
    [Serializable]
	[NodeMenuItem("技能效果/单位/单位模型抖动", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/单位模型抖动", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/单位模型抖动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITY_MODEL_SHAKE : SkillEffectConfigNode
    {
		// 单位模型抖动
		// 参数0 : 目标抖动单位
		// 参数1 : 抖动时间（毫秒）
		// 参数2 : 抖动幅度百分比
		// 参数3 : 抖动强度
		// 参数4 : 震动随机值（0-180）
        public TSET_ENTITY_MODEL_SHAKE() : base(TSkillEffectType.TSET_ENTITY_MODEL_SHAKE) { }
    }
    #endregion SkillEffectConfig: 单位模型抖动


    #region SkillEffectConfig: 获取常量数据配置值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取常量数据配置值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取常量数据配置值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取常量数据配置值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CONST_VALUE : SkillEffectConfigNode
    {
		// 获取常量数据配置值
		// 参数0 : 配置ID-ConstValueConfig
        public TSET_GET_CONST_VALUE() : base(TSkillEffectType.TSET_GET_CONST_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取常量数据配置值


    #region SkillEffectConfig: 通用显示层设置
    [Serializable]
	[NodeMenuItem("技能效果/UI/通用显示层设置", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/通用显示层设置", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/通用显示层设置", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_VIEW_OPTION : SkillEffectConfigNode
    {
		// 通用显示层设置
		// 参数0 : 显示层配置ID-BattleCustomParamConfig
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
        public TSET_VIEW_OPTION() : base(TSkillEffectType.TSET_VIEW_OPTION) { }
    }
    #endregion SkillEffectConfig: 通用显示层设置


    #region SkillEffectConfig: 设置游戏时长变化
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/设置游戏时长变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/设置游戏时长变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/设置游戏时长变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_LIMIT_TIME_CHANGE : SkillEffectConfigNode
    {
		// 设置游戏时长变化
		// 参数0 : 变化帧数
        public TSET_SET_LIMIT_TIME_CHANGE() : base(TSkillEffectType.TSET_SET_LIMIT_TIME_CHANGE) { }
    }
    #endregion SkillEffectConfig: 设置游戏时长变化


    #region SkillEffectConfig: 显示天气特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/显示天气特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/显示天气特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/显示天气特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_WEATHER_EFFECT : SkillEffectConfigNode
    {
		// 显示天气特效
		// 参数0 : 天气ID-WeatherConfig
		// 参数1 : 持续时间（暂时无效）
		// 参数2 : 关闭战斗效果执行（1关闭）
        public TSET_SHOW_WEATHER_EFFECT() : base(TSkillEffectType.TSET_SHOW_WEATHER_EFFECT) { }
    }
    #endregion SkillEffectConfig: 显示天气特效


    #region SkillEffectConfig: 隐藏天气特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/隐藏天气特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/隐藏天气特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/隐藏天气特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_HIDE_WEATHER_EFFECT : SkillEffectConfigNode
    {
		// 隐藏天气特效
		// 参数0 : 天气ID-WeatherConfig
        public TSET_HIDE_WEATHER_EFFECT() : base(TSkillEffectType.TSET_HIDE_WEATHER_EFFECT) { }
    }
    #endregion SkillEffectConfig: 隐藏天气特效


    #region SkillEffectConfig: 打印调试日志
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/打印调试日志", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/打印调试日志", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/打印调试日志", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LOG_INFO : SkillEffectConfigNode
    {
		// 打印调试日志
		// 参数0 : 打印值
		// 参数1 : 打印值
		// 参数2 : 打印值
		// 参数3 : 打印值
        public TSET_LOG_INFO() : base(TSkillEffectType.TSET_LOG_INFO) { }
    }
    #endregion SkillEffectConfig: 打印调试日志


    #region SkillEffectConfig: 获取子弹运动类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取子弹运动类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹运动类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹运动类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BULLET_FLY_TYPE : SkillEffectConfigNode
    {
		// 获取子弹运动类型
		// 参数0 : 子弹ID
        public TSET_GET_BULLET_FLY_TYPE() : base(TSkillEffectType.TSET_GET_BULLET_FLY_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取子弹运动类型


    #region SkillEffectConfig: 房间-创建房间传送门
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/房间-创建房间传送门", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-创建房间传送门", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-创建房间传送门", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_CREATE_TRANSPORT : SkillEffectConfigNode
    {
		// 房间-创建房间传送门
		// 参数0 : 房间配表ID[默认主体单位所在房间]-BattleRoomConfig
		// 参数1 : 房间内坐标X[厘米]
		// 参数2 : 房间内坐标Y[厘米]
		// 参数3 : 传送至的房间配表ID-BattleRoomConfig
		// 参数4 : 传送至入口位置（1_2_3_4=上_下_左_右）
		// 参数5 : 传送门模型ID-ModelConfig
        public TSET_ROOM_CREATE_TRANSPORT() : base(TSkillEffectType.TSET_ROOM_CREATE_TRANSPORT) { }
    }
    #endregion SkillEffectConfig: 房间-创建房间传送门


    #region SkillEffectConfig: 修改轨迹跟随形状参数
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改轨迹跟随形状参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改轨迹跟随形状参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改轨迹跟随形状参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MODIFY_BATTLE_FOLLOW_SHAPE_PARAM : SkillEffectConfigNode
    {
		// 修改轨迹跟随形状参数
		// 参数0 : 被修改参数单位
		// 参数1 : 形状宽
		// 参数2 : 形状高
		// 参数3 : 中心点偏移X
		// 参数4 : 中心点偏移Y
		// 参数5 : 特定旋转速度
		// 参数6 : 【必填】指定修改类型-TBattleFollowConfigFieldType
        public TSET_MODIFY_BATTLE_FOLLOW_SHAPE_PARAM() : base(TSkillEffectType.TSET_MODIFY_BATTLE_FOLLOW_SHAPE_PARAM) { }
    }
    #endregion SkillEffectConfig: 修改轨迹跟随形状参数


    #region SkillEffectConfig: 获取轨迹跟随形状宽
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取轨迹跟随形状宽", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状宽", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状宽", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_FOLLOW_SHAPE_WIDTH : SkillEffectConfigNode
    {
		// 获取轨迹跟随形状宽
		// 参数0 : 被获取参数单位
        public TSET_GET_BATTLE_FOLLOW_SHAPE_WIDTH() : base(TSkillEffectType.TSET_GET_BATTLE_FOLLOW_SHAPE_WIDTH) { }
    }
    #endregion SkillEffectConfig: 获取轨迹跟随形状宽


    #region SkillEffectConfig: 获取轨迹跟随形状高
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取轨迹跟随形状高", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状高", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状高", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_FOLLOW_SHAPE_HEIGHT : SkillEffectConfigNode
    {
		// 获取轨迹跟随形状高
		// 参数0 : 被获取参数单位
        public TSET_GET_BATTLE_FOLLOW_SHAPE_HEIGHT() : base(TSkillEffectType.TSET_GET_BATTLE_FOLLOW_SHAPE_HEIGHT) { }
    }
    #endregion SkillEffectConfig: 获取轨迹跟随形状高


    #region SkillEffectConfig: 获取轨迹跟随形状偏移X
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取轨迹跟随形状偏移X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状偏移X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状偏移X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_X : SkillEffectConfigNode
    {
		// 获取轨迹跟随形状偏移X
		// 参数0 : 被获取参数单位
        public TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_X() : base(TSkillEffectType.TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_X) { }
    }
    #endregion SkillEffectConfig: 获取轨迹跟随形状偏移X


    #region SkillEffectConfig: 获取轨迹跟随形状偏移Y
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取轨迹跟随形状偏移Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状偏移Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状偏移Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_Y : SkillEffectConfigNode
    {
		// 获取轨迹跟随形状偏移Y
		// 参数0 : 被获取参数单位
        public TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_Y() : base(TSkillEffectType.TSET_GET_BATTLE_FOLLOW_SHAPE_OFFSET_Y) { }
    }
    #endregion SkillEffectConfig: 获取轨迹跟随形状偏移Y


    #region SkillEffectConfig: 房间-控制出入口开启
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/房间-控制出入口开启", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-控制出入口开启", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-控制出入口开启", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_SET_DOOR_OPEN : SkillEffectConfigNode
    {
		// 房间-控制出入口开启
		// 参数0 : 房间配表ID[默认主体单位所在房间]-BattleRoomConfig
		// 参数1 : 入口位置[上_下_左_右=1_2_3_4]
		// 参数2 : 开启1_关闭0
        public TSET_ROOM_SET_DOOR_OPEN() : base(TSkillEffectType.TSET_ROOM_SET_DOOR_OPEN) { }
    }
    #endregion SkillEffectConfig: 房间-控制出入口开启


    #region SkillEffectConfig: 房间-获取房间原点坐标X
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/房间-获取房间原点坐标X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-获取房间原点坐标X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-获取房间原点坐标X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_GET_POS_X : SkillEffectConfigNode
    {
		// 房间-获取房间原点坐标X
		// 参数0 : 所在房间单位
        public TSET_ROOM_GET_POS_X() : base(TSkillEffectType.TSET_ROOM_GET_POS_X) { }
    }
    #endregion SkillEffectConfig: 房间-获取房间原点坐标X


    #region SkillEffectConfig: 房间-获取房间原点坐标Y
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/房间-获取房间原点坐标Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-获取房间原点坐标Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-获取房间原点坐标Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_GET_POS_Y : SkillEffectConfigNode
    {
		// 房间-获取房间原点坐标Y
		// 参数0 : 所在房间单位
        public TSET_ROOM_GET_POS_Y() : base(TSkillEffectType.TSET_ROOM_GET_POS_Y) { }
    }
    #endregion SkillEffectConfig: 房间-获取房间原点坐标Y


    #region SkillEffectConfig: 瞬间移动
    [Serializable]
	[NodeMenuItem("技能效果/单位/瞬间移动", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/瞬间移动", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/瞬间移动", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TELEPORT_POSITION : SkillEffectConfigNode
    {
		// 瞬间移动
		// 参数0 : 被瞬移单位
		// 参数1 : 方向类型-TSkillDirectionType
		// 参数2 : 偏移角度
		// 参数3 : 距离[厘米]
		// 参数4 : 是否穿越角色[1_0]
		// 参数5 : 是否穿越中间阻挡[1_0]
		// 参数6 : 落点在阻挡内是否移出[1_0]
		// 参数7 : 是否强制同步位置（无相机平滑）[1_0]
		// 参数8 : 无用参数
        public TSET_TELEPORT_POSITION() : base(TSkillEffectType.TSET_TELEPORT_POSITION) { }
    }
    #endregion SkillEffectConfig: 瞬间移动


    #region SkillEffectConfig: 修改表现层时间缩放
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/修改表现层时间缩放", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改表现层时间缩放", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改表现层时间缩放", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_VIEW_LAYER_TIME_SCALE : SkillEffectConfigNode
    {
		// 修改表现层时间缩放
		// 参数0 : 玩家ID（不填写默认当前玩家）
		// 参数1 : 时间缩放（百分）
		// 参数2 : 恢复时间（毫秒）
        public TSET_CHANGE_VIEW_LAYER_TIME_SCALE() : base(TSkillEffectType.TSET_CHANGE_VIEW_LAYER_TIME_SCALE) { }
    }
    #endregion SkillEffectConfig: 修改表现层时间缩放


    #region SkillEffectConfig: 修改场景表现参数
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/修改场景表现参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改场景表现参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改场景表现参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_VIEW_LAYER_ENVIROMENT_PARAM : SkillEffectConfigNode
    {
		// 修改场景表现参数
		// 参数0 : 颜色R（0-255）
		// 参数1 : 颜色G（0-255）
		// 参数2 : 颜色B（0-255）
		// 参数3 : 场景亮度（百分）
		// 参数4 : 变化时间
		// 参数5 : 持续时间
        public TSET_CHANGE_VIEW_LAYER_ENVIROMENT_PARAM() : base(TSkillEffectType.TSET_CHANGE_VIEW_LAYER_ENVIROMENT_PARAM) { }
    }
    #endregion SkillEffectConfig: 修改场景表现参数


    #region SkillEffectConfig: 房间-传送到指定房间
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/房间-传送到指定房间", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-传送到指定房间", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/房间-传送到指定房间", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_TRANSPORT : SkillEffectConfigNode
    {
		// 房间-传送到指定房间
		// 参数0 : 被传送单位
		// 参数1 : 是否包括队友[1是_0否]
		// 参数2 : 房间ID类型[1按房间Index_0按表ID]
		// 参数3 : 传送到房间ID[房间Index或表ID]
		// 参数4 : 是否传送到指定位置[1是_0否]
		// 参数5 : 传送到房间内坐标X[厘米]
		// 参数6 : 传送到房间内坐标Y[厘米]
        public TSET_ROOM_TRANSPORT() : base(TSkillEffectType.TSET_ROOM_TRANSPORT) { }
    }
    #endregion SkillEffectConfig: 房间-传送到指定房间


    #region SkillEffectConfig: 修改单位头像
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位头像", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位头像", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位头像", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_HEAD_ICON : SkillEffectConfigNode
    {
		// 修改单位头像
		// 参数0 : 被修改头像单位
		// 参数1 : 新头像战斗配置ID-BattleUnitConfig
        public TSET_CHANGE_ENTITY_HEAD_ICON() : base(TSkillEffectType.TSET_CHANGE_ENTITY_HEAD_ICON) { }
    }
    #endregion SkillEffectConfig: 修改单位头像


    #region SkillEffectConfig: 修改碰撞属性
    [Serializable]
    public sealed partial class TSET_CHANGE_FIXTURE_CONFIG_ATTR : SkillEffectConfigNode
    {
		// 修改碰撞属性
        public TSET_CHANGE_FIXTURE_CONFIG_ATTR() : base(TSkillEffectType.TSET_CHANGE_FIXTURE_CONFIG_ATTR) { }
    }
    #endregion SkillEffectConfig: 修改碰撞属性


    #region SkillEffectConfig: 修改单位碰撞额外偏移
    [Serializable]
    public sealed partial class TSET_CHANGE_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET : SkillEffectConfigNode
    {
		// 修改单位碰撞额外偏移
        public TSET_CHANGE_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET() : base(TSkillEffectType.TSET_CHANGE_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET) { }
    }
    #endregion SkillEffectConfig: 修改单位碰撞额外偏移


    #region SkillEffectConfig: 获取单位碰撞额外偏移X
    [Serializable]
    public sealed partial class TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_X : SkillEffectConfigNode
    {
		// 获取单位碰撞额外偏移X
        public TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_X() : base(TSkillEffectType.TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_X) { }
    }
    #endregion SkillEffectConfig: 获取单位碰撞额外偏移X


    #region SkillEffectConfig: 获取单位碰撞额外偏移Y
    [Serializable]
    public sealed partial class TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_Y : SkillEffectConfigNode
    {
		// 获取单位碰撞额外偏移Y
        public TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_Y() : base(TSkillEffectType.TSET_GET_ENTITY_FIXTURE_CONFIG_EXTRA_OFFSET_Y) { }
    }
    #endregion SkillEffectConfig: 获取单位碰撞额外偏移Y


    #region SkillEffectConfig: 能量球-生成能量球[按布局]
    [Serializable]
	[NodeMenuItem("技能效果/技能/能量球-生成能量球[按布局]", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-生成能量球[按布局]", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-生成能量球[按布局]", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENERGY_BALL_CREATE_LAYOUT : SkillEffectConfigNode
    {
		// 能量球-生成能量球[按布局]
		// 参数0 : 生成范围-中心坐标X[厘米]
		// 参数1 : 生成范围-中心坐标Y[厘米]
		// 参数2 : 能量球布局表ID-BattleEnergyBallLayoutConfig
        public TSET_ENERGY_BALL_CREATE_LAYOUT() : base(TSkillEffectType.TSET_ENERGY_BALL_CREATE_LAYOUT) { }
    }
    #endregion SkillEffectConfig: 能量球-生成能量球[按布局]


    #region SkillEffectConfig: 能量球-生成能量球[自定义随机]
    [Serializable]
	[NodeMenuItem("技能效果/技能/能量球-生成能量球[自定义随机]", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-生成能量球[自定义随机]", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-生成能量球[自定义随机]", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENERGY_BALL_CREATE_CUSTOM : SkillEffectConfigNode
    {
		// 能量球-生成能量球[自定义随机]
		// 参数0 : 生成范围-中心坐标X[厘米]
		// 参数1 : 生成范围-中心坐标Y[厘米]
		// 参数2 : 生成范围-矩形宽[厘米]
		// 参数3 : 生成范围-矩形高[厘米]
		// 参数4 : 生成范围-最小距离半径[厘米]
		// 参数5 : 能量球表ID-BattleEnergyBallConfig
		// 参数6 : 生成数量
		// 参数7 : 是否中心点飞溅动效[1是_0否]
        public TSET_ENERGY_BALL_CREATE_CUSTOM() : base(TSkillEffectType.TSET_ENERGY_BALL_CREATE_CUSTOM) { }
    }
    #endregion SkillEffectConfig: 能量球-生成能量球[自定义随机]


    #region SkillEffectConfig: 能量球-范围吸取能量球
    [Serializable]
	[NodeMenuItem("技能效果/技能/能量球-范围吸取能量球", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-范围吸取能量球", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/能量球-范围吸取能量球", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENERGY_BALL_EAT_IN_AREA : SkillEffectConfigNode
    {
		// 能量球-范围吸取能量球
		// 参数0 : 目标吸取单位
		// 参数1 : 吸取范围-中心坐标X[厘米]
		// 参数2 : 吸取范围-中心坐标Y[厘米]
		// 参数3 : 吸取范围-半径[厘米]
        public TSET_ENERGY_BALL_EAT_IN_AREA() : base(TSkillEffectType.TSET_ENERGY_BALL_EAT_IN_AREA) { }
    }
    #endregion SkillEffectConfig: 能量球-范围吸取能量球


    #region SkillEffectConfig: 设置战斗数据
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/设置战斗数据", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/设置战斗数据", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/设置战斗数据", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_DATA_SET : SkillEffectConfigNode
    {
		// 设置战斗数据
		// 参数0 : 战斗数据字段类型-TBattleDataType
		// 参数1 : 值
        public TSET_BATTLE_DATA_SET() : base(TSkillEffectType.TSET_BATTLE_DATA_SET) { }
    }
    #endregion SkillEffectConfig: 设置战斗数据


    #region SkillEffectConfig: 获取战斗数据
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/获取战斗数据", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取战斗数据", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取战斗数据", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_DATA_GET : SkillEffectConfigNode
    {
		// 获取战斗数据
		// 参数0 : 战斗数据字段类型-TBattleDataType
        public TSET_BATTLE_DATA_GET() : base(TSkillEffectType.TSET_BATTLE_DATA_GET) { }
    }
    #endregion SkillEffectConfig: 获取战斗数据


    #region SkillEffectConfig: 执行技能效果
    [Serializable]
	[NodeMenuItem("技能效果/技能/执行技能效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/执行技能效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/执行技能效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RUN_SKILL_EFFECT : SkillEffectConfigNode
    {
		// 执行技能效果
		// 参数0 : 执行者主体单位
		// 参数1 : 目标单位
		// 参数2 : 技能效果ID-SkillEffectConfig
        public TSET_RUN_SKILL_EFFECT() : base(TSkillEffectType.TSET_RUN_SKILL_EFFECT) { }
    }
    #endregion SkillEffectConfig: 执行技能效果


    #region SkillEffectConfig: 技能禁用
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能禁用", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能禁用", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能禁用", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_FORBIDDEN_ON : SkillEffectConfigNode
    {
		// 技能禁用
		// 参数0 : 被禁用单位
		// 参数1 : 是否技能结束时自动解禁
		// 参数2 : 禁用UI显示级别[禁用触控]-TSkillForbidenLevelType
		// 参数3 : 统一禁用帧数[填0统一不禁用]
		// 参数4 : 功法-禁用帧数[特值:0读统一帧数|-1不禁用]
		// 参数5 : 奇术1-禁用帧数[同上]
		// 参数6 : 奇术2-禁用帧数[同上]
		// 参数7 : 神通-禁用帧数[同上]
		// 参数8 : 位移-禁用帧数[同上]
		// 参数9 : 法宝-禁用帧数[同上]
		// 参数10 : 坐骑-禁用帧数[同上]
		// 参数11 : 法相-禁用帧数[同上]
		// 参数12 : 战斗道具-禁用帧数[同上]
		// 参数13 : 连携-禁用帧数[同上]
		// 参数14 : 神位-禁用帧数[同上]
		// 参数15 : 奇术3-禁用帧数[同上]
		// 参数16 : 化身技能-禁用帧数[同上]
		// 参数17 : 衍生技能1-禁用帧数[同上]
		// 参数18 : 衍生技能2-禁用帧数[同上]
		// 参数19 : 衍生技能3-禁用帧数[同上]
		// 参数20 : 识海技能-禁用帧数[同上]
        public TSET_SKILL_FORBIDDEN_ON() : base(TSkillEffectType.TSET_SKILL_FORBIDDEN_ON) { }
    }
    #endregion SkillEffectConfig: 技能禁用


    #region SkillEffectConfig: 技能解禁
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能解禁", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能解禁", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能解禁", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_FORBIDDEN_OFF : SkillEffectConfigNode
    {
		// 技能解禁
		// 参数0 : 被解禁单位
		// 参数1 : 功法-是否解禁
		// 参数2 : 奇术1-是否解禁
		// 参数3 : 奇术2-是否解禁
		// 参数4 : 神通-是否解禁
		// 参数5 : 位移-是否解禁
		// 参数6 : 法宝-是否解禁
		// 参数7 : 坐骑-是否解禁
		// 参数8 : 法相-是否解禁
		// 参数9 : 战斗道具-是否解禁
		// 参数10 : 连携-是否解禁
		// 参数11 : 神位-是否解禁
		// 参数12 : 奇术3-是否解禁
		// 参数13 : 化身-是否解禁
		// 参数14 : 衍生1-是否解禁
		// 参数15 : 衍生2-是否解禁
		// 参数16 : 衍生3-是否解禁
		// 参数17 : 识海-是否解禁
        public TSET_SKILL_FORBIDDEN_OFF() : base(TSkillEffectType.TSET_SKILL_FORBIDDEN_OFF) { }
    }
    #endregion SkillEffectConfig: 技能解禁


    #region SkillEffectConfig: 获取坐骑战斗角速度
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取坐骑战斗角速度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取坐骑战斗角速度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取坐骑战斗角速度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNT_ANGLESPEED : SkillEffectConfigNode
    {
		// 获取坐骑战斗角速度
		// 参数0 : 坐骑表ID
        public TSET_MOUNT_ANGLESPEED() : base(TSkillEffectType.TSET_MOUNT_ANGLESPEED) { }
    }
    #endregion SkillEffectConfig: 获取坐骑战斗角速度


    #region SkillEffectConfig: 获取战斗坐骑移速加成
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取战斗坐骑移速加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取战斗坐骑移速加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取战斗坐骑移速加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNT_MOUNTSPEEDPLUS : SkillEffectConfigNode
    {
		// 获取战斗坐骑移速加成
		// 参数0 : 坐骑表ID
        public TSET_MOUNT_MOUNTSPEEDPLUS() : base(TSkillEffectType.TSET_MOUNT_MOUNTSPEEDPLUS) { }
    }
    #endregion SkillEffectConfig: 获取战斗坐骑移速加成


    #region SkillEffectConfig: 获取坐骑御剑持续时间
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取坐骑御剑持续时间", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取坐骑御剑持续时间", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取坐骑御剑持续时间", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNT_MOUNDLASTTIME : SkillEffectConfigNode
    {
		// 获取坐骑御剑持续时间
		// 参数0 : 坐骑表ID
        public TSET_MOUNT_MOUNDLASTTIME() : base(TSkillEffectType.TSET_MOUNT_MOUNDLASTTIME) { }
    }
    #endregion SkillEffectConfig: 获取坐骑御剑持续时间


    #region SkillEffectConfig: 镜头偏移恢复
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头偏移恢复", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头偏移恢复", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头偏移恢复", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERAOFFSET_RECOVER : SkillEffectConfigNode
    {
		// 镜头偏移恢复
		// 参数0 : 目标单位 
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 帧数
		// 参数3 : 恢复跟随单位
		// 参数4 : 恢复偏移
		// 参数5 : 恢复深度
		// 参数6 : 参数占坑
		// 参数7 : 恢复俯仰角度
        public TSET_CAMERAOFFSET_RECOVER() : base(TSkillEffectType.TSET_CAMERAOFFSET_RECOVER) { }
    }
    #endregion SkillEffectConfig: 镜头偏移恢复


    #region SkillEffectConfig: 设置技能CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/设置技能CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SETSKILLCD : SkillEffectConfigNode
    {
		// 设置技能CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽-TSkillSlotType
		// 参数2 : 值
		// 参数3 : 是否进入CD
        public TSET_SETSKILLCD() : base(TSkillEffectType.TSET_SETSKILLCD) { }
    }
    #endregion SkillEffectConfig: 设置技能CD


    #region SkillEffectConfig: 获取坐骑技能CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取坐骑技能CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取坐骑技能CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取坐骑技能CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MOUNTGETSKILLCD : SkillEffectConfigNode
    {
		// 获取坐骑技能CD
		// 参数0 : 坐骑表ID
        public TSET_MOUNTGETSKILLCD() : base(TSkillEffectType.TSET_MOUNTGETSKILLCD) { }
    }
    #endregion SkillEffectConfig: 获取坐骑技能CD


    #region SkillEffectConfig: 获取战斗御剑耐力条类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取战斗御剑耐力条类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取战斗御剑耐力条类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取战斗御剑耐力条类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_HP_BAR_TYPE : SkillEffectConfigNode
    {
		// 获取战斗御剑耐力条类型
        public TSET_GET_MOUNT_HP_BAR_TYPE() : base(TSkillEffectType.TSET_GET_MOUNT_HP_BAR_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取战斗御剑耐力条类型


    #region SkillEffectConfig: 获取两点间距离
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取两点间距离", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两点间距离", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取两点间距离", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_DISTANCE_BETWEEN_POINT : SkillEffectConfigNode
    {
		// 获取两点间距离
		// 参数0 : 点A坐标_X
		// 参数1 : 点A坐标_Y
		// 参数2 : 点B坐标_X
		// 参数3 : 点B坐标_Y
        public TSET_GET_DISTANCE_BETWEEN_POINT() : base(TSkillEffectType.TSET_GET_DISTANCE_BETWEEN_POINT) { }
    }
    #endregion SkillEffectConfig: 获取两点间距离


    #region SkillEffectConfig: 数值运算-指数幂
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/常规运算/数值运算-指数幂", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算-指数幂", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算-指数幂", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_NUM_CALCULATE_POW : SkillEffectConfigNode
    {
		// 数值运算-指数幂
		// 参数0 : 幂的底数（a>0&&a!=1）
		// 参数1 : 幂的指数
        public TSET_NUM_CALCULATE_POW() : base(TSkillEffectType.TSET_NUM_CALCULATE_POW) { }
    }
    #endregion SkillEffectConfig: 数值运算-指数幂


    #region SkillEffectConfig: 数值运算-平方根万分比
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/常规运算/数值运算-平方根万分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算-平方根万分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/常规运算/数值运算-平方根万分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_NUM_CALCULATE_SQRT : SkillEffectConfigNode
    {
		// 数值运算-平方根万分比
		// 参数0 : 被开方数（>0）
        public TSET_NUM_CALCULATE_SQRT() : base(TSkillEffectType.TSET_NUM_CALCULATE_SQRT) { }
    }
    #endregion SkillEffectConfig: 数值运算-平方根万分比


    #region SkillEffectConfig: 论剑-获得随机宗门技能
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得随机宗门技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机宗门技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机宗门技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_SKILL_CARD : SkillEffectConfigNode
    {
		// 论剑-获得随机宗门技能
		// 参数0 : 技能子类型[不填:任意]-TBattleSkillSubType
		// 参数1 : 品质[不填:任意]
		// 参数2 : 等级
		// 参数3 : 数量
        public TSET_LJ_GAIN_SKILL_CARD() : base(TSkillEffectType.TSET_LJ_GAIN_SKILL_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得随机宗门技能


    #region SkillEffectConfig: 论剑-获得指定已装备技能
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得指定已装备技能", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得指定已装备技能", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得指定已装备技能", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_EQUIPED_SKILL_CARD : SkillEffectConfigNode
    {
		// 论剑-获得指定已装备技能
		// 参数0 : 技能子类型[不填:任意]-TBattleSkillSubType
		// 参数1 : 数量
        public TSET_LJ_GAIN_EQUIPED_SKILL_CARD() : base(TSkillEffectType.TSET_LJ_GAIN_EQUIPED_SKILL_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得指定已装备技能


    #region SkillEffectConfig: 论剑-获得随机法宝灵宝
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得随机法宝灵宝", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机法宝灵宝", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机法宝灵宝", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_LING_BAO_CARD : SkillEffectConfigNode
    {
		// 论剑-获得随机法宝灵宝
		// 参数0 : 类型[法宝0_灵宝1]
		// 参数1 : 品质[不填:任意]
		// 参数2 : 数量
        public TSET_LJ_GAIN_LING_BAO_CARD() : base(TSkillEffectType.TSET_LJ_GAIN_LING_BAO_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得随机法宝灵宝


    #region SkillEffectConfig: 论剑-获得灵石
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得灵石", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得灵石", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得灵石", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_GOLD : SkillEffectConfigNode
    {
		// 论剑-获得灵石
		// 参数0 : 数量
        public TSET_LJ_GAIN_GOLD() : base(TSkillEffectType.TSET_LJ_GAIN_GOLD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得灵石


    #region SkillEffectConfig: 论剑-获取当前灵石数量
    [Serializable]
	[NodeMenuItem("效果/论剑-获取当前灵石数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GET_GOLD : SkillEffectConfigNode
    {
		// 论剑-获取当前灵石数量
        public TSET_LJ_GET_GOLD() : base(TSkillEffectType.TSET_LJ_GET_GOLD) { }
    }
    #endregion SkillEffectConfig: 论剑-获取当前灵石数量


    #region SkillEffectConfig: 论剑-获得随机伙伴
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得随机伙伴", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机伙伴", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机伙伴", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_FRIEND_CARD : SkillEffectConfigNode
    {
		// 论剑-获得随机伙伴
		// 参数0 : 数量
        public TSET_LJ_GAIN_FRIEND_CARD() : base(TSkillEffectType.TSET_LJ_GAIN_FRIEND_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得随机伙伴


    #region SkillEffectConfig: 论剑-商店秘籍价格优惠
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-商店秘籍价格优惠", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-商店秘籍价格优惠", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-商店秘籍价格优惠", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_REDUCE_SHOP_BUY_CARD_COST : SkillEffectConfigNode
    {
		// 论剑-商店秘籍价格优惠
		// 参数0 : 减少值
        public TSET_LJ_REDUCE_SHOP_BUY_CARD_COST() : base(TSkillEffectType.TSET_LJ_REDUCE_SHOP_BUY_CARD_COST) { }
    }
    #endregion SkillEffectConfig: 论剑-商店秘籍价格优惠


    #region SkillEffectConfig: 论剑-商店免费刷新
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-商店免费刷新", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-商店免费刷新", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-商店免费刷新", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_FREE_REFRESH_SHOP_CARD : SkillEffectConfigNode
    {
		// 论剑-商店免费刷新
		// 参数0 : 次数
        public TSET_LJ_FREE_REFRESH_SHOP_CARD() : base(TSkillEffectType.TSET_LJ_FREE_REFRESH_SHOP_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-商店免费刷新


    #region SkillEffectConfig: 论剑-玩家血量恢复
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-玩家血量恢复", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家血量恢复", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家血量恢复", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_ADD_PLAYER_HP : SkillEffectConfigNode
    {
		// 论剑-玩家血量恢复
		// 参数0 : 恢复量
        public TSET_LJ_ADD_PLAYER_HP() : base(TSkillEffectType.TSET_LJ_ADD_PLAYER_HP) { }
    }
    #endregion SkillEffectConfig: 论剑-玩家血量恢复


    #region SkillEffectConfig: 论剑-玩家提升到某境界
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-玩家提升到某境界", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家提升到某境界", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家提升到某境界", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_PROMOTE_PLAYER_STATE : SkillEffectConfigNode
    {
		// 论剑-玩家提升到某境界
		// 参数0 : 境界
        public TSET_LJ_PROMOTE_PLAYER_STATE() : base(TSkillEffectType.TSET_LJ_PROMOTE_PLAYER_STATE) { }
    }
    #endregion SkillEffectConfig: 论剑-玩家提升到某境界


    #region SkillEffectConfig: 论剑-玩家服用九转还魂丹
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-玩家服用九转还魂丹", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家服用九转还魂丹", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-玩家服用九转还魂丹", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_RESIST_PLAYER_DIE : SkillEffectConfigNode
    {
		// 论剑-玩家服用九转还魂丹
		// 参数0 : 抵御致命伤害次数
        public TSET_LJ_RESIST_PLAYER_DIE() : base(TSkillEffectType.TSET_LJ_RESIST_PLAYER_DIE) { }
    }
    #endregion SkillEffectConfig: 论剑-玩家服用九转还魂丹


    #region SkillEffectConfig: 论剑-角色卡牌额外附加属性
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-角色卡牌额外附加属性", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-角色卡牌额外附加属性", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-角色卡牌额外附加属性", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_ADD_ROLE_CARD_EXTRA_ATTR : SkillEffectConfigNode
    {
		// 论剑-角色卡牌额外附加属性
		// 参数0 : 角色卡牌的战斗实例ID
		// 参数1 : 属性类型-TBattleNatureEnum
		// 参数2 : 额外附加值
        public TSET_LJ_ADD_ROLE_CARD_EXTRA_ATTR() : base(TSkillEffectType.TSET_LJ_ADD_ROLE_CARD_EXTRA_ATTR) { }
    }
    #endregion SkillEffectConfig: 论剑-角色卡牌额外附加属性


    #region SkillEffectConfig: 论剑-获取对决玩家ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/论剑-获取对决玩家ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/论剑-获取对决玩家ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/论剑-获取对决玩家ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GET_DUEL_PLAYER_INDEX : SkillEffectConfigNode
    {
		// 论剑-获取对决玩家ID
        public TSET_LJ_GET_DUEL_PLAYER_INDEX() : base(TSkillEffectType.TSET_LJ_GET_DUEL_PLAYER_INDEX) { }
    }
    #endregion SkillEffectConfig: 论剑-获取对决玩家ID


    #region SkillEffectConfig: 论剑-获取对决玩家阵营
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/论剑-获取对决玩家阵营", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/论剑-获取对决玩家阵营", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/论剑-获取对决玩家阵营", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GET_DUEL_PLAYER_CAMP : SkillEffectConfigNode
    {
		// 论剑-获取对决玩家阵营
        public TSET_LJ_GET_DUEL_PLAYER_CAMP() : base(TSkillEffectType.TSET_LJ_GET_DUEL_PLAYER_CAMP) { }
    }
    #endregion SkillEffectConfig: 论剑-获取对决玩家阵营


    #region SkillEffectConfig: 获取单位碰撞参数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取单位碰撞参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位碰撞参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取单位碰撞参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_FIXTURE_CONFIG_GETPARAM : SkillEffectConfigNode
    {
		// 获取单位碰撞参数
		// 参数0 : 单位实例ID
		// 参数1 : 反应层级-TCollisionLayer
		// 参数2 : 碰撞类型-TCollisionType
		// 参数3 : 碰撞子类型-TCollisionSubType
		// 参数4 : 获取参数索引:0=参数1,1=参数2,2=参数3...
        public TSET_GET_ENTITY_FIXTURE_CONFIG_GETPARAM() : base(TSkillEffectType.TSET_GET_ENTITY_FIXTURE_CONFIG_GETPARAM) { }
    }
    #endregion SkillEffectConfig: 获取单位碰撞参数


    #region SkillEffectConfig: 技能单位组_增加单位到组
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_增加单位到组", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_增加单位到组", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_增加单位到组", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_ADD_ID : SkillEffectConfigNode
    {
		// 技能单位组_增加单位到组
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 被加到单位组的单位实例ID
		// 参数2 : 单位组所属单位实例ID
		// 参数3 : 技能ID
        public TSET_ENTITYGROUP_ADD_ID() : base(TSkillEffectType.TSET_ENTITYGROUP_ADD_ID) { }
    }
    #endregion SkillEffectConfig: 技能单位组_增加单位到组


    #region SkillEffectConfig: 技能单位组_单位从组中移除
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_单位从组中移除", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_单位从组中移除", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_单位从组中移除", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_REMOVE_ID : SkillEffectConfigNode
    {
		// 技能单位组_单位从组中移除
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 被移除单位组的单位实例ID(0;移除最早加入的角色ID)
		// 参数2 : 是否销毁单位(0:不销毁,1:销毁,2:销毁-不走死亡)
		// 参数3 : 单位组所属单位实例ID
		// 参数4 : 技能ID
        public TSET_ENTITYGROUP_REMOVE_ID() : base(TSkillEffectType.TSET_ENTITYGROUP_REMOVE_ID) { }
    }
    #endregion SkillEffectConfig: 技能单位组_单位从组中移除


    #region SkillEffectConfig: 技能单位组_获得单位
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_获得单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获得单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获得单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_GET_ID : SkillEffectConfigNode
    {
		// 技能单位组_获得单位
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 单位下标
		// 参数2 : 单位组所属单位实例ID
		// 参数3 : 技能ID
        public TSET_ENTITYGROUP_GET_ID() : base(TSkillEffectType.TSET_ENTITYGROUP_GET_ID) { }
    }
    #endregion SkillEffectConfig: 技能单位组_获得单位


    #region SkillEffectConfig: 技能单位组_删除组
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_删除组", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_删除组", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_删除组", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_REMOVE_GROUP : SkillEffectConfigNode
    {
		// 技能单位组_删除组
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 是否销毁单位(0:不销毁,1:销毁,2:销毁-不走死亡)
		// 参数2 : 单位组所属单位实例ID
		// 参数3 : 技能ID
        public TSET_ENTITYGROUP_REMOVE_GROUP() : base(TSkillEffectType.TSET_ENTITYGROUP_REMOVE_GROUP) { }
    }
    #endregion SkillEffectConfig: 技能单位组_删除组


    #region SkillEffectConfig: 论剑-获得随机宗门心法
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获得随机宗门心法", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机宗门心法", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获得随机宗门心法", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GAIN_XIN_FA_CARD : SkillEffectConfigNode
    {
		// 论剑-获得随机宗门心法
		// 参数0 : 品质[不填:任意]
		// 参数1 : 等级[不填:任意]
		// 参数2 : 数量
        public TSET_LJ_GAIN_XIN_FA_CARD() : base(TSkillEffectType.TSET_LJ_GAIN_XIN_FA_CARD) { }
    }
    #endregion SkillEffectConfig: 论剑-获得随机宗门心法


    #region SkillEffectConfig: 创建掉落物[纯表现]
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/创建掉落物[纯表现]", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/创建掉落物[纯表现]", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/创建掉落物[纯表现]", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_DROP_ITEM : SkillEffectConfigNode
    {
		// 创建掉落物[纯表现]
		// 参数0 : 主体单位
		// 参数1 : 道具ID[不填:默认图标]
		// 参数2 : 自定义品质[不填:读道具品质]
		// 参数3 : 自定义掉落时长[帧]
		// 参数4 : 自定义等待时长[帧]
		// 参数5 : 自定义拾取时长[帧]
		// 参数6 : 仅指定单位可见
        public TSET_CREATE_DROP_ITEM() : base(TSkillEffectType.TSET_CREATE_DROP_ITEM) { }
    }
    #endregion SkillEffectConfig: 创建掉落物[纯表现]


    #region SkillEffectConfig: 论剑-获取玩家流程控制器实例ID
    [Serializable]
	[NodeMenuItem("技能效果/论剑玩法/论剑-获取玩家流程控制器实例ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获取玩家流程控制器实例ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/论剑玩法/论剑-获取玩家流程控制器实例ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LJ_GET_PLAYER_CONTROLLER : SkillEffectConfigNode
    {
		// 论剑-获取玩家流程控制器实例ID
		// 参数0 : 玩家ID
        public TSET_LJ_GET_PLAYER_CONTROLLER() : base(TSkillEffectType.TSET_LJ_GET_PLAYER_CONTROLLER) { }
    }
    #endregion SkillEffectConfig: 论剑-获取玩家流程控制器实例ID


    #region SkillEffectConfig: 获得战斗单位类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获得战斗单位类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获得战斗单位类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获得战斗单位类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_UNITTYPE : SkillEffectConfigNode
    {
		// 获得战斗单位类型
		// 参数0 : 目标单位
        public TSET_GET_BATTLE_UNITTYPE() : base(TSkillEffectType.TSET_GET_BATTLE_UNITTYPE) { }
    }
    #endregion SkillEffectConfig: 获得战斗单位类型


    #region SkillEffectConfig: AI技能躲避
    [Serializable]
	[NodeMenuItem("效果/单位/AI技能躲避", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_SKILL_ESCAPE : SkillEffectConfigNode
    {
		// AI技能躲避
		// 参数0 : 躲避技能标签组ID-BattleSkillTagGroupConfig
		// 参数1 : 躲避技能范围
		// 参数2 : 躲避角度
		// 参数3 : 触发远离距离
		// 参数4 : 最大加成远离角度
		// 参数5 : 是否只躲避地面子弹
        public TSET_AI_SKILL_ESCAPE() : base(TSkillEffectType.TSET_AI_SKILL_ESCAPE) { }
    }
    #endregion SkillEffectConfig: AI技能躲避


    #region SkillEffectConfig: 战斗界面操作引导
    [Serializable]
	[NodeMenuItem("技能效果/UI/战斗界面操作引导", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/战斗界面操作引导", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/战斗界面操作引导", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_UI_GUIDE : SkillEffectConfigNode
    {
		// 战斗界面操作引导
		// 参数0 : 引导配置ID-GuideConfig
		// 参数1 : 引导完成后执行技能效果-SkillEffectConfig
		// 参数2 : 是否不暂停游戏[0暂停|1不暂停]
        public TSET_BATTLE_UI_GUIDE() : base(TSkillEffectType.TSET_BATTLE_UI_GUIDE) { }
    }
    #endregion SkillEffectConfig: 战斗界面操作引导


    #region SkillEffectConfig: 模拟操作
    [Serializable]
	[NodeMenuItem("技能效果/单位/模拟操作", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/模拟操作", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/模拟操作", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_OPERATE_SIMULATION : SkillEffectConfigNode
    {
		// 模拟操作
		// 参数0 : 目标单位
		// 参数1 : 按钮类型-TSimulateButtonType
		// 参数2 : 按钮状态-TSkillButtonStage
		// 参数3 : 摇杆角度
		// 参数4 : 施法摇杆百分比
		// 参数5 : 施法指定目标ID
        public TSET_OPERATE_SIMULATION() : base(TSkillEffectType.TSET_OPERATE_SIMULATION) { }
    }
    #endregion SkillEffectConfig: 模拟操作


    #region SkillEffectConfig: 条件顺序执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/条件顺序执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/条件顺序执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/条件顺序执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CONDITIONORDER_EXECUTE : SkillEffectConfigNode
    {
		// 条件顺序执行
		// 参数0 : 技能效果ID-SkillEffectConfig
        public TSET_CONDITIONORDER_EXECUTE() : base(TSkillEffectType.TSET_CONDITIONORDER_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 条件顺序执行


    #region SkillEffectConfig: 计数条件执行
    [Serializable]
	[NodeMenuItem("效果/行为/计数条件执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CONDITION_COUNT : SkillEffectConfigNode
    {
		// 计数条件执行
		// 参数0 : 条件ID-SkillConditionConfig
		// 参数1 : 次数限制
		// 参数2 : 成功执行效果-SkillEffectConfig
		// 参数3 : 失败执行效果-SkillEffectConfig
        public TSET_CONDITION_COUNT() : base(TSkillEffectType.TSET_CONDITION_COUNT) { }
    }
    #endregion SkillEffectConfig: 计数条件执行


    #region SkillEffectConfig: 获取技能伤害类型
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能伤害类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能伤害类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能伤害类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_DAMAGE_TYPE : SkillEffectConfigNode
    {
		// 获取技能伤害类型
		// 参数0 : 技能ID-SkillConfig
        public TSET_GET_SKILL_DAMAGE_TYPE() : base(TSkillEffectType.TSET_GET_SKILL_DAMAGE_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取技能伤害类型


    #region SkillEffectConfig: 阵型创建子弹
    [Serializable]
	[NodeMenuItem("技能效果/阵型创建子弹", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/阵型创建子弹", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/阵型创建子弹", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_MULTI_BULLET_IN_SHAPE : SkillEffectConfigNode
    {
		// 阵型创建子弹
		// 参数0 : 子弹表ID-BulletConfig
		// 参数1 : 子弹数量
		// 参数2 : 形状类型-TShapeType
		// 参数3 : 形状参数1
		// 参数4 : 形状参数2
		// 参数5 : 形状参数3
		// 参数6 : 形状参数4
		// 参数7 : 形状参数5
		// 参数8 : 预创建延迟(帧)
		// 参数9 : 预创建后移动到分布位置时间(帧)
		// 参数10 : 激活延迟(帧)
		// 参数11 : 预创建位置X
		// 参数12 : 预创建位置Y
		// 参数13 : 分布位置偏移X
		// 参数14 : 分布位置偏移Y
		// 参数15 : 子弹角度
		// 参数16 : 是否是子子弹
		// 参数17 : 单位组-TSkillEntityGroupType
		// 参数18 : 形状角度
        public TSET_CREATE_MULTI_BULLET_IN_SHAPE() : base(TSkillEffectType.TSET_CREATE_MULTI_BULLET_IN_SHAPE) { }
    }
    #endregion SkillEffectConfig: 阵型创建子弹


    #region SkillEffectConfig: 获取技能目标单位ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能目标单位ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能目标单位ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能目标单位ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_LAST_TARGET_ID : SkillEffectConfigNode
    {
		// 获取技能目标单位ID
		// 参数0 : 目标单位
		// 参数1 : 技能ID
		// 参数2 : 是否通过技能槽位获得
		// 参数3 : 技能槽位-TSkillSlotType
        public TSET_GET_SKILL_LAST_TARGET_ID() : base(TSkillEffectType.TSET_GET_SKILL_LAST_TARGET_ID) { }
    }
    #endregion SkillEffectConfig: 获取技能目标单位ID


    #region SkillEffectConfig: 获取到目标单位射线碰撞位置X
    [Serializable]
    public sealed partial class TSET_GET_COLLIDE_ENTITY_POS_X : SkillEffectConfigNode
    {
		// 获取到目标单位射线碰撞位置X
		// 参数0 : 目标单位
		// 参数1 : 碰撞类型-TCollisionLayer
        public TSET_GET_COLLIDE_ENTITY_POS_X() : base(TSkillEffectType.TSET_GET_COLLIDE_ENTITY_POS_X) { }
    }
    #endregion SkillEffectConfig: 获取到目标单位射线碰撞位置X


    #region SkillEffectConfig: 获取到目标单位射线碰撞位置Y
    [Serializable]
    public sealed partial class TSET_GET_COLLIDE_ENTITY_POS_Y : SkillEffectConfigNode
    {
		// 获取到目标单位射线碰撞位置Y
		// 参数0 : 目标单位实例ID
		// 参数1 : 碰撞类型-TCollisionLayer
        public TSET_GET_COLLIDE_ENTITY_POS_Y() : base(TSkillEffectType.TSET_GET_COLLIDE_ENTITY_POS_Y) { }
    }
    #endregion SkillEffectConfig: 获取到目标单位射线碰撞位置Y


    #region SkillEffectConfig: 复活单位
    [Serializable]
	[NodeMenuItem("技能效果/单位/复活单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/复活单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/复活单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITY_REBORN : SkillEffectConfigNode
    {
		// 复活单位
		// 参数0 : 复活单位
		// 参数1 : 复活血量百分比
        public TSET_ENTITY_REBORN() : base(TSkillEffectType.TSET_ENTITY_REBORN) { }
    }
    #endregion SkillEffectConfig: 复活单位


    #region SkillEffectConfig: 播放音乐
    [Serializable]
	[NodeMenuItem("技能效果/音效/播放音乐", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/播放音乐", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/播放音乐", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PLAY_MUSIC : SkillEffectConfigNode
    {
		// 播放音乐
		// 参数0 : 播放选项(0:暂停|1:播放|2:重置音乐|3:切换激烈程度)
		// 参数1 : 音乐表格ID-VoiceConfig
		// 参数2 : 音乐激烈程度(选项3才生效）-LayoutConfig_TBattleWaveIntensity
        public TSET_PLAY_MUSIC() : base(TSkillEffectType.TSET_PLAY_MUSIC) { }
    }
    #endregion SkillEffectConfig: 播放音乐


    #region SkillEffectConfig: 获取怪物当前波次数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取怪物当前波次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取怪物当前波次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取怪物当前波次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CUR_MONSTER_WAVE_INDEX : SkillEffectConfigNode
    {
		// 获取怪物当前波次数
        public TSET_GET_CUR_MONSTER_WAVE_INDEX() : base(TSkillEffectType.TSET_GET_CUR_MONSTER_WAVE_INDEX) { }
    }
    #endregion SkillEffectConfig: 获取怪物当前波次数


    #region SkillEffectConfig: 获取怪物总波次数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取怪物总波次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取怪物总波次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取怪物总波次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_TOTAL_MONSTER_WAVE_INDEX : SkillEffectConfigNode
    {
		// 获取怪物总波次数
        public TSET_GET_TOTAL_MONSTER_WAVE_INDEX() : base(TSkillEffectType.TSET_GET_TOTAL_MONSTER_WAVE_INDEX) { }
    }
    #endregion SkillEffectConfig: 获取怪物总波次数


    #region SkillEffectConfig: 获取波次怪物剩余数量
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取波次怪物剩余数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取波次怪物剩余数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取波次怪物剩余数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_REMAIN_MONSTER_COUNT : SkillEffectConfigNode
    {
		// 获取波次怪物剩余数量
		// 参数0 : 单位类型-UnitType
        public TSET_GET_REMAIN_MONSTER_COUNT() : base(TSkillEffectType.TSET_GET_REMAIN_MONSTER_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取波次怪物剩余数量


    #region SkillEffectConfig: 切换AI
    [Serializable]
	[NodeMenuItem("技能效果/AI/切换AI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/AI/切换AI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/AI/切换AI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_AI : SkillEffectConfigNode
    {
		// 切换AI
		// 参数0 : 目标切换单位
		// 参数1 : AI表格ID-BattleAIConfig
        public TSET_CHANGE_AI() : base(TSkillEffectType.TSET_CHANGE_AI) { }
    }
    #endregion SkillEffectConfig: 切换AI


    #region SkillEffectConfig: 设置ai任务节点的状态为成功
    [Serializable]
	[NodeMenuItem("效果/技能/设置ai任务节点的状态为成功", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_AI_TASKNODE_SUCCESS : SkillEffectConfigNode
    {
		// 设置ai任务节点的状态为成功
		// 参数0 : 目标单位
		// 参数1 : ai任务节点Id-AITaskNodeConfig
        public TSET_SET_AI_TASKNODE_SUCCESS() : base(TSkillEffectType.TSET_SET_AI_TASKNODE_SUCCESS) { }
    }
    #endregion SkillEffectConfig: 设置ai任务节点的状态为成功


    #region SkillEffectConfig: 设置ai任务节点的状态为失败
    [Serializable]
	[NodeMenuItem("效果/技能/设置ai任务节点的状态为失败", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_AI_TASKNODE_FAIL : SkillEffectConfigNode
    {
		// 设置ai任务节点的状态为失败
		// 参数0 : 目标单位
		// 参数1 : ai任务节点Id-AITaskNodeConfig
        public TSET_SET_AI_TASKNODE_FAIL() : base(TSkillEffectType.TSET_SET_AI_TASKNODE_FAIL) { }
    }
    #endregion SkillEffectConfig: 设置ai任务节点的状态为失败


    #region SkillEffectConfig: 添加附加位置Z变化
    [Serializable]
	[NodeMenuItem("技能效果/单位/添加附加位置Z变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加附加位置Z变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加附加位置Z变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_POSZ_ADD : SkillEffectConfigNode
    {
		// 添加附加位置Z变化
		// 参数0 : 目标单位
		// 参数1 : 上升速度[厘米_帧]
		// 参数2 : 上升时间[帧]
		// 参数3 : 持续时间[帧]
		// 参数4 : 下降时间[帧]
        public TSET_CHANGE_POSZ_ADD() : base(TSkillEffectType.TSET_CHANGE_POSZ_ADD) { }
    }
    #endregion SkillEffectConfig: 添加附加位置Z变化


    #region SkillEffectConfig: 移除附加位置Z变化
    [Serializable]
	[NodeMenuItem("技能效果/单位/移除附加位置Z变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除附加位置Z变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除附加位置Z变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_POSZ_REMOVE : SkillEffectConfigNode
    {
		// 移除附加位置Z变化
		// 参数0 : 目标单位
		// 参数1 : 添加时的返回值Key
		// 参数2 : 是否保留下降过程
        public TSET_CHANGE_POSZ_REMOVE() : base(TSkillEffectType.TSET_CHANGE_POSZ_REMOVE) { }
    }
    #endregion SkillEffectConfig: 移除附加位置Z变化


    #region SkillEffectConfig: 获取AI参数
    [Serializable]
	[NodeMenuItem("效果/获取AI参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_AI_PARAM : SkillEffectConfigNode
    {
		// 获取AI参数
		// 参数0 : AI拥有者单位
		// 参数1 : 技能参数ID-SkillTagsConfig
		// 参数2 : 是否获取最终值（按照Tag表配置效果）
        public TSET_GET_AI_PARAM() : base(TSkillEffectType.TSET_GET_AI_PARAM) { }
    }
    #endregion SkillEffectConfig: 获取AI参数


    #region SkillEffectConfig: 设置AI参数
    [Serializable]
	[NodeMenuItem("效果/设置AI参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_AI_PARAM : SkillEffectConfigNode
    {
		// 设置AI参数
		// 参数0 : AI拥有者单位
		// 参数1 : 技能参数ID-SkillTagsConfig
		// 参数2 : 参数值
        public TSET_SET_AI_PARAM() : base(TSkillEffectType.TSET_SET_AI_PARAM) { }
    }
    #endregion SkillEffectConfig: 设置AI参数


    #region SkillEffectConfig: 设置技能连招当前段数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/设置技能连招当前段数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/设置技能连招当前段数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/设置技能连招当前段数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_SKILL_COMBO_INDEX : SkillEffectConfigNode
    {
		// 设置技能连招当前段数
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 当前阶段数
        public TSET_SET_SKILL_COMBO_INDEX() : base(TSkillEffectType.TSET_SET_SKILL_COMBO_INDEX) { }
    }
    #endregion SkillEffectConfig: 设置技能连招当前段数


    #region SkillEffectConfig: 获取技能连招当前段数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能连招当前段数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能连招当前段数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能连招当前段数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_COMBO_INDEX : SkillEffectConfigNode
    {
		// 获取技能连招当前段数
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
        public TSET_GET_SKILL_COMBO_INDEX() : base(TSkillEffectType.TSET_GET_SKILL_COMBO_INDEX) { }
    }
    #endregion SkillEffectConfig: 获取技能连招当前段数


    #region SkillEffectConfig: 获取单位自身碰撞配置的位置X
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位自身碰撞配置的位置X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的位置X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的位置X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_FIXTURE_CENTER_X : SkillEffectConfigNode
    {
		// 获取单位自身碰撞配置的位置X
		// 参数0 : 单位实体ID
		// 参数1 : 出手点类型-TFixtureAttackPosFlag-TFixtureAttackPosFlag
        public TSET_GET_FIXTURE_CENTER_X() : base(TSkillEffectType.TSET_GET_FIXTURE_CENTER_X) { }
    }
    #endregion SkillEffectConfig: 获取单位自身碰撞配置的位置X


    #region SkillEffectConfig: 获取单位自身碰撞配置的位置Y
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位自身碰撞配置的位置Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的位置Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的位置Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_FIXTURE_CENTER_Y : SkillEffectConfigNode
    {
		// 获取单位自身碰撞配置的位置Y
		// 参数0 : 单位实体ID
		// 参数1 : 出手点类型-TFixtureAttackPosFlag-TFixtureAttackPosFlag
        public TSET_GET_FIXTURE_CENTER_Y() : base(TSkillEffectType.TSET_GET_FIXTURE_CENTER_Y) { }
    }
    #endregion SkillEffectConfig: 获取单位自身碰撞配置的位置Y


    #region SkillEffectConfig: 添加护盾buff
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/添加护盾buff", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/添加护盾buff", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/添加护盾buff", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_SHIELD_BUFF : SkillEffectConfigNode
    {
		// 添加护盾buff
		// 参数0 : 挂载Buff目标单位
		// 参数1 : Buff来源单位
		// 参数2 : 护盾BuffID-BuffConfig
		// 参数3 : 持续时间(帧数)
		// 参数4 : 护盾类型-TBattleShieldType
		// 参数5 : 护盾值(破盾后移除buff)
        public TSET_ADD_SHIELD_BUFF() : base(TSkillEffectType.TSET_ADD_SHIELD_BUFF) { }
    }
    #endregion SkillEffectConfig: 添加护盾buff


    #region SkillEffectConfig: 获取轨迹跟随形状参数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取轨迹跟随形状参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取轨迹跟随形状参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_FOLLOW_SHAPE_PARAM : SkillEffectConfigNode
    {
		// 获取轨迹跟随形状参数
		// 参数0 : 被获取参数单位
		// 参数1 : 参数类型-TBattleFollowConfigFieldType
		// 参数2 : 是否获取表格数据（0:否1:是）
        public TSET_GET_BATTLE_FOLLOW_SHAPE_PARAM() : base(TSkillEffectType.TSET_GET_BATTLE_FOLLOW_SHAPE_PARAM) { }
    }
    #endregion SkillEffectConfig: 获取轨迹跟随形状参数


    #region SkillEffectConfig: 获取坐骑的战斗单位表ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取坐骑的战斗单位表ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑的战斗单位表ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑的战斗单位表ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_BATTLE_UNIT_CONFIG_ID : SkillEffectConfigNode
    {
		// 获取坐骑的战斗单位表ID
		// 参数0 : 所属单位
        public TSET_GET_MOUNT_BATTLE_UNIT_CONFIG_ID() : base(TSkillEffectType.TSET_GET_MOUNT_BATTLE_UNIT_CONFIG_ID) { }
    }
    #endregion SkillEffectConfig: 获取坐骑的战斗单位表ID


    #region SkillEffectConfig: 添加单位范围触发器
    [Serializable]
	[NodeMenuItem("技能效果/单位/添加单位范围触发器", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加单位范围触发器", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加单位范围触发器", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ENTITY_RANGE_TRIGGER_EVENT : SkillEffectConfigNode
    {
		// 添加单位范围触发器
		// 参数0 : 持续检测时间[帧](-1永久)
		// 参数1 : 持续检测范围筛选-SkillSelectConfig
		// 参数2 : 筛选目标首次进入时效果-SkillEffectConfig
		// 参数3 : 筛选目标离开时效果-SkillEffectConfig
		// 参数4 : 触发器添加时效果-SkillEffectConfig
		// 参数5 : 触发器结束(移除)时效果-SkillEffectConfig
		// 参数6 : 触发器显示特效ID-ModelConfig
		// 参数7 : 触发器显示特效缩放百分比
		// 参数8 : 触发器显示特效朝向[角度]
		// 参数9 : 筛选目标停留时效果[注意每帧会执行]-SkillEffectConfig
        public TSET_ADD_ENTITY_RANGE_TRIGGER_EVENT() : base(TSkillEffectType.TSET_ADD_ENTITY_RANGE_TRIGGER_EVENT) { }
    }
    #endregion SkillEffectConfig: 添加单位范围触发器


    #region SkillEffectConfig: 移除单位范围触发器
    [Serializable]
	[NodeMenuItem("技能效果/单位/移除单位范围触发器", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除单位范围触发器", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除单位范围触发器", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REMOVE_ENTITY_RANGE_TRIGGER_EVENT : SkillEffectConfigNode
    {
		// 移除单位范围触发器
		// 参数0 : 按添加时的返回值Key
        public TSET_REMOVE_ENTITY_RANGE_TRIGGER_EVENT() : base(TSkillEffectType.TSET_REMOVE_ENTITY_RANGE_TRIGGER_EVENT) { }
    }
    #endregion SkillEffectConfig: 移除单位范围触发器


    #region SkillEffectConfig: 创建线形预警圈
    [Serializable]
	[NodeMenuItem("技能效果/技能/创建线形预警圈", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建线形预警圈", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建线形预警圈", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_WARNING_LINE : SkillEffectConfigNode
    {
		// 创建线形预警圈
		// 参数0 : 创建者实例ID
		// 参数1 : 跟随目标实例ID
		// 参数2 : 指示器资源ID
		// 参数3 : 坐标X
		// 参数4 : 坐标Y
		// 参数5 : 面向
		// 参数6 : 线形宽度
		// 参数7 : 线形长度
		// 参数8 : 结束前闪烁帧数
		// 参数9 : 持续帧数
		// 参数10 : 是否跟随朝向（0不跟随_1跟随）
		// 参数11 : 加入单位组：-TSkillEntityGroupType
		// 参数12 : 高度Z是否跟随
		// 参数13 : 位置偏移_右侧
		// 参数14 : 位置偏移_面前
        public TSET_CREATE_WARNING_LINE() : base(TSkillEffectType.TSET_CREATE_WARNING_LINE) { }
    }
    #endregion SkillEffectConfig: 创建线形预警圈


    #region SkillEffectConfig: 创建矩形预警圈
    [Serializable]
	[NodeMenuItem("技能效果/技能/创建矩形预警圈", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建矩形预警圈", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建矩形预警圈", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_WARNING_RECTANGLE : SkillEffectConfigNode
    {
		// 创建矩形预警圈
		// 参数0 : 创建者实例ID
		// 参数1 : 跟随目标实例ID
		// 参数2 : 指示器资源ID
		// 参数3 : 坐标X
		// 参数4 : 坐标Y
		// 参数5 : 面向
		// 参数6 : 矩形宽度
		// 参数7 : 矩形长度
		// 参数8 : 结束前闪烁帧数
		// 参数9 : 持续帧数
		// 参数10 : 是否跟随朝向
		// 参数11 : 加入单位组：-TSkillEntityGroupType
		// 参数12 : 高度Z是否跟随
		// 参数13 : 位置偏移_右侧
		// 参数14 : 位置偏移_面前
        public TSET_CREATE_WARNING_RECTANGLE() : base(TSkillEffectType.TSET_CREATE_WARNING_RECTANGLE) { }
    }
    #endregion SkillEffectConfig: 创建矩形预警圈


    #region SkillEffectConfig: 设置子弹目标坐标
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置子弹目标坐标", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置子弹目标坐标", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置子弹目标坐标", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_BULLET_TARGET_POS : SkillEffectConfigNode
    {
		// 设置子弹目标坐标
		// 参数0 : 子弹实例ID
		// 参数1 : 目标坐标_X
		// 参数2 : 目标坐标_Y
		// 参数3 : 是否到达目标销毁（0否，1是）
        public TSET_SET_BULLET_TARGET_POS() : base(TSkillEffectType.TSET_SET_BULLET_TARGET_POS) { }
    }
    #endregion SkillEffectConfig: 设置子弹目标坐标


    #region SkillEffectConfig: 技能单位组_获取单位数量
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_获取单位数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获取单位数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获取单位数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_SIZE : SkillEffectConfigNode
    {
		// 技能单位组_获取单位数量
		// 参数0 : 单位组类型-TSkillEntityGroupType
		// 参数1 : 单位组所属单位实例ID
		// 参数2 : 技能ID
		// 参数3 : 指定单位类型(0全部)-UnitType
        public TSET_ENTITYGROUP_SIZE() : base(TSkillEffectType.TSET_ENTITYGROUP_SIZE) { }
    }
    #endregion SkillEffectConfig: 技能单位组_获取单位数量


    #region SkillEffectConfig: 强制移动至目标点
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/强制移动至目标点", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/强制移动至目标点", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/强制移动至目标点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_FORCE_MOVE_TO_TARGET_POINT : SkillEffectConfigNode
    {
		// 强制移动至目标点
		// 参数0 : 单位实例ID
		// 参数1 : 目标位置X[厘米]
		// 参数2 : 目标位置Y[厘米]
		// 参数3 : 移动持续时间[帧]
        public TSET_FORCE_MOVE_TO_TARGET_POINT() : base(TSkillEffectType.TSET_FORCE_MOVE_TO_TARGET_POINT) { }
    }
    #endregion SkillEffectConfig: 强制移动至目标点


    #region SkillEffectConfig: 获取最近可行走位置点X
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/获取最近可行走位置点X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取最近可行走位置点X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取最近可行走位置点X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_FIND_NEAREST_WALKABLE_POINT_X : SkillEffectConfigNode
    {
		// 获取最近可行走位置点X
		// 参数0 : 单位实例ID
        public TSET_FIND_NEAREST_WALKABLE_POINT_X() : base(TSkillEffectType.TSET_FIND_NEAREST_WALKABLE_POINT_X) { }
    }
    #endregion SkillEffectConfig: 获取最近可行走位置点X


    #region SkillEffectConfig: 获取最近可行走位置点Y
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/获取最近可行走位置点Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取最近可行走位置点Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取最近可行走位置点Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_FIND_NEAREST_WALKABLE_POINT_Y : SkillEffectConfigNode
    {
		// 获取最近可行走位置点Y
		// 参数0 : 单位实例ID
        public TSET_FIND_NEAREST_WALKABLE_POINT_Y() : base(TSkillEffectType.TSET_FIND_NEAREST_WALKABLE_POINT_Y) { }
    }
    #endregion SkillEffectConfig: 获取最近可行走位置点Y


    #region SkillEffectConfig: 移除单位AI
    [Serializable]
	[NodeMenuItem("技能效果/单位/移除单位AI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除单位AI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/移除单位AI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REMOVE_ENTITY_AI : SkillEffectConfigNode
    {
		// 移除单位AI
		// 参数0 : 单位实例ID
		// 参数1 : 移除(0)_加回(1)
        public TSET_REMOVE_ENTITY_AI() : base(TSkillEffectType.TSET_REMOVE_ENTITY_AI) { }
    }
    #endregion SkillEffectConfig: 移除单位AI


    #region SkillEffectConfig: 添加捕获宠物
    [Serializable]
	[NodeMenuItem("技能效果/单位/添加捕获宠物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加捕获宠物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加捕获宠物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ACCEPTED_PET : SkillEffectConfigNode
    {
		// 添加捕获宠物
		// 参数0 : 抓捕单位实例ID
		// 参数1 : 被抓捕单位实例ID
		// 参数2 : 是否抓捕成功,1:是成功,0是失败
        public TSET_ADD_ACCEPTED_PET() : base(TSkillEffectType.TSET_ADD_ACCEPTED_PET) { }
    }
    #endregion SkillEffectConfig: 添加捕获宠物


    #region SkillEffectConfig: 修改技能冷却类型
    [Serializable]
	[NodeMenuItem("技能效果/技能/修改技能冷却类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能冷却类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能冷却类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_SKILL_CD_TYPE : SkillEffectConfigNode
    {
		// 修改技能冷却类型
		// 参数0 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数1 : 目标冷却类型-TSkillColdType
		// 参数2 : 冷却时间(0:读取原有配置)
		// 参数3 : 充能次数
		// 参数4 : 充能冷却时间
        public TSET_CHANGE_SKILL_CD_TYPE() : base(TSkillEffectType.TSET_CHANGE_SKILL_CD_TYPE) { }
    }
    #endregion SkillEffectConfig: 修改技能冷却类型


    #region SkillEffectConfig: 获取子弹的子弹表ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取子弹的子弹表ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹的子弹表ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹的子弹表ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BULLET_CONFIG_ID : SkillEffectConfigNode
    {
		// 获取子弹的子弹表ID
		// 参数0 : 被获取参数的子弹单位
        public TSET_GET_BULLET_CONFIG_ID() : base(TSkillEffectType.TSET_GET_BULLET_CONFIG_ID) { }
    }
    #endregion SkillEffectConfig: 获取子弹的子弹表ID


    #region SkillEffectConfig: 获取子弹初始技能ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取子弹初始技能ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹初始技能ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹初始技能ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BULLET_ORIGIN_SKILL_ID : SkillEffectConfigNode
    {
		// 获取子弹初始技能ID
		// 参数0 : 被获取参数的子弹单位
        public TSET_GET_BULLET_ORIGIN_SKILL_ID() : base(TSkillEffectType.TSET_GET_BULLET_ORIGIN_SKILL_ID) { }
    }
    #endregion SkillEffectConfig: 获取子弹初始技能ID


    #region SkillEffectConfig: 获取子弹初始技能实例ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取子弹初始技能实例ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹初始技能实例ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取子弹初始技能实例ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BULLET_ORIGIN_SKILL_INST_ID : SkillEffectConfigNode
    {
		// 获取子弹初始技能实例ID
		// 参数0 : 被获取参数的子弹单位
        public TSET_GET_BULLET_ORIGIN_SKILL_INST_ID() : base(TSkillEffectType.TSET_GET_BULLET_ORIGIN_SKILL_INST_ID) { }
    }
    #endregion SkillEffectConfig: 获取子弹初始技能实例ID


    #region SkillEffectConfig: 获取坐骑表的骑乘者高度偏移
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取坐骑表的骑乘者高度偏移", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑表的骑乘者高度偏移", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑表的骑乘者高度偏移", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_CONFIG_RIDER_OFFSET_Y : SkillEffectConfigNode
    {
		// 获取坐骑表的骑乘者高度偏移
		// 参数0 : 坐骑表ID
        public TSET_GET_MOUNT_CONFIG_RIDER_OFFSET_Y() : base(TSkillEffectType.TSET_GET_MOUNT_CONFIG_RIDER_OFFSET_Y) { }
    }
    #endregion SkillEffectConfig: 获取坐骑表的骑乘者高度偏移


    #region SkillEffectConfig: 获取坐骑表的坐骑高度偏移
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取坐骑表的坐骑高度偏移", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑表的坐骑高度偏移", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取坐骑表的坐骑高度偏移", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MOUNT_CONFIG_MOUNT_OFFSET_Y : SkillEffectConfigNode
    {
		// 获取坐骑表的坐骑高度偏移
		// 参数0 : 坐骑表ID
        public TSET_GET_MOUNT_CONFIG_MOUNT_OFFSET_Y() : base(TSkillEffectType.TSET_GET_MOUNT_CONFIG_MOUNT_OFFSET_Y) { }
    }
    #endregion SkillEffectConfig: 获取坐骑表的坐骑高度偏移


    #region SkillEffectConfig: 增加拥有的Buff持续时间(单次)
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/增加拥有的Buff持续时间(单次)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/增加拥有的Buff持续时间(单次)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/增加拥有的Buff持续时间(单次)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BUFF_DURATION : SkillEffectConfigNode
    {
		// 增加拥有的Buff持续时间(单次)
		// 参数0 : 单位实例ID
		// 参数1 : BuffID
		// 参数2 : 增加帧数
		// 参数3 : Buff来源单位实例ID（0不区分）
        public TSET_ADD_BUFF_DURATION() : base(TSkillEffectType.TSET_ADD_BUFF_DURATION) { }
    }
    #endregion SkillEffectConfig: 增加拥有的Buff持续时间(单次)


    #region SkillEffectConfig: 千层塔-获取当前章节ID
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取当前章节ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前章节ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前章节ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_CUR_CHAPTER_ID : SkillEffectConfigNode
    {
		// 千层塔-获取当前章节ID
        public TSET_CHAPTER_GET_CUR_CHAPTER_ID() : base(TSkillEffectType.TSET_CHAPTER_GET_CUR_CHAPTER_ID) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取当前章节ID


    #region SkillEffectConfig: 千层塔-获取当前层数ID
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取当前层数ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前层数ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前层数ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_CUR_LEVEL_ID : SkillEffectConfigNode
    {
		// 千层塔-获取当前层数ID
        public TSET_CHAPTER_GET_CUR_LEVEL_ID() : base(TSkillEffectType.TSET_CHAPTER_GET_CUR_LEVEL_ID) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取当前层数ID


    #region SkillEffectConfig: 千层塔-获取当前章节总层数
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取当前章节总层数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前章节总层数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前章节总层数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_CUR_CHAPTER_TOTAL_LEVEL_COUNT : SkillEffectConfigNode
    {
		// 千层塔-获取当前章节总层数
        public TSET_CHAPTER_GET_CUR_CHAPTER_TOTAL_LEVEL_COUNT() : base(TSkillEffectType.TSET_CHAPTER_GET_CUR_CHAPTER_TOTAL_LEVEL_COUNT) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取当前章节总层数


    #region SkillEffectConfig: 千层塔-获取累计已通关的层数数量
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取累计已通关的层数数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取累计已通关的层数数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取累计已通关的层数数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_TOTAL_CLEARANCE_LEVEL_COUNT : SkillEffectConfigNode
    {
		// 千层塔-获取累计已通关的层数数量
        public TSET_CHAPTER_GET_TOTAL_CLEARANCE_LEVEL_COUNT() : base(TSkillEffectType.TSET_CHAPTER_GET_TOTAL_CLEARANCE_LEVEL_COUNT) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取累计已通关的层数数量


    #region SkillEffectConfig: 千层塔-进入下一层战斗
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-进入下一层战斗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-进入下一层战斗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-进入下一层战斗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_ENTER_NEXT_LEVEL_BATTLE : SkillEffectConfigNode
    {
		// 千层塔-进入下一层战斗
        public TSET_CHAPTER_ENTER_NEXT_LEVEL_BATTLE() : base(TSkillEffectType.TSET_CHAPTER_ENTER_NEXT_LEVEL_BATTLE) { }
    }
    #endregion SkillEffectConfig: 千层塔-进入下一层战斗


    #region SkillEffectConfig: 获取技能槽位类型
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能槽位类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能槽位类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能槽位类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_SLOT_TYPE : SkillEffectConfigNode
    {
		// 获取技能槽位类型
		// 参数0 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数1 : 单位实例ID（按表格ID获取时需要配置，0：默认主体单位）
        public TSET_GET_SKILL_SLOT_TYPE() : base(TSkillEffectType.TSET_GET_SKILL_SLOT_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取技能槽位类型


    #region SkillEffectConfig: 获取单位子类型
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位子类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位子类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位子类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_SUB_TYPE : SkillEffectConfigNode
    {
		// 获取单位子类型
		// 参数0 : 被获取目标单位
        public TSET_GET_ENTITY_SUB_TYPE() : base(TSkillEffectType.TSET_GET_ENTITY_SUB_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取单位子类型


    #region SkillEffectConfig: 获取随机怪物战斗单位
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取随机怪物战斗单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取随机怪物战斗单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取随机怪物战斗单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_RANDOM_MONSTER_BATTLE_UNIT : SkillEffectConfigNode
    {
		// 获取随机怪物战斗单位
		// 参数0 : 种族类型-TRaceType
		// 参数1 : 种族子类型-TRaceSubType
		// 参数2 : 最低单位类型(闭区间)-UnitType
		// 参数3 : 最高单位类型(闭区间)-UnitType
		// 参数4 : 五行-TElementsType
		// 参数5 : 最低境界
		// 参数6 : 最高境界
        public TSET_GET_RANDOM_MONSTER_BATTLE_UNIT() : base(TSkillEffectType.TSET_GET_RANDOM_MONSTER_BATTLE_UNIT) { }
    }
    #endregion SkillEffectConfig: 获取随机怪物战斗单位


    #region SkillEffectConfig: 继承心法
    [Serializable]
	[NodeMenuItem("技能效果/单位/继承心法", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承心法", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承心法", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_XIN_FA : SkillEffectConfigNode
    {
		// 继承心法
		// 参数0 : 继承者单位实例ID
		// 参数1 : 传承者单位实例ID
        public TSET_COPY_ENTITY_XIN_FA() : base(TSkillEffectType.TSET_COPY_ENTITY_XIN_FA) { }
    }
    #endregion SkillEffectConfig: 继承心法


    #region SkillEffectConfig: 继承法器
    [Serializable]
	[NodeMenuItem("技能效果/单位/继承法器", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承法器", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承法器", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_EQUIP_SPECIAL : SkillEffectConfigNode
    {
		// 继承法器
		// 参数0 : 继承者单位实例ID
		// 参数1 : 传承者单位实例ID
        public TSET_COPY_ENTITY_EQUIP_SPECIAL() : base(TSkillEffectType.TSET_COPY_ENTITY_EQUIP_SPECIAL) { }
    }
    #endregion SkillEffectConfig: 继承法器


    #region SkillEffectConfig: 继承本命法宝
    [Serializable]
	[NodeMenuItem("技能效果/单位/继承本命法宝", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承本命法宝", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/继承本命法宝", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_ORIGIN_TREASURE : SkillEffectConfigNode
    {
		// 继承本命法宝
		// 参数0 : 继承者单位实例ID
		// 参数1 : 传承者单位实例ID
        public TSET_COPY_ENTITY_ORIGIN_TREASURE() : base(TSkillEffectType.TSET_COPY_ENTITY_ORIGIN_TREASURE) { }
    }
    #endregion SkillEffectConfig: 继承本命法宝


    #region SkillEffectConfig: BOSS出场秀表演进行中
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/BOSS出场秀表演进行中", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/BOSS出场秀表演进行中", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/BOSS出场秀表演进行中", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BOSS_BORN_SHOW_START : SkillEffectConfigNode
    {
		// BOSS出场秀表演进行中
        public TSET_BOSS_BORN_SHOW_START() : base(TSkillEffectType.TSET_BOSS_BORN_SHOW_START) { }
    }
    #endregion SkillEffectConfig: BOSS出场秀表演进行中


    #region SkillEffectConfig: BOSS出场秀表演结束
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/BOSS出场秀表演结束", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/BOSS出场秀表演结束", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/BOSS出场秀表演结束", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BOSS_BORN_SHOW_END : SkillEffectConfigNode
    {
		// BOSS出场秀表演结束
        public TSET_BOSS_BORN_SHOW_END() : base(TSkillEffectType.TSET_BOSS_BORN_SHOW_END) { }
    }
    #endregion SkillEffectConfig: BOSS出场秀表演结束


    #region SkillEffectConfig: 战斗任务-强制结束当前任务组
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/战斗任务-强制结束当前任务组", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-强制结束当前任务组", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-强制结束当前任务组", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_MISSION_FORCE_END_CUR_MISSION_GROUP : SkillEffectConfigNode
    {
		// 战斗任务-强制结束当前任务组
        public TSET_BATTLE_MISSION_FORCE_END_CUR_MISSION_GROUP() : base(TSkillEffectType.TSET_BATTLE_MISSION_FORCE_END_CUR_MISSION_GROUP) { }
    }
    #endregion SkillEffectConfig: 战斗任务-强制结束当前任务组


    #region SkillEffectConfig: 复刻单位外观
    [Serializable]
	[NodeMenuItem("技能效果/单位/复刻单位外观", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/复刻单位外观", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/复刻单位外观", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_COPY_ENTITY_OUTLOOK : SkillEffectConfigNode
    {
		// 复刻单位外观
		// 参数0 : 单位实例ID
		// 参数1 : 复制目标单位ID
		// 参数2 : 是否设为原始外观
        public TSET_COPY_ENTITY_OUTLOOK() : base(TSkillEffectType.TSET_COPY_ENTITY_OUTLOOK) { }
    }
    #endregion SkillEffectConfig: 复刻单位外观


    #region SkillEffectConfig: 动态解锁技能槽位
    [Serializable]
	[NodeMenuItem("技能效果/UI/动态解锁技能槽位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/动态解锁技能槽位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/动态解锁技能槽位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_SLOT_UNLOCK : SkillEffectConfigNode
    {
		// 动态解锁技能槽位
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽类型-TSkillSlotType
		// 参数2 : 解锁动效时长[帧]
        public TSET_SKILL_SLOT_UNLOCK() : base(TSkillEffectType.TSET_SKILL_SLOT_UNLOCK) { }
    }
    #endregion SkillEffectConfig: 动态解锁技能槽位


    #region SkillEffectConfig: 蓄能型技能-设置最大蓄能次数
    [Serializable]
	[NodeMenuItem("技能效果/技能/蓄能型技能-设置最大蓄能次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/蓄能型技能-设置最大蓄能次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/蓄能型技能-设置最大蓄能次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_STORAGE_SKILL_SET_MAX_STORE_COUNT : SkillEffectConfigNode
    {
		// 蓄能型技能-设置最大蓄能次数
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽类型-TSkillSlotType
		// 参数2 : 操作符-TBattleSetAttrOperationType
		// 参数3 : 次数
        public TSET_STORAGE_SKILL_SET_MAX_STORE_COUNT() : base(TSkillEffectType.TSET_STORAGE_SKILL_SET_MAX_STORE_COUNT) { }
    }
    #endregion SkillEffectConfig: 蓄能型技能-设置最大蓄能次数


    #region SkillEffectConfig: 蓄能型技能-获取最大蓄能次数
    [Serializable]
	[NodeMenuItem("技能效果/技能/蓄能型技能-获取最大蓄能次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/蓄能型技能-获取最大蓄能次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/蓄能型技能-获取最大蓄能次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_STORAGE_SKILL_GET_MAX_STORE_COUNT : SkillEffectConfigNode
    {
		// 蓄能型技能-获取最大蓄能次数
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽类型-TSkillSlotType
        public TSET_STORAGE_SKILL_GET_MAX_STORE_COUNT() : base(TSkillEffectType.TSET_STORAGE_SKILL_GET_MAX_STORE_COUNT) { }
    }
    #endregion SkillEffectConfig: 蓄能型技能-获取最大蓄能次数


    #region SkillEffectConfig: Switch分支执行
    [Serializable]
	[NodeMenuItem("通用配置/Switch分支执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/Switch分支执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/Switch分支执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SWITCH_EXECUTE : SkillEffectConfigNode
    {
		// Switch分支执行
		// 参数0 : Switch选择值
		// 参数1 : Default效果(未匹配任意Case时执行)-SkillEffectConfig
		// 参数2 : Case匹配值{0}
		// 参数3 : Case效果{0}-SkillEffectConfig
        public TSET_SWITCH_EXECUTE() : base(TSkillEffectType.TSET_SWITCH_EXECUTE) { }
    }
    #endregion SkillEffectConfig: Switch分支执行


    #region SkillEffectConfig: 随机召唤怪物波次布局单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/随机召唤怪物波次布局单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/随机召唤怪物波次布局单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/随机召唤怪物波次布局单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RANDOM_CREATE_PLAYER_WAVE_ENTITY : SkillEffectConfigNode
    {
		// 随机召唤怪物波次布局单位
		// 参数0 : 面向
		// 参数1 : 位置X
		// 参数2 : 位置Y
		// 参数3 : 最低单位类型[包含]-UnitType
		// 参数4 : 最高单位类型[包含]-UnitType
		// 参数5 : 随机方式-TBattleRandomCreateMonsterType
		// 参数6 : 出生前额外执行效果-SkillEffectConfig
		// 参数7 : 出生后额外执行效果-SkillEffectConfig
        public TSET_RANDOM_CREATE_PLAYER_WAVE_ENTITY() : base(TSkillEffectType.TSET_RANDOM_CREATE_PLAYER_WAVE_ENTITY) { }
    }
    #endregion SkillEffectConfig: 随机召唤怪物波次布局单位


    #region SkillEffectConfig: AI朝目标点移动(寻路方向)
    [Serializable]
	[NodeMenuItem("效果/AI朝目标点移动(寻路方向)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_MOVETO_POS : SkillEffectConfigNode
    {
		// AI朝目标点移动(寻路方向)
		// 参数0 : AI移动单位
		// 参数1 : 坐标X
		// 参数2 : 坐标Y
        public TSET_AI_MOVETO_POS() : base(TSkillEffectType.TSET_AI_MOVETO_POS) { }
    }
    #endregion SkillEffectConfig: AI朝目标点移动(寻路方向)


    #region SkillEffectConfig: 开启战斗任务
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/开启战斗任务", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/开启战斗任务", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/开启战斗任务", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_OPEN_TASK : SkillEffectConfigNode
    {
		// 开启战斗任务
		// 参数0 : 进入任务组index(-1表示进入下一个组)
		// 参数1 : 动态添加新任务组（任务组ID）
		// 参数2 : 是否进入新加的任务组
        public TSET_OPEN_TASK() : base(TSkillEffectType.TSET_OPEN_TASK) { }
    }
    #endregion SkillEffectConfig: 开启战斗任务


    #region SkillEffectConfig: 获取技能CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILLCD : SkillEffectConfigNode
    {
		// 获取技能CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽-TSkillSlotType
		// 参数2 : 技能CD类型（0：最终CD，1：基础CD，2剩余CD）
        public TSET_GET_SKILLCD() : base(TSkillEffectType.TSET_GET_SKILLCD) { }
    }
    #endregion SkillEffectConfig: 获取技能CD


    #region SkillEffectConfig: 技能单位组_获得数量
    [Serializable]
	[NodeMenuItem("技能效果/技能/技能单位组_获得数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获得数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/技能单位组_获得数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENTITYGROUP_GET_GROUP_COUNT : SkillEffectConfigNode
    {
		// 技能单位组_获得数量
		// 参数0 : 单位组所属单位实例ID
		// 参数1 : 技能ID
        public TSET_ENTITYGROUP_GET_GROUP_COUNT() : base(TSkillEffectType.TSET_ENTITYGROUP_GET_GROUP_COUNT) { }
    }
    #endregion SkillEffectConfig: 技能单位组_获得数量


    #region SkillEffectConfig: 召唤灵宠
    [Serializable]
	[NodeMenuItem("技能效果/召唤灵宠", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/召唤灵宠", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/召唤灵宠", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_PET : SkillEffectConfigNode
    {
		// 召唤灵宠
        public TSET_CREATE_PET() : base(TSkillEffectType.TSET_CREATE_PET) { }
    }
    #endregion SkillEffectConfig: 召唤灵宠


    #region SkillEffectConfig: 设置技能连招CD
    [Serializable]
	[NodeMenuItem("技能效果/技能/设置技能连招CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能连招CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能连招CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_SKILLCOMBO_CD : SkillEffectConfigNode
    {
		// 设置技能连招CD
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能槽-TSkillSlotType
		// 参数2 : 连招段数
		// 参数3 : CD值（帧数）
        public TSET_SET_SKILLCOMBO_CD() : base(TSkillEffectType.TSET_SET_SKILLCOMBO_CD) { }
    }
    #endregion SkillEffectConfig: 设置技能连招CD


    #region SkillEffectConfig: 增加Buff层数上限(永久)
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/增加Buff层数上限(永久)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/增加Buff层数上限(永久)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/增加Buff层数上限(永久)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BUFF_OVERLYING_MAX : SkillEffectConfigNode
    {
		// 增加Buff层数上限(永久)
		// 参数0 : 单位实例ID
		// 参数1 : BuffID
		// 参数2 : 增加层数
        public TSET_ADD_BUFF_OVERLYING_MAX() : base(TSkillEffectType.TSET_ADD_BUFF_OVERLYING_MAX) { }
    }
    #endregion SkillEffectConfig: 增加Buff层数上限(永久)


    #region SkillEffectConfig: 获取目标拥有的buff种类数量
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/获取目标拥有的buff种类数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取目标拥有的buff种类数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取目标拥有的buff种类数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_DIFFERENT_BUFF_ID_NUM : SkillEffectConfigNode
    {
		// 获取目标拥有的buff种类数量
		// 参数0 : 单位实例ID
		// 参数1 : Buff类型(0表示全部)-TBuffType
        public TSET_GET_DIFFERENT_BUFF_ID_NUM() : base(TSkillEffectType.TSET_GET_DIFFERENT_BUFF_ID_NUM) { }
    }
    #endregion SkillEffectConfig: 获取目标拥有的buff种类数量


    #region SkillEffectConfig: 获取技能等级
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能等级", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能等级", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能等级", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_LEVEL : SkillEffectConfigNode
    {
		// 获取技能等级
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 是否获取未激活技能(0-1)
        public TSET_GET_SKILL_LEVEL() : base(TSkillEffectType.TSET_GET_SKILL_LEVEL) { }
    }
    #endregion SkillEffectConfig: 获取技能等级


    #region SkillEffectConfig: 召唤剧情编辑的单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/召唤剧情编辑的单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤剧情编辑的单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/召唤剧情编辑的单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_QUEST_UNITS : SkillEffectConfigNode
    {
		// 召唤剧情编辑的单位
		// 参数0 : 剧情战斗单位组ID-QuestUnitConfig
		// 参数1 : 怪物位置ID-LocationConfig
		// 参数2 : 敌方NPC位置ID-LocationConfig
		// 参数3 : 友方NPC位置ID-LocationConfig
		// 参数4 : 玩家索引(默认创建者所属玩家索引)
        public TSET_CREATE_QUEST_UNITS() : base(TSkillEffectType.TSET_CREATE_QUEST_UNITS) { }
    }
    #endregion SkillEffectConfig: 召唤剧情编辑的单位


    #region SkillEffectConfig: 论剑-购买推荐卡牌
    [Serializable]
	[NodeMenuItem("效果/论剑-购买推荐卡牌", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_WENJIAN_BUY_SUGGESTCARD : SkillEffectConfigNode
    {
		// 论剑-购买推荐卡牌
        public TSET_WENJIAN_BUY_SUGGESTCARD() : base(TSkillEffectType.TSET_WENJIAN_BUY_SUGGESTCARD) { }
    }
    #endregion SkillEffectConfig: 论剑-购买推荐卡牌


    #region SkillEffectConfig: 论剑-刷新商店
    [Serializable]
	[NodeMenuItem("效果/论剑-刷新商店", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_WENJIAN_REFRESHSTORE : SkillEffectConfigNode
    {
		// 论剑-刷新商店
        public TSET_WENJIAN_REFRESHSTORE() : base(TSkillEffectType.TSET_WENJIAN_REFRESHSTORE) { }
    }
    #endregion SkillEffectConfig: 论剑-刷新商店


    #region SkillEffectConfig: 获取Buff时间相关
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/获取Buff时间相关", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取Buff时间相关", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取Buff时间相关", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BUFF_TIME : SkillEffectConfigNode
    {
		// 获取Buff时间相关
		// 参数0 : 单位实例ID
		// 参数1 : BuffID-BuffConfig
		// 参数2 : 获取类型-TBattleBuffTimeType
		// 参数3 : Buff来源单位实例ID（0不区分）
        public TSET_GET_BUFF_TIME() : base(TSkillEffectType.TSET_GET_BUFF_TIME) { }
    }
    #endregion SkillEffectConfig: 获取Buff时间相关


    #region SkillEffectConfig: 播放Timeline
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/播放Timeline", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放Timeline", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放Timeline", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PLAY_TIMELINE : SkillEffectConfigNode
    {
		// 播放Timeline
		// 参数0 : 播放来源单位实例ID
		// 参数1 : TimelineConfig表格ID-TimelineConfig
		// 参数2 : 播放坐标X
		// 参数3 : 播放坐标Y
        public TSET_PLAY_TIMELINE() : base(TSkillEffectType.TSET_PLAY_TIMELINE) { }
    }
    #endregion SkillEffectConfig: 播放Timeline


    #region SkillEffectConfig: AI设置单位面向和移动方向是否同步
    [Serializable]
	[NodeMenuItem("效果/AI设置单位面向和移动方向是否同步", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_FACE_MOVE_SYNC_ANGLE : SkillEffectConfigNode
    {
		// AI设置单位面向和移动方向是否同步
		// 参数0 : 面向是否跟随移动方向
        public TSET_SET_FACE_MOVE_SYNC_ANGLE() : base(TSkillEffectType.TSET_SET_FACE_MOVE_SYNC_ANGLE) { }
    }
    #endregion SkillEffectConfig: AI设置单位面向和移动方向是否同步


    #region SkillEffectConfig: 获取拥有的所有Buff层数总和
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取拥有的所有Buff层数总和", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取拥有的所有Buff层数总和", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取拥有的所有Buff层数总和", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ALL_BUFF_LAYER_COUNT_SUM : SkillEffectConfigNode
    {
		// 获取拥有的所有Buff层数总和
		// 参数0 : 目标单位实例ID
		// 参数1 : 对应Buff类型(0表示全部)-TBuffType
        public TSET_GET_ALL_BUFF_LAYER_COUNT_SUM() : base(TSkillEffectType.TSET_GET_ALL_BUFF_LAYER_COUNT_SUM) { }
    }
    #endregion SkillEffectConfig: 获取拥有的所有Buff层数总和


    #region SkillEffectConfig: 增加拥有的Buff的层数
    [Serializable]
	[NodeMenuItem("技能效果/技能/增加拥有的Buff的层数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加拥有的Buff的层数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加拥有的Buff的层数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ALL_BUFF_LAYER_COUNT : SkillEffectConfigNode
    {
		// 增加拥有的Buff的层数
		// 参数0 : 目标单位实例ID
		// 参数1 : 操作类型-TSkillEffectBuffOverlyingOptType
		// 参数2 : 操作类型参数
		// 参数3 : 增加方式-TSkillEffectBuffOverlyingAddType
		// 参数4 : 增加方式参数
		// 参数5 : Buff来源单位实例ID（0不区分）
        public TSET_ADD_ALL_BUFF_LAYER_COUNT() : base(TSkillEffectType.TSET_ADD_ALL_BUFF_LAYER_COUNT) { }
    }
    #endregion SkillEffectConfig: 增加拥有的Buff的层数


    #region SkillEffectConfig: 修改技能时长
    [Serializable]
	[NodeMenuItem("技能效果/技能/修改技能时长", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能时长", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/修改技能时长", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_SKILL_DURATION : SkillEffectConfigNode
    {
		// 修改技能时长
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能基础时长修改类型（0-增加，1-修改）
		// 参数3 : 技能基础时长变化帧数
        public TSET_CHANGE_SKILL_DURATION() : base(TSkillEffectType.TSET_CHANGE_SKILL_DURATION) { }
    }
    #endregion SkillEffectConfig: 修改技能时长


    #region SkillEffectConfig: 获取战斗道具品阶
    [Serializable]
	[NodeMenuItem("技能效果/获取战斗道具品阶", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具品阶", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具品阶", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_ITEM_RANK : SkillEffectConfigNode
    {
		// 获取战斗道具品阶
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
        public TSET_GET_BATTLE_ITEM_RANK() : base(TSkillEffectType.TSET_GET_BATTLE_ITEM_RANK) { }
    }
    #endregion SkillEffectConfig: 获取战斗道具品阶


    #region SkillEffectConfig: 获取战斗道具品质
    [Serializable]
	[NodeMenuItem("技能效果/获取战斗道具品质", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具品质", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具品质", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_ITEM_QUALITY : SkillEffectConfigNode
    {
		// 获取战斗道具品质
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
        public TSET_GET_BATTLE_ITEM_QUALITY() : base(TSkillEffectType.TSET_GET_BATTLE_ITEM_QUALITY) { }
    }
    #endregion SkillEffectConfig: 获取战斗道具品质


    #region SkillEffectConfig: 获取战斗道具使用次数
    [Serializable]
	[NodeMenuItem("技能效果/获取战斗道具使用次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具使用次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具使用次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_ITEM_USECOUNT : SkillEffectConfigNode
    {
		// 获取战斗道具使用次数
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
        public TSET_GET_BATTLE_ITEM_USECOUNT() : base(TSkillEffectType.TSET_GET_BATTLE_ITEM_USECOUNT) { }
    }
    #endregion SkillEffectConfig: 获取战斗道具使用次数


    #region SkillEffectConfig: 战斗道具恢复使用次数
    [Serializable]
	[NodeMenuItem("技能效果/战斗道具恢复使用次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗道具恢复使用次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗道具恢复使用次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RESTORE_BATTLE_ITEM_USECOUNT : SkillEffectConfigNode
    {
		// 战斗道具恢复使用次数
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
        public TSET_RESTORE_BATTLE_ITEM_USECOUNT() : base(TSkillEffectType.TSET_RESTORE_BATTLE_ITEM_USECOUNT) { }
    }
    #endregion SkillEffectConfig: 战斗道具恢复使用次数


    #region SkillEffectConfig: 战斗道具增加使用次数上限
    [Serializable]
	[NodeMenuItem("技能效果/战斗道具增加使用次数上限", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗道具增加使用次数上限", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗道具增加使用次数上限", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BATTLE_ITEM_MAX_USECOUNT : SkillEffectConfigNode
    {
		// 战斗道具增加使用次数上限
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
		// 参数3 : 增加次数
        public TSET_ADD_BATTLE_ITEM_MAX_USECOUNT() : base(TSkillEffectType.TSET_ADD_BATTLE_ITEM_MAX_USECOUNT) { }
    }
    #endregion SkillEffectConfig: 战斗道具增加使用次数上限


    #region SkillEffectConfig: 获取战斗道具蓝耗
    [Serializable]
	[NodeMenuItem("技能效果/获取战斗道具蓝耗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具蓝耗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取战斗道具蓝耗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_ITEM_MPCOST : SkillEffectConfigNode
    {
		// 获取战斗道具蓝耗
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
        public TSET_GET_BATTLE_ITEM_MPCOST() : base(TSkillEffectType.TSET_GET_BATTLE_ITEM_MPCOST) { }
    }
    #endregion SkillEffectConfig: 获取战斗道具蓝耗


    #region SkillEffectConfig: 修改战斗道具蓝耗
    [Serializable]
	[NodeMenuItem("技能效果/修改战斗道具蓝耗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/修改战斗道具蓝耗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/修改战斗道具蓝耗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_BATTLE_ITEM_MPCOST : SkillEffectConfigNode
    {
		// 修改战斗道具蓝耗
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
		// 参数3 : 变化值(大于0增加蓝耗，小于0减少)
        public TSET_CHANGE_BATTLE_ITEM_MPCOST() : base(TSkillEffectType.TSET_CHANGE_BATTLE_ITEM_MPCOST) { }
    }
    #endregion SkillEffectConfig: 修改战斗道具蓝耗


    #region SkillEffectConfig: 修改根技能并执行效果
    [Serializable]
	[NodeMenuItem("技能条件/技能/修改根技能并执行效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/修改根技能并执行效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能条件/技能/修改根技能并执行效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ROOT_SKILL : SkillEffectConfigNode
    {
		// 修改根技能并执行效果
		// 参数0 : 单位实例ID
		// 参数1 : 变更根技能ID（伤害等统计归属技能）
		// 参数2 : 附加技能效果-SkillEffectConfig
        public TSET_CHANGE_ROOT_SKILL() : base(TSkillEffectType.TSET_CHANGE_ROOT_SKILL) { }
    }
    #endregion SkillEffectConfig: 修改根技能并执行效果


    #region SkillEffectConfig: 设置战斗道具使用次数恢复cd
    [Serializable]
	[NodeMenuItem("技能效果/设置战斗道具使用次数恢复cd", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/设置战斗道具使用次数恢复cd", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/设置战斗道具使用次数恢复cd", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_BATTLE_ITEM_USECOUNT_RECOVER : SkillEffectConfigNode
    {
		// 设置战斗道具使用次数恢复cd
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
		// 参数3 : 使用次数恢复CD(帧)
        public TSET_SET_BATTLE_ITEM_USECOUNT_RECOVER() : base(TSkillEffectType.TSET_SET_BATTLE_ITEM_USECOUNT_RECOVER) { }
    }
    #endregion SkillEffectConfig: 设置战斗道具使用次数恢复cd


    #region SkillEffectConfig: 修改战斗道具CD
    [Serializable]
	[NodeMenuItem("技能效果/修改战斗道具CD", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/修改战斗道具CD", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/修改战斗道具CD", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_BATTLE_ITEM_CD : SkillEffectConfigNode
    {
		// 修改战斗道具CD
		// 参数0 : 拥有者单位
		// 参数1 : 道具槽-TItemSlotType
		// 参数2 : 道具类型-TBattleItemType
		// 参数3 : 战斗道具CD最终值
        public TSET_SET_BATTLE_ITEM_CD() : base(TSkillEffectType.TSET_SET_BATTLE_ITEM_CD) { }
    }
    #endregion SkillEffectConfig: 修改战斗道具CD


    #region SkillEffectConfig: 获取技能指示器信息
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能指示器信息", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能指示器信息", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能指示器信息", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_INDICATOR_INFO : SkillEffectConfigNode
    {
		// 获取技能指示器信息
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 指示器参数索引(0,1...)
        public TSET_GET_SKILL_INDICATOR_INFO() : base(TSkillEffectType.TSET_GET_SKILL_INDICATOR_INFO) { }
    }
    #endregion SkillEffectConfig: 获取技能指示器信息


    #region SkillEffectConfig: AI使用技能-按照技能AI标签
    [Serializable]
	[NodeMenuItem("效果/技能/AI使用技能-按照技能AI标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_AI_USE_SKILL_WITH_AI_TAG : SkillEffectConfigNode
    {
		// AI使用技能-按照技能AI标签
		// 参数0 : 技能目标单位
		// 参数1 : 距离超出不移动（0移动_1不移动）
		// 参数2 : 施法状态判定（0忽略_1施法状态则不释放_建议BOSS）
		// 参数3 : 期望执行的技能AI标签（单个，不填执行下面多个）-TBattleSkillAITag
		// 参数4 : 期望执行的技能AI标签组（多个）-BattleSkillTagGroupConfig
        public TSET_AI_USE_SKILL_WITH_AI_TAG() : base(TSkillEffectType.TSET_AI_USE_SKILL_WITH_AI_TAG) { }
    }
    #endregion SkillEffectConfig: AI使用技能-按照技能AI标签


    #region SkillEffectConfig: 获取技能真元消耗值
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能真元消耗值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能真元消耗值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能真元消耗值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_MP_COST : SkillEffectConfigNode
    {
		// 获取技能真元消耗值
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID
		// 参数2 : 获取值类型-TGetSkillMPCostType
        public TSET_GET_SKILL_MP_COST() : base(TSkillEffectType.TSET_GET_SKILL_MP_COST) { }
    }
    #endregion SkillEffectConfig: 获取技能真元消耗值


    #region SkillEffectConfig: 千层塔-获取难度塔的难度ID
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取难度塔的难度ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取难度塔的难度ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取难度塔的难度ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_DIFFICULTY_ID : SkillEffectConfigNode
    {
		// 千层塔-获取难度塔的难度ID
        public TSET_CHAPTER_GET_DIFFICULTY_ID() : base(TSkillEffectType.TSET_CHAPTER_GET_DIFFICULTY_ID) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取难度塔的难度ID


    #region SkillEffectConfig: 获取所有波次怪物的总血量值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取所有波次怪物的总血量值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取所有波次怪物的总血量值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取所有波次怪物的总血量值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ALL_MONSTER_WAVE_HP_TOTAL : SkillEffectConfigNode
    {
		// 获取所有波次怪物的总血量值
        public TSET_GET_ALL_MONSTER_WAVE_HP_TOTAL() : base(TSkillEffectType.TSET_GET_ALL_MONSTER_WAVE_HP_TOTAL) { }
    }
    #endregion SkillEffectConfig: 获取所有波次怪物的总血量值


    #region SkillEffectConfig: 获取当前持续技能槽位
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取当前持续技能槽位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取当前持续技能槽位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取当前持续技能槽位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_USING_SKILLSLOT_BY_USETYPE : SkillEffectConfigNode
    {
		// 获取当前持续技能槽位
		// 参数0 : 单位实例ID
		// 参数1 : 特指技能表格ID-SkillConfig
		// 参数2 : 特指技能使用类型（不填遍历所有）-TSkillUseType
        public TSET_GET_USING_SKILLSLOT_BY_USETYPE() : base(TSkillEffectType.TSET_GET_USING_SKILLSLOT_BY_USETYPE) { }
    }
    #endregion SkillEffectConfig: 获取当前持续技能槽位


    #region SkillEffectConfig: 添加道具
    [Serializable]
	[NodeMenuItem("技能效果/道具/添加道具", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/添加道具", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/添加道具", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ITEM : SkillEffectConfigNode
    {
		// 添加道具
		// 参数0 : 单位实例ID
		// 参数1 : 道具表格ID
		// 参数2 : 道具槽位类型-TItemSlotType
		// 参数3 : 道具数量（默认0按照道具表配置最大数量）
		// 参数4 : 是否强制替换添加（1：替换）
        public TSET_ADD_ITEM() : base(TSkillEffectType.TSET_ADD_ITEM) { }
    }
    #endregion SkillEffectConfig: 添加道具


    #region SkillEffectConfig: 单位属性替换
    [Serializable]
	[NodeMenuItem("技能效果/单位/单位属性替换", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/单位属性替换", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/单位属性替换", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ATTR_REPLACE : SkillEffectConfigNode
    {
		// 单位属性替换
		// 参数0 : 单位实例ID
		// 参数1 : 属性表ID-RoleAttrCustomConfig
		// 参数2 : 额外缩放万分比
        public TSET_ATTR_REPLACE() : base(TSkillEffectType.TSET_ATTR_REPLACE) { }
    }
    #endregion SkillEffectConfig: 单位属性替换


    #region SkillEffectConfig: 镜头俯仰角度变化
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头俯仰角度变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头俯仰角度变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头俯仰角度变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERA_PITCH_ANGLE_CHANGE : SkillEffectConfigNode
    {
		// 镜头俯仰角度变化
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 优先级-TCameraPriority
		// 参数3 : 优先级插入方式-TPriorityInsertType
		// 参数4 : 仰角角度偏移
		// 参数5 : 移动时间（帧数）
		// 参数6 : 维持时间（帧数）
		// 参数7 : 是否切回
		// 参数8 : 切回时间（帧数）
        public TSET_CAMERA_PITCH_ANGLE_CHANGE() : base(TSkillEffectType.TSET_CAMERA_PITCH_ANGLE_CHANGE) { }
    }
    #endregion SkillEffectConfig: 镜头俯仰角度变化


    #region SkillEffectConfig: 获取血量百分比
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取血量百分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取血量百分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取血量百分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GETHPPERCENT : SkillEffectConfigNode
    {
		// 获取血量百分比
		// 参数0 : 单位实例ID
		// 参数1 : 是否考虑护盾值（1：考虑护盾）
        public TSET_GETHPPERCENT() : base(TSkillEffectType.TSET_GETHPPERCENT) { }
    }
    #endregion SkillEffectConfig: 获取血量百分比


    #region SkillEffectConfig: 获取技能槽位-通过技能AI标签
    [Serializable]
	[NodeMenuItem("效果/技能/获取技能槽位-通过技能AI标签", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_SLOT_BY_SKILL_AI_TAG : SkillEffectConfigNode
    {
		// 获取技能槽位-通过技能AI标签
		// 参数0 : 单位实例ID
		// 参数1 : 期望获取的技能AI标签（单个，不填执行下面多个）-TBattleSkillAITag
		// 参数2 : 期望获取的技能AI标签组（多个）-BattleSkillTagGroupConfig
        public TSET_GET_SKILL_SLOT_BY_SKILL_AI_TAG() : base(TSkillEffectType.TSET_GET_SKILL_SLOT_BY_SKILL_AI_TAG) { }
    }
    #endregion SkillEffectConfig: 获取技能槽位-通过技能AI标签


    #region SkillEffectConfig: 千层塔-获取当前难度对应怪物攻击缩放万分比
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取当前难度对应怪物攻击缩放万分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前难度对应怪物攻击缩放万分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前难度对应怪物攻击缩放万分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_DIFFICULTY_ATK_FACTOR : SkillEffectConfigNode
    {
		// 千层塔-获取当前难度对应怪物攻击缩放万分比
        public TSET_CHAPTER_GET_DIFFICULTY_ATK_FACTOR() : base(TSkillEffectType.TSET_CHAPTER_GET_DIFFICULTY_ATK_FACTOR) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取当前难度对应怪物攻击缩放万分比


    #region SkillEffectConfig: 千层塔-获取当前层怪物强化Buff层数
    [Serializable]
	[NodeMenuItem("技能效果/千层塔玩法/千层塔-获取当前层怪物强化Buff层数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前层怪物强化Buff层数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/千层塔玩法/千层塔-获取当前层怪物强化Buff层数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHAPTER_GET_LEVEL_MONSTER_BUFF_COUNT : SkillEffectConfigNode
    {
		// 千层塔-获取当前层怪物强化Buff层数
        public TSET_CHAPTER_GET_LEVEL_MONSTER_BUFF_COUNT() : base(TSkillEffectType.TSET_CHAPTER_GET_LEVEL_MONSTER_BUFF_COUNT) { }
    }
    #endregion SkillEffectConfig: 千层塔-获取当前层怪物强化Buff层数


    #region SkillEffectConfig: 获取控制器实例ID
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/获取控制器实例ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取控制器实例ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取控制器实例ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ROOM_CONTROLLER : SkillEffectConfigNode
    {
		// 获取控制器实例ID
		// 参数0 : 房间索引（-1表示全局控制器）
        public TSET_GET_ROOM_CONTROLLER() : base(TSkillEffectType.TSET_GET_ROOM_CONTROLLER) { }
    }
    #endregion SkillEffectConfig: 获取控制器实例ID


    #region SkillEffectConfig: 获取伤害加成限制表数据
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取伤害加成限制表数据", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取伤害加成限制表数据", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取伤害加成限制表数据", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_DMG_ADD_CONFIG_DATA : SkillEffectConfigNode
    {
		// 获取伤害加成限制表数据
		// 参数0 : 单位实例ID
		// 参数1 : 表数据类型-TDamageBonusCapConfigDataType
        public TSET_GET_DMG_ADD_CONFIG_DATA() : base(TSkillEffectType.TSET_GET_DMG_ADD_CONFIG_DATA) { }
    }
    #endregion SkillEffectConfig: 获取伤害加成限制表数据


    #region SkillEffectConfig: 获取技能ID
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_ID : SkillEffectConfigNode
    {
		// 获取技能ID
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽位类型-TSkillSlotType
		// 参数2 : 技能ID类型[0:技能表格ID|1:技能实例ID]
        public TSET_GET_SKILL_ID() : base(TSkillEffectType.TSET_GET_SKILL_ID) { }
    }
    #endregion SkillEffectConfig: 获取技能ID


    #region SkillEffectConfig: 大世界刷怪-召唤波次布局单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大世界刷怪-召唤波次布局单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-召唤波次布局单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-召唤波次布局单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MAP_MONSTER_CREATE_PLAYER_LAYOUT_ENTITY : SkillEffectConfigNode
    {
		// 大世界刷怪-召唤波次布局单位
		// 参数0 : 玩家索引
		// 参数1 : 单位子类型[0表示所有]-TEntitySubType
		// 参数2 : 单位境界[0表示所有]
		// 参数3 : 面向
		// 参数4 : 位置X
		// 参数5 : 位置Y
		// 参数6 : 布局索引(波次)[-1表示所有波次]
		// 参数7 : 怪物种族子类型[0表示所有]-TRaceSubType
		// 参数8 : 单位类型1[0表示所有]-UnitType
		// 参数9 : 或者单位类型2[0表示无效]-UnitType
		// 参数10 : 出生前执行效果-SkillEffectConfig
		// 参数11 : 出生后执行效果-SkillEffectConfig
        public TSET_MAP_MONSTER_CREATE_PLAYER_LAYOUT_ENTITY() : base(TSkillEffectType.TSET_MAP_MONSTER_CREATE_PLAYER_LAYOUT_ENTITY) { }
    }
    #endregion SkillEffectConfig: 大世界刷怪-召唤波次布局单位


    #region SkillEffectConfig: 大世界刷怪-获取波次剩余未召唤单位数量
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大世界刷怪-获取波次剩余未召唤单位数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取波次剩余未召唤单位数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取波次剩余未召唤单位数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_ENTITY_LEFT_COUNT : SkillEffectConfigNode
    {
		// 大世界刷怪-获取波次剩余未召唤单位数量
		// 参数0 : 玩家索引
		// 参数1 : 单位子类型[0表示所有]-TEntitySubType
		// 参数2 : 单位境界[0表示所有]
		// 参数3 : 布局索引(波次)[-1表示所有波次]
		// 参数4 : 怪物种族子类型[0表示所有]-TRaceSubType
		// 参数5 : 单位类型1[0表示所有]-UnitType
		// 参数6 : 或者单位类型2[0表示无效]-UnitType
        public TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_ENTITY_LEFT_COUNT() : base(TSkillEffectType.TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_ENTITY_LEFT_COUNT) { }
    }
    #endregion SkillEffectConfig: 大世界刷怪-获取波次剩余未召唤单位数量


    #region SkillEffectConfig: 大世界刷怪-获取总波次数
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大世界刷怪-获取总波次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取总波次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取总波次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_COUNT : SkillEffectConfigNode
    {
		// 大世界刷怪-获取总波次数
		// 参数0 : 玩家索引
        public TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_COUNT() : base(TSkillEffectType.TSET_MAP_MONSTER_GET_PLAYER_LAYOUT_COUNT) { }
    }
    #endregion SkillEffectConfig: 大世界刷怪-获取总波次数


    #region SkillEffectConfig: 获得护主条
    [Serializable]
	[NodeMenuItem("技能效果/单位/获得护主条", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获得护主条", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获得护主条", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROTECT_HORD_ADD : SkillEffectConfigNode
    {
		// 获得护主条
		// 参数0 : 单位实例ID
        public TSET_PROTECT_HORD_ADD() : base(TSkillEffectType.TSET_PROTECT_HORD_ADD) { }
    }
    #endregion SkillEffectConfig: 获得护主条


    #region SkillEffectConfig: 大世界刷怪-获取拉入的多怪物总只数
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大世界刷怪-获取拉入的多怪物总只数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取拉入的多怪物总只数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取拉入的多怪物总只数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MAP_MONSTER_GET_TOTAL_TAKE_IN_MAP_MONSTER_NUM : SkillEffectConfigNode
    {
		// 大世界刷怪-获取拉入的多怪物总只数
        public TSET_MAP_MONSTER_GET_TOTAL_TAKE_IN_MAP_MONSTER_NUM() : base(TSkillEffectType.TSET_MAP_MONSTER_GET_TOTAL_TAKE_IN_MAP_MONSTER_NUM) { }
    }
    #endregion SkillEffectConfig: 大世界刷怪-获取拉入的多怪物总只数


    #region SkillEffectConfig: 大世界刷怪-获取多怪物的种族子类型
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大世界刷怪-获取多怪物的种族子类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取多怪物的种族子类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大世界刷怪-获取多怪物的种族子类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MAP_MONSTER_GET_MAP_MONSTER_RACE_SUB_TYPE : SkillEffectConfigNode
    {
		// 大世界刷怪-获取多怪物的种族子类型
		// 参数0 : 多怪物的编队序号Index（从0开始）
        public TSET_MAP_MONSTER_GET_MAP_MONSTER_RACE_SUB_TYPE() : base(TSkillEffectType.TSET_MAP_MONSTER_GET_MAP_MONSTER_RACE_SUB_TYPE) { }
    }
    #endregion SkillEffectConfig: 大世界刷怪-获取多怪物的种族子类型


    #region SkillEffectConfig: 数学库-获取环形带缺口随机角度
    [Serializable]
	[NodeMenuItem("通用配置/数值运算/角度距离计算位置/数学库-获取环形带缺口随机角度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/数学库-获取环形带缺口随机角度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/数值运算/角度距离计算位置/数学库-获取环形带缺口随机角度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MATH_GET_RANDOM_RING_ANGLE : SkillEffectConfigNode
    {
		// 数学库-获取环形带缺口随机角度
		// 参数0 : 圆心位置X[厘米]
		// 参数1 : 圆心位置Y[厘米]
		// 参数2 : 内环半径[厘米]
		// 参数3 : 外环半径[厘米]
		// 参数4 : 缺口中线方向[角度]
		// 参数5 : 缺口大小[角度](0表示无缺口)
		// 参数6 : 是否允许随机到地图外面
        public TSET_MATH_GET_RANDOM_RING_ANGLE() : base(TSkillEffectType.TSET_MATH_GET_RANDOM_RING_ANGLE) { }
    }
    #endregion SkillEffectConfig: 数学库-获取环形带缺口随机角度


    #region SkillEffectConfig: 击破护主条
    [Serializable]
	[NodeMenuItem("技能效果/单位/击破护主条", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/击破护主条", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/击破护主条", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROTECT_HORD_BREAK_UP : SkillEffectConfigNode
    {
		// 击破护主条
		// 参数0 : 受击者单位实例ID
		// 参数1 : 攻击者单位实例ID
        public TSET_PROTECT_HORD_BREAK_UP() : base(TSkillEffectType.TSET_PROTECT_HORD_BREAK_UP) { }
    }
    #endregion SkillEffectConfig: 击破护主条


    #region SkillEffectConfig: 创建圆形预警圈-缩圈
    [Serializable]
	[NodeMenuItem("技能效果/技能/创建圆形预警圈-缩圈", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建圆形预警圈-缩圈", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/创建圆形预警圈-缩圈", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_WARNING_CIRCLE_SHRINK : SkillEffectConfigNode
    {
		// 创建圆形预警圈-缩圈
		// 参数0 : 资源模型ID-ModelConfig
		// 参数1 : 位置X
		// 参数2 : 位置Y
		// 参数3 : 内圈半径-厘米
		// 参数4 : 持续帧数(<=0:永久)
		// 参数5 : 加入单位组-TSkillEntityGroupType
		// 参数6 : 位置偏移_右侧
		// 参数7 : 位置偏移_面前
        public TSET_CREATE_WARNING_CIRCLE_SHRINK() : base(TSkillEffectType.TSET_CREATE_WARNING_CIRCLE_SHRINK) { }
    }
    #endregion SkillEffectConfig: 创建圆形预警圈-缩圈


    #region SkillEffectConfig: 创建战斗单位-阻挡墙壁
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/创建战斗单位-阻挡墙壁", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建战斗单位-阻挡墙壁", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建战斗单位-阻挡墙壁", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_BATTLE_UNIT_BLOCK_WALL : SkillEffectConfigNode
    {
		// 创建战斗单位-阻挡墙壁
		// 参数0 : 位置X
		// 参数1 : 位置Y
		// 参数2 : 面向
		// 参数3 : 存活帧数-自动死亡[填0:不自动死亡]
		// 参数4 : 受击次数[0表示不可破坏]
		// 参数5 : 模型ID-ModelConfig
		// 参数6 : 模型缩放百分比
		// 参数7 : 出生前额外执行效果-SkillEffectConfig
		// 参数8 : 出生后额外执行效果-SkillEffectConfig
		// 参数9 : 同阵营是否可穿越
        public TSET_CREATE_BATTLE_UNIT_BLOCK_WALL() : base(TSkillEffectType.TSET_CREATE_BATTLE_UNIT_BLOCK_WALL) { }
    }
    #endregion SkillEffectConfig: 创建战斗单位-阻挡墙壁


    #region SkillEffectConfig: 宗门技能体验开启
    [Serializable]
	[NodeMenuItem("技能效果/UI/宗门技能体验开启", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/宗门技能体验开启", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/宗门技能体验开启", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SECT_EXPERIENXE_OPEN : SkillEffectConfigNode
    {
		// 宗门技能体验开启
		// 参数0 : 显示状态（1.开 0.关）
		// 参数1 : 金宗门（0:解禁 1:禁用）
		// 参数2 : 木宗门（0:解禁 1:禁用）
		// 参数3 : 水宗门（0:解禁 1:禁用）
		// 参数4 : 火宗门（0:解禁 1:禁用）
		// 参数5 : 土宗门（0:解禁 1:禁用）
        public TSET_SECT_EXPERIENXE_OPEN() : base(TSkillEffectType.TSET_SECT_EXPERIENXE_OPEN) { }
    }
    #endregion SkillEffectConfig: 宗门技能体验开启


    #region SkillEffectConfig: 外力速度影响
    [Serializable]
	[NodeMenuItem("技能效果/外力速度影响", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/外力速度影响", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/外力速度影响", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SIMULATE_FORCE_SPEED_IMPACT : SkillEffectConfigNode
    {
		// 外力速度影响
		// 参数0 : 影响目标
		// 参数1 : 外力源单位(和源角度选一个配置)
		// 参数2 : 外力源固定角度(和源单位选一个配置)
		// 参数3 : 持续时间(帧)
		// 参数4 : 影响速度
		// 参数5 : 影响加速度
		// 参数6 : 最小距离
		// 参数7 : 是否忽略模型受力倍率
		// 参数8 : 是否无视不动如山
        public TSET_SIMULATE_FORCE_SPEED_IMPACT() : base(TSkillEffectType.TSET_SIMULATE_FORCE_SPEED_IMPACT) { }
    }
    #endregion SkillEffectConfig: 外力速度影响


    #region SkillEffectConfig: 增加子弹距离上限
    [Serializable]
	[NodeMenuItem("技能效果/技能/增加子弹距离上限", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加子弹距离上限", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/增加子弹距离上限", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_BULLET_MAX_DISTANCE : SkillEffectConfigNode
    {
		// 增加子弹距离上限
		// 参数0 : 子弹单位实例ID
		// 参数1 : 增加距离上限值
        public TSET_ADD_BULLET_MAX_DISTANCE() : base(TSkillEffectType.TSET_ADD_BULLET_MAX_DISTANCE) { }
    }
    #endregion SkillEffectConfig: 增加子弹距离上限


    #region SkillEffectConfig: 添加单位状态
    [Serializable]
	[NodeMenuItem("技能效果/单位/添加单位状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加单位状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/添加单位状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_ENTITY_STATE : SkillEffectConfigNode
    {
		// 添加单位状态
		// 参数0 : 单位实例ID
		// 参数1 : 状态枚举值-TEntityState
		// 参数2 : 增加状态值（默认1，恢复填-1）
        public TSET_ADD_ENTITY_STATE() : base(TSkillEffectType.TSET_ADD_ENTITY_STATE) { }
    }
    #endregion SkillEffectConfig: 添加单位状态


    #region SkillEffectConfig: 获取战斗时间伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/获取战斗时间伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取战斗时间伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/获取战斗时间伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_BATTLE_TIME_DMGADD : SkillEffectConfigNode
    {
		// 获取战斗时间伤害加成
        public TSET_GET_BATTLE_TIME_DMGADD() : base(TSkillEffectType.TSET_GET_BATTLE_TIME_DMGADD) { }
    }
    #endregion SkillEffectConfig: 获取战斗时间伤害加成


    #region SkillEffectConfig: 奇遇空战-修改背景移动速度
    [Serializable]
	[NodeMenuItem("技能效果/空间/奇遇空战-修改背景移动速度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/空间/奇遇空战-修改背景移动速度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/空间/奇遇空战-修改背景移动速度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ENCOUNTER_BACKGROUND_MOVE_SPEED : SkillEffectConfigNode
    {
		// 奇遇空战-修改背景移动速度
		// 参数0 : X轴速度[厘米_秒]
		// 参数1 : Y轴速度[厘米_秒]
        public TSET_ENCOUNTER_BACKGROUND_MOVE_SPEED() : base(TSkillEffectType.TSET_ENCOUNTER_BACKGROUND_MOVE_SPEED) { }
    }
    #endregion SkillEffectConfig: 奇遇空战-修改背景移动速度


    #region SkillEffectConfig: 获取设备屏幕分辨率比值万分比(仅单机)
    [Serializable]
	[NodeMenuItem("技能效果/UI/获取设备屏幕分辨率比值万分比(仅单机)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/获取设备屏幕分辨率比值万分比(仅单机)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/获取设备屏幕分辨率比值万分比(仅单机)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SCREEN_RESOLUTION_RATIO_PER : SkillEffectConfigNode
    {
		// 获取设备屏幕分辨率比值万分比(仅单机)
        public TSET_GET_SCREEN_RESOLUTION_RATIO_PER() : base(TSkillEffectType.TSET_GET_SCREEN_RESOLUTION_RATIO_PER) { }
    }
    #endregion SkillEffectConfig: 获取设备屏幕分辨率比值万分比(仅单机)


    #region SkillEffectConfig: 创建阻挡边界-矩形
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/创建阻挡边界-矩形", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建阻挡边界-矩形", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建阻挡边界-矩形", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_MAP_EDGE_RECT : SkillEffectConfigNode
    {
		// 创建阻挡边界-矩形
		// 参数0 : 中心位置X
		// 参数1 : 中心位置Y
		// 参数2 : 矩形X长
		// 参数3 : 矩形Y长
		// 参数4 : 矩形旋转角度
		// 参数5 : 碰撞层-TCollisionLayer
        public TSET_CREATE_MAP_EDGE_RECT() : base(TSkillEffectType.TSET_CREATE_MAP_EDGE_RECT) { }
    }
    #endregion SkillEffectConfig: 创建阻挡边界-矩形


    #region SkillEffectConfig: 创建战斗单位-岩盾类
    [Serializable]
    public sealed partial class TSET_CREATE_BATTLE_UNIT_SHIELD : SkillEffectConfigNode
    {
		// 创建战斗单位-岩盾类
		// 参数0 : 召唤的战斗单位ID-BattleUnitConfig
		// 参数1 : 角度
		// 参数2 : 位置X
		// 参数3 : 位置Y
		// 参数4 : 阵营(默认创建者所属阵营)-TEntityCamp
		// 参数5 : 玩家索引(默认创建者所属玩家索引)
		// 参数6 : 位置偏移X
		// 参数7 : 位置偏移Y
		// 参数8 : 召唤位置限制-TBattlePositionClampType
		// 参数9 : 存活基础帧数-自动死亡[填0:不自动死亡]
		// 参数10 : 加入单位组：-TSkillEntityGroupType
		// 参数11 : 是否标志为召唤物
		// 参数12 : 创建者单位实例ID（填0设置为召唤物自己）
		// 参数13 : 自定义模型ID-ModelConfig
		// 参数14 : 出生前额外执行效果-SkillEffectConfig
		// 参数15 : 出生后额外执行效果-SkillEffectConfig
		// 参数16 : 战斗结束后是否继续执行
        public TSET_CREATE_BATTLE_UNIT_SHIELD() : base(TSkillEffectType.TSET_CREATE_BATTLE_UNIT_SHIELD) { }
    }
    #endregion SkillEffectConfig: 创建战斗单位-岩盾类


    #region SkillEffectConfig: 获取技能数值系数-百分比
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能数值系数-百分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能数值系数-百分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能数值系数-百分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_PARAM_VALUE : SkillEffectConfigNode
    {
		// 获取技能数值系数-百分比
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
        public TSET_GET_SKILL_PARAM_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_PARAM_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能数值系数-百分比


    #region SkillEffectConfig: 获取本命法宝战斗内模型ID
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取本命法宝战斗内模型ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取本命法宝战斗内模型ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取本命法宝战斗内模型ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ORIGINTREASURE_MODELID : SkillEffectConfigNode
    {
		// 获取本命法宝战斗内模型ID
		// 参数0 : 单位实例ID
        public TSET_GET_ORIGINTREASURE_MODELID() : base(TSkillEffectType.TSET_GET_ORIGINTREASURE_MODELID) { }
    }
    #endregion SkillEffectConfig: 获取本命法宝战斗内模型ID


    #region SkillEffectConfig: 筛选执行
    [Serializable]
	[NodeMenuItem("通用配置/逻辑执行/筛选执行", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/筛选执行", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/通用配置/逻辑执行/筛选执行", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SELECT_EXECUTE : SkillEffectConfigNode
    {
		// 筛选执行
		// 参数0 : 筛选ID-SkillSelectConfig
		// 参数1 : 对筛选目标执行效果ID-SkillEffectConfig
		// 参数2 : 无目标执行效果ID-SkillEffectConfig
        public TSET_SELECT_EXECUTE() : base(TSkillEffectType.TSET_SELECT_EXECUTE) { }
    }
    #endregion SkillEffectConfig: 筛选执行


    #region SkillEffectConfig: 顿帧效果
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/顿帧效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/顿帧效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/顿帧效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_STOP_FRAME : SkillEffectConfigNode
    {
		// 顿帧效果
		// 参数0 : 攻击者(一般是飞弹)
		// 参数1 : 受击者
		// 参数2 : 施法者(默认0 不参与顿帧)
		// 参数3 : 时长
		// 参数4 : 放慢倍率
		// 参数5 : 忽略顿帧时长
        public TSET_STOP_FRAME() : base(TSkillEffectType.TSET_STOP_FRAME) { }
    }
    #endregion SkillEffectConfig: 顿帧效果


    #region SkillEffectConfig: 获取单位自身碰撞配置的假高度Z
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位自身碰撞配置的假高度Z", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的假高度Z", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位自身碰撞配置的假高度Z", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_FIXTURE_CENTER_Z : SkillEffectConfigNode
    {
		// 获取单位自身碰撞配置的假高度Z
		// 参数0 : 单位实体ID
		// 参数1 : 出手点类型-TFixtureAttackPosFlag-TFixtureAttackPosFlag
        public TSET_GET_FIXTURE_CENTER_Z() : base(TSkillEffectType.TSET_GET_FIXTURE_CENTER_Z) { }
    }
    #endregion SkillEffectConfig: 获取单位自身碰撞配置的假高度Z


    #region SkillEffectConfig: 修改单位名字
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位名字", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位名字", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位名字", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_NAME : SkillEffectConfigNode
    {
		// 修改单位名字
		// 参数0 : 被修改单位
		// 参数1 : 名字文本id-TextConfig
		// 参数2 : 需要继承单位id
        public TSET_CHANGE_ENTITY_NAME() : base(TSkillEffectType.TSET_CHANGE_ENTITY_NAME) { }
    }
    #endregion SkillEffectConfig: 修改单位名字


    #region SkillEffectConfig: 修改单位境界
    [Serializable]
	[NodeMenuItem("技能效果/单位/修改单位境界", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位境界", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/修改单位境界", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ENTITY_EXP_STATE : SkillEffectConfigNode
    {
		// 修改单位境界
		// 参数0 : 被修改单位
		// 参数1 : 境界
        public TSET_CHANGE_ENTITY_EXP_STATE() : base(TSkillEffectType.TSET_CHANGE_ENTITY_EXP_STATE) { }
    }
    #endregion SkillEffectConfig: 修改单位境界


    #region SkillEffectConfig: 播放命中特效
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/播放命中特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放命中特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/播放命中特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_HIT_EFFECT : SkillEffectConfigNode
    {
		// 播放命中特效
		// 参数0 : 特效ID-ModelConfig
		// 参数1 : 角度
		// 参数2 : 缩放百分比
		// 参数3 : 持续时间[帧数]
		// 参数4 : 位置类型-TBattleHitEffectPosType
        public TSET_CREATE_HIT_EFFECT() : base(TSkillEffectType.TSET_CREATE_HIT_EFFECT) { }
    }
    #endregion SkillEffectConfig: 播放命中特效


    #region SkillEffectConfig: 关闭单位特效
    [Serializable]
	[NodeMenuItem("技能效果/单位/关闭单位特效", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/关闭单位特效", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/关闭单位特效", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_DISABLE_ENTITY_EFFECT : SkillEffectConfigNode
    {
		// 关闭单位特效
		// 参数0 : 应用特效目标单位
		// 参数1 : 单位特效ID-SkillEffectConfig
        public TSET_DISABLE_ENTITY_EFFECT() : base(TSkillEffectType.TSET_DISABLE_ENTITY_EFFECT) { }
    }
    #endregion SkillEffectConfig: 关闭单位特效


    #region SkillEffectConfig: 显示次级对话-单位
    [Serializable]
	[NodeMenuItem("技能效果/UI/显示次级对话-单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示次级对话-单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示次级对话-单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_SECONDARY_DIALOGUE_BY_ENTITY : SkillEffectConfigNode
    {
		// 显示次级对话-单位
		// 参数0 : 单位实例ID
		// 参数1 : 对话文本ID-TextConfig
		// 参数2 : 持续时间-帧
        public TSET_SHOW_SECONDARY_DIALOGUE_BY_ENTITY() : base(TSkillEffectType.TSET_SHOW_SECONDARY_DIALOGUE_BY_ENTITY) { }
    }
    #endregion SkillEffectConfig: 显示次级对话-单位


    #region SkillEffectConfig: 是否显示技能按钮(仅UI层)
    [Serializable]
	[NodeMenuItem("技能效果/UI/是否显示技能按钮(仅UI层)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/是否显示技能按钮(仅UI层)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/是否显示技能按钮(仅UI层)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_BTN_VISIBLE : SkillEffectConfigNode
    {
		// 是否显示技能按钮(仅UI层)
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 是否显示(1:显示 0:不显示)
        public TSET_SKILL_BTN_VISIBLE() : base(TSkillEffectType.TSET_SKILL_BTN_VISIBLE) { }
    }
    #endregion SkillEffectConfig: 是否显示技能按钮(仅UI层)


    #region SkillEffectConfig: 技能按钮缩放(仅UI层)
    [Serializable]
	[NodeMenuItem("技能效果/UI/技能按钮缩放(仅UI层)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/技能按钮缩放(仅UI层)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/技能按钮缩放(仅UI层)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_BTN_SCALE : SkillEffectConfigNode
    {
		// 技能按钮缩放(仅UI层)
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 缩放值( 缩放值*0.01: 显示值 )
        public TSET_SKILL_BTN_SCALE() : base(TSkillEffectType.TSET_SKILL_BTN_SCALE) { }
    }
    #endregion SkillEffectConfig: 技能按钮缩放(仅UI层)


    #region SkillEffectConfig: 技能按钮坐标(仅UI层)
    [Serializable]
	[NodeMenuItem("技能效果/UI/技能按钮坐标(仅UI层)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/技能按钮坐标(仅UI层)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/技能按钮坐标(仅UI层)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_BTN_POS : SkillEffectConfigNode
    {
		// 技能按钮坐标(仅UI层)
		// 参数0 : 技能槽位-TSkillSlotType
		// 参数1 : 坐标X
		// 参数2 : 坐标Y
		// 参数3 : 是否回到初始点(1:回 0:不回 默认按设置坐标)
        public TSET_SKILL_BTN_POS() : base(TSkillEffectType.TSET_SKILL_BTN_POS) { }
    }
    #endregion SkillEffectConfig: 技能按钮坐标(仅UI层)


    #region SkillEffectConfig: 刷新房间markers
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/刷新房间markers", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/刷新房间markers", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/刷新房间markers", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ROOM_REFRESH_MARKERS : SkillEffectConfigNode
    {
		// 刷新房间markers
		// 参数0 : 房间配表ID[默认主体单位所在房间]-BattleRoomConfig
        public TSET_ROOM_REFRESH_MARKERS() : base(TSkillEffectType.TSET_ROOM_REFRESH_MARKERS) { }
    }
    #endregion SkillEffectConfig: 刷新房间markers


    #region SkillEffectConfig: 切换战场UI
    [Serializable]
	[NodeMenuItem("技能效果/UI/切换战场UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/切换战场UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/切换战场UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SWITCH_BATTLE_UI : SkillEffectConfigNode
    {
		// 切换战场UI
		// 参数0 : 模板ID (0:默认布局 1:变身后布局)
        public TSET_SWITCH_BATTLE_UI() : base(TSkillEffectType.TSET_SWITCH_BATTLE_UI) { }
    }
    #endregion SkillEffectConfig: 切换战场UI


    #region SkillEffectConfig: 塔科夫-房间毁灭预警
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-房间毁灭预警", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-房间毁灭预警", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-房间毁灭预警", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_ROOM_DESTROY_WARNING : SkillEffectConfigNode
    {
		// 塔科夫-房间毁灭预警
		// 参数0 : 最终剩余房间数量
		// 参数1 : 预警持续时间[帧]
		// 参数2 : 毁灭持续时间[帧]
        public TSET_TARKOV_ROOM_DESTROY_WARNING() : base(TSkillEffectType.TSET_TARKOV_ROOM_DESTROY_WARNING) { }
    }
    #endregion SkillEffectConfig: 塔科夫-房间毁灭预警


    #region SkillEffectConfig: 塔科夫-撤离点开启
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-撤离点开启", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-撤离点开启", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-撤离点开启", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_PULL_BACK_OPEN : SkillEffectConfigNode
    {
		// 塔科夫-撤离点开启
		// 参数0 : 开启倒计时[帧]
        public TSET_TARKOV_PULL_BACK_OPEN() : base(TSkillEffectType.TSET_TARKOV_PULL_BACK_OPEN) { }
    }
    #endregion SkillEffectConfig: 塔科夫-撤离点开启


    #region SkillEffectConfig: 塔科夫-奇珍投放
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-奇珍投放", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-奇珍投放", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-奇珍投放", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_AIR_DROP_OPEN : SkillEffectConfigNode
    {
		// 塔科夫-奇珍投放
		// 参数0 : 投放倒计时[帧]
		// 参数1 : 投放数量
        public TSET_TARKOV_AIR_DROP_OPEN() : base(TSkillEffectType.TSET_TARKOV_AIR_DROP_OPEN) { }
    }
    #endregion SkillEffectConfig: 塔科夫-奇珍投放


    #region SkillEffectConfig: 塔科夫-BOSS出现
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-BOSS出现", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-BOSS出现", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-BOSS出现", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_BOSS_OPEN : SkillEffectConfigNode
    {
		// 塔科夫-BOSS出现
		// 参数0 : 出现倒计时[帧]
        public TSET_TARKOV_BOSS_OPEN() : base(TSkillEffectType.TSET_TARKOV_BOSS_OPEN) { }
    }
    #endregion SkillEffectConfig: 塔科夫-BOSS出现


    #region SkillEffectConfig: 是否显示实体交互UI
    [Serializable]
	[NodeMenuItem("技能效果/UI/是否显示实体交互UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/是否显示实体交互UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/是否显示实体交互UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_VISIBLE_ENTITY_INTERACTIVE_UI : SkillEffectConfigNode
    {
		// 是否显示实体交互UI
		// 参数0 : 交互实例ID
		// 参数1 : 是否显示(1:显示 0:隐藏)
        public TSET_VISIBLE_ENTITY_INTERACTIVE_UI() : base(TSkillEffectType.TSET_VISIBLE_ENTITY_INTERACTIVE_UI) { }
    }
    #endregion SkillEffectConfig: 是否显示实体交互UI


    #region SkillEffectConfig: 塔科夫-BOSS房紧急撤离点开启
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-BOSS房紧急撤离点开启", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-BOSS房紧急撤离点开启", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-BOSS房紧急撤离点开启", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_BOSS_PULL_BACK_OPEN : SkillEffectConfigNode
    {
		// 塔科夫-BOSS房紧急撤离点开启
		// 参数0 : [备注]开启条件为BOSS死亡
        public TSET_TARKOV_BOSS_PULL_BACK_OPEN() : base(TSkillEffectType.TSET_TARKOV_BOSS_PULL_BACK_OPEN) { }
    }
    #endregion SkillEffectConfig: 塔科夫-BOSS房紧急撤离点开启


    #region SkillEffectConfig: 塔科夫-房间传送
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-房间传送", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-房间传送", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-房间传送", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_ROOM_TELEPORT : SkillEffectConfigNode
    {
		// 塔科夫-房间传送
		// 参数0 : 传送点实例ID
		// 参数1 : 传送提示文本ID
        public TSET_TARKOV_ROOM_TELEPORT() : base(TSkillEffectType.TSET_TARKOV_ROOM_TELEPORT) { }
    }
    #endregion SkillEffectConfig: 塔科夫-房间传送


    #region SkillEffectConfig: 设置相机缓动参数
    [Serializable]
	[NodeMenuItem("技能效果/相机/设置相机缓动参数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/设置相机缓动参数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/设置相机缓动参数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERA_DAMPING : SkillEffectConfigNode
    {
		// 设置相机缓动参数
		// 参数0 : 缓动值 ( 0代表没有缓动 )
        public TSET_CAMERA_DAMPING() : base(TSkillEffectType.TSET_CAMERA_DAMPING) { }
    }
    #endregion SkillEffectConfig: 设置相机缓动参数


    #region SkillEffectConfig: 塔科夫-撤离
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-撤离", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-撤离", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-撤离", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_ESCAPE : SkillEffectConfigNode
    {
		// 塔科夫-撤离
		// 参数0 : 玩家ID
        public TSET_TARKOV_ESCAPE() : base(TSkillEffectType.TSET_TARKOV_ESCAPE) { }
    }
    #endregion SkillEffectConfig: 塔科夫-撤离


    #region SkillEffectConfig: 取消跟随
    [Serializable]
	[NodeMenuItem("技能效果/技能/取消跟随", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/取消跟随", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/取消跟随", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CANCEL_FOLLOW : SkillEffectConfigNode
    {
		// 取消跟随
		// 参数0 : 跟随者主体单位
		// 参数1 : 是否立即同步跟随位置
        public TSET_CANCEL_FOLLOW() : base(TSkillEffectType.TSET_CANCEL_FOLLOW) { }
    }
    #endregion SkillEffectConfig: 取消跟随


    #region SkillEffectConfig: 塔科夫-生成道具
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-生成道具", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-生成道具", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-生成道具", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_CREATE_ITEM : SkillEffectConfigNode
    {
		// 塔科夫-生成道具
		// 参数0 : 掉落ID
        public TSET_TARKOV_CREATE_ITEM() : base(TSkillEffectType.TSET_TARKOV_CREATE_ITEM) { }
    }
    #endregion SkillEffectConfig: 塔科夫-生成道具


    #region SkillEffectConfig: 塔科夫-拾取道具
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-拾取道具", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-拾取道具", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-拾取道具", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_GET_ITEM : SkillEffectConfigNode
    {
		// 塔科夫-拾取道具
		// 参数0 : 拾取实例ID
        public TSET_TARKOV_GET_ITEM() : base(TSkillEffectType.TSET_TARKOV_GET_ITEM) { }
    }
    #endregion SkillEffectConfig: 塔科夫-拾取道具


    #region SkillEffectConfig: 塔科夫-打开宝箱UI
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-打开宝箱UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-打开宝箱UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-打开宝箱UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_TREASURE_UI : SkillEffectConfigNode
    {
		// 塔科夫-打开宝箱UI
		// 参数0 : 宝箱实例ID
        public TSET_TARKOV_TREASURE_UI() : base(TSkillEffectType.TSET_TARKOV_TREASURE_UI) { }
    }
    #endregion SkillEffectConfig: 塔科夫-打开宝箱UI


    #region SkillEffectConfig: 获取玩家KDA数据-击杀次数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取玩家KDA数据-击杀次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-击杀次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-击杀次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PLAYER_KDA_KILL_COUNT : SkillEffectConfigNode
    {
		// 获取玩家KDA数据-击杀次数
		// 参数0 : 玩家PlayerIndex
        public TSET_GET_PLAYER_KDA_KILL_COUNT() : base(TSkillEffectType.TSET_GET_PLAYER_KDA_KILL_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取玩家KDA数据-击杀次数


    #region SkillEffectConfig: 获取玩家KDA数据-进入濒死状态次数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取玩家KDA数据-进入濒死状态次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-进入濒死状态次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-进入濒死状态次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PLAYER_KDA_DYING_COUNT : SkillEffectConfigNode
    {
		// 获取玩家KDA数据-进入濒死状态次数
		// 参数0 : 玩家PlayerIndex
        public TSET_GET_PLAYER_KDA_DYING_COUNT() : base(TSkillEffectType.TSET_GET_PLAYER_KDA_DYING_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取玩家KDA数据-进入濒死状态次数


    #region SkillEffectConfig: 获取玩家KDA数据-助攻次数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取玩家KDA数据-助攻次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-助攻次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取玩家KDA数据-助攻次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PLAYER_KDA_ASSIST_COUNT : SkillEffectConfigNode
    {
		// 获取玩家KDA数据-助攻次数
		// 参数0 : 玩家PlayerIndex
        public TSET_GET_PLAYER_KDA_ASSIST_COUNT() : base(TSkillEffectType.TSET_GET_PLAYER_KDA_ASSIST_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取玩家KDA数据-助攻次数


    #region SkillEffectConfig: 镜头FOV变化
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头FOV变化", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头FOV变化", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头FOV变化", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CAMERA_DEPTH_CHANGE_FOV : SkillEffectConfigNode
    {
		// 镜头FOV变化
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 优先级-TCameraPriority
		// 参数3 : 优先级插入方式-TPriorityInsertType
		// 参数4 : 设置FOV
		// 参数5 : 曲线ID
		// 参数6 : 移动时间（帧数）
		// 参数7 : 维持时间（帧数）
		// 参数8 : 是否切回
		// 参数9 : 切回时间（帧数）
		// 参数10 : 数值变化类型(增加是相对基础fov的)-TDepthChangeType
        public TSET_CAMERA_DEPTH_CHANGE_FOV() : base(TSkillEffectType.TSET_CAMERA_DEPTH_CHANGE_FOV) { }
    }
    #endregion SkillEffectConfig: 镜头FOV变化


    #region SkillEffectConfig: 召唤化身单位
    [Serializable]
	[NodeMenuItem("技能效果/召唤化身单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/召唤化身单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/召唤化身单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_AVATAR_UNIT : SkillEffectConfigNode
    {
		// 召唤化身单位
		// 参数0 : 角度
		// 参数1 : 位置X
		// 参数2 : 位置Y
		// 参数3 : 偏移位置X
		// 参数4 : 偏移位置Y
		// 参数5 : 召唤位置限制-TBattlePositionClampType
		// 参数6 : 延迟自动销毁帧数[0:不自动销毁]
		// 参数7 : 单位组-TSkillEntityGroupType
		// 参数8 : 出生前额外执行效果-SkillEffectConfig
		// 参数9 : 出生后额外执行效果-SkillEffectConfig
        public TSET_CREATE_AVATAR_UNIT() : base(TSkillEffectType.TSET_CREATE_AVATAR_UNIT) { }
    }
    #endregion SkillEffectConfig: 召唤化身单位


    #region SkillEffectConfig: 修改镜头基础FOV
    [Serializable]
	[NodeMenuItem("技能效果/相机/修改镜头基础FOV", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头基础FOV", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/修改镜头基础FOV", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_CAMERA_BASE_FOV : SkillEffectConfigNode
    {
		// 修改镜头基础FOV
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 设置基础FOV
		// 参数3 : 进入时间（帧数）
		// 参数4 : 维持时间（帧数）
		// 参数5 : 切回时间（帧数）
        public TSET_CHANGE_CAMERA_BASE_FOV() : base(TSkillEffectType.TSET_CHANGE_CAMERA_BASE_FOV) { }
    }
    #endregion SkillEffectConfig: 修改镜头基础FOV


    #region SkillEffectConfig: 塔科夫-关闭宝箱UI
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-关闭宝箱UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-关闭宝箱UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-关闭宝箱UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_CLOSE_TREASURE_UI : SkillEffectConfigNode
    {
		// 塔科夫-关闭宝箱UI
		// 参数0 : 宝箱实例ID
        public TSET_TARKOV_CLOSE_TREASURE_UI() : base(TSkillEffectType.TSET_TARKOV_CLOSE_TREASURE_UI) { }
    }
    #endregion SkillEffectConfig: 塔科夫-关闭宝箱UI


    #region SkillEffectConfig: 设置自定义基础待机移动动作
    [Serializable]
	[NodeMenuItem("技能效果/单位/设置自定义基础待机移动动作", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置自定义基础待机移动动作", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/设置自定义基础待机移动动作", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SWITCH_CUSTOM_BASE_IDLE_AND_MOVE_ANIM : SkillEffectConfigNode
    {
		// 设置自定义基础待机移动动作
		// 参数0 : 单位实例ID
		// 参数1 : 自定义基础待机动作-TRoleAnimType
		// 参数2 : 自定义基础移动动作-TRoleAnimType
		// 参数3 : 血条高度偏移
        public TSET_SWITCH_CUSTOM_BASE_IDLE_AND_MOVE_ANIM() : base(TSkillEffectType.TSET_SWITCH_CUSTOM_BASE_IDLE_AND_MOVE_ANIM) { }
    }
    #endregion SkillEffectConfig: 设置自定义基础待机移动动作


    #region SkillEffectConfig: 创建交互物
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/创建交互物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建交互物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/创建交互物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_MARKER : SkillEffectConfigNode
    {
		// 创建交互物
		// 参数0 : Marker配置ID-BattleMarkerConfig
		// 参数1 : 角度
		// 参数2 : 位置X
		// 参数3 : 位置Y
		// 参数4 : 位置限制-TBattlePositionClampType
        public TSET_CREATE_MARKER() : base(TSkillEffectType.TSET_CREATE_MARKER) { }
    }
    #endregion SkillEffectConfig: 创建交互物


    #region SkillEffectConfig: 交互CD进度条显示
    [Serializable]
	[NodeMenuItem("技能效果/UI/交互CD进度条显示", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/交互CD进度条显示", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/交互CD进度条显示", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_INTERACTIVE_CD_BAR : SkillEffectConfigNode
    {
		// 交互CD进度条显示
		// 参数0 : CD倒计时(帧数)
		// 参数1 : 显示文本
        public TSET_INTERACTIVE_CD_BAR() : base(TSkillEffectType.TSET_INTERACTIVE_CD_BAR) { }
    }
    #endregion SkillEffectConfig: 交互CD进度条显示


    #region SkillEffectConfig: 交互技能效果
    [Serializable]
	[NodeMenuItem("技能效果/技能/交互技能效果", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/交互技能效果", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/交互技能效果", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_INTERACTIVE_SKILL_EFFECT : SkillEffectConfigNode
    {
		// 交互技能效果
		// 参数0 : 交互实例ID
		// 参数1 : 交互技能效果ID(可不填)
        public TSET_INTERACTIVE_SKILL_EFFECT() : base(TSkillEffectType.TSET_INTERACTIVE_SKILL_EFFECT) { }
    }
    #endregion SkillEffectConfig: 交互技能效果


    #region SkillEffectConfig: 塔科夫-创建尸体宝箱
    [Serializable]
	[NodeMenuItem("技能效果/塔科夫玩法/塔科夫-创建尸体宝箱", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-创建尸体宝箱", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/塔科夫玩法/塔科夫-创建尸体宝箱", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TARKOV_CREATE_DEATH_TREASURE : SkillEffectConfigNode
    {
		// 塔科夫-创建尸体宝箱
		// 参数0 : 死亡单位实例ID
		// 参数1 : Marker配置ID-BattleMarkerConfig
		// 参数2 : 是否直接掉落物品
		// 参数3 : 掉落半径最小值
		// 参数4 : 掉落半径最大值
        public TSET_TARKOV_CREATE_DEATH_TREASURE() : base(TSkillEffectType.TSET_TARKOV_CREATE_DEATH_TREASURE) { }
    }
    #endregion SkillEffectConfig: 塔科夫-创建尸体宝箱


    #region SkillEffectConfig: 设置护体灵光条显示
    [Serializable]
	[NodeMenuItem("技能效果/UI/设置护体灵光条显示", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/设置护体灵光条显示", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/设置护体灵光条显示", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROTECT_BAR_SET_VISIBLE : SkillEffectConfigNode
    {
		// 设置护体灵光条显示
		// 参数0 : 是否显示(1:显示 0:隐藏)
        public TSET_PROTECT_BAR_SET_VISIBLE() : base(TSkillEffectType.TSET_PROTECT_BAR_SET_VISIBLE) { }
    }
    #endregion SkillEffectConfig: 设置护体灵光条显示


    #region SkillEffectConfig: 获取战斗带入时最初技能ID
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取战斗带入时最初技能ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取战斗带入时最初技能ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取战斗带入时最初技能ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_INIT_BASE_SKILL_ID : SkillEffectConfigNode
    {
		// 获取战斗带入时最初技能ID
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽类型-TSkillSlotType
        public TSET_GET_INIT_BASE_SKILL_ID() : base(TSkillEffectType.TSET_GET_INIT_BASE_SKILL_ID) { }
    }
    #endregion SkillEffectConfig: 获取战斗带入时最初技能ID


    #region SkillEffectConfig: 获取战斗带入时最初技能等级
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取战斗带入时最初技能等级", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取战斗带入时最初技能等级", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取战斗带入时最初技能等级", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_INIT_BASE_SKILL_LEVEL : SkillEffectConfigNode
    {
		// 获取战斗带入时最初技能等级
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽类型-TSkillSlotType
        public TSET_GET_INIT_BASE_SKILL_LEVEL() : base(TSkillEffectType.TSET_GET_INIT_BASE_SKILL_LEVEL) { }
    }
    #endregion SkillEffectConfig: 获取战斗带入时最初技能等级


    #region SkillEffectConfig: 显示技能蓄力读条UI
    [Serializable]
	[NodeMenuItem("技能效果/UI/显示技能蓄力读条UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示技能蓄力读条UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示技能蓄力读条UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_SKILL_BUILD_UP_PROGRESS_BAR_UI : SkillEffectConfigNode
    {
		// 显示技能蓄力读条UI
		// 参数0 : 是否显示[1显示_0关闭]
		// 参数1 : 读条时长[帧]
		// 参数2 : 读条样式[0短按_1长按]
        public TSET_SHOW_SKILL_BUILD_UP_PROGRESS_BAR_UI() : base(TSkillEffectType.TSET_SHOW_SKILL_BUILD_UP_PROGRESS_BAR_UI) { }
    }
    #endregion SkillEffectConfig: 显示技能蓄力读条UI


    #region SkillEffectConfig: 镜头基础FOV回到默认值
    [Serializable]
	[NodeMenuItem("技能效果/相机/镜头基础FOV回到默认值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头基础FOV回到默认值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/相机/镜头基础FOV回到默认值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SET_CAMERA_FOV_TO_DEFAULT : SkillEffectConfigNode
    {
		// 镜头基础FOV回到默认值
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 切回时间（帧数）
        public TSET_SET_CAMERA_FOV_TO_DEFAULT() : base(TSkillEffectType.TSET_SET_CAMERA_FOV_TO_DEFAULT) { }
    }
    #endregion SkillEffectConfig: 镜头基础FOV回到默认值


    #region SkillEffectConfig: 寻路网格-移除特殊区域
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/寻路网格-移除特殊区域", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-移除特殊区域", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-移除特殊区域", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REMOVE_NAVMEST_SPECIAL_AREA : SkillEffectConfigNode
    {
		// 寻路网格-移除特殊区域
		// 参数0 : 创建寻路网格特殊区域时的返回值
        public TSET_REMOVE_NAVMEST_SPECIAL_AREA() : base(TSkillEffectType.TSET_REMOVE_NAVMEST_SPECIAL_AREA) { }
    }
    #endregion SkillEffectConfig: 寻路网格-移除特殊区域


    #region SkillEffectConfig: 获取灵宠资质
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取灵宠资质", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取灵宠资质", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取灵宠资质", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PET_APTITUDE : SkillEffectConfigNode
    {
		// 获取灵宠资质
		// 参数0 : 单位实例ID
		// 参数1 : 御灵技能ID
        public TSET_GET_PET_APTITUDE() : base(TSkillEffectType.TSET_GET_PET_APTITUDE) { }
    }
    #endregion SkillEffectConfig: 获取灵宠资质


    #region SkillEffectConfig: 更换本命法宝
    [Serializable]
	[NodeMenuItem("技能效果/技能/更换本命法宝", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/更换本命法宝", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/更换本命法宝", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ORIN_TREASURE : SkillEffectConfigNode
    {
		// 更换本命法宝
		// 参数0 : 单位实例ID
		// 参数1 : 本命法宝ID
		// 参数2 : 本命法宝等级
        public TSET_CHANGE_ORIN_TREASURE() : base(TSkillEffectType.TSET_CHANGE_ORIN_TREASURE) { }
    }
    #endregion SkillEffectConfig: 更换本命法宝


    #region SkillEffectConfig: 寻路网格-修改区域寻路费用
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/寻路网格-修改区域寻路费用", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-修改区域寻路费用", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-修改区域寻路费用", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_NAVMEST_AREA_COST : SkillEffectConfigNode
    {
		// 寻路网格-修改区域寻路费用
		// 参数0 : 单位实例ID
		// 参数1 : 区域类型-TCollisionLayer
		// 参数2 : 寻路费用值(默认为1)
        public TSET_CHANGE_NAVMEST_AREA_COST() : base(TSkillEffectType.TSET_CHANGE_NAVMEST_AREA_COST) { }
    }
    #endregion SkillEffectConfig: 寻路网格-修改区域寻路费用


    #region SkillEffectConfig: 寻路网格-创建特殊区域(矩形)
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/寻路网格-创建特殊区域(矩形)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-创建特殊区域(矩形)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-创建特殊区域(矩形)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_NAVMEST_SPECIAL_AREA_RECT : SkillEffectConfigNode
    {
		// 寻路网格-创建特殊区域(矩形)
		// 参数0 : 区域类型-TCollisionLayer
		// 参数1 : 矩形中心坐标X
		// 参数2 : 矩形中心坐标Y
		// 参数3 : 矩形X长
		// 参数4 : 矩形Y长
		// 参数5 : 矩形旋转角度
        public TSET_CREATE_NAVMEST_SPECIAL_AREA_RECT() : base(TSkillEffectType.TSET_CREATE_NAVMEST_SPECIAL_AREA_RECT) { }
    }
    #endregion SkillEffectConfig: 寻路网格-创建特殊区域(矩形)


    #region SkillEffectConfig: 寻路网格-创建特殊区域(圆形)
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/寻路网格-创建特殊区域(圆形)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-创建特殊区域(圆形)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/寻路网格-创建特殊区域(圆形)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_NAVMEST_SPECIAL_AREA_CIRCLE : SkillEffectConfigNode
    {
		// 寻路网格-创建特殊区域(圆形)
		// 参数0 : 区域类型-TCollisionLayer
		// 参数1 : 矩形中心坐标X
		// 参数2 : 矩形中心坐标Y
		// 参数3 : 半径
        public TSET_CREATE_NAVMEST_SPECIAL_AREA_CIRCLE() : base(TSkillEffectType.TSET_CREATE_NAVMEST_SPECIAL_AREA_CIRCLE) { }
    }
    #endregion SkillEffectConfig: 寻路网格-创建特殊区域(圆形)


    #region SkillEffectConfig: 修改镜头视距
    [Serializable]
    public sealed partial class TSET_CAMERA_DISTANCE_CHANGE : SkillEffectConfigNode
    {
		// 修改镜头视距
		// 参数0 : 单位实例ID
		// 参数1 : 对谁生效(0表示都生效)-TViewEventTargetType
		// 参数2 : 优先级-TCameraPriority
		// 参数3 : 优先级插入方式-TPriorityInsertType
		// 参数4 : 视距（cm）
		// 参数5 : 曲线ID
		// 参数6 : 移动时间（帧数）
		// 参数7 : 维持时间（帧数）
		// 参数8 : 是否切回
		// 参数9 : 切回时间（帧数）
		// 参数10 : 视距变化类型-TDepthChangeType
        public TSET_CAMERA_DISTANCE_CHANGE() : base(TSkillEffectType.TSET_CAMERA_DISTANCE_CHANGE) { }
    }
    #endregion SkillEffectConfig: 修改镜头视距


    #region SkillEffectConfig: 添加心法
    [Serializable]
	[NodeMenuItem("技能效果/技能/添加心法", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/添加心法", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/添加心法", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_ADD_XIN_FA : SkillEffectConfigNode
    {
		// 添加心法
		// 参数0 : 单位实例ID
		// 参数1 : 心法表ID(SkillXinfaConfig)
		// 参数2 : 心法槽类型-TechniquesSlotType
        public TSET_ADD_XIN_FA() : base(TSkillEffectType.TSET_ADD_XIN_FA) { }
    }
    #endregion SkillEffectConfig: 添加心法


    #region SkillEffectConfig: 移除阻挡边界(由节点创建的)
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/移除阻挡边界(由节点创建的)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/移除阻挡边界(由节点创建的)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/移除阻挡边界(由节点创建的)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_REMOVE_MAP_EDGE : SkillEffectConfigNode
    {
		// 移除阻挡边界(由节点创建的)
		// 参数0 : 创建阻挡边界时的返回值(BodyKey)
        public TSET_REMOVE_MAP_EDGE() : base(TSkillEffectType.TSET_REMOVE_MAP_EDGE) { }
    }
    #endregion SkillEffectConfig: 移除阻挡边界(由节点创建的)


    #region SkillEffectConfig: 伤害流程_物理闪避检查
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_物理闪避检查", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理闪避检查", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理闪避检查", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750038 : SkillEffectConfigNode
    {
		// 伤害流程_物理闪避检查
        public TSET_PROC_DAMAGE_1750038() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750038) { }
    }
    #endregion SkillEffectConfig: 伤害流程_物理闪避检查


    #region SkillEffectConfig: 伤害流程_物理格挡检查
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_物理格挡检查", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理格挡检查", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理格挡检查", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750043 : SkillEffectConfigNode
    {
		// 伤害流程_物理格挡检查
        public TSET_PROC_DAMAGE_1750043() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750043) { }
    }
    #endregion SkillEffectConfig: 伤害流程_物理格挡检查


    #region SkillEffectConfig: 伤害流程_物理暴击检查
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_物理暴击检查", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理暴击检查", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理暴击检查", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750044 : SkillEffectConfigNode
    {
		// 伤害流程_物理暴击检查
        public TSET_PROC_DAMAGE_1750044() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750044) { }
    }
    #endregion SkillEffectConfig: 伤害流程_物理暴击检查


    #region SkillEffectConfig: 伤害流程_设置技能条件增伤伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_设置技能条件增伤伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_设置技能条件增伤伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_设置技能条件增伤伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_146003303 : SkillEffectConfigNode
    {
		// 伤害流程_设置技能条件增伤伤害加成
        public TSET_PROC_DAMAGE_146003303() : base(TSkillEffectType.TSET_PROC_DAMAGE_146003303) { }
    }
    #endregion SkillEffectConfig: 伤害流程_设置技能条件增伤伤害加成


    #region SkillEffectConfig: 伤害流程_计算护主条伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算护主条伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算护主条伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算护主条伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_146002819 : SkillEffectConfigNode
    {
		// 伤害流程_计算护主条伤害加成
        public TSET_PROC_DAMAGE_146002819() : base(TSkillEffectType.TSET_PROC_DAMAGE_146002819) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算护主条伤害加成


    #region SkillEffectConfig: 伤害流程_物理吸血
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_物理吸血", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理吸血", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_物理吸血", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750080 : SkillEffectConfigNode
    {
		// 伤害流程_物理吸血
        public TSET_PROC_DAMAGE_1750080() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750080) { }
    }
    #endregion SkillEffectConfig: 伤害流程_物理吸血


    #region SkillEffectConfig: 伤害流程_移除状态标识
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_移除状态标识", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_移除状态标识", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_移除状态标识", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750075 : SkillEffectConfigNode
    {
		// 伤害流程_移除状态标识
        public TSET_PROC_DAMAGE_1750075() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750075) { }
    }
    #endregion SkillEffectConfig: 伤害流程_移除状态标识


    #region SkillEffectConfig: 伤害流程_计算物理攻防差值
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理攻防差值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理攻防差值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理攻防差值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750040 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理攻防差值
        public TSET_PROC_DAMAGE_1750040() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750040) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理攻防差值


    #region SkillEffectConfig: 伤害流程_计算技能威力系数
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算技能威力系数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能威力系数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能威力系数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66001144 : SkillEffectConfigNode
    {
		// 伤害流程_计算技能威力系数
        public TSET_PROC_DAMAGE_66001144() : base(TSkillEffectType.TSET_PROC_DAMAGE_66001144) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算技能威力系数


    #region SkillEffectConfig: 伤害流程_计算技能额外伤害
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算技能额外伤害", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能额外伤害", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能额外伤害", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66001191 : SkillEffectConfigNode
    {
		// 伤害流程_计算技能额外伤害
        public TSET_PROC_DAMAGE_66001191() : base(TSkillEffectType.TSET_PROC_DAMAGE_66001191) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算技能额外伤害


    #region SkillEffectConfig: 伤害流程_计算物理暴击伤害系数
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理暴击伤害系数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理暴击伤害系数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理暴击伤害系数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750050 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理暴击伤害系数
        public TSET_PROC_DAMAGE_1750050() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750050) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理暴击伤害系数


    #region SkillEffectConfig: 伤害流程_计算物理化解伤害系数
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理化解伤害系数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理化解伤害系数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理化解伤害系数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000493 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理化解伤害系数
        public TSET_PROC_DAMAGE_66000493() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000493) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理化解伤害系数


    #region SkillEffectConfig: 伤害流程_计算物理基础伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理基础伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理基础伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理基础伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750058 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理基础伤害加成
        public TSET_PROC_DAMAGE_1750058() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750058) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理基础伤害加成


    #region SkillEffectConfig: 伤害流程_PVP系统伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_PVP系统伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_PVP系统伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_PVP系统伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000983 : SkillEffectConfigNode
    {
		// 伤害流程_PVP系统伤害加成
        public TSET_PROC_DAMAGE_66000983() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000983) { }
    }
    #endregion SkillEffectConfig: 伤害流程_PVP系统伤害加成


    #region SkillEffectConfig: 伤害流程_PVE系统伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_PVE系统伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_PVE系统伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_PVE系统伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000984 : SkillEffectConfigNode
    {
		// 伤害流程_PVE系统伤害加成
        public TSET_PROC_DAMAGE_66000984() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000984) { }
    }
    #endregion SkillEffectConfig: 伤害流程_PVE系统伤害加成


    #region SkillEffectConfig: 伤害流程_计算物理附加伤害值
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理附加伤害值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理附加伤害值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理附加伤害值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750049 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理附加伤害值
        public TSET_PROC_DAMAGE_1750049() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750049) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理附加伤害值


    #region SkillEffectConfig: 伤害流程_计算物理技能类型攻防附加伤害值
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理技能类型攻防附加伤害值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理技能类型攻防附加伤害值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理技能类型攻防附加伤害值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750051 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理技能类型攻防附加伤害值
        public TSET_PROC_DAMAGE_1750051() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750051) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理技能类型攻防附加伤害值


    #region SkillEffectConfig: 伤害流程_计算物理生命附加伤害值
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理生命附加伤害值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理生命附加伤害值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理生命附加伤害值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750052 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理生命附加伤害值
        public TSET_PROC_DAMAGE_1750052() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750052) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理生命附加伤害值


    #region SkillEffectConfig: 伤害流程_计算物理总伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理总伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理总伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理总伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750054 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理总伤害加成
        public TSET_PROC_DAMAGE_1750054() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750054) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理总伤害加成


    #region SkillEffectConfig: 伤害流程_技能形态伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_技能形态伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能形态伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能形态伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000976 : SkillEffectConfigNode
    {
		// 伤害流程_技能形态伤害加成
        public TSET_PROC_DAMAGE_66000976() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000976) { }
    }
    #endregion SkillEffectConfig: 伤害流程_技能形态伤害加成


    #region SkillEffectConfig: 伤害流程_技能距离伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_技能距离伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能距离伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能距离伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000979 : SkillEffectConfigNode
    {
		// 伤害流程_技能距离伤害加成
        public TSET_PROC_DAMAGE_66000979() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000979) { }
    }
    #endregion SkillEffectConfig: 伤害流程_技能距离伤害加成


    #region SkillEffectConfig: 伤害流程_技能异常状态伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_技能异常状态伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能异常状态伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_技能异常状态伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000980 : SkillEffectConfigNode
    {
		// 伤害流程_技能异常状态伤害加成
        public TSET_PROC_DAMAGE_66000980() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000980) { }
    }
    #endregion SkillEffectConfig: 伤害流程_技能异常状态伤害加成


    #region SkillEffectConfig: 伤害流程_五行伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_五行伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_五行伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_五行伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000981 : SkillEffectConfigNode
    {
		// 伤害流程_五行伤害加成
        public TSET_PROC_DAMAGE_66000981() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000981) { }
    }
    #endregion SkillEffectConfig: 伤害流程_五行伤害加成


    #region SkillEffectConfig: 伤害流程_神识海伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_神识海伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_神识海伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_神识海伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000982 : SkillEffectConfigNode
    {
		// 伤害流程_神识海伤害加成
        public TSET_PROC_DAMAGE_66000982() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000982) { }
    }
    #endregion SkillEffectConfig: 伤害流程_神识海伤害加成


    #region SkillEffectConfig: 伤害流程_计算物理种族伤害加成
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理种族伤害加成", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理种族伤害加成", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理种族伤害加成", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750056 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理种族伤害加成
        public TSET_PROC_DAMAGE_1750056() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750056) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理种族伤害加成


    #region SkillEffectConfig: 伤害流程_计算物理神念值伤害加成系数
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算物理神念值伤害加成系数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理神念值伤害加成系数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算物理神念值伤害加成系数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_1750059 : SkillEffectConfigNode
    {
		// 伤害流程_计算物理神念值伤害加成系数
        public TSET_PROC_DAMAGE_1750059() : base(TSkillEffectType.TSET_PROC_DAMAGE_1750059) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算物理神念值伤害加成系数


    #region SkillEffectConfig: 获取数值Clamp值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取数值Clamp值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取数值Clamp值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取数值Clamp值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_CLAMP_VALUE : SkillEffectConfigNode
    {
		// 获取数值Clamp值
		// 参数0 : 原数值
		// 参数1 : 下限值
		// 参数2 : 上限值
        public TSET_GET_CLAMP_VALUE() : base(TSkillEffectType.TSET_GET_CLAMP_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取数值Clamp值


    #region SkillEffectConfig: 获取单位碰撞半径
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取单位碰撞半径", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位碰撞半径", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取单位碰撞半径", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_FIXTURE_RADIUS : SkillEffectConfigNode
    {
		// 获取单位碰撞半径
		// 参数0 : 单位实例ID
        public TSET_GET_ENTITY_FIXTURE_RADIUS() : base(TSkillEffectType.TSET_GET_ENTITY_FIXTURE_RADIUS) { }
    }
    #endregion SkillEffectConfig: 获取单位碰撞半径


    #region SkillEffectConfig: 伤害流程_计算技能基础伤害
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算技能基础伤害", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能基础伤害", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算技能基础伤害", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_175000327 : SkillEffectConfigNode
    {
		// 伤害流程_计算技能基础伤害
        public TSET_PROC_DAMAGE_175000327() : base(TSkillEffectType.TSET_PROC_DAMAGE_175000327) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算技能基础伤害


    #region SkillEffectConfig: 伤害流程_计算理论伤害
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算理论伤害", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算理论伤害", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算理论伤害", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_175000300 : SkillEffectConfigNode
    {
		// 伤害流程_计算理论伤害
        public TSET_PROC_DAMAGE_175000300() : base(TSkillEffectType.TSET_PROC_DAMAGE_175000300) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算理论伤害


    #region SkillEffectConfig: 伤害流程_计算保底伤害
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/伤害流程_计算保底伤害", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算保底伤害", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/伤害流程_计算保底伤害", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROC_DAMAGE_66000969 : SkillEffectConfigNode
    {
		// 伤害流程_计算保底伤害
        public TSET_PROC_DAMAGE_66000969() : base(TSkillEffectType.TSET_PROC_DAMAGE_66000969) { }
    }
    #endregion SkillEffectConfig: 伤害流程_计算保底伤害


    #region SkillEffectConfig: 设置技能韧性条显示
    [Serializable]
	[NodeMenuItem("技能效果/技能/设置技能韧性条显示", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能韧性条显示", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能韧性条显示", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_RESILIENCE_SET_VISIBLE : SkillEffectConfigNode
    {
		// 设置技能韧性条显示
		// 参数0 : 是否显示(1:显示 0:隐藏)
		// 参数1 : UI挂点位置-AttachPos
        public TSET_SKILL_RESILIENCE_SET_VISIBLE() : base(TSkillEffectType.TSET_SKILL_RESILIENCE_SET_VISIBLE) { }
    }
    #endregion SkillEffectConfig: 设置技能韧性条显示


    #region SkillEffectConfig: 设置技能吟唱条显示
    [Serializable]
	[NodeMenuItem("技能效果/技能/设置技能吟唱条显示", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能吟唱条显示", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/设置技能吟唱条显示", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SKILL_CHANT_SET_VISIBLE : SkillEffectConfigNode
    {
		// 设置技能吟唱条显示
		// 参数0 : 是否显示(1:显示 0:隐藏)
		// 参数1 : 技能ID
        public TSET_SKILL_CHANT_SET_VISIBLE() : base(TSkillEffectType.TSET_SKILL_CHANT_SET_VISIBLE) { }
    }
    #endregion SkillEffectConfig: 设置技能吟唱条显示


    #region SkillEffectConfig: 获取技能护体灵光破除值
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能护体灵光破除值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能护体灵光破除值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能护体灵光破除值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_LG_DAMAGE_VALUE : SkillEffectConfigNode
    {
		// 获取技能护体灵光破除值
		// 参数0 : 下标Index(从0开始编号)
        public TSET_GET_SKILL_LG_DAMAGE_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_LG_DAMAGE_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能护体灵光破除值


    #region SkillEffectConfig: 标记本次技能已反制成功
    [Serializable]
	[NodeMenuItem("技能效果/技能/标记本次技能已反制成功", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标记本次技能已反制成功", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/标记本次技能已反制成功", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARK_HURT_SKILL_CHANT_COUNTER : SkillEffectConfigNode
    {
		// 标记本次技能已反制成功
		// 参数0 : 受击者单位实例ID
		// 参数1 : 攻击者实例ID
		// 参数2 : 伤害来源技能ID
		// 参数3 : 伤害来源技能使用开始时间(帧)
        public TSET_MARK_HURT_SKILL_CHANT_COUNTER() : base(TSkillEffectType.TSET_MARK_HURT_SKILL_CHANT_COUNTER) { }
    }
    #endregion SkillEffectConfig: 标记本次技能已反制成功


    #region SkillEffectConfig: 退出战斗
    [Serializable]
	[NodeMenuItem("技能效果/单位/退出战斗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/退出战斗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/退出战斗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_LEAVE_BATTLE : SkillEffectConfigNode
    {
		// 退出战斗
		// 参数0 : 单位实例ID
        public TSET_LEAVE_BATTLE() : base(TSkillEffectType.TSET_LEAVE_BATTLE) { }
    }
    #endregion SkillEffectConfig: 退出战斗


    #region SkillEffectConfig: 开启技能参数历史值记录
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/开启技能参数历史值记录", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/开启技能参数历史值记录", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/开启技能参数历史值记录", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_OPEN_RECORD_HISTORY_SKILL_TAG_VALUE : SkillEffectConfigNode
    {
		// 开启技能参数历史值记录
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 技能参数类型-TSkillTagsType
		// 参数4 : 记录历史值时长帧(填0关闭)
        public TSET_OPEN_RECORD_HISTORY_SKILL_TAG_VALUE() : base(TSkillEffectType.TSET_OPEN_RECORD_HISTORY_SKILL_TAG_VALUE) { }
    }
    #endregion SkillEffectConfig: 开启技能参数历史值记录


    #region SkillEffectConfig: 获取技能参数历史值
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取技能参数历史值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能参数历史值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取技能参数历史值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_TAG_HISTORY_VALUE : SkillEffectConfigNode
    {
		// 获取技能参数历史值
		// 参数0 : 技能拥有者单位
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
		// 参数2 : 技能参数ID-SkillTagsConfig
		// 参数3 : 技能参数类型-TSkillTagsType
		// 参数4 : 第N帧前(例如:填1表示上一帧的值)
        public TSET_GET_SKILL_TAG_HISTORY_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_TAG_HISTORY_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能参数历史值


    #region SkillEffectConfig: 采集交互物
    [Serializable]
	[NodeMenuItem("未配置-模块/采集交互物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/未配置-模块/采集交互物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/未配置-模块/采集交互物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GATHER_MARKER : SkillEffectConfigNode
    {
		// 采集交互物
		// 参数0 : 交互物Marker实例ID
		// 参数1 : 采集者实例ID
        public TSET_GATHER_MARKER() : base(TSkillEffectType.TSET_GATHER_MARKER) { }
    }
    #endregion SkillEffectConfig: 采集交互物


    #region SkillEffectConfig: 获取技能资源类型
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能资源类型", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能资源类型", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能资源类型", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_SCHOOL_RES_TYPE : SkillEffectConfigNode
    {
		// 获取技能资源类型
		// 参数0 : 单位实例ID
		// 参数1 : 技能ID(大于0:表格ID,小于0:实例ID)
        public TSET_GET_SKILL_SCHOOL_RES_TYPE() : base(TSkillEffectType.TSET_GET_SKILL_SCHOOL_RES_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取技能资源类型


    #region SkillEffectConfig: 获取技能吟唱反制打断值
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能吟唱反制打断值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能吟唱反制打断值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能吟唱反制打断值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_CHANT_COUNTER_VALUE : SkillEffectConfigNode
    {
		// 获取技能吟唱反制打断值
		// 参数0 : 下标Index(从0开始编号)
        public TSET_GET_SKILL_CHANT_COUNTER_VALUE() : base(TSkillEffectType.TSET_GET_SKILL_CHANT_COUNTER_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取技能吟唱反制打断值


    #region SkillEffectConfig: 创建副本通关宝箱掉落物
    [Serializable]
	[NodeMenuItem("技能效果/创建副本通关宝箱掉落物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/创建副本通关宝箱掉落物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/创建副本通关宝箱掉落物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_DUNGEON_WIN_DROP_ITEM : SkillEffectConfigNode
    {
		// 创建副本通关宝箱掉落物
		// 参数0 : 坐标X
		// 参数1 : 坐标Y
		// 参数2 : 自定义掉落时长[帧]
		// 参数3 : 自定义等待时长[帧]
		// 参数4 : 自定义拾取时长[帧]
		// 参数5 : 玩家索引
		// 参数6 : 延迟自动退出战斗(毫秒)
		// 参数7 : 开启后是否销毁宝箱
        public TSET_CREATE_DUNGEON_WIN_DROP_ITEM() : base(TSkillEffectType.TSET_CREATE_DUNGEON_WIN_DROP_ITEM) { }
    }
    #endregion SkillEffectConfig: 创建副本通关宝箱掉落物


    #region SkillEffectConfig: 显示复活弹窗
    [Serializable]
	[NodeMenuItem("技能效果/显示复活弹窗", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/显示复活弹窗", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/显示复活弹窗", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_REBORN_WINDOW : SkillEffectConfigNode
    {
		// 显示复活弹窗
		// 参数0 : 显示玩家
        public TSET_SHOW_REBORN_WINDOW() : base(TSkillEffectType.TSET_SHOW_REBORN_WINDOW) { }
    }
    #endregion SkillEffectConfig: 显示复活弹窗


    #region SkillEffectConfig: 重置队伍可复活次数
    [Serializable]
	[NodeMenuItem("技能效果/重置队伍可复活次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/重置队伍可复活次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/重置队伍可复活次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RESET_TEAM_CAN_REBORN_COUNT : SkillEffectConfigNode
    {
		// 重置队伍可复活次数
		// 参数0 : 阵营
		// 参数1 : 可复活次数
        public TSET_RESET_TEAM_CAN_REBORN_COUNT() : base(TSkillEffectType.TSET_RESET_TEAM_CAN_REBORN_COUNT) { }
    }
    #endregion SkillEffectConfig: 重置队伍可复活次数


    #region SkillEffectConfig: 获取队伍可复活次数
    [Serializable]
	[NodeMenuItem("技能效果/获取队伍可复活次数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取队伍可复活次数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取队伍可复活次数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_TEAM_CAN_REBORN_COUNT : SkillEffectConfigNode
    {
		// 获取队伍可复活次数
		// 参数0 : 阵营
        public TSET_GET_TEAM_CAN_REBORN_COUNT() : base(TSkillEffectType.TSET_GET_TEAM_CAN_REBORN_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取队伍可复活次数


    #region SkillEffectConfig: 修改角色动作播放速度
    [Serializable]
	[NodeMenuItem("技能效果/纯表现/修改角色动作播放速度", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改角色动作播放速度", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/纯表现/修改角色动作播放速度", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CHANGE_ROLE_ANIM_SPEED : SkillEffectConfigNode
    {
		// 修改角色动作播放速度
		// 参数0 : 动作播放单位
		// 参数1 : 动作枚举-TRoleAnimType
		// 参数2 : 动画速度百分比
		// 参数3 : 是否受急速属性影响(加快动画速度)
		// 参数4 : 动画播放时间-帧（反推播放速度）
        public TSET_CHANGE_ROLE_ANIM_SPEED() : base(TSkillEffectType.TSET_CHANGE_ROLE_ANIM_SPEED) { }
    }
    #endregion SkillEffectConfig: 修改角色动作播放速度


    #region SkillEffectConfig: 战斗任务-切换任务描述
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/战斗任务-切换任务描述", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-切换任务描述", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-切换任务描述", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_MISSION_SWITCH_MISSION_ONE_DESC : SkillEffectConfigNode
    {
		// 战斗任务-切换任务描述
		// 参数0 : 指定单任务下标Index(从0开始)
		// 参数1 : 指定描述下标Index(从0开始)
        public TSET_BATTLE_MISSION_SWITCH_MISSION_ONE_DESC() : base(TSkillEffectType.TSET_BATTLE_MISSION_SWITCH_MISSION_ONE_DESC) { }
    }
    #endregion SkillEffectConfig: 战斗任务-切换任务描述


    #region SkillEffectConfig: 获取五行伤害加成万分比
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取五行伤害加成万分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行伤害加成万分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行伤害加成万分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_ATK_PER : SkillEffectConfigNode
    {
		// 获取五行伤害加成万分比
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_ATK_PER() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_ATK_PER) { }
    }
    #endregion SkillEffectConfig: 获取五行伤害加成万分比


    #region SkillEffectConfig: 获取五行伤害减免万分比
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取五行伤害减免万分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行伤害减免万分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取五行伤害减免万分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ENTITY_ELEMENTS_DEF_PER : SkillEffectConfigNode
    {
		// 获取五行伤害减免万分比
		// 参数0 : 被获取值的单位
		// 参数1 : 五行类型-TElementsType
        public TSET_GET_ENTITY_ELEMENTS_DEF_PER() : base(TSkillEffectType.TSET_GET_ENTITY_ELEMENTS_DEF_PER) { }
    }
    #endregion SkillEffectConfig: 获取五行伤害减免万分比


    #region SkillEffectConfig: 大秘境-开启房间传送点
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大秘境-开启房间传送点", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-开启房间传送点", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-开启房间传送点", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT : SkillEffectConfigNode
    {
		// 大秘境-开启房间传送点
		// 参数0 : 房间Index
        public TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT() : base(TSkillEffectType.TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT) { }
    }
    #endregion SkillEffectConfig: 大秘境-开启房间传送点


    #region SkillEffectConfig: 大秘境-房间传送
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大秘境-房间传送", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-房间传送", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-房间传送", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_DO_ROOM_TELEPORT : SkillEffectConfigNode
    {
		// 大秘境-房间传送
		// 参数0 : 传送点实例ID
		// 参数1 : 被传送者实例ID
        public TSET_BIG_DUNGEON_DO_ROOM_TELEPORT() : base(TSkillEffectType.TSET_BIG_DUNGEON_DO_ROOM_TELEPORT) { }
    }
    #endregion SkillEffectConfig: 大秘境-房间传送


    #region SkillEffectConfig: Marker-战斗单位点-刷出单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/Marker-战斗单位点-刷出单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-战斗单位点-刷出单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-战斗单位点-刷出单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_UNIT_POINT_CREATE_ROLE : SkillEffectConfigNode
    {
		// Marker-战斗单位点-刷出单位
		// 参数0 : 指定MarkerID
		// 参数1 : 来源触发器ID（被哪个触发器激活）
        public TSET_MARKER_UNIT_POINT_CREATE_ROLE() : base(TSkillEffectType.TSET_MARKER_UNIT_POINT_CREATE_ROLE) { }
    }
    #endregion SkillEffectConfig: Marker-战斗单位点-刷出单位


    #region SkillEffectConfig: Marker-点位组-刷出单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/Marker-点位组-刷出单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-点位组-刷出单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-点位组-刷出单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_GROUP_POINT_CREATE_ROLE : SkillEffectConfigNode
    {
		// Marker-点位组-刷出单位
		// 参数0 : 指定点位组MarkerID
		// 参数1 : 来源触发器ID（被哪个触发器激活）
        public TSET_MARKER_GROUP_POINT_CREATE_ROLE() : base(TSkillEffectType.TSET_MARKER_GROUP_POINT_CREATE_ROLE) { }
    }
    #endregion SkillEffectConfig: Marker-点位组-刷出单位


    #region SkillEffectConfig: Marker-激活触发器
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/Marker-激活触发器", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-激活触发器", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-激活触发器", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_ACTIVE_TRIGGER : SkillEffectConfigNode
    {
		// Marker-激活触发器
		// 参数0 : 指定触发器MarkerID
		// 参数1 : 来源触发器ID（被哪个触发器激活）
        public TSET_MARKER_ACTIVE_TRIGGER() : base(TSkillEffectType.TSET_MARKER_ACTIVE_TRIGGER) { }
    }
    #endregion SkillEffectConfig: Marker-激活触发器


    #region SkillEffectConfig: 获取模板参数列表总数量
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/获取模板参数列表总数量", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取模板参数列表总数量", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取模板参数列表总数量", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_EXTRA_PARAM_COUNT : SkillEffectConfigNode
    {
		// 获取模板参数列表总数量
        public TSET_GET_EXTRA_PARAM_COUNT() : base(TSkillEffectType.TSET_GET_EXTRA_PARAM_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取模板参数列表总数量


    #region SkillEffectConfig: 获取模板参数值
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/获取模板参数值", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取模板参数值", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/获取模板参数值", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_EXTRA_PARAM_VALUE : SkillEffectConfigNode
    {
		// 获取模板参数值
		// 参数0 : 模板参数序号(从1开始)
        public TSET_GET_EXTRA_PARAM_VALUE() : base(TSkillEffectType.TSET_GET_EXTRA_PARAM_VALUE) { }
    }
    #endregion SkillEffectConfig: 获取模板参数值


    #region SkillEffectConfig: 大秘境-进入最终阶段(BOSS)
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/大秘境-进入最终阶段(BOSS)", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-进入最终阶段(BOSS)", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/大秘境-进入最终阶段(BOSS)", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_ENTER_FINAL_STAGE : SkillEffectConfigNode
    {
		// 大秘境-进入最终阶段(BOSS)
        public TSET_BIG_DUNGEON_ENTER_FINAL_STAGE() : base(TSkillEffectType.TSET_BIG_DUNGEON_ENTER_FINAL_STAGE) { }
    }
    #endregion SkillEffectConfig: 大秘境-进入最终阶段(BOSS)


    #region SkillEffectConfig: Marker-获取单位创建时来源触发器MarkerID
    [Serializable]
	[NodeMenuItem("技能效果/单位/Marker-获取单位创建时来源触发器MarkerID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-获取单位创建时来源触发器MarkerID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-获取单位创建时来源触发器MarkerID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_GET_MONSTER_FROM_TRIGGER_ID : SkillEffectConfigNode
    {
		// Marker-获取单位创建时来源触发器MarkerID
		// 参数0 : 单位实例ID
        public TSET_MARKER_GET_MONSTER_FROM_TRIGGER_ID() : base(TSkillEffectType.TSET_MARKER_GET_MONSTER_FROM_TRIGGER_ID) { }
    }
    #endregion SkillEffectConfig: Marker-获取单位创建时来源触发器MarkerID


    #region SkillEffectConfig: 获取技能槽位-通过权重
    [Serializable]
	[NodeMenuItem("技能效果/技能/获取技能槽位-通过权重", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能槽位-通过权重", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/获取技能槽位-通过权重", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SKILL_SLOT_BY_WEIGHT : SkillEffectConfigNode
    {
		// 获取技能槽位-通过权重
		// 参数0 : 单位实例ID
		// 参数1 : 获取类型(0:随机_1:最大权重_2:最小权重)
		// 参数2 : 是否包含功法
		// 参数3 : 是否包含AI标签-特殊
        public TSET_GET_SKILL_SLOT_BY_WEIGHT() : base(TSkillEffectType.TSET_GET_SKILL_SLOT_BY_WEIGHT) { }
    }
    #endregion SkillEffectConfig: 获取技能槽位-通过权重


    #region SkillEffectConfig: Marker-标记为交互中
    [Serializable]
	[NodeMenuItem("技能效果/单位/Marker-标记为交互中", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-标记为交互中", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-标记为交互中", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_SET_IN_INTERACTIVE_ING : SkillEffectConfigNode
    {
		// Marker-标记为交互中
		// 参数0 : Marker单位实例ID
		// 参数1 : 交互者实例ID
		// 参数2 : 是否独立交互(多玩家各自标记交互中)
        public TSET_MARKER_SET_IN_INTERACTIVE_ING() : base(TSkillEffectType.TSET_MARKER_SET_IN_INTERACTIVE_ING) { }
    }
    #endregion SkillEffectConfig: Marker-标记为交互中


    #region SkillEffectConfig: Marker-取消交互中标记
    [Serializable]
	[NodeMenuItem("技能效果/单位/Marker-取消交互中标记", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-取消交互中标记", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-取消交互中标记", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_CANCEL_IN_INTERACTIVE_ING : SkillEffectConfigNode
    {
		// Marker-取消交互中标记
		// 参数0 : Marker单位实例ID
		// 参数1 : 交互者实例ID
		// 参数2 : 是否独立交互(多玩家各自标记交互中)
        public TSET_MARKER_CANCEL_IN_INTERACTIVE_ING() : base(TSkillEffectType.TSET_MARKER_CANCEL_IN_INTERACTIVE_ING) { }
    }
    #endregion SkillEffectConfig: Marker-取消交互中标记


    #region SkillEffectConfig: Marker-标记为不可交互
    [Serializable]
	[NodeMenuItem("技能效果/单位/Marker-标记为不可交互", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-标记为不可交互", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-标记为不可交互", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_SET_INTERACTIVE_ED : SkillEffectConfigNode
    {
		// Marker-标记为不可交互
		// 参数0 : Marker单位实例ID
		// 参数1 : 交互者实例ID
		// 参数2 : 是否独立交互(多玩家各自标记不可交互)
        public TSET_MARKER_SET_INTERACTIVE_ED() : base(TSkillEffectType.TSET_MARKER_SET_INTERACTIVE_ED) { }
    }
    #endregion SkillEffectConfig: Marker-标记为不可交互


    #region SkillEffectConfig: Marker-创建随机祭坛
    [Serializable]
	[NodeMenuItem("技能效果/单位/Marker-创建随机祭坛", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-创建随机祭坛", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/Marker-创建随机祭坛", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_MARKER_CREATE_ALTAR : SkillEffectConfigNode
    {
		// Marker-创建随机祭坛
        public TSET_MARKER_CREATE_ALTAR() : base(TSkillEffectType.TSET_MARKER_CREATE_ALTAR) { }
    }
    #endregion SkillEffectConfig: Marker-创建随机祭坛


    #region SkillEffectConfig: 大秘境-改变房间出现五行的权重
    [Serializable]
	[NodeMenuItem("技能效果/大秘境玩法/大秘境-改变房间出现五行的权重", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-改变房间出现五行的权重", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-改变房间出现五行的权重", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_CHANGE_ROOM_ELEMENTS_WEIGHT : SkillEffectConfigNode
    {
		// 大秘境-改变房间出现五行的权重
		// 参数0 : 五行-TElementsType
		// 参数1 : 运算符-TBattleSetAttrOperationType
		// 参数2 : 权重百分比
        public TSET_BIG_DUNGEON_CHANGE_ROOM_ELEMENTS_WEIGHT() : base(TSkillEffectType.TSET_BIG_DUNGEON_CHANGE_ROOM_ELEMENTS_WEIGHT) { }
    }
    #endregion SkillEffectConfig: 大秘境-改变房间出现五行的权重


    #region SkillEffectConfig: 大秘境-获取房间出现五行的权重
    [Serializable]
	[NodeMenuItem("技能效果/大秘境玩法/大秘境-获取房间出现五行的权重", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-获取房间出现五行的权重", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-获取房间出现五行的权重", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_GET_ROOM_ELEMENTS_WEIGHT : SkillEffectConfigNode
    {
		// 大秘境-获取房间出现五行的权重
		// 参数0 : 五行-TElementsType
        public TSET_BIG_DUNGEON_GET_ROOM_ELEMENTS_WEIGHT() : base(TSkillEffectType.TSET_BIG_DUNGEON_GET_ROOM_ELEMENTS_WEIGHT) { }
    }
    #endregion SkillEffectConfig: 大秘境-获取房间出现五行的权重


    #region SkillEffectConfig: 大秘境-开启房间传送投票
    [Serializable]
	[NodeMenuItem("技能效果/大秘境玩法/大秘境-开启房间传送投票", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-开启房间传送投票", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-开启房间传送投票", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT_VOTE : SkillEffectConfigNode
    {
		// 大秘境-开启房间传送投票
		// 参数0 : 玩家PlayerIndex
		// 参数1 : 传送门实例ID
		// 参数2 : 传送所需最少同意人数
		// 参数3 : 投票时长[帧]
        public TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT_VOTE() : base(TSkillEffectType.TSET_BIG_DUNGEON_OPEN_ROOM_TELEPORT_VOTE) { }
    }
    #endregion SkillEffectConfig: 大秘境-开启房间传送投票


    #region SkillEffectConfig: 大秘境-关闭房间传送投票
    [Serializable]
	[NodeMenuItem("技能效果/大秘境玩法/大秘境-关闭房间传送投票", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-关闭房间传送投票", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-关闭房间传送投票", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_CLOSE_ROOM_TELEPORT_VOTE : SkillEffectConfigNode
    {
		// 大秘境-关闭房间传送投票
		// 参数0 : 投票是否成功
        public TSET_BIG_DUNGEON_CLOSE_ROOM_TELEPORT_VOTE() : base(TSkillEffectType.TSET_BIG_DUNGEON_CLOSE_ROOM_TELEPORT_VOTE) { }
    }
    #endregion SkillEffectConfig: 大秘境-关闭房间传送投票


    #region SkillEffectConfig: 大秘境-获取正在房间传送投票中的传送门实例ID
    [Serializable]
	[NodeMenuItem("技能效果/大秘境玩法/大秘境-获取正在房间传送投票中的传送门实例ID", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-获取正在房间传送投票中的传送门实例ID", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/大秘境玩法/大秘境-获取正在房间传送投票中的传送门实例ID", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BIG_DUNGEON_GET_CUR_ROOM_TELEPORT_VOTE_MARKER_ENTITY_ID : SkillEffectConfigNode
    {
		// 大秘境-获取正在房间传送投票中的传送门实例ID
        public TSET_BIG_DUNGEON_GET_CUR_ROOM_TELEPORT_VOTE_MARKER_ENTITY_ID() : base(TSkillEffectType.TSET_BIG_DUNGEON_GET_CUR_ROOM_TELEPORT_VOTE_MARKER_ENTITY_ID) { }
    }
    #endregion SkillEffectConfig: 大秘境-获取正在房间传送投票中的传送门实例ID


    #region SkillEffectConfig: 获取模拟操作类型-通过技能槽位
    [Serializable]
	[NodeMenuItem("技能效果/单位/获取模拟操作类型-通过技能槽位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取模拟操作类型-通过技能槽位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/获取模拟操作类型-通过技能槽位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_SIMULATE_BUTTON_TYPE_BY_SKILL_SLOT_TYPE : SkillEffectConfigNode
    {
		// 获取模拟操作类型-通过技能槽位
		// 参数0 : 技能槽位类型-TSkillSlotType
        public TSET_GET_SIMULATE_BUTTON_TYPE_BY_SKILL_SLOT_TYPE() : base(TSkillEffectType.TSET_GET_SIMULATE_BUTTON_TYPE_BY_SKILL_SLOT_TYPE) { }
    }
    #endregion SkillEffectConfig: 获取模拟操作类型-通过技能槽位


    #region SkillEffectConfig: 获取队伍玩家人数
    [Serializable]
	[NodeMenuItem("技能效果/数据参数获取/获取队伍玩家人数", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取队伍玩家人数", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/数据参数获取/获取队伍玩家人数", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_TEAM_PLAYER_COUNT : SkillEffectConfigNode
    {
		// 获取队伍玩家人数
		// 参数0 : 阵营Camp
        public TSET_GET_TEAM_PLAYER_COUNT() : base(TSkillEffectType.TSET_GET_TEAM_PLAYER_COUNT) { }
    }
    #endregion SkillEffectConfig: 获取队伍玩家人数


    #region SkillEffectConfig: 获取抓宠基础概率
    [Serializable]
	[NodeMenuItem("技能效果/道具/获取抓宠基础概率", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/获取抓宠基础概率", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/获取抓宠基础概率", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_PET_CATCH_PROBABILITY : SkillEffectConfigNode
    {
		// 获取抓宠基础概率
		// 参数0 : 怪物单位实例ID
        public TSET_GET_PET_CATCH_PROBABILITY() : base(TSkillEffectType.TSET_GET_PET_CATCH_PROBABILITY) { }
    }
    #endregion SkillEffectConfig: 获取抓宠基础概率


    #region SkillEffectConfig: 执行抓宠并返回状态
    [Serializable]
	[NodeMenuItem("技能效果/道具/执行抓宠并返回状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/执行抓宠并返回状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/执行抓宠并返回状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_EXCUTE_CATCH_PET : SkillEffectConfigNode
    {
		// 执行抓宠并返回状态
		// 参数0 : 被抓怪物单位实例ID
        public TSET_EXCUTE_CATCH_PET() : base(TSkillEffectType.TSET_EXCUTE_CATCH_PET) { }
    }
    #endregion SkillEffectConfig: 执行抓宠并返回状态


    #region SkillEffectConfig: 创建战斗结算掉落物
    [Serializable]
	[NodeMenuItem("技能效果/创建战斗结算掉落物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/创建战斗结算掉落物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/创建战斗结算掉落物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_BATTLE_SETTLE_DROP_ITEM : SkillEffectConfigNode
    {
		// 创建战斗结算掉落物
		// 参数0 : 坐标X
		// 参数1 : 坐标Y
		// 参数2 : 自定义掉落时长[帧]
		// 参数3 : 自定义等待时长[帧]
		// 参数4 : 自定义拾取时长[帧]
		// 参数5 : 开启后是否销毁宝箱
		// 参数6 : 延迟自动退出战斗(毫秒)
        public TSET_CREATE_BATTLE_SETTLE_DROP_ITEM() : base(TSkillEffectType.TSET_CREATE_BATTLE_SETTLE_DROP_ITEM) { }
    }
    #endregion SkillEffectConfig: 创建战斗结算掉落物


    #region SkillEffectConfig: 获取地图中心位置X
    [Serializable]
	[NodeMenuItem("技能效果/获取地图中心位置X", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取地图中心位置X", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取地图中心位置X", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MAP_CENTER_POS_X : SkillEffectConfigNode
    {
		// 获取地图中心位置X
        public TSET_GET_MAP_CENTER_POS_X() : base(TSkillEffectType.TSET_GET_MAP_CENTER_POS_X) { }
    }
    #endregion SkillEffectConfig: 获取地图中心位置X


    #region SkillEffectConfig: 获取地图中心位置Y
    [Serializable]
	[NodeMenuItem("技能效果/获取地图中心位置Y", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取地图中心位置Y", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/获取地图中心位置Y", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_MAP_CENTER_POS_Y : SkillEffectConfigNode
    {
		// 获取地图中心位置Y
        public TSET_GET_MAP_CENTER_POS_Y() : base(TSkillEffectType.TSET_GET_MAP_CENTER_POS_Y) { }
    }
    #endregion SkillEffectConfig: 获取地图中心位置Y


    #region SkillEffectConfig: 显示技能槽位的核心技特效UI
    [Serializable]
	[NodeMenuItem("技能效果/UI/显示技能槽位的核心技特效UI", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示技能槽位的核心技特效UI", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/显示技能槽位的核心技特效UI", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_SKILL_RES_CORE_UI_EFFECT : SkillEffectConfigNode
    {
		// 显示技能槽位的核心技特效UI
		// 参数0 : 单位实例ID
		// 参数1 : 技能槽位类型-TSkillSlotType
		// 参数2 : 是否显示(1显示|0隐藏)
        public TSET_SHOW_SKILL_RES_CORE_UI_EFFECT() : base(TSkillEffectType.TSET_SHOW_SKILL_RES_CORE_UI_EFFECT) { }
    }
    #endregion SkillEffectConfig: 显示技能槽位的核心技特效UI


    #region SkillEffectConfig: 战斗任务-修改任务栏标题描述
    [Serializable]
	[NodeMenuItem("技能效果/UI/战斗任务-修改任务栏标题描述", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/战斗任务-修改任务栏标题描述", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/战斗任务-修改任务栏标题描述", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_MISSION_CHANGE_MISSION_GOAL_TITLE : SkillEffectConfigNode
    {
		// 战斗任务-修改任务栏标题描述
		// 参数0 : 描述文本ID（TextConfigID）-TextConfig
        public TSET_BATTLE_MISSION_CHANGE_MISSION_GOAL_TITLE() : base(TSkillEffectType.TSET_BATTLE_MISSION_CHANGE_MISSION_GOAL_TITLE) { }
    }
    #endregion SkillEffectConfig: 战斗任务-修改任务栏标题描述


    #region SkillEffectConfig: 展示BOSS出场名字
    [Serializable]
	[NodeMenuItem("技能效果/UI/展示BOSS出场名字", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/展示BOSS出场名字", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/UI/展示BOSS出场名字", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_BOSS_NAME : SkillEffectConfigNode
    {
		// 展示BOSS出场名字
		// 参数0 : BOSS单位实例ID
        public TSET_SHOW_BOSS_NAME() : base(TSkillEffectType.TSET_SHOW_BOSS_NAME) { }
    }
    #endregion SkillEffectConfig: 展示BOSS出场名字


    #region SkillEffectConfig: 折返子弹进入停留状态
    [Serializable]
	[NodeMenuItem("技能效果/折返子弹进入停留状态", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/折返子弹进入停留状态", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/折返子弹进入停留状态", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_TURN_BACK_BULLET_ENTER_KEEP_STATE : SkillEffectConfigNode
    {
		// 折返子弹进入停留状态
		// 参数0 : 子弹单位
        public TSET_TURN_BACK_BULLET_ENTER_KEEP_STATE() : base(TSkillEffectType.TSET_TURN_BACK_BULLET_ENTER_KEEP_STATE) { }
    }
    #endregion SkillEffectConfig: 折返子弹进入停留状态


    #region SkillEffectConfig: Marker-战斗单位点-通过单位组手动刷出单位
    [Serializable]
	[NodeMenuItem("技能效果/战场基础行为/Marker-战斗单位点-通过单位组手动刷出单位", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-战斗单位点-通过单位组手动刷出单位", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战场基础行为/Marker-战斗单位点-通过单位组手动刷出单位", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CUSTOM_FLUSH_MARKER_UNIT_BY_GROUPID : SkillEffectConfigNode
    {
		// Marker-战斗单位点-通过单位组手动刷出单位
		// 参数0 : 单位组ID-TSkillEntityGroupType
		// 参数1 : 出生前执行效果-SkillEffectConfig
		// 参数2 : 出生后执行效果-SkillEffectConfig
        public TSET_CUSTOM_FLUSH_MARKER_UNIT_BY_GROUPID() : base(TSkillEffectType.TSET_CUSTOM_FLUSH_MARKER_UNIT_BY_GROUPID) { }
    }
    #endregion SkillEffectConfig: Marker-战斗单位点-通过单位组手动刷出单位


    #region SkillEffectConfig: 召唤小怪
    [Serializable]
	[NodeMenuItem("技能效果/技能/召唤小怪", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/召唤小怪", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/召唤小怪", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_MONSTER : SkillEffectConfigNode
    {
		// 召唤小怪
		// 参数0 : 召唤者单位实例ID
		// 参数1 : 出生前额外执行效果
		// 参数2 : 出生后额外执行效果
		// 参数3 : 加入单位组：-TSkillEntityGroupType
		// 参数4 : 召唤数量
		// 参数5 : 扇环位置-方向角度
		// 参数6 : 扇环位置-夹角(最大360=圆环)
		// 参数7 : 扇环位置-最小半径
		// 参数8 : 扇环位置-最大半径
		// 参数9 : 继承血量比例万分比
		// 参数10 : 继承攻击比例万分比
		// 参数11 : 继承防御比例万分比
        public TSET_CREATE_MONSTER() : base(TSkillEffectType.TSET_CREATE_MONSTER) { }
    }
    #endregion SkillEffectConfig: 召唤小怪


    #region SkillEffectConfig: 战斗任务-重置当前任务
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/战斗任务-重置当前任务", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-重置当前任务", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/战斗任务-重置当前任务", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLE_MISSION_RESET_CUR_MISSION_GROUP : SkillEffectConfigNode
    {
		// 战斗任务-重置当前任务
        public TSET_BATTLE_MISSION_RESET_CUR_MISSION_GROUP() : base(TSkillEffectType.TSET_BATTLE_MISSION_RESET_CUR_MISSION_GROUP) { }
    }
    #endregion SkillEffectConfig: 战斗任务-重置当前任务


    #region SkillEffectConfig: 重置伤害统计数据
    [Serializable]
	[NodeMenuItem("技能效果/战斗状态/重置伤害统计数据", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/重置伤害统计数据", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/战斗状态/重置伤害统计数据", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CLEAR_ALL_STATISTICS_LOG_INFO : SkillEffectConfigNode
    {
		// 重置伤害统计数据
        public TSET_CLEAR_ALL_STATISTICS_LOG_INFO() : base(TSkillEffectType.TSET_CLEAR_ALL_STATISTICS_LOG_INFO) { }
    }
    #endregion SkillEffectConfig: 重置伤害统计数据


    #region SkillEffectConfig: 怪物单位是否允许被抓
    [Serializable]
	[NodeMenuItem("技能效果/道具/怪物单位是否允许被抓", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/怪物单位是否允许被抓", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/道具/怪物单位是否允许被抓", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_BATTLEUNIT_CATCHABLE : SkillEffectConfigNode
    {
		// 怪物单位是否允许被抓
		// 参数0 : 怪物单位实例ID
        public TSET_BATTLEUNIT_CATCHABLE() : base(TSkillEffectType.TSET_BATTLEUNIT_CATCHABLE) { }
    }
    #endregion SkillEffectConfig: 怪物单位是否允许被抓


    #region SkillEffectConfig: 显示跑马灯
    [Serializable]
	[NodeMenuItem("技能效果/显示跑马灯", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/显示跑马灯", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/显示跑马灯", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_SHOW_MARQUEE_FORM : SkillEffectConfigNode
    {
		// 显示跑马灯
		// 参数0 : 内容-TextConfig
		// 参数1 : 滚动次数
        public TSET_SHOW_MARQUEE_FORM() : base(TSkillEffectType.TSET_SHOW_MARQUEE_FORM) { }
    }
    #endregion SkillEffectConfig: 显示跑马灯


    #region SkillEffectConfig: 清空战斗跑马灯
    [Serializable]
	[NodeMenuItem("技能效果/清空战斗跑马灯", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/清空战斗跑马灯", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/清空战斗跑马灯", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CLEAR_MARQUEE_FORM : SkillEffectConfigNode
    {
		// 清空战斗跑马灯
        public TSET_CLEAR_MARQUEE_FORM() : base(TSkillEffectType.TSET_CLEAR_MARQUEE_FORM) { }
    }
    #endregion SkillEffectConfig: 清空战斗跑马灯


    #region SkillEffectConfig: 创建场景物
    [Serializable]
	[NodeMenuItem("技能效果/单位/创建场景物", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/创建场景物", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/单位/创建场景物", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_CREATE_SCENE_OBJECT : SkillEffectConfigNode
    {
		// 创建场景物
		// 参数0 : 召唤者单位实例ID
		// 参数1 : 场景物表格ID-BattleSceneObjectConfig
		// 参数2 : 面向
		// 参数3 : 位置X
		// 参数4 : 位置Y
		// 参数5 : 位置Z
		// 参数6 : 加入单位组：-TSkillEntityGroupType
		// 参数7 : 出生前额外执行效果
		// 参数8 : 出生后额外执行效果
        public TSET_CREATE_SCENE_OBJECT() : base(TSkillEffectType.TSET_CREATE_SCENE_OBJECT) { }
    }
    #endregion SkillEffectConfig: 创建场景物


    #region SkillEffectConfig: 暂存当前伤害统计
    [Serializable]
	[NodeMenuItem("技能效果/暂存当前伤害统计", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/暂存当前伤害统计", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/暂存当前伤害统计", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_KEEP_CUR_BATTLE_STATISTICS : SkillEffectConfigNode
    {
		// 暂存当前伤害统计
        public TSET_KEEP_CUR_BATTLE_STATISTICS() : base(TSkillEffectType.TSET_KEEP_CUR_BATTLE_STATISTICS) { }
    }
    #endregion SkillEffectConfig: 暂存当前伤害统计


    #region SkillEffectConfig: 重置伤害统计
    [Serializable]
	[NodeMenuItem("技能效果/重置伤害统计", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/重置伤害统计", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/重置伤害统计", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_RESET_BATTLE_STATISTICS : SkillEffectConfigNode
    {
		// 重置伤害统计
		// 参数0 : 是否重置为暂存数据
        public TSET_RESET_BATTLE_STATISTICS() : base(TSkillEffectType.TSET_RESET_BATTLE_STATISTICS) { }
    }
    #endregion SkillEffectConfig: 重置伤害统计


    #region SkillEffectConfig: 停止音效音乐
    [Serializable]
	[NodeMenuItem("技能效果/音效/停止音效音乐", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/停止音效音乐", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/音效/停止音效音乐", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_STOP_SOUND : SkillEffectConfigNode
    {
		// 停止音效音乐
		// 参数0 : 停止的音效单位实例ID
		// 参数1 : 停止的音效表格ID-VoiceConfig
        public TSET_STOP_SOUND() : base(TSkillEffectType.TSET_STOP_SOUND) { }
    }
    #endregion SkillEffectConfig: 停止音效音乐


    #region SkillEffectConfig: 获取异常状态增伤总加成万分比
    [Serializable]
	[NodeMenuItem("技能效果/伤害流程/获取异常状态增伤总加成万分比", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/获取异常状态增伤总加成万分比", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/伤害流程/获取异常状态增伤总加成万分比", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_GET_ALL_DEBUFF_DMG_PER : SkillEffectConfigNode
    {
		// 获取异常状态增伤总加成万分比
		// 参数0 : 攻击者单位实例ID
		// 参数1 : 受击者单位实例ID
        public TSET_GET_ALL_DEBUFF_DMG_PER() : base(TSkillEffectType.TSET_GET_ALL_DEBUFF_DMG_PER) { }
    }
    #endregion SkillEffectConfig: 获取异常状态增伤总加成万分比


    #region SkillEffectConfig: 位移每帧过程处理
    [Serializable]
	[NodeMenuItem("技能效果/技能/位移每帧过程处理", typeof(NodeEditor.SkillEditor.SkillGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/位移每帧过程处理", typeof(NodeEditor.GamePlayEditor.GamePlayGraph))]
	[NodeMenuItem("技能编辑器/技能效果/技能/位移每帧过程处理", typeof(NodeEditor.AIEditor.AIGraph))]
    public sealed partial class TSET_PROCESS_DISPLACEMENT_STEP : SkillEffectConfigNode
    {
		// 位移每帧过程处理
		// 参数0 : 被位移单位实例ID
		// 参数1 : 目标坐标X
		// 参数2 : 目标坐标Y
		// 参数3 : 本次位移距离[厘米]
		// 参数4 : 是否穿越角色[1_0]
		// 参数5 : 是否穿越中间阻挡[1_0]
		// 参数6 : 落点在阻挡内是否移出[1_0]
		// 参数7 : 是否强制同步位置（无相机平滑）[1_0]
		// 参数8 : 是否为最后一次位移
        public TSET_PROCESS_DISPLACEMENT_STEP() : base(TSkillEffectType.TSET_PROCESS_DISPLACEMENT_STEP) { }
    }
    #endregion SkillEffectConfig: 位移每帧过程处理


// TEMPLATE_CONTENT_END
}

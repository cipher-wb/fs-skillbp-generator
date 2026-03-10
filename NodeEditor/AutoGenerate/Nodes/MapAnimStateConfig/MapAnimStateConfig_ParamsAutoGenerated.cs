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
    #region MapAnimStateConfig: 功能节点
    [Serializable]
	[NodeMenuItem("常用节点/功能节点", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class Default : MapAnimStateConfigNode
    {
		// 功能节点
        public Default() : base(MapAnimStateConfig_PointType.Default) { }
    }
    #endregion MapAnimStateConfig: 功能节点


    #region MapAnimStateConfig: 动作节点
    [Serializable]
	[NodeMenuItem("常用节点/动作节点", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class Anim : MapAnimStateConfigNode
    {
		// 动作节点
        public Anim() : base(MapAnimStateConfig_PointType.Anim) { }
    }
    #endregion MapAnimStateConfig: 动作节点


    #region MapAnimStateConfig: 刷新随机值
    [Serializable]
	[NodeMenuItem("常用节点/刷新随机值", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class UpdateRandom : MapAnimStateConfigNode
    {
		// 刷新随机值
        public UpdateRandom() : base(MapAnimStateConfig_PointType.UpdateRandom) { }
    }
    #endregion MapAnimStateConfig: 刷新随机值


    #region MapAnimStateConfig: 帧事件
    [Serializable]
	[NodeMenuItem("常用节点/帧事件", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class MapEvent : MapAnimStateConfigNode
    {
		// 帧事件
        public MapEvent() : base(MapAnimStateConfig_PointType.MapEvent) { }
    }
    #endregion MapAnimStateConfig: 帧事件


    #region MapAnimStateConfig: 根节点
    [Serializable]
	[NodeMenuItem("常用节点/根节点", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class Root : MapAnimStateConfigNode
    {
		// 根节点
        public Root() : base(MapAnimStateConfig_PointType.Root) { }
    }
    #endregion MapAnimStateConfig: 根节点


    #region MapAnimStateConfig: 特效节点
    [Serializable]
	[NodeMenuItem("常用节点/特效节点", typeof(NodeEditor.MapAnimEditor.MapAnimGraph))]
    public sealed partial class CreateEffect : MapAnimStateConfigNode
    {
		// 特效节点
        public CreateEffect() : base(MapAnimStateConfig_PointType.CreateEffect) { }
    }
    #endregion MapAnimStateConfig: 特效节点


// TEMPLATE_CONTENT_END
}

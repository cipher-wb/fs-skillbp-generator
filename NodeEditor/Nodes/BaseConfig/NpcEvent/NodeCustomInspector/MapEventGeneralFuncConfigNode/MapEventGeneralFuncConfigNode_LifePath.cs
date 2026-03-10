using Funny.Base.Utils;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_LifePath : INodeCustomInspector
    {
        public List<MapEventStoryConfigNode> UsableEndStoryNodes { get; private set; }

        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_LifePath(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
            SetPathLifeData = new SetPathLifeData(this);
        }

        #region 结束剧情相关 IntParams1
        /// <summary>
        /// 设置道具
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("设置经历相关")]
        [OnValueChanged("OnChangedSetPathLifeData", true)]
        public SetPathLifeData SetPathLifeData { get; private set; }

        private void OnChangedSetPathLifeData()
        {
            baseNode.Config?.ExSetValue("IntParams1", SetPathLifeData.ToIntParams1());

            CheckError();
        }
        #endregion

        #region Target1 添加经历对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("添加经历对象")]
        [OnValueChanged("OnChangedTarget1", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddTarget1")]
        public List<MapEventTarget> Target1 { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddTarget1()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedTarget1()
        {
            baseNode.SaveConfigTarget1(Target1);

            CheckError();
        }
        #endregion

        #region Target2 占位符对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("文字占位符对象")]
        [OnValueChanged("OnChangedTarget2", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddTarget2")]
        public List<MapEventTarget> Target2 { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddTarget2()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedTarget2()
        {
            baseNode.SaveConfigTarget2(Target2);

            CheckError();
        }
        #endregion

        public void ConfigToData()
        {
            //刷新可用节点
            var eventNode = baseNode.GetLoopPreviousNode<NpcEventConfigNode>();
            if (eventNode != default)
            {
                UsableEndStoryNodes = eventNode.GetChildNodes<ConfigPortType_MapEventStoryConfig, MapEventStoryConfigNode>("StroyExit");
            }

            //IntParams1
            SetPathLifeData = new SetPathLifeData(this, baseNode.Config.IntParams1);

            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, Target1);

            //Target2
            baseNode.RestoreTargets(baseNode.Config?.Target2, Target2);
        }

        public void SetDefault()
        {
            //刷新可用节点
            var eventNode = baseNode.GetLoopPreviousNode<NpcEventConfigNode>();
            if (eventNode != default)
            {
                UsableEndStoryNodes = eventNode.GetChildNodes<ConfigPortType_MapEventStoryConfig, MapEventStoryConfigNode>("StroyExit");
            }
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(SetPathLifeData.StoryEndID == 0)
            {
                baseNode.InspectorError += "【结束剧情错误】";
            }

            baseNode.AddInspectorErrorTableNotSelect(SetPathLifeData.LiftPathTable);
        }
    }
}

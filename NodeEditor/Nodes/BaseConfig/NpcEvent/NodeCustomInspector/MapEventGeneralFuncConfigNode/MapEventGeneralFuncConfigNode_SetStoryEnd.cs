using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_SetStoryEnd : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public List<MapEventStoryConfigNode> UsableEndStoryNodes { get; private set; }

        public MapEventGeneralFuncConfigNode_SetStoryEnd(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("结束剧情ID")]
        [OnValueChanged("OnChangedVariableData", true)]
        [ValueDropdown("GetStoryEndID")]
        public int StoryEndID = 0;

        private IEnumerable<ValueDropdownItem> GetStoryEndID()
        {
            if(UsableEndStoryNodes == default)
            {
                yield break;
            }

            foreach(var node in UsableEndStoryNodes)
            {
                yield return new ValueDropdownItem($"{node.Config.ID}-{node.Config.TitleEditor}", node.Config.ID);
            }
        }

        private void OnChangedVariableData()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int>() { StoryEndID });

            CheckError();
        }

        public void ConfigToData()
        {
            baseNode.Config.IntParams1?.ForEach(params1 =>
            {
                StoryEndID = params1;
            });

            //刷新可用节点
            var eventNode = baseNode.GetLoopPreviousNode<NpcEventConfigNode>();
            if(eventNode != default)
            {
                UsableEndStoryNodes = eventNode.GetChildNodes<ConfigPortType_MapEventStoryConfig, MapEventStoryConfigNode>("StroyExit");
            }
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

            if (StoryEndID == 0)
            {
                baseNode.InspectorError += "【结束剧情ID=0】";
            }
        }
    }
}

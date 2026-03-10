using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDR;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_DemonInvasionMopUp : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_DemonInvasionMopUp(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("改变对象列表")]
        [OnValueChanged("OnActorTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnActorTargetsAdd")]
        public List<MapEventTarget> ActorTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnActorTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_AllCostar);
        }

        private void OnActorTargetsChanged()
        {
            baseNode.SaveConfigTarget1(ActorTargets);

            CheckError();
        }
      
        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetIsEmpty(ActorTargets);
            if(ActorTargets.Count != 1)
            {
                baseNode.InspectorError += $"【只能有一个目标】\n";
            }
            else if(ActorTargets[0].TargetType != MapEventTargetType.MapEventTargetType_SpecificActor)
            {
                baseNode.InspectorError += $"【目标类型必须是指定演员】\n";
            }

            if (!baseNode.IsLastNode())
            {
                baseNode.InspectorError += $"【必须是列表的最后一个】\n";
            }
        }

        public void ConfigToData()
        {
            baseNode.RestoreTargets(baseNode.Config?.Target1, ActorTargets);
        }

        public void SetDefault()
        {
            ActorTargets.Clear();
            ActorTargets.Add(OnActorTargetsAdd());
            baseNode.SaveConfigTarget1(ActorTargets);
        }
    }
}

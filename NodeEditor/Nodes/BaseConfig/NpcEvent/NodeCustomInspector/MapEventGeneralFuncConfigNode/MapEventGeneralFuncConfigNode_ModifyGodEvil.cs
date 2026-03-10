using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDR;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_ModifyGodEvil : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_ModifyGodEvil(MapEventGeneralFuncConfigNode baseNode)
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

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("正魔值")]
        [OnValueChanged("OnChangeModifyGodEvilData", true), DelayedProperty]
        public ModifyGodEvilData ModifyGodEvilData { get; private set; }

        private void OnChangeModifyGodEvilData()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int>()
            {
                (int)ModifyGodEvilData.ModifyType,
                ModifyGodEvilData.ChangeValue,
            });

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetIsEmpty(ActorTargets);

            if(ModifyGodEvilData.ChangeValue == 0)
            {
                baseNode.InspectorError += $"【正魔值不能为0】\n";
            }
        }

        public void ConfigToData()
        {
            baseNode.RestoreTargets(baseNode.Config?.Target1, ActorTargets);
            if (baseNode.Config?.IntParams1?.Count == 2)
            {
                ModifyGodEvilData = new ModifyGodEvilData((ModifyType)baseNode.Config.IntParams1[0], baseNode.Config.IntParams1[1]);
            }
        }

        public void SetDefault()
        {
            ActorTargets.Clear();
            ActorTargets.Add(OnActorTargetsAdd());
            baseNode.SaveConfigTarget1(ActorTargets);

            ModifyGodEvilData = new ModifyGodEvilData(ModifyType.Add, 0);
            OnChangeModifyGodEvilData();
        }
    }
}

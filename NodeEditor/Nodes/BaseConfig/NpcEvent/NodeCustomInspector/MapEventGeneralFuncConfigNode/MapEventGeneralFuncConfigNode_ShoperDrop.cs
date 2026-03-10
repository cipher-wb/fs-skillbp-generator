using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_ShoperDrop : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_ShoperDrop(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 添加对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落对象")]
        [OnValueChanged("OnChangedTargets", true), DelayedProperty]
        public MapEventTarget Target { get; private set; } = new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);

        private void OnChangedTargets()
        {
            baseNode.SaveConfigTarget1(new List<MapEventTarget>() { Target });

            CheckError();
        }
        #endregion

        #region 掉落次数
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落次数")]
        [OnValueChanged("OnChangedDropCount", true)]
        [MinValue(1)]
        public int DropCount = 1;

        private void OnChangedDropCount()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { DropCount, (int)DropType });

            CheckError();
        }
        #endregion

        #region 掉落类型
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落类型")]
        [OnValueChanged("OnDropTypeChanged", true), DelayedProperty]
        public TDropInfoPushType DropType { get; private set; } = TDropInfoPushType.TD_Scatter;

        private void OnDropTypeChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { DropCount, (int)DropType });

            CheckError();
        }
        #endregion

        public void ConfigToData()
        {
            //IntParams1
            if (baseNode.Config.IntParams1?.Count > 0)
            {
                DropCount = baseNode.Config.IntParams1[0];
            }
            if (baseNode.Config.IntParams1?.Count > 1)
            {
                DropType = (TDropInfoPushType)baseNode.Config.IntParams1[1];
            }
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { DropCount, (int)DropType });

            //Target1
            if(baseNode.Config?.Target1?.Count > 0)
            {
                var targets = new List<MapEventTarget>();
                baseNode.RestoreTargets(baseNode.Config?.Target1, targets);
                if (targets?.Count > 0)
                {
                    Target = targets[0];
                }
            }
            else
            {
                baseNode.SaveConfigTarget1(new List<MapEventTarget>() { Target });
            }
        }

        public void SetDefault()
        {

        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(Target.TargetType == MapEventTargetType.MapEventTargetType_Null 
                || Target.TargetType == MapEventTargetType.MapEventTargetType_Player
                || Target.TargetType == MapEventTargetType.MapEventTargetType_AllCostar)
            {
                baseNode.InspectorError += "【掉落对象错误】";
            }
            baseNode.AddInspectorErrorDropType(DropType);
        }
    }
}

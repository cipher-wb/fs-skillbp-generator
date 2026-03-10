using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class PlayRotateData
    {
        [LabelText("朝向类型")]
        public TEventFaceType RotateType;

        [LabelText("角度"), ShowIf("@RotateType == TableDR.TEventFaceType.TEFaceType_Angle")]
        public int Yaw;

        [LabelText("演员索引"), ShowIf("@RotateType == TableDR.TEventFaceType.TEFaceType_ToActor")]
        public int ActorIndex;

        public PlayRotateData()
        {

        }

        public PlayRotateData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            if(RotateType == TEventFaceType.TEFaceType_Angle)
            {
                return $"{(int)RotateType}|{Yaw}";
            }
            else if(RotateType == TEventFaceType.TEFaceType_ToLeader)
            {
                return $"{(int)RotateType}|0";
            }
            else if(RotateType == TEventFaceType.TEFaceType_ToActor)
            {
                return $"{(int)RotateType}|{ActorIndex}";
            }

            return $"{(int)RotateType}|0";
        }

        public void ToData(string param)
        {
            if(string.IsNullOrEmpty(param)) { return; }

            var split = param.Split('|');
            if (split.Length != 2) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1))
            {
                return;
            }

            RotateType = (TEventFaceType)param0;

            if(RotateType == TEventFaceType.TEFaceType_Angle)
            {
                Yaw = param1;
            }
            else if(RotateType == TEventFaceType.TEFaceType_ToActor)
            {
                ActorIndex = param1;
            }
        }
    }

    public class MapEventPerformanceConfigNode_Rotate : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_Rotate(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayRotateData perfData = new PlayRotateData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new PlayRotateData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PlayRotateData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

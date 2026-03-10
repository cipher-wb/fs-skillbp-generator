using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static NodeEditor.MapEventPerformanceConfigNode;

namespace NodeEditor
{
    public class PerfMoveData
    {
        [LabelText("目标类型")]
        public TargetType TargetType;

        [LabelText("演员索引或自定义ID"), ShowIf("@TargetType == NodeEditor.MapEventPerformanceConfigNode.TargetType.Other")]
        public int CustomID;

        [LabelText("坐标偏移")]
        public Vector2 OffSet;

        [LabelText("到达目标点朝向")]
        public float Yaw;

        [LabelText("等待结束")]
        public bool WaitFinished;

        [LabelText("移动速度")]
        public int Speed;

        public PerfMoveData()
        {

        }

        public PerfMoveData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{(int)TargetType}|{CustomID}|{(int)OffSet.x}|{(int)OffSet.y}|{Yaw}|{(WaitFinished ? 1 : 0)}|{Speed}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                TargetType = TargetType.Self;
                return;
            }

            var split = param.Split('|');
            if (split.Length < 6) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1)
                || !int.TryParse(split[2], out var param2)
                || !int.TryParse(split[3], out var param3)
                || !int.TryParse(split[4], out var param4)
                || !int.TryParse(split[5], out var param5))
            { 
                return; 
            }

            TargetType = (TargetType)param0;
            CustomID = param1;
            OffSet = new Vector2(param2, param3);
            Yaw = param4;
            WaitFinished = param5 != 0 ? true : false;

            if(split.Length > 6 && int.TryParse(split[6], out var param6))
            {
                Speed = param6;
            }
        }
    }

    public class MapEventPerformanceConfigNode_Move : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_Move(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PerfMoveData perfData = new PerfMoveData();

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
            perfData = new PerfMoveData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PerfMoveData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static NodeEditor.MapEventPerformanceConfigNode;

namespace NodeEditor
{
    public class PerfSpecialMoveData
    {
        [LabelText("目标类型")]
        public TargetType TargetType;

        [LabelText("演员索引或自定义ID"), ShowIf("@TargetType == NodeEditor.MapEventPerformanceConfigNode.TargetType.Other")]
        public int CustomID;

        [LabelText("坐标偏移")]
        public Vector3 Offset;

        [LabelText("移动速度")]
        public int Speed;

        [LabelText("等待结束")]
        public bool WaitFinished;
        
        [LabelText("同步坐标")]
        public bool SyncPos;
        
        public PerfSpecialMoveData()
        {

        }

        public PerfSpecialMoveData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{(int)TargetType}|{CustomID}|{(int)Offset.x}|{(int)Offset.y}|{(int)Offset.z}|{Speed}|{(WaitFinished ? 1 : 0)}|{(SyncPos ? 1 : 0)}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                TargetType = TargetType.Self;
                return;
            }

            var split = param.Split('|');
            if (split.Length != 8) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1)
                || !int.TryParse(split[2], out var param2)
                || !int.TryParse(split[3], out var param3)
                || !int.TryParse(split[4], out var param4)
                || !int.TryParse(split[5], out var param5)
                || !int.TryParse(split[6], out var param6)
                || !int.TryParse(split[7], out var param7))
            { 
                return; 
            }

            TargetType = (TargetType)param0;
            CustomID = param1;
            Offset = new Vector3(param2, param3, param4);
            Speed = param5;
            WaitFinished = param6 != 0 ? true : false;
            SyncPos = param7 != 0 ? true : false;
        }
    }

    public class MapEventPerformanceConfigNode_SpecialMove : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_SpecialMove(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PerfSpecialMoveData perfData = new();

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
            perfData = new PerfSpecialMoveData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PerfSpecialMoveData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

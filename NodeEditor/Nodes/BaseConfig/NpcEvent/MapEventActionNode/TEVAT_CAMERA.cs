using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionCameraData : ActionData
    {
        [LabelText("演员索引（-1表示玩家）"), DelayedProperty]
        public int ActorIndex;

        [LabelText("坐标偏移X"), DelayedProperty]
        public int OffsetX;

        [LabelText("坐标偏移Y"), DelayedProperty]
        public int OffsetY;

        [LabelText("移动时间（毫秒）"), DelayedProperty]
        public int MoveTime;

        [LabelText("镜头远近"), DelayedProperty]
        public int CameraDistance;

        public ActionCameraData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            var eventNode = BaseNode.GetLoopPreviousNode<NpcEventConfigNode>();
            if (eventNode != default && ActorIndex != -1 && !eventNode.IsActorInEvent(ActorIndex))
            {
                BaseNode.InspectorError += $"演员不存在\n";
            }

            if(MoveTime == 0)
            {
                BaseNode.InspectorError += $"移动时间为0\n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 5)
            {
                ActorIndex = -1;
                OffsetX = 0;
                OffsetY = 0;
                MoveTime = 1000;
                CameraDistance = 0;
                return;
            }

            ActorIndex = param[0];
            OffsetX = param[1];
            OffsetY = param[2];
            MoveTime = param[3];
            CameraDistance = param[4];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { ActorIndex, OffsetX, OffsetY, MoveTime, CameraDistance };
        }
    }

    [Serializable]
    [NodeMenuItem("剧情行为/相机", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_CAMERA : NpcEventActionConfigNode
    {
        // 相机
        // 参数0 : 演员索引（-1表示玩家）
        // 参数1 : 坐标偏移X
        // 参数2 : 坐标偏移Y
        // 参数3 : 移动时间（毫秒）
        // 参数4 : 镜头远近
        public TEVAT_CAMERA() : base(NpcEventActionConfig_TEventActionType.TEVAT_CAMERA) 
        {
            ActionData = new ActionCameraData(this);
        }
    }
}
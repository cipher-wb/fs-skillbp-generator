using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public enum PlayerArrivedType
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("保持")]
        Keep = 0,
        [Sirenix.OdinInspector.ShowInInspector, LabelText("行走")]
        Land,
        [Sirenix.OdinInspector.ShowInInspector, LabelText("飞行")]
        Fly,
    }

    // 玩家移动
    // 参数0 : 事件中心点偏移X
    // 参数1 : 事件中心点偏移Y
    // 参数2 : 到达行走状态(0.保持 1.行走 2.飞行)
    // 参数3 : 到达朝向
    public class ActionPlayerMoveData : ActionData
    {
        [LabelText("事件中心点偏移X"), DelayedProperty]
        public int OffsetX;

        [LabelText("事件中心点偏移Y"), DelayedProperty]
        public int OffsetY;

        [LabelText("到达行走状态"), DelayedProperty]
        public PlayerArrivedType ArrivedType;

        [LabelText("到达朝向"), DelayedProperty]
        public int ArrivedYaw;

        public ActionPlayerMoveData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {

        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 4)
            {
                OffsetX = 0;
                OffsetY = 0;
                ArrivedType = PlayerArrivedType.Keep;
                ArrivedYaw = 180;
                return;
            }

            OffsetX = param[0];
            OffsetY = param[1];
            ArrivedType = (PlayerArrivedType)param[2];
            ArrivedYaw = param[3];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { OffsetX, OffsetY, (int)ArrivedType, ArrivedYaw };
        }
    }

    [Serializable]
    [NodeMenuItem("剧情行为/玩家移动", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_PLAYER_MOVE : NpcEventActionConfigNode
    {
        // 玩家移动
        // 参数0 : 事件中心点偏移X
        // 参数1 : 事件中心点偏移Y
        // 参数2 : 到达行走状态(0.保持 1.行走 2.飞行)
        // 参数3 : 到达朝向
        public TEVAT_PLAYER_MOVE() : base(NpcEventActionConfig_TEventActionType.TEVAT_PLAYER_MOVE) 
        {
            ActionData = new ActionPlayerMoveData(this);
        }
    }
}
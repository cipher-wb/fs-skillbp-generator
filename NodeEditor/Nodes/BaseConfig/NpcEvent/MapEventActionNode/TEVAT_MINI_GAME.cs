using GameApp.Coroutine;
using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionMiniGameData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("分数"), DelayedProperty]
        public int Score;

        public ActionMiniGameData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if (Score <= 0)
            {
                BaseNode.InspectorError += $"分数为0 \n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 1)
            {
                return;
            }

            Score = param[0];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { Score };
        }
    }

    [Serializable]
    [NodeMenuItem("主行为/小游戏", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_MINI_GAME : NpcEventActionConfigNode
    {
        // 小游戏
        // 参数0 : 分数
        public TEVAT_MINI_GAME() : base(NpcEventActionConfig_TEventActionType.TEVAT_MINI_GAME) 
        {
            ActionData = new ActionMiniGameData(this);
        }
    }
}
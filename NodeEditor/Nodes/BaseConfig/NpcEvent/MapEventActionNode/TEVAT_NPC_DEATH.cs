using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionDeadData : ActionData
    {
        public ActionDeadData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {

        }

        public override void ToData(IReadOnlyList<int> param)
        {
        }

        public override List<int> ToParam()
        {
            return default;
        }
    }

    [Serializable]
    [NodeMenuItem("主行为/NPC死亡", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_NPC_DEATH : NpcEventActionConfigNode
    {
        // NPC死亡
        public TEVAT_NPC_DEATH() : base(NpcEventActionConfig_TEventActionType.TEVAT_NPC_DEATH) 
        {
            ActionData = new ActionDeadData(this);
        }
    }
}
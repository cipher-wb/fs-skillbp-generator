using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionWaitData : ActionData
    {
        public ActionWaitData(NpcEventActionConfigNode baseNode)
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
            return new List<int>();
        }
    }

    [Serializable]
    [NodeMenuItem("主行为/空等待", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_JUSTWAIT : NpcEventActionConfigNode
    {
        // 空等待
        public TEVAT_JUSTWAIT() : base(NpcEventActionConfig_TEventActionType.TEVAT_JUSTWAIT) 
        {
            ActionData = new ActionWaitData(this);
        }
    }
}
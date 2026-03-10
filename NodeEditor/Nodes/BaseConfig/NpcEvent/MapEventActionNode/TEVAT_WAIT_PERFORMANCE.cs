using GraphProcessor;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionWaitPerformanceData : ActionData
    {
        public ActionWaitPerformanceData(NpcEventActionConfigNode baseNode) 
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
    [NodeMenuItem("剧情行为/等待表演结束", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_WAIT_PERFORMANCE : NpcEventActionConfigNode
    {
        // 等待表演结束
        public TEVAT_WAIT_PERFORMANCE() : base(NpcEventActionConfig_TEventActionType.TEVAT_WAIT_PERFORMANCE) 
        {
            ActionData = new ActionWaitPerformanceData(this);
        }
    }
}
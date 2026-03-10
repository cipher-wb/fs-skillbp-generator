using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;
using static NodeEditor.NpcEventActionConfigNode;

namespace NodeEditor
{
    public class ActionStoryEndData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("类型")]
        public EndStoryType EndStoryType;

        public ActionStoryEndData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {

        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 1)
            {
                return;
            }

            EndStoryType = (EndStoryType)param[0];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { (int)EndStoryType };
        }
    }

    [Serializable]
    [NodeMenuItem("剧情行为/结束剧情", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_STORYEND : NpcEventActionConfigNode
    {
        // 结束剧情
        // 参数0 : 0 待续 1 结束
        public TEVAT_STORYEND() : base(NpcEventActionConfig_TEventActionType.TEVAT_STORYEND) 
        {
            ActionData = new ActionStoryEndData(this);
        }
    }
}
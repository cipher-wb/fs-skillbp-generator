using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionStoryBeginData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("对话组"), EnableIf("@false")]
        public int NpcTalkGroupID;

        public ActionStoryBeginData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if (NpcTalkGroupID == 0)
            {
                BaseNode.InspectorError += $"缺少对话组\n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 1)
            {
                return;
            }

            NpcTalkGroupID = param[0];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { NpcTalkGroupID };
        }
    }

    [Serializable]
    [NodeMenuItem("剧情行为/剧情开始界面", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_STORYBEGIN : NpcEventActionConfigNode
    {
        // 剧情开始界面
        // 参数0 : 对话组-NpcTalkGroupConfig
        public TEVAT_STORYBEGIN() : base(NpcEventActionConfig_TEventActionType.TEVAT_STORYBEGIN) 
        {
            ActionData = new ActionStoryBeginData(this);
        }

        public ActionStoryBeginData DialogData => ActionData as ActionStoryBeginData;

        [Output("对话组"), HideIf("@true")]
        public int NpcTalkGroupID;

        [CustomPortBehavior(nameof(NpcTalkGroupID))]
        public IEnumerable<PortData> NpcTalkGroup_Behavior(List<SerializableEdge> edges)
        {
            yield return new PortData
            {
                displayName = $"对话组",
                displayType = typeof(ConfigPortType_NpcTalkGroupConfig),
                identifier = nameof(NpcTalkGroupID),
                acceptMultipleEdges = false,
            };
        }

        [CustomPortOutput(nameof(NpcTalkGroupID), typeof(int), true)]
        public void NpcTalkGroup_OutPut(List<SerializableEdge> edges, NodePort outputPort)
        {
            if (!graph.isEnabled) { return; }

            if (Config.ID == 0) { return; }

            if (outputPort == null || outputPort.portData == null || edges?.Count <= 0)
            {
                DialogData.NpcTalkGroupID = 0;
            }

            foreach (var edge in edges)
            {
                var inputNode = edge.inputNode;
                if (inputNode != null && inputNode is ConfigBaseNode inputConfigNode)
                {
                    DialogData.NpcTalkGroupID = inputConfigNode.ID;
                    break;
                }
            }

            SetConfigValue(nameof(Config.Param), DialogData.ToParam());

            CheckError();
        }
    }
}
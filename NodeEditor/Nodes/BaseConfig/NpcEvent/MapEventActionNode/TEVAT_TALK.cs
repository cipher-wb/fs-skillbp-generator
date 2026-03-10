using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionTalkData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("对话组"), EnableIf("@false")]
        public int NpcTalkGroupID;

        public ActionTalkData(NpcEventActionConfigNode baseNode)
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
    [NodeMenuItem("主行为/气泡对话", typeof(NpcEventEditor.NpcEventGraph))]
    [NodeMenuItem("子行为/气泡对话", typeof(NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_TALK : NpcEventActionConfigNode
    {
        /// <summary>
        /// 当一个节点没有连线时要不要设置为null
        /// </summary>
        public override bool IsPortNoEdgeSetNull => false;

        // 气泡对话
        // 参数0 : 对话组ID-NpcTalkGroupConfig
        public TEVAT_TALK() : base(NpcEventActionConfig_TEventActionType.TEVAT_TALK) 
        {
            ActionData = new ActionTalkData(this);
        }

        public ActionTalkData DialogData => ActionData as ActionTalkData;

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
using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public class ActionDialogData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("对话组"), EnableIf("@false")]
        public List<int> NpcTalkGroupIDs = new List<int>();

        public ActionDialogData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            if (NpcTalkGroupIDs.Count == 0)
            {
                BaseNode.InspectorError += $"缺少对话组\n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            NpcTalkGroupIDs.Clear();
            param?.ForEach(id =>
            {
                NpcTalkGroupIDs.Add(id);
            });
        }

        public override List<int> ToParam()
        {
            return new List<int>(NpcTalkGroupIDs);
        }
    }

    [Serializable]
    [NodeMenuItem("子行为/对话框", typeof(NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_DIALOG : NpcEventActionConfigNode
    {
        /// <summary>
        /// 当一个节点没有连线时要不要设置为null
        /// </summary>
        public override bool IsPortNoEdgeSetNull => false;

        // 对话框
        // 参数0 : 对话组-NpcTalkGroupConfig
        public TEVAT_DIALOG() : base(NpcEventActionConfig_TEventActionType.TEVAT_DIALOG)
        {
            ActionData = new ActionDialogData(this);
        }

        public ActionDialogData DialogData => ActionData as ActionDialogData;

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
                acceptMultipleEdges = true,
            };
        }

        [CustomPortOutput(nameof(NpcTalkGroupID), typeof(int), true)]
        public void NpcTalkGroup_OutPut(List<SerializableEdge> edges, NodePort outputPort)
        {
            if (!graph.isEnabled) { return; }

            if (Config.ID == 0) { return; }

            DialogData.NpcTalkGroupIDs.Clear();

            //连线排序
            edges?.Sort(SortInputNodes);
            edges?.ForEach(edge =>
            {
                var inputNode = edge.inputNode;
                if (inputNode == null)
                {
                    return;
                }

                if (inputNode is ConfigBaseNode inputConfigNode)
                {
                    DialogData.NpcTalkGroupIDs.Add(inputConfigNode.ID);
                }
                else if (inputNode is RefConfigBaseNode refConfigNode && refConfigNode.TableManagerName == TableHelper.ToTableManager(typeof(NpcTalkGroupConfig).Name))
                {
                    DialogData.NpcTalkGroupIDs.Add(refConfigNode.ID);
                }
            });

            var param = DialogData.ToParam();
            if(param.Count == 0)
            {
                //int a = 1;
            }
            SetConfigValue(nameof(Config.Param), param);

            CheckError();
        }
    }
}
using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class ActionCreateModelData : ActionData
    {
        [HideReferenceObjectPicker, LabelText("模型"), DelayedProperty]
        public int ModelID;

        public ActionCreateModelData(NpcEventActionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            var nextNode = BaseNode.GetNextNodes<NpcEventModelConfigNode>(typeof(ConfigPortType_NpcEventModelConfig));
            if(nextNode?.Count > 0)
            {
                return;
            }

            if (NpcEventModelConfigManager.Instance.GetItem(ModelID) == default)
            {
                BaseNode.InspectorError += $"配置表不存在 \n";
            }
        }

        public override void ToData(IReadOnlyList<int> param)
        {
            if (param?.Count != 1)
            {
                return;
            }

            ModelID = param[0];
        }

        public override List<int> ToParam()
        {
            return new List<int>() { ModelID };
        }
    }

    [Serializable]
    [NodeMenuItem("全局行为/创建一个模型", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    [NodeMenuItem("子行为/创建一个模型", typeof(NodeEditor.NpcEventEditor.NpcEventGraph))]
    public sealed partial class TEVAT_CREATE_MODEL : NpcEventActionConfigNode
    {
        // 创建一个模型
        // 参数0 : NpcEventModelConfig表ID-NpcEventModelConfig
        public TEVAT_CREATE_MODEL() : base(NpcEventActionConfig_TEventActionType.TEVAT_CREATE_MODEL) 
        {
            ActionData = new ActionCreateModelData(this);
        }

        public ActionCreateModelData CreateModelData => ActionData as ActionCreateModelData;

        [Output("模型"), HideIf("@true")]
        public int ModelID;

        [CustomPortBehavior(nameof(ModelID))]
        public IEnumerable<PortData> NpcTalkGroup_Behavior(List<SerializableEdge> edges)
        {
            yield return new PortData
            {
                displayName = $"模型",
                displayType = typeof(ConfigPortType_NpcEventModelConfig),
                identifier = nameof(ModelID),
                acceptMultipleEdges = false,
            };
        }

        [CustomPortOutput(nameof(ModelID), typeof(int), true)]
        public void NpcTalkGroup_OutPut(List<SerializableEdge> edges, NodePort outputPort)
        {
            if (!graph.isEnabled) { return; }

            if (Config.ID == 0) { return; }

            if (outputPort == null || outputPort.portData == null || edges?.Count <= 0)
            {
                CreateModelData.ModelID = 0;
            }

            foreach (var edge in edges)
            {
                var inputNode = edge.inputNode;
                if (inputNode != null && inputNode is ConfigBaseNode inputConfigNode)
                {
                    CreateModelData.ModelID = inputConfigNode.ID;
                    break;
                }
            }

            SetConfigValue(nameof(Config.Param), CreateModelData.ToParam());

            CheckError();
        }

    }
}
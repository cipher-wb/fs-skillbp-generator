using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace NodeEditor
{
    /// <summary>
    /// 子行为
    /// </summary>
    public partial class NpcEventActionConfigNode
    {
        [Output, HideIf("@true")]
        public int SubActions;

        private List<int> subActionList;

        /// <summary>
        /// SubAction的Port
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        [CustomPortBehavior(nameof(SubActions))]
        public IEnumerable<PortData> SubAction_Behavior(List<SerializableEdge> edges)
        {
            var desc = GetConfigDecription(nameof(SubActions));

            yield return new PortData
            {
                displayName = $"{desc}",
                displayType = typeof(INpcEventActionConfig),
                identifier = nameof(SubActions),
                acceptMultipleEdges = true,
            };
        }

        /// <summary>
        /// SubAction port数据传输
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="outputPort"></param>
        [CustomPortOutput(nameof(SubActions), typeof(int), true)]
        public void SubAction_OutPut(List<SerializableEdge> edges, NodePort outputPort)
        {
            try
            {
                if (!graph.isEnabled) { return; }

                if (outputPort == null || outputPort.portData == null || edges?.Count <= 0)
                {
                    SetConfigValue(nameof(SubActions), default);
                }

                //连线排序
                edges.Sort(SortInputNodes);

                subActionList ??= new List<int>();
                subActionList.Clear();

                foreach(var edge in edges)
                {
                    var inputNode = edge.inputNode;
                    if (inputNode != null && inputNode is ConfigBaseNode inputConfigNode)
                    {
                        subActionList.Add(inputConfigNode.ID);
                    }
                }

                SetConfigValue(nameof(SubActions), subActionList);
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()} SubAction \n{ex}");
            }
        }
    }
}

using GraphProcessor;
using NodeEditor.PortType;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class NpcEventActionGroupConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            var groupType = Config.ActionGroupType;
            if (groupType == EventActionGroupType.TAGT_SEQUENCE)
            {
                SetCustomName($"[{Config.ID}][行为组][{Utils.GetEnumDescription(groupType)}]");
            }
            else
            {
                SetCustomName($"[{Config.ID}][行为组][{Utils.GetEnumDescription(groupType)}][{Utils.GetEnumDescription(Config.ActionChooseConditionType)}]");
            }
        }

        /// <summary>
        /// 连接到node时，刷新ActionGroupID,ActionGroupType
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            if (edges.Count != 1)
            {
                return;
            }
            var edge = edges.First();
            if(edge.outputNode is NpcEventActionGroupIndexConfigNode outputNode)
            {
                SetConfigValue(nameof(Config.ActionGroupID), outputNode.Config.ID);
                SetConfigValue(nameof(Config.ActionGroupType), outputNode.Config.ActionGroupType);
            }
        }

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public override bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            if(portType == typeof(RefConfigBaseNode))
            {
                return true;
            }

            //限制剧情行为
            var groupIndexNode = GetPreviousNode<NpcEventActionGroupIndexConfigNode>();
            var storyNode = groupIndexNode?.GetPreviousNode<MapEventStoryConfigNode>();
            if (storyNode != default)
            {
                if (!portTitle.Contains("剧情行为/"))
                {
                    return false;
                }
                return true;
            }

            //限制主行为
            if (outputPort.portData.identifier == nameof(Config.ActionList))
            {
                if (!portTitle.Contains("主行为/"))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取运行时节点
        /// </summary>
        /// <param name="actionID"></param>
        /// <param name="nodeList"></param>
        public void GetRuntimeNodes(int actionID, List<BaseNode> nodeList)
        {
            //获取已执行的actionID
            HashSet<int> actionIDSet = default;
            foreach (var actionId in Config.ActionList)
            {
                if (actionId != actionID)
                {
                    actionIDSet ??= new HashSet<int>();
                    actionIDSet.Add(actionId);
                }
                else
                {
                    break;
                }
            }

            if (actionIDSet == default) { return; }

            foreach (var outPort in outputPorts)
            {
                if (typeof(INpcEventActionConfig).IsAssignableFrom(outPort.portData.displayType) == false) { continue; }

                foreach (var edge in outPort.GetEdges())
                {
                    var outputNode = edge.inputNode;
                    if (outputNode is NpcEventActionConfigNode actionNode && actionIDSet.Contains(actionNode.Config.ID))
                    {
                        actionNode.GetRuntimeNodes(nodeList);
                    }
                }
            }
        }
    }
}

using GraphProcessor;
using NodeEditor.PortType;
using Scheme;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class NpcEventActionConfigNode
    {
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, HideLabel]
        [InfoBox("@InspectorError", InfoMessageType.Error, "IsExitInspectorError")]
        [FoldoutGroup("参数设置", true, order: 1)]
        [OnValueChanged("OnChangedActionData", true)]
        public ActionData ActionData { get; protected set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public bool IsSubActionNode => GetPreviousNode<NpcEventActionConfigNode>() != default;

        /// <summary>
        /// 主节点
        /// </summary>
        public bool IsMainActionNode
        {
            get
            {
                var groupNode = GetPreviousNode<NpcEventActionGroupConfigNode>();
                var storyNode = groupNode?.GetPreviousNode<NpcEventActionGroupIndexConfigNode>()?.GetPreviousNode<MapEventStoryConfigNode>();
                return storyNode == default && groupNode != default;
            }
        }

        /// <summary>
        /// 全局节点
        /// </summary>
        public bool IsGlobalActionNode => GetPreviousNode<NpcEventLinkConfigNode>() != default;

        /// <summary>
        /// 剧情节点
        /// </summary>
        public bool IsPerformanceNode
        {
            get
            {
                var storyNode =  GetPreviousNode<NpcEventActionGroupConfigNode>()?.GetPreviousNode<NpcEventActionGroupIndexConfigNode>()?.GetPreviousNode<MapEventStoryConfigNode>();
                return storyNode != default;
            }
        }

        public NpcEventActionConfigNode(NpcEventActionConfig_TEventActionType type) : base()
        {
            Config.ExSetValue(nameof(Config.ActionType), type);
        }

        /// <summary>
        /// 参数变动保存
        /// </summary>
        private void OnChangedActionData()
        {
            if (Config.ID == 0) { return; }

            Config?.ExSetValue(nameof(Config.Param), ActionData.ToParam());

            CheckError();
        }

        /// <summary>
        /// 重写node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"[{Config.ID}][行为][{Config.ActionType.GetDescription(false)}]");
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            if(Config.ID == 0)
            {
                return;
            }
            else if(Config.ID == 211000290)
            {
                //int a = 1;
            }

            ActionData.ToData(Config.Param);

            CheckError();

            ConfigToAI();
        }

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public override bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            if (portType == typeof(RefConfigBaseNode))
            {
                return true;
            }

            //只显示子行为
            if (outputPort.portData.identifier == "SubActions")
            {
                if (!portTitle.Contains("子行为/"))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取运行时节点
        /// </summary>
        /// <param name="nodeList"></param>
        public void GetRuntimeNodes(List<BaseNode> nodeList)
        {
            //添加自己
            nodeList.Add(this);

            //添加子行为
            foreach (var outPort in outputPorts)
            {
                if (typeof(INpcEventActionConfig).IsAssignableFrom(outPort.portData.displayType))
                {
                    foreach (var edge in outPort.GetEdges())
                    {
                        nodeList.Add(edge.inputNode);
                    }
                }
            }
        }

        /// <summary>
        /// 遍历获取所有父节点
        /// </summary>
        /// <param name="parentNodes"></param>
        public override void GetParentNodes(List<BaseNode> parentNodes)
        {
            var inputPort = inputPorts[0];

            foreach (var edge in inputPort.GetEdges())
            {
                //添加父节点
                var outputNode = edge.outputNode;
                parentNodes.Add(outputNode);

                //如果是行为组，需要添加已执行的行为节点
                if (outputNode is NpcEventActionGroupConfigNode groupNode)
                {
                    groupNode.GetRuntimeNodes(Config.ID, parentNodes);
                }

                outputNode.GetParentNodes(parentNodes);
            }
        }
    }
}

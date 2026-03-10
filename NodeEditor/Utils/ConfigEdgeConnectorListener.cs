using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphProcessor
{
    public class ConfigEdgeConnectorListener : BaseEdgeConnectorListener
    {
        public ConfigEdgeConnectorListener(BaseGraphView graphView) : base(graphView)
        {
        }

        public override void OnDrop(GraphView graphView, Edge edge)
        {
            if (edge.output != null && edge.output.node is BaseNodeView baseNodeView && baseNodeView.nodeTarget.hideChildNodes)
            {
                if (graphView is ConfigGraphView configGraphView)
                {
                    configGraphView.GetConfigGraphWindow().ShowNotification("节点隐藏，取消隐藏再连接");
                }
                return;
            }

            base.OnDrop(graphView, edge);
        }

        public override void OnDropOutsidePort(Edge edge, Vector2 position)
        {
            if (edge.output != null && edge.output.node is BaseNodeView baseNodeView && baseNodeView.nodeTarget.hideChildNodes)
            {
                if(graphView is ConfigGraphView configGraphView)
                {
                    configGraphView.GetConfigGraphWindow().ShowNotification("节点隐藏，取消隐藏再连接");
                }
                return;
            }

            base.OnDropOutsidePort(edge, position);
        }
    }
}

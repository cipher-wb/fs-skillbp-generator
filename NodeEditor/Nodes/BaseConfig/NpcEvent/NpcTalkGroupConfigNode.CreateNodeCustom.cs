using GraphProcessor;
using NodeEditor.PortType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    /// <summary>
    /// 自定义创建节点
    /// </summary>
    public partial class NpcTalkGroupConfigNode
    {
        public static void CreateNodeCustom(ConfigGraphView graphView, NodeProvider.PortDescription portDescription, Vector2 firstNodePosition, PortView inputPortView, PortView outputPortView)
        {
            //NpcTalkGroupConfigNode
            var firstNodeType = portDescription.nodeType;
            var firstNodeView = graphView.AddNode(BaseNode.CreateFromType(firstNodeType, firstNodePosition));

            var firstInputPort = firstNodeView.GetPortViewFromFieldName(portDescription.portFieldName, portDescription.portIdentifier);
            if(firstInputPort != null)
            {
                graphView.Connect(firstInputPort, outputPortView);
            }
            var nextOutputPort = firstNodeView.GetPortViewFromFieldName("PackedMembersOutput","TalkIDs");

            //NpcTalkConfigNode
            var nextNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(NpcTalkConfigNode), firstNodePosition + new Vector2(300, 0)));
            if (nextNodeView != null)
            {
                var nextInputPort = nextNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }
        }
    }
}

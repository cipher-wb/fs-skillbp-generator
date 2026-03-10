using GraphProcessor;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// 自定义创建节点
    /// </summary>
    public partial class NpcEventActionGroupIndexConfigNode
    {
        public static void CreateNodeCustom(ConfigGraphView graphView, NodeProvider.PortDescription portDescription, Vector2 firstNodePosition, PortView inputPortView, PortView outputPortView)
        {
            var firstNodeType = portDescription.nodeType;
            var firstNodeView = graphView.AddNode(BaseNode.CreateFromType(firstNodeType, firstNodePosition));

            var firstInputPort = firstNodeView.GetPortViewFromFieldName(portDescription.portFieldName, portDescription.portIdentifier);
            if(firstInputPort != null)
            {
                graphView.Connect(firstInputPort, outputPortView);
            }

            //NpcEventActionGroupConfigNode
            var nextPosition = firstNodePosition + new Vector2(200, 0);
            var nextOutputPort = firstNodeView.GetFirstPortViewFromFieldName("ConnectedGroupIDS");
            var actionGroupNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(NpcEventActionGroupConfigNode), nextPosition));
            if (actionGroupNodeView != null)
            {
                var nextInputPort = actionGroupNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }

            //TEVAT_JUSTWAIT 主行为-空等待
            nextPosition = nextPosition + new Vector2(250, 0);
            nextOutputPort = actionGroupNodeView.GetPortViewFromFieldName("PackedMembersOutput", "ActionList");
            var justWaitNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(TEVAT_JUSTWAIT),  nextPosition));
            if (justWaitNodeView != null)
            {
                var nextInputPort = justWaitNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }

            //TEVAT_DIALOG 子行为-对话框
            nextPosition = nextPosition + new Vector2(200, 0);
            nextOutputPort = justWaitNodeView.GetFirstPortViewFromFieldName("SubActions");
            var dialogNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(TEVAT_DIALOG), nextPosition));
            if (nextOutputPort != null)
            {
                var nextInputPort = dialogNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }

            //NpcTalkGroupConfigNode 对话组
            nextPosition = nextPosition + new Vector2(200, 0);
            nextOutputPort = dialogNodeView.GetFirstPortViewFromFieldName("NpcTalkGroupID");
            var talkGroupNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(NpcTalkGroupConfigNode), nextPosition));
            if (nextOutputPort != null)
            {
                var nextInputPort = talkGroupNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }

            //NpcTalkConfigNode 对话
            nextPosition = nextPosition + new Vector2(250, 0);
            nextOutputPort = talkGroupNodeView.GetPortViewFromFieldName("PackedMembersOutput", "TalkIDs");
            var talkNodeView = graphView.AddNode(BaseNode.CreateFromType(typeof(NpcTalkConfigNode), nextPosition));
            if (nextOutputPort != null)
            {
                var nextInputPort = talkNodeView.GetFirstPortViewFromFieldName("ID");
                if (nextInputPort != null)
                {
                    graphView.Connect(nextInputPort, nextOutputPort);
                }
            }
        }
    }
}

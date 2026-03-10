using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    // 事件
    public partial class BaseGraphView
    {
        protected Action<KeyDownEvent> keyDownCallbackEvent;
        public Action<KeyDownEvent> KeyDownCallbackEvent { get => keyDownCallbackEvent; set => keyDownCallbackEvent = value; }

        protected Action<EventType, IMouseEvent> mouseCallBackEvent;
        public Action<EventType, IMouseEvent> MouseCallBackEvent { get => mouseCallBackEvent; set => mouseCallBackEvent = value; }

        /// todo 后面需要实现子节点跟随父节点拖动，目前通过group去移动
        /// <summary>
        /// 显示、隐藏节点所有的子节点
        /// </summary>
        /// <param name="baseNode"></param>
        /// <param name="showOrHide"></param>
        public void ShowOrHideNodeChildsNodes(BaseNode baseNode, bool showOrHide)
        {
            ShowOrHideNodeOutputEdges(baseNode, showOrHide);

            //获取所有子节点
            var childNodes = new List<BaseNode>();
            baseNode.GetChildNodesRecursive(childNodes);
            if (!nodeViewsPerNode.TryGetValue(baseNode, out var baseNodeView))
            {
                return;
            }

            Vector2 offsetPos = Vector2.zero;

            if (!showOrHide && childNodes.Count > 0)
            {
                baseNodeView.ShowOrHideNodeInfoLabel();
                baseNode.hidePos = baseNodeView.GetPosition().position;
                baseNode.hideChildNodes = true;
            }
            else
            {
                baseNodeView.ShowOrHideNodeInfoLabel(false);
                offsetPos = baseNodeView.GetPosition().position - baseNode.hidePos;
                baseNode.hideChildNodes = false;
            }

            foreach (var childNode in childNodes)
            {
                if (nodeViewsPerNode.TryGetValue(childNode, out var childNodeView))
                {
                    //显示隐藏的位置计算
                    if (!showOrHide)
                    {
                        childNode.hideCounter++;
                        childNode.hidePos = childNodeView.GetPosition().position;
                        childNodeView.hideGroupView = this.RemoveNodeToGroup(childNodeView);

                        //stack
                        var hideStackView = this.RemoveNodeToStack(childNodeView);
                        hideStackView?.Hide();
                        childNodeView.hideStackView = hideStackView;
                    }
                    else
                    {
                        childNode.hideCounter--;
                        var rect = childNodeView.GetPosition();
                        rect.position = childNode.hidePos + offsetPos;
                        childNodeView.SetPosition(rect);

                        //group
                        if (childNodeView.hideGroupView != null)
                            this.AddNodeToGroup(childNodeView.hideGroupView, childNodeView);

                        //stack
                        var hideStackView = childNodeView.hideStackView;
                        hideStackView?.Show(offsetPos);
                        if (hideStackView != null)
                        {
                            this.AddNodeToStack(childNodeView.hideStackView, childNodeView);
                            childNodeView.hideStackView = default;
                        }
                    }

                    bool isShow = childNode.hideCounter <= 0;
                    ShowOrHideNodeOutputEdges(childNode, isShow);
                    if (childNode.isHideChildNodes)
                    {
                        childNodeView.ShowOrHideNodeInfoLabel(isShow);
                        //对于有隐藏子节点的父节点，需要走一遍
                        ShowOrHideNodeOutputEdges(childNode, false);
                    }
                    childNode.visible = isShow;
                    childNodeView.visible = isShow;
                }
            }
        }

        /// <summary>
        /// 显示、隐藏所有节点的edgeview
        /// </summary>
        /// <param name="baseNode"></param>
        /// <param name="showOrHide"></param>
        public void ShowOrHideNodeOutputEdges(BaseNode baseNode, bool showOrHide)
        {
            foreach (var outputPort in baseNode.outputPorts)
            {
                foreach (var edge in edgeViews)
                {
                    if (edge.serializedEdge.outputNode == baseNode)
                    {
                        edge.serializedEdge.isVisible = showOrHide;

                        edge.visible = showOrHide;
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    public partial class BaseNodeView
    {
        [HideInInspector]
        public GroupView hideGroupView;
        
        [HideInInspector]
        public BaseStackNodeView hideStackView;

        protected Label nodeHideCounter;

        protected Button hideChildsBtn;

        protected Label logCounter;

        protected Label nodeDesc;
        
        /// <summary>
        /// 引用节点的引用数量
        /// </summary>
        protected Label refCounter;

        private const string nodeCustomPath = "GraphProcessorElements/NodeCustom";
        void InitNodeCustom()
        {
            //资源路径
            var visualTree = Resources.Load<VisualTreeAsset>(nodeCustomPath);
            if (visualTree == null)
            {
                Debug.Log($"Don`t Find Path : {nodeCustomPath} ");
            }
            VisualElement labelFromUXML = visualTree.Instantiate();

            nodeHideCounter = labelFromUXML.Q<UnityEngine.UIElements.Label>("NodeHideCounter");
            hideChildsBtn = labelFromUXML.Q<UnityEngine.UIElements.Button>("HideChildsBtn");
            logCounter = labelFromUXML.Q<UnityEngine.UIElements.Label>("LogCounter");
            nodeDesc = labelFromUXML.Q<UnityEngine.UIElements.Label>("NodeDesc");
            refCounter = labelFromUXML.Q<UnityEngine.UIElements.Label>("RefCounter");

            Add(nodeHideCounter);
            m_CollapseButton.parent.Add(hideChildsBtn);
            //titleContainer.Add(hideChildsBtn);
            Add(logCounter);
            SetLabelVisible(logCounter, false);

            Add(refCounter);
            SetLabelVisible(refCounter, false);
            
            // 暂时维持原样
            // nodeBorder ??= this.Q("node-border");
            // nodeBorder.Add(nodeDesc);
            // SetLabelVisible(nodeDesc, false);
            //nodeDesc.text = "测试一下";
            //Add(nodeDesc);

            hideChildsBtn.clickable.clicked += ToggleCollapse;
            if (nodeTarget.visible && this.nodeTarget.isHideChildNodes)
                ShowOrHideNodeInfoLabel(true);
            else
                ShowOrHideNodeInfoLabel(false);
        }

        public void UpdateRefCounter(int refCount)
        {
            nodeTarget.RefCount = refCount;
            if (nodeTarget.RefCount > 0)
            {
                SetLabelVisible(refCounter, true);
                refCounter.text = $"被引用（{refCount}）";
            }
            else
            {
                SetLabelVisible(refCounter, false);
            }
        }

        public void ShowLogCounter(int count = 0)
        {
            bool isShow = count > 0;
            SetLabelVisible(logCounter, isShow);
            if (isShow)
            {
                logCounter.text = $"Log:{count}";
            }
        }

        public void ShowLogExtraMsg(string msg)
        {
            SetLabelVisible(logCounter, true);
            logCounter.text = msg;
        }

        public void ShowOrHideNodeInfoLabel(bool isShow = true)
        {
            if (!isShow)
            {
                SetLabelVisible(nodeHideCounter, isShow);
                SetLabelVisible(logCounter, isShow);
                return;
            }
            var childNodes = new List<BaseNode>();
            this.nodeTarget.GetChildNodesRecursive(childNodes);

            int count = childNodes.Count;
            //foreach (var childNode in childNodes)
            //{
            //    //if (!(childNode is BaseNodeView baseNodeView)) continue;
            //    count++;
            //}

            nodeHideCounter.text = $"{count}";
            if (count == 0)
                isShow = false;
            
            SetLabelVisible(nodeHideCounter, isShow);
        }

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public virtual bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            return true;
        }

        public virtual void ResetConfigNodeView(string configJson) 
        {
        }

        private void SetLabelVisible(Label label, bool visible)
        {
            if (label != null)
            {
                label.visible = visible;
                label.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }


    // Tootip
    public partial class BaseNodeView
    {
        private static readonly Vector2 offset = new(10, 0);

        private NodeTooltipView nodeTooltipView;
        private Label tooltipLabel;
        private bool isShowing = false;

        void InitializeTootip()
        {
            if (nodeTooltipView != null)
            {
                return;
            }
            nodeTooltipView = new NodeTooltipView();
            nodeTooltipView.visible = false;

            tooltipLabel = new Label();
            tooltipLabel.style.fontSize = Utils.DefaultLabelFontSize;
            //tooltipLabel.style.backgroundColor = Color.black;
            tooltipLabel.style.color = Color.white;
            //tooltipLabel.style.unityTextAlign = TextAnchor.MiddleCenter;

            var headLabelElement = new VisualElement();
            // Add Node type specific settings
            headLabelElement.Add(tooltipLabel);
            nodeTooltipView.Add(headLabelElement);
            Add(nodeTooltipView);

            if (Application.isPlaying)
            {
                // 仅运行时打开
                RegisterCallback<MouseEnterEvent>(OnMouseEnter, TrickleDown.TrickleDown);
                RegisterCallback<MouseLeaveEvent>(OnMouseLeave, TrickleDown.TrickleDown);
            }
        }
        public void OpenTootip()
        {
            if (nodeTooltipView != null)
            {
                nodeTooltipView.visible = true;
                nodeTooltipView.BringToFront();
            }
        }

        public void CloseTooltip()
        {
            if (nodeTooltipView != null)
            {
                nodeTooltipView.visible = false;
            }
        }

        void OnMouseEnter(MouseEnterEvent evt)
        {
            switch (evt.target)
            {
                case PortView portView:
                    if (!string.IsNullOrEmpty(portView.CustomTooltip))
                    {
                        isShowing = true;
                        OpenTootip();
                        var nodeMousePosition = this.WorldToLocal(portView.worldTransform.GetPosition());
                        nodeTooltipView.transform.position = nodeMousePosition + new Vector2(portView.layout.width / 2, portView.layout.height - layout.height + 20);
                        tooltipLabel.text = portView.CustomTooltip;
                    }
                    break;
                default:
                    break;
            }
        }

        void OnMouseLeave(MouseLeaveEvent evt)
        {
            if (!isShowing) return;

            CloseTooltip();
            isShowing = false;
        }
    }
}

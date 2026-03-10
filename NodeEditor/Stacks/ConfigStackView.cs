using GraphProcessor;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    [CustomStackNodeView(typeof(ConfigStackNode))]
    public class ConfigStackView : BaseStackNodeView
    {
        [HideInInspector]
        public Label nodeInfoLabel;
        [HideInInspector]
        public Button hideChildsBtn;
        [HideInInspector]
        public Label logCounter;
        [HideInInspector]
        public const string NodeCustomPath = "GraphProcessorElements/NodeCustom";

        public ConfigStackNode baseStackNode;

        public ConfigStackView(BaseStackNode stackNode) : base(stackNode)
        {
            baseStackNode = stackNode as ConfigStackNode;
        }

        protected override bool CanInsert(BaseNodeView nodeView)
        {
            if(nodeView is ConfigBaseNodeView configNodeView)
            {
                var configBaseNode = configNodeView.ConfigBaseNode;
                var memberInfo = configBaseNode.GetType().GetMethod("GetConfigName");
                if(memberInfo == null) { return false; }

                var configName = memberInfo.Invoke(configBaseNode, null);

                if (baseStackNode.ConfigType.Name == (string)configName)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Initialize(BaseGraphView graphView)
        {
            base.Initialize(graphView);

            InitNodeCustom();
        }

        public void InitNodeCustom()
        {
            //资源路径
            //var visualTree = Resources.Load<VisualTreeAsset>(NodeCustomPath);
            //if (visualTree == null)
            //{
            //    Debug.Log($"Don`t Find Path : {NodeCustomPath} ");
            //}
            //VisualElement labelFromUXML = visualTree.Instantiate();

            //nodeInfoLabel = labelFromUXML.Q<Label>("NodeHideCounter");
            //hideChildsBtn = labelFromUXML.Q<Button>("HideChildsBtn");
            //logCounter = labelFromUXML.Q<Label>("LogCounter");

            //this.outputContainer.Add(labelFromUXML);
            //m_CollapseButton.parent.Add(hideChildsBtn);
            //titleContainer.Add(hideChildsBtn);
            //Add(logCounter);

            //hideChildsBtn.clickable.clicked += ToggleCollapse;
            //logCounter.visible = false;
        }

    }
}

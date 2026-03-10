using GraphProcessor;
using System;
using UnityEngine.UIElements;

namespace NodeEditor
{
    [NodeCustomEditor(typeof(ConfigBaseNode))]
    public class ConfigBaseNodeView : BaseNodeView
    {
        // TODO 描述显示方式优化
        // 改成自带编辑器
        //HelpBox helpBox = new HelpBox() { messageType = HelpBoxMessageType.Info };

        private ConfigBaseNode configBaseNode;
        public ConfigBaseNode ConfigBaseNode { get { return configBaseNode; } }

        public override void Enable()
        {
            base.Enable();
            configBaseNode = nodeTarget as ConfigBaseNode;
            configBaseNode.OnNodeChanged += OnNodeChanged;
            configBaseNode.OnConfigNodeChanged += OnConfigChaged;
        }

        public override void Disable()
        {
            configBaseNode.OnNodeChanged -= OnNodeChanged;
            configBaseNode.OnConfigNodeChanged -= OnConfigChaged;
            base.Disable();
        }
        protected override void DrawDefaultInspector(bool fromInspector = false)
        {
            //UpdateHelpBox();
            //controlsContainer.Add(helpBox);

            base.DrawDefaultInspector(fromInspector);
        }

        protected override void UpdateFieldValues()
        {
            base.UpdateFieldValues();

            //UpdateHelpBox();
        }

        //private void UpdateHelpBox()
        //{
        //    var node = nodeTarget as ConfigBaseNode;
        //    // 显示模板参数列表
        //    if (node.IsTemplate)
        //    {
        //        helpBox.text = node.desc;
        //        helpBox.visible = !string.IsNullOrEmpty(node.desc);
        //    }
        //    else
        //    {
        //        helpBox.visible = false;
        //    }
        //}

        private void OnNodeChanged(string filedName)
        {
            //owner.RegisterCompleteObjectUndo($"Node Changed");
            if (filedName == nameof(configBaseNode.Desc))
            {
                controlsContainer.Clear();
                DrawDefaultInspector();
            }
            NotifyNodeChanged();
            UpdateFieldValues();
        }

        private void OnConfigChaged(string oldJson, string newJson)
        {
            if(!initializing)
                owner.AddUnReDoInfo(nodeTarget.GUID, BaseGraphView.URDControlType.URDCT_ConfigNodeChange, oldJson, newJson);
        }

        protected override void ToggleCollapse()
        {
            //base.ToggleCollapse();

            if (nodeTarget.hideChildNodes)
            {
                OpenChildNodeViews();
            }
            else
            {
                HideChildNodeViews();
            }
        }

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public override bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            return configBaseNode.SpecificNodeFiltering(outputPort, portTitle, portType);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
        }

        public override void ResetConfigNodeView(string configJson)
        {
            if(configBaseNode != null)
            {
                configBaseNode.DeserializeFromJson(configJson);
            }
        }
    }
}

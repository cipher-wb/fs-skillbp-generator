using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;

namespace GraphProcessor
{
    class NodeTooltipView : VisualElement
    {
        VisualElement m_ContentContainer;

        public NodeTooltipView()
        {
            pickingMode = PickingMode.Ignore;
            styleSheets.Add(Resources.Load<StyleSheet>("GraphProcessorStyles/NodeTooltip"));
            var uxml = Resources.Load<VisualTreeAsset>("GraphProcessorElements/NodeTooltip");
            uxml.CloneTree(this);

            // Get the element we want to use as content container
            m_ContentContainer = this.Q("contentContainer");
            RegisterCallback<MouseDownEvent>(OnMouseDown);
            RegisterCallback<MouseUpEvent>(OnMouseUp);
        }

        void OnMouseUp(MouseUpEvent evt)
        {
            evt.StopPropagation();
        }

        void OnMouseDown(MouseDownEvent evt)
        {
            evt.StopPropagation();
        }

        public override VisualElement contentContainer
        {
            get { return m_ContentContainer; }
        }
    }
}
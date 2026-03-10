using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Linq;

namespace GraphProcessor
{
    /// <summary>
    /// Custom editor of the node inspector, you can inherit from this class to customize your node inspector.
    /// </summary>
    [CustomEditor(typeof(NodeInspectorObject))]
    //public class NodeInspectorObjectEditor : OdinEditor
    //{
    //    NodeInspectorObject inspector;
    //    protected VisualElement root;
    //    protected VisualElement selectedNodeList;
    //    protected VisualElement placeholder;

    //    protected virtual void OnEnable()
    //    {
    //        inspector = target as NodeInspectorObject;
    //        inspector.nodeSelectionUpdated += UpdateNodeInspectorList;
    //        root = new VisualElement();
    //        selectedNodeList = new VisualElement();
    //        selectedNodeList.styleSheets.Add(Resources.Load<StyleSheet>("GraphProcessorStyles/InspectorView"));
    //        root.Add(selectedNodeList);
    //        placeholder = new Label("Select a node to show it's settings in the inspector");
    //        placeholder.AddToClassList("PlaceHolder");
    //        UpdateNodeInspectorList();
    //    }

    //    protected virtual void OnDisable()
    //    {
    //        inspector.nodeSelectionUpdated -= UpdateNodeInspectorList;
    //    }

    //    public override VisualElement CreateInspectorGUI() => root;

    //    protected virtual void UpdateNodeInspectorList()
    //    {
    //        selectedNodeList.Clear();

    //        if (inspector.selectedNodes.Count == 0)
    //            selectedNodeList.Add(placeholder);

    //        foreach (var nodeView in inspector.selectedNodes)
    //            selectedNodeList.Add(CreateNodeBlock(nodeView));
    //    }

    //    protected VisualElement CreateNodeBlock(BaseNodeView nodeView)
    //    {
    //        var view = new VisualElement();

    //        view.Add(new Label(nodeView.nodeTarget.name));

    //        var tmp = nodeView.controlsContainer;
    //        nodeView.controlsContainer = view;
    //        nodeView.Enable(true);
    //        nodeView.controlsContainer.AddToClassList("NodeControls");
    //        var block = nodeView.controlsContainer;
    //        nodeView.controlsContainer = tmp;

    //        return block;
    //    }
    //}
    public class NodeInspectorObjectEditor : OdinEditor
    {
        NodeInspectorObject inspector;

        protected override void OnEnable()
        {
            base.OnEnable();
            inspector = target as NodeInspectorObject;
            inspector.nodeSelectionUpdated += UpdateNodeInspectorList;
        }

        protected override void OnDisable()
        {
            inspector.nodeSelectionUpdated -= UpdateNodeInspectorList;
        }

        protected virtual void UpdateNodeInspectorList(bool isSelectChanged)
        {
            if (isSelectChanged)
            {
                Repaint();
            }
        }

        public override void OnInspectorGUI()
        {
            try
            {
                base.OnInspectorGUI();
            }
            catch (ExitGUIException)
            {
                GUIUtility.ExitGUI();
            }
            catch (Exception e)
            {
                //捕捉Unity的异常，通过日志接口来输出
                Utils.Error($"NodeInspectorObject.OnInspectorGUI error ex: {e}");
            }
            //Repaint();
        }
    }

    /// <summary>
    /// Node inspector object, you can inherit from this class to customize your node inspector.
    /// </summary>
    public class NodeInspectorObject : SerializedScriptableObject
    {
        [HideInInspector]
        /// <summary>Previously selected object by the inspector</summary>
        public Object previouslySelectedObject;
        /// <summary>List of currently selected nodes</summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("当前选择节点列表:")]
        [ListDrawerSettings(ShowFoldout = true, HideAddButton = true, HideRemoveButton = true, DraggableItems = false, NumberOfItemsPerPage = 1)]
        public HashSet<BaseNodeView> selectedNodes { get; private set; } = new HashSet<BaseNodeView>();

        private BaseNodeView lastNodeView = null;

        /// <summary>Triggered when the selection is updated</summary>
        public event Action<bool> nodeSelectionUpdated;

        /// <summary>Updates the selection from the graph</summary>
        public virtual void UpdateSelectedNodes(HashSet<BaseNodeView> views)
        {
            selectedNodes = views;
            RefreshNodes();
        }

        public virtual void RefreshNodes()
        {
            bool isNeedRefresh = IsNeedRefresh();
            nodeSelectionUpdated?.Invoke(isNeedRefresh);
            lastNodeView = selectedNodes.FirstOrDefault();
        }

        public virtual void NodeViewRemoved(BaseNodeView view)
        {
            selectedNodes.Remove(view);
            RefreshNodes();
        }

        private bool IsNeedRefresh()
        {
            if (lastNodeView != selectedNodes.FirstOrDefault())
            {
                return true;
            }
            return false;
        }
    }
}
using DG.DemiEditor;
using GameApp;
using GameApp.Native.Battle;
using GraphProcessor;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.IMGUI.Controls;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameApp.Battle.Battle;
using TreeView = UnityEditor.IMGUI.Controls.TreeView;

namespace NodeEditor
{
    public sealed class ConfigNodesTreeView : TreeView
    {
        private TreeViewItem root;

        #region Event
        private Action<int> onSingleClicked;
        public event Action<int> OnSingleClicked
        {
            add
            {
                onSingleClicked -= value;
                onSingleClicked += value;
            }
            remove { onSingleClicked -= value; }
        }
        private Action<int> onDoubleClicked;
        public event Action<int> OnDoubleClicked
        {
            add
            {
                onDoubleClicked -= value;
                onDoubleClicked += value;
            }
            remove { onDoubleClicked -= value; }
        }
        #endregion

        public ConfigNodesTreeView(TreeViewState state) : base(state)
        {
            root = new TreeViewItem(-1, -1, "节点列表");
            this.showBorder = true;
        }

        protected override void SingleClickedItem(int id)
        {
            base.SingleClickedItem(id);
            onSingleClicked?.Invoke(id);
        }
        protected override void DoubleClickedItem(int id)
        {
            base.DoubleClickedItem(id);
            onDoubleClicked?.Invoke(id);
        }

        protected override TreeViewItem BuildRoot()
        {
            return root;
        }
        public void Refresh(List<TreeViewItem> treeViewItems)
        {
            root.children = treeViewItems;
            Reload();
        }

        protected override void AfterRowsGUI()
        {
            base.AfterRowsGUI();
        }
    }

    [Obsolete("采用IMGUI实现，已经统一UIToolkit方案实现")]
    public sealed class OldConfigPinnedView : PinnedElementView
    {
        public enum TabType
        {
            NodeExistList = 0,   // 当前节点列表
            NodeTypeList = 1,   // 节点类型列表
            ConsoleList = 2,   //调试节点列表
        }

        private readonly string[] tabDescs = new string[] { "当前节点列表", "节点类型列表", "日志节点列表" };

        private BaseGraphView graphView;

        private TabType currentTab = TabType.NodeExistList;
        private ToolbarPopupSearchField popupSearchField;
        private ConfigNodesTreeView treeView;

        int clearLogFrame = 150;
        private int lastAllNodeCount;

        //bool m_popupSearchFieldOn;
        public bool needRefresh;
        [HideInInspector]
        public string NodeCustomPath = "Assets/Thirds/NodeGraphProcessor/Editor/Resources/GraphProcessorElements/DragCreateNode.uxml";
        public VisualElement dragCreateNodeBox;

        public OldConfigPinnedView()
        {
            title = "节点列表";
            treeView = new ConfigNodesTreeView(new TreeViewState());
            treeView.OnDoubleClicked += OnDoubleClickItem;
            treeView.OnSingleClicked += OnSingleClickItem;
        }
        protected override void Initialize(BaseGraphView graphView)
        {
            return;
            this.graphView = graphView;
            if (this.graphView != null && dragCreateNodeBox == null)
            {
                var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(NodeCustomPath);
                VisualElement labelFromUXML = visualTree.Instantiate();
                dragCreateNodeBox = labelFromUXML.Q<VisualElement>("DragCreateNodeBox");
                graphView.Add(dragCreateNodeBox);
                dragCreateNodeBox.visible = false;
            }
            popupSearchField = new ToolbarPopupSearchField();
            popupSearchField.RegisterValueChangedCallback(OnSearchTextChanged);
            popupSearchField.tooltip = "搜索";
            // TODO Filter？
            //popupSearchField.menu.AppendAction(
            //    "Popup Search Field",
            //    a => m_popupSearchFieldOn = !m_popupSearchFieldOn,
            //    a => m_popupSearchFieldOn ?
            //    DropdownMenuAction.Status.Checked :
            //    DropdownMenuAction.Status.Normal);
            content.Add(popupSearchField);

            if (graphView is ConfigGraphView configGraphView)
            {
                configGraphView.KeyDownCallbackEvent -= KeyDownCallback;
                configGraphView.KeyDownCallbackEvent += KeyDownCallback;
            }

            var treeNodes = new IMGUIContainer(() =>
            {
                EditorGUI.BeginChangeCheck();
                currentTab = (TabType)GUILayout.Toolbar((int)currentTab, tabDescs, GUILayout.ExpandWidth(false), GUILayout.Height(21));
                if (EditorGUI.EndChangeCheck())
                {
                    needRefresh = true;
                }
                switch (currentTab)
                {
                    case TabType.NodeExistList:
                        if (GUILayout.Button($"当前节点总数:{lastAllNodeCount}）"))
                        {
                            needRefresh = true;
                        }
                        break;
                    case TabType.ConsoleList:
                        ConsoleSelect();

                        EditorGUILayout.BeginHorizontal();
                        {
                            clearLogFrame = EditorGUILayout.IntField(clearLogFrame);

                            if (GUILayout.Button("清空"))
                            {
                                ClearLogFrame();
                            }

                        }
                        EditorGUILayout.EndHorizontal();
                        break;
                }
                OnRefresh();
                Rect treeRect = GUILayoutUtility.GetRect(300, 300, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                treeRect.y += 2;
                treeRect.height -= 2;
                treeView.OnGUI(treeRect);
            });
            content.Add(treeNodes);
        }
        private void OnSearchTextChanged(ChangeEvent<string> evt)
        {
            needRefresh = true;
        }

        public void KeyDownCallback(KeyDownEvent e)
        {
            switch (currentTab)
            {
                case TabType.NodeTypeList:
                    {
                        if (e.keyCode != KeyCode.G || tempCreateNodeType == default || !(graphView is ConfigGraphView configGraphView))
                        {
                            break;
                        }

                        Vector2 mousePos = (e.currentTarget as VisualElement).ChangeCoordinatesTo(configGraphView.contentViewContainer, e.originalMousePosition);
                        graphView.AddNode(BaseNode.CreateFromType(tempCreateNodeType, mousePos));
                        break;
                    }
                case TabType.ConsoleList:
                    {
                        //暂停
                        if (e.keyCode != KeyCode.P || !(graphView is ConfigGraphView configGraphView))
                        {
                            break;
                        }

                        //恢复暂停
                        if (Time.timeScale == 0)
                        {
                            Time.timeScale = 1;
                        }
                        //暂停
                        else
                        {
                            Time.timeScale = 0;
                            ClearLogFrame();
                        }

                        break;
                    }

            }
        }

        private void OnRefresh()
        {
            switch (currentTab)
            {
                case TabType.NodeTypeList:
                    if (UnityEngine.Event.current.type == EventType.MouseDrag && tempCreateNodeType != null)
                    {
                        Vector2 mousePos = this.ChangeCoordinatesTo(graphView, UnityEngine.Event.current.mousePosition);
                        dragCreateNodeBox.style.left = mousePos.x;
                        dragCreateNodeBox.style.top = mousePos.y;
                        dragCreateNodeBox.visible = true;
                        dragCreateNodeBox.BringToFront();
                    }
                    else if (UnityEngine.Event.current.type == EventType.MouseUp && dragCreateNodeBox.visible)
                    {

                        if (!this.BoundingBoxTest(dragCreateNodeBox) && tempCreateNodeType != null)
                        {
                            Vector2 mousePos = this.ChangeCoordinatesTo(graphView.contentViewContainer, UnityEngine.Event.current.mousePosition);
                            graphView.AddNode(BaseNode.CreateFromType(tempCreateNodeType, mousePos));
                        }

                        dragCreateNodeBox.visible = false;
                        tempCreateNodeType = null;
                    }

                    if (needRefresh)
                    {
                        var searchText = popupSearchField.value;
                        var listItems = new List<TreeViewItem>();
                        int i = 0;
                        foreach (var nodeItem in NodeProvider.GetNodeMenuEntries(graphView.graph))
                        {
                            //Log.Error($"{nodeItem.path}, {nodeItem.type}");
                            var displayName = $"{nodeItem.path}";
                            if (string.IsNullOrEmpty(searchText) || displayName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                            {
                                var item = new TreeViewItem
                                {
                                    id = ++i,
                                    displayName = displayName,
                                };

                                listItems.Add(item);
                            }


                        }
                        treeView.Refresh(listItems);
                    }
                    break;
                case TabType.NodeExistList:
                    {
                        var allNodeCount = graphView.nodeViews.Count;
                        if (needRefresh || lastAllNodeCount != allNodeCount)
                        {
                            lastAllNodeCount = allNodeCount;
                            var searchText = popupSearchField.value;
                            var listItems = new List<TreeViewItem>();
                            for (int i = 0; i < allNodeCount; i++)
                            {
                                var nodeView = graphView.nodeViews[i];
                                var node = nodeView.nodeTarget;
                                var displayName = $"{i}-{node.GetCustomName()}";
                                if (string.IsNullOrEmpty(searchText) || displayName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    var item = new TreeViewItem
                                    {
                                        id = i,
                                        displayName = displayName,
                                    };
                                    listItems.Add(item);
                                }
                            }
                            treeView.Refresh(listItems);
                        }
                        break;
                    }
                case TabType.ConsoleList:
                    {
                        var allNodeCount = graphView.selection.Count;
                        if (needRefresh || lastAllNodeCount != allNodeCount || graphView.isReframable)
                        {
                            lastAllNodeCount = allNodeCount;
                            var searchText = popupSearchField.value;
                            var listItems = new List<TreeViewItem>();
                            for (int i = 0; i < allNodeCount; i++)
                            {
                                if (!(graphView.selection[i] is BaseNodeView nodeView))
                                {
                                    continue;
                                }

                                var node = nodeView.nodeTarget;
                                var displayName = $"{i}-{node.GetCustomName()}";
                                if (string.IsNullOrEmpty(searchText) || displayName.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) != -1)
                                {
                                    var item = new TreeViewItem
                                    {
                                        id = i,
                                        displayName = displayName,
                                    };
                                    listItems.Add(item);

                                    if (!(graphView is ConfigGraphView configGraphView) || configGraphView.GetConfigGraphWindow() == default) continue;
                                    if (configGraphView.GetConfigGraphWindow().allConsoleConfigIdInfoDic.TryGetValue(node.GUID, out var logs))
                                    {
                                        var logsList = logs.ToList();
                                        var debugInfos = new List<string>();
                                        for (var j = logs.Count - 1; j >= 0; j--)
                                        {
                                            var log = logsList[j];
                                            if (string.IsNullOrEmpty(log))
                                                continue;
                                            debugInfos.AddRange(log.Split('\n'));
                                        }
                                        debugInfos.ForEach((info) =>
                                        {
                                            var infoItem = new TreeViewItem
                                            {
                                                id = i,
                                                displayName = info,
                                            };
                                            item.AddChild(infoItem);
                                        });
                                    }
                                }
                            }
                            treeView.Refresh(listItems);
                            treeView.ExpandAll();
                        }
                        break;
                    }
            }
            needRefresh = false;
        }


        public void ClearLogFrame()
        {
            if (!(graphView is ConfigGraphView configGraphView))
            {
                return;
            }

            var configGraphWindow = configGraphView.GetConfigGraphWindow();

            configGraphWindow.HideEdgeFlowPoint(clearLogFrame);
            configGraphWindow.ShowEdgeFlowPoint();
        }

        private void ConsoleSelect()
        {
            if (!(graphView is ConfigGraphView configGraphView) || configGraphView.GetConfigGraphWindow() == default) return;
            ConfigGraphWindow configGraphWindow = configGraphView.GetConfigGraphWindow();

            string isConsole = $"日志({(configGraphWindow.enableConsole ? "开启" : "关闭")})";
            string isDebug = $"调试({(configGraphWindow.isDebug ? "开启" : "关闭")})";

            int select = GUILayout.Toolbar(-1, new string[] { isConsole });

            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                configGraphWindow.isDebug = false;
                configGraphWindow.enableConsole = false;
                AppFacade.EnableEditorConsole = false;

                if (select >= 0)
                {
                    configGraphWindow.ShowNotification("请进入战斗中后再开启");
                }
                return;
            }

            if (select < 0) return;

            switch (select)
            {
                case 0:
                    //configGraphWindow.EnableConsole(!configGraphWindow.enableConsole);
                    //if ((ConfigGraphWindow.ConsoleCount > 0 && !AppFacade.EnableEditorConsole)
                    //    || (ConfigGraphWindow.ConsoleCount <= 0 && AppFacade.EnableEditorConsole))
                    //{
                    //    AppFacade.EnableEditorConsole = !AppFacade.EnableEditorConsole;
                    //    BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_ENABLE_SKILL_EFFECT_LOG, "0", "0", "0");
                    //}
                    break;
                case 1:
                    configGraphWindow.isDebug = !configGraphWindow.isDebug;
                    if (!configGraphWindow.isDebug)
                    {
                        battle.ResumeGame();
                        battle.SetBattleLogicUpdateStop(false, BattleLogicUpdateStopType.Default);
                    }
                    break;
            }

        }

        Type tempCreateNodeType = null;
        private void OnSingleClickItem(int id)
        {
            switch (currentTab)
            {
                case TabType.NodeExistList:
                    var nodeView = graphView.nodeViews.ExGet(id);
                    nodeView?.FocusSelect(false);
                    break;
                case TabType.NodeTypeList:
                    var path = treeView.GetRows()[id - 1].displayName;
                    var nodeEntries = NodeProvider.GetNodeMenuEntries(graphView.graph);
                    tempCreateNodeType = nodeEntries.Where(n => n.path == path).First().type;
                    break;
            }
        }
        private void OnDoubleClickItem(int id)
        {
            switch (currentTab)
            {
                case TabType.NodeExistList:
                    var nodeView = graphView.nodeViews.ExGet(id);
                    nodeView?.FocusSelect();
                    break;
            }
        }
    }
}
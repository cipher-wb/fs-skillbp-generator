using UnityEngine.UIElements;
using UnityEngine;
using System;
using GraphProcessor;
using NodeEditor.NpcEventEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.Linq;

namespace NodeEditor
{
    public class MapEventPinnedView : PinnedElementView
	{
        public enum TabType
        {
            NodeList = 0,       //节点列表
            ErrorList,          //错误列表
            Max                 //最大数量，请定义在此之前
        }

        public override string pinnedElementTree => "GraphProcessorElements/MapEventPinned";

        public ConfigGraphView graphView;

        public ConfigGraphWindow graphWindow;
        public TabType CurrentTab { get; private set; } = TabType.NodeList;
        public int CurrentTabIndex => (int)CurrentTab;
        public ScrollView TreeView { get; private set; }
        public ToolbarPopupSearchField SearchBar { get; private set; }
        public Toolbar Toolbar { get; private set; }
        public List<VisualElement> ToolbarTabs { get; private set; }
        public Button ToolBtn { get; private set; }

        /// <summary>
        /// NodeList中选中节点
        /// </summary>
        public VisualElement SelectedNodeButton { get; private set; }

        protected override void Initialize(BaseGraphView graphView)
        {
            if (!(graphView is NpcEventGraphView configGraphView))
            {
                return;
            }

            title = "工具栏";
            this.graphView = configGraphView;
            this.graphView.graph.curTab = (int)TabType.NodeList;

            this.graphWindow = configGraphView.GetConfigGraphWindow();

            InitLisenter();

            InitView();
        }

        public void InitLisenter()
        {
            graphView.graph.onGraphChanges -= OnGraphChanges;
            graphView.graph.onGraphChanges += OnGraphChanges;

            graphView.graph.graphFatalCallback -= AddToFatalList;
            graphView.graph.graphFatalCallback += AddToFatalList;
        }

        public void InitView()
        {
            try
            {
                ToolBtn = this.Q<Button>("ToolBtn");

                //显示列表
                content = this.Q<VisualElement>("content");
                TreeView = this.Q<ScrollView>("TreeView");

                //Tab栏
                Toolbar = this.Q<Toolbar>("ToolBar");
                ToolbarTabs = Toolbar.Children().Where(c => c is Button).ToList();
                for (int i = 0; i < Toolbar.childCount; i++)
                {
                    var button = ToolbarTabs[i] as Button;
                    if(button == default) { continue; }

                    Action clickTabButton = () =>
                    {
                        ChangeTab(ToolbarTabs.IndexOf(button));
                    };

                    button.clickable.clicked -= clickTabButton;
                    button.clickable.clicked += clickTabButton;
                }

                //搜索栏
                SearchBar = this.Q<ToolbarPopupSearchField>("SeachBar");
                SearchBar.RegisterValueChangedCallback((s) =>
                {
                    if (s.newValue != string.Empty)
                        ChangeTab(CurrentTabIndex);
                });

                ChangeTab(graphView.graph.curTab);
            }
            catch
            {
                Log.Fatal("Cannot Find The Corresponding Component");
            }
        }

        public void ChangeTab(int index)
        {
            //选中状态
            for(int i = 0; i < ToolbarTabs.Count; i++)
            {
                ToolbarTabs[i].OnSelectCilcked(i == index ? true : false);
            }

            if (CurrentTabIndex != index)
            {
                SearchBar.value = string.Empty;
            }

            CurrentTab = (TabType)index;
            graphView.graph.curTab = index;

            TreeView.style.display = DisplayStyle.None;
            TreeView.Clear();

            ToolBtn.style.display = DisplayStyle.None;
            ToolBtn.clickable = null;
            ToolBtn.tooltip = string.Empty;

            switch (CurrentTab)
            {
                case TabType.NodeList:
                    {
                        CreateNodeList();
                        break;
                    }
                case TabType.ErrorList:
                    {

                        break;
                    }
            }

            TreeView.style.display = DisplayStyle.Flex;

            //OnRefresh();
        }

        void OnGraphChanges(GraphChanges changes)
        {
            if (CurrentTab != TabType.NodeList) { return; }

            if (changes.addedNode != null)
            {
                CreateNode(changes.addedNode);
            }

            if (changes.removedNode != null)
            {
                string name = changes.removedNode.GetCustomName();
                var children = TreeView.Children().ToList();
                VisualElement element = children.Where(e => e.name == name).FirstOrDefault();
                if (element != null)
                {
                    TreeView.Remove(element);
                }
            }
        }

        void CreateNodeList()
        {
            graphView.nodeViews?.ForEach(node =>
            {
                CreateNode(node.nodeTarget);
            });
        }

        void CreateNode(BaseNode node)
        {
            var searchName = SearchBar.value;
            string extend = !node.isVisible ? "：隐藏" : string.Empty;
            var displayName = $"{node.GetCustomName()}{extend}";
            if (string.IsNullOrEmpty(searchName) 
                || displayName.IndexOf(searchName, StringComparison.OrdinalIgnoreCase) != -1)
            {
                Button nodebtn = null;
                nodebtn = Utils.CreateTextBtn(displayName, () =>
                {
                    SelectedNodeButton?.OnSelectCilcked(false);

                    if (!graphView.nodeViewsPerNode.TryGetValue(node, out BaseNodeView nodeView))
                    {
                        return;
                    }

                    //视图中选中节点
                    nodeView?.FocusSelect(true);

                    SelectedNodeButton = nodebtn;
                    SelectedNodeButton.OnSelectCilcked();
                });

                nodebtn.name = node.GetCustomName();

                TreeView.Add(nodebtn);
            }
        }

        void AddToFatalList(string fatal, object obj)
        {
            CreateFatalList();
        }

        void CreateFatalList()
        {
            TreeView.Clear();

            List<(string, object)> plannerFatals = graphView.graph.fatalInfos.ToList();
            plannerFatals?.ForEach(tuple =>
            {
                CreateFatal(tuple.Item1, tuple.Item2);
            });
        }

        void CreateFatal(string fatal, object obj = null)
        {
            string searchText = SearchBar.value;
            string[] fatalChilds = fatal.Split('\n');
            if (fatalChilds.Length <= 0)
            {
                return;
            }

            string fatalHead = fatalChilds[0];
            if (!string.IsNullOrEmpty(searchText) && fatalHead.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) == -1)
            {
                return;
            }

            var fatalBtn = Utils.CreateTextBtn(fatalHead, () =>
            {
                if (obj is BaseNodeView baseNodeView
                || (obj is BaseNode baseNode && graphView.nodeViewsPerNode.TryGetValue(baseNode, out baseNodeView)))
                {
                    baseNodeView?.FocusSelect();
                }
            });

            fatalBtn.text = fatalHead;
            fatalBtn.tooltip = fatal;
            fatalBtn.style.color = new StyleColor(Color.red);
            TreeView.Add(fatalBtn);
        }
    }
}

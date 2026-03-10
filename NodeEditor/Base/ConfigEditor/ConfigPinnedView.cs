using GameApp;
using GameApp.Native.Battle;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using Funny.Base.Utils;
using TableDR;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class ConfigPinnedView : PinnedElementView
    {
        public enum TabType
        {
            None = -1,
            NodeExistList = 0,  // 当前节点列表
            NodeTypeList = 1,   // 节点类型列表
            ParamNodeList = 2,  // 小黑板
            ConsoleList = 3,    // 调试节点列表
            FatalList = 4,      // Fatal级别错误列表
            Max                 // 最大数量，请定义在此之前
        }

        public ConfigGraphView graphView;
        public ConfigGraphWindow graphWindow;
        private TabType currentTab = TabType.NodeExistList;

        public List<VisualElement> toolbarBtns;
        private ToolbarPopupSearchField searchField;
        private Toolbar toolbar;
        private Button toolBtn;
        private Button dmgLogBtn;
        private SliderInt dmgLogLevelSlider;
        private Toggle dmgShowIDToggle;
        private SliderInt toolSlider;
        private Button clearBtn;
        private Label fatalCount;
        //ListView无法实现多级目录
        private ScrollView treeView;

        private int clearLogFrame = 30;
        private int lastAllNodeCount;
        private int lastDebugNodeCount;

        public const string NodeCustomPath = "GraphProcessorElements/NodeCustom";
        public VisualElement dragCreateNodeBox;
        public Color focusColor = Color.white;
        long clickTime;

        public ConfigPinnedView()
        {
            title = "工具栏";
            //EditorApplication.update += OnUpdate;
        }

        protected override void Initialize(BaseGraphView graphView)
        {
            if (!(graphView is ConfigGraphView configGraphView))
            {
                Log.Fatal("This Feature Is Currently Only Available For The ConfigGraphView");
                return;
            }

            //EditorApplication.update -= OnUpdate;
            //EditorApplication.update += OnUpdate;

            this.graphView = configGraphView;
            this.graphWindow = configGraphView.GetConfigGraphWindow();

            var visualTree = Resources.Load<VisualTreeAsset>(NodeCustomPath);
            VisualElement labelFromUXML = visualTree.Instantiate();
            //初始化拖拽创建节点
            if (graphView != null && dragCreateNodeBox == null)
            {
                dragCreateNodeBox = labelFromUXML.Q<VisualElement>("DragCreateNodeBox");
                graphView.Add(dragCreateNodeBox);
                dragCreateNodeBox.visible = false;

                InitLisenter();
            }

            InitView();
        }

        public void InitLisenter()
        {
            graphView.graph.onGraphChanges -= ChangeNodeExistList;
            graphView.graph.onGraphChanges += ChangeNodeExistList;
            graphWindow.LogEvent -= AddConsoleLog;
            graphWindow.LogEvent += AddConsoleLog;
            graphView.MouseCallBackEvent -= MouseCallBackEvent;
            graphView.MouseCallBackEvent += MouseCallBackEvent;
            graphWindow.OnRefreshPinnedView -= OnRefresh;
            graphWindow.OnRefreshPinnedView += OnRefresh;

            Log.Inst.fatalLogCallBackEvent -= AddToFatalList;
            Log.Inst.fatalLogCallBackEvent += AddToFatalList;
            //WWYTODO-=和+=有问题，记得修复一下
            graphView.graph.graphFatalCallback -= AddToFatalList;
            graphView.graph.graphFatalCallback += AddToFatalList;
        }

        public void InitView()
        {
            try
            {
                searchField = this.Q<ToolbarPopupSearchField>("SeachBar");
                content = this.Q<VisualElement>("content");
                toolbar = this.Q<Toolbar>("ToolBar");
                toolBtn = this.Q<Button>("ToolBtn");
                dmgLogBtn = this.Q<Button>("DmgLogBtn");
                dmgLogLevelSlider = this.Q<SliderInt>("DmgLogLevelSlider");
                dmgShowIDToggle = this.Q<Toggle>("ShowIDToggle");
                toolSlider = this.Q<SliderInt>("ToolSlider");
                clearBtn = this.Q<Button>("ClearBtn");
                treeView = this.Q<ScrollView>("TreeView");
                fatalCount = this.Q<Label>("FatalCount");
                toolbarBtns = toolbar.Children().Where(c => c is Button).ToList();

                if (toolbarBtns.Count < (int)TabType.Max)
                {
                    Log.Fatal("Unreasonable Definition");
                    return;
                }

                for (int i = toolbarBtns.Count - 1; i >= 0; i--)
                {
                    if (!(toolbarBtns[i] is Button button))
                        continue;

                    Action changeToolBarView = () =>
                    {
                        ChangeToolBarView(toolbarBtns.IndexOf(button));
                    };

                    button.clickable.clicked -= changeToolBarView;
                    button.clickable.clicked += changeToolBarView;

                }
                searchField.RegisterValueChangedCallback((s) =>
                {
                    if(s.newValue != string.Empty)
                        ChangeToolBarView((int)currentTab);
                });

                ChangeToolBarView(graphView.graph.curTab);
            }
            catch
            {
                Log.Fatal("Cannot Find The Corresponding Component");
            }
        }

        public void OnUpdate()
        {
            if (!this.pinnedElement.opened)
            {
                EditorApplication.update -= OnUpdate;
                return;
            }
        }

        private void MouseCallBackEvent(EventType eventType, IMouseEvent mouseEvent)
        {
            switch(eventType)
            {
                case EventType.MouseDown:
                    if(currentTab != TabType.ConsoleList)
                    {
                        break;
                    }

                    OnRefresh();
                    break;
            }
        }

        public void ChangeToolBarView(int index)
        {
            if (currentTab != TabType.None)
            {
                toolbarBtns[(int)currentTab]?.OnSelectCilcked(false);
            }

            if((int)currentTab != index)
            {
                searchField.value = string.Empty;
            }

            Button lastToolbarBtn = toolbarBtns[(int)currentTab] as Button;
            Button curToolbarBtn = toolbarBtns[index] as Button;
            currentTab = (TabType)index;
            graphView.graph.curTab = index;

            if (index < 0 || toolbarBtns.Count <= index || curToolbarBtn == null)
            {
                return;
            }

            curToolbarBtn.OnSelectCilcked();
            treeView.style.display = DisplayStyle.None;
            toolBtn.style.display = DisplayStyle.None;
            toolSlider.style.display = DisplayStyle.None;
            toolBtn.clickable = null;
            toolBtn.tooltip = string.Empty;
            dmgLogBtn.style.display = DisplayStyle.None;
            dmgLogBtn.clickable = null;
            dmgLogBtn.tooltip = "开启伤害计算过程输出到控制台";
            dmgLogLevelSlider.style.display = DisplayStyle.None;

            treeView.Clear();
            switch (currentTab)
            {
                case TabType.NodeExistList:
                    {
                        toolBtn.style.display = DisplayStyle.Flex;

                        lastAllNodeCount = graphView.nodeViews.Count;

                        toolBtn.clicked -= ClickShowAllNode;
                        toolBtn.clicked += ClickShowAllNode;

                        CreateNodeExistList();
                        break;
                    }
                case TabType.NodeTypeList:
                    {

                        break;
                    }
                case TabType.ParamNodeList:
                    {
                        toolBtn.style.display = DisplayStyle.Flex;
                        CreateParamNodeList();
                        break;
                    }
                case TabType.ConsoleList:
                    {
                        toolBtn.style.display = DisplayStyle.Flex;
                        dmgLogBtn.style.display = DisplayStyle.Flex;
                        toolSlider.style.display = DisplayStyle.Flex;
                        dmgLogLevelSlider.style.display = graphWindow.enableDmgLog ? DisplayStyle.Flex : DisplayStyle.None;

                        //清理节点逻辑
                        Action clearBtnEvt = () =>
                        {
                            graphWindow.HideEdgeFlowPoint(clearLogFrame);
                            graphWindow.ShowEdgeFlowPoint();
                            OnRefresh();
                        };

                        //帧数同步的事件
                        EventCallback<ChangeEvent<int>> valueChange = (changeEvent) =>
                        {
                            clearLogFrame = changeEvent.newValue;
                            toolSlider.label = $"清理{changeEvent.newValue}帧前的日志";
                        };

                        //开关节点逻辑
                        Action toolBtnEvt = () =>
                        {
                            graphWindow.EnableConsole(!graphWindow.enableConsole);
                            toolBtn.text = $"日志（已{(graphWindow.enableConsole ? "开启" : "关闭")}）";
                        };

                        //开关伤害计算输出功能
                        Action dmgLogBtnEvt = () =>
                        {
                            graphWindow.enableDmgLog = !graphWindow.enableDmgLog;
                            graphWindow.EnableConsole(graphWindow.enableDmgLog);
                            dmgLogBtn.text = graphWindow.enableDmgLog ? "伤害计算输出 开启中" : "开启伤害计算输出";
                            toolBtn.text = $"日志（已{(graphWindow.enableConsole ? "开启" : "关闭")}）";
                            dmgLogLevelSlider.style.display = graphWindow.enableDmgLog ? DisplayStyle.Flex : DisplayStyle.None;
                        };
                        EventCallback<ChangeEvent<int>> logLevelChange = (changeEvent) =>
                        {
                            graphWindow.dmgLogLevel = changeEvent.newValue;
                            dmgLogLevelSlider.label = $"伤害输出等级:{changeEvent.newValue}";
                        };
                        EventCallback<ChangeEvent<bool>> showIDTGChange = (changeEvent) =>
                        {
                            graphWindow.dmgLogIDVisible = changeEvent.newValue;
                        };
                       

                        toolBtn.clicked -= toolBtnEvt;
                        toolBtn.clicked += toolBtnEvt;

                        clearBtn.clicked -= clearBtnEvt;
                        clearBtn.clicked += clearBtnEvt;

                        toolSlider.UnregisterValueChangedCallback(valueChange);
                        toolSlider.RegisterValueChangedCallback(valueChange);

                        dmgLogBtn.clicked -= dmgLogBtnEvt;
                        dmgLogBtn.clicked += dmgLogBtnEvt;

                        dmgLogLevelSlider.UnregisterValueChangedCallback(logLevelChange);
                        dmgLogLevelSlider.RegisterValueChangedCallback(logLevelChange);

                        dmgShowIDToggle.UnregisterValueChangedCallback(showIDTGChange);
                        dmgShowIDToggle.RegisterValueChangedCallback(showIDTGChange);
                        break;
                    }
                case TabType.FatalList:
                    {
                        toolBtn.style.display = DisplayStyle.Flex;
                        toolBtn.text = "复制全部&清理";
                        toolBtn.tooltip = "复制所有程序问题并且进行清理";
                        Action copyEvt = () =>
                        {
                            if(treeView.childCount <= 0)
                            {
                                return;
                            }

                            Foldout foldout = treeView.Q<Foldout>("Procedure");
                            if (foldout != null)
                            {
                                string copyTxt = "";
                                for (int i = foldout.childCount - 1; i >= 0; i--)
                                {
                                    var ele = foldout[i];
                                    if (ele is Button btn)
                                    {
                                        copyTxt = $"{copyTxt}\n{btn.tooltip}";
                                    }
                                }
                                GUIUtility.systemCopyBuffer = copyTxt;
                                graphWindow.ShowNotification($"复制并清理成功,一共有{treeView.childCount}个程序问题，快去让程序解决");
                            }

                            treeView.Clear();
                            Log.Inst.fatalTroubles.Clear();
                            graphView.graph.fatalInfos.Clear();
                            OnRefresh();
                        };
                        toolBtn.clicked -= copyEvt;
                        toolBtn.clicked += copyEvt;
                        break;
                    }
            }

            treeView.style.display = DisplayStyle.Flex;
            OnRefresh();

            lastToolbarBtn.text = Regex.Replace(lastToolbarBtn.text, "\\([^()]*\\)$", string.Empty);
            curToolbarBtn.text = $"{curToolbarBtn.text}({treeView.childCount})";
        }

        

        public void OnRefresh()
        {
            Button curToolbarBtn = toolbarBtns[(int)currentTab] as Button;
            clickTime = 0;
            switch (currentTab)
            {
                case TabType.NodeExistList:
                    {
                        int hideNodeCount = 0;
                        int debugCount = 0;
                        for(int i = graphView.nodeViews.Count - 1; i >= 0; i--)
                        {
                            var nodeView = graphView.nodeViews[i];
                            if (nodeView!= null && !nodeView.nodeTarget.isVisible)
                            {
                                hideNodeCount++;
                            }
                            if (nodeView != null && nodeView.nodeTarget.debug)
                                debugCount++;
                        }

                        toolBtn.text = $"节点总数：（{graphView.nodeViews.Count}）|隐藏节点：（{hideNodeCount}）";
                        if (lastAllNodeCount != graphView.nodeViews.Count)
                        {
                            lastAllNodeCount = graphView.nodeViews.Count;
                            lastDebugNodeCount = debugCount;
                            CreateNodeExistList();
                        }
                        else if (lastDebugNodeCount != debugCount)
                        {
                            lastDebugNodeCount = debugCount;
                            treeView.Clear();
                            CreateNodeExistList();
                        }
                        break;
                    }
                case TabType.NodeTypeList:
                    {
                        CreateNodeTypeList();

                        break;
                    }
                case TabType.ParamNodeList:
                    {
                        CreateParamNodeList();
                        break;
                    }
                case TabType.ConsoleList:
                    {
                        toolBtn.text = $"日志（已{(graphWindow.enableConsole ? "开启" : "关闭")}）";
                        toolSlider.label = $"清理{toolSlider.value}帧前的日志";
                        dmgLogBtn.text = graphWindow.enableDmgLog ? "伤害计算输出 开启中" : "开启伤害计算输出";
                        dmgLogLevelSlider.style.display = graphWindow.enableDmgLog ? DisplayStyle.Flex : DisplayStyle.None;
                        CreateConsolelist();
                        break;
                    }
                case TabType.FatalList:
                    {
                        CreateFatalList();
                        break;
                    }
            }

            if (Log.Inst.fatalTroubles.Count > 0 || graphView.graph.fatalInfos.Count > 0)
            {
                fatalCount.visible = true;
                fatalCount.text = $"{Log.Inst.fatalTroubles.Count + graphView.graph.fatalInfos.Count}";
            }
            else
            {
                fatalCount.visible = false;
                fatalCount.text = "0";
            }
            
        }

        #region 封装方法
        int clickCounter;
        void ClickShowAllNode()
        {
            clickCounter++;
            if(clickCounter == 2 && DateTime.Now.Ticks - clickTime < int.MaxValue / 100)
            {
                clickCounter = 0;
                foreach (var nodeView in graphView.nodeViews)
                {
                    graphView.ShowOrHideNodeOutputEdges(nodeView.nodeTarget, true);
                    nodeView.visible = true;
                    nodeView.nodeTarget.visible = true;
                    nodeView.nodeTarget.hideCounter = 0;
                    nodeView.nodeTarget.hideChildNodes = false;
                }
            }
            else
            {
                clickCounter = 1;
                graphWindow.ShowNotification("再次点击展示所有节点");
                clickTime = DateTime.Now.Ticks;
            }

            treeView.Clear();
            CreateNodeExistList();
        }

        void ChangeNodeExistList(GraphChanges changes)
        {
            if (currentTab != TabType.NodeExistList)
            {
                return;
            }

            OnRefresh();

            //if (changes.addedNode != null)
            //{
            //    CreateNodeExistNode(changes.addedNode, treeView.childCount);
            //}

            //if(changes.removedNode != null)
            //{
            //    string name = changes.removedNode.GetCustomName();
            //    var children = treeView.Children().ToList();
            //    VisualElement element = children.Where(e => e.name == name).FirstOrDefault();
            //    if(element != null)
            //    {
            //        treeView.Remove(element);
            //    }

            //}
        }

        #region 节点列表
        VisualElement curSelectNodeEle;

        void CreateNodeExistList()
        {
            if (lastAllNodeCount == 0)
                return;
     
            var nodeGUIDs = graphView.graph.nodesPerGUID.Keys.ToHashSet();
            var elements = new List<(VisualElement, double)>();


            foreach (var groupView in graphView.groupViews)
            {
                var foldout = new Foldout();
                foldout.name = groupView.name;
                foldout.text = groupView.TitleLabel.text;
                foldout.style.color = Color.red;
                foldout.value = false;

                foreach (var nodeGUID in groupView.group.innerNodeGUIDs.ToList())
                {
                    if (graphView.graph.nodesPerGUID.TryGetValue(nodeGUID, out BaseNode node))
                    {
                        CreateNodeExistNode(node, foldout);
                        nodeGUIDs.Remove(nodeGUID);
                    }
                }

                if(foldout.childCount > 0)
                {
                    //用来比较大小
                    double value = groupView.GetPosition().x * 10 + groupView.GetPosition().y;
                    elements.Add((foldout, value));
                }
            }

            foreach(var nodeGUID in nodeGUIDs)
            {
                if (graphView.graph.nodesPerGUID.TryGetValue(nodeGUID, out BaseNode node))
                {
                    double value = node.position.position.x * 10 + node.position.position.y;
                    var elem = CreateNodeExistNode(node);
                    if(elem != null)
                        elements.Add((elem, value));
                }
            }

            int nodeIndex = 0;

            elements = elements.OrderBy(e => e.Item2).ToList();

            foreach (var ele in elements)
            {
                nodeIndex++;
                if (ele.Item1 is Foldout foldout)
                {
                    foldout.text = $"{nodeIndex}-{foldout.text}";
                }
                else if(ele.Item1 is Button btn)
                {
                    btn.text = $"{nodeIndex}-{btn.text}";
                }
                ele.Item1.style.opacity = 0.7f;
                treeView.Add(ele.Item1);
            }
        }

        VisualElement CreateNodeExistNode(BaseNode node, VisualElement root = null)
        {
            var configNode = node as IConfigBaseNode;
            var configName = configNode.GetConfigName();
            var configType = Utils.GetConfigNodeType(configName);
            var isDebug = node.debug && configType != 0;
            var debugTag = isDebug ? "●" : string.Empty;

            var searchText = searchField.value;
            var visibleStr = !node.isVisible ? "(隐藏)" : string.Empty;

            var searchName = node.GetNodeSearchName();
            var displayName = $"{debugTag}{visibleStr}{searchName}";
            if (string.IsNullOrEmpty(searchText) || displayName.ContainsFuzzy(searchText))
            {
                if (!isDebug)
                {
                    Button btn = null;
                    btn = Utils.CreateTextBtn(displayName, () =>
                    {
                        if (curSelectNodeEle != null)
                        {
                            curSelectNodeEle.OnSelectCilcked(false);
                        }
                        if (!graphView.nodeViewsPerNode.TryGetValue(node, out BaseNodeView nodeView))
                        {
                            return;
                        }

                        // 切换仅更新选择，重复点击聚焦节点，便于查看节点位置
                        var frameSelection = curSelectNodeEle != null && curSelectNodeEle == btn;
                        nodeView?.FocusSelect(frameSelection);
                        if (btn != null)
                        {
                            curSelectNodeEle = btn;
                            curSelectNodeEle.OnSelectCilcked();
                        }
                    });
                    btn.name = searchName;

                    if (root != null)
                        root.Add(btn);
                    return btn;
                }
                else
                {
                    var foldout = new Foldout();
                    foldout.name = searchName;
                    foldout.text = displayName;
                    foldout.style.color = Color.yellow;
                    foldout.value = false;
                    foldout.RegisterValueChangedCallback<bool>((open) => 
                    {
                        if (curSelectNodeEle != null)
                        {
                            curSelectNodeEle.OnSelectCilcked(false);
                        }
                        if (!graphView.nodeViewsPerNode.TryGetValue(node, out BaseNodeView nodeView))
                        {
                            return;
                        }

                        // 切换仅更新选择，重复点击聚焦节点，便于查看节点位置
                        var frameSelection = curSelectNodeEle != null && curSelectNodeEle == foldout;
                        nodeView?.FocusSelect(frameSelection);
                        if (foldout != null)
                        {
                            curSelectNodeEle = foldout;
                            curSelectNodeEle.OnSelectCilcked();
                        }
                    });

                    CreateNodeDebugParams(node, foldout);

                    if (root != null)
                        root.Add(foldout);
                    return foldout;
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 调试断点参数条件列表
        /// </summary>
        /// <param name="node"></param>
        /// <param name="root"></param>
        void CreateNodeDebugParams(BaseNode node, VisualElement root)
        {
            var paramsNode = node as IParamsNode;
            var annos = paramsNode.GetParamsAnnotation();
            for (int i = 0; i < annos.paramsAnn.Count; ++i)
            {
                TParamAnnotation anno = annos.paramsAnn.ExGet(i);
                if (anno != null)
                {
                    var paramItem = CreateNodeDebugParamBox(node, i + 1, anno.GetName());
                    paramItem.style.color = Color.green;
                    root.Add(paramItem);
                }
            }
            var outItem = CreateNodeDebugParamBox(node, - 1, "返回值:");
            outItem.style.color = Color.cyan;
            root.Add(outItem);

            var customItem = CreateNodeDebugParamBox(node, - 2, "自定义技能条件ID:");
            customItem.style.color = Color.cyan;
            root.Add(customItem);

        }

        VisualElement CreateNodeDebugParamBox(BaseNode node, int paramIndex, string paramName)
        {
            var configNode = node as IConfigBaseNode;
            var configName = configNode.GetConfigName();
            var configType = Utils.GetConfigNodeType(configName);
            var conifgId = configNode.GetConfigID();

            var paramBox = new Box();
            
            paramBox.style.flexDirection = FlexDirection.Row;   //布局方向改为 水平
            paramBox.style.alignItems = Align.Auto;
            var annoName = paramIndex > 0 ? $"参数{paramIndex}：{paramName}" : paramName;
            var itemName = new Label(annoName);
            //itemName.style.color = Color.cyan;
            itemName.style.width = 250;
            paramBox.Add(itemName);

            var bpData = node.GetBreakpointData(paramIndex);

            var label1 = new Label("条件：");
            label1.visible = bpData != null ? bpData.Enable : false;
            //label1.style.color = Color.white;
            label1.style.width = 35;
            paramBox.Add(label1);

            var opText = new TextField();//"判断符："
            opText.visible = bpData != null ? bpData.Enable : false;
            opText.style.width = 25;
            opText.value = bpData != null ? bpData.OpTypeToString() : "==";
            opText.RegisterValueChangedCallback((value) => 
            {
                opText.value = Regex.Replace(opText.value, @"[^!=<>]", "");
                var bpData = node.GetAndCretteBreakpointData(paramIndex);
                bpData.SetOpType(opText.value);
                var battle = AppFacade.BattleManager?.Battle;
                if (battle != null)
                    BattleWrapper.UnityEditorDebug_RefreshDebugCondition(configType, conifgId, bpData.ParamIndex, bpData.Enable, bpData.OpType, bpData.CValue);
            });
            paramBox.Add(opText);

            var valueText = new TextField();//"数值："
            valueText.visible = bpData != null ? bpData.Enable : false;
            valueText.style.width = 130;
            valueText.value = bpData != null ? bpData.CValue.ToString() : "0";
            valueText.RegisterValueChangedCallback((value) => 
            {
                valueText.value = Regex.Replace(valueText.value, @"[^0-9]", "");
                var bpData = node.GetAndCretteBreakpointData(paramIndex);
                bpData.CValue = int.Parse(valueText.value);
                var battle = AppFacade.BattleManager?.Battle;
                if (battle != null)
                    BattleWrapper.UnityEditorDebug_RefreshDebugCondition(configType, conifgId, bpData.ParamIndex, bpData.Enable, bpData.OpType, bpData.CValue);
            });
            paramBox.Add(valueText);

            var label2 = new Label("开启条件");
            //label2.style.color = Color.white;
            label2.style.width = 50;
            paramBox.Add(label2);
            var selBtn = new Toggle();
            selBtn.style.color = Color.cyan;
            selBtn.style.alignSelf = Align.FlexEnd;
            selBtn.style.width = 30;
            selBtn.value = bpData != null ? bpData.Enable : false;
            selBtn.RegisterValueChangedCallback((isOn) => 
            {
                label1.visible = selBtn.value;
                opText.visible = selBtn.value;
                valueText.visible = selBtn.value;
                var bpData = node.GetAndCretteBreakpointData(paramIndex);
                bpData.Enable = selBtn.value;
                var battle = AppFacade.BattleManager?.Battle;
                if (battle != null)
                    BattleWrapper.UnityEditorDebug_RefreshDebugCondition(configType, conifgId, bpData.ParamIndex, bpData.Enable, bpData.OpType, bpData.CValue);

            });
            paramBox.Add(selBtn);

            return paramBox;
        }
        #endregion

        #region 创建列表

        Type createNodeType = null;

        void CreateNodeTypeList()
        {
            var searchText = searchField.value;

            var nodeItems = new List<(string path, Type type)>();
            var pathSaves = GraphHelper.GetPathSaves(graphView.graph.GetType());
            Utils.PathFormat(ref pathSaves);
            var jsonPaths = Directory.GetFiles(pathSaves, $"*.json", SearchOption.AllDirectories);
            for (int i = 0, length = jsonPaths.Length; i < length; i++)
            {
                var jsonPath = jsonPaths[i];
                Utils.PathFormat(ref jsonPath);
                if (JsonGraphManager.Inst.IsTemplate(jsonPath))
                {
                    nodeItems.Add((jsonPath, null));
                }
            }

            nodeItems.AddRange(NodeProvider.GetNodeMenuEntries(graphView.graph).ToList());
            foreach (var nodeItem in nodeItems)
            {
                string templatePath = "模板列表";
                var path = nodeItem.path;
                if (JsonGraphManager.Inst.IsTemplate(nodeItem.path))
                {
                    if (JsonGraphManager.Inst.IsChildTemplate(nodeItem.path))
                    {
                        continue;
                    }

                    path = $"{templatePath}/{nodeItem.path.Substring(pathSaves.Length + 1)}";
                }
                string[] paths = path.Split('/');

                var displayName = paths[paths.Length - 1];
                VisualElement root = treeView;

                if (!string.IsNullOrEmpty(searchText) && !displayName.ContainsFuzzy(searchText))
                {
                    continue;
                }

                if (paths.Length > 1)
                {
                    string name = "";
                    for (int i = 0; i < paths.Length - 1; i++)
                    {
                        name = $"{name}{paths[i]}";
                        Foldout foldout = root.Q<Foldout>($"{name}");

                        if (foldout == null)
                        {
                            foldout = new Foldout();
                            foldout.name = name;
                            foldout.text = paths[i];
                            foldout.style.color = Color.red;
                            root.Add(foldout);
                            if(string.IsNullOrEmpty(searchText))
                                foldout.value = false;
                            if (i == 1)
                                foldout.style.left = i * 10;
                        }
                        root = foldout;
                    }
                }
                else
                {
                    Foldout foldout = root.Q<Foldout>($"其他配置");
                    if (foldout == null)
                    {
                        foldout = new Foldout();
                        foldout.name = $"其他配置";
                        foldout.text = $"其他配置";
                        foldout.style.color = Color.red;
                        root.Add(foldout);
                        if (string.IsNullOrEmpty(searchText))
                            foldout.value = false;
                    }
                    root = foldout;
                }

                Button btn = null;
                btn = Utils.CreateTextBtn(displayName);
                btn.name = nodeItem.path;
                var clickable = new CommonClickable(ClickCreateNodeEvent);
                btn.clickable = clickable;
                if (nodeItem.path.Contains($"{nodeItem.path}") 
                    && JsonGraphManager.Inst.IsTemplate(nodeItem.path))
                {
                    string baseText = "双击打开模板";
                    //var info = TemplateProvider.Get(graphView.graph.GetType(), nodeItem.path);
                    //if(info.Node != null && !string.IsNullOrEmpty(info.Node.Desc))
                    //{
                    //    baseText = $"{baseText} \n {info.Node.Desc}";
                    //}

                    btn.tooltip = baseText;
                }
                root.Add(btn);
            }
        }

        void ClickCreateNodeEvent(EventType eventType, Vector2 mousePos, VisualElement target)
        {
            string path = target.name;
            var pathSaves = GraphHelper.GetPathSaves(graphView.graph.GetType());
            //mousePos传过来是局部坐标，首先转换成世界坐标
            mousePos.y += (target.worldBound.y - worldBound.y);
            mousePos.x += (target.worldBound.x - worldBound.x);
            switch (eventType)
            {
                case EventType.MouseDown:
                    //模板技能节点
                    Type tempCreateNodeType = null;
                    dragCreateNodeBox.Q<Label>("NodeName").text = path;
                    if (JsonGraphManager.Inst.IsTemplate(path))
                    {
                        tempCreateNodeType = GraphHelper.TryGetTemplateNodeType(graphView.graph, path);

                        if (createNodeType == tempCreateNodeType)
                        {
                            if (DateTime.Now.Ticks - clickTime < int.MaxValue / 100)
                            {
                                var ret = GraphAssetCallbacks.OpenGraphWindow(path);

                                string info = !ret ? $"打开失败:{path}" : $"打开成功:{path}";
                                graphWindow.ShowNotification(info);
                            }

                            createNodeType = null;
                            break;
                        }
                    }
                    else
                    {
                        var nodeEntries = NodeProvider.GetNodeMenuEntries(graphView.graph);
                        tempCreateNodeType = nodeEntries.Where(n => n.path == path).First().type;
                    }

                    if (tempCreateNodeType == null)
                    {
                        this.graphWindow?.ShowNotification($"暂未支持，找不到节点类型:{path}");
                        break;
                    }

                    clickTime = DateTime.Now.Ticks;
                    createNodeType = tempCreateNodeType;
                    break;
                case EventType.MouseMove:
                    if (createNodeType == null) break;
                    mousePos = this.ChangeCoordinatesTo(graphView, mousePos);
                    dragCreateNodeBox.style.left = mousePos.x;
                    dragCreateNodeBox.style.top = mousePos.y;
                    dragCreateNodeBox.visible = true;
                    dragCreateNodeBox.BringToFront();
                    break;
                case EventType.MouseUp:
                    if (!dragCreateNodeBox.visible) break;
                    if (!this.BoundingBoxTest(dragCreateNodeBox) && createNodeType != null)
                    {
                        mousePos = this.ChangeCoordinatesTo(graphView.contentViewContainer, mousePos);
                        var node = BaseNode.CreateFromType(createNodeType, mousePos);
                        graphView.AddNode(node);

                        if (node is ITemplateReceiverNode templateNode)
                        {
                            templateNode.TemplateNodeData.RefreshGraphRef(path);
                        }
                    }

                    dragCreateNodeBox.visible = false;
                    createNodeType = null;
                    break;
            }
        }
        #endregion

        void CreateParamNodeList()
        {
            treeView.Clear();

            var extraParamNodeDic = new Dictionary<int, List<BaseNode>>(); //额外参数收集
            var skillTagNodeDic = new Dictionary<int, List<BaseNode>>(); //技能参数收集
            var nodeGUIDs = graphView.graph.nodesPerGUID.Keys.ToHashSet();
            List<TParamAnnotation> templateParams = null;
            foreach (var nodeGUID in nodeGUIDs)
            {
                if (graphView.graph.nodesPerGUID.TryGetValue(nodeGUID, out BaseNode node))
                {
                    var paramsNode = node as IParamsNode;
                    if (paramsNode == null)
                        continue;
                    var paramList = paramsNode.GetParamsList();
                    if (paramList == null || paramList.Count == 0)
                        continue;
                    if (node is ConfigBaseNode templateNode && templateNode.IsTemplate)
                    {
                        if (templateParams == null)
                        {
                            templateParams = templateNode.TemplateParams;
                        }
                        else
                        {
                            Log.Fatal($"当前不允许存在多个模板配置，请检查:{graphView.graph.path}");
                        }
                    }
                    foreach(var param in paramList)
                    {
                        if(param.ParamType == TParamType.TPT_EXTRA_PARAM)
                        {
                            var paramIndex = param.Value;
                            if (extraParamNodeDic.TryGetValue(paramIndex, out var extraNodeList))
                            {
                                extraNodeList.Add(node);
                            }
                            else
                            {
                                extraNodeList = new List<BaseNode>();
                                extraNodeList.Add(node);
                                extraParamNodeDic.Add(paramIndex, extraNodeList);
                            }
                        }
                    }

                    var skillTagConfigId = 0;
                    if (node is SkillEffectConfigNode skillEffectConfigNode)
                    {
                        var config = skillEffectConfigNode.Config;
                        if(config != null)
                        {
                            switch (config.SkillEffectType)
                            {
                                case TSkillEffectType.TSET_GET_SKILL_TAG_VALUE:
                                case TSkillEffectType.TSET_ADD_SKILL_TAG_VALUE:
                                case TSkillEffectType.TSET_MODIFY_SKILL_TAG_VALUE:
                                    skillTagConfigId = paramList[2].Value;
                                    break;
                                case TSkillEffectType.TSET_GET_AI_PARAM:
                                case TSkillEffectType.TSET_SET_AI_PARAM:
                                    skillTagConfigId = paramList[1].Value;
                                    break;
                            }
                        }
                    }
                    else if (node is SkillConditionConfigNode skillConditionConfigNode)
                    {
                        var config = skillConditionConfigNode.Config;
                        if (config != null)
                        {
                            switch (config.SkillConditionType)
                            {
                                case TSkillConditionType.TSCT_SKILLTAGS:
                                    skillTagConfigId = paramList[2].Value;
                                    break;
                            }
                        }
                    }
                    if (skillTagConfigId != 0)
                    {
                        if (skillTagNodeDic.TryGetValue(skillTagConfigId, out var tagNodeList))
                        {
                            tagNodeList.Add(node);
                        }
                        else
                        {
                            tagNodeList = new List<BaseNode>();
                            tagNodeList.Add(node);
                            skillTagNodeDic.Add(skillTagConfigId, tagNodeList);
                        }
                    }
                }
            }
            var extraFoldout = new Foldout();
            extraFoldout.name = "额外参数列表";
            extraFoldout.text = "额外参数列表";
            extraFoldout.style.color = Color.white;
            extraFoldout.value = true;
            foreach (var extraParamData in extraParamNodeDic)
            {
                var extraIndex = extraParamData.Key;
                var desc = templateParams?.GetAt(extraIndex - 1)?.Name ?? "空描述";
                var foldout = new Foldout();
                foldout.name = $"ExtraIndex{extraIndex}";
                foldout.text = $"{extraIndex}-{desc}";
                foldout.value = false;
                foldout.style.left = 10;
                foldout.style.width = 300;
                var extraNodeList = extraParamData.Value;
                foreach (var node in extraNodeList)
                {
                    CreateSkillExtraNode(node, foldout, extraIndex);
                }
                extraFoldout.Add(foldout);
            }
            treeView.Add(extraFoldout);


            var tagFoldout = new Foldout();
            tagFoldout.name = "技能参数列表";
            tagFoldout.text = "技能参数列表";
            tagFoldout.style.color = Color.white;
            tagFoldout.value = true;
            foreach(var skillTagData in skillTagNodeDic)
            {
                var skillTagConfigId = skillTagData.Key;
                var skillTagConfig = DesignTable.GetTableCell<SkillTagsConfig>(skillTagConfigId);
                var foldout = new Foldout();
                foldout.name = $"skillTag{skillTagConfigId}";
                foldout.text = $"{skillTagConfigId}-{skillTagConfig?.Desc ?? "空描述"}";
                foldout.value = false;
                foldout.style.left = 10;
                foldout.style.width = 300;
                var skillTagNodeList = skillTagData.Value;
                foreach(var node in skillTagNodeList)
                {
                    CreateSkillTagNode(node, foldout);
                }
                tagFoldout.Add(foldout);
            }
            treeView.Add(tagFoldout);

            toolBtn.text = $"额外参数：（{extraParamNodeDic.Count}）|技能参数：（{skillTagNodeDic.Count}）";
        }

        VisualElement CreateSkillTagNode(BaseNode node, VisualElement root)
        {
            string vis = !node.isVisible ? "(隐藏)" : string.Empty;

            string name = node.GetCustomName();

            var displayName = $"{vis}{name}";
           
            Button btn = null;
            btn = Utils.CreateTextBtn(displayName, () =>
            {
                if (curSelectNodeEle != null)
                {
                    curSelectNodeEle.OnSelectCilcked(false);
                }
                if (!graphView.nodeViewsPerNode.TryGetValue(node, out BaseNodeView nodeView))
                {
                    return;
                }

                // 切换仅更新选择，重复点击聚焦节点，便于查看节点位置
                var frameSelection = curSelectNodeEle != null && curSelectNodeEle == btn;
                nodeView?.FocusSelect(frameSelection);
                if (btn != null)
                {
                    curSelectNodeEle = btn;
                    curSelectNodeEle.OnSelectCilcked();
                }
            });
            btn.name = node.GetCustomName();
            btn.style.width = 400;

            root.Add(btn);
            return btn;
               
        }

        VisualElement CreateSkillExtraNode(BaseNode node, VisualElement root, int extraIndex)
        {
            string vis = !node.isVisible ? "(隐藏)" : string.Empty;

            string name = node.GetCustomName();
            var paramsNode = node as IParamsNode;
            var paramList = paramsNode.GetParamsList();
            var annos = paramsNode.GetParamsAnnotation();
            var anIndex = paramList.FindIndex((param) => { return param.ParamType == TParamType.TPT_EXTRA_PARAM && param.Value == extraIndex; });
            var paramName = (annos != null && annos.paramsAnn != null && anIndex >= 0 && anIndex < annos.paramsAnn.Count) ? annos.paramsAnn.ExGet(anIndex).GetName() : string.Empty;

            var displayName = $"{vis}{name} - {paramName}";

            Button btn = null;
            btn = Utils.CreateTextBtn(displayName, () =>
            {
                if (curSelectNodeEle != null)
                {
                    curSelectNodeEle.OnSelectCilcked(false);
                }
                if (!graphView.nodeViewsPerNode.TryGetValue(node, out BaseNodeView nodeView))
                {
                    return;
                }

                // 切换仅更新选择，重复点击聚焦节点，便于查看节点位置
                var frameSelection = curSelectNodeEle != null && curSelectNodeEle == btn;
                nodeView?.FocusSelect(frameSelection);
                if (btn != null)
                {
                    curSelectNodeEle = btn;
                    curSelectNodeEle.OnSelectCilcked();
                }
            });
            btn.name = node.GetCustomName();
            btn.style.width = 400;

            root.Add(btn);
            return btn;

        }

        void AddConsoleLog(string log,BaseNode logNode)
        {
            if (currentTab != TabType.ConsoleList)
                return;

            var allSelectNodeCount = graphView.selection.Count;
            var searchText = searchField.value;

            if (!string.IsNullOrEmpty(searchText) && !log.ContainsFuzzy(searchText))
            {
                return;
            }

            for (int i = allSelectNodeCount - 1; i >= 0; i--)
            {
                if (!(graphView.selection[i] is BaseNodeView baseNodeView) || baseNodeView.nodeTarget != logNode)
                {
                    continue;
                }

                Button btn = Utils.CreateTextBtn(log);
                if (allSelectNodeCount > 1)
                {
                    Foldout foldout = treeView.Q<Foldout>(logNode.GetCustomName());
                    if(foldout == null)
                    {
                        foldout = new Foldout();
                        foldout.name = logNode.GetCustomName();
                        foldout.text = logNode.GetCustomName();
                        treeView.Add(foldout);
                    }

                    if (foldout.childCount >= ConfigGraphWindow.LOGMAXCOUNT)
                    {
                        foldout.Clear();
                    }

                    foldout.Insert(0,btn);
                }
                else
                {
                    if(treeView.childCount >= ConfigGraphWindow.LOGMAXCOUNT)
                    {
                        treeView.Clear();
                    }

                    treeView.Insert(0, btn);
                }
            }
        }

        void CreateConsolelist()
        {
            var allSelectNodeCount = graphView.selection.Count;

            treeView.Clear();
            var searchText = searchField.value;
            for (int i = allSelectNodeCount - 1; i >= 0; i--)
            {
                if (!(graphView.selection[i] is BaseNodeView baseNodeView))
                {
                    continue;
                }

                var node = baseNodeView.nodeTarget;
                var displayName = $"{i}-{node.GetCustomName()}";

                if(!graphWindow.allConsoleConfigIdInfoDic.TryGetValue(node.GUID,out var logs))
                {
                    continue;
                }

                foreach (var info in logs)
                {
                    if (!string.IsNullOrEmpty(searchText) && !info.ContainsFuzzy(searchText))
                    {
                        continue;
                    }

                    Button btn = Utils.CreateTextBtn(info);

                    if (allSelectNodeCount > 1)
                    {
                        Foldout foldout = treeView.Q<Foldout>(node.GetCustomName());
                        if (foldout == null)
                        {
                            foldout = new Foldout();
                            foldout.name = node.GetCustomName();
                            foldout.text = node.GetCustomName();
                            treeView.Add(foldout);
                        }
                        foldout.Insert(0, btn);
                    }
                    else
                    {
                        treeView.Insert(0, btn);
                    }
                }
            }
        }

        void AddToFatalList(string fatal,object obj)
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle != null && currentTab == TabType.ConsoleList)
            {
                graphWindow.ShowNotification("出现严重异常，请联系程序", 1, LogLevel.None);
                OnRefresh();
            }
            else
            {
                try
                {
                    graphWindow.ShowNotification("出现严重异常，请联系程序", 1, LogLevel.None);
                    this.ChangeToolBarView((int)TabType.FatalList);
                    if (!this.pinnedElement.opened)
                    {
                        graphView.ToggleView<ConfigPinnedView>();
                    }
                }
                catch (Exception ex)
                {
                    Log.Exception(ex);
                }
            }
        }

        void CreateFatalList()
        {
            treeView.Clear();
            List<string> procedureFatals = Log.Inst.fatalTroubles.ToList();
            if(procedureFatals.Count > 0)
            {
                Foldout foldout = new Foldout();
                foldout.name = "Procedure";
                treeView.Add(foldout);
                foldout.style.color = new StyleColor(new Color(255, 0, 0));
                foldout.text = "程序解决（点击复制）";
                for (int i = procedureFatals.Count - 1; i >= 0; i--)
                {
                    CreateFatalBtn(procedureFatals[i], foldout);
                }
            }

            List<(string,object)> plannerFatals = graphView.graph.fatalInfos.ToList();
            if (plannerFatals.Count > 0)
            {
                Foldout foldout = new Foldout();
                foldout.name = "Planner";
                treeView.Add(foldout);
                foldout.style.color = new StyleColor(new Color(255, 0, 0));
                foldout.text = "策划解决（点击定位）";
                for (int i = plannerFatals.Count - 1; i >= 0; i--)
                {
                    CreateFatalBtn(plannerFatals[i].Item1, foldout, plannerFatals[i].Item2);
                }
            }
        }

        void CreateFatalBtn(string fatal,VisualElement root,object obj = null)
        {
            string searchText = searchField.value;
            string[] fatalChilds = fatal.Split('\n');
            if (fatalChilds.Length <= 0)
            {
                return;
            }

            string fatalHead = fatalChilds[0];

            if (!string.IsNullOrEmpty(searchText) && !fatalHead.ContainsFuzzy(searchText))
            {
                return;
            }

            Button fatalBtn = null;
            if(obj == null)
            {
                fatalBtn = Utils.CreateTextBtn(fatalHead, () =>
                {
                    GUIUtility.systemCopyBuffer = fatalBtn.tooltip;
                    graphWindow.ShowNotification($"复制该问题成功，快去让程序解决");
                });
            }
            else
            {
                fatalBtn = Utils.CreateTextBtn(fatalHead, () =>
                {
                    if (obj is BaseNodeView baseNodeView 
                    ||(obj is BaseNode baseNode && graphView.nodeViewsPerNode.TryGetValue(baseNode, out baseNodeView)))
                    {
                        baseNodeView?.FocusSelect();
                    }
                });
            }

            fatalBtn.text = fatalHead;
            fatalBtn.tooltip = fatal;
            fatalBtn.style.color = new StyleColor(Color.red);
            root.Add(fatalBtn);
        }
        #endregion

    }
}

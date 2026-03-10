using Apps.Scheme;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public partial class ConfigGraphView : UniversalGraphView
    {
        ConfigGraphWindow configGraphWindow;
        static HashSet<string> checkRefInfos = new HashSet<string>();
        static HashSet<string> checkedFileNames = new HashSet<string>();

        protected override void CreateNodeMenuWindow()
        {
            base.CreateNodeMenuWindow();
        }
        protected override BaseEdgeConnectorListener CreateEdgeConnectorListener() => new ConfigEdgeConnectorListener(this);
        public ConfigGraphWindow GetConfigGraphWindow()
        {
            return configGraphWindow;
        }
        protected override void BuildGroupContextualMenu(ContextualMenuPopulateEvent evt, int menuPosition = -1)
        {
            base.BuildGroupContextualMenu(evt, menuPosition);

            if (evt.target is GraphElement graphElement && graphElement.IsGroupable() && groupViews.Exists(x => x.ContainsElement(graphElement)))
                evt.menu.InsertAction(menuPosition, "Break From Group", (e) => this.RemoveNodeToGroup(graphElement), DropdownMenuAction.AlwaysEnabled);
        }

        SearchNodeMenuWindow searchNodeMenu;
        private static List<ISelectable> copySelectDatas= new List<ISelectable>();
        public ConfigGraphView(EditorWindow window) : base(window)
        {
            configGraphWindow = window as ConfigGraphWindow;
            searchNodeMenu = ScriptableObject.CreateInstance<SearchNodeMenuWindow>();
            searchNodeMenu.Initialize(this, window);
        }
        protected override void KeyDownCallback(KeyDownEvent e)
        {
            base.KeyDownCallback(e);

            // 打印记录快捷键操作
            var graphName = graph?.name ?? "空";
            if (e.keyCode == KeyCode.F && e.ctrlKey)
            {
                OpenSearchNodeWindow();
                e.StopPropagation();
                Log.Debug($"{graphName} : 操作_查找_Ctrl+F");
            }
            else if (e.keyCode == KeyCode.Z && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_回退_Ctrl+Z");
                if(UnReDoInfoList != null && UnReDoInfoList.Count > 0 && CurrentUnReDoIndex > 0)
                {
                    var currUnReDo = UnReDoInfoList[CurrentUnReDoIndex - 1];
                    Log.Debug($"回退-----index:{CurrentUnReDoIndex} type:{currUnReDo.ControlType} id:{currUnReDo.NodeID}");
                }
                OnNodeUnDo();
                //if (EditorUtility.DisplayDialog($"撤回前保存文件", "撤回可能存在问题，撤回操作前保存一下，测试没问题后再取消提示", "保存", "不保存"))
                //{
                 //   configGraphWindow.IgnoreFlagSave();
                //}
                //EditorUtility.DisplayDialog("警告", "撤销操作和回滚监听操作产生冲突，暂时屏蔽！，修复之后再使用", "好的");
                //e.StopPropagation();
            }
            else if (e.keyCode == KeyCode.C && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_复制_Ctrl+C");

            }
            else if (e.keyCode == KeyCode.V && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_粘贴_Ctrl+V");
                whenPasteMousePos = (e.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, e.originalMousePosition);
            }
            else if (e.keyCode == KeyCode.D && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_复制粘贴_Ctrl+D");
            }
            else if (e.keyCode == KeyCode.S && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_保存_Ctrl+S");
            }
            else if (e.keyCode == KeyCode.R && e.ctrlKey)
            {
                if(configGraphWindow.IsPauseGame)
                {
                    configGraphWindow.NodeDebugContinue();
                }
                else
                {
                    configGraphWindow.IsPauseGame = true;
                }
                string info = configGraphWindow.IsPauseGame ? "暂停游戏" : "继续运行";
                Log.Debug($"{graphName} : {info}");
            }
            else if (e.keyCode == KeyCode.F5)
            {
                configGraphWindow.NodeDebugContinue();
            }
            else if (e.keyCode == KeyCode.F9)
            {
                configGraphWindow.NodeDebugStateSwitch();
            }
            else if (e.keyCode == KeyCode.F10)
            {
                configGraphWindow.NodeDebugRunNext();
            }
            else if (e.keyCode == KeyCode.W && e.ctrlKey)
            {
                Log.Debug($"{graphName} : 操作_关闭窗口_Ctrl+W");
                schedule.Execute(() =>
                {
                    configGraphWindow.Close();
                }).ExecuteLater(50);
            }

            keyDownCallbackEvent?.Invoke(e);
        }

        protected override string SerializeGraphElementsCallback(IEnumerable<GraphElement> elements)
        {
            List<GraphElement> addElems = new List<GraphElement>();
            foreach (var elem in elements)
            {
                addElems.Add(elem);

                if(!(elem is BaseNodeView nodeView))
                {
                    continue;
                }

                if (nodeView.nodeTarget.isHideChildNodes)
                {
                    //获取所有子节点
                    var childNodes = new List<BaseNode>();
                    nodeView.nodeTarget.GetChildNodesRecursive(childNodes);
                    foreach(var childNode in childNodes)
                    {
                        if (!nodeViewsPerNode.TryGetValue(childNode, out var childNodeView))
                        {
                            continue;
                        }

                        if (!childNode.isVisible && childNode.hideCounter > 0)
                        {
                            addElems.Add(childNodeView);
                        }

                        foreach (var outputPort in childNode.outputPorts)
                        {
                            foreach (var edge in edgeViews)
                            {
                                if (edge.serializedEdge.outputNode == childNode)
                                {
                                    addElems.Add(edge);
                                }
                            }
                        }
                    }
                }
            }

            return base.SerializeGraphElementsCallback(addElems.AsEnumerable());
        }

        //WWYTODO可以使用API实现 GetBoundingBox()
        private Vector2 CalElementsRelativeCenterPos<T>(List<T> elements) where T : GraphElement
        {
            if (elements == default)
            {
                return default;
            }

            Vector2 center = Vector2.zero;
            //左右上下
            Vector4 vec4 = new Vector4(float.MaxValue, float.MinValue,float.MinValue, float.MaxValue);
            for (int i = elements.Count - 1; i >= 0; i--)
            {
                T element = elements[i];
                Rect rect = default;
                switch (element)
                {
                    case BaseNodeView baseNodeView:
                        rect = baseNodeView.nodeTarget.position;
                        break;
                    case StickyNoteView stickyNoteView:
                        rect = stickyNoteView.note.position;
                        break;
                }

                if(rect == default)
                {
                    continue;
                }

                Vector2 pos = rect.position;
                float left = pos.x - rect.width / 2;
                float right = pos.x + rect.width / 2;

                float top = pos.y + rect.height / 2;
                float bottom = pos.y - rect.height / 2;


                vec4.x = Mathf.Min(left, vec4.x);
                vec4.y = Mathf.Max(right, vec4.y);
                vec4.z = Mathf.Max(top, vec4.z);
                vec4.w = Mathf.Min(bottom, vec4.w);
            }

            center.x = (vec4.x + vec4.y) / 2;
            center.y = (vec4.z + vec4.w) / 2;
            return center;
        }
        private void AllSetMousePosition<T>(List<T> elementLst) where T : GraphElement
        {
            
            if (whenPasteMousePos == Vector2.zero || elementLst == default)
            {
                return;
            }
            initializing = true;
            Vector2 oldCenter = CalElementsRelativeCenterPos(elementLst);
            Vector2 offset = whenPasteMousePos - oldCenter;

            for (int i = elementLst.Count - 1; i >= 0; i--)
            {
                T addElement = elementLst[i];
                Rect rect = default;
                switch (addElement)
                {
                    case BaseNodeView baseNodeView:
                        {
                            bool isShow = baseNodeView.nodeTarget.hideCounter <= 0;
                            ShowOrHideNodeOutputEdges(baseNodeView.nodeTarget, isShow);
                            if (baseNodeView.nodeTarget.isHideChildNodes)
                            {
                                baseNodeView.ShowOrHideNodeInfoLabel();
                                ShowOrHideNodeOutputEdges(baseNodeView.nodeTarget, false);
                            }

                            rect = baseNodeView.nodeTarget.position;
                            break;
                        }
                    case StickyNoteView stickyNoteView:
                        {
                            rect = stickyNoteView.note.position;
                            break;
                        }
                }

                if (rect == default)
                {
                    continue;
                }

                rect.position += offset;
                addElement.SetPosition(rect);
            }
            initializing = false;
        }

        private Vector2 whenPasteMousePos = Vector2.zero;
        protected override void UnserializeAndPasteCallback(string operationName, string serializedData)
        {
            int oldElementsCount = this.graphElements.Count();

            base.UnserializeAndPasteCallback(operationName, serializedData);

            if (oldElementsCount != this.graphElements.Count())
            {
                var elements = this.graphElements.ToList();
                var addElements = elements.GetAddRangeByOld(oldElementsCount);
                AllSetMousePosition(addElements);
            }
            whenPasteMousePos = Vector2.zero;
        }
        protected override void MouseDownCallback(MouseDownEvent e)
        {
            if (e.button == 0)
            {
                // 遍历查找父节点，可能存在选中节点内其他UI情况
                BaseNodeView nodeView = null;
                VisualElement mouseTarget = e.target as VisualElement;
                while (mouseTarget != null)
                {
                    nodeView = mouseTarget as BaseNodeView;
                    if (nodeView != null)
                    {
                        break;
                    }
                    else
                    {
                        mouseTarget = mouseTarget.parent;
                    }
                }
                if (nodeView != null)
                {
                    if (e.clickCount > 1)
                    {
                        // 获取引用的节点
                        if (nodeView.nodeTarget is RefConfigBaseNode)
                        {
                            var refNode = nodeView.nodeTarget.GetReferenceNode();
                            if (refNode != default && nodeViewsPerNode.TryGetValue(refNode, out var refNodeView))
                            {
                                refNodeView.selected = true;
                                selection.Add(refNodeView);
                            }
                        }
                        // 双击选择连续的子节点
                        else
                        {
                            var childNodes = new List<BaseNode>();
                            nodeView.nodeTarget.GetChildNodesRecursive(childNodes);
                            foreach (var childNode in childNodes)
                            {
                                nodeViewsPerNode.TryGetValue(childNode, out var childNodeView);
                                childNodeView.selected = true;
                                selection.Add(childNodeView);
                            }
                        }
                    }
                }
            }

            mouseCallBackEvent?.Invoke(EventType.MouseDown, e);
            base.MouseDownCallback(e);
        }
        protected override void MouseUpCallback(MouseUpEvent e)
        {
            base.MouseUpCallback(e);
            mouseCallBackEvent?.Invoke(EventType.MouseUp, e);
        }
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            BuildConfigNodeMenu(evt);
        }
        protected override void BuildHelpContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildHelpContextualMenu(evt);
            evt.menu.AppendAction("帮助文档", e =>
            {
                NodeEditorWindow.ShowHelp();
            });
        }
        // 节点相关选项
        protected void BuildConfigNodeMenu(ContextualMenuPopulateEvent evt)
        {
            // 添加到前面
            var menuPosition = 0;

            var nodeView = evt.target as BaseNodeView;
            var nodeTarget = nodeView?.nodeTarget;
            if (nodeTarget is IRefConfigBaseNode refNode)
            {
                var isTemplateNode = nodeTarget is ITemplateReceiverNode;
                var info = isTemplateNode ? "打开模板文件" : "定位引用节点";
                evt.menu.InsertAction(menuPosition++, info, (e) =>
                {
                    JsonGraphManager.Inst.TryOpenGraphWithProgressBar(refNode.RefConfigName, refNode.RefConfigID);
                });
            }
            else if (nodeTarget is TSET_CHANGE_AI changeAINode)
            {
                evt.menu.InsertAction(menuPosition++, "打开切换AI", (e) =>
                {
                    JsonGraphManager.Inst.TryOpenGraphWithProgressBar("AITaskNodeConfig", changeAINode.Config.Params[1].Value);
                });
            }
            if (nodeTarget is IConfigBaseNode configNode)
            {
                evt.menu.InsertAction(menuPosition++, "创建引用节点", (e) =>
                {
                    var refNode = BaseNode.CreateFromType<RefConfigBaseNode>(nodeView.GetPosition().position + new UnityEngine.Vector2(nodeView.GetPosition().width + 50, 0));
                    refNode.CreateNodeByTableName(configNode.GetConfigName(), configNode.GetConfigID());
                    nodeView.owner.AddNode(refNode);
                });
            }

            evt.menu.InsertAction(menuPosition++, "查找节点Ctrl+F", (e) =>
            {
                OpenSearchNodeWindow();
            }, DropdownMenuAction.AlwaysEnabled);

            evt.menu.InsertSeparator(null, menuPosition++);
        }

        protected override void BuildSelectAssetContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("定位文件", (e) => configGraphWindow?.PingObject(), DropdownMenuAction.AlwaysEnabled);
            base.BuildSelectAssetContextualMenu(evt);

            if (evt.target is BaseNodeView baseNodeView && selection.Count == 1)
            {
                evt.menu.AppendAction("节点自动排版", (e) =>
                {
                    NodeAutoLayouter.Layout(this.Init(selection[0]));
                    //AutoFormat();
                }, DropdownMenuAction.AlwaysEnabled);
            }

            if (selection.Count > 1)
            {
                evt.menu.AppendAction("对齐/左对齐", (e) =>
                {
                    Align(NodeAlignType.Left);
                }, DropdownMenuAction.AlwaysEnabled);

                evt.menu.AppendAction("对齐/右对齐", (e) =>
                {
                    Align(NodeAlignType.Right);
                }, DropdownMenuAction.AlwaysEnabled);

                evt.menu.AppendAction("对齐/上对齐", (e) =>
                {
                    Align(NodeAlignType.Top);
                }, DropdownMenuAction.AlwaysEnabled);

                evt.menu.AppendAction("对齐/下对齐", (e) =>
                {
                    Align(NodeAlignType.Bottom);
                }, DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void OpenSearchNodeWindow()
        {
            var windowPosition = configGraphWindow.position;
            var position = windowPosition.position + windowPosition.size / 2;
            var width = configGraphWindow.position.size.x * 0.7f;
            SearchWindow.Open(new SearchWindowContext(position, width), searchNodeMenu);
            // TODO 缓存搜索
        }
        public virtual IEnumerable<(string name, BaseNodeView node)> FilterSearchNodeMenuEntries()
        {
            foreach (var nodeView in nodeViews)
            {
                var nodeSearchName = nodeView.nodeTarget.GetNodeSearchName();
                yield return (nodeSearchName, nodeView);
            }
        }

        /// <summary>
        /// 连线创建节点入口
        /// </summary>
        /// <param name="searchTreeEntry"></param>
        /// <param name="context"></param>
        /// <param name="window"></param>
        /// <param name="inputPortView"></param>
        /// <param name="outputPortView"></param>
        /// <returns></returns>
        public override bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context, EditorWindow window, PortView inputPortView, PortView outputPortView)
        {
            // window to graph position
            var windowRoot = window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - window.position.position);
            var graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);

            var nodeType = searchTreeEntry.userData is Type ? (Type)searchTreeEntry.userData : ((NodeProvider.PortDescription)searchTreeEntry.userData).nodeType;

            RegisterCompleteObjectUndo("Added " + nodeType);

            var createNode = BaseNode.CreateFromType(nodeType, graphMousePosition);
            //刷新引用类节点
            if (outputPortView != null && createNode is RefConfigBaseNode refConfigBaseNode)
            {
                refConfigBaseNode.CreateNodeByTableName(outputPortView.portType.Name);
            }
            var view = AddNode(createNode);

            if (searchTreeEntry.userData is NodeProvider.PortDescription desc)
            {
                var targetPort = view.GetPortViewFromFieldName(desc.portFieldName, desc.portIdentifier);
                if (inputPortView == null)
                    Connect(targetPort, outputPortView);
                else
                    Connect(inputPortView, targetPort);
            }
            return true;
        }

        public override void RemoveNode(BaseNode node)
        {
            //WWYTODO先移除隐藏的子节点，可能会有无效递归，可以做优化
            if (node.isHideChildNodes)
            {
                //获取所有子节点
                var childNodes = new List<BaseNode>();
                node.GetChildNodesRecursive(childNodes);

                for (var i = childNodes.Count - 1; i >= 0; i--)
                {
                    RemoveNode(childNodes[i]);
                }
            }

            base.RemoveNode(node);
        }

        protected override GraphViewChange GraphViewChangedCallback(GraphViewChange changes)
        {
            if (changes.elementsToRemove != null)
            {
                checkRefInfos.Clear();
                checkedFileNames.Clear();
                // 检查节点是否可以被删除
                foreach (var elementToRemove in changes.elementsToRemove)
                {
                    // 如果是常规节点，检查下是否被引用（引用节点不检查）
                    if (elementToRemove is ConfigBaseNodeView configBaseNodeView && 
                        configBaseNodeView.nodeTarget is IConfigBaseNode configBaseNode &&
                        !(configBaseNode is IRefConfigBaseNode))
                    {
                        var configName = configBaseNode.GetConfigName();
                        var configID = configBaseNode.GetConfigID();

                        // 查找当前打开的窗口（针对未保存情况）
                        foreach (var win in ConfigGraphWindow.CacheOpenedWindows)
                        {
                            var graph = win.GetGraph();
                            if (graph)
                            {
                                foreach (var node in graph.nodes)
                                {
                                    if (node is IConfigBaseNode configNode && node is IRefConfigBaseNode &&
                                        configNode.GetID() == configID && configNode.GetConfigName() == configName)
                                    {
                                        var info = $"删除节点存在引用：{configName}_{configID}";
                                        checkRefInfos.Add(info);
                                        graph.AddGraphFatalInfo(info, configBaseNode);
                                    }
                                }
                                checkedFileNames.Add(graph.FileName);
                            }
                        }

                        if (JsonGraphManager.Inst.TryGetRefConfigIDNodeInfos(configName, configID, out var outNodeInfos))
                        {
                            bool haveUnChecked = false;
                            foreach (var nodeInfo in outNodeInfos)
                            {
                                if (nodeInfo.Owner!= null && !checkedFileNames.Contains(nodeInfo.Owner.graphName))
                                {
                                    haveUnChecked = true;
                                    break;
                                }
                            }
                            if (haveUnChecked)
                            {
                                var info = $"删除节点存在引用：{configName}_{configID}";
                                checkRefInfos.Add(info);
                                graph.AddGraphFatalInfo(info, configBaseNode);
                            }
                        }
                    }
                }
                if (checkRefInfos.Count > 0)
                {
                    // 如果存在引用节点，那么弹窗提示，并取消本次删除操作
                    if (!UnityEditor.EditorUtility.DisplayDialog("【注意】节点存在引用情况", string.Join("\n", checkRefInfos), "继续删除", "取消删除"))
                    {
                        changes.elementsToRemove.Clear();
                    }
                }
            }
            return base.GraphViewChangedCallback(changes);
        }

        //public void FocusNode(BaseNodeView nodeView)
        //{
        //    Focus();
        //    ClearSelection();
        //    // 按下shift键盘
        //    Utils.keybd_event(16, 0, 0, 0);
        //    Utils.keybd_event(16, 0, 2, 0);
        //    if (nodeView != null)
        //    {
        //        AddToSelection(nodeView);
        //        // 模拟下键盘按下F键
        //        Utils.keybd_event(70, 0, 0, 0);
        //        Utils.keybd_event(70, 0, 2, 0);
        //    }
        //    else
        //    {
        //        // 模拟下键盘按下A键
        //        Utils.keybd_event(65, 0, 0, 0);
        //        Utils.keybd_event(65, 0, 2, 0);
        //    }
        //    // 避免是中文输入，按下Enter键盘
        //    Utils.keybd_event(13, 0, 0, 0);
        //    Utils.keybd_event(13, 0, 2, 0);
        //    // 恢复shift键盘
        //    Utils.keybd_event(16, 0, 0, 0);
        //    Utils.keybd_event(16, 0, 2, 0);
        //}
    }
}

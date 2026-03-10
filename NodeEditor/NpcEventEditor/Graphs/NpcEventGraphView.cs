using GraphProcessor;
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor.NpcEventEditor
{
    public partial class NpcEventGraphView : ConfigGraphView
    {
        public int EventID
        {
            get
            {
                var eventNode = nodeViews.FirstOrDefault(n => n.nodeTarget is NpcEventConfigNode).nodeTarget as NpcEventConfigNode;
                return eventNode?.GetConfigID() ?? 0;
            }
        }

        public NpcEventGraphWindow npcEventGraphWindow;
        public NpcEventGraphView(EditorWindow window) : base(window)
        {
            npcEventGraphWindow = window as NpcEventGraphWindow;
        }

        protected override void CreateNodeMenuWindow()
        {
            createNodeMenu = ScriptableObject.CreateInstance<NpcEventCreateNodeMenuWindow>();
            createNodeMenu.Initialize(this, window);
        }

        protected override BaseEdgeConnectorListener CreateEdgeConnectorListener() => new NpcEventEdgeConnectorListener(this);

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
            var nodeType = searchTreeEntry.userData is Type ? (Type)searchTreeEntry.userData : ((NodeProvider.PortDescription)searchTreeEntry.userData).nodeType;

            var windowRoot = window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - window.position.position);
            var graphMousePosition = contentViewContainer.WorldToLocal(windowMousePosition);

            // window to graph position
            RegisterCompleteObjectUndo("Added " + nodeType);

            //BaseVirtualNode 虚拟节点
            if (nodeType.IsSubclassOf(typeof(BaseVirtualNode)))
            {
                var node = Activator.CreateInstance(nodeType) as BaseVirtualNode;

                //获取要拷贝的视图
                GraphHelper.ProcessGraph(node.JsonAssetPath, (copyGraph) =>
                {
                    //创建新的试图
                    if (window is NpcEventGraphWindow configGraphWindow)
                    {
                        var graphView = configGraphWindow.GetGraphView();
                        if (graphView != null)
                        {
                            GraphHelper.CopyGraph(graphView, copyGraph, graphMousePosition, outputPortView);
                        }
                    }
                });
            }
            //BaseNode 常规节点
            else
            {
                var method = nodeType.GetMethod("CreateNodeCustom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
                if (method != null)
                {
                    if (searchTreeEntry.userData is NodeProvider.PortDescription portDescription)
                    {
                        method.Invoke(nodeType, new object[] { this, portDescription, graphMousePosition, inputPortView, outputPortView });
                    }
                }
                else
                {
                    var createNode = BaseNode.CreateFromType(nodeType, graphMousePosition);
                    if (createNode is RefConfigBaseNode refConfigBaseNode)
                    {
                        refConfigBaseNode.CreateNodeByTableName(outputPortView.portType.Name);
                    }

                    var view = AddNode(createNode);

                    if (searchTreeEntry.userData is NodeProvider.PortDescription portDescription)
                    {
                        var targetPort = view.GetPortViewFromFieldName(portDescription.portFieldName, portDescription.portIdentifier);
                        if (inputPortView == null)
                            Connect(targetPort, outputPortView);
                        else
                            Connect(inputPortView, targetPort);
                    }
                }
            }

            return true;
        }
        
        /// <summary>
        /// 创建队列菜单
        /// </summary>
        /// <param name="evt"></param>
        protected override void BuildStackNodeContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 position = (evt.currentTarget as VisualElement).ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

            //to do 后续做成配置
            evt.menu.AppendAction("创建队列/对话", (e) => AddStackNode(new NpcTalkConfigStackNode(position, "对话")),
                DropdownMenuAction.AlwaysEnabled);

            evt.menu.AppendAction("创建队列/NPC行为", (e) => AddStackNode(new NpcEventActionConfigStackNode(position, "NPC行为")),
    DropdownMenuAction.AlwaysEnabled);

            evt.menu.AppendAction("创建队列/NPC行为组", (e) => AddStackNode(new NpcEventActionGroupConfigStackNode(position, "NPC行为组")),
DropdownMenuAction.AlwaysEnabled);

            evt.menu.AppendAction("创建队列/条件组", (e) => AddStackNode(new MapEventConditionConfigStackNode(position, "条件组")),
DropdownMenuAction.AlwaysEnabled);

            evt.menu.AppendAction("创建队列/公式组", (e) => AddStackNode(new MapEventFormulaConfigStackNode(position, "公式组")),
DropdownMenuAction.AlwaysEnabled);
        }

        public override BaseStackNodeView AddStackNodeView(BaseStackNode stackNode)
        {
            var viewType = StackNodeViewProvider.GetStackNodeCustomViewType(stackNode.GetType());
            if (viewType == null && stackNode.GetType().HasImplementedRawGeneric(typeof(ConfigStackNode<>)))
            {
                viewType = typeof(ConfigStackView);
            }
            else
            {
                viewType = typeof(BaseStackNodeView);
            }

            var stackView = Activator.CreateInstance(viewType, stackNode) as BaseStackNodeView;

            AddElement(stackView);
            stackNodeViews.Add(stackView);

            stackView.Initialize(this);

            return stackView;
        }

        /// <summary>
        /// 获取NpcEventActionConfigNode
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public NpcEventActionConfigNode GetNpcEventActionNode(int actionID)
        {
            var actionNode = nodeViews.FirstOrDefault((n) => {
                if(n.nodeTarget is NpcEventActionConfigNode node && node.Config.ID == actionID)
                {
                    return true;
                }
                return false;
            }).nodeTarget as NpcEventActionConfigNode;
            return actionNode;
        }
        
        protected override GraphViewChange GraphViewChangedCallback(GraphViewChange changes)
        {
            if (changes.elementsToRemove != null)
            {
                // 检查节点是否可以被删除
                IConfigBaseNode node = null;
                foreach (var elementToRemove in changes.elementsToRemove)
                {
                    var removeNodeView = elementToRemove as ConfigBaseNodeView;
                    if (removeNodeView == null)
                    {
                        continue;
                    }

                    var removeNode = removeNodeView.nodeTarget as IConfigBaseNode;
                    if (removeNode != null && removeNodeView.nodeTarget.HasBeRef)
                    {
                        node = removeNode;
                        break;
                    }
                }
                
                if (node != null)
                {
                    changes.elementsToRemove.Clear();
                    
                    if (EditorUtility.DisplayDialog("警告", $"节点 {node.GetConfigName()}_{node.GetConfigID()} 存在引用情况", "确定"))
                    {
                        // 如果存在引用节点，那么弹窗提示，并取消本次删除操作
                        if (node is ConfigBaseNode configNode)
                        {
                            configNode.OpenQuoteNode();
                        }

                        EditorUtility.DisplayDialog("提示", "已自动开启查看引用", "确定");
                    }
                }
            }
            
            return base.GraphViewChangedCallback(changes);
        }
    }
}

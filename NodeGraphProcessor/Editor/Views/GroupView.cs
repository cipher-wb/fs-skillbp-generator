using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphProcessor
{
    public class GroupView : UnityEditor.Experimental.GraphView.Group
	{
		public BaseGraphView	owner;
		public Group		    group;

        Label                   titleLabel;
        float titleLabelFontSizeOriginal = 15;

        public Label TitleLabel => titleLabel;

        ColorField              colorField;

        static readonly string         groupStyle = "GraphProcessorStyles/GroupView";

        [HideInInspector]
        public bool initializing = false;
        [HideInInspector]
        public bool Moving = false;

        public GroupView()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(groupStyle));
		}
		
		private static void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            if(evt.currentTarget is GroupView groupView)
            {
                //添加移除group
                evt.menu.AppendAction("Remove Group", (e) => {
                    groupView.owner.RemoveGroupView(groupView);
                }, DropdownMenuAction.AlwaysEnabled);
            }
        }

        public void Initialize(BaseGraphView graphView, Group block)
		{
            initializing = true;
            group = block;
			owner = graphView;

            title = block.title;
            SetPosition(block.position);

            this.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
			
            headerContainer.Q<TextField>().RegisterCallback<ChangeEvent<string>>(TitleChangedCallback);
            titleLabel = headerContainer.Q<Label>();
            titleLabel.style.textOverflow = TextOverflow.Ellipsis;
            // 内部文字存在优化？会导致粗细显示错乱
            //titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleLabel.AddToClassList("my-custom-title-bold");
            titleLabel.style.color = Color.white;
            titleLabel.style.opacity = 1;
            // 动态调整字体大小
            owner.viewTransformChanged -= OnViewScale;
            owner.viewTransformChanged += OnViewScale;

            colorField = new ColorField{ value = group.color, name = "headerColorPicker" };
            colorField.RegisterValueChangedCallback(e =>
            {
                UpdateGroupColor(e.newValue);
            });
            UpdateGroupColor(group.color);

            headerContainer.Add(colorField);

            InitializeInnerNodes();
            InitializeInnerStickyNotes();
            initializing = false;
        }

        void OnViewScale(GraphView graphView)
        {
            if (graphView == null) return;
            // 先按照缩放试图调整大小，可能导致group大小变化
            var scale = titleLabelFontSizeOriginal / graphView.viewTransform.scale.x;
            scale = Mathf.Max(titleLabelFontSizeOriginal, scale);
            //scale = Mathf.Clamp(scale, titleLabelFontSizeOriginal, titleLabelFontSizeOriginal * 2);
            titleLabel.style.fontSize = scale;
        }

        void InitializeInnerNodes()
        {
            foreach (var nodeGUID in group.innerNodeGUIDs.ToList())
            {
                if (!owner.graph.nodesPerGUID.TryGetValue(nodeGUID, out var node))
                {
                    Debug.LogWarning("Node GUID not found: " + nodeGUID);
                    group.innerNodeGUIDs.Remove(nodeGUID);
                    continue ;
                }

                var nodeView = owner.nodeViewsPerNode[node];
                AddElement(nodeView);
            }
        }

        void InitializeInnerStickyNotes()
        {
#if UNITY_2020_1_OR_NEWER
            foreach (var stickyNoteGUID in group.innerStickyNoteGUIDs.ToList())
            {
                if (!owner.graph.stickyNotesPerGUID.TryGetValue(stickyNoteGUID, out var stickyNote))
                {
                    Debug.LogWarning("StickyNote GUID not found: " + stickyNoteGUID);
                    group.innerStickyNoteGUIDs.Remove(stickyNoteGUID);
                    continue;
                }
                if (owner.stickyNoteViewsPerNote.TryGetValue(stickyNote, out var stickyNoteView))
                {
                    AddElement(stickyNoteView);
                }
                else
                {
                    Debug.LogWarning("StickyNote not found,  GUID: " + stickyNoteGUID);
                }
            }
#endif
        }

        protected override void OnElementsAdded(IEnumerable<GraphElement> elements)
        {
            List<GraphElement> addElements = new List<GraphElement>();
            foreach (var element in elements)
            {
                switch (element)
                {
                    case BaseNodeView node:
                        {
                            // Adding an element that is not a node currently supported
                            //添加了一个判断，修复了原作者的一个逻辑错误
                            if (node == null || !node.visible)
                            {
                                continue;
                            }

                            var guid = node.nodeTarget.GUID;
                            if (!group.innerNodeGUIDs.Contains(guid))
                            {
                                addElements.Add(node);
                                group.innerNodeGUIDs.Add(guid);
                            }
                            break;
                        }
                    case StickyNoteView stickyNoteView:
                        {
                            var guid = stickyNoteView.note.GUID;
                            if (!group.innerStickyNoteGUIDs.Contains(guid))
                            {
                                addElements.Add(stickyNoteView);
                                group.innerStickyNoteGUIDs.Add(guid);
                            }
                            break;
                        }
                }
            }
            base.OnElementsAdded(addElements);
        }

        protected override void OnElementsRemoved(IEnumerable<GraphElement> elements)
        {
            // Only remove the nodes when the group exists in the hierarchy
            if (parent != null)
            {
                foreach (var element in elements)
                {
                    switch (element)
                    {
                        case BaseNodeView nodeView:
                            {
                                group.innerNodeGUIDs.Remove(nodeView.nodeTarget.GUID);
                                break;
                            }
                        case StickyNoteView stickyNoteView:
                            {
                                group.innerStickyNoteGUIDs.Remove(stickyNoteView.note.GUID);
                                break;
                            }
                        default:
                            break;
                    }
                }
            }
            base.OnElementsRemoved(elements);
        }

        public void UpdateGroupColor(Color newColor)
        {
            if (!initializing)
            {
                if (newColor == group.color)
                    return;
                var oldJson = JsonUtility.ToJson(group);
                group.color = newColor;
                style.backgroundColor = newColor;
                var newJson = JsonUtility.ToJson(group);
                OnGroupInfoChanged(oldJson, newJson);
            }
            else
            {
                group.color = newColor;
                style.backgroundColor = newColor;
            }
        }

        void TitleChangedCallback(ChangeEvent< string > e)
        {
            if (group.title == e.newValue)
                return;
            if (!initializing)
            {
                var oldJson = JsonUtility.ToJson(group);
                group.title = e.newValue;
                var newJson = JsonUtility.ToJson(group);
                OnGroupInfoChanged(oldJson, newJson);
            }
            else
                group.title = e.newValue;
        }

		public override void SetPosition(Rect newPos)
		{
            if (group.position == newPos)
                return;
            Moving = true;
            base.SetPosition(newPos);
            if (!initializing)
            {
                var oldJson = JsonUtility.ToJson(group);
                group.position = newPos;
                var newJson = JsonUtility.ToJson(group);
                OnGroupInfoChanged(oldJson, newJson);
            }
            else
                group.position = newPos;
            Moving = false;
        }

        private void OnGroupInfoChanged(string oldJson, string newJson)
        {
            owner.AddUnReDoInfo(group.GUID, BaseGraphView.URDControlType.URDCT_GroupChange, oldJson, newJson);
        }
    }
}
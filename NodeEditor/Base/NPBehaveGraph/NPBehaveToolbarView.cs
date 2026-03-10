using GraphProcessor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NodeEditor
{
    public class NPBehaveToolbarView : UniversalToolbarView
    {
        public class BlackboardInspectorViewer : SerializedScriptableObject
        {
            public NP_BlackBoard Blackboard;
        }

        private static BlackboardInspectorViewer _BlackboardInspectorViewer;

        private static BlackboardInspectorViewer s_BlackboardInspectorViewer
        {
            get
            {
                if (_BlackboardInspectorViewer == null)
                {
                    _BlackboardInspectorViewer = ScriptableObject.CreateInstance<BlackboardInspectorViewer>();
                }

                return _BlackboardInspectorViewer;
            }
        }

        public NPBehaveToolbarView(BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) : base(graphView,
            miniMap, baseGraph)
        {
        }

        protected override void AddButtons()
        {
            base.AddButtons();

            //AddButton(new GUIContent("Blackboard", "打开Blackboard数据面板"),
            //    () =>
            //    {
            //        NPBehaveToolbarView.s_BlackboardInspectorViewer.Blackboard =
            //            (this.m_BaseGraph as NPBehaveGraph).NpBlackBoard;
            //        Selection.activeObject = s_BlackboardInspectorViewer;
            //    }, false);

            //AddButton(new GUIContent("TestNode", "打开数据面板"),
            //    () =>
            //    {
            //        foreach (var e in this.graphView.selection)
            //        {
            //            if (e is SkillConfigNodeView v && this.graphView.Contains(v) && v.nodeTarget.needsInspector)
            //            {
            //                Selection.activeObject = this.graphView.nodeInspector;
            //                break;
            //            }
            //        }
            //    }, false);
        }
    }
}
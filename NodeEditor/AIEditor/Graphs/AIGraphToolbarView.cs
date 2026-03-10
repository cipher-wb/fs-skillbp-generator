using GraphProcessor;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor.AIEditor
{
    public class AIGraphToolbarView : ConfigGraphToolbarView
    {
        AIGraphWindow aiGraphWindow;
        public AIGraphToolbarView(AIGraphWindow graphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) 
            : base(graphWindow, graphView, miniMap, baseGraph)
        {
            aiGraphWindow = graphWindow;
        }
    }
}

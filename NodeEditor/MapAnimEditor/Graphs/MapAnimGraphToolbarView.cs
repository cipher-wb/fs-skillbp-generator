using GraphProcessor;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor.MapAnimEditor
{
    public class MapAnimGraphToolbarView : ConfigGraphToolbarView
    {
        MapAnimGraphWindow aiGraphWindow;
        public MapAnimGraphToolbarView(MapAnimGraphWindow graphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) 
            : base(graphWindow, graphView, miniMap, baseGraph)
        {
            aiGraphWindow = graphWindow;
        }
    }
}

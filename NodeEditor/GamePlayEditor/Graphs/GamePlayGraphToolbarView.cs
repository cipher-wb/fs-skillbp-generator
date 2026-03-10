using GraphProcessor;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor.GamePlayEditor
{
    public class GamePlayGraphToolbarView : ConfigGraphToolbarView
    {
        GamePlayGraphWindow gamePlayGraphWindow;
        public GamePlayGraphToolbarView(GamePlayGraphWindow graphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) 
            : base(graphWindow, graphView, miniMap, baseGraph)
        {
            gamePlayGraphWindow = graphWindow;
        }
    }
}

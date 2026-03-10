using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor;

namespace GraphProcessor
{
    public class NpcEventEdgeConnectorListener : BaseEdgeConnectorListener
    {
        public NpcEventEdgeConnectorListener(BaseGraphView graphView):base(graphView)
        {

        }

        public override void OnDropOutsidePort(Edge edge, Vector2 position)
        {
			this.graphView.RegisterCompleteObjectUndo("Disconnect edge");

			//If the edge was already existing, remove it
			if (!edge.isGhostEdge)
				graphView.Disconnect(edge as EdgeView);

            // when on of the port is null, then the edge was created and dropped outside of a port
            if (edge.input == null || edge.output == null)
                ShowNodeCreationMenuFromEdge(edge as EdgeView, position);
        }

        public override void OnDrop(GraphView graphView, Edge edge)
        {
            base.OnDrop(graphView, edge);
        }

        void ShowNodeCreationMenuFromEdge(EdgeView edgeView, Vector2 position)
        {
            if (edgeNodeCreateMenuWindow == null)
                edgeNodeCreateMenuWindow = ScriptableObject.CreateInstance< NpcEventCreateNodeMenuWindow >();

            edgeNodeCreateMenuWindow.Initialize(graphView, EditorWindow.focusedWindow, edgeView);
			SearchWindow.Open(new SearchWindowContext(position + EditorWindow.focusedWindow.position.position), edgeNodeCreateMenuWindow);
        }
    }
}
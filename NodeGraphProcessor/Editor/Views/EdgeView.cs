using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;

namespace GraphProcessor
{
	public partial class EdgeView : Edge
	{
		public bool					isConnected = false;

		public SerializableEdge		serializedEdge { get { return userData as SerializableEdge; } }

		readonly string				edgeStyle = "GraphProcessorStyles/EdgeView";

		protected BaseGraphView		owner => ((input ?? output) as PortView).owner.owner;

		public Vector2[] GetPointsAndTangents => PointsAndTangents;

		public List<VisualElement> EdgeFlowPointVisualElements;
		
		public List<float> FlowPointProgress = new List<float>();

        public EdgeView() : base()
		{
			styleSheets.Add(Resources.Load<StyleSheet>(edgeStyle));
			RegisterCallback<MouseDownEvent>(OnMouseDown);

            InitFlowImage();
        }

        public override void OnPortChanged(bool isInput)
		{
			base.OnPortChanged(isInput);
			UpdateEdgeSize();
        }

		public void UpdateEdgeSize()
		{
			if (input == null && output == null)
				return;

			PortData inputPortData = (input as PortView)?.portData;
			PortData outputPortData = (output as PortView)?.portData;

			for (int i = 1; i < 20; i++)
				RemoveFromClassList($"edge_{i}");
			int maxPortSize = Mathf.Max(inputPortData?.sizeInPixel ?? 0, outputPortData?.sizeInPixel ?? 0);
			if (maxPortSize > 0)
				AddToClassList($"edge_{Mathf.Max(1, maxPortSize - 6)}");
		}

		protected override void OnCustomStyleResolved(ICustomStyle styles)
		{
			base.OnCustomStyleResolved(styles);

			UpdateEdgeControl();
		}

        public void OnUpdate()
        {
            UpdateFlow();
        }

        void OnMouseDown(MouseDownEvent e)
        {
            // modified by wmt
            //if (e.clickCount == 2)
            //{
            //    // Empirical offset:
            //    var position = e.mousePosition;
            //    position += new Vector2(-10f, -28);
            //    Vector2 mousePos = owner.ChangeCoordinatesTo(owner.contentViewContainer, position);

            //    owner.AddRelayNode(input as PortView, output as PortView, mousePos);
            //}
        }

        public string ToEdgeString()
        {
            var edgeStr = JsonUtility.ToJson(serializedEdge);
            return edgeStr;// $"{serializedEdge.GUID},{serializedEdge.inputNode.GUID},{serializedEdge.inputFieldName},{serializedEdge.inputPortIdentifier},{serializedEdge.outputNode.GUID},{serializedEdge.outputFieldName},{serializedEdge.outputPortIdentifier}";
        }
    }
}
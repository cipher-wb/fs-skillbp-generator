using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public partial class ConfigGraphView : INodeForLayoutConvertor
    {
        #region OldLayout
        private class FormattedNode
        {
            /// <summary>
            /// The actual node
            /// </summary>
            public BaseNode Node;

            /// <summary>
            /// Starting from 0 at the main nodes
            /// </summary>
            public int Layer;

            /// <summary>
            /// Child node cache
            /// </summary>
            /// <remarks>
            /// Only contains selected nodes. Also pretends that we have a tree instead of a graph.
            /// </remarks>
            public List<FormattedNode> ChildNodes;

            /// <summary>
            /// Position in the layer
            /// </summary>
            public int Offset;

            /// <summary>
            /// How far the subtree needs to be moved additionally
            /// </summary>
            public int SubtreeOffset;
        }
        public void AutoFormat()
        {
            var nodes = graph.nodes;
            if (nodes.Count <= 1) return;

            // Depth first search
            // false = not visited
            // true = visited
            // Also used to test if a node should be visited
            var nodesToVisit = nodes.ToDictionary(n => n, _ => false);
            int visitedNodesCount = 0;

            // While we have not processed every node, run the breadth first search
            while (visitedNodesCount < nodes.Count)
            {
                var output = new List<BaseNode>();

                var toProcess = new Queue<BaseNode>();

                var start = nodesToVisit.First(kvp => !kvp.Value).Key;

                nodesToVisit[start] = true;
                toProcess.Enqueue(start);

                while (toProcess.Count > 0)
                {
                    var node = toProcess.Dequeue();
                    output.Add(node);
                    visitedNodesCount++;

                    foreach (var edge in node.GetOutputEdges()) // all ports instead of just the inputs
                    {
                        if (nodesToVisit.TryGetValue(edge.inputNode, out bool visited) && !visited)
                        {
                            nodesToVisit[edge.inputNode] = true;
                            toProcess.Enqueue(edge.inputNode);
                        }
                    }
                }

                // Call the internal auto-format method
                AutoFormatConnectedNodes(output, GetBoundingBox(output));
            }
        }

        private Rect GetBoundingBox(List<BaseNode> nodes)
        {
            Vector2 min = nodes.Select(n => n.position.position).Aggregate((a, b) => Vector2.Min(a, b));
            Vector2 max = nodes.Select(n => n.position.position + GetSize(n)).Aggregate((a, b) => Vector2.Max(a, b));
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private Vector2 GetSize(BaseNode node)
        {
            return new Vector2(node.position.width, node.position.height);
        }

        private void AutoFormatConnectedNodes(List<BaseNode> nodes, Rect boundingBox)
        {
            if (nodes.Count <= 1) return;

            // Assumes that all nodes are connected

            // Storing extra data for each node
            var graphNodes = nodes.ToDictionary(n => n, n => new FormattedNode() { Node = n });

            // Find rightmost nodes (nodes that don't have any of our nodes to the right)
            var endNodes = graphNodes.Values
                            .Where(n => !n.Node.GetInputNodes().Any(c => graphNodes.ContainsKey(c)))
                            .OrderBy(n => n.Node.position.y) // Keep the order of the endNodes
                            .ToList();

            var endNode = new FormattedNode() { Layer = -1, ChildNodes = endNodes };

            // Sorted child nodes, as a tree
            SetChildNodes(graphNodes, endNode);

            // Longest path layering
            int maxLayer = SetLayers(graphNodes, endNode);

            // Set offsets
            int maxOffset = SetOffsets(endNode, maxLayer);

            Vector2 topRightPosition = endNodes.First().Node.position.position;

            // TODO: Better node spacing (figure out how much space the biggest node in this row takes up and make the row a bit bigger than that)
            foreach (var n in graphNodes)
            {
                if (nodeViewsPerNode.TryGetValue(n.Key, out var nodeView))
                {
                    var pos = n.Key.position;
                    pos.x = n.Value.Layer * 450 + topRightPosition.x;
                    pos.y = n.Value.Offset * 250 + topRightPosition.y;
                    nodeView.SetPosition(pos);
                }
            }
        }

        private static int SetOffsets(FormattedNode endNode, int maxLayer)
        {
            int maxOffset = 0;
            int[] offsets = new int[maxLayer + 1];
            // TODO: Replace with iterative version?
            /*var nodeStack = new Stack<Tuple<FormattedNode, FormattedNode>>();
            nodeStack.Push(Tuple.Create(endNode, endNode));
            while(nodeStack.Count > 0)
            {
                
            }*/
            void SetOffsets(FormattedNode node, FormattedNode straightParent)
            {
                if (node.Layer >= 0 && offsets[node.Layer] > node.Offset)
                {
                    straightParent.SubtreeOffset = Math.Max(straightParent.SubtreeOffset, offsets[node.Layer] - node.Offset);
                }

                int childOffset = node.Offset;
                bool firstIteration = true;
                foreach (var childNode in node.ChildNodes)
                {
                    childNode.Offset = childOffset;
                    SetOffsets(childNode, firstIteration ? straightParent : childNode);
                    childOffset = childNode.Offset + 1;

                    firstIteration = false;
                }

                if (node.Layer >= 0)
                {
                    node.Offset += straightParent.SubtreeOffset;
                    maxOffset = Math.Max(maxOffset, node.Offset);
                    offsets[node.Layer] = node.Offset + 1;
                }
            }

            SetOffsets(endNode, endNode);
            return maxOffset;
        }

        private void SetChildNodes(Dictionary<BaseNode, FormattedNode> graphNodes, FormattedNode endNode)
        {
            var nodesStack = new Stack<FormattedNode>();
            nodesStack.Push(endNode);
            var visitedChildNodes = new HashSet<FormattedNode>();
            while (nodesStack.Count > 0)
            {
                var node = nodesStack.Pop();

                if (node.ChildNodes == null)
                {
                    node.ChildNodes = node.Node.GetOutputNodes()
                        .Select(c => graphNodes.TryGetValue(c, out FormattedNode n) ? n : null)
                        .Where(n => n != null && !visitedChildNodes.Contains(n))
                        .ToList();
                }

                for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
                {
                    visitedChildNodes.Add(node.ChildNodes[i]);
                    nodesStack.Push(node.ChildNodes[i]);
                }
            }
        }

        private static int SetLayers(Dictionary<BaseNode, FormattedNode> graphNodes, FormattedNode endNode)
        {
            int maxLayer = 0;
            var nodesStack = new Stack<BaseNode>(endNode.ChildNodes.Select(c => c.Node));
            while (nodesStack.Count > 0)
            {
                var node = nodesStack.Pop();
                int layer = graphNodes[node].Layer;

                foreach (var outputNode in node.GetOutputNodes())
                {
                    if (outputNode != null && graphNodes.TryGetValue(outputNode, out var inputGraphNode))
                    {
                        inputGraphNode.Layer = Math.Max(inputGraphNode.Layer, layer + 1);
                        maxLayer = Math.Max(maxLayer, inputGraphNode.Layer);
                        nodesStack.Push(outputNode);
                    }
                }
            }

            return maxLayer;
        }

        // TODO sort with port index
        //private List<NodePort> Sort(IEnumerable<NodePort> nodePorts)
        //{
        //    return nodePorts.OrderBy(p =>
        //    {
        //        if (XNodeEditor.NodeEditor.portPositions.TryGetValue(p, out Vector2 value))
        //        {
        //            return value.y;
        //        }
        //        else
        //        {
        //            return float.MinValue;
        //        }
        //    }).ToList();
        //} 
        #endregion

        #region Align

        public enum NodeAlignType
        {
            Left, Right, Top, Bottom
        }

        public void Align(NodeAlignType alignType)
        {
            //WWYTODO后续优化一下逻辑
            if(selection.Count <= 0)
            {
                return;
            }
            //右、下
            Vector2 up = Vector2.one * int.MinValue;
            //左、上
            Vector2 down = Vector2.one * int.MaxValue;
            foreach (var selectable in selection)
            {
                if(selectable == null || !(selectable is BaseNodeView nodeView))
                {
                    continue;
                }

                var rect = nodeView.GetPosition();
                var pos = rect.position;

                switch (alignType)
                {
                    case NodeAlignType.Left:
                        if (pos.x  < down.x)
                        {
                            down.x = pos.x;
                        }
                        break;
                    case NodeAlignType.Right:
                        if (pos.x + rect.width > up.x)
                        {
                            up.x = pos.x + rect.width;
                        }
                        break;
                    case NodeAlignType.Top:
                        if (pos.y < down.y)
                        {
                            down.y = pos.y;
                        }
                        break;
                    case NodeAlignType.Bottom:
                        if (pos.y + rect.height > up.y)
                        {
                            up.y = pos.y + rect.height;
                        }
                        break;
                }
            }

            foreach (var selectable in selection)
            {
                if (selectable == null || !(selectable is BaseNodeView nodeView))
                {
                    continue;
                }

                var rect = nodeView.GetPosition();

                switch (alignType)
                {
                    case NodeAlignType.Left:
                        rect.x = down.x;
                        nodeView.SetPosition(rect);
                        break;
                    case NodeAlignType.Right:
                        rect.x = up.x - rect.width;
                        nodeView.SetPosition(rect);
                        break;
                    case NodeAlignType.Top:
                        rect.y = down.y;
                        nodeView.SetPosition(rect);
                        break;
                    case NodeAlignType.Bottom:
                        rect.y = up.y - rect.height;
                        nodeView.SetPosition(rect);
                        break;
                }
            }
        }

        #endregion

        #region AutoLayout

        public float SiblingDistance => 50;
        public float TreeDistance { get; }

        private NodeAutoLayouter.TreeNode layoutRootNode;
        public NodeAutoLayouter.TreeNode LayoutRootNode => layoutRootNode;

        public object PrimRootNode => primRootNode;
        private object primRootNode;

        public INodeForLayoutConvertor Init(object primRootNode)
        {
            this.primRootNode = primRootNode;
            return this;
        }

        public NodeAutoLayouter.TreeNode PrimNode2LayoutNode()
        {
            BaseNodeView graphNodeViewBase = primRootNode as BaseNodeView;

            if (graphNodeViewBase.layout.width == Single.NaN)
            {
                return null;
            }

            layoutRootNode =
                new NodeAutoLayouter.TreeNode(graphNodeViewBase.layout.height + SiblingDistance,
                    graphNodeViewBase.layout.width,
                    graphNodeViewBase.layout.y,
                    NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Positive);

            Convert2LayoutNode(
                graphNodeViewBase,
                layoutRootNode, 
                graphNodeViewBase.layout.y + graphNodeViewBase.layout.width,
                NodeAutoLayouter.CalculateMode.Horizontal | NodeAutoLayouter.CalculateMode.Positive);


            return layoutRootNode;
        }

        public void LayoutNode2PrimNode()
        {
            BaseNodeView root = primRootNode as BaseNodeView;
            Vector2 calculateRootResult = layoutRootNode.GetPos();

            //root.Position = calculateRootResult;
            //root.SetPosition(new Rect(calculateRootResult, Vector2.zero));

            Convert2PrimNode(primRootNode as BaseNodeView, layoutRootNode);
        }

        private void Convert2LayoutNode(
            BaseNodeView rootPrimNode,
            NodeAutoLayouter.TreeNode rootLayoutNode, 
            float lastHeightPoint,
            NodeAutoLayouter.CalculateMode calculateMode)
        {
            List<BaseNode> childNodes = new List<BaseNode>();

            if(rootPrimNode == null)
            {
                return;
            }

            rootPrimNode.nodeTarget?.GetChildNodes(childNodes);
            //rootPrimNode.nodeTarget.GetChildNodesRecursive(childNodes);
            foreach (var childNode in childNodes)
            {
                if (!nodeViewsPerNode.TryGetValue(childNode, out var childNodeView))
                {
                    continue;
                }

                NodeAutoLayouter.TreeNode childLayoutNode =
                    new NodeAutoLayouter.TreeNode(childNodeView.layout.height + SiblingDistance, childNodeView.layout.width,
                        lastHeightPoint + this.SiblingDistance,
                        calculateMode);
                rootLayoutNode.AddChild(childLayoutNode);
                Convert2LayoutNode(
                    childNodeView as ConfigBaseNodeView, 
                    childLayoutNode,
                    lastHeightPoint + this.SiblingDistance + childNodeView.layout.width, 
                    calculateMode);
            }
        }

        private void Convert2PrimNode(
            BaseNodeView rootPrimNode,
            NodeAutoLayouter.TreeNode rootLayoutNode)
        {
            if (rootPrimNode == null)
            {
                return;
            }

            List<BaseNode> childNodes = new List<BaseNode>();
            rootPrimNode.nodeTarget.GetChildNodes(childNodes);
            for (int i = 0; i < rootLayoutNode.children.Count; i++)
            {
                if (!nodeViewsPerNode.TryGetValue(childNodes[i], out var childNodeView))
                {
                    continue;
                }

                //Vector2 calculateResult = rootLayoutNode.children[i].GetPos();
                Vector2 calculateResult = rootPrimNode.nodeTarget.position.position + (rootLayoutNode.children[i].GetPos() - rootLayoutNode.GetPos());

                //childNode.Position = calculateResult;
                childNodeView.SetPosition(new Rect(calculateResult, Vector2.zero));

                Convert2PrimNode(childNodeView as ConfigBaseNodeView, rootLayoutNode.children[i]);
            }
        }

        #endregion
    }
}

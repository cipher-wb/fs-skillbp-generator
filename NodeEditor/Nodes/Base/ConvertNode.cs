//using GraphProcessor;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace NodeEditor.SkillEditor
//{
//    [System.Serializable, NodeMenuItem("Convert")]
//    public sealed class ConvertNode : BaseNode
//    {
//        const string packIdentifier = "_Pack";

//        [HideInInspector]
//        public struct PackedRelayData
//        {
//            public List<object> values;
//            public List<string> names;
//            public List<Type> types;
//        }

//        [Input(name = "In")]
//        public PackedRelayData input;

//        [Output(name = "Out")]
//        public PackedRelayData output;

//        public bool unpackOutput = false;
//        public bool packInput = false;
//        public int inputEdgeCount = 0;
//        [System.NonSerialized]
//        int outputIndex = 0;

//        SerializableType inputType = new SerializableType(typeof(object));

//        const int k_MaxPortSize = 14;

//        protected override void Process()
//        {
//            outputIndex = 0;
//            input = output;
//        }

//        public override string layoutStyle => "GraphProcessorStyles/RelayNode";

//        [CustomPortInput(nameof(input), typeof(object), true)]
//        public void GetInputs(List<SerializableEdge> edges)
//        {
//            inputEdgeCount = edges.Count;

//            var inputPortEdges = inputPorts[0].GetEdges();
//            var outputPort = outputPorts[0];

//            if (outputPort.portData.identifier != packIdentifier && outputIndex >= 0 && (unpackOutput || inputPortEdges.Count == 1))
//            {
//                if (output.values == null)
//                    return;

//                // When we unpack the output, there is one port per type of data in output
//                // That means that this function will be called the same number of time than the output port count
//                // Thus we use a class field to keep the index.
//                object data = output.values[outputIndex++];

//                foreach (var edge in edges)
//                {
//                    var inputRelay = edge.inputNode as ConvertNode;
//                    edge.passThroughBuffer = inputRelay != null && !inputRelay.packInput ? output : data;
//                }
//            }
//            else
//            {
//                foreach (var edge in edges)
//                    edge.passThroughBuffer = output;
//            }
//        }

//        [CustomPortOutput(nameof(output), typeof(object), true)]
//        public void PushOutputs(List<SerializableEdge> edges, NodePort outputPort)
//        {
//            if (outputPorts.Count == 0)
//                return;

//            // If the relay is only connected to another relay:
//            if (edges.Count == 1 && edges.First().inputNode.GetType() == typeof(ConvertNode))
//            {
//                if (edges.First().passThroughBuffer != null)
//                    output = (PackedRelayData)edges.First().passThroughBuffer;
//            }
//            else
//            {
//                output.values = edges.Select(e => e.passThroughBuffer).ToList();
//                output.names = edges.Select(e => e.inputPort.portData.displayName).ToList();
//                output.types = edges.Select(e => e.inputPort.portData.displayType ?? e.inputPort.fieldInfo.FieldType).ToList();
//            }
//        }

//        [CustomPortBehavior(nameof(input))]
//        IEnumerable<PortData> InputPortBehavior(List<SerializableEdge> edges)
//        {
//            // When the node is initialized, the input ports is empty because it's this function that generate the ports
//            int sizeInPixel = 0;
//            if (inputPorts.Count != 0)
//            {
//                // Add the size of all input edges:
//                var inputEdges = inputPorts[0]?.GetEdges();
//                sizeInPixel = inputEdges.Sum(e => Mathf.Max(0, e.outputPort.portData.sizeInPixel - 8));
//            }

//            if (edges.Count == 1 && !packInput)
//                inputType.type = edges[0].outputPort.portData.displayType;
//            else
//                inputType.type = typeof(object);

//            yield return new PortData
//            {
//                displayName = "",
//                displayType = inputType.type,
//                identifier = "0",
//                acceptMultipleEdges = true,
//                sizeInPixel = Mathf.Min(k_MaxPortSize, sizeInPixel + 8),
//            };
//        }

//        [CustomPortBehavior(nameof(output))]
//        IEnumerable<PortData> OutputPortBehavior(List<SerializableEdge> edges)
//        {
//            if (inputPorts.Count == 0)
//            {
//                // Default dummy port to avoid having a relay without any output:
//                yield return new PortData
//                {
//                    displayName = "",
//                    displayType = typeof(object),
//                    identifier = "0",
//                    acceptMultipleEdges = true,
//                };
//                yield break;
//            }

//            var inputPortEdges = inputPorts[0].GetEdges();
//            var underlyingPortData = GetUnderlyingPortDataList();
//            if (unpackOutput && inputPortEdges.Count == 1)
//            {
//                yield return new PortData
//                {
//                    displayName = "Pack",
//                    identifier = packIdentifier,
//                    displayType = inputType.type,
//                    acceptMultipleEdges = true,
//                    sizeInPixel = Mathf.Min(k_MaxPortSize, Mathf.Max(underlyingPortData.Count, 1) + 7), // TODO: function
//                };

//                // We still keep the packed data as output when unpacking just in case we want to continue the relay after unpacking
//                for (int i = 0; i < underlyingPortData.Count; i++)
//                {
//                    yield return new PortData
//                    {
//                        displayName = underlyingPortData?[i].name ?? "",
//                        displayType = underlyingPortData?[i].type ?? typeof(object),
//                        identifier = i.ToString(),
//                        acceptMultipleEdges = true,
//                        sizeInPixel = 0,
//                    };
//                }
//            }
//            else
//            {
//                yield return new PortData
//                {
//                    displayName = "",
//                    displayType = inputType.type,
//                    identifier = "0",
//                    acceptMultipleEdges = true,
//                    sizeInPixel = Mathf.Min(k_MaxPortSize, Mathf.Max(underlyingPortData.Count, 1) + 7),
//                };
//            }
//        }

//        static List<(Type, string)> s_empty = new List<(Type, string)>();
//        public List<(Type type, string name)> GetUnderlyingPortDataList()
//        {
//            // get input edges:
//            if (inputPorts.Count == 0)
//                return s_empty;

//            var inputEdges = GetNonRelayEdges();

//            if (inputEdges != null)
//                return inputEdges.Select(e => (e.outputPort.portData.displayType ?? e.outputPort.fieldInfo.FieldType, e.outputPort.portData.displayName)).ToList();

//            return s_empty;
//        }

//        public List<SerializableEdge> GetNonRelayEdges()
//        {
//            var inputEdges = inputPorts?[0]?.GetEdges();

//            // Iterate until we don't have a relay node in input
//            while (inputEdges.Count == 1 && inputEdges.First().outputNode.GetType() == typeof(RelayNode))
//                inputEdges = inputEdges.First().outputNode.inputPorts[0]?.GetEdges();

//            return inputEdges;
//        }
//    }
//}

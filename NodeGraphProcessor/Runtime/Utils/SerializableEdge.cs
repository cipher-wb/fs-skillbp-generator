using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphProcessor
{
	[System.Serializable]
	public partial class SerializableEdge : ISerializationCallbackReceiver
	{
		public string	GUID;

		[System.NonSerialized]
		BaseGraph		owner;

		[SerializeField]
		string			inputNodeGUID;
		[SerializeField]
		string			outputNodeGUID;

		[System.NonSerialized]
		public BaseNode	inputNode;

		[System.NonSerialized]
		public NodePort	inputPort;
		[System.NonSerialized]
		public NodePort outputPort;

		//temporary object used to send port to port data when a custom input/output function is used.
		[System.NonSerialized]
		public object	passThroughBuffer;

		[System.NonSerialized]
		public BaseNode	outputNode;

		public string	inputFieldName;
		public string	outputFieldName;

		// Use to store the id of the field that generate multiple ports
		public string	inputPortIdentifier;
		public string	outputPortIdentifier;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isVisible = true;

        public SerializableEdge() {}

		public static SerializableEdge CreateNewEdge(BaseGraph graph, NodePort inputPort, NodePort outputPort)
		{
			SerializableEdge	edge = new SerializableEdge();

			edge.owner = graph;
			edge.GUID = System.Guid.NewGuid().ToString();
			edge.inputNode = inputPort.owner;
			edge.inputFieldName = inputPort.fieldName;
			edge.outputNode = outputPort.owner;
			edge.outputFieldName = outputPort.fieldName;
			edge.inputPort = inputPort;
			edge.outputPort = outputPort;
			edge.inputPortIdentifier = inputPort.portData.identifier;
			edge.outputPortIdentifier = outputPort.portData.identifier;

			return edge;
		}

		public void OnBeforeSerialize()
		{
			if (outputNode == null || inputNode == null)
				return;

			outputNodeGUID = outputNode.GUID;
			inputNodeGUID = inputNode.GUID;
		}

		public void OnAfterDeserialize() {}

		//here our owner have been deserialized
		private void Deserialize()
		{
			if (outputNodeGUID == null || inputNodeGUID == null ||
                !owner.nodesPerGUID.ContainsKey(outputNodeGUID) || !owner.nodesPerGUID.ContainsKey(inputNodeGUID))
            {
                Debug.LogException(new System.Exception($"[{owner.name}] NodeEditor-edge-check SerializableEdge Deserialize failed, GUID:{GUID ?? string.Empty}"));
                return;
            }

			outputNode = owner.nodesPerGUID[outputNodeGUID];
			inputNode = owner.nodesPerGUID[inputNodeGUID];
			inputPort = inputNode.GetPort(inputFieldName, inputPortIdentifier);
			outputPort = outputNode.GetPort(outputFieldName, outputPortIdentifier);

            if(outputPort == default && outputFieldName == "PackedParamsOutput" && outputPortIdentifier == "0")
            {
                if(outputNode.GetType().Name.Contains("TEVAT_TALK") 
                    || outputNode.GetType().Name.Contains("TEVAT_DIALOG")
                    || outputNode.GetType().Name.Contains("TEVAT_ROLE_DIALOG")
                    || outputNode.GetType().Name.Contains("TEVAT_STORYBEGIN")
                    || outputNode.GetType().Name.Contains("TEVAT_STORYEND"))
                {
                    outputPort = outputNode.GetNpcTalkGroupIDPort();
                    outputFieldName = "NpcTalkGroupID";
                    outputPortIdentifier = "NpcTalkGroupID";
                }
                else if (outputNode.GetType().Name.Contains("TEVAT_CREATE_MODEL"))
                {
                    outputPort = outputNode.GetModelIDPort(); 
                    outputFieldName = "ModelID";
                    outputPortIdentifier = "ModelID";
                }
            }
		}
        public void Deserialize(BaseGraph baseGraph)
        {
            owner = baseGraph;
            Deserialize();
        }

        /// <summary>
        /// SyncDatas
        /// </summary>
        public void SyncDatas()
        {
            inputNode.inputPorts.PullDatas();
            outputNode.outputPorts.PushDatas();
            inputNode.UpdateAllPortsLocal();
            outputNode.UpdateAllPortsLocal();
        }

		public override string ToString() => $"{outputNode.name}:{outputPort.fieldName} -> {inputNode.name}:{inputPort.fieldName}";
	}
}

namespace GraphProcessor
{
    public partial class SerializableEdge
    {
        public bool IsValid()
        {
            return owner != null
                && owner.nodesPerGUID.TryGetValue(inputNodeGUID, out var input) && input == inputNode
                && owner.nodesPerGUID.TryGetValue(outputNodeGUID, out var output) && output == outputNode;
        }
    }
}

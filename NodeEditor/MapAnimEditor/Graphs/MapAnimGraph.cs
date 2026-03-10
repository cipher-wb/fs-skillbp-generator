using System;

namespace NodeEditor.MapAnimEditor
{
    [Serializable]
    public partial class MapAnimGraph : ConfigGraph
    {
        public override string Version => Constants.MapAnimEditor.Version;
    }
}

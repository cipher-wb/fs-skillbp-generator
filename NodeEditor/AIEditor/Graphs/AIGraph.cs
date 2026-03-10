using System;

namespace NodeEditor.AIEditor
{
    [Serializable]
    public partial class AIGraph : ConfigGraph
    {
        public override string Version => Constants.AIEditor.Version;
    }
}

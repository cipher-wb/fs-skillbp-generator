using System;

namespace NodeEditor.GamePlayEditor
{
    [Serializable]
    public partial class GamePlayGraph : ConfigGraph
    {
        public override string Version => Constants.GamePlayEditor.Version;
    }
}

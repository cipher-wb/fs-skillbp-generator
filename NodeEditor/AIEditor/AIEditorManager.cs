using TableDR;

namespace NodeEditor.AIEditor
{
    public partial class AIEditorManager : ConfigEditorManager<AIEditorManager, AIEditorSetting, AIGraph, AIGraphWindow>
    {
        public override string Version => Constants.AIEditor.Version;
        public override string Name => "AI编辑器";
        public override string Path => "Assets/Thirds/NodeEditor/AIEditor";
    }
}

using TableDR;

namespace NodeEditor.GamePlayEditor
{
    public partial class GamePlayEditorManager : ConfigEditorManager<GamePlayEditorManager, GamePlayEditorSetting, GamePlayGraph, GamePlayGraphWindow>
    {
        public override string Version => Constants.GamePlayEditor.Version;
        public override string Name => "玩法编辑器";
        public override string Path => "Assets/Thirds/NodeEditor/GamePlayEditor";
    }
}

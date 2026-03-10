using TableDR;

namespace NodeEditor.MapAnimEditor
{
    public partial class MapAnimEditorManager : ConfigEditorManager<MapAnimEditorManager, MapAnimEditorSetting, MapAnimGraph, MapAnimGraphWindow>
    {
        public override string Version => Constants.MapAnimEditor.Version;
        public override string Name => "地图动作编辑器";
        public override string Path => "Assets/Thirds/NodeEditor/MapAnimEditor";
    }
}

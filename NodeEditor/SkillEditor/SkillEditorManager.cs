namespace NodeEditor.SkillEditor
{
    public partial class SkillEditorManager : ConfigEditorManager<SkillEditorManager, SkillEditorSetting, SkillGraph, SkillGraphWindow>
    {
        public override string Version => Constants.SkillEditor.Version;
        public override string Name => "技能编辑器";
        public override string Path => "Assets/Thirds/NodeEditor/SkillEditor";
    }
}

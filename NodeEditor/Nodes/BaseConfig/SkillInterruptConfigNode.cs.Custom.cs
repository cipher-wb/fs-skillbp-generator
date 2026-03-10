namespace NodeEditor
{
    public partial class SkillInterruptConfigNode
    {
        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.SkillTagID), new TableDR.TParam());
        }
    }
}

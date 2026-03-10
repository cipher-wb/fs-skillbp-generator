using System;

namespace NodeEditor.SkillEditor
{
    [Serializable]
    public class SkillGraph : ConfigGraph
    {
        public override string Version => Constants.SkillEditor.Version;
    }
}
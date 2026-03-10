using System.Collections.Generic;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal sealed class SkillTagsConfigProcessor : NodeEditorBaseProcessor<SkillTagsConfig>
    {
        public SkillTagsConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.NameKey),
            };
        }
    }
}

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using TableDR;
using Sirenix.OdinInspector;
using System.Linq;
using System.Reflection;

namespace NodeEditor.SkillEditor
{
    internal sealed class SkillTagInfoProcessor : NodeEditorBaseProcessor<SkillTagInfo>
    {
        static class SelfAttributes
        {
        }
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            base.ProcessSelfAttributes(property, attributes);

            switch (property.Parent.Parent.ParentValues[0])
            {
                case SkillConfigNode skillConfigNode:
                case BattleAIConfigNode battleAIConfigNode:
                    {
                        attributes.Add(DefaultAttributes.HideReferenceObjectPickerAttribute);
                        break;
                    }
            }
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            switch (member.Name)
            {
                case nameof(TConfig.SkillTagConfigID):
                    {
                        attributes.Add(SkillTagsConfig.VD_TagsValue);
                        break;
                    }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

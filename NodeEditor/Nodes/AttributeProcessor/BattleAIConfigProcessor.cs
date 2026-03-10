using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal class BattleAIConfigProcessor : NodeEditorBaseProcessor<BattleAIConfig>
    {
        static class SelfAttributes
        {
            public static ListDrawerSettingsAttribute AI参数列表 = new ListDrawerSettingsAttribute
            {
                HideAddButton = true,
                OnTitleBarGUI = "OnTitleBarGUI_AISkillTagsList",
                OnBeginListElementGUI = "OnBeginListElement_AISkillTagsList",
                OnEndListElementGUI = "OnEndListElement_AISkillTagsList"
            };
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.AISkillTagsList):
                        {
                            attributes.Add(SelfAttributes.AI参数列表);
                            break;
                        }
                }
                base.ProcessChildMemberAttributes(parentProperty, member, attributes);
            }
        }
    }
}
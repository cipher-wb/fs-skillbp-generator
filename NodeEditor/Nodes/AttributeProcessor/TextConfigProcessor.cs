using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    internal sealed class TextConfigProcessor : NodeEditorBaseProcessor<TextConfig>
    {
        public TextConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.TextKey),
                nameof(TConfig.VoiceTextKey),
            };
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.TextEditor):
                    case nameof(config.VoiceTextEditor):
                        {
                            // 字符串，多行显示
                            attributes.Add(DefaultAttributes.TextAreaAttribute);
                            // delay标签影响多行显示，删除下
                            attributes.RemoveAll(attr => attr is DelayedPropertyAttribute);
                            break;
                        }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

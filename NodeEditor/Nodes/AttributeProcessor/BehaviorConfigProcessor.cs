using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    internal sealed class BehaviorConfigProcessor : NodeEditorBaseProcessor<BehaviorConfig>
    {
        public static class SelfAttributes
        {
            public static InfoBoxAttribute TypeInfoBox = new InfoBoxAttribute(
                "【注意】\n" +
                "-如需新增类型，联系程序增加\n" +
                "-切换类型会清空数据", InfoMessageType.Warning);
            public static ValueDropdownAttribute ValueDropdownAttribute_BehaviorType = new ValueDropdownAttribute("@VD_BehaviorType");
            public static ShowIfAttribute ShowIfAttribute = new ShowIfAttribute("@IsShowProperty($property)");
            public static string DefaultLabelText = "@GetPropertyDesc($property.Name)";
        }
        public BehaviorConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.Loc1Key),
                nameof(TConfig.Loc2Key),
            };
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.BaseID):
                        break;
                    case nameof(config.Type):
                        attributes.Add(SelfAttributes.ValueDropdownAttribute_BehaviorType);
                        attributes.Add(SelfAttributes.TypeInfoBox);
                        break;
                    default:
                        var labelText = attributes.Find(attr => attr is LabelTextAttribute) as LabelTextAttribute;
                        if (labelText != null)
                        {
                            labelText.Text = SelfAttributes.DefaultLabelText;
                        }
                        break;
                }
                switch (member.Name)
                {
                    //case nameof(config.StrArg1):
                    //case nameof(config.StrArg2):
                    //case nameof(config.StrArg3):
                    //case nameof(config.StrArg4):
                    case nameof(config.Loc1Editor):
                    case nameof(config.Loc2Editor):
                        // 字符串，多行显示
                        attributes.Add(DefaultAttributes.TextAreaAttribute);
                        // delay标签影响多行显示，删除下
                        attributes.RemoveAll(attr => attr is DelayedPropertyAttribute);
                        break;
                }
                attributes.Add(SelfAttributes.ShowIfAttribute);
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

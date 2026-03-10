using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal class GuideConfigProcessor : NodeEditorBaseProcessor<GuideConfig>
    {
        static class SelfAttributes
        {
            public static TitleGroupAttribute 引导类型 = new TitleGroupAttribute("引导类型", indent: true, boldTitle: false);
            public static TitleGroupAttribute UI节点 = new TitleGroupAttribute("UI节点 - 参考局外GuideConfi", indent: true, boldTitle: false);
            public static TitleGroupAttribute 提示文本 = new TitleGroupAttribute("提示文本", indent: true, boldTitle: false);
        }

        public GuideConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.TextKey),
            };
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    switch (member.Name)
                    {
                        case nameof(config.GuideStepType):
                            attributes.Add(SelfAttributes.引导类型);
                            break;

                        case nameof(config.WindowName):
                        case nameof(config.FragName):
                        case nameof(config.ComponentPath):
                            attributes.Add(SelfAttributes.UI节点);
                            break;

                        case nameof(config.Text):
                        case nameof(config.GuideAlignType):
                            attributes.Add(SelfAttributes.提示文本);
                            break;

                        case nameof(config.GuideEventParams):
                        case nameof(config.Param1):
                        case nameof(config.ElementScale):
                        case nameof(config.ElementSize):
                        case nameof(config.ElementOffset):
                            // 关闭显示
                            attributes.Add(DefaultAttributes.HideIfAttribute_True);
                            break;
                        default:
                            break;
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

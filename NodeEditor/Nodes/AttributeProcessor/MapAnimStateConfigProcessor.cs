using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using Funny.Base.Utils;
using TableDR;
using GameApp;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace NodeEditor.SkillEditor
{
    internal class MapAnimStateConfigProcessor : NodeEditorBaseProcessor<MapAnimStateConfig>
    {
        static class SelfAttrbutes
        {
            public static TitleGroupAttribute 不要直接填的 = new TitleGroupAttribute("不要直接填的", indent: true, order: 99, boldTitle: false);
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
                        case nameof(config.Type):
                            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
                            break;
                        case nameof(config.JumpType):
                            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
                            SirenixEditorGUI.DrawBorders(new Rect(new Vector2(0, 0), new Vector2(50, 50)), 500);

                            switch (config.JumpType)
                            {
                                case MapAnimStateConfig_TJumpType.Random:
                                    config.Values.GetListRef().FitCount(config.JumpId.Count);
                                    break;
                                case MapAnimStateConfig_TJumpType.Condition:
                                case MapAnimStateConfig_TJumpType.ConditionOrEnd:
                                    config.StrValues.GetListRef().FitCount(config.JumpId.Count);
                                    config.Values.GetListRef().FitCount(config.JumpId.Count);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case nameof(config.ID):
                        case nameof(config.UnitAnimType):
                        case nameof(config.TimeOut):
                        case nameof(config.OutValues):
                        case nameof(config.PreconditionTarget):
                        case nameof(config.Preconditions):
                            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
                            break;
                        default:
                            attributes.Add(SelfAttrbutes.不要直接填的);
                            break;
                    }
                }
            }
        }
        static public void FitCount<T>(List<T> list, int count)
        {
            for (int i = list.Count; i < count; i++)
            {
                list.Add(default(T));
            }
        }
    }
}

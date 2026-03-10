using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class BuffConfigProcessor : NodeEditorBaseProcessor<BuffConfig>
    {
        public static class SelfAttributes
        {
            public static TitleGroupAttribute 基础信息 = new TitleGroupAttribute("基础信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute UI信息 = new TitleGroupAttribute("UI信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 特效信息 = new TitleGroupAttribute("特效信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 逻辑信息 = new TitleGroupAttribute("逻辑信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute buff组信息 = new TitleGroupAttribute("buff组信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 效果信息 = new TitleGroupAttribute("效果信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 状态信息 = new TitleGroupAttribute("状态信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 属性信息 = new TitleGroupAttribute("属性信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 技能参数值信息 = new TitleGroupAttribute("技能参数值信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 未配置模块 = new TitleGroupAttribute("未配置模块", indent: true, order: 1, boldTitle: false);

            public static OnValueChangedAttribute OnValueChange_AddState = new OnValueChangedAttribute("OnValueChange_AddState");
        }

        public BuffConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.BuffDescKey),
                nameof(TConfig.BuffNameKey),
            };
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                // TODO 属性默认Add行为修改
                switch (member.Name)
                {
                    case nameof(config.Attrs):
                        break;
                }
                switch (member.Name)
                {
                    case nameof(config.ID):
                    case nameof(config.BuffName):
                    case nameof(config.BuffNameKey):
                    case nameof(config.BuffNameEditor):
                    case nameof(config.BuffDesc):
                    case nameof(config.BuffDescKey):
                    case nameof(config.BuffDescEditor):
                        attributes.Add(SelfAttributes.基础信息);
                        break;
                    case nameof(config.Icon):
                    case nameof(config.IsShowIcon):
                    case nameof(config.IconPriority):
                    case nameof(config.TagIcon):
                    case nameof(config.MarkResourcePath):
                    case nameof(config.MarkResourceShowType):
                    case nameof(config.MarkResourceShowPos):
                    case nameof(config.ShowIconVisibleType):
                    case nameof(config.MarkResourceShowAnim):
                    case nameof(config.MarkResourceShowFullEffect):
                        attributes.Add(SelfAttributes.UI信息);
                        break;
                    case nameof(config.EffectID):
                    case nameof(config.EffectIsOverlay):
                        attributes.Add(SelfAttributes.特效信息);
                        break;
                    case nameof(config.BuffTypeFlags):
                    case nameof(config.IsRefreshTime):
                    case nameof(config.LastTime):
                    case nameof(config.LoopTime):
                    case nameof(config.OverlyingMax):
                    case nameof(config.IsDeadExist):
                    case nameof(config.SameCoexistType):
                    case nameof(config.DiffCoexistType):
                    case nameof(config.IsSkillLevelHighReplaceLow):
                    case nameof(config.IsIgnoreStateRules):
                    case nameof(config.BuffLevel):
                        attributes.Add(SelfAttributes.逻辑信息);
                        break;
                    case nameof(config.BuffGroups):
                    case nameof(config.RemoveOtherBuffGroups):
                        attributes.Add(SelfAttributes.buff组信息);
                        break;
                    case nameof(config.SkillEffectListOnStart):
                    case nameof(config.SkillEffectListOnLoop):
                    case nameof(config.SkillEffectListOnEnd):
                    case nameof(config.SkillEffectListOnInterrupt):
                    case nameof(config.SkillEffectListOnOverlayingMax):
                        attributes.Add(SelfAttributes.效果信息);
                        break;
                    case nameof(config.AddState):
                        {
                            attributes.Add(SelfAttributes.状态信息);
                            var vdAttr = attributes.Find((attr) => { return attr is ValueDropdownAttribute; }) as ValueDropdownAttribute;
                            if (vdAttr != null)
                            {
                                vdAttr.ValuesGetter = $"@TableDR.CustomEnumUtility.VD_TEntityState_Write";
                            }
                            else
                            {
                                // 导表工具问题，暂时新增保证下拉能正常
                                attributes.Add(new ValueDropdownAttribute($"@TableDR.CustomEnumUtility.VD_TEntityState_Write"));
                            }
                            attributes.Add(SelfAttributes.OnValueChange_AddState);
                        }
                        break;
                    case nameof(config.IsHideUiProgress):
                        attributes.Add(SelfAttributes.状态信息);
                        break;
                    case nameof(config.Attrs):
                    case nameof(config.IsTempAttrs):
                    case nameof(config.EffectAttrsOnlySelf):
                        attributes.Add(SelfAttributes.属性信息);
                        break;
                    case nameof(config.SkillTags):
                    case nameof(config.IsTempSkillTags):
                        attributes.Add(SelfAttributes.技能参数值信息);
                        break;
                    default:
                        attributes.Add(SelfAttributes.未配置模块);
                        break;
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        protected override bool ColorIfConditionAction(object obj, string propertyName)
        {
            if (obj is BuffConfig config)
            {
                switch (propertyName)
                {
                    case nameof(config.ID):
                        return config.ID == default;
                    case nameof(config.BuffNameEditor):
                        return config.BuffNameEditor == default;
                    case nameof(config.BuffDescEditor):
                        return config.IsShowIcon && config.BuffDescEditor == default;
                    case nameof(config.Icon):
                        return config.IsShowIcon && config.Icon == default;
                    case nameof(config.BuffTypeFlags):
                        return config.IsShowIcon && config.BuffTypeFlags == default;
                }
            }
            return base.ColorIfConditionAction(obj, propertyName);
        }
    }
}

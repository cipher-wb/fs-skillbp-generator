using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal sealed class SkillConfigProcessor : NodeEditorBaseProcessor<SkillConfig>
    {
        static class SelfAttributes
        {
            public static TitleGroupAttribute 逻辑相关 = new TitleGroupAttribute("逻辑相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 施法相关 = new TitleGroupAttribute("施法相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 指示器_目标相关 = new TitleGroupAttribute("指示器_目标相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 智能施法 = new TitleGroupAttribute("智能施法", indent: true, boldTitle: false);
            public static TitleGroupAttribute 施法目标 = new TitleGroupAttribute("施法目标", indent: true, boldTitle: false);
            public static TitleGroupAttribute 指示器_目标相关_即将废弃 = new TitleGroupAttribute("(即将废弃)指示器_目标相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 打断相关 = new TitleGroupAttribute("打断相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 时间相关 = new TitleGroupAttribute("时间相关-帧", indent: true, boldTitle: false);
            public static TitleGroupAttribute 施法距离 = new TitleGroupAttribute("施法距离-厘米", indent: true, boldTitle: false);
            public static TitleGroupAttribute UI相关 = new TitleGroupAttribute("UI相关", indent: true, boldTitle: false, order: -1);
            public static TitleGroupAttribute 基础信息 = new TitleGroupAttribute("基础信息", indent: true, boldTitle: false, order: -2);
        }
        
        public SkillConfigProcessor()
        {
            hideMembers = new HashSet<string>
            {
                nameof(TConfig.SkillNameKey),
                nameof(TConfig.SkillDescKey),
            };
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var configNode = parentProperty.ParentValues[0] as SkillConfigNode;
            if (configNode != null)
            {
                var config = configNode.Config;
                if (member.MemberType == MemberTypes.Property)
                {
                    switch (member.Name)
                    {
                        case nameof(config.SkillMainType):
                            {
                                attributes.Add(SkillConfig.Attribute_OnValueChange_SkillMainType);
                                break;
                            }
                        case nameof(config.SkillSubType):
                            {
                                attributes.Add(SkillConfig.Attribute_OnValueChange_SkillSubType);

                                foreach (var item in attributes)
                                {
                                    if (item is ValueDropdownAttribute)
                                    {
                                        attributes.Remove(item);
                                        break;
                                    }    
                                }
                                attributes.Add(SkillConfig.Attribute_CustomValueDrawerAttribute_SkillSubType);
                                break;
                            }
                        case nameof(config.SkillDamageTagsList):
                            {
                                attributes.Add(SkillConfig.Attribute_SkillDamageTagsList);
                                break;
                            }
                        case nameof(config.SkillTagsList):
                            {
                                attributes.Add(SkillConfig.Attribute_SkillTagsList);
                                break;
                            }
                        case nameof(config.SkillTipsConditionSkillTagsList):
                            {
                                attributes.Add(SkillConfig.Attribute_SkillTipsConditionSkillTagsList);
                                break;
                            }
                        case nameof(config.SkillIndicatorType):
                            {
                                attributes.Add(SkillConfig.Attribute_TIndicatorType);
                                break;
                            }
                        case nameof(config.SkillIndicatorParam):
                            {
                                attributes.Add(SkillConfig.Attribute_IndicatorParam);
                                break;
                            }
                        case nameof(config.SkillIndicatorParamTagConfigIds):
                            {
                                attributes.Add(SkillConfig.Attribute_SkillIndicatorParamTagConfigIds);
                                break;
                            }
                        case nameof(config.SkillIndicatorResParam):
                            {
                                attributes.Add(SkillConfig.Attribute_IndicatorResParam);
                                break;
                            }
                        case nameof(config.SkillIndicatorResParamTagConfigIds):
                            {
                                attributes.Add(SkillConfig.Attribute_SkillIndicatorResParamTagConfigIds);
                                break;
                            }
                        case nameof(config.CdType):
                            {
                                attributes.Add(SkillConfig.Attribute_CdType);
                            }
                            break;
                        case nameof(config.ComboCdList):
                            {
                                attributes.Add(SkillConfig.Attribute_ComboCDList);
                                attributes.Add(SkillConfig.Attribute_HideIf_CDType);
                            }
                            break;
                        case nameof(config.SmartCastTargetCondTemplate):
                            attributes.Add(SkillConfig.Attribute_GetCastTargetCondTemplateOptions);
                            attributes.Add(SkillConfig.Attribute_OnValueChange_SmartCastTargetCondTemplate);
                            break;
                        case nameof(config.CastTargetCondTemplate):
                            attributes.Add(SkillConfig.Attribute_GetCastTargetCondTemplateOptions);
                            attributes.Add(SkillConfig.Attribute_OnValueChange_CastTargetCondTemplate);
                            break;
                        case nameof(config.SkillRangeTagConfigId):
                            attributes.Add(SkillConfig.Attribute_CustomValueDrawerAttribute_SkillRangeTagConfigId);
                            break;
                        case nameof(config.SkillMinRangeTagConfigId):
                            attributes.Add(SkillConfig.Attribute_CustomValueDrawerAttribute_SkillMinRangeTagConfigId);
                            break;
                        case nameof(config.BDLabels):
                            attributes.Add(SkillConfig.Attribute_CustomValueDrawerAttribute_BDLabels);
                            break;
                    }

                    switch (member.Name)
                    {
                        case nameof(config.Icon):
                        case nameof(config.SkillDescEditor):
                        case nameof(config.SkillNameEditor):
                        case nameof(config.SkillGrowthDesc):
                        case nameof(config.IsHideContinueUseSkillHeadBar):
                        case nameof(config.EnhanceSkillBuffConfigID):
                            attributes.Add(SelfAttributes.UI相关);
                            break;
                        case nameof(config.Condition):
                        case nameof(config.AICastCondition):
                        case nameof(config.SkillEffectExecuteInfo):
                        case nameof(config.SkillEffectPassiveExecuteInfo):
                        case nameof(config.ChantCounterValuesList):
                        case nameof(config.LGDamageValuesList):
                        case nameof(config.SkillTagsList):
                        case nameof(config.SkillDamageTagsList):
                        case nameof(config.SkillTipsConditionSkillTagsList):
                        case nameof(config.CasterEffectList):
                        case nameof(config.SkillEffectOnUnEquip):
                            attributes.Add(SelfAttributes.逻辑相关);
                            break;
                        case nameof(config.NeedTargetInRange):
                        case nameof(config.IsSkillCastNoTargetInIdle):
                        case nameof(config.LockEntityPosTypeAfterUseSkill):
                        case nameof(config.UseSkillForbidUpdateFaceDir):
                        case nameof(config.ButtonUpConfig):
                        case nameof(config.InterruptConfig):
                        case nameof(config.ReActiveConfig):
                        case nameof(config.UseSkillSpeedDownTime):
                        case nameof(config.UseSkillSpeedDownValue):
                            attributes.Add(SelfAttributes.施法相关);
                            break;
                        case nameof(config.SkillIndicatorType):
                        case nameof(config.SkillIndicatorParam):
                        case nameof(config.SkillIndicatorParamTagConfigIds):
                        case nameof(config.SkillIndicatorResParam):
                        case nameof(config.SkillIndicatorResParamTagConfigIds):
                            attributes.Add(SelfAttributes.指示器_目标相关);
                            break;
                        case nameof(config.SkillRange):
                        case nameof(config.SkillMinRange):
                        case nameof(config.SkillRangeTagConfigId):
                        case nameof(config.SkillMinRangeTagConfigId):
                        case nameof(config.AISkillRange):
                        case nameof(config.ExtraAlertRange):
                        case nameof(config.IsCloseChaseInAlertRange):
                            attributes.Add(SelfAttributes.施法距离);
                            break;
                        case nameof(config.SkillCastIsNotInterruptable):
                        case nameof(config.SkillEffectOnSkillCastInterrupt):
                            attributes.Add(SelfAttributes.打断相关);
                            break;
                        case nameof(config.CdTime):
                        case nameof(config.CdType):
                            attributes.Add(SelfAttributes.时间相关);
                            attributes.Add(new PropertyOrderAttribute(-1));
                            break;
                        case nameof(config.CDMaxStoreCount):
                        case nameof(config.SkillFixCdTime):
                            attributes.Add(SelfAttributes.时间相关);
                            attributes.Add(new PropertyOrderAttribute(-1));
                            attributes.Add(new EnableIfAttribute("EnableShowStorePropry"));
                            break;
                        case nameof(config.ComboCdList):
                        case nameof(config.SkillBaseDuration):
                        case nameof(config.LockEntityAfterUseSkillDuration):
                        case nameof(config.SkillCastFrame):
                        case nameof(config.IsPassiveSkillHideCD):
                        case nameof(config.IsPassiveSkillNotRunByCD):
                        case nameof(config.SkillBufferFrame):
                        case nameof(config.SkillBufferStartFrame):
                        case nameof(config.IsSkillBufferFrameCanMove):
                            attributes.Add(SelfAttributes.时间相关);
                            break;
                        case nameof(config.SmartCastTargetBasePriority):
                        case nameof(config.SmartCastTargetCondTemplate):
                        case nameof(config.SmartCastTargetMonsterRankCond):
                        case nameof(config.SmartCastTargetCampCond):
                        case nameof(config.SmartCastNoTargetIndicatorPos):
                        case nameof(config.SmartCastNoTargetCancelUse):
                            attributes.Add(SelfAttributes.智能施法);
                            break;
                        case nameof(config.CastTargetCondTemplate):
                        case nameof(config.CastTargetMonsterRankCond):
                        case nameof(config.CastTargetCampCond):
                            attributes.Add(SelfAttributes.施法目标);
                            break;
                        default:
                            attributes.Add(SelfAttributes.基础信息);
                            break;
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        protected override bool ColorIfConditionAction(object obj, string propertyName)
        {
            if (obj is SkillConfig config)
            {
                switch (propertyName)
                {
                    case nameof(config.ID):
                        return config.ID == default;
                    case nameof(config.SkillNameEditor):
                        return config.SkillNameEditor == default;
                    case nameof(config.SkillDescEditor):
                        return config.SkillDescEditor == default;
                    case nameof(config.Icon):
                        return config.Icon == default;
                    case nameof(config.SkillRange):
                        return config.SkillRange < 0;
                    case nameof(config.SkillMinRange):
                        return config.SkillMinRange < 0;
                    case nameof(config.AISkillRange):
                        return config.AISkillRange < 0;
                    case nameof(config.SkillRangeTagConfigId):
                        return config.SkillRangeTagConfigId == default;
                    case nameof(config.SkillMinRangeTagConfigId):
                        return config.SkillMinRangeTagConfigId == default;
                }
            }
            return base.ColorIfConditionAction(obj, propertyName);
        }
    }
}

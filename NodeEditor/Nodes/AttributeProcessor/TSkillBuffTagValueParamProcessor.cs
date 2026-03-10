using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using TableDR;
using Sirenix.OdinInspector;
using System.Reflection;

namespace NodeEditor.SkillEditor
{
    internal sealed class TSkillBuffTagValueParamProcessor : NodeEditorBaseProcessor<TSkillBuffTagValueParam>
    {

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (parentProperty.ValueEntry.WeakSmartValue is TSkillBuffTagValueParam param)
            {
                bool bIsParamValue = false;
                TParamType paramType = TParamType.TPT_NULL;
                switch (member.Name)
                {
                    case nameof(param.AddValue_ParamValue):
                        {
                            bIsParamValue = true;
                            paramType = param.AddValue_ParamType;
                        }
                        break;
                    case nameof(param.SkillID_ParamValue):
                        {
                            bIsParamValue = true;
                            paramType = param.SkillID_ParamType;
                        }
                        break;
                    case nameof(param.SkillTagID_ParamValue):
                        {
                            bIsParamValue = true;
                            paramType = param.SkillTagID_ParamType;
                        }
                        break;
                    case nameof(param.AddValue_ParamType):
                        attributes.Add(new OnValueChangedAttribute("OnValueChange_AddValueParamType"));
                        break;
                    case nameof(param.SkillID_ParamType):
                        attributes.Add(new OnValueChangedAttribute("OnValueChange_SkillIDParamType"));
                        break;
                    case nameof(param.SkillTagID_ParamType):
                        attributes.Add(new OnValueChangedAttribute("OnValueChange_SkillTagIDParamType"));
                        break;
                    default:
                        break;
                }

                if (bIsParamValue == true)
                {
                    switch (paramType)
                    {
                        case TParamType.TPT_NULL:
                            break;
                        case TParamType.TPT_ATTR:
                            {
                                var vdAttr = new ValueDropdownAttribute($"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Read");
                                attributes.Add(vdAttr);
                                var desc = paramType.GetDescription(false);
                                vdAttr.DropdownTitle = $"请选择 {desc}...";
                            }
                            break;
                        case TParamType.TPT_COMMON_PARAM:
                            {
                                var vdAttr = new ValueDropdownAttribute($"{Constants.EnumVDPefix}{nameof(TCommonParamType)}");
                                attributes.Add(vdAttr);
                                var desc = paramType.GetDescription(false);
                                vdAttr.DropdownTitle = $"请选择 {desc}...";
                            }
                            break;
                        case TParamType.TPT_COMMON_SKILL_PARAM:
                            {
                                var vdAttr = new ValueDropdownAttribute($"{Constants.EnumVDPefix}{nameof(TCommonSkillParamType)}");
                                attributes.Add(vdAttr);
                                var desc = paramType.GetDescription(false);
                                vdAttr.DropdownTitle = $"请选择 {desc}...";
                            }
                            break;
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

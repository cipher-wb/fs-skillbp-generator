using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class BattleCustomParamConfig_ParamInfoProcessor : NodeEditorBaseProcessor<BattleCustomParamConfig_ParamInfo>
    {
        public static class SelfAttributes
        {
            public static OnValueChangedAttribute OnValueChange_ParamOptionTypeType = new OnValueChangedAttribute("OnValueChange_ParamOptionTypeType");
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (parentProperty.ValueEntry.WeakSmartValue is BattleCustomParamConfig_ParamInfo paramInfo)
            {
                switch (member.Name)
                {
                    case nameof(paramInfo.OptionType):
                        {
                            attributes.Add(SelfAttributes.OnValueChange_ParamOptionTypeType);
                        }
                        break;
                    case nameof(paramInfo.ParamList):
                        {
                            bool hideAdd = (paramInfo.OptionType == BattleCustomParamConfig_TParamOptionType.TPOT_QTE);
                            attributes.Add(new ListDrawerSettingsAttribute
                            {
                                OnBeginListElementGUI = "OnBeginListElement_OptionParams",
                                OnEndListElementGUI = "OnEndListElement_OptionParams",
                                HideAddButton = hideAdd,
                                HideRemoveButton = hideAdd,
                                ShowFoldout = true,
                                DraggableItems = false,
                            });
                        }
                        break;
                    default:
                        break;
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}
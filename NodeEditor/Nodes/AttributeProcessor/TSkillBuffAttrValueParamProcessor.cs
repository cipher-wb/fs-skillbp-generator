using System;
using System.Collections.Generic;
using Sirenix.OdinInspector.Editor;
using TableDR;
using System.Reflection;
using Sirenix.OdinInspector;

namespace NodeEditor.SkillEditor
{
    internal sealed class TSkillBuffAttrValueParamProcessor : NodeEditorBaseProcessor<TSkillBuffAttrValueParam>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (parentProperty.ValueEntry.WeakSmartValue is TSkillBuffAttrValueParam param)
            {
                if (member.Name == nameof(param.AttrType))
                {
                    var vdAttr = attributes.Find((attr) => { return attr is ValueDropdownAttribute; }) as ValueDropdownAttribute;
                    if (vdAttr != null)
                    {
                        vdAttr.ValuesGetter = $"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Write";
                    }
                    else
                    {
                        attributes.Add(new ValueDropdownAttribute($"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Write"));
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

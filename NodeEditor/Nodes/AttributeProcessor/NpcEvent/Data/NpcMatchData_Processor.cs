using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcMatchData_Processor : NodeEditorBaseProcessor<NpcMatchData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if(parentProperty.Parent.ParentType == typeof(MapEventFormulaConfigNode))
            {
                var node = parentProperty.Parent.ParentValues[0];
                if (node is MapEventFormulaConfigNode formulaConfig)
                {
                    ProcessMapEventFormulaConfig(formulaConfig, member, attributes);
                }
            }
            else if (parentProperty.Parent.ParentType.Name.Contains("MapEventGeneralFuncConfigNode"))
            {
                ProcessMapEventGeneralFuncConfig(parentProperty.Parent.Name, member, attributes);
            }

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        public void ProcessMapEventFormulaConfig(MapEventFormulaConfigNode node, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name != "Value" && member.Name != "ValueEnd") { return; }

            if (node.Config.IsRangeTypeState())
            {
                attributes.Add(new ValueDropdownAttribute("VDL_State"));
            }
            else if (node.Config.IsRangeTypeRankLevel())
            {
                attributes.Add(new ValueDropdownAttribute("VDL_RankLevel"));
            }
        }

        public void ProcessMapEventGeneralFuncConfig(string parentName, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name != "Value" && member.Name != "ValueEnd") { return; }

            if (parentName == "ItemRanks")
            {
                attributes.Add(new ValueDropdownAttribute("VDL_ItemRank")
                {
                    DoubleClickToConfirm = true,
                });
            }
        }
    }
}

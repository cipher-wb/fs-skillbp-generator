using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class MapEventTarget_Processor : NodeEditorBaseProcessor<MapEventTarget>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if(member.Name == "TargetIndex")
            {
                attributes.Add(new ShowIfAttribute("@IsShowTargetIndex"));
            }

            if ( parentProperty.Parent.ParentType.Name.Contains("MapEventGeneralFuncConfigNode")
                || parentProperty.Parent.ParentType == typeof(AddHeadLineData))
            {
                ProcessMapEventGeneralFuncConfig(parentProperty.Parent.Name, member, attributes);
            }

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        public void ProcessMapEventGeneralFuncConfig(string parentName, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name != "TargetType") { return; }

            var vdaName = parentName switch
            {
                "TargetsForm" => "VDL_Type1",
                "TargetsTo" => "VDL_Type1",
                "ModifyTargets" => "VDL_Type1",
                "RelativeTarget" => "VDL_Type1",
                "MapResData" => "VDL_Type1",

                "ActorFavorRelativeTargets" => "VDL_Type2",
                "AwardDropTargets" => "VDL_Type2",
                "CampFavorTargets" => "VDL_Type2",

                "ActorFavorTargets" => "VDL_Type3",
                "FriendTargets" => "VDL_Type3",
                "EnemyTargets" => "VDL_Type3",

                "DeadTargets" => "VDL_Type4",

                "AddHeadLineTargets" => "VDL_Type5",
                _ => string.Empty,
            };

            if (!string.IsNullOrEmpty(vdaName))
            {
                foreach(var attr in attributes)
                {
                    if (attr is ValueDropdownAttribute vda && vda.ValuesGetter == "@TableDR.EnumUtility.VD_MapEventTargetType")
                    {
                        attributes.Remove(attr);
                        break;
                    }
                }

                attributes.Add(new ValueDropdownAttribute(vdaName));
            }
        }
    }
}

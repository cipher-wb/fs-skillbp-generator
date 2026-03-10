using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcEventLinkConfigProcessor : NodeEditorBaseProcessor<NpcEventLinkConfig>
    {
        public readonly HashSet<string> HideMemeber = new HashSet<string>()
        {
            "ActorParam",
            "LinkActorParamDes",
        };
        
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("参数",0), new HashSet<string> { "RandomPointID", "RelativeCoordX", "RelativeCoordY", "ScreenRange" } },
            {("外部引用(连线操作)",99), new HashSet<string> { "ID", "ActorCondition", "FirstGroupID", "GlobalEventAction", "NpcTemplateID" } }
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessHideIf(member.Name, attributes);

            ProcessGroupInfo(member.Name, attributes, GroupInfo);

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        /// <summary>
        /// HideIfAttribute
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="attributes"></param>
        private void ProcessHideIf(string memberName, List<Attribute> attributes)
        {
            if (HideMemeber.Contains(memberName))
            {
                attributes.Add(new HideIfAttribute("@true"));
            }
        }
    }
}

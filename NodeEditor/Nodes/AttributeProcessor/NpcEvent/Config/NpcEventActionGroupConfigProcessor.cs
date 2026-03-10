using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcEventActionGroupConfigProcessor : NodeEditorBaseProcessor<NpcEventActionGroupConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("外部引用(连线操作)",99), new HashSet<string> {"ActionList", "NextGroupID" } }
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessEnableIf(member.Name, attributes);

            ProcessGroupInfo(member.Name, attributes, GroupInfo);

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        /// <summary>
        /// EnableIfAttribute
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="attributes"></param>
        private void ProcessEnableIf(string memberName, List<Attribute> attributes)
        {
            switch (memberName)
            {
                case "ID":
                case "ActionGroupID":
                    {
                        attributes.Add(new EnableIfAttribute("@false"));
                    }
                    break;
            }
        }
    }
}

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcEventActionGroupIndexConfigProcessor : NodeEditorBaseProcessor<NpcEventActionGroupIndexConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessEnableIf(member.Name, attributes);

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
                    {
                        attributes.Add(new EnableIfAttribute("@false"));
                    }
                    break;
            }
        }
    }
}

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcTalkOptionConfigProcessor : NodeEditorBaseProcessor<NpcTalkOptionConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new EnableIfAttribute("@false"));

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

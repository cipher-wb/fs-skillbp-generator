using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcEventActionConfigProcessor : NodeEditorBaseProcessor<NpcEventActionConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("外部引用(连线操作)", 99), new HashSet<string> {"ID", "ActionType", "AIParamsID", "SubActions", "TriggerInteract", "ActiveInteract", "Param", "Tip", "Des" } },
            {("其他参数",1), new HashSet<string> { "Life", "EvoLife", "WalkState" } },
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessGroupInfo(member.Name, attributes, GroupInfo);

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

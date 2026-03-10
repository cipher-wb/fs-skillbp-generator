using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class MapEventConditionConfigProcessor : NodeEditorBaseProcessor<MapEventConditionConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("外部引用(连线操作)", 0), new HashSet<string> { "ID", "FormulaID", "GameEntityType", "SubGameEntityType" } }
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessGroupInfo(member.Name, attributes, GroupInfo);

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class MapEventStoryConfigProcessor : NodeEditorBaseProcessor<MapEventStoryConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("外部引用(连线操作)", 99), new HashSet<string> { "ID", "ActionGroupIndexID", "TriggerType", "IntParams1", "PerformanceGroupID" } },
            {("标题", 1), new HashSet<string> { "TitleEditor" } }
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
            switch (memberName)
            {
                case "TitleKey":
                case "Title":
                    attributes.Add(new HideIfAttribute("@true"));
                    break;
            }
        }
    }
}

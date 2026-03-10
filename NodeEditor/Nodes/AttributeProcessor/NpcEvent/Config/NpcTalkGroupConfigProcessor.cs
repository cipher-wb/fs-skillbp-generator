using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcTalkGroupConfigProcessor : NodeEditorBaseProcessor<NpcTalkGroupConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("对话组类型",0), new HashSet<string> { "TalkGroupType"} },
            {("气泡参数",1), new HashSet<string> { "IsLoop", "IsOrder", "IsSoliloquy", "RandomIntervalSecMin", "RandomIntervalSecMax"} },
            {("外部引用(连线操作)",99), new HashSet<string> { "ID", "TalkIDs" } },
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
                case "RandomIntervalSecs":
                case "DescKey":
                case "Desc":
                case "TitleKey":
                case "Title":
                case "Conditions":
                {
                    attributes.Add(new HideIfAttribute("@true"));   
                    break;
                } 
            }
        }
    }
}

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcTalkConfigProcessor : NodeEditorBaseProcessor<NpcTalkConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("演员编号",0), new HashSet<string> { "NpcIndex", "NarratorIndex", "StealthIndex" } },
            {("文本",1), new HashSet<string> { "TalkDescEditor" } },
            {("对话气泡",2), new HashSet<string> { "RandomIntervalSecMin", "RandomIntervalSecMax", "WaitSecs", "FixIntervalSecs" } },
            {("诱惑气泡",3), new HashSet<string> { "NpcAssitBubbleType" } },
            {("外部引用(连线操作)",99), new HashSet<string> { "ID", "TalkGroupID", "DialogOptionID" } },
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            ProcessHideIf(member.Name, attributes);

            ProcessSuffixLabel(member.Name, attributes);

            ProcessTextArea(member.Name, attributes);

            ProcessGroupInfo(member.Name, attributes, GroupInfo);

            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }



        /// <summary>
        /// SuffixLabelAttribute
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="attributes"></param>
        private void ProcessSuffixLabel(string memberName, List<Attribute> attributes)
        {
            switch (memberName)
            {
                case "WaitSecs":
                    attributes.Add(new SuffixLabelAttribute("秒"));
                    break;
            }
        }

        /// <summary>
        /// TextAreaAttribute
        /// </summary>
        /// <param name="memberName"></param>
        /// <para
        private void ProcessTextArea(string memberName, List<Attribute> attributes)
        {
            switch (memberName)
            {
                case "TalkDescEditor":
                    attributes.Add(new UnityEngine.TextAreaAttribute(5,20));
                    attributes.Add(new InfoBoxAttribute("$TalkDescEditor"));
                    break;
            }
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
                case "IntervalSecs":
                case "TalkDescKey":
                case "TalkDesc":
                case "StealthIndex":
                    attributes.Add(new HideIfAttribute("@true"));
                    break;
            }
        }
    }
}

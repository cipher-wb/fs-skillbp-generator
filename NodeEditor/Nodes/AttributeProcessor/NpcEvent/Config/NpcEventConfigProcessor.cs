using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class NpcEventConfigProcessor: NodeEditorBaseProcessor<NpcEventConfig>
    {
        public readonly Dictionary<(string Title, int order), HashSet<string>> GroupInfo = new Dictionary<(string Title, int order), HashSet<string>>()
        {
            {("事件优先级",0), new HashSet<string> { "Priority"} },
            {("事件信息",1), new HashSet<string> { "Close", "EventGroupID", "EventDes", "EventType", "Weight", "EndCondition", "MaxByEvo", "EventTag", "TaskTag", "TriggerType", "BackHome", "DynamicActor", "AIPriority", "LogMark" } },
            {("外部引用(连线操作)",99), new HashSet<string> { "MainActorCondition", "MainFirstGroupID", "GlobalEventAction", "LinkActor", 
                "PlayerConditionID", "ConfirmTalkGroupID", "LifePathIds", "ActorFormation", "StroyEntrance", "StroyExit", 
                "StartGeneralFuncs", "EndGeneralFuncs", "BeginPerformanceGroupID", "StandbyPerformanceGroupID", "StoryPerformanceGroupID", "StoryEndPerformanceGroupID" } }
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
                    {
                        attributes.Add(new EnableIfAttribute("@false"));
                    }
                    break;
            }
        }
    }
}

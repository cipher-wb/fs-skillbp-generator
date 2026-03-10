using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_OPEN_STORY_ARENA : TalkOptionData
    {
        public TNEDOT_OPEN_STORY_ARENA(NpcTalkOptionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            BaseNode.InspectorError = string.Empty;
        }

        public override void OnConfigToData()
        {

        }

        public override void OnValueChanged()
        {

        }

        public override void SetDefault()
        {

        }
    }
}
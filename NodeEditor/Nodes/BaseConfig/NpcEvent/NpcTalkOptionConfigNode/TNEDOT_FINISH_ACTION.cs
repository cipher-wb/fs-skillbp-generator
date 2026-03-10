using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_FINISH_ACTION : TalkOptionData
    {
        public TNEDOT_FINISH_ACTION(NpcTalkOptionConfigNode baseNode)
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
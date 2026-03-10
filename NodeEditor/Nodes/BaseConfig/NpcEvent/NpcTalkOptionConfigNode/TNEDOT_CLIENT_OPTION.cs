using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_CLIENT_OPTION : TalkOptionData
    {
        public TNEDOT_CLIENT_OPTION(NpcTalkOptionConfigNode baseNode)
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
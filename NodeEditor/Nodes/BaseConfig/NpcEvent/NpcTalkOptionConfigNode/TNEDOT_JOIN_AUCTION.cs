using System;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_JOIN_AUCTION : TalkOptionData
    {
        public TNEDOT_JOIN_AUCTION(NpcTalkOptionConfigNode baseNode)
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
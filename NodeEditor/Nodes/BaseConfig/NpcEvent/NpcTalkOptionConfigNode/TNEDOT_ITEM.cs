using System;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_ITEM : TalkOptionData
    {
        public TNEDOT_ITEM(NpcTalkOptionConfigNode baseNode)
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
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_USE_OT : TalkOptionData
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("本命法宝"), HideReferenceObjectPicker]
        public TOTForMapEvent OrinTreasureType { get; private set; } = TOTForMapEvent.MOT_None;

        public TNEDOT_USE_OT(NpcTalkOptionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            BaseNode.InspectorError = string.Empty;

            if(OrinTreasureType == TOTForMapEvent.MOT_None)
            {
                BaseNode.InspectorError += "本命法宝未选择\n";
            }
        }

        public override void OnValueChanged()
        {
            BaseNode.SetConfigValue(nameof(BaseNode.Config.IntArg1), (int)OrinTreasureType);
        }

        public override void OnConfigToData()
        {
            OrinTreasureType = (TOTForMapEvent)BaseNode.Config.IntArg1;
        }

        public override void SetDefault()
        {
            OrinTreasureType = TOTForMapEvent.MOT_None;
        }
    }
}
using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class TNEDOT_MINIGAME : TalkOptionData
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("小游戏类型"), HideReferenceObjectPicker]
        public TMiniGameType MiniGameType { get; private set; } = TMiniGameType.TMiniGame_None;

        [Sirenix.OdinInspector.ShowInInspector, LabelText("筑基突破分数"), HideReferenceObjectPicker]
        public int MiniGamePoint { get; private set; }

        public TNEDOT_MINIGAME(NpcTalkOptionConfigNode baseNode)
        {
            BaseNode = baseNode;
        }

        public override void CheckError()
        {
            BaseNode.InspectorError = string.Empty;

            if (MiniGameType == TMiniGameType.TMiniGame_None)
            {
                BaseNode.InspectorError += "游戏类型未选择\n";
            }

            if(MiniGamePoint <= 0)
            {
                BaseNode.InspectorError += "游戏分数未填写\n";
            }
        }

        public override void OnValueChanged()
        {
            BaseNode.SetConfigValue(nameof(BaseNode.Config.IntArg1), (int)MiniGameType);
            BaseNode.SetConfigValue(nameof(BaseNode.Config.IntArg2), MiniGamePoint);
        }

        public override void OnConfigToData()
        {
            MiniGameType = (TMiniGameType)BaseNode.Config.IntArg1;
            MiniGamePoint = BaseNode.Config.IntArg2;
        }

        public override void SetDefault()
        {
            MiniGameType = TMiniGameType.TMiniGame_None;
            MiniGamePoint = 0;
        }
    }
}
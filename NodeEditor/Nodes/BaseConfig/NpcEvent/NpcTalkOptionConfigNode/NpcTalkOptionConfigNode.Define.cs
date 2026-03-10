using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    public abstract class TalkOptionData
    {
        public NpcTalkOptionConfigNode BaseNode { get; protected set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("描述"), HideReferenceObjectPicker]
        [OnValueChanged("OnOptionDescChanged"), DelayedProperty]
        public string OptionDesc { get; protected set; }

        private void OnOptionDescChanged()
        {
            BaseNode.SetConfigValue(nameof(BaseNode.Config.OptionDescEditor), OptionDesc);
        }

        public virtual void OnRefreshCustomName()
        {
            var title = $"[{BaseNode.Config.ID}][选项][{EnumUtility.GetDescription(BaseNode.Config.NpcEventDialogOptionType, false)}]";
            BaseNode.SetCustomName(title);
        }

        public abstract void OnValueChanged();

        public abstract void CheckError();

        public void ConfigToData()
        {
            OptionDesc = BaseNode.Config.OptionDescEditor;

            OnConfigToData();
        }

        public abstract void OnConfigToData();

        public abstract void SetDefault();
    }

    /// <summary>
    /// NpcTalkOptionConfigNode定义类
    /// </summary>
    public partial class NpcTalkOptionConfigNode
    {
        public static Sirenix.OdinInspector.ValueDropdownList<TNpcEventDialogOptionType> VD_TNpcEventDialogOptionType = new Sirenix.OdinInspector.ValueDropdownList<TNpcEventDialogOptionType>()
        {
            {"继续",TNpcEventDialogOptionType.TNEDOT_NEXT},
            {"战斗",TNpcEventDialogOptionType.TNEDOT_BATTLE},
            {"NPC索要物品",TNpcEventDialogOptionType.TNEDOT_ITEM},
            {"参加拍卖会",TNpcEventDialogOptionType.TNEDOT_JOIN_AUCTION},
            {"小游戏",TNpcEventDialogOptionType.TNEDOT_MINIGAME},
            {"结束行为",TNpcEventDialogOptionType.TNEDOT_FINISH_ACTION},
            {"客户端选项",TNpcEventDialogOptionType.TNEDOT_CLIENT_OPTION},
            {"打开剧情擂台",TNpcEventDialogOptionType.TNEDOT_OPEN_STORY_ARENA},
            {"选用本命法宝",TNpcEventDialogOptionType.TNEDOT_USE_OT},
            {"堪舆",TNpcEventDialogOptionType.TNEDOT_KANYU}, 
        };

        /// <summary>
        /// 类型映射
        /// </summary>
        private Dictionary<TNpcEventDialogOptionType, Type> optionTypeMapping = new Dictionary<TNpcEventDialogOptionType, Type>()
        {
            { TNpcEventDialogOptionType.TNEDOT_NEXT, typeof(TNEDOT_NEXT) },     //有参数
            { TNpcEventDialogOptionType.TNEDOT_BATTLE, typeof(TNEDOT_BATTLE) },
            { TNpcEventDialogOptionType.TNEDOT_ITEM, typeof(TNEDOT_ITEM) },
            { TNpcEventDialogOptionType.TNEDOT_JOIN_AUCTION, typeof(TNEDOT_JOIN_AUCTION) },
            { TNpcEventDialogOptionType.TNEDOT_MINIGAME, typeof(TNEDOT_MINIGAME) },             //有参数
            { TNpcEventDialogOptionType.TNEDOT_FINISH_ACTION, typeof(TNEDOT_FINISH_ACTION) },
            { TNpcEventDialogOptionType.TNEDOT_CLIENT_OPTION, typeof(TNEDOT_CLIENT_OPTION) },
            { TNpcEventDialogOptionType.TNEDOT_OPEN_STORY_ARENA, typeof(TNEDOT_OPEN_STORY_ARENA) }, 
            { TNpcEventDialogOptionType.TNEDOT_USE_OT, typeof(TNEDOT_USE_OT) },     //有参数
            { TNpcEventDialogOptionType.TNEDOT_KANYU, typeof(TNEDOT_KANYU) },
        };
    }
}

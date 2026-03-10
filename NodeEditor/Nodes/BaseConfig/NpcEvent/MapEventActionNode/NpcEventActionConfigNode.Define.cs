using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    public enum ActionAIMode
    {
        None = 0,
        InteractOnce = 10003,
        InteractCD = 10004,
        CatchPlayerOnce = 10005,
        RunningBusinessKiller = 10006,
        InteractOnceLongDistance = 10007,
        InteractCDLongDistance = 10008,
    }

    /// <summary>
    /// NpcEventActionConfigNode定义类
    /// </summary>
    public partial class NpcEventActionConfigNode
    {
        /// <summary>
        /// GameEntityType Entity大类
        /// </summary>
        public static ValueDropdownList<int> VD_AIMode = new ValueDropdownList<int>()
        {
            {"无", (int)ActionAIMode.None},
            {"主动交互(仅一次，关闭后不会再次主动交互)", (int)ActionAIMode.InteractOnce},
            {"主动交互(多次、5秒CD)", (int)ActionAIMode.InteractCD},
            {"主动交互（远距离、仅一次，关闭后不会再次主动交互)", (int)ActionAIMode.InteractOnceLongDistance},
            {"主动交互（远距离、多次、5秒CD)", (int)ActionAIMode.InteractCDLongDistance},
            {"追杀玩家交互(一次, 超出5000返回)", (int)ActionAIMode.CatchPlayerOnce},
            {"无条件追杀玩家交互(多次，5秒CD，不会返回)", (int)ActionAIMode.RunningBusinessKiller},
        };

        public static HashSet<NpcEventActionConfig_TEventActionType> MainActionSet = new HashSet<NpcEventActionConfig_TEventActionType>()
        {
            NpcEventActionConfig_TEventActionType.TEVAT_NPC_DEATH,
            NpcEventActionConfig_TEventActionType.TEVAT_CREATE_ENCOUNTER_AUCTION,
            NpcEventActionConfig_TEventActionType.TEVAT_MINI_GAME,
            NpcEventActionConfig_TEventActionType.TEVAT_TALK,
            NpcEventActionConfig_TEventActionType.TEVAT_JUSTWAIT,
        };

        public static HashSet<NpcEventActionConfig_TEventActionType> SubActionSet = new HashSet<NpcEventActionConfig_TEventActionType>()
        {
            NpcEventActionConfig_TEventActionType.TEVAT_CREATE_MODEL,
            NpcEventActionConfig_TEventActionType.TEVAT_DIALOG,
            NpcEventActionConfig_TEventActionType.TEVAT_TALK,
        };

        public static HashSet<NpcEventActionConfig_TEventActionType> GlobalActionSet = new HashSet<NpcEventActionConfig_TEventActionType>()
        {
            NpcEventActionConfig_TEventActionType.TEVAT_CREATE_MODEL,
        };

        public static HashSet<NpcEventActionConfig_TEventActionType> PerformanceActionSet = new HashSet<NpcEventActionConfig_TEventActionType>()
        {
            NpcEventActionConfig_TEventActionType.TEVAT_CAMERA,
            NpcEventActionConfig_TEventActionType.TEVAT_PLAYER_MOVE,
            NpcEventActionConfig_TEventActionType.TEVAT_ROLE_DIALOG,
            NpcEventActionConfig_TEventActionType.TEVAT_STORYBEGIN,
            NpcEventActionConfig_TEventActionType.TEVAT_STORYEND,
            NpcEventActionConfig_TEventActionType.TEVAT_WAIT_PERFORMANCE,
        };

        public enum EndStoryType
        {
            [LabelText("继续")]
            Continue,
            [LabelText("结束")]
            End,
        }
    }

    public abstract class ActionData
    {
        public NpcEventActionConfigNode BaseNode { get; protected set; }

        public abstract List<int> ToParam();

        public abstract void ToData(IReadOnlyList<int> param);

        public abstract void CheckError();
    }
}

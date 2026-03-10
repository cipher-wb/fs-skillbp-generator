using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        /// <summary>
        /// 常规符号
        /// </summary>
        public enum SymbolType
        {
            Greater = 0,        //大于
            Less = 1,           //小于
            GreaterEqual = 2,   //大于等于
            LessEqual = 3,      //小于等于
            Equal = 4,          //等于
            NotEqual = 5,       //不等于
        }

        /// <summary>
        /// 怪物
        /// </summary>
        public static readonly List<MapEventConditionType> VD_MonsterType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
            MapEventConditionType.MECT_RACE,
            MapEventConditionType.MECT_WUXING,
            MapEventConditionType.MECT_MONSTER_RANK,
            MapEventConditionType.MECT_STATE_FIXED,
            MapEventConditionType.MECT_STATE_WITHPLAYER,
            MapEventConditionType.MECT_STATE_WITHPLAYER_DYNAMIC,
        };

        /// <summary>
        /// 灵植
        /// </summary>
        public static readonly List<MapEventConditionType> VD_PlantType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
            MapEventConditionType.MECT_QUALITY,
            MapEventConditionType.MECT_RANK_LEVEL,
        };

        /// <summary>
        /// 矿物
        /// </summary>
        public static readonly List<MapEventConditionType> VD_MetalType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
            MapEventConditionType.MECT_QUALITY,
            MapEventConditionType.MECT_RANK_LEVEL,
        };

        /// <summary>
        /// 鱼
        /// </summary>
        public static readonly List<MapEventConditionType> VD_FishType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
            MapEventConditionType.MECT_QUALITY,
            MapEventConditionType.MECT_RANK_LEVEL,
        };

        /// <summary>
        /// 宝箱
        /// </summary>
        public static readonly List<MapEventConditionType> VD_BoxType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
        };

        /// <summary>
        /// 灵气团
        /// </summary>
        public static readonly List<MapEventConditionType> VD_LingQiType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,
            MapEventConditionType.MECT_LINGQI,
        };

        /// <summary>
        /// NPC
        /// </summary>
        public static readonly List<MapEventConditionType> VD_NpcType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_LIMIT_ACTOR,

            MapEventConditionType.MECT_STATE_FIXED,
            MapEventConditionType.MECT_STATE_WITHPLAYER,
            MapEventConditionType.MECT_STATE_WITHPLAYER_DYNAMIC,

            MapEventConditionType.MECT_HOUSE_FIXED,
            MapEventConditionType.MECT_HOUSE_SAMEPLYER,
            MapEventConditionType.MECT_HOUSE_DIFFPLYER,

            MapEventConditionType.MECT_ITEMPURIFY,
            MapEventConditionType.MECT_ITEMID,

            MapEventConditionType.MECT_SILVER,
            MapEventConditionType.MECT_FAVOR_WITHPLAYER,
            MapEventConditionType.MECT_LEVEL_FIXED,
            MapEventConditionType.MECT_QUALITY,
            MapEventConditionType.MECT_WUXING,
            MapEventConditionType.MECT_SEX,
            MapEventConditionType.MECT_NPC_STATUS,
            MapEventConditionType.MECT_GODEVIL_VAL,
            MapEventConditionType.MECT_IN_DEMON_INVASION_AREA,
            MapEventConditionType.MECT_NPC_TYPE,
            MapEventConditionType.MECT_GAMETAG,
            MapEventConditionType.MECT_LIKE,
            MapEventConditionType.MECT_REGIONID,
            MapEventConditionType.MECT_REGION_GAMEPLAY,
            MapEventConditionType.MECT_WEATHER,
            MapEventConditionType.MECT_TASK,
        };

        /// <summary>
        /// 全局判断
        /// </summary>
        public static readonly List<MapEventConditionType> VD_PlayerType = new List<MapEventConditionType>()
        {
            MapEventConditionType.MECT_TASK,
            MapEventConditionType.MECT_VARIABLE,
            MapEventConditionType.MECT_MAP_EXPLORATION,
            MapEventConditionType.MECT_PLAYER_STATE,
            MapEventConditionType.MECT_PLAYER_FUNC_UNLOCK,
        };
    }
}

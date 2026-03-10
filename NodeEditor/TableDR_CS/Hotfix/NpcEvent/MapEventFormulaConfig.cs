#if UNITY_EDITOR

namespace TableDR
{
    /// <summary> 
    /// FormulaA 范围公式
    /// FormulaB Int数组
    /// FormulaC 五行
    /// FormulaD Int64数组
    /// FormulaE Int数组列表
    /// </summary>
    public partial class MapEventFormulaConfig
    {
        /// <summary>
        /// 无参类型
        /// </summary>
        public bool IsInvisable() =>
            ConditionType == MapEventConditionType.MECT_HOUSE_SAMEPLYER
            || ConditionType == MapEventConditionType.MECT_HOUSE_DIFFPLYER
            || ConditionType == MapEventConditionType.MECT_NULL
            || ConditionType == MapEventConditionType.MECT_IN_DEMON_INVASION_AREA;

        #region FormulaA
        /// <summary>
        /// FormulaA 范围公式
        /// </summary>
        public bool IsFormulaA() => IsRangeTypeState() || IsRangeTypeInt() || IsRangeTypeRankLevel();

        /// <summary>
        /// FormulaA 境界
        /// </summary>
        public bool IsRangeTypeState() =>
            ConditionType == MapEventConditionType.MECT_STATE_FIXED
            || ConditionType == MapEventConditionType.MECT_PLAYER_STATE;
            

        /// <summary>
        /// FormulaA 品阶
        /// </summary>
        /// <returns></returns>
        public bool IsRangeTypeRankLevel() => ConditionType == MapEventConditionType.MECT_RANK_LEVEL;

        /// <summary>
        /// FormulaA 数值
        /// </summary>
        public bool IsRangeTypeInt() =>
            ConditionType == MapEventConditionType.MECT_STATE_WITHPLAYER
            || ConditionType == MapEventConditionType.MECT_FAVOR_WITHPLAYER
            || ConditionType == MapEventConditionType.MECT_LEVEL_FIXED
            || ConditionType == MapEventConditionType.MECT_SILVER
            || ConditionType == MapEventConditionType.MECT_GODEVIL_VAL
            || ConditionType == MapEventConditionType.MECT_RANK_LEVEL
            || ConditionType == MapEventConditionType.MECT_LINGQI;
       
        #endregion

        #region FormulaB Int数组
        /// <summary>
        /// FormulaB 总类型
        /// </summary>
        public bool IsFormulaB() => IsRefType() || IsEnumType();

        /// <summary>
        /// FormulaB 表格类型
        /// </summary>
        public bool IsRefType() =>
            ConditionType == MapEventConditionType.MECT_ITEMID
            || ConditionType == MapEventConditionType.MECT_REGIONID
            || ConditionType == MapEventConditionType.MECT_LIMIT_ACTOR
            || ConditionType == MapEventConditionType.MECT_TASK
            || ConditionType == MapEventConditionType.MECT_WEATHER
            || ConditionType == MapEventConditionType.MECT_HOUSE_FIXED;

        /// <summary>
        /// FormulaB 枚举类型
        /// </summary>
        public bool IsEnumType() =>
            ConditionType == MapEventConditionType.MECT_ITEMPURIFY
            || ConditionType == MapEventConditionType.MECT_SEX
            || ConditionType == MapEventConditionType.MECT_QUALITY
            || ConditionType == MapEventConditionType.MECT_LIKE
            || ConditionType == MapEventConditionType.MECT_NPC_STATUS
            || ConditionType == MapEventConditionType.MECT_NPC_TYPE
            || ConditionType == MapEventConditionType.MECT_REGION_GAMEPLAY
            || ConditionType == MapEventConditionType.MECT_MONSTER_RANK;
        #endregion

        #region FormulaC 五行
        /// <summary>
        /// FormulaC 总类型
        /// </summary>
        public bool IsFormulaC() =>
            ConditionType == MapEventConditionType.MECT_WUXING;

        #endregion

        #region FormulaD Int64数组
        /// <summary>
        /// FormulaC 总类型
        /// </summary>
        public bool IsFormulaD() =>
            ConditionType == MapEventConditionType.MECT_GAMETAG;

        #endregion

        #region FormulaE Int数组列表
        /// <summary>
        /// FormulaC 总类型
        /// </summary>
        public bool IsFormulaE() =>
            ConditionType == MapEventConditionType.MECT_RACE
            || ConditionType == MapEventConditionType.MECT_VARIABLE
            || ConditionType == MapEventConditionType.MECT_MAP_EXPLORATION
            || ConditionType == MapEventConditionType.MECT_PLAYER_FUNC_UNLOCK;

        #endregion
    }
}
#endif

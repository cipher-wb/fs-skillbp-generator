using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// MapEventConditionConfigNode定义类
    /// </summary>
    public partial class MapEventConditionConfigNode
    {
        /// <summary>
        /// GameEntityType Entity大类
        /// </summary>
        public static ValueDropdownList<GameEntityType> VD_GameEntityType = new ValueDropdownList<GameEntityType>()
        {
            {GameEntityType.ET_Npc.GetDescription(), GameEntityType.ET_Npc},
            {GameEntityType.TET_MRT_METAL.GetDescription(), GameEntityType.TET_MRT_METAL},
            {GameEntityType.TET_MRT_PLANT.GetDescription(), GameEntityType.TET_MRT_PLANT},
            {GameEntityType.TET_MRT_BOX.GetDescription(), GameEntityType.TET_MRT_BOX},
            {GameEntityType.TET_MRT_FISH.GetDescription(), GameEntityType.TET_MRT_FISH},
            {GameEntityType.TET_MRT_MONSTER.GetDescription(), GameEntityType.TET_MRT_MONSTER},
            {GameEntityType.TET_MRT_LINGQITUAN.GetDescription(), GameEntityType.TET_MRT_LINGQITUAN},
        };

        /// <summary>
        /// 怪物
        /// </summary>
        public static ValueDropdownList<TMapMonsterSubType> VD_MonsterSubType = new ValueDropdownList<TMapMonsterSubType>()
        {
            {TMapMonsterSubType.TMMOST_LAND_MONSTER.GetDescription(), TMapMonsterSubType.TMMOST_LAND_MONSTER},
            {TMapMonsterSubType.TMMOST_STATIC_MONSTER.GetDescription(), TMapMonsterSubType.TMMOST_STATIC_MONSTER}
        };

        /// <summary>
        /// 灵植
        /// </summary>
        public static ValueDropdownList<TMapPlantSubType> VD_PlantSubType = new ValueDropdownList<TMapPlantSubType>()
        {
            {TMapPlantSubType.TMPST_PLANT.GetDescription(), TMapPlantSubType.TMPST_PLANT},
            {TMapPlantSubType.TMPST_LARGE_PLANT.GetDescription(), TMapPlantSubType.TMPST_LARGE_PLANT},
        };

        /// <summary>
        /// 矿物
        /// </summary>
        public static ValueDropdownList<TMapMetalSubType> VD_MetalSubType = new ValueDropdownList<TMapMetalSubType>()
        {
            {TMapMetalSubType.TMMEST_METAL.GetDescription(), TMapMetalSubType.TMMEST_METAL},
            {TMapMetalSubType.TMMEST_LARGE_METAL.GetDescription(), TMapMetalSubType.TMMEST_LARGE_METAL},
        };

        /// <summary>
        /// 鱼
        /// </summary>
        public static ValueDropdownList<TMapFishSubType> VD_FishSubType = new ValueDropdownList<TMapFishSubType>()
        {
            {TMapFishSubType.TMFST_FISH.GetDescription(), TMapFishSubType.TMFST_FISH},
            {TMapFishSubType.TMFST_LARGE_FISH.GetDescription(), TMapFishSubType.TMFST_LARGE_FISH},
        };

        /// <summary>
        /// 宝箱
        /// </summary>
        public static ValueDropdownList<TMapBoxSubType> VD_MapBoxSubType = new ValueDropdownList<TMapBoxSubType>()
        {
            {TMapBoxSubType.TMBST_WUXING_BOX.GetDescription(), TMapBoxSubType.TMBST_WUXING_BOX},
            {TMapBoxSubType.TMBST_STATIC_BOX.GetDescription(), TMapBoxSubType.TMBST_STATIC_BOX},
            {TMapBoxSubType.TMBST_DYNAMIC_BOX.GetDescription(), TMapBoxSubType.TMBST_DYNAMIC_BOX},
            {TMapBoxSubType.TMBST_WATER_BOX.GetDescription(), TMapBoxSubType.TMBST_WATER_BOX},
            {TMapBoxSubType.TMBST_PLANT_BOX.GetDescription(), TMapBoxSubType.TMBST_PLANT_BOX},
            {TMapBoxSubType.TMBST_METAL_BOX.GetDescription(), TMapBoxSubType.TMBST_METAL_BOX},
            {TMapBoxSubType.TMBST_COLLECT_BOX.GetDescription(), TMapBoxSubType.TMBST_COLLECT_BOX},
        };

        /// <summary>
        /// 灵气团
        /// </summary>
        public static ValueDropdownList<TMapLingQiSubType> VD_LingQiTuanSubType = new ValueDropdownList<TMapLingQiSubType>()
        {
            {TMapLingQiSubType.TMLQST_STATIC.GetDescription(), TMapLingQiSubType.TMLQST_STATIC},
            {TMapLingQiSubType.TMLQST_WUXING.GetDescription(), TMapLingQiSubType.TMLQST_WUXING},
        };
    }
}

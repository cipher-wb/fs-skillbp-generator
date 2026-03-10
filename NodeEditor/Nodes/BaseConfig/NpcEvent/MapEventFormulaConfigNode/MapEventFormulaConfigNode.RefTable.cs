using GameApp.Editor;
using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        private Dictionary<(MapEventConditionType condition, GameEntityType entity, int subEntity), Type> conditionActorTableDict = new Dictionary<(MapEventConditionType condition, GameEntityType entity, int subEntity), Type>()
        {
            //Npc
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.ET_Npc, default), typeof(NpcConfig)},

            //怪物
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_MONSTER, (int)TMapMonsterSubType.TMMOST_LAND_MONSTER), typeof(MapLandMonsterConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_MONSTER, (int)TMapMonsterSubType.TMMOST_STATIC_MONSTER), typeof(MapMonsterConfig)},

            //灵植
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_PLANT, (int)TMapPlantSubType.TMPST_PLANT), typeof(MapPlantConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_PLANT, (int)TMapPlantSubType.TMPST_LARGE_PLANT), typeof(MapLargePlantConfig)},

            //矿物
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_METAL, (int)TMapMetalSubType.TMMEST_METAL), typeof(MapMetalConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_METAL, (int)TMapMetalSubType.TMMEST_LARGE_METAL), typeof(MapLargeMetalConfig)},

            //鱼
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_FISH, (int)TMapFishSubType.TMFST_FISH), typeof(MapFishConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_FISH, (int)TMapFishSubType.TMFST_LARGE_FISH), typeof(MapLargeFishConfig)},

            //宝箱
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_WUXING_BOX), typeof(MapBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_STATIC_BOX), typeof(TreasureBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_DYNAMIC_BOX), typeof(TreasureBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_PLANT_BOX), typeof(TreasureBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_METAL_BOX), typeof(TreasureBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_WATER_BOX), typeof(TreasureBoxConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_BOX, (int)TMapBoxSubType.TMBST_COLLECT_BOX), typeof(TreasureBoxConfig)},

            //灵气团
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_LINGQITUAN, (int)TMapLingQiSubType.TMLQST_STATIC), typeof(MapLingQiTuanConfig)},
            {(MapEventConditionType.MECT_LIMIT_ACTOR, GameEntityType.TET_MRT_LINGQITUAN, (int)TMapLingQiSubType.TMLQST_WUXING), typeof(MapLingQiTuanConfig)},
        };

        [Sirenix.OdinInspector.ShowInInspector, ShowIf("@IsRefType"), LabelText("表格"), HideReferenceObjectPicker]
        [BoxGroup("公式", centerLabel: true, order: 0)]
        [OnValueChanged("OnTableSelectDataChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnTableSelectDataAdd")]
        private List<TableSelectData> tableSelectDataList = new List<TableSelectData>();

        private bool IsRefType => Config?.IsRefType() ?? false;

        public TableSelectData OnTableSelectDataAdd()
        {
            var tableName = GetTableName(Config.ConditionType, gameEntityType, subGameEntityType);
            if (!string.IsNullOrEmpty(tableName))
            {
                return new TableSelectData(tableName, 0);
            }
            return default;
        }

        public void OnTableSelectDataChanged()
        {
            List<int> idList = default;
            foreach(var tableData in tableSelectDataList)
            {
                if(tableData.ID != 0) 
                {
                    idList ??= new List<int>();
                    idList.Add(tableData.ID);
                }
            }

            SetConfigValue(nameof(Config.FormulaB), idList);
        }

        private void RestoreFormulaB_RefTable()
        {
            tableSelectDataList?.Clear();

            var tableName = GetTableName(Config.ConditionType, gameEntityType, subGameEntityType);
            Config?.FormulaB?.ForEach(id => 
            {
                var tableSelectData = new TableSelectData(tableName, (int)id);
                tableSelectData.OnSelectedID();
                tableSelectDataList.Add(tableSelectData);
            });
        }

        private string GetTableName(MapEventConditionType conditionType, GameEntityType entityType, int subEntityType)
        {
            if(conditionType == MapEventConditionType.MECT_ITEMID) 
            { 
                return typeof(ItemConfig).FullName; 
            }
            else if (conditionType == MapEventConditionType.MECT_REGIONID)
            {
                return typeof(MapRandomRegion).FullName;
            }
            else if(conditionType == MapEventConditionType.MECT_TASK)
            {
                return typeof(QuestConfig).FullName;
            }
            else if (conditionType == MapEventConditionType.MECT_WEATHER)
            {
                return typeof(WeatherConfig).FullName;
            }
            else if(conditionType == MapEventConditionType.MECT_HOUSE_FIXED)
            {
                return typeof(FairyHouseConfig).FullName;
            }
            else if(conditionType == MapEventConditionType.MECT_LIMIT_ACTOR)
            {
                if (conditionActorTableDict.TryGetValue((conditionType, entityType, subEntityType), out var tableType))
                {
                    return tableType.FullName;
                }
            }
            return "";
        }
    }
}

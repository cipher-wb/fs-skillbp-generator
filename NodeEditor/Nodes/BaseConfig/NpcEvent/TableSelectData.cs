using GameApp.Editor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    //表格选择数据
    public class TableSelectData
    {
        public TableSelectData(string tableName, int id)
        {
            TableManagerName = tableName;
            ID = id;
            ManualID = id;
            //OnSelectedID();
        }

        private const string invalidMessage = "【注意】Excel未找到对应ID";

        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, LabelText("选择表格"), ValueDropdown("GetTableManagerNames"), EnableIf("@false")]
        [OnValueChanged("OnSelectedTable"), DelayedProperty]
        public string TableManagerName;

        [LabelText("ID"), Sirenix.OdinInspector.ShowInInspector, HideInInspector]
        [ValueDropdown("GetTableManagerIDS"), OnValueChanged("OnSelectedID"), DelayedProperty]
        public int ID;

        [LabelText("输入ID")]
        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, OnValueChanged("OnDataChangedManualID"), DelayedProperty]
        public int ManualID;

        /// <summary>
        /// 选择的表格
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("表格数据"), HideReferenceObjectPicker, EnableIf("@false"), GraphProcessor.ShowInInspector(false)]
        [InfoBox(invalidMessage, InfoMessageType.Error, "Invalid")]
        public object TableConfig;

        /// <summary>
        /// 表格选择是否有效
        /// </summary>
        private bool Invalid { get { return TableConfig == null; } }

        private string TableFullName { get { return TableManagerName?.Replace(Constants.TableManagerSuffix, "") ?? string.Empty; } }
        private string TableName
        {
            get
            {
                var split = TableFullName.Split('.');
                return split.Length == 2 ? split[1] : string.Empty;
            }
        }

        /// <summary>
        /// 表格列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetTableManagerNames()
        {
            return AppEditorFacade.DesignTable?.TableName2Manager.Select(kv => new ValueDropdownItem(kv.Key, kv.Value.GetType().FullName)) ?? null;
        }

        /// <summary>
        /// 选择表格回调
        /// </summary>
        private void OnSelectedTable()
        {
            TableConfig = default;
            ID = 0;
            ManualID = 0;
        }

        /// <summary>
        /// 选择ID回调
        /// </summary>
        public void OnSelectedID()
        {
            if (!string.IsNullOrEmpty(TableManagerName))
            {
                TableConfig = DesignTable.GetTableCell(TableName, ID);
            }
        }

        /// <summary>
        /// 手动输入ID变化
        /// </summary>
        private void OnDataChangedManualID()
        {
            ID = ManualID;

            OnSelectedID();
        }

        /// <summary>
        /// 获取ConfigID
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetTableManagerIDS()
        {
            var refTableManager = DesignTable.GetTableManager(TableName);

            dynamic refItemArray = null;

            try
            {
                refItemArray = refTableManager.GetType().GetProperty("ItemArray").GetValue(refTableManager);
            }
            catch
            {
                Log.Warning("NullReferenceException: Object reference not set to an instance of an object");
            }

            if (refItemArray != null)
            {
                foreach (var item in refItemArray.Items)
                {
                    //ConditionConfigManager
                    if (refTableManager is ConditionConfigManager)
                    {
                        if(item.BaseID >= 10000 && item.BaseID <= 20000)
                        {
                            yield return new ValueDropdownItem(item.BaseID.ToString(), (int)item.BaseID);
                        }
                    }
                    else if(refTableManager is CommonSubmitItemConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.CondDesc}", (int)item.ID);
                    }
                    else if(refTableManager is SubmitItemGroupConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.Desc}", (int)item.ID);
                    }
                    else if(refTableManager is TagConfigManager)
                    {
                        if(item.ParamType == TagConfig_QuestTagType.QSTCT_GLOBAL)
                        {
                            yield return new ValueDropdownItem($"{item.ID}-{item.Param1}", (int)item.ID);
                        }
                    }
                    else if(refTableManager is DestinyConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.DestinyName}", (int)item.ID);
                    }
                    else if (refTableManager is QuestConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.BaseID}-{item.Name}", (int)item.BaseID);
                    }
                    else if (refTableManager is MapEventPerformanceGroupConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}", (int)item.ID);
                    }
                    else if(refTableManager is VoiceConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}", (int)item.ID);
                    }
                    else if (refTableManager is ProvinceConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.ProvinceName}", (int)item.ID);
                    }         
                    else if (refTableManager is FairyHouseConfigManager 
                        || refTableManager is MapBoxConfigManager 
                        || refTableManager is EvolutionHeadlineConfigManager 
                        || refTableManager is AuctionConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.Name}", (int)item.ID);
                    }
                    else if (refTableManager is MapRandomPointManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.Name}", (int)item.ID);
                    }
                    else if (refTableManager is WeatherConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.NameString}", (int)item.ID);
                    }
                    else if (refTableManager is MapPosConfigManager)
                    {
                        yield return new ValueDropdownItem($"{item.ID}-{item.MapDynamicItemName}", (int)item.ID);
                    }
                    else
                    {
                        yield return new ValueDropdownItem(item.ID.ToString(), item.ID);
                    }
                    
                }
            }
        }
    }
}

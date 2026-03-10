using GameApp.Editor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    //表格选择数据  非主键
    public class TableSelectFilterData
    {
        public TableSelectFilterData(string tableName, int id)
        {
            TableManagerName = tableName;
            ID = id;
            ManualID = id;
        }

        private const string invalidMessage = "【注意】Excel未找到对应ID";

        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, LabelText("选择表格"), ValueDropdown("GetTableManagerNames"), EnableIf("@false")]
        [OnValueChanged("OnSelectedTable"), DelayedProperty]
        public string TableManagerName;
        
        /// <summary>
        /// 筛选过滤器
        /// </summary>
        private HashSet<int> filterIDSet = new();

        [LabelText("ID"), Sirenix.OdinInspector.ShowInInspector, HideInInspector]
        [ValueDropdown("GetTableManagerIDS")]
        public int ID;

        [LabelText("输入ID")]
        [Sirenix.OdinInspector.ShowInInspector, HideInInspector, OnValueChanged("OnManualID")]
        public int ManualID;

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
            ID = 0;
            ManualID = 0;
        }

        /// <summary>
        /// 手动输入ID变化
        /// </summary>
        private void OnManualID()
        {
            ID = ManualID;
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

            filterIDSet.Clear();
            if (refItemArray != null)
            {
                foreach (var item in refItemArray.Items)
                {
                    //DropConfigManager
                    if (refTableManager is DropConfigManager)
                    {
                        var dropID = (int)item.DropID;
                        if (filterIDSet.Add(dropID))
                        {
                            yield return new ValueDropdownItem(dropID.ToString(), dropID);
                        }
                    }
                }
            }
        }
    }
}

using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcTalkOptionConfigNode定义类
    /// </summary>
    public partial class NpcTalkOptionConfigNode
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("显示条件"), HideReferenceObjectPicker]
        [FoldoutGroup("条件相关", true, order: 1)]
        [OnValueChanged("OnShowConditionChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddConditionTable")]
        private List<TableSelectData> showConditionList = new List<TableSelectData>();

        public void OnShowConditionChanged()
        {
            List<long> idList = default;
            foreach (var tableData in showConditionList)
            {
                if (tableData.ID != 0)
                {
                    idList ??= new List<long>();
                    idList.Add(tableData.ID);
                }
            }

            SetConfigValue(nameof(Config.ShowConditionId), idList);
        }

        private TableSelectData OnAddConditionTable()
        {
            var tableFullName = typeof(ConditionConfig).FullName;
            return new TableSelectData(tableFullName, 0);
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("点击条件"), HideReferenceObjectPicker]
        [FoldoutGroup("条件相关", true, order: 1)]
        [OnValueChanged("OnClickConditionChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddConditionTable")]
        private List<TableSelectData> clickConditionList = new List<TableSelectData>();

        public void OnClickConditionChanged()
        {
            var condition = clickConditionList?.Count > 0 ? clickConditionList[0].ID : 0;
            SetConfigValue(nameof(Config.ConditionId), condition);
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("描述"), HideReferenceObjectPicker]
        [FoldoutGroup("条件相关", true, order: 1)]
        [OnValueChanged("OnConditionDescChanged"), DelayedProperty]
        private string conditionDesc = "";

        public void OnConditionDescChanged()
        {
            SetConfigValue(nameof(Config.ConditionDescEditor), conditionDesc);
        }

        private void ConfigToCondition()
        {
            var tableName = typeof(ConditionConfig).FullName;

            //ShowConditionId
            showConditionList?.Clear();
            Config?.ShowConditionId?.ForEach(id =>
            {
                var tableData = new TableSelectData(tableName, (int)id);
                tableData.OnSelectedID();
                showConditionList.Add(tableData);
            });

            //ConditionId
            clickConditionList?.Clear();
            if (Config?.ConditionId > 0)
            {
                var tableData = new TableSelectData(tableName, Config.ConditionId);
                tableData.OnSelectedID();
                clickConditionList.Add(tableData);
            }

            conditionDesc = Config?.ConditionDescEditor;
        }
    }
}

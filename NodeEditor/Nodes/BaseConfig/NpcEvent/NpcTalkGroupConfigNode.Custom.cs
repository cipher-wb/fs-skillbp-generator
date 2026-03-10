using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcTalkGroupConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            if (string.IsNullOrEmpty(Config.DescEditor))
            {
                SetCustomName($"[{Config.ID}][对话组]");
            }
            else
            {
                SetCustomName($"[{Config.ID}][对话组][{Config.DescEditor}]");
            }
        }
        
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);
            
            ConfigToCondition();
        }
        
        [FoldoutGroup("条件相关", true, order: 1)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("列表"), HideReferenceObjectPicker]
        [OnValueChanged("OnClickConditionChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddConditionTable")]
        private List<TableSelectData> ConditionList = new();

        public void OnClickConditionChanged()
        {
            var conditions = new List<long>();
            foreach (var condition in ConditionList)
            {
                conditions.Add(condition.ID);
            }
            SetConfigValue(nameof(Config.Conditions), conditions);
        }
        
        private TableSelectData OnAddConditionTable()
        {
            var tableFullName = typeof(ConditionConfig).FullName;
            return new TableSelectData(tableFullName, 0);
        }
        
        private void ConfigToCondition()
        {
            var tableName = typeof(ConditionConfig).FullName;
            
            //Conditions
            ConditionList?.Clear();
            Config.Conditions?.ForEach(id =>
            {
                var tableData = new TableSelectData(tableName, (int)id);
                tableData.OnSelectedID();
                ConditionList.Add(tableData);
            });
        }
    }
}

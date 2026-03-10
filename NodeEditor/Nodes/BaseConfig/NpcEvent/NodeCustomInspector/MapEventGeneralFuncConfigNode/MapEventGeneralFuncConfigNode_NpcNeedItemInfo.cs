using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_NpcNeedItem : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_NpcNeedItem(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("NPC消费系数"), PropertyTooltip("0表示NPC不给钱，非0表示NPC用物品价值乘以系数的灵石转移给玩家")]
        [MinValue(0), OnValueChanged("OnConsumeFactorChanged", true), DelayedProperty, ]
        public int ConsumeFactor { get; private set; }

        private void OnConsumeFactorChanged()
        {
            baseNode.Config?.ExSetValue("IntParams2", new List<int> { ConsumeFactor });
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("提交道具组表")]
        [OnValueChanged("OnTableDataChanged", true), DelayedProperty]
        public TableSelectData TableData { get; private set; } = new TableSelectData(typeof(SubmitItemGroupConfig).FullName, 0);

        private void OnTableDataChanged()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int> { TableData.ID });

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTableNotSelect(TableData);

            if(ConsumeFactor < 0)
            {
                baseNode.InspectorError += "【消费系数错误】";
            }
        }

        public void ConfigToData()
        {
            //IntParams1
            if (baseNode.Config?.IntParams1?.Count == 1)
            {
                TableData = new TableSelectData(typeof(SubmitItemGroupConfig).FullName, baseNode.Config?.IntParams1[0] ?? 0);
                TableData.OnSelectedID();
            }

            //IntParams2
            if (baseNode.Config?.IntParams2?.Count == 1)
            {
                ConsumeFactor = baseNode.Config.IntParams2[0];
            }
        }

        public void SetDefault()
        {

        }
    }
}

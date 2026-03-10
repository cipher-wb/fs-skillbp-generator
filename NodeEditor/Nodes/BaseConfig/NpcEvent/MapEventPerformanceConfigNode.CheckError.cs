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
    /// 检视面板错误提示
    /// </summary>
    public partial class MapEventPerformanceConfigNode
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        private bool IsExitInspectorError => !string.IsNullOrEmpty(InspectorError);

        public string InspectorError { get; set; }

        /// <summary>
        /// 检测表格是否选择
        /// </summary>
        /// <param name="tables"></param>
        public void AddInspectorErrorTableNotSelect(List<TableSelectData> tables)
        {
            if(tables.Count == 0)
            {
                InspectorError += "【表格未选择】\n";
                return;
            }

            foreach (var table in tables)
            {
                if (table.ID == 0)
                {
                    InspectorError += "【表格未选择】\n";
                    return;
                }
            }
        }

        /// <summary>
        /// 检测表格是否选择
        /// </summary>
        /// <param name="table"></param>
        public void AddInspectorErrorTableNotSelect(TableSelectData table)
        {
            if (table == default || (table != default && table.ID == 0))
            {
                InspectorError += "【表格未选择】\n";
            }
        }

        /// <summary>
        /// 掉落类型
        /// </summary>
        /// <param name="dropIDs"></param>
        /// <param name="dropType"></param>
        public void AddInspectorErrorDropType(TDropInfoPushType dropType)
        {
            if (dropType == TDropInfoPushType.TD_NoTips)
            {
                InspectorError += $"【掉落类型错误】\n";
            }
        }
    }
}

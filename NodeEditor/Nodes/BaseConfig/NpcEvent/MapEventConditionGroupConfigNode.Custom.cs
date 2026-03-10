using GraphProcessor;
using NodeEditor.PortType;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventConditionGroupConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][条件组]");
        }
    }
}

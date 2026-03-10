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
    /// 通用功能组节点
    /// </summary>
    public partial class MapEventGeneralFuncGroupConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            var title = $"[{Config.ID}][功能组]";
            //描述
            if (!string.IsNullOrEmpty(Config.Desc))
            {
                title += $"[{Config.Desc}]";
            }
            SetCustomName(title);
        }
    }
}

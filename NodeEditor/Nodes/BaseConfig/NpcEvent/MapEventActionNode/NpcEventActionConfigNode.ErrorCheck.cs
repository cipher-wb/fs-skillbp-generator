using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionConfigNode定义类
    /// </summary>
    public partial class NpcEventActionConfigNode
    {
        /// <summary>
        /// 错误提示
        /// </summary>
        private bool IsExitInspectorError => !string.IsNullOrEmpty(InspectorError);

        public string InspectorError { get; set; }

        public override bool OnSaveCheck()
        {
            CheckError();

            if (IsExitInspectorError)
            {
                AppendSaveMapEventRet(InspectorError);
                return false;
            }

            return base.OnSaveCheck();
        }

        /// <summary>
        /// 检查错误
        /// </summary>
        public void CheckError()
        {
            InspectorError = string.Empty;

            //上一个节点也是子节点
            var previousNode = GetPreviousNode<NpcEventActionConfigNode>();
            if (previousNode != default && previousNode.IsSubActionNode)
            {
                InspectorError += $"不允许子行为嵌套\n";
            }

            //检查行为节点位置
            if (IsSubActionNode && !SubActionSet.Contains(Config.ActionType))
            {
                InspectorError += $"非子行为 处于子行为节点\n";
            }
            else if (IsMainActionNode && !MainActionSet.Contains(Config.ActionType))
            {
                InspectorError += $"非主行为 处于主行为节点\n";
            }
            else if (IsGlobalActionNode && !GlobalActionSet.Contains(Config.ActionType))
            {
                InspectorError += $"非全局行为 处于全局行为节点\n";
            }
            else if (IsPerformanceNode && !PerformanceActionSet.Contains(Config.ActionType))
            {
                InspectorError += $"非剧情行为 处于剧情行为节点\n";
            }
            

            ActionData?.CheckError();
        }
    }
}

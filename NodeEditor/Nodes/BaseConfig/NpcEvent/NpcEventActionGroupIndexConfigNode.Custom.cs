using GraphProcessor;
using NodeEditor.PortType;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class NpcEventActionGroupIndexConfigNode
    {
        /// <summary>
        /// 可以连接多个行为，从而实现并行
        /// </summary>
        [Output]
        [HideInInspector]
        public int ConnectedGroupIDS;

        /// <summary>
        /// 预设参数
        /// </summary>
        protected override void OnPreset()
        {
            SetConfigValue(nameof(Config.ActionGroupType), EventActionGroupType.TAGT_SEQUENCE);
        }

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}]行为组[{Utils.GetEnumDescription(Config.ActionGroupType)}]");
        }

        /// <summary>
        /// 重写input 不显示名字
        /// </summary>
        /// <returns></returns>
        protected override PortData CustomPortBehavior_ID()
        {
            // TODO
            var refPortType = TablePortTypesHelper.GetTypeInput(ConfigType);
            if (refPortType == null)
            {
                refPortType = ConfigType;
                Log.Error($"InputPortBehavior_ID failed, {ConfigType.Name}");
            }
            return new PortData
            {
                displayName = "",
                displayType = refPortType,
                identifier = "0",
                acceptMultipleEdges = false,
                portColor = TableAnnotation.Inst.GetNodeColor(ConfigType),
            };
        }

        #region CustomPort ConnectedGroupIDS
        /// <summary>
        /// 按照表格目前的规则，无法生成port  额外添加MainFirstGroupID的Port
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        [CustomPortBehavior(nameof(ConnectedGroupIDS))]
        public IEnumerable<PortData> ConnectedGroupIDS_Behavior(List<SerializableEdge> edges)
        {
            //var desc = Utils.GetEnumDescription(Config.ActionGroupType);
            bool moreEdges = Config?.ActionGroupType == EventActionGroupType.TAGT_CHOOSE ? true : false;

            yield return new PortData
            {
                //displayName = $"{desc}",
                displayName = "",
                displayType = typeof(INpcEventActionGroupConfig),
                identifier = nameof(ConnectedGroupIDS),
                acceptMultipleEdges = moreEdges
            };
        }
        #endregion
    }
}

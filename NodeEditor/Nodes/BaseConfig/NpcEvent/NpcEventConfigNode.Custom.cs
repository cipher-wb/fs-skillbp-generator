using GraphProcessor;
using HotFix.Game.MapEvent;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using Funny.Base.Utils;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class NpcEventConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][事件][{Utils.GetEnumDescription(Config.EventType)}]");

            //开场
            beginPerformanceGroup.Clear();
            Config.BeginPerformanceGroupID?.ForEach(groupID =>
            {
                var tableData = new TableSelectData(typeof(MapEventPerformanceGroupConfig).FullName, groupID);
                tableData.OnSelectedID();
                beginPerformanceGroup.Add(tableData);
            });

            //待机
            standbyPerformanceGroup.Clear();
            Config.StandbyPerformanceGroupID?.ForEach(groupID =>
            {
                var tableData = new TableSelectData(typeof(MapEventPerformanceGroupConfig).FullName, groupID);
                tableData.OnSelectedID();
                standbyPerformanceGroup.Add(tableData);
            });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            //自动包含主角结束
            if (Config.EndCondition == default)
            {
                SetConfigValue(nameof(Config.EndCondition), new List<NpcEventConfig_TNpcEventEndCondition>()
                {
                    NpcEventConfig_TNpcEventEndCondition.TNEEC_MAIN_ACTOR
                });
            }
            else if (!Config.EndCondition.Contains(NpcEventConfig_TNpcEventEndCondition.TNEEC_MAIN_ACTOR))
            {
                Config.EndCondition.GetListRef().Add(NpcEventConfig_TNpcEventEndCondition.TNEEC_MAIN_ACTOR);
            }

            //默认添加日志标签1001
            if (Config.LogMark == 0)
            {
                SetConfigValue(nameof(Config.LogMark), 1001);
            }
        }

        /// <summary>
        /// 演员是否存在
        /// </summary>
        /// <param name="actorIndex"></param>
        /// <returns></returns>
        public bool IsActorInEvent(int actorIndex)
        {
            var actorNodes = GetNextNodes<NpcEventLinkConfigNode>(typeof(ConfigPortType_NpcEventLinkConfig));
            if(actorIndex + 1 <= actorNodes.Count)
            {
                return true;
            }
            return false;
        }

        #region 表演
        [Sirenix.OdinInspector.ShowInInspector, FoldoutGroup("表演组"), LabelText("开场表演（并轴）"), HideReferenceObjectPicker]
        [OnValueChanged("OnChangedBeginPerformanceGroup", true)]
        [ListDrawerSettings(CustomAddFunction = "OnAddPerformanceGroup")]
        private List<TableSelectData> beginPerformanceGroup = new List<TableSelectData>();

        public void OnChangedBeginPerformanceGroup()
        {
            var groupList = beginPerformanceGroup?.Select(c => c.ID)?.ToList() ?? default;
            SetConfigValue(nameof(Config.BeginPerformanceGroupID), groupList);
        }

        [Sirenix.OdinInspector.ShowInInspector, FoldoutGroup("表演组"), LabelText("待机表演（单轴）"), HideReferenceObjectPicker]
        [OnValueChanged("OnChangedStandbyPerformanceGroup", true)]
        [ListDrawerSettings(CustomAddFunction = "OnAddPerformanceGroup")]
        private List<TableSelectData> standbyPerformanceGroup = new List<TableSelectData>();

        public void OnChangedStandbyPerformanceGroup()
        {
            var groupList = standbyPerformanceGroup?.Select(c => c.ID)?.ToList() ?? default;
            SetConfigValue(nameof(Config.StandbyPerformanceGroupID), groupList);
        }

        private TableSelectData OnAddPerformanceGroup()
        {
            return new TableSelectData(typeof(MapEventPerformanceGroupConfig).FullName, 0);
        }
        #endregion
    }
}

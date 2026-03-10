using GraphProcessor;
using HotFix.Game.MapEvent.Performance;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// npcevent自定义节点
    /// </summary>
    public partial class MapEventStoryConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][剧情][{Config.TriggerType.GetDescription(false)}]");
        }

        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            TriggerType = Config.TriggerType;
            if (TriggerType == TEventStoryTriggerType.TEventStoryTriggerType_EventPos)
            {
                if (Config.IntParams1?.Count == 2)
                {
                    Distance = Config.IntParams1[1];
                }
            }
            else if (TriggerType == TEventStoryTriggerType.TEventStoryTriggerType_Exit)
            {

            }

            OnRefreshCustomName();

            //剧情出口
            performanceGroup.Clear();
            Config.PerformanceGroupID?.ForEach(groupID =>
            {
                var tableData = new TableSelectData(typeof(MapEventPerformanceGroupConfig).FullName, groupID);
                tableData.OnSelectedID();
                performanceGroup.Add(tableData);
            });
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("触发类型"), HideReferenceObjectPicker]
        [FoldoutGroup("触发相关")]
        [OnValueChanged("OnTriggerTypeChanged", true), ValueDropdown("@TableDR.EnumUtility.VD_TEventStoryTriggerType")]
        public TEventStoryTriggerType TriggerType { get; private set; }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("与玩家距离"), ShowIf("@TriggerType == TEventStoryTriggerType.TEventStoryTriggerType_EventPos")]
        [FoldoutGroup("触发相关")]
        [OnValueChanged("OnTriggerTypeChanged", true)]
        public int Distance;

        private void OnTriggerTypeChanged()
        {
            SetConfigValue(nameof(Config.TriggerType), TriggerType);

            if (TriggerType == TEventStoryTriggerType.TEventStoryTriggerType_EventPos)
            {
                SetConfigValue(nameof(Config.IntParams1), new List<int> { 0, Distance });
            }
            else if(TriggerType == TEventStoryTriggerType.TEventStoryTriggerType_Exit)
            {
                SetConfigValue(nameof(Config.IntParams1), default);
            }

            OnRefreshCustomName();
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("表演组（并轴）"), FoldoutGroup("表演相关"), HideReferenceObjectPicker]
        [OnValueChanged("OnChangedPerformanceGroup", true)]
        [ListDrawerSettings(CustomAddFunction = "OnAddPerformanceGroup")]
        private List<TableSelectData> performanceGroup = new List<TableSelectData>();

        public void OnChangedPerformanceGroup()
        {
            var groupList = performanceGroup?.Select(c => c.ID)?.ToList() ?? default;
            SetConfigValue(nameof(Config.PerformanceGroupID), groupList);
        }

        private TableSelectData OnAddPerformanceGroup()
        {
            return new TableSelectData(typeof(MapEventPerformanceGroupConfig).FullName, 0);
        }
    }
}

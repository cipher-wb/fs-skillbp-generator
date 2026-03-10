using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// MEGFT_AwardDrop
    /// </summary>
    public class MapEventGeneralFuncConfigNode_AwardDrop : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;
        
        private HashSet<int> dropIDSet = new HashSet<int>();

        public MapEventGeneralFuncConfigNode_AwardDrop(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 奖励对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("奖励对象")]
        [OnValueChanged("OnAwardDropTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAwardDropTargetsAdd")]
        public List<MapEventTarget> AwardDropTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAwardDropTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Player);
        }

        private void OnAwardDropTargetsChanged()
        {
            baseNode.SaveConfigTarget1(AwardDropTargets);

            CheckError();
        }
        #endregion

        #region 奖励掉落
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落设置")]
        [OnValueChanged("OnDropDataChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddDropData")]
        public List<TableSelectFilterData> DropTableDatas { get; private set; } = new();

        public TableSelectFilterData OnAddDropData()
        {
            return new TableSelectFilterData(typeof(DropConfig).FullName, 0);
        }

        private void OnDropDataChanged()
        {
            var dropIDs= new List<int>();
            DropTableDatas?.ForEach(dropData =>
            {
                if (dropData.ID > 0)
                {
                    dropIDs.Add(dropData.ID);   
                }
            });

            baseNode.Config?.ExSetValue("IntParams1", dropIDs);

            CheckError();
        }
        #endregion

        #region 掉落类型
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落类型")]
        [OnValueChanged("OnDropTypeChanged", true), DelayedProperty]
        public TDropInfoPushType DropType { get; private set; } = TDropInfoPushType.TD_NoTips;

        private void OnDropTypeChanged()
        {
            baseNode.Config?.ExSetValue("IntParams2", new List<int> { (int)DropType });

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetOnlyOne(AwardDropTargets);

            if (DropTableDatas.Count == 0)
            {
                baseNode.InspectorError += $"【掉落列表为空】\n";
            }
            // DropTableDatas?.ForEach((dropData) =>
            // {
            //     var dropConfig = DropConfigManager.Instance.GetGroupConfigs(dropData.ID);
            //     if (dropConfig == default || dropConfig.Count == 0)
            //     {
            //         baseNode.InspectorError += $"【掉落配置不存在 {dropData.ID}】\n";
            //     }
            // });

            baseNode.AddInspectorErrorDropType(DropType);
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, AwardDropTargets);

            //IntParams1
            DropTableDatas.Clear();
            baseNode.Config?.IntParams1?.ForEach(dropID =>
            {
                if (dropID > 0)
                {
                    DropTableDatas.Add(new TableSelectFilterData(typeof(DropConfig).FullName, dropID));   
                }
            });

            //IntParams2
            if(baseNode?.Config?.IntParams2?.Count > 0)
            {
                DropType = (TDropInfoPushType)baseNode.Config.IntParams2[0];
            }
        }

        public void SetDefault()
        {
            AwardDropTargets.Clear();
            AwardDropTargets.Add(OnAwardDropTargetsAdd());
            baseNode.SaveConfigTarget1(AwardDropTargets);

            DropType = TDropInfoPushType.TD_Window;
            baseNode.SetConfigValue(nameof(baseNode.Config.IntParams2), new List<int> { (int)DropType });
        }
    }
}

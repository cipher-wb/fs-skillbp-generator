using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_ItemTransfer : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_ItemTransfer(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 失去道具对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("失去道具对象")]
        [OnValueChanged("OnChangedTargetsForm", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddTargetsForm")]
        public List<MapEventTarget> TargetsForm { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddTargetsForm()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedTargetsForm()
        {
            baseNode.SaveConfigTarget1(TargetsForm);

            CheckError();
        }
        #endregion

        #region Target2 获得道具对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("获得道具对象")]
        [OnValueChanged("OnChangedTargetsTo", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddTargetsTo")]
        public List<MapEventTarget> TargetsTo { get; private set; } = new List<MapEventTarget>();

        public MapEventTarget OnAddTargetsTo()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Player);
        }

        private void OnChangedTargetsTo()
        {
            baseNode.SaveConfigTarget2(TargetsTo);

            CheckError();
        }
        #endregion

        #region 设置道具
        /// <summary>
        /// 设置道具
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("设置")]
        [OnValueChanged("OnChangedTransferItemData", true), DelayedProperty]
        public TransferItemData TransferItemData { get; private set; } = new TransferItemData();

        private void OnChangedTransferItemData()
        {
            baseNode.Config?.ExSetValue("IntParams1", TransferItemData.ToIntParams1());

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetOnlyOne(TargetsForm);

            baseNode.AddInspectorErrorTargetOnlyOne(TargetsTo);

            baseNode.AddInspectorErrorTargetSame(TargetsForm, TargetsTo);

            if (TransferItemData.TransferType == TransferItemType.SpecialItem || TransferItemData.TransferType == TransferItemType.SpecialQuestSubmitItem)
            {
                baseNode.AddInspectorErrorTableNotSelect(TransferItemData.ItemTable);
            }

            if (TransferItemData.ItemCount <= 0)
            {
                baseNode.InspectorError += $"【道具数量错误】\n";
            }
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, TargetsForm);

            //Target2
            baseNode.RestoreTargets(baseNode.Config?.Target2, TargetsTo);

            //IntParams1
            TransferItemData = new TransferItemData(baseNode.Config.IntParams1);
        }

        public void SetDefault()
        {
            TargetsForm.Clear();
            TargetsForm.Add(OnAddTargetsForm());
            baseNode.SaveConfigTarget1(TargetsForm);

            TargetsTo.Clear();
            TargetsTo.Add(OnAddTargetsTo());
            baseNode.SaveConfigTarget2(TargetsTo);
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_PlayerAssetType : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_PlayerAssetType(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region 资产设置
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("资产")]
        [OnValueChanged("OnChangePlayerAssetData", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddPlayerAssetData")]
        public List<STPlayerAssetData> PlayerAssetDatas { get; private set; } = new List<STPlayerAssetData>();

        private STPlayerAssetData OnAddPlayerAssetData()
        {
            return new STPlayerAssetData(TPlayerAssetType.TPAT_STEMINA,  0);
        }

        private void OnChangePlayerAssetData()
        {
            var dyList = new List<GFDynamic>();
            PlayerAssetDatas?.ForEach(data =>
            {
                var dyData = new GFDynamic();
                dyData.ExSetValue("DynmaicInt1", (int)data.AssetType);
                dyData.ExSetValue("DynmaicInt2", data.ChangeValue);
                dyList.Add(dyData);
            });
            baseNode.Config?.ExSetValue("DynamicClass1", dyList);

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(PlayerAssetDatas.Count == 0)
            {
                baseNode.InspectorError += "【参数列表为空】";
            }

            PlayerAssetDatas?.ForEach(data =>
            {
                if(data.ChangeValue == 0)
                {
                    baseNode.InspectorError += "【数值=0】";
                }
            });
        }

        public void ConfigToData()
        {
            PlayerAssetDatas.Clear();
            baseNode.Config.DynamicClass1?.ForEach(gfData =>
            {
                var tableData = new STPlayerAssetData((TPlayerAssetType)gfData.DynmaicInt1, gfData.DynmaicInt2);
                PlayerAssetDatas.Add(tableData);
            });
        }

        public void SetDefault()
        {

        }
    }
}

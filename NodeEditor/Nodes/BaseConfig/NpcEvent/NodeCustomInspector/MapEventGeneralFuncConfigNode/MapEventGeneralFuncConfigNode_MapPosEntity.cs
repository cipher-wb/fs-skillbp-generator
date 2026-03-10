using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapPosEntityData
    {
        #region IntParams1
        [ShowInInspector, HideReferenceObjectPicker, LabelText("MapPosConfig")]
        public TableSelectData TableData { get; private set; } = new TableSelectData(typeof(MapPosConfig).FullName, 0);

        [ValueDropdown("@MapEventGeneralFuncConfigNode.vd_MapPosEntityOptType")]
        public SymbolType SymbolType;
        #endregion

        public MapPosEntityData(int mapPosID, SymbolType symbolType)
        {
            TableData = new TableSelectData(typeof(MapPosConfig).FullName, mapPosID);
            TableData.OnSelectedID();

            SymbolType = symbolType;
        }

        public GFDynamic ToGFDynamic()
        {
            return new GFDynamic(TableData.ID, (int)SymbolType);
        }

        public void SetDefault()
        {

        }
    }

    /// <summary>
    /// MEGFT_AwardDrop
    /// </summary>
    public class MapEventGeneralFuncConfigNode_MapPosEntity : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("地图物件")]
        [OnValueChanged("OnChangedMapPosEntityData", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddMapPosEntityData")]
        public List<MapPosEntityData> MapPosEntityDatas = new List<MapPosEntityData>();

        public MapEventGeneralFuncConfigNode_MapPosEntity(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        private MapPosEntityData OnAddMapPosEntityData()
        {
            return new MapPosEntityData(0, SymbolType.Add);
        }

        private void OnChangedMapPosEntityData()
        {
            var dyList = new List<GFDynamic>();
            MapPosEntityDatas?.ForEach(data =>
            {
                var dyData = new GFDynamic();
                dyData.ExSetValue("DynmaicInt1", data.TableData.ID);
                dyData.ExSetValue("DynmaicInt2", (int)data.SymbolType);
                dyList.Add(dyData);
            });
            baseNode.Config?.ExSetValue("DynamicClass1", dyList);

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            MapPosEntityDatas?.ForEach(data =>
            {
                baseNode.AddInspectorErrorTableNotSelect(data.TableData);
            });
        }

        public void ConfigToData()
        {
            MapPosEntityDatas.Clear();
            baseNode.Config.DynamicClass1?.ForEach(gfData =>
            {
                var tableData = new MapPosEntityData(gfData.DynmaicInt1, (SymbolType)gfData.DynmaicInt2);
                MapPosEntityDatas.Add(tableData);
            });
        }

        public void SetDefault()
        {

        }
    }
}

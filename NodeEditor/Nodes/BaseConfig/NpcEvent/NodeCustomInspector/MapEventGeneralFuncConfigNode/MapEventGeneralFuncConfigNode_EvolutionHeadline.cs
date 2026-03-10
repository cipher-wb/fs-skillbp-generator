using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    [LabelText("数据")]
    public class AddHeadLineData
    {
        [LabelText("词条表"), HideReferenceObjectPicker]
        public TableSelectData HeadLineTable;

        [LabelText("城市表(0=玩家当前的)"), HideReferenceObjectPicker]
        public TableSelectData CityTable;

        [LabelText("词条相关演员"), HideReferenceObjectPicker]
        [ListDrawerSettings(CustomAddFunction = "OnAddHeadLineTargets")]
        public List<MapEventTarget> AddHeadLineTargets = new List<MapEventTarget>();

        public AddHeadLineData(int headLineID, int cityID, List<MapEventTarget> actorTargets)
        {
            HeadLineTable = new TableSelectData(typeof(EvolutionEntryConfig).FullName, headLineID);
            HeadLineTable.OnSelectedID();

            CityTable = new TableSelectData(typeof(FairyHouseConfig).FullName, cityID);
            CityTable.OnSelectedID();

            AddHeadLineTargets = actorTargets;
        }

        public AddHeadLineData(int headLineID, int cityID, IReadOnlyList<int> actorIndexs)
        {
            HeadLineTable = new TableSelectData(typeof(EvolutionEntryConfig).FullName, headLineID);
            HeadLineTable.OnSelectedID();

            CityTable = new TableSelectData(typeof(FairyHouseConfig).FullName, cityID);
            CityTable.OnSelectedID();

            AddHeadLineTargets = new List<MapEventTarget>();
            actorIndexs?.ForEach(index =>
            {
                if (index == 0)
                {
                    AddHeadLineTargets.Add(new MapEventTarget(MapEventTargetType.MapEventTargetType_Leader));
                }
                else
                {
                    AddHeadLineTargets.Add(new MapEventTarget(MapEventTargetType.MapEventTargetType_SpecificActor, index));
                }
            });
        }

        private MapEventTarget OnAddHeadLineTargets()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Leader);
        }

        public List<int> ToDynmaicListInt1()
        {
            var intList = new List<int>();
            AddHeadLineTargets?.ForEach(target =>
            {
                if(target.TargetType == MapEventTargetType.MapEventTargetType_Leader)
                {
                    intList.Add(0);
                }
                else if(target.TargetType == MapEventTargetType.MapEventTargetType_SpecificActor)
                {
                    intList.Add(target.TargetIndex);
                }
            });

            return intList;
        }
    }

    public class MapEventGeneralFuncConfigNode_EvolutionHeadline : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_EvolutionHeadline(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region 增加词条
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("增加词条")]
        [OnValueChanged("OnAddHeadlineDataChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddHeadlineTableDataAdd")]
        public List<AddHeadLineData> AddHeadlineTableDatas { get; private set; } = new List<AddHeadLineData>();

        private AddHeadLineData OnAddHeadlineTableDataAdd()
        {
            return new AddHeadLineData(0,0, new List<MapEventTarget>());
        }    

        private void OnAddHeadlineDataChanged()
        {
            var dyList = new List<GFDynamic>();
            AddHeadlineTableDatas?.ForEach(data =>
            {
                var dyData = new GFDynamic();
                dyData.ExSetValue("DynmaicInt1", data.HeadLineTable.ID);
                dyData.ExSetValue("DynmaicInt2", data.CityTable.ID);
                dyData.ExSetValue("DynmaicListInt1", data.ToDynmaicListInt1());
                dyList.Add(dyData);
            });

            baseNode.Config?.ExSetValue("DynamicClass1", dyList);

            CheckError();
        }
        #endregion

        #region 减少头条
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("删除头条")]
        [OnValueChanged("OnReduceHeadlineDataChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnReduceHeadlineTableDataAdd")]
        public List<TableSelectData> ReduceHeadlineTableDatas { get; private set; } = new List<TableSelectData>();

        private TableSelectData OnReduceHeadlineTableDataAdd()
        {
            return new TableSelectData(typeof(EvolutionHeadlineConfig).FullName, 0);
        }

        private void OnReduceHeadlineDataChanged()
        {
            var tableDatas = new List<int>();
            ReduceHeadlineTableDatas?.ForEach(table =>
            {
                tableDatas.Add(table.ID);
            });

            baseNode.Config?.ExSetValue("IntParams1", tableDatas);

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(AddHeadlineTableDatas.Count == 0 && ReduceHeadlineTableDatas.Count == 0)
            {
                baseNode.InspectorError += "【词条和头条都没有配置】";
            }

            AddHeadlineTableDatas?.ForEach(addLine =>
            {
                baseNode.AddInspectorErrorTableNotSelect(addLine.HeadLineTable);
            });

            ReduceHeadlineTableDatas?.ForEach(reduceLine =>
            {
                baseNode.AddInspectorErrorTableNotSelect(reduceLine);
            });
        }

        public void ConfigToData()
        {
            //增加词条
            AddHeadlineTableDatas.Clear();
            baseNode.Config.DynamicClass1?.ForEach(gfData =>
            {
                var tableData = new AddHeadLineData(gfData.DynmaicInt1, gfData.DynmaicInt2, gfData.DynmaicListInt1);
                AddHeadlineTableDatas.Add(tableData);
            });

            //减少头条
            ReduceHeadlineTableDatas.Clear();
            baseNode.Config.IntParams1?.ForEach(tableID =>
            {
                var tableData = new TableSelectData(typeof(EvolutionHeadlineConfig).FullName, tableID);
                tableData.OnSelectedID();
                ReduceHeadlineTableDatas.Add(tableData);
            });
        }

        public void SetDefault()
        {

        }
    }
}

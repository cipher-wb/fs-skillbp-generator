using Funny.Base.Utils;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_Destiny : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_Destiny(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region 气运
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("气运ID")]
        [OnValueChanged("OnChangedDestinys", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddDestinys")]
        public List<TableSelectData> Destinys { get; private set; } = new List<TableSelectData>();

        private TableSelectData OnAddDestinys()
        {
            return new TableSelectData(typeof(DestinyConfig).FullName, 0);
        }

        private void OnChangedDestinys()
        {
            var tableDatas = new List<int>();
            Destinys?.ForEach(table =>
            {
                tableDatas.Add(table.ID);
            });

            baseNode.Config?.ExSetValue("IntParams1", tableDatas);

            CheckError();
        }
        #endregion

        #region Target1 添加对象
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("添加对象")]
        [OnValueChanged("OnChangedTargets", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddTargets")]
        public List<MapEventTarget> Targets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddTargets()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedTargets()
        {
            baseNode.SaveConfigTarget1(Targets);

            CheckError();
        }
        #endregion

        public void ConfigToData()
        {
            //指定气运
            Destinys.Clear();
            baseNode.Config.IntParams1?.ForEach(tableID =>
            {
                var tableData = new TableSelectData(typeof(DestinyConfig).FullName, tableID);
                tableData.OnSelectedID();
                Destinys.Add(tableData);
            });

            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, Targets);
        }

        public void SetDefault()
        {

        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTableNotSelect(Destinys);
            baseNode.AddInspectorErrorTargetIsEmpty(Targets);
        }
    }
}

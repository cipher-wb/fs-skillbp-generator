using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_CampFavor : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_CampFavor(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        #region Target1 改变对象列表
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("改变对象列表")]
        [OnValueChanged("OnCampFavorTargetsChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnCampFavorTargetsAdd")]
        public List<MapEventTarget> CampFavorTargets { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnCampFavorTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Player);
        }

        private void OnCampFavorTargetsChanged()
        {
            baseNode.SaveConfigTarget1(CampFavorTargets);

            CheckError();
        }
        #endregion

        #region Target2 取这个演员的势力
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("相对演员的势力")]
        [OnValueChanged("OnChangedRelativeTarget", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddRelativeTarget")]
        public List<MapEventTarget> RelativeTarget { get; private set; } = new List<MapEventTarget>();

        private MapEventTarget OnAddRelativeTarget()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);
        }

        private void OnChangedRelativeTarget()
        {
            baseNode.SaveConfigTarget2(RelativeTarget);

            CheckError();
        }
        #endregion

        #region 势力表设置
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("势力表")]
        [OnValueChanged("OnCampTableChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnCampTableAdd")]
        public List<TableSelectData> CampTableData { get; private set; } = new List<TableSelectData>();

        private TableSelectData OnCampTableAdd()
        {
            return new TableSelectData(typeof(FairyHouseConfig).FullName, 0);
        }

        private void OnCampTableChanged()
        {
            var tableDatas = new List<int>();
            CampTableData?.ForEach(table =>
            {
                tableDatas.Add(table.ID);
            });

            baseNode.Config?.ExSetValue("IntParams1", tableDatas);

            CheckError();
        }
        #endregion

        #region 好感度设置
        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("好感度")]
        [OnValueChanged("OnChangeCampFavor", true), DelayedProperty]
        public ChangeFavorData CampFavorData { get; private set; }

        public void OnChangeCampFavor()
        {
            baseNode.Config?.ExSetValue("IntParams2", new List<int>()
            {
                (int)CampFavorData.ChangeType,
                CampFavorData.ChangeValue,
            });

            CheckError();
        }
        #endregion

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorTargetOnlyOne(CampFavorTargets);

            baseNode.AddInspectorErrorFavor(CampFavorData);

            if(RelativeTarget.Count == 0 && CampTableData.Count == 0)
            {
                baseNode.InspectorError += "【相对目标】和【势力表】都为空\n";
            }
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, CampFavorTargets);

            //Target2
            baseNode.RestoreTargets(baseNode.Config?.Target2, RelativeTarget);

            //IntParams1
            CampTableData.Clear();
            baseNode.Config.IntParams1?.ForEach(tableID =>
            {
                var tableData = new TableSelectData(typeof(FairyHouseConfig).FullName, tableID);
                tableData.OnSelectedID();
                CampTableData.Add(tableData);
            });

            //IntParams2
            if (baseNode.Config?.IntParams2?.Count == 2)
            {
                CampFavorData = new ChangeFavorData((SymbolType)baseNode.Config.IntParams2[0], baseNode.Config.IntParams2[1]);
            }
        }

        public void SetDefault()
        {
            CampFavorTargets.Clear();
            CampFavorTargets.Add(OnCampFavorTargetsAdd());
            baseNode.SaveConfigTarget1(CampFavorTargets);

            RelativeTarget.Clear();
            RelativeTarget.Add(OnAddRelativeTarget());
            baseNode.SaveConfigTarget2(RelativeTarget);

            CampFavorData = new ChangeFavorData(SymbolType.Add, 10);
            OnChangeCampFavor();
        }
    }
}

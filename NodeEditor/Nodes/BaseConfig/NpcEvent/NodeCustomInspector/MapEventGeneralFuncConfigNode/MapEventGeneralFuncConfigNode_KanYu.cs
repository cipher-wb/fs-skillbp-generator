using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class KanYuInfo
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        #region IntParams1
        [ShowInInspector, HideReferenceObjectPicker, LabelText("堪舆类型"), GUIColor(0f, 1f, 0f, 1f)]
        public TMapEventKanYuType KanYuType { get; private set; }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("天气"), ShowIf("@this.KanYuType == TMapEventKanYuType.TMapEventKanYuType_TianQi")]
        public TableSelectData TableData { get; private set; } = new TableSelectData(typeof(WeatherConfig).FullName, 0);

        [ShowInInspector, HideReferenceObjectPicker, LabelText("五行"), ShowIf("@this.KanYuType == TMapEventKanYuType.TMapEventKanYuType_WuXing")]
        public RandomPointState WuXingState { get; private set; }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("灵气"), ShowIf("@this.KanYuType == TMapEventKanYuType.TMapEventKanYuType_LingQi")]
        public int LingQi { get; private set; }
        #endregion

        public KanYuInfo(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public List<int> ToIntParams1()
        {
            switch(KanYuType)
            {
                case TMapEventKanYuType.TMapEventKanYuType_LingQi:
                    {
                        return new List<int> { (int)KanYuType, LingQi };
                    }
                case TMapEventKanYuType.TMapEventKanYuType_WuXing:
                    {
                        return new List<int> { (int)KanYuType, (int)WuXingState };
                    }
                case TMapEventKanYuType.TMapEventKanYuType_TianQi:
                    {
                        return new List<int> { (int)KanYuType, TableData.ID };
                    }
                default:
                    return new List<int> { -1, 0 };
            }
        }

        public void ConfigToData()
        {
            if (baseNode.Config?.IntParams1?.Count >= 1)
            {
                KanYuType = (TMapEventKanYuType)baseNode.Config.IntParams1[0];
            }


            switch (KanYuType)
            {
                case TMapEventKanYuType.TMapEventKanYuType_LingQi:
                    {
                        if (baseNode.Config?.IntParams1?.Count >= 2)
                        {
                            LingQi = baseNode.Config.IntParams1[1];
                        }
                    }
                    break;
                case TMapEventKanYuType.TMapEventKanYuType_WuXing:
                    {
                        if (baseNode.Config?.IntParams1?.Count >= 2)
                        {
                            WuXingState = (RandomPointState)baseNode.Config.IntParams1[1];
                        }
                    }
                    break;
                case TMapEventKanYuType.TMapEventKanYuType_TianQi:
                    {
                        if (baseNode.Config?.IntParams1?.Count >= 2)
                        {
                            TableData = new TableSelectData(typeof(WeatherConfig).FullName, baseNode.Config.IntParams1[1]);
                            TableData.OnSelectedID();
                        }
                    }
                    break;
            }
        }

        public void SetDefault()
        {
            KanYuType = TMapEventKanYuType.TMapEventKanYuType_None;
        }
    }

    /// <summary>
    /// MEGFT_KanYu
    /// </summary>
    public class MapEventGeneralFuncConfigNode_KanYu : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        [ShowInInspector, HideReferenceObjectPicker, LabelText("堪舆信息")]
        [OnValueChanged("OnChangedKanYuInfo", true), DelayedProperty]
        public KanYuInfo KanYuInfo;

        public MapEventGeneralFuncConfigNode_KanYu(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        private void OnChangedKanYuInfo()
        {
            baseNode.Config?.ExSetValue("IntParams1", KanYuInfo.ToIntParams1());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (KanYuInfo != default)
            {
                if (KanYuInfo.KanYuType == TMapEventKanYuType.TMapEventKanYuType_TianQi)
                {
                    baseNode.AddInspectorErrorTableNotSelect(KanYuInfo.TableData);
                }
            }
        }

        public void ConfigToData()
        {
            KanYuInfo = new KanYuInfo(baseNode);
            KanYuInfo.ConfigToData();
        }

        public void SetDefault()
        {
            KanYuInfo ??= new KanYuInfo(baseNode);
            KanYuInfo.SetDefault();
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class QiQiuInfo
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        #region IntParams1
        [ShowInInspector, HideReferenceObjectPicker, LabelText("指定道具")]
        public TableSelectData TableData { get; private set; } = new TableSelectData(typeof(ItemConfig).FullName, 0);

        [ShowInInspector, HideReferenceObjectPicker, LabelText("概率(1-100)"), MinValue(1), MaxValue(100)]
        public int Probability { get; private set; }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("跑马灯文本")]
        [ValueDropdown("OnMarqueeAdd")]
        public int MarqueeID { get; private set; }

        private HashSet<int> marqueeIDSet = new HashSet<int>();

        private IEnumerable<ValueDropdownItem> OnMarqueeAdd()
        {
            marqueeIDSet.Clear();

            var itemMarquees = ItemMarqueeConfigManager.Instance.ItemArray.Items;
            foreach (var marqueesItem in itemMarquees)
            {
                if (!marqueeIDSet.Contains(marqueesItem.GroupID))
                {
                    marqueeIDSet.Add(marqueesItem.GroupID);
                    yield return new ValueDropdownItem(marqueesItem.GroupID.ToString(), marqueesItem.GroupID);
                }
            }
        }
        #endregion

        public QiQiuInfo(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        public List<int> ToIntParams1()
        {
            return new List<int> { TableData.ID, Probability, MarqueeID };
        }

        public void ConfigToData()
        {
            if (baseNode.Config?.IntParams1?.Count > 0)
            {
                TableData = new TableSelectData(typeof(ItemConfig).FullName, baseNode.Config.IntParams1[0]);
                TableData.OnSelectedID();
            }

            if (baseNode.Config?.IntParams1?.Count > 1)
            {
                Probability = baseNode.Config.IntParams1[1];
            }

            if (baseNode.Config?.IntParams1?.Count > 2)
            {
                MarqueeID = baseNode.Config.IntParams1[2];
            }
        }

        public void SetDefault()
        {
            Probability = 50;
        }
    }

    /// <summary>
    /// MEGFT_AwardDrop
    /// </summary>
    public class MapEventGeneralFuncConfigNode_QiQiu : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        [ShowInInspector, HideReferenceObjectPicker, LabelText("祈求信息")]
        [OnValueChanged("OnChangedQiQiuInfo", true), DelayedProperty]
        public QiQiuInfo QiQiuInfo;

        public MapEventGeneralFuncConfigNode_QiQiu(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        private void OnChangedQiQiuInfo()
        {
            baseNode.Config?.ExSetValue("IntParams1", QiQiuInfo.ToIntParams1());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (QiQiuInfo != default)
            {
                baseNode.AddInspectorErrorTableNotSelect(QiQiuInfo.TableData);
            }
        }

        public void ConfigToData()
        {
            QiQiuInfo = new QiQiuInfo(baseNode);
            QiQiuInfo.ConfigToData();
        }

        public void SetDefault()
        {
            QiQiuInfo ??= new QiQiuInfo(baseNode);
            QiQiuInfo.SetDefault();
        }
    }
}

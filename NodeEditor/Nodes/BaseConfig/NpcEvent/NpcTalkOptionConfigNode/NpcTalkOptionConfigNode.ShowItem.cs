using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// NpcTalkOptionConfigNode定义类
    /// </summary>
    public partial class NpcTalkOptionConfigNode
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("展示类型"), HideReferenceObjectPicker]
        [FoldoutGroup("奖励显示相关", true, order: 2)]
        [OnValueChanged("OnShowItemTypeChanged", true)]
        private TShowItemType showItemType = TShowItemType.TShowItemType_Hand;

        private void OnShowItemTypeChanged()
        {
            SetConfigValue(nameof(Config.ShowItemsType), (int)showItemType);
        }

        [Sirenix.OdinInspector.ShowInInspector, LabelText("奖励道具"), HideReferenceObjectPicker]
        [FoldoutGroup("奖励显示相关", true, order: 2)]
        [OnValueChanged("OnShowItemListChanged", true), DelayedProperty]
        [ListDrawerSettings(CustomAddFunction = "OnAddShowItemTable")]
        private List<TableSelectData> showItemList = new List<TableSelectData>();

        private void OnShowItemListChanged()
        {
            List<int> showItems = new List<int>();
            showItemList?.ForEach(item => showItems.Add(item.ID));
            SetConfigValue(nameof(Config.ShowItems), showItems);
        }

        private TableSelectData OnAddShowItemTable()
        {
            var tableFullName = typeof(ItemConfig).FullName;
            return new TableSelectData(tableFullName, 0);
        }

        private void ConfigToShowItem()
        {
            //showItemType
            showItemType = Config?.NpcEventDialogOptionType == TNpcEventDialogOptionType.TNEDOT_NULL ? TShowItemType.TShowItemType_GeneralFunc : Config.ShowItemsType;
            SetConfigValue(nameof(Config.ShowItemsType), (int)showItemType);

            //ShowItems
            showItemList?.Clear();
            Config?.ShowItems?.ForEach(id =>
            {
                var tableData = new TableSelectData(typeof(ItemConfig).FullName, (int)id);
                tableData.OnSelectedID();
                showItemList.Add(tableData);
            });
        }
    }
}

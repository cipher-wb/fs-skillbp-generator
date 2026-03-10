using CSGameShare.CSEffectAttribute;
using GraphProcessor;
using Newtonsoft.Json;
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
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        /// <summary>
        /// 转换列表
        /// </summary>
        private List<long> convertLongList = new List<long>();

        private void OnLongChanged()
        {
            convertLongList.Clear();

            if (IsShowGameTag) { gameTagList?.ForEach(converType => convertLongList.Add(converType)); }

            SetConfigValue(nameof(Config.FormulaD), convertLongList);
        }

        private void RestoreFormulaD()
        {
            if (IsShowGameTag) { gameTagList.Clear(); Config?.FormulaD?.ForEach(id => gameTagList.Add(id)); }
        }

        /// <summary>
        /// 游戏标签
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("游戏标签"), ValueDropdown("GetGameTagItem", DoubleClickToConfirm = true)]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnLongChanged", true), ShowIf("@IsShowGameTag")]
        private List<long> gameTagList = new List<long>();
        private bool IsShowGameTag => Config?.ConditionType == MapEventConditionType.MECT_GAMETAG;

        private IEnumerable<ValueDropdownItem> GetGameTagItem()
        {
            foreach (var item in GameTagConfigManager.Instance.ItemArray.Items)
            {
                yield return new ValueDropdownItem($"{item.TagId}_{item.TagLevel}_{item.TagName}_{item.TagType.GetDescription(false)}", GameTagData.CombTagData(item.TagId, item.TagLevel));
            }
        }
    }
}

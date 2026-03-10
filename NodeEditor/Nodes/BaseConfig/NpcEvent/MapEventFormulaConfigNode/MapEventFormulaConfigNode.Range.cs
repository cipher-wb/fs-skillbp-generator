using GameApp.Editor;
using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        [Sirenix.OdinInspector.ShowInInspector, LabelText("范围")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker, ShowIf("@IsFormulaA")]
        [OnValueChanged("OnNpcMatchDataChanged", true), DelayedProperty]
        private List<NpcMatchData> npcMatchDataList = new List<NpcMatchData>();

        public void OnNpcMatchDataChanged()
        {
            List<NpcMatchData> tempList = default;
            npcMatchDataList?.ForEach(npcMatchData =>
            {
                tempList ??= new List<NpcMatchData>();
                tempList.Add(npcMatchData);
            });

            SetConfigValue(nameof(Config.FormulaA), tempList);
        }

        private void RestoreFormulaA()
        {
            npcMatchDataList.Clear();
            Config?.FormulaA?.ForEach(data => npcMatchDataList.Add(data));
        }
    }
}

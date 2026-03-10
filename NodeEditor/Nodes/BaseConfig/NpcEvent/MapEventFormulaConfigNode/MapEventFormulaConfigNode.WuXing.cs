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
        [Sirenix.OdinInspector.ShowInInspector, ShowIf("@IsFormulaC"), LabelText("五行"), HideReferenceObjectPicker]
        [BoxGroup("公式", centerLabel: true, order: 0)]
        [OnValueChanged("OnElementsChanged", true), DelayedProperty]
        private List<ElementsProperty> elementsList = new List<ElementsProperty>();

        public void OnElementsChanged()
        {
            List<ElementsProperty> tempList = default;
            foreach(var element in elementsList)
            {
                tempList ??= new List<ElementsProperty>();
                tempList.Add(element);
            }

            SetConfigValue(nameof(Config.FormulaC), tempList);
        }

        public void RestoreFormulaC()
        {
            //范围
            elementsList.Clear();
            Config?.FormulaC?.ForEach(data => elementsList.Add(data));
        }
    }
}

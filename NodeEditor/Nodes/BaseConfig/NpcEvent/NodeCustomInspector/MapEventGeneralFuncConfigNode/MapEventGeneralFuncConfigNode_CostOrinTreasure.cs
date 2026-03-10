using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// 消耗本命法宝使用次数
    /// </summary>
    public class MapEventGeneralFuncConfigNode_CostOrinTreasure : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_CostOrinTreasure(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("本命法宝")]
        [OnValueChanged("OnChangedOrinTreasure", true), DelayedProperty]
        public TOTForMapEvent OrinTreasure { get; private set; } = TOTForMapEvent.MOT_None;

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (OrinTreasure == TOTForMapEvent.MOT_None)
            {
                baseNode.InspectorError += "【尚未指定本命法宝！】\n";
            }
        }

        private void OnChangedOrinTreasure()
        {
            baseNode.Config?.ExSetValue("IntParams1", new List<int>() { (int)OrinTreasure });

            CheckError();
        }

        public void ConfigToData()
        {
            if (baseNode.Config.IntParams1 != null && baseNode.Config.IntParams1.Count > 0)
            {
                OrinTreasure = (TOTForMapEvent)baseNode.Config.IntParams1[0];
            }
            else
            {
                OrinTreasure = TOTForMapEvent.MOT_None;
            }
        }

        public void SetDefault()
        {
            OnChangedOrinTreasure();
        }
    }
}

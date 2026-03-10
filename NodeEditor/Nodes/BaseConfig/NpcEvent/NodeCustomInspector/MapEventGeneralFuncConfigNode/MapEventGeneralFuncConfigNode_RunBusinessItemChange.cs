using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    /// <summary>
    /// 跑商商品变化
    /// </summary>
    public class MapEventGeneralFuncConfigNode_RunBusinessItemChange : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_RunBusinessItemChange(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("商品变化")]
        [OnValueChanged("OnChangeRunBuisnessItemData", true)]
        public SetRunBuisnessItemData RunBuisnessItemData { get; private set; } = new SetRunBuisnessItemData();

        private void OnChangeRunBuisnessItemData()
        {
            baseNode.Config?.ExSetValue("IntParams1", RunBuisnessItemData.ToIntParams1());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            baseNode.AddInspectorErrorDropType(RunBuisnessItemData.PushType);

            if(RunBuisnessItemData.ValueMin == 0 && RunBuisnessItemData.ValueMax == 0)
            {
                baseNode.InspectorError += $"【最大最小值错误】\n";
            }
        }

        public void ConfigToData()
        {
            RunBuisnessItemData = new SetRunBuisnessItemData(baseNode.Config.IntParams1);
        }

        public void SetDefault()
        {
            baseNode.Config?.ExSetValue("IntParams1", RunBuisnessItemData.ToIntParams1());
        }
    }
}

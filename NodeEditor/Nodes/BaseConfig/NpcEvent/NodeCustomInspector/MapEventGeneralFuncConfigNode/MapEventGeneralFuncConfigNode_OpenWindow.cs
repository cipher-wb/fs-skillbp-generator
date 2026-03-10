using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public class MapEventGeneralFuncConfigNode_OpenWindow : INodeCustomInspector
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_OpenWindow(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("窗口参数")]
        [OnValueChanged("OnWindowParamsChanged", true), DelayedProperty]
        public List<string> WindowParams { get; private set; } = new List<string>();

        private void OnWindowParamsChanged()
        {
            var stringParams = new List<string>();
            WindowParams?.ForEach(param =>
            {
                stringParams.Add(param);
            });

            baseNode.Config?.ExSetValue("StrParams1", stringParams);

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if(WindowParams.Count == 0)
            {
                baseNode.InspectorError += "【参数未填写】";
            }
        }

        public void ConfigToData()
        {
            //StrParams1
            WindowParams.Clear();
            baseNode.Config?.StrParams1?.ForEach(param =>
            {
                WindowParams.Add(param);
            });
        }

        public void SetDefault()
        {
            //StrParams1
            WindowParams.Clear();
        }
    }
}

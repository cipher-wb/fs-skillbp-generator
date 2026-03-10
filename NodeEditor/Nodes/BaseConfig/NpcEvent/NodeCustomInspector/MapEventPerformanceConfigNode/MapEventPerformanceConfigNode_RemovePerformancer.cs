using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeEditor
{
    public class RemovePerformancerData
    {
        [LabelText("自定义ID列表")]
        public List<int> CustomIDList = new List<int>();

        public RemovePerformancerData() { }

        public RemovePerformancerData(string param)
        {
            ToData(param);
        }

        //自定义ID|特效ID|特效类型|坐标X|坐标Y
        public override string ToString()
        {
            return string.Join("|", CustomIDList);
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param)) { return; }

            var split = param.Split('|');
            if (split.Length <= 0) { return; }

            foreach (var customID in split)
            {
                if (int.TryParse(customID, out var customIDInt))
                {
                    CustomIDList.Add(customIDInt);
                }
            }
        }
    }

    public class MapEventPerformanceConfigNode_RemovePerformancer : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_RemovePerformancer(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private RemovePerformancerData perfData = new RemovePerformancerData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new RemovePerformancerData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new RemovePerformancerData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

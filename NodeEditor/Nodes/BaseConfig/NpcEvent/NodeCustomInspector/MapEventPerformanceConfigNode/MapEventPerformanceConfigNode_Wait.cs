using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace NodeEditor
{
    public class PerfWaitData
    {
        [HideReferenceObjectPicker, LabelText("持续时间（毫秒）")]
        public int Duration;

        public PerfWaitData()
        {

        }

        public PerfWaitData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{Duration}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                Duration = 2000;
                return;
            }

            var split = param.Split('|');
            if (split.Length != 1) { return; }

            if (int.TryParse(split[0], out var param1))
            {
                Duration = param1;
            }
        }
    }

    public class MapEventPerformanceConfigNode_Wait : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_Wait(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PerfWaitData perfData = new PerfWaitData();

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
            perfData = new PerfWaitData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PerfWaitData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;
using static NodeEditor.MapEventPerformanceConfigNode;

namespace NodeEditor
{
    public class FadeInOutData
    {
        [LabelText("渐变类型")]
        public FadeType FadeType;

        [LabelText("持续时间")]
        public int Duration;

        public FadeInOutData()
        {

        }

        public FadeInOutData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{(int)FadeType}|{Duration}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                FadeType = FadeType.FadeIn;
                Duration = 1000;
                return;
            }

            var split = param.Split('|');
            if (split.Length != 2) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1))
            {
                return;
            }

            FadeType = (FadeType)param0;
            Duration = param1;
        }
    }

    public class MapEventPerformanceConfigNode_FadeInOut : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_FadeInOut(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数"), DelayedProperty]
        [OnValueChanged("OnParamChanged", true)]
        private FadeInOutData perfData = new FadeInOutData();

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
            perfData = new FadeInOutData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new FadeInOutData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

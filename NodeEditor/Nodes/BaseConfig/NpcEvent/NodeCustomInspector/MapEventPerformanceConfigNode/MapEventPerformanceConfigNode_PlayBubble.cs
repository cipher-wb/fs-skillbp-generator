using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class PlayBubbleData
    {
        /// <summary>
        /// 气泡类型
        /// </summary>
        [LabelText("类型")]
        public TBubbleImageType BubbleType;

        /// <summary>
        /// 持续时间 毫秒
        /// </summary>
        [LabelText("持续时间（毫秒）")]
        public int Duration;

        [LabelText("文本")]
        public string BubbleText;

        /// <summary>
        /// 等待动作播完
        /// </summary>
        [LabelText("等待播完")]
        public bool WaitFinished;

        public PlayBubbleData()
        {

        }

        public PlayBubbleData(string param, string text)
        {
            ToData(param, text);
        }

        public override string ToString()
        {
            return $"{(int)BubbleType}|{Duration}|{(WaitFinished ? 1 : 0)}";
        }

        public void ToData(string param, string text)
        {
            if(string.IsNullOrEmpty(param)) { return; }

            var split = param.Split('|');
            if (split.Length != 3) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1)
                || !int.TryParse(split[2], out var param2))
            {
                return;
            }

            BubbleType = (TBubbleImageType)param0;
            Duration = param1;
            WaitFinished = param2 != 0 ? true : false;
            BubbleText = text;
        }
    }

    public class MapEventPerformanceConfigNode_PlayBubble : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_PlayBubble(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayBubbleData perfData = new PlayBubbleData();

        private void OnParamChanged()
        {
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.TextEditor), perfData.BubbleText);

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;
        }

        public void ConfigToData()
        {
            perfData = new PlayBubbleData(baseNode.Config.Param, baseNode.Config.TextEditor);
        }

        public void SetDefault()
        {
            perfData = new PlayBubbleData(string.Empty, string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

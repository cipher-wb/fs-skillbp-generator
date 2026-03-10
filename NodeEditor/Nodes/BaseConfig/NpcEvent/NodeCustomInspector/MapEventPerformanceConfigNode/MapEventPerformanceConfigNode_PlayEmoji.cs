using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace NodeEditor
{
    public class PlayEmojiData
    {
        /// <summary>
        /// 心情ID
        /// </summary>
        [LabelText("心情ID")]
        public int EmojiID;

        /// <summary>
        /// 持续时间 毫秒
        /// </summary>
        [LabelText("持续时间（毫秒）")]
        public int Duration;

        /// <summary>
        /// 等待播完
        /// </summary>
        [LabelText("等待播完")]
        public bool WaitFinished;

        public PlayEmojiData()
        {

        }

        public PlayEmojiData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{EmojiID}|{Duration}|{(WaitFinished ? 1 : 0)}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param)) { return; }

            var split = param.Split('|');
            if (split.Length != 3) { return; }

            if (!int.TryParse(split[0], out var param0)
                || !int.TryParse(split[1], out var param1)
                || !int.TryParse(split[2], out var param2))
            {
                return;
            }

            EmojiID = param0;
            Duration = param1;
            WaitFinished = param2 != 0 ? true : false;
        }
    }

    public class MapEventPerformanceConfigNode_PlayEmoji : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_PlayEmoji(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlayEmojiData perfData = new PlayEmojiData();

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
            perfData = new PlayEmojiData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PlayEmojiData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

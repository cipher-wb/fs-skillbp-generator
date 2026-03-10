using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public class PlaySoundData
    {
        [HideReferenceObjectPicker, LabelText("音效ID")]
        public TableSelectData VoiceTable;

        public PlaySoundData()
        {

        }

        public PlaySoundData(string param)
        {
            ToData(param);
        }

        public override string ToString()
        {
            return $"{VoiceTable.ID}";
        }

        public void ToData(string param)
        {
            if (string.IsNullOrEmpty(param))
            {
                VoiceTable = new TableSelectData(typeof(VoiceConfig).FullName, 0);
                return;
            }

            var split = param.Split('|');
            if (split.Length != 1) { return; }

            if (!int.TryParse(split[0], out var param0))
            {
                return;
            }

            //获取特效
            VoiceTable = new TableSelectData(typeof(VoiceConfig).FullName, param0);
            VoiceTable.OnSelectedID();
        }
    }

    public class MapEventPerformanceConfigNode_PlaySound : INodeCustomInspector
    {
        private readonly MapEventPerformanceConfigNode baseNode;

        public MapEventPerformanceConfigNode_PlaySound(MapEventPerformanceConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("参数")]
        [OnValueChanged("OnParamChanged", true)]
        private PlaySoundData perfData = new PlaySoundData();

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
            perfData = new PlaySoundData(baseNode.Config.Param);
        }

        public void SetDefault()
        {
            perfData = new PlaySoundData(string.Empty);
            baseNode.Config?.ExSetValue(nameof(baseNode.Config.Param), perfData.ToString());
        }
    }
}

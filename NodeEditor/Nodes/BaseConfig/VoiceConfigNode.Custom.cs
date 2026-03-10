using Sirenix.OdinInspector;
using UnityEditor;

namespace NodeEditor
{
    public partial class VoiceConfigNode
    {
        protected override void OnPreset()
        {
            base.OnPreset();

            // 默认模块-战斗音效
            Config.ExSetValue(nameof(Config.BelongModule), "战斗音效");
            // 默认对象
            Config.ExSetValue(nameof(Config.Content_1P), new TableDR.VoiceConfig_VoiceData());
            // 默认对象
            Config.ExSetValue(nameof(Config.Content_3P), new TableDR.VoiceConfig_VoiceData());
        }

        [Button("打开音效查看器(FMOD Events)", ButtonSizes.Medium)]
        private void OpenFMODEvent()
        {
            EditorApplication.ExecuteMenuItem("FMOD/Event Browser");
        }
    }
}

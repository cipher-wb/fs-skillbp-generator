#if UNITY_EDITOR

namespace TableDR
{
    public partial class ModelConfig
    {
        private bool HideIf_ModelLoopAudio()
        {
            return ModelLoopAudio == 0;
        }

        private bool HideIf_ModelBornAudio()
        {
            return ModelBornAudio == 0;
        }

        private bool HideIf_ModelDeathAudio()
        {
            return ModelDeathAudio == 0;
        }
    }
}

#endif
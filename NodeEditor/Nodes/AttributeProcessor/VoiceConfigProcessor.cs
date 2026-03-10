using TableDR;

namespace NodeEditor
{
    internal sealed class VoiceConfigProcessor : NodeEditorBaseProcessor<VoiceConfig>
    {
        protected override bool ColorIfConditionAction(object obj, string propertyName)
        {
            if (obj is VoiceConfig config)
            {
                switch (propertyName)
                {
                    case nameof(config.Desc):
                        return !string.IsNullOrEmpty(config.Desc);
                    case nameof(config.BelongModule):
                        return !string.IsNullOrEmpty(config.BelongModule);
                }
            }
            return base.ColorIfConditionAction(obj, propertyName);
        }
    }
}


using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class ModelConfigProcessor : NodeEditorBaseProcessor<ModelConfig>
    {
        static class SelfAttributes
        {
            public static TitleGroupAttribute 基础信息 = new TitleGroupAttribute("基础信息", indent: true, order: -6, boldTitle: false);
            public static TitleGroupAttribute 跟随相关 = new TitleGroupAttribute("跟随相关", indent: true, order: -5, boldTitle: false);
            public static TitleGroupAttribute 特效相关 = new TitleGroupAttribute("特效相关", indent: true, order: -4, boldTitle: false);
            public static TitleGroupAttribute 音效相关 = new TitleGroupAttribute("音效相关", indent: true, order: -3, boldTitle: false);
            public static TitleGroupAttribute 角色相关 = new TitleGroupAttribute("角色相关", indent: true, order: -2, boldTitle: false);
            public static TitleGroupAttribute 其他信息 = new TitleGroupAttribute("其他信息", indent: true, order: -1, boldTitle: false);
            public static TitleGroupAttribute 战斗无关信息 = new TitleGroupAttribute("战斗无关信息", indent: true, order: 0, boldTitle: false);

            public static HideIfAttribute HideIf_ModelLoopAudio = new HideIfAttribute("HideIf_ModelLoopAudio");
            public static HideIfAttribute HideIf_ModelBornAudio = new HideIfAttribute("HideIf_ModelBornAudio");
            public static HideIfAttribute HideIf_ModelDeathAudio = new HideIfAttribute("HideIf_ModelDeathAudio");
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                switch (member.Name)
                {
                    case nameof(config.ID):
                    case nameof(config.ModelPath):
                    case nameof(config.ModelAmplify):
                        attributes.Add(SelfAttributes.基础信息);
                        break;
                    case nameof(config.IsFollowAnim):
                    case nameof(config.IsFollowScale):
                        attributes.Add(SelfAttributes.跟随相关);
                        break;
                    case nameof(config.AttachPos):
                    case nameof(config.Flags):
                    case nameof(config.IsDeadDelete):
                    case nameof(config.ViewEventTargetType):
                    case nameof(config.DisappearType):
                        attributes.Add(SelfAttributes.特效相关);
                        break;
                    case nameof(config.ModelLoopAudio):
                    case nameof(config.ModelBornAudio):
                    case nameof(config.ModelDeathAudio):
                        attributes.Add(SelfAttributes.音效相关);
                        break;
                    case nameof(config.ModelLoopAudioSoundType):
                    case nameof(config.ModelLoopAudioStopOnDisable):
                    case nameof(config.ModelLoopAudioFadeOutFrame):
                        attributes.Add(SelfAttributes.音效相关);
                        attributes.Add(SelfAttributes.HideIf_ModelLoopAudio);
                        break;
                    case nameof(config.ModelBornAudioSoundType):
                    case nameof(config.ModelBornAudioStopOnDisable):
                    case nameof(config.ModelBornAudioFadeOutFrame):
                        attributes.Add(SelfAttributes.音效相关);
                        attributes.Add(SelfAttributes.HideIf_ModelBornAudio);
                        break;
                    case nameof(config.ModelDeathAudioSoundType):
                    case nameof(config.ModelDeathAudioStopOnDisable):
                    case nameof(config.ModelDeathAudioFadeOutFrame):
                        attributes.Add(SelfAttributes.音效相关);
                        attributes.Add(SelfAttributes.HideIf_ModelDeathAudio);
                        break;
                    case nameof(config.BodyType):
                    case nameof(config.BodyLevelType):
                    case nameof(config.BodyForceMoveFactor):
                    case nameof(config.FixtureConfigId):
                    case nameof(config.MapCollider):
                    case nameof(config.SelectLevel):
                    case nameof(config.HitEffectAmplify):
                    case nameof(config.BattleStateBaseSpeed):
                    case nameof(config.NormalStateBaseSpeed):
                        attributes.Add(SelfAttributes.角色相关);
                        break;
                    case nameof(config.IsLoadModelSync):
                        attributes.Add(SelfAttributes.其他信息);
                        break;
                    default:
                        //attributes.Add(SelfAttributes.战斗无关信息);
                        attributes.Add(DefaultAttributes.HideIfAttribute_True);
                        break;
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        protected override bool ColorIfConditionAction(object obj, string propertyName)
        {
            if (obj is ModelConfig config)
            {
                switch (propertyName)
                {
                    case nameof(config.ID):
                        return config.ID == 0;
                    case nameof(config.ModelPath):
                        return string.IsNullOrEmpty(config.ModelPath);
                }
            }
            return base.ColorIfConditionAction(obj, propertyName);
        }
    }
}


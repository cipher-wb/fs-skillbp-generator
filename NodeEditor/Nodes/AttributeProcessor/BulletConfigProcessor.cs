using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    internal sealed class BulletConfigProcessor : NodeEditorBaseProcessor<BulletConfig>
    {
        public static class SelfAttributes
        {
            public static TitleGroupAttribute 基础信息 = new TitleGroupAttribute("基础信息", indent: true, boldTitle: false);
            public static TitleGroupAttribute 运动相关 = new TitleGroupAttribute("运动相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 运动仰角相关 = new TitleGroupAttribute("运动仰角相关(仅追踪目标有效)", indent: true, boldTitle: false);
            public static TitleGroupAttribute 命中逻辑相关 = new TitleGroupAttribute("命中逻辑相关", indent: true, boldTitle: false);
            public static TitleGroupAttribute 生命控制 = new TitleGroupAttribute("生命控制", indent: true, boldTitle: false);
            public static TitleGroupAttribute 未配置模块 = new TitleGroupAttribute("未配置模块", indent: true, order:1, boldTitle: false);

            public static OnValueChangedAttribute OnValueChange_FlyType = new OnValueChangedAttribute("OnValueChange_FlyType");
            public static OnValueChangedAttribute OnValueChange_TracePathType = new OnValueChangedAttribute("OnValueChange_TracePathType");
            public static HideIfAttribute HideIf_FlyType_TracePath = new HideIfAttribute("HideIf_FlyType_TracePath");

            public static ListDrawerSettingsAttribute ListDrawerSettingsAttribute_TracePathParams = new ListDrawerSettingsAttribute
            {
                CustomAddFunction = "CustomAddFunction_TracePathParams",
                OnBeginListElementGUI = "OnBeginListElement_TracePathParams",
                OnEndListElementGUI = "OnEndListElement_TracePathParams",
                HideAddButton = false,
                HideRemoveButton = false,
                ShowFoldout = true,
                DraggableItems = false,
            };
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                // 模块划分处理
                switch (member.Name)
                {
                    case nameof(config.ID):
                    case nameof(config.ModelBaseScalePercent):
                    case nameof(config.Hp):
                    case nameof(config.ATK):
                    case nameof(config.Model):
                    case nameof(config.ChainModel):
                    case nameof(config.ChainModelScalePercent):
                    case nameof(config.ChainTilingFactor):
                        attributes.Add(SelfAttributes.基础信息);
                        break;
                    case nameof(config.FlyType):
                    case nameof(config.AngleAdjustType):
                    case nameof(config.Speed):
                    case nameof(config.AcceSpeed):
                    case nameof(config.MaxSpeed):
                    case nameof(config.MaxTurnSpeed):
                    case nameof(config.TurnSpeed):
                    case nameof(config.TurnAcceSpeed):
                    case nameof(config.IsCloseTurnAutoHit):
                    case nameof(config.IsOpenPhysicalReflect):
                    case nameof(config.PhysicalReflectCount):
                    case nameof(config.PhysicalReflectEndActionType):
                    case nameof(config.TracePathType):
                    case nameof(config.TracePathParams):
                    case nameof(config.TrackEntityNoTargetSkillEffectConfigID):
                        attributes.Add(SelfAttributes.运动相关);
                        break;
                    case nameof(config.PitchTurnSpeed):
                    case nameof(config.PitchTurnAcceSpeed):
                    case nameof(config.PitchMaxTurnSpeed):
                        attributes.Add(SelfAttributes.运动仰角相关);
                        break;
                    case nameof(config.BeforeBornSkillEffectExecuteInfo):
                    case nameof(config.AfterBornSkillEffectExecuteInfo):
                    case nameof(config.DieSkillEffectExecuteInfo):
                    case nameof(config.DisappearEffect):
                    case nameof(config.IsAiEscape):
                        attributes.Add(SelfAttributes.命中逻辑相关);
                        break;
                    case nameof(config.delayDestroyTime):
                    case nameof(config.LastTime):
                    case nameof(config.DestroyWhenCreatorDie):
                    case nameof(config.LifeFlag):
                    case nameof(config.MaxDistance):
                        attributes.Add(SelfAttributes.生命控制);
                        break;
                    default:
                        // TODO 碰撞相关
                        attributes.Add(SelfAttributes.未配置模块);
                        break;
                }
                // 额外Attribute处理
                switch (member.Name)
                {
                    case nameof(config.FlyType):
                        attributes.Add(SelfAttributes.OnValueChange_FlyType);
                        break;
                    case nameof(config.TracePathType):
                        attributes.Add(SelfAttributes.OnValueChange_TracePathType);
                        attributes.Add(SelfAttributes.HideIf_FlyType_TracePath);
                        break;
                    case nameof(config.TracePathParams):
                        attributes.Add(SelfAttributes.ListDrawerSettingsAttribute_TracePathParams);
                        attributes.Add(SelfAttributes.HideIf_FlyType_TracePath);
                        break;
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }

        protected override bool ColorIfConditionAction(object obj, string propertyName)
        {
            if (obj is BulletConfig config)
            {
                switch (propertyName)
                {
                    case nameof(config.ID):
                        return config.ID == 0;
                    case nameof(config.Hp):
                        return config.Hp == 0;
                    case nameof(config.MaxSpeed):
                        return config.MaxSpeed == 0;
                    case nameof(config.Model):
                        return config.Model == 0;
                    case nameof(config.LastTime):
                        if (config.LifeFlag.HasFlag(BulletConfig_TBulletLifeFlag.TBLT_TIME))
                        {
                            return config.LastTime == 0;
                        }
                        break;
                    case nameof(config.MaxDistance):
                        if (config.LifeFlag.HasFlag(BulletConfig_TBulletLifeFlag.TBLT_DISTANCE))
                        {
                            return config.MaxDistance == 0;
                        }
                        break;

                }
            }
            return base.ColorIfConditionAction(obj, propertyName);
        }
    }
}

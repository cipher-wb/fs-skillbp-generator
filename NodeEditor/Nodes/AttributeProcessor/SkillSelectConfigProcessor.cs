using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal sealed class SkillSelectConfigProcessor : NodeEditorBaseProcessor<SkillSelectConfig>
    {
        static class SelfAttributes
        {
            public static TitleGroupAttribute 单位类型筛选配置 = new TitleGroupAttribute("单位类型筛选配置", indent: true, boldTitle: false);
            public static TitleGroupAttribute 二次筛选配置 = new TitleGroupAttribute("二次筛选配置", indent: true, boldTitle: false);
            public static TitleGroupAttribute 筛选条件配置 = new TitleGroupAttribute("筛选条件配置", indent: true, boldTitle: false);
            public static TitleGroupAttribute 其他配置 = new TitleGroupAttribute("其他配置", indent: true, boldTitle: false, order: 99);
            public static HideIfAttribute HideIf_EntityTypeFilters = new HideIfAttribute("HideIf_EntityTypeFilters");
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    // 模块划分处理
                    switch (member.Name)
                    {
                        case nameof(config.ID):
                        case nameof(config.Params):
                        case nameof(config.SkillSelectType):
                            //attributes.Add(new TitleGroupAttribute("筛选-基础信息", indent: true));
                            break;
                        case nameof(config.EntityTypeFilters):
                        case nameof(config.BattleUnitTypeFilters):
                        case nameof(config.ViewEffectTypeFilters):
                        case nameof(config.CollisionSubType):
                        case nameof(config.EntitySelectCD):
                        case nameof(config.EntitySelectMaxNum):
                        case nameof(config.ConllisionLayer):
                            attributes.Add(SelfAttributes.单位类型筛选配置);
                            break;
                        case nameof(config.SecondSelectType):
                        case nameof(config.SecondSelecrParams):
                            attributes.Add(SelfAttributes.二次筛选配置);
                            break;
                        case nameof(config.ConditionID):
                        case nameof(config.SpecialSkillSelectFlag):
                            attributes.Add(SelfAttributes.筛选条件配置);
                            break;
                        default:
                            attributes.Add(SelfAttributes.其他配置);
                            break;
                    }
                    var anno = TableAnnotation.Inst.GetParamsAnnotation(config.SkillSelectType);
                    if (anno != null)
                    {
                        switch (member.Name)
                        {
                            case nameof(config.Params):
                                {
                                    // 添加选择说明
                                    attributes.Add(new ListDrawerSettingsAttribute
                                    {
                                        HideAddButton = true,
                                        HideRemoveButton = true,
                                        ShowFoldout = true,
                                        DraggableItems = false,
                                    });
                                    break;
                                }
                            case nameof(config.SkillSelectType):
                            case nameof(config.ID):
                                {
                                    attributes.Add(DefaultAttributes.EnableIfAttribute_False);
                                }
                                break;
                            case nameof(config.SecondSelectType):
                                attributes.Add(SkillSelectConfig.Attribute_SecondSelectType);
                                break;
                            case nameof(config.SecondSelecrParams):
                                attributes.Add(SkillSelectConfig.Attribute_SecondSelecrParams);
                                attributes.Add(DefaultAttributes.ListDrawerSettingsAttribute_Hide);
                                break;
                        }
                    }
                    // 额外Attribute处理
                    switch (member.Name)
                    {                        
                        case nameof(config.BattleUnitTypeFilters):
                            attributes.Add(SelfAttributes.HideIf_EntityTypeFilters);
                            break;
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

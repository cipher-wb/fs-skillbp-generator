using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal class SkillConditionConfigProcessor : NodeEditorBaseProcessor<SkillConditionConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    var anno = TableAnnotation.Inst.GetParamsAnnotation(config.SkillConditionType);
                    if (anno != null)
                    {
                        switch (member.Name)
                        {
                            case nameof(config.Params):
                                {
                                    // 添加效果说明
                                    if (config.SkillConditionType == TSkillConditionType.TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG)
                                    {
                                        // 可动态添加
                                        attributes.Add(new ListDrawerSettingsAttribute
                                        {
                                            CustomRemoveIndexFunction = "CustomRemoveIndexFunction_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG",
                                            // 沿用编辑器自带的添加删除，因为自定义不触发面板刷新
                                            //CustomAddFunction = "CustomAddFunction_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG",
                                            //OnTitleBarGUI = "OnTitleBarGUI_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG",
                                            NumberOfItemsPerPage = 50,
                                            ShowFoldout = true,
                                            DraggableItems = false,
                                        });
                                    }
                                    else
                                    {
                                        // 添加条件说明
                                        attributes.Add(new ListDrawerSettingsAttribute
                                        {
                                            HideAddButton = true,
                                            HideRemoveButton = true,
                                            ShowFoldout = true,
                                            DraggableItems = false,
                                        });
                                    }
                                    break;
                                }
                            case nameof(config.SkillConditionType):
                            case nameof(config.ID):
                                {
                                    attributes.Add(DefaultAttributes.EnableIfAttribute_False);
                                    break;
                                }
                        }
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

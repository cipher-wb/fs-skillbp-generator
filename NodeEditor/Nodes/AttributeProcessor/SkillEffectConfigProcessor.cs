using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal sealed class SkillEffectConfigProcessor : NodeEditorBaseProcessor<SkillEffectConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    var anno = TableAnnotation.Inst.GetParamsAnnotation(config.SkillEffectType);
                    if (anno != null)
                    {
                        switch (member.Name)
                        {
                            case nameof(config.Params):
                                {
                                    // 添加效果说明
                                    if (config.SkillEffectType == TSkillEffectType.TSET_NUM_CALCULATE)
                                    {
                                        // 数值运算可动态添加
                                        attributes.Add(new ListDrawerSettingsAttribute
                                        {
                                            CustomRemoveIndexFunction = "CustomRemoveIndexFunction_Params_TSET_NUM_CALCULATE",
                                            // 沿用编辑器自带的添加删除，因为自定义不触发面板刷新
                                            //CustomAddFunction = "CustomAddFunction_Params_TSET_NUM_CALCULATE",
                                            //OnTitleBarGUI = "OnTitleBarGUI_Params_TSET_NUM_CALCULATE",
                                            NumberOfItemsPerPage = 50,
                                            ShowFoldout = true,
                                            DraggableItems = false,
                                        });
                                    }
                                    else if(config.SkillEffectType == TSkillEffectType.TSET_SWITCH_EXECUTE)
                                    {
                                        attributes.Add(new ListDrawerSettingsAttribute
                                        {
                                            CustomRemoveIndexFunction = "CustomRemoveIndexFunction_Params_TSET_SWITCH_EXECUTE",
                                            // 沿用编辑器自带的添加删除，因为自定义不触发面板刷新
                                            CustomAddFunction = "CustomAddFunction_Params_TSET_SWITCH_EXECUTE",
                                            //OnTitleBarGUI = "OnTitleBarGUI_Params_TSET_SWITCH_EXECUTE",
                                            NumberOfItemsPerPage = 50,
                                            ShowFoldout = true,
                                            DraggableItems = false,
                                        });
                                    }
                                    // 注册技能消息-检测和提示子类型
                                    else if (config.SkillEffectType == TSkillEffectType.TSET_REGISTER_SKILL_EVENT)
                                    {
                                        //InfoBox
                                        attributes.Add(new InfoBoxAttribute("$GetSkillEventParamNames", InfoMessageType.Warning));
                                        attributes.Add(new InfoBoxAttribute("为了性能,请填写事件子类型和子类型的值", InfoMessageType.Warning, "CheckSkillEventParamWarning"));
                                        attributes.Add(new InfoBoxAttribute("子类型和子类型的值有误!", InfoMessageType.Error, "CheckSkillEventParamError"));
                                        attributes.Add(DefaultAttributes.ListDrawerSettingsAttribute_Hide);
                                    }
                                    else if (config.SkillEffectType == TSkillEffectType.TSET_AI_MOVETO_POS)
                                    {
                                        attributes.Add(new InfoBoxAttribute("使用绝对坐标请注意ai的通用性!", InfoMessageType.Warning, "MoveToTargetPosCheckError"));
                                        attributes.Add(DefaultAttributes.ListDrawerSettingsAttribute_Hide);
                                    }
                                    else
                                    {
                                        attributes.Add(DefaultAttributes.ListDrawerSettingsAttribute_Hide);
                                    }
                                    break;
                                }
                            case nameof(config.SkillEffectType):
                            case nameof(config.ID):
                                {
                                    attributes.Add(DefaultAttributes.EnableIfAttribute_False);
                                }
                                break;
                        }
                    }
                }
            }
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);
        }
    }
}

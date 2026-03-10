using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TableDR;

namespace NodeEditor.SkillEditor
{
    internal class AITaskNodeConfigProcessor : NodeEditorBaseProcessor<AITaskNodeConfig>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var config = GetConfig(parentProperty);
            if (config != null)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    var anno = TableAnnotation.Inst.GetParamsAnnotation(config.TaskNodeType);
                    if (anno != null)
                    {
                        switch (member.Name)
                        {
                            case nameof(config.Params):
                                {
                                    // 添加效果说明
                                    if (config.TaskNodeType == AITaskNodeType.AI_TNT_SWITCH)
                                    {
                                        attributes.Add(new ListDrawerSettingsAttribute
                                        {
                                            CustomRemoveIndexFunction = "CustomRemoveIndexFunction_Params_AI_TNT_SWITCH",
                                            // 沿用编辑器自带的添加删除，因为自定义不触发面板刷新
                                            CustomAddFunction = "CustomAddFunction_Params_AI_TNT_SWITCH",
                                            NumberOfItemsPerPage = 50,
                                            ShowFoldout = true,
                                            DraggableItems = false,
                                        });
                                    }
                                    else
                                    {
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
                            case nameof(config.TaskNodeType):
                                {
                                    // 节点类型不可编辑
                                    attributes.Add(new EnableIfAttribute("@false"));
                                    if (LocalSettings.IsProgramer() && attributes.Count((attr) => { return attr is EnableIfAttribute; }) > 1)
                                    {
                                        Log.Error("属性添加存在重复添加情况，需要先检测");
                                    }
                                    break;
                                }
                            case nameof(config.ID):
                                {
                                    attributes.Add(new EnableIfAttribute("@false"));
                                }
                                break;
                            case nameof(config.SkillTagsList):
                                {
                                    attributes.Add(new ListDrawerSettingsAttribute
                                    {
                                        HideAddButton = true,
                                        OnTitleBarGUI = "OnTitleBarGUI_SkillTagsList",
                                        OnBeginListElementGUI = "OnBeginListElement_SkillTagsList",
                                        OnEndListElementGUI = "OnEndListElement_SkillTagsList"
                                    });
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

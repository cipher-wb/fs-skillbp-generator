using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Editor;
using TableDR;
using Sirenix.OdinInspector;
using System.Linq;
using Funny.Base.Utils;

namespace NodeEditor.SkillEditor
{
    internal sealed class TParamProcessor : NodeEditorBaseProcessor<TParam>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            base.ProcessSelfAttributes(property, attributes);

            var parentValue = property.Parent.Parent?.ParentValues[0];
            if (parentValue is IParamsNode ||
                parentValue is ParamsAnnotation)
            {
                attributes.Add(DefaultAttributes.HideReferenceObjectPickerAttribute);
            }
        }
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            try
            {
                if (parentProperty.ValueEntry.WeakSmartValue is TParam param)
                {
                    ParamsAnnotation anno = null;
                    TParamAnnotation paramAnn = null;
                    // 索引到参数列表
                    int index = -1;
                    var parentValue = parentProperty.Parent.Parent.ParentValues[0];
                    if (parentValue is IConfigBaseNode configNode)
                    {
                        var config = configNode.GetConfig();
                        // 索引到参数列表
                        switch (config)
                        {
                            case SkillEffectConfig skillEffectConfig:
                                {
                                    index = skillEffectConfig.Params.IndexOf(param);
                                    break;
                                }
                            case SkillConditionConfig conditionConfig:
                                {
                                    index = conditionConfig.Params.IndexOf(param);
                                    break;
                                }
                            case SkillSelectConfig selectConfig:
                                {
                                    index = selectConfig.Params.IndexOf(param);
                                    break;
                                }
                            case AITaskNodeConfig aiNodeConfig:
                                {
                                    index = aiNodeConfig.Params.IndexOf(param);
                                    break;
                                }
                        }
                    }
                    else if (parentProperty.Parent.Parent.Parent?.Parent?.ParentValues[0] is ParamsAnnotation paramsAnnotation)
                    {
                        anno = paramsAnnotation;
                        index = paramsAnnotation.paramsAnn.FindIndex(p => { return p.DefalutParam == param; });
                    }
                    else if (parentProperty.Parent.ValueEntry?.WeakSmartValue is TSkillBuffAttrValueParam buffAttrValueParam)
                    {
                        paramAnn = TParamAnnotation.Empty;
                    }
                    else if (parentValue is TableDR.BulletConfig bulletConfig)
                    {
                        paramAnn = TParamAnnotation.Empty;
                    }
                    IParamsNode paramsNode = null;
                    if (parentValue is IParamsNode)
                    {
                        paramsNode = parentValue as IParamsNode;
                    }
                    if (paramsNode != null)
                    {
                        anno = paramsNode.GetParamsAnnotation();
                    }
                    if (anno != null)
                    {
                        // 添加描述
                        var annoIndex = index;
                        if (anno.IsArray && index >= anno.ArrayStart)
                        {
                            // 如果是列表，那么只读取第一个作为描述
                            annoIndex = anno.ArrayStart;
                        }
                        paramAnn = anno.paramsAnn.ExGet(annoIndex);
                    }
                    if (paramAnn != null)
                    {
                        if (string.IsNullOrEmpty(param.GetDisplayName()))
                        {
                            paramAnn.CheckTParam(param, out var displayName, out var error, true);
                        }
                        attributes.Add(DefaultAttributes.IndentAttribute);
                        var customDes = param.GetCustomDescription(member.Name);
                        if (!string.IsNullOrEmpty(customDes))
                        {
                            for (int i = 0; i < attributes.Count; i++)
                            {
                                if (attributes[i] is System.ComponentModel.DescriptionAttribute descriptionAttribute)
                                {
                                    descriptionAttribute.GetType().GetProperty("DescriptionValue", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(descriptionAttribute, customDes);
                                }
                                else if (attributes[i] is LabelTextAttribute labelTextAttribute)
                                {
                                    labelTextAttribute.Text = customDes;
                                }
                            }
                        }

                        switch (member.Name)
                        {
                            case nameof(param.Value):
                                {
                                    attributes.Add(new TitleAttribute("$GetDisplayName", bold: false));
                                    attributes.Add(new InfoBoxAttribute("$GetErrorMessage", InfoMessageType.Error, "IsError"));
                                    //attributes.Add(new OnInspectorGUIAttribute("OnInspectorGUI_Value", false));
                                    // 如果是配表ID，那么这个ID不允许配置，直接按照连线获取
                                    var vdAttr = new TParamValueAttribute(param, paramAnn, parentValue);
                                    if (paramAnn.isConfigId)
                                    {
                                        // TODO 暂时还是打开让策划可配置
                                        //attributes.Add(new EnableIfAttribute("@false"));
                                    }
                                    attributes.Add(vdAttr);
                                    break;
                                }
                            case nameof(param.ParamType):
                            case nameof(param.Factor):
                            default:
                                break;
                        }
                        paramsNode?.ProcessParamAttributes(param, member, attributes);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                base.ProcessChildMemberAttributes(parentProperty, member, attributes);
            }
        }
    }
}
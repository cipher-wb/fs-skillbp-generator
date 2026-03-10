using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// 基础表格节点类型说明
    /// </summary>
    [Serializable, HideReferenceObjectPicker]
    public class ConfigAnnotation : NodeBaseAnnotation
    {
        [Serializable]
        public class ConfigColor
        {
            public float r = 1;
            public float g = 1;
            public float b = 1;
            public float a = 0.5f;
            public Color Get()
            {
                return new Color(r, g, b, a);
            }
            public static ConfigColor New(Color color)
            {
                return new ConfigColor
                {
                    r = color.r,
                    g = color.g,
                    b = color.b,
                    a = color.a
                };
            }
        }

        [Serializable, LabelText("@($property.ValueEntry.WeakSmartValue as ConfigMemberDesc).ToString()")]
        public class ConfigMemberDesc
        {
            [ReadOnly, TableColumnWidth(60), HideInInspector]
            public int FieldIndex;

            [ReadOnly, TableColumnWidth(120), HideInInspector]
            public string Colume;

            [ReadOnly, TableColumnWidth(120), HideInInspector]
            public string Name;

            [LabelText("【字段描述】"), MultiLineProperty]
            public string Desc;

            public override string ToString()
            {
                return $"【{FieldIndex}】{Colume}-{Name}";
            }
        }

        #region Override
        public override string Name => name;
        public override bool IsConfigDerive() => false;
        #endregion

        private bool invalid = false;
        private string message;

        [FoldoutGroup("$FoldoutGroupName"), LabelText("节点颜色"), JsonIgnore]
        public Color ColorPick = new UnityEngine.Color(1, 1, 1, 0.5f);

        [HideIf("@true")]
        public ConfigColor Color = new ConfigColor();

        [OnValueChanged("OnChangeRefTypeName"), DelayedProperty]
        [InfoBox("$message", InfoMessageType.Error, "invalid")]
        [FoldoutGroup("$FoldoutGroupName"), LabelText("表格名："), ShowInInspector]
        public string name;

        [FoldoutGroup("$FoldoutGroupName"), LabelText("表格描述："), ShowInInspector, MultiLineProperty]
        public string desc;

        [FoldoutGroup("$FoldoutGroupName"), LabelText("表格字段描述："), ShowInInspector, HideReferenceObjectPicker]
        //[TableList(/*AlwaysExpanded = true, */HideToolbar = true)]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowFoldout = true, ShowIndexLabels = false, DraggableItems = false)]
        public List<ConfigMemberDesc> configMemberDescs;

        [NonSerialized]
        public Dictionary<string, PropertyTooltipAttribute> MemberName2Tips = new Dictionary<string, PropertyTooltipAttribute>();

        //[InfoBox("如需导出本类型模板引用节点，则填写")]
        //[FoldoutGroup("$FoldoutGroupName"), LabelText("模板引用节点名"), ShowInInspector]
        //public string RefTemplateNodeName;
        public override string FoldoutGroupName
        {
            get
            {
                if (invalid)
                {
                    return message;
                }
                return name + base.FoldoutGroupName;
            }
        }
        public bool Check(StringBuilder sbError)
        {
            OnChangeRefTypeName();
            if (invalid)
            {
                sbError?.AppendLine(message);
            }
            return !invalid;
        }

        public override void Load()
        {
            base.Load();

            ColorPick = Color.Get();
            RefreshMemberDesc();
        }

        private void RefreshMemberDesc()
        {
            // 刷新描述信息
            configMemberDescs ??= new List<ConfigMemberDesc>();
            Utils.SafeCall(() =>
            {
                if (ExcelManager.Inst == null)
                {
                    // 存在嵌套初始化问题，判下空
                    return;
                }
                var members = ExcelManager.Inst.ProjectConfig.GetConfigJsonMembers(name);
                if (members != null)
                {
                    // 清理被移除字段
                    configMemberDescs.RemoveAll(
                        (info) =>
                        {
                            return members.FindIndex((m) => { return m.Name == info.Name; }) == -1;
                        });
                    foreach (var member in members)
                    {
                        var memberDesc = configMemberDescs.Find((info) => { return info.Name == member.Name; });
                        if (memberDesc == null)
                        {
                            // 新增字段
                            configMemberDescs.Add(new ConfigMemberDesc { Name = member.Name, FieldIndex = member.FieldIndex, Colume = member.Colume });
                        }
                        else
                        {
                            // 修改字段PropertyOder
                            memberDesc.FieldIndex = member.FieldIndex;
                            memberDesc.Colume = member.Colume;
                        }
                    }
                    // 按照FieldIndex排序
                    configMemberDescs.Sort((a, b) => { return a.FieldIndex.CompareTo(b.FieldIndex); });
                    // 刷新缓存数据
                    MemberName2Tips.Clear();
                    foreach (var item in configMemberDescs)
                    {
                        if (!string.IsNullOrEmpty(item.Desc))
                        {
                            MemberName2Tips.Add(item.Name, new PropertyTooltipAttribute(item.Desc));
                        }
                    }
                }
            });
        }

        public override string Save(StringBuilder sbSaveInfo)
        {
            var saveContent = string.Empty;
            Utils.SafeCall(() =>
            {
                RefreshMemberDesc();
                // 基础config代码生成
                {
                    Color = ConfigColor.New(ColorPick);
                    //Color.a = 0.5f;
                    PathDirNodeScript = Constants.NodesScriptDir;
                    NodeName = $"{name}Node";
                    ConfigName = name;

                    var scriptContent = ConfigAnnotation.TemplateClassContent;
                    var descCode = ConfigName;
                    if (!string.IsNullOrEmpty(desc))
                        descCode += $"_{desc}";
                    // 生成所属模块及编辑器
                    var attributes = string.Empty;
                    if (!IsIgnoreConfig())
                    {
                        foreach (var graphData in GetGraphDatasSaving())
                        {
                            var graphType = graphData?.GetGraphType();
                            if (graphType != null)
                            {
                                var menuDesc = descCode;
                                var moduleName = graphData.GetModuleName(null);
                                if (moduleName != null)
                                {
                                    menuDesc = $"{moduleName}/{descCode}";
                                }
                                var moduleNamePerfix = graphData.GetModuleNamePerfix();
                                attributes += $"\n\t[NodeMenuItem(\"{moduleNamePerfix}{menuDesc}\", typeof({graphType.FullName}))]";
                            }
                        }
                    }
                    scriptContent = scriptContent
                        .Replace("#DESCRIPTION#", $"\n\t\t// {descCode}")
                        .Replace("#ATTRIBUTES#", attributes)
                        .Replace("#CONFIGNAME#", name);

                    saveContent = scriptContent;
                    Utils.DeleteFile(PathNodeScript);   // 避免老数据残留，尝试再删一遍
                    //// 屏蔽单独文件写入，改为统一在一个文件
                    //saveContent = templateFileContent.Replace(templateClassContent, scriptContent);
                    //WriteToFile(PathNodeScript, scriptContent, sbSaveInfo);
                }
            });
            return saveContent;
        }
        private void OnChangeRefTypeName()
        {
            var refType = TableHelper.GetTableType($"{Constants.TableNameSpace}.{name}");
            if (refType == null)
            {
                invalid = true;
                if (string.IsNullOrEmpty(name))
                {
                    message = $"{Constants.BaseConfigAnnotaionSheetName} 未配置表格名";
                }
                else
                {
                    message = $"{Constants.BaseConfigAnnotaionSheetName} 找不到表格数据类型: {name}";
                }
            }
            else
            {
                invalid = false;
                message = string.Empty;
            }
        }

        private static string templateClassContent;
        private static string templateFileContent;
        public static string TemplateClassContent
        {
            get
            {
                if (templateClassContent == null)
                {
                    InitTemplateContent();
                }
                return templateClassContent;
            }
        }
        public static string TemplateFileContent
        {
            get
            {
                if (templateFileContent == null)
                {
                    InitTemplateContent();
                }
                return templateFileContent;
            }
        }
        private static void InitTemplateContent()
        {
            if (string.IsNullOrEmpty(templateFileContent))
            {
                templateFileContent = Utils.ReadAllText(Constants.NodesScriptTemplatePath);
                templateClassContent = Utils.GetMiddleString(templateFileContent, "// TEMPLATE_CONTENT_BEGIN\n", "// TEMPLATE_CONTENT_END\n");
            }
        }
    }
}

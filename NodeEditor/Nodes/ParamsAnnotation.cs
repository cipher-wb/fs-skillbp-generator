using System;
using System.Collections.Generic;
using TableDR;
using Sirenix.OdinInspector;
using System.Linq;
using NodeEditor.PortType;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

#pragma warning disable CS0414

namespace NodeEditor
{
    [Serializable, HideReferenceObjectPicker]
    public class TParamAnnotation : ISerializationCallbackReceiver
    {
        public static readonly TParamAnnotation Empty = new TParamAnnotation();

        public const string DefaultName = "未配置参数说明";

        private bool invalid = false;
        private string invalidMesage;
        private bool invalidPortTypeName = false;
        private string invalidMesagePortTypeName;
        public const bool Debug = false;

        [HideIf("@true"), JsonIgnore]
        public string DefaultValueDesc = string.Empty;

        [FoldoutGroup("$Name"), LabelText("参数说明"), DelayedProperty]
        public string Name;

        [FoldoutGroup("$Name"), LabelText("引用类型名"), OnValueChanged("OnChangeRefTypeName"), DelayedProperty, ValueDropdown("GetRefTypes")]
        [InfoBox("@invalidMesage", InfoMessageType.Error, "invalid")]
        public string RefTypeName;

        [FoldoutGroup("$Name"), LabelText("节点类型限制"), OnValueChanged("OnChangePortTypeName"), DelayedProperty]
        [InfoBox("@invalidMesagePortTypeName", InfoMessageType.Error, "invalidPortTypeName")]
        public string RefPortTypeNames;

        // 获取接口：GetDefalutParam()
        [FoldoutGroup("$Name"), BoxGroup("$Name/参数默认值"), DelayedProperty, HideLabel, OnValueChanged("OnValueChanged_DefalutParam", true)]
        public TParam DefalutParam;

        // Json数据-记录DefalutParam
        [HideIf("@true")]
        public string DefalutParamJson;

        #region DEBUG
        [ReadOnly, LabelText("参数引用类型"), ShowIf("@Debug")]
        private Type refType;

        [ReadOnly, LabelText("节点类型"), ShowIf("@Debug"), NonSerialized]
        public List<string> RefPortTypeNameList;

        [ReadOnly, LabelText("节点类型"), ShowIf("@Debug")]
        private Type refPortType;

        [ReadOnly, LabelText("是否表格引用"), ShowIf("@Debug")]
        public bool isConfigId;

        [ReadOnly, LabelText("是否枚举引用"), ShowIf("@Debug")]
        public bool isEnum;

        //[ReadOnly, LabelText("是否位组合"), ShowIf("@Debug")]
        //public bool isFlags;

        [ReadOnly, LabelText("引用类型全称"), ShowIf("@Debug")]
        public string RefTableFullName;

        [ReadOnly, LabelText("引用表格Manager"), ShowIf("@Debug")]
        public string RefTableManagerName;

        [ReadOnly, LabelText("节点类型名"), ShowIf("@Debug")]
        public string RefPortTypeName;

        [ReadOnly, LabelText("节点类型全称"), ShowIf("@Debug")]
        public string RefPortTypeFullName;

        [ReadOnly, LabelText("是否作为返回值类型"), ShowIf("@Debug")]
        public bool IsFunctionReturn;

        #endregion

        public static Sirenix.OdinInspector.ValueDropdownList<string> VD_TableRefTypes;
        // TODO RefPortTypeNames 后续优化成多选
        private IEnumerable<ValueDropdownItem<string>> GetRefTypes()
        {
            if (VD_TableRefTypes == null)
            {
                VD_TableRefTypes = new Sirenix.OdinInspector.ValueDropdownList<string>();
                VD_TableRefTypes.Add("-", null);
                foreach (var kv in ExcelManager.Inst.ProjectConfig.Tables)
                {
                    var table = kv.Value;
                    var tableName = table.Name;
                    VD_TableRefTypes.Add(tableName);
                }
                foreach (var subClass in ExcelManager.Inst.ProjectConfig.Classes)
                {
                    var subClassName = subClass.Name;
                    if (subClass.Enum)
                    {
                        VD_TableRefTypes.Add($"枚举类型-{subClassName}", subClassName);
                    }
                    else
                    {
                        VD_TableRefTypes.Add(subClassName);
                    }
                }
            }
            foreach (var vd in VD_TableRefTypes)
            {
                yield return vd;
            }
        }

        public override bool Equals(object obj)
        {
            if(obj.GetType() != this.GetType()) return false;

            var comparer = obj as TParamAnnotation;

            bool paramEqual = this.Name == comparer.Name
                && this.RefTypeName == comparer.RefTypeName
                && this.RefPortTypeNames == comparer.RefPortTypeNames
                && this.DefalutParam == comparer.DefalutParam;

            if((this.DefalutParam != null && this.DefalutParam != null))
            {
                paramEqual = paramEqual && this.DefalutParam.Value == comparer.DefalutParam.Value
                && this.DefalutParam.ParamType == comparer.DefalutParam.ParamType
                && this.DefalutParam.Factor == comparer.DefalutParam.Factor;
            }

            return paramEqual;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(RefTypeName) ? Name : $"{Name}|{RefTypeName}";
        }

        /// <summary>
        /// 参数默认都算效果表类型
        /// TODO SkillTagInfo需要处理下
        /// </summary>
        public static readonly Type DefaultRefType = typeof(SkillEffectConfig);

        /// <summary>
        /// 参数默认都算效果表类型
        /// TODO SkillTagInfo需要处理下
        /// </summary>
        public static readonly Type DefaultPortType = typeof(ConfigPortType_SkillEffectConfig);

        /// <summary>
        /// 获取所有限制节点类型，否则获取单个引用类型
        /// </summary>
        public List<string> GetValidRefTypeNames()
        {
            List<string> typeNames = default;
            //优先返回限制类型
            if (!string.IsNullOrEmpty(RefPortTypeNames))
            {
                typeNames = RefPortTypeNames.Split('|').ToList();
                typeNames.Sort();
                for (int i = 0, length = typeNames.Count; i < length; i++)
                {
                    var typeName = typeNames[i];
                    typeNames.Add(typeName);
                }
            }
            //否则返回单个引用类型
            else
            {
                //为空采用默认类型
                typeNames = new List<string>
                {
                    !string.IsNullOrEmpty(RefTypeName) ? RefTypeName : DefaultRefType.Name,
                };
            }

            return typeNames;
        }
        public Type GetRefType(Type defaultType)
        {
            if (refType != null)
            {
                return refType;
            }
            Type type = defaultType;
            if (refType == null)
            {
                if (string.IsNullOrEmpty(RefTableFullName))
                {
                    return type;
                }
                refType = TableHelper.GetTableType(RefTableFullName);
            }
            if (refType != null)
            {
                type = refType;
            }
            return type;
        }
        public Type GetRefPortType(Type defaultType)
        {
            // TODO 节点类型变化会导致GUID变化！，先屏蔽
            //return GetRefType(DefaultRefType);
            if (refPortType != null)
            {
                return refPortType;
            }
            Type type = defaultType;
            if (refPortType == null)
            {
                if (string.IsNullOrEmpty(RefPortTypeFullName))
                {
                    if (!string.IsNullOrEmpty(RefTableFullName))
                    {
                        var refType = GetRefType(null);
                        if (refType != null)
                        {
                            type = TablePortTypesHelper.GetType(refType);
                        }
                    }
                    refPortType = type;
                    return type;
                }
                else
                {
                    refPortType = typeof(TablePortTypesHelper).Assembly.GetType(RefPortTypeFullName);
                }
            }
            if (refPortType != null)
            {
                type = refPortType;
            }
            return type;
        }
        public TParam CopyDefaultParam()
        {
            if (DefalutParam == null)
            {
                return new TParam();
            }
            return Utils.DeepCopyByBinary(DefalutParam);
        }
        public void ForceDoChange()
        {
            OnChangeRefTypeName();
            OnChangePortTypeName();
        }
        public string GetName()
        {
            var name = Name;
            if (string.IsNullOrEmpty(Name))
            {
                name = DefaultName;
            }
            return name.Replace("/", "_");
        }
        public string GetDisplayName(TParam tParam, bool port = false)
        {
            var name = GetName();
            var displayName = port || string.IsNullOrEmpty(RefTypeName) ? name : $"{name}-{RefTypeName}";
            if (tParam == null)
            {
                return displayName;
            }
            var valueDesc = DefaultValueDesc;
            if (!string.IsNullOrEmpty(Name))
            {
                var refType = GetRefType(DefaultRefType);
                if (isEnum)
                {
                    // 枚举，直接显示枚举的描述
                    if (tParam.ParamType == TParamType.TPT_NULL)
                    {
                        valueDesc = Utils.GetEnumDescription(refType, tParam.Value, string.Empty);
                    }
                }
                else if (RefTypeName == nameof(SkillTagsConfig))
                {
                    try
                    {
                        var config = DesignTable.GetTableCell<SkillTagsConfig>(tParam.Value);
                        if (config != null)
                        {
                            valueDesc = config.Desc;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }
                }
                else if (RefTypeName == nameof(SkillEventConfig))
                {
                    try
                    {
                        var config = DesignTable.GetTableCell<SkillEventConfig>(tParam.Value);
                        if (config != null)
                        {
                            valueDesc = config.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Exception(ex);
                    }
                }
                else
                {
                    switch (tParam.ParamType)
                    {
                        case TParamType.TPT_ATTR:
                            {
                                valueDesc = Utils.GetEnumDescription(typeof(TBattleNatureEnum), tParam.Value, string.Empty);
                                break;
                            }
                        case TParamType.TPT_COMMON_PARAM:
                            {
                                valueDesc = Utils.GetEnumDescription(typeof(TCommonParamType), tParam.Value, string.Empty);
                                break;
                            }
                        case TParamType.TPT_COMMON_SKILL_PARAM:
                            {
                                valueDesc = Utils.GetEnumDescription(typeof(TCommonSkillParamType), tParam.Value, string.Empty);
                                break;
                            }
                    }
                }
            }
            // 显示万分比
            var factor = tParam.Factor != 0 ? $"*{Math.Round(tParam.Factor / 10000.0f, 2)}" : string.Empty;
            if (port)
            {
                var displayValue = string.IsNullOrEmpty(valueDesc) ? $"[{tParam.Value}]" : $"[{valueDesc}:{tParam.Value}]";
                return $"{displayName}-{displayValue}{factor}";
            }
            else
            {
                return string.IsNullOrEmpty(valueDesc) ? displayName : $"{displayName}-[{valueDesc}]{factor}";
            }
        }

        /// <summary>
        /// 更新参数表现信息
        /// </summary>
        /// <param name="tParam">参数对象</param>
        /// <param name="error">参数错误信息</param>
        /// <returns>参数有效性</returns>
        public bool CheckTParam(TParam tParam, out string displayName, out string error, bool isUseOldDisplayName = false)
        {
            displayName = tParam?.GetDisplayName();
            if (isUseOldDisplayName && !string.IsNullOrEmpty(displayName))
            {
                // 显示信息不变
            }
            else
            {
                displayName = GetDisplayName(tParam);
            }
            var valid = IsValid(tParam, out error);
            tParam?.RefreshDisplay(displayName, error);
            return valid;
        }
        public bool IsValid(TParam tParam, out string error)
        {
            error = string.Empty;
            if (tParam == null)
            {
                error = "参数为空";
                return false;
            }
            else
            {
                if (isEnum)
                {
                    // 参数为枚举时
                    switch (tParam.ParamType)
                    {
                        case TParamType.TPT_NULL:
                            {
                                var refType = GetRefType(null);
                                if (refType != null && !Utils.IsEnumDefined(refType, tParam.Value))
                                {
                                    error = $"数值枚举未定义";
                                    return false;
                                }
                                break;
                            }
                        case TParamType.TPT_EXTRA_PARAM:
                        case TParamType.TPT_EVENT_PARAM:
                            // 额外参数暂不处理枚举（可能是模板节点）
                            // 额外参数避免下ID误传
                            if (tParam.Value > 100)
                            {
                                error = "额外参数数值过大";
                                return false;
                            }
                            if (tParam.Value <= 0)
                            {
                                error = "额外参数最小值为1";
                                return false;
                            }
                            break;
                        case TParamType.TPT_FUNCTION_RETURN:
                            if (typeof(ISkillEffectConfig).IsAssignableFrom(GetRefPortType(null)))
                            {
                                // 端口类型包含效果类型，那么不做数值检测
                            }
                            else
                            {
                                // 否则提示返回值报错
                                error = $"枚举参数不支持返回值";
                            }
                            break;
                        case TParamType.TPT_ATTR:
                            if (!Enum.IsDefined(typeof(TBattleNatureEnum), tParam.Value))
                            {
                                error = $"数值枚举未定义";
                                return false;
                            }
                            break;
                        default:
                            error = $"数值类型错误:{Utils.GetEnumDescription(typeof(TParamType), (int)tParam.ParamType, "错误枚举值")}";
                            return false;
                    }
                }
                else
                {
                    switch (tParam.ParamType)
                    {
                        case TParamType.TPT_ATTR:
                            if (!Enum.IsDefined(typeof(TBattleNatureEnum), tParam.Value))
                            {
                                error = $"数值枚举未定义";
                                return false;
                            }
                            break;
                        case TParamType.TPT_COMMON_PARAM:
                            if (!Enum.IsDefined(typeof(TCommonParamType), tParam.Value))
                            {
                                error = $"数值枚举未定义";
                                return false;
                            }
                            break;
                        case TParamType.TPT_COMMON_SKILL_PARAM:
                            if (!Enum.IsDefined(typeof(TCommonSkillParamType), tParam.Value))
                            {
                                error = $"数值枚举未定义";
                                return false;
                            }
                            break;
                        case TParamType.TPT_EXTRA_PARAM:
                            // 额外参数避免下ID误传
                            if (tParam.Value > 100)
                            {
                                error = "额外参数数值过大";
                                return false;
                            }
                            break;
                        case TParamType.TPT_NULL:
                            {
                                // 如果是表格引用，检查下表格数据是否存在，通用检查不打开ID=0报错，需要检查单独配置扩展处理
                                if (isConfigId && tParam.Value > 0)
                                {
                                    var type = GetRefType(null);
                                    if (type != null)
                                    {
                                        var isValidConfig = ConfigIDManager.Inst.HasConfigData(type.Name, tParam.Value);
                                        var refPortTypeNameList = GetRefPortTypeNameList(false);
                                        if (!isValidConfig && refPortTypeNameList != null)
                                        {
                                            // 节点连线支持多个，需要判断多个是否存在，如：SkillEffectConfig|AITaskNodeConfig
                                            foreach (var typeRef in refPortTypeNameList)
                                            {
                                                if (ConfigIDManager.Inst.HasConfigData(typeRef, tParam.Value))
                                                {
                                                    isValidConfig = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!isValidConfig)
                                        {
                                            error = $"表格缺失{type.Name}:{tParam.Value}";
                                            return false;
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                return true;
            }
        }
        private void OnChangeRefTypeName()
        {
            if (DefalutParam == null) DefalutParam = new TParam();

            refType = TableHelper.GetTableType($"{Constants.TableNameSpace}.{RefTypeName}");
            if (refType == null && !string.IsNullOrEmpty(RefTypeName))
            {
                invalid = true;
                invalidMesage = $"找不到表格数据类型: {RefTypeName}";
            }
            else
            {
                invalid = false;
            }
            isConfigId = refType?.IsClass ?? false;
            isEnum = refType?.IsEnum ?? false;
            //isFlags = false;
            if (isConfigId)
            {
                RefTableFullName = refType.FullName;
                RefTableManagerName = refType.FullName + Constants.TableManagerSuffix;
            }
            else if (isEnum)
            {
                //var flags = refType.GetCustomAttributes(typeof(FlagsAttribute), false);
                //isFlags = flags?.Length > 0;
                RefTableFullName = refType.FullName;
                RefTableManagerName = string.Empty;
            }
            else
            {
                RefTableFullName = string.Empty;
                RefTableManagerName = string.Empty;
            }
        }
        private List<string> GetRefPortTypeNameList(bool bForceUpdateCache = false)
        {
            if (string.IsNullOrEmpty(RefPortTypeNames))
            {
                return null;
            }
            if (RefPortTypeNameList == null || bForceUpdateCache)
            {
                var typeNames = RefPortTypeNames.Split('|');
                //if (typeNames.Length <= 1)
                //{
                //    return;
                //}
                RefPortTypeNameList = typeNames.ToList();
                // 必须排序，避免不同顺序创建重复type
                RefPortTypeNameList.Sort();
            }
            return RefPortTypeNameList;
        }
        private void OnChangePortTypeName()
        {
            if (string.IsNullOrEmpty(RefPortTypeNames) && !string.IsNullOrEmpty(RefTypeName))
            {
                RefPortTypeNames = RefTypeName;
            }
            if (RefTypeName == RefPortTypeNames && !string.IsNullOrEmpty(RefTypeName))
            {
                RefPortTypeNames = null;
            }
            IsFunctionReturn = false;
            RefPortTypeName = null;
            RefPortTypeFullName = null;
            if (string.IsNullOrEmpty(RefPortTypeNames))
            {
                return;
            }
            GetRefPortTypeNameList(true);
            RefPortTypeName = nameof(ConfigPortType);
            invalidPortTypeName = false;
            for (int i = 0, length = RefPortTypeNameList.Count; i < length; i++)
            {
                var typeName = RefPortTypeNameList[i];
                var refConfig = TableHelper.GetTableType($"{Constants.TableNameSpace}.{typeName}");
                if (refConfig == null)
                {
                    // TODO 找不到对应表格节点，尝试找特定节点
                    invalidPortTypeName = true;
                    invalidMesagePortTypeName = $"找不到表格数据类型: {typeName}";
                    break;
                }
                RefPortTypeName += $"_{typeName}";
            }
            if (invalidPortTypeName)
            {
                RefPortTypeNameList = null;
                RefPortTypeName = null;
                RefPortTypeFullName = null;
            }
            else
            {
                RefPortTypeFullName = $"{TablePortTypesHelper.NameSpace}.{RefPortTypeName}";
                IsFunctionReturn = RefTypeName != nameof(SkillEffectConfig) && RefPortTypeNameList.Contains(nameof(SkillEffectConfig));
            }
        }
        public void Load()
        {
            if (!string.IsNullOrEmpty(DefalutParamJson))
            {
                try
                {
                    DefalutParam = JsonConvert.DeserializeObject<TParam>(DefalutParamJson);
                }
                catch
                {
                    DefalutParam = null;
                }
            }
            if (DefalutParam == null)
            {
                DefalutParam = new TParam();
            }
        }
        public void UnLoad()
        {
            OnValueChanged_DefalutParam();
        }
        private void OnValueChanged_DefalutParam()
        {
            if (DefalutParam != null)
            {
                DefalutParamJson = JsonConvert.SerializeObject(DefalutParam);
            }
        }
        public void OnBeforeSerialize()
        {
            // 注：ISerializationCallbackReceiver不断触发刷新，存在性能问题，记录数据只在OnValueChanged_DefalutParam处理
            //DefalutParamJson = JsonConvert.SerializeObject(DefalutParam);
        }
        public void OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(DefalutParamJson))
            {
                DefalutParam = JsonConvert.DeserializeObject<TParam>(DefalutParamJson);
            }
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public class ParamsAnnotation : NodeBaseAnnotation
    {
        #region Override
        public override string Name => name;
        public override bool IsConfigDerive() => true;
        #endregion

        private bool invalidArrayStart
        {
            get
            {
                if (IsArray && ArrayStart != 0 && ArrayStartTmp == ArrayStart && paramsAnn.Count != paramsAnnTmp.Count)
                {
                    return true;
                }
                return false;
            }
        }

        public const string DefaultTips = "未配置参数描述";
        public const string DefaultModuleName = "未配置模块";

        [FoldoutGroup("$FoldoutGroupName"), LabelText("节点模板配置"), ShowIf("IsShowVirtualNodeDatas")]
        public List<VirtualNodeData> VirtualNodeDatas = new List<VirtualNodeData>();

        [JsonIgnore]
        private bool IsShowVirtualNodeDatas => ConfigName == nameof(NpcEventActionConfig);

        [LabelText("类型"), ReadOnly, FoldoutGroup("$FoldoutGroupName", expanded: false)/*, HorizontalGroup("$name/类型")*/]
        public string name;

        [HideIf("@true")]
        public int EnumValue;

        [LabelText("说明"), FoldoutGroup("$FoldoutGroupName", expanded: false),TextArea(3,10)]
        public string tips;

        [LabelText("是否不定长数组"), FoldoutGroup("$FoldoutGroupName", expanded: false)]
        [InfoBox("不定数组如果需要加修改固定参数的话需要关闭相关Graph窗口，否则连线会丢失!", InfoMessageType.Warning, "tipWaring")]
        public bool IsArray;

        [LabelText("不定长数组开始索引"), OnValueChanged("OnChangeArrayStart"), FoldoutGroup("$FoldoutGroupName", expanded: false)]
        [InfoBox("参数数量有改动,ArrayStart值也需要改动", InfoMessageType.Error, "invalidArrayStart")]
        [InfoBox("修改此值请确保Graph已经处于关闭状态，否则连线会丢失!", InfoMessageType.Warning, "tipWaring")]
        public int ArrayStart = 0;
        private bool tipWaring => IsArray;
        /// <summary>
        /// 用来缓存ArrayStart，当ArrayStart值有变化时ArrayStartTmp就是原来的值
        /// </summary>
        private int ArrayStartTmp = 0;

        [JsonIgnore, HideIf("@true")]
        public Type TypeParams;

        [LabelText("参数列表"), FoldoutGroup("$FoldoutGroupName", expanded: false)]
        [ListDrawerSettings(ShowFoldout = true, CustomAddFunction = "CustomAddFunction_TParamAnnotation")]
        public List<TParamAnnotation> paramsAnn = new List<TParamAnnotation>();

        /// <summary>
        /// 缓存的参数注解,用来记录固定参数的数量修改
        /// </summary>
        private List<TParamAnnotation> paramsAnnTmp = new List<TParamAnnotation>();

        public ParamsAnnotation(string name)
        {
            this.name = name;
            this.tips = DefaultTips;
        }
        public override string FoldoutGroupName => name + base.FoldoutGroupName;
        private TParamAnnotation CustomAddFunction_TParamAnnotation()
        {
            return new TParamAnnotation
            {
                DefalutParam = new TParam()
            };
        }

        public override void Load()
        {
            base.Load();
            ArrayStartTmp = ArrayStart;
            foreach (var paramAnn in paramsAnn)
            {
                paramAnn.Load();
                if (IsArray)
                {
                    paramsAnnTmp.Add(paramAnn);
                }
            }
        }

        public override string Save(StringBuilder sbSaveInfo)
        {
            var saveContent = string.Empty;
            Utils.SafeCall(() =>
            {
                //检测ArrayStart的改动
                //-todo 将IsArray 改成非IsArray 或者将非IsArray改成IsArray,固定参数去有参数变动等等，根据后续是否有需求再做修改
                if (IsArray)
                {
                    if (ArrayStart != ArrayStartTmp)
                    {
                        if (!EditorUtility.DisplayDialog("警告", $"由于你修改了不定数组的起始值，即将修复所有使用了‘{Name}’的Graph连线，请做好存储和提交svn准备!", "已处理好继续"))
                        {
                            return;
                        }
                        AnnotationArrayStartChangedHandle.CheckAndHandlerGraphEdges(this, NodeName, ConfigName, ArrayStartTmp, ArrayStart, paramsAnnTmp, paramsAnn);
                        ArrayStartTmp = ArrayStart;
                        paramsAnnTmp.Clear();
                        paramsAnnTmp.AddRange(paramsAnn);
                    }
                }

                foreach (var paramAnn in paramsAnn)
                {
                    paramAnn.UnLoad();
                }

                // 生成参数模板代码
                var scriptContent = ParamsAnnotation.TemplateClassContent;
                // 生成所属模块及编辑器
                var attributes = string.Empty;
                foreach (var graphData in GetGraphDatasSaving())
                {
                    var graphType = graphData?.GetGraphType();
                    if (graphType != null)
                    {
                        var moduleName = graphData.GetModuleName();
                        var moduleNamePerfix = graphData.GetModuleNamePerfix();

                        //如果有虚拟节点，需要把自己放到列表内
                        if (VirtualNodeDatas?.Count > 0)
                        {
                            attributes += $"\n\t[NodeMenuItem(\"{moduleNamePerfix}{moduleName}/{name}/{name}\", typeof({graphType.FullName}))]";
                        }
                        else
                        {
                            attributes += $"\n\t[NodeMenuItem(\"{moduleNamePerfix}{moduleName}/{name}\", typeof({graphType.FullName}))]";
                        }
                    }
                }
                var desc = $"\n\t\t// {name}";
                for (int i = 0, length = paramsAnn.Count; i < length; i++)
                {
                    var anno = paramsAnn[i];
                    desc += $"\n\t\t// 参数{i} : {anno.GetDisplayName(null, false)}";
                }
                scriptContent = scriptContent
                    .Replace("#ATTRIBUTES#", attributes)
                    .Replace("#CONFIGNAME#", ConfigName)
                    .Replace("#DESCRIPTION#", desc)
                    .Replace("#SCRIPTNAME#", NodeName)
                    .Replace("#TYPENAME#", TypeParams.Name)
                    .Replace("#TYPEDESC#", name);

                saveContent = scriptContent;
                Utils.DeleteFile(PathNodeScript);   // 避免老数据残留，尝试再删一遍
                // 屏蔽单独文件写入，改为统一在一个文件
                //saveContent = TemplateFileContent.Replace(TemplateClassContent, scriptContent);
                //WriteToFile(PathNodeScript, saveContent, sbSaveInfo);

                //创建虚拟节点代码
                CreateVirtualNodeCode(sbSaveInfo);
            });
            return saveContent;
        }

        public override string ToString()
        {
            // 顺序执行,顺序执行一系列技能效果,技能效果IDSkillEffectConfig
            var info = paramsAnn.Select((anno, index) => $"{index + 1}-{anno.ToString()}");
            return $"\"{name}\",\"{tips}\",{string.Join(",", info)}";
        }

        /// <summary>
        /// 创建虚拟节点代码
        /// </summary>
        /// <param name="sbSaveInfo"></param>
        protected override string CreateVirtualNodeCode(StringBuilder sbSaveInfo)
        {
            var saveContent = string.Empty;
            if (VirtualNodeDatas.Count <= 0) { return saveContent; }

            var templateText = Utils.ReadAllText(Constants.SubVirtualNodesScriptTemplatePath);

            // 生成参数模板代码
            foreach (var virtualNodeData in VirtualNodeDatas)
            {
                if (!virtualNodeData.IsValid) { continue; }

                var scriptContent = templateText;
                var showName = virtualNodeData.ShowName;
                var codeName = virtualNodeData.CodeName;

                //1.生成所属模块及编辑器
                var attributes = string.Empty;
                foreach (var grapData in GetGraphDatasSaving())
                {
                    var graphType = grapData?.GetGraphType();
                    if (graphType != null)
                    {
                        var moduleName = grapData.GetModuleName();
                        attributes += $"\n\t[NodeMenuItem(\"{moduleName}/{name}/{showName}\", typeof({graphType.FullName}))]";
                    }
                }

                //2.类名
                var className = $"{NodeName}_{codeName}";

                //3.配置文件接口
                var configPortType = $"I{ConfigName}";

                //3.json路径
                var jsonPath = $"\"{virtualNodeData.GraphJsonPath}\"";

                saveContent = scriptContent
                    .Replace("#ATTRIBUTES#", attributes)
                    .Replace("#SCRIPTNAME#", className)
                    .Replace("#CONFIGPORTTYPE#", configPortType)
                    .Replace("#JSONASSETPATH#", jsonPath);

                var scriptPath = $"{Constants.SubNodesScriptDir}/{ConfigName}/{className}.cs";
                WriteToFile(scriptPath, saveContent, sbSaveInfo);
            }
            return saveContent;
        }

        /// <summary>
        /// 不定长数组的开始索引变化，需要更改所有相关的连线
        /// </summary>
        public void OnChangeArrayStart()
        {

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
                templateFileContent = Utils.ReadAllText(Constants.SubNodesScriptTemplatePath);
                templateClassContent = Utils.GetMiddleString(templateFileContent, "// TEMPLATE_CONTENT_BEGIN\n", "// TEMPLATE_CONTENT_END\n");
            }
        }
    }
}

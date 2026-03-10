using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TableDR;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using NodeEditor.PortType;

namespace NodeEditor
{
    // TODO 改为SingletonJson
    [Serializable, HideMonoScript]
    public sealed class TableAnnotation : ISetting
    {
        public const string name = "【节点说明配置】";
        private static TableAnnotation inst;
        public static TableAnnotation Inst
        {
            get
            {
                if (inst == null)
                {
                    CreateInstance();
                }
                return inst;
            }
        }
        private static Action onChangedJson;
        public static event Action OnChangedJson
        {
            add
            {
                onChangedJson -= value;
                onChangedJson += value;
            }
            remove { onChangedJson -= value; }
        }

        #region 常量定义
        public static readonly Dictionary<Type, string> SpecialConfigMap = new Dictionary<Type, string>
        {
            { typeof(TSkillEffectType),     nameof(SkillEffectConfig)},
            { typeof(TSkillConditionType),  nameof(SkillConditionConfig)},
            { typeof(TSkillSelectType),     nameof(SkillSelectConfig)},
            { typeof(AITaskNodeType),       nameof(AITaskNodeConfig)},
            { typeof(MapAnimStateConfig_PointType), nameof(MapAnimStateConfig)},
        };

        /// <summary>
        /// 忽略的事件行为类型
        /// </summary>
        public static HashSet<NpcEventActionConfig_TEventActionType> MapEventIgronType = new HashSet<NpcEventActionConfig_TEventActionType>()
        {
            NpcEventActionConfig_TEventActionType.TEVAT_ITEM_EXCHANGE_BY_RULE,
            NpcEventActionConfig_TEventActionType.TEVAT_LONGMOVE_NORMAL,
            NpcEventActionConfig_TEventActionType.TEVAT_LONGMOVE_BUSINESS,
            NpcEventActionConfig_TEventActionType.TEVAT_SET_FORMATION,
            NpcEventActionConfig_TEventActionType.TEVAT_SHUANGXIU_FORMATION,
            NpcEventActionConfig_TEventActionType.TEVAT_WAITING_LINK_NPC,
            NpcEventActionConfig_TEventActionType.TEVAT_INRANGE_OPENWIN,
            NpcEventActionConfig_TEventActionType.TEVAT_GIVE_ITEM_TO_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_ASK_ITEM_FROM_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_STEAL_ITEM_FROM_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_TOBE_CONFIRMED,
            NpcEventActionConfig_TEventActionType.TEVAT_FIGHT_WITH_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_BUY_WITH_HIGH_PRICE,
            NpcEventActionConfig_TEventActionType.TEVAT_XIULIAN,
            NpcEventActionConfig_TEventActionType.TEVAT_GIVE_STATIC_ITEM_TO_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_COMPETITION,
            NpcEventActionConfig_TEventActionType.TEVAT_DUEL_WITH_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_PRODUCTION,
            NpcEventActionConfig_TEventActionType.TEVAT_DUEL_WITH_PLAYER,
            NpcEventActionConfig_TEventActionType.TEVAT_MARK_ITEM,
            NpcEventActionConfig_TEventActionType.TEVAT_NEW_QUEST,
            NpcEventActionConfig_TEventActionType.TEVAT_PRODUCTION_Steal,
            NpcEventActionConfig_TEventActionType.TEVAT_PRODUCTION_GETFRUNACE,
            NpcEventActionConfig_TEventActionType.TEVAT_NPC_DESTINY,
            NpcEventActionConfig_TEventActionType.TEVAT_SEND_SIGNAL,
            NpcEventActionConfig_TEventActionType.TEVAT_WAITING_SIGNAL,
            NpcEventActionConfig_TEventActionType.TEVAT_NPC_REVIVE,
            NpcEventActionConfig_TEventActionType.TEVAT_NPC_SOUL_SEAL,
            NpcEventActionConfig_TEventActionType.TEVAT_NPC_FIGHT_BOSS,
            NpcEventActionConfig_TEventActionType.TEVAT_GLOBAL_EMPTY,
            NpcEventActionConfig_TEventActionType.TEVAT_MOVE_TYPE,
            NpcEventActionConfig_TEventActionType.TEVAT_MOVE_TARGET,
            NpcEventActionConfig_TEventActionType.TEVAT_LONG_MOVE_TARGET,
            NpcEventActionConfig_TEventActionType.TEVAT_FIGHT_STAGE,
            NpcEventActionConfig_TEventActionType.TEVAT_FIGHT_N,
            NpcEventActionConfig_TEventActionType.TEVAT_DUEL_N,
            NpcEventActionConfig_TEventActionType.TEVAT_ENCOUNTER_ITEM_GET,
        };
        #endregion

        #region 运行时辅助变量
        //[PropertyOrder(-2)]
        [InfoBox("@sbError.ToString()", InfoMessageType.Error), HideIf("@sbError.Length == 0"), LabelText("【注意：上述错误信息，请尽快解决】"), HideReferenceObjectPicker, ShowInInspector]
        private StringBuilder sbError = new StringBuilder();

        /// <summary>
        /// 保存弹框显示信息
        /// </summary>
        private StringBuilder sbSaveInfo = new StringBuilder();

        /// <summary>
        /// 是否导出对应节点的C#代码文件（在配置信息后才会自动导出）
        /// </summary>
        private bool isSavingScript = false;

        /// <summary>
        /// 当前Inst对应json序列化数据
        /// </summary>       
        [JsonIgnore, HideInInspector]
        private static string jsonDataRecord;

        [JsonIgnore, HideInInspector]
        public Dictionary<string, Dictionary<int, ParamsAnnotation>> Type2ParamsAnn = new Dictionary<string, Dictionary<int, ParamsAnnotation>>();

        #endregion

        #region 编辑器查找
        [ShowInInspector, TabGroup("查找节点"), LabelText("双击选择节点类型"), ValueDropdown("GetNodeBaseAnnotation", DoubleClickToConfirm = true), OnValueChanged("OnValueChanged_Search")]
        private string search;
        [ShowInInspector, TabGroup("查找节点")/*, LabelText("节点类型详情")*/, HideLabel, HideReferenceObjectPicker, ShowIf("@searchAnnotation", null)]
        private NodeBaseAnnotation searchAnnotation;

        private List<NodeBaseAnnotation> nodeAnnotations = new List<NodeBaseAnnotation>();
        public IEnumerable<ValueDropdownItem> GetNodeBaseAnnotation()
        {
            foreach (var anno in GetNodeBaseAnnotations())
            {
                if (!string.IsNullOrEmpty(anno.ConfigName) && !string.IsNullOrEmpty(anno.NodeName))
                {
                    yield return new ValueDropdownItem($"{anno.ConfigName}/{anno.Name}_{anno.NodeName}", anno.Name);
                }
            }
        }
        private void OnValueChanged_Search()
        {
            if (string.IsNullOrEmpty(search))
            {
                searchAnnotation = null;
            }
            else
            {
                foreach (var anno in GetNodeBaseAnnotations())
                {
                    if (anno.Name == search)
                    {
                        searchAnnotation = anno;
                        break;
                    }
                }
            }
        }
        #endregion

        #region 记录数据
        [LabelText(Constants.EffectAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.EffectAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<ParamsAnnotation> EffectParams = new List<ParamsAnnotation>();

        [LabelText(Constants.SelectAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.SelectAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<ParamsAnnotation> SelectParams = new List<ParamsAnnotation>();

        [LabelText(Constants.ConditionAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.ConditionAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<ParamsAnnotation> ConditionParams = new List<ParamsAnnotation>();

        [LabelText(Constants.BaseConfigAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.BaseConfigAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false/*, HideAddButton = true, HideRemoveButton = true*/)]
        public List<ConfigAnnotation> BaseConfigAnnos = new List<ConfigAnnotation>();

        [LabelText(Constants.AITaskNodeAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.AITaskNodeAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<ParamsAnnotation> AITaskNodeParams = new List<ParamsAnnotation>();

        [LabelText(Constants.MapAnimAnnotaionSheetName), HideReferenceObjectPicker, TabGroup(Constants.MapAnimAnnotaionSheetName)]
        [ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        public List<ParamsAnnotation> MapAnimParams = new List<ParamsAnnotation>();

        [HideIf("@true")]
        public BattleNatureAnnotation BattleNatureAnno = new BattleNatureAnnotation();
        [HideIf("@true")]
        public EntityStateAnnotation EntityStateAnno = new EntityStateAnnotation();
        [HideIf("@true")]
        public CommonParamAnnotation CommonParamAnnotation = new CommonParamAnnotation();
        [HideIf("@true")]
        public CommonSkillParamAnnotation CommonSkillParamAnnotation = new CommonSkillParamAnnotation();
        [HideIf("@true"), JsonIgnore]
        public List<string> ExposedConfigNames = new List<string>();
        #endregion

        #region Interface
        //[Button("保存数据", ButtonSizes.Large), PropertyOrder(-1)]
        public bool SaveSetting(StringBuilder saveInfo)
        {
            try
            {
                sbSaveInfo.Length = 0;

                var savePath = PathSetting;
                isSavingScript = true;
                {
                    EditorUtility.DisplayProgressBar(name, "同步json数据", 0.2f);
                    SyncParams();

                    // 同步数据到json文件
                    EditorUtility.DisplayProgressBar(name, "保存json数据", 0.6f);
                    var jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
                    if (jsonData != jsonDataRecord)
                    {
                        jsonDataRecord = jsonData;
                        File.WriteAllText(savePath, jsonData);
                        var csInfo = sbSaveInfo.Length == 0 ? "" : "*.cs/";
                        sbSaveInfo.AppendLine($"\n\n【保存成功】\n\n【请上传对应文件({csInfo}TableAnnotation.json)】");
                        // 手动触发先变化事件
                        onChangedJson?.Invoke();
                    }
                    // 同步数据到json文件
                    EditorUtility.DisplayProgressBar(name, "生成节点类型代码", 0.9f);
                    GeneratePortTypes();
                }
                saveInfo.Append(sbSaveInfo);
                if (sbSaveInfo.Length > 0)
                {
                    return false;
                }
                else
                {
                    // 代码/json变化，刷新下
                    AssetDatabase.Refresh();
                }
                return false;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                isSavingScript = false;
                sbSaveInfo.Length = 0;
                EditorUtility.ClearProgressBar();
            }
            try
            {
                GUIUtility.ExitGUI();
            }
            catch { }
            return false;
        }
        public string PathSetting => Constants.SkillEditor.PathTableAnnotation;
        public List<string> PathCommitSetting => new List<string> { PathSetting, Constants.AutoGenerateDir };
        #endregion

        #region Public
        public TableAnnotation()
        {
            TurnFileWatcher(true);
        }
        /// <summary>
        /// 依据json文件实例化数据单例
        /// </summary>
        public static void CreateInstance()
        {
            try
            {
                jsonDataRecord = File.ReadAllText(Constants.SkillEditor.PathTableAnnotation);
                inst = JsonConvert.DeserializeObject<TableAnnotation>(jsonDataRecord);
                if (inst == null)
                {
                    throw new NullReferenceException($"加载文件失败");
                }
                // 初始化参数类声明
                foreach (var (paramAnno, enumType, _) in inst.GetParamsAnnotations())
                {
                    // 初始化加载
                    paramAnno.Load();

                    // 初始化描述 
                    if (!inst.Type2ParamsAnn.TryGetValue(enumType.Name, out var enum2desc))
                    {
                        enum2desc = new Dictionary<int, ParamsAnnotation>();
                        inst.Type2ParamsAnn.Add(enumType.Name, enum2desc);
                    }
                    enum2desc[paramAnno.EnumValue] = paramAnno;
                }
                // 初始化基础表格声明
                inst.ExposedConfigNames ??= new List<string>();
                inst.ExposedConfigNames.Clear();
                foreach (var baseAnno in inst.BaseConfigAnnos)
                {
                    baseAnno.Load();
                    // 收集暴露节点的表格名数据
                    if (baseAnno.IsShowNode)
                    {
                        inst.ExposedConfigNames.Add(baseAnno.ConfigName);
                    }
                }
                inst.BattleNatureAnno.UpdateVDList();
                inst.EntityStateAnno.UpdateVDList();
                inst.CommonParamAnnotation.UpdateVDList();
                inst.CommonSkillParamAnnotation.UpdateVDList();
            }
            catch (Exception ex)
            {
                throw new Exception($"解析声明文件失败: {Constants.SkillEditor.PathTableAnnotation}\n{ex}");
            }
        }
        public IEnumerable<NodeBaseAnnotation> GetNodeBaseAnnotations()
        {
            foreach (var anno in BaseConfigAnnos) yield return anno;
            foreach (var anno in EffectParams) yield return anno;
            foreach (var anno in SelectParams) yield return anno;
            foreach (var anno in ConditionParams) yield return anno;
            foreach (var anno in AITaskNodeParams) yield return anno;
            foreach (var anno in MapAnimParams) yield return anno;
        }
        public IEnumerable<(ParamsAnnotation, Type, string)> GetParamsAnnotations()
        {
            foreach (var anno in EffectParams) yield return (anno, typeof(TSkillEffectType), Constants.EffectAnnotaionSheetName);
            foreach (var anno in SelectParams) yield return (anno, typeof(TSkillSelectType), Constants.SelectAnnotaionSheetName);
            foreach (var anno in AITaskNodeParams) yield return (anno, typeof(AITaskNodeType), Constants.AITaskNodeAnnotaionSheetName);
            foreach (var anno in ConditionParams) yield return (anno, typeof(TSkillConditionType), Constants.ConditionAnnotaionSheetName);
            foreach (var anno in MapAnimParams) yield return (anno, typeof(MapAnimStateConfig_PointType), Constants.MapAnimAnnotaionSheetName);
        }
        /// <summary>
        /// 获取技能描述信息
        /// </summary>
        public ParamsAnnotation GetParamsAnnotation<T>(T type) where T : Enum
        {
            if (type == null) return null;
            if (Type2ParamsAnn.TryGetValue(type.GetType().Name, out var enum2desc))
            {
                if (enum2desc.TryGetValue(type.ToInt(), out var paramsAnnotation))
                {
                    return paramsAnnotation;
                }
            }
            return null;
        }
        public ConfigAnnotation GetConfigAnnotation<T>() where T : class
        {
            var type = typeof(T);
            return BaseConfigAnnos.Find(t => { return t.name == type.Name; });
        }
        /// <summary>
        /// 同步config.json数据
        /// </summary>
        /// <param name="isSave">是否保存导出C#代码</param>
        public void SyncParams()
        {
            sbError.Length = 0;
            // 序列化后，依据枚举数据，自动扩展生成数据，避免手动添加
            BattleNatureAnno.PresetParams();
            EntityStateAnno.PresetParams();
            CommonParamAnnotation.PresetParams();
            CommonSkillParamAnnotation.PresetParams();
            PresetParams<TSkillEffectType>(EffectParams);
            PresetParams<TSkillConditionType>(ConditionParams);
            PresetParams<TSkillSelectType>(SelectParams);
            PresetParams<AITaskNodeType>(AITaskNodeParams);
            PresetParams<MapAnimStateConfig_PointType>(MapAnimParams);
            PresetParams();
        }

        public Color GetNodeColor(Type type, Color defaultColor = default(Color))
        {
            if (type != null)
            {
                foreach (var baseAnno in BaseConfigAnnos)
                {
                    if (baseAnno.ConfigName == type.Name)
                    {
                        return baseAnno.ColorPick;
                    }
                }
            }
            return defaultColor;
        }

        public bool IsExposedConfig(string configName)
        {
            return ExposedConfigNames.Contains(configName);
        }
        #endregion

        #region Private
        private void PresetParams<T>(List<ParamsAnnotation> paramsAnnotations) where T : Enum
        {
            try
            {
                var enum2desc = new Dictionary<int, string>();
                var typeT = typeof(T);
                foreach (var enumValue in Enum.GetValues(typeT))
                {
                    var desc = Utils.GetEnumDescription((T)enumValue);
                    enum2desc.Add((int)enumValue, desc);
                }
                var descs = enum2desc.Values;
                // 清理被删除的类型
                paramsAnnotations.RemoveAll(p =>
                {
                    var res = !descs.Contains(p.name);
                    if (res)
                    {
                        sbError.AppendLine($"【删除】描述名： {p.name}");
                    }
                    return res;
                });
                foreach (var kv in enum2desc)
                {
                    var desc = kv.Value;
                    var paramsAnnotation = paramsAnnotations.Find((p) => { return p.name == desc; });
                    if (paramsAnnotation == null)
                    {
                        // 未找到配置信息,直接New一个
                        paramsAnnotation = new ParamsAnnotation(desc);
                        paramsAnnotations.Add(paramsAnnotation);
                        sbError.AppendLine($"【新增】描述名： {desc}");
                    }
                    paramsAnnotation.EnumValue = kv.Key;
                }
                SpecialConfigMap.TryGetValue(typeT, out var configName);
                var scriptDirPath = string.IsNullOrEmpty(configName) ? Constants.SubNodesScriptDir : $"{Constants.SubNodesScriptDir}/{configName}";
                // 强制刷新下数据
                var sbParamsContent = new StringBuilder();
                foreach (var p in paramsAnnotations)
                {
                    foreach (var a in p.paramsAnn)
                    {
                        a.ForceDoChange();
                    };
                    if (p.tips == ParamsAnnotation.DefaultTips)
                    {
                        if (p.name != "-")
                        {
                            sbError.AppendLine($"【未配置】描述名： {p.name}");
                        }
                    }
                    else
                    {
                        // 已配置参数信息的
                        // 依据枚举类型自动生成对应节点cs代码
                        if (isSavingScript)
                        {
                            var name = Enum.GetName(typeT, p.EnumValue);
                            p.NodeName = name;
                            p.ConfigName = configName;
                            p.PathDirNodeScript = scriptDirPath;
                            p.TypeParams = typeT;
                            var content = p.Save(sbSaveInfo);
                            sbParamsContent.AppendLine(content);
                        }
                    }
                };
                if (isSavingScript)
                {
                    // 参数细化代码统一在一个文件，方便清理废弃数据，以及查看等
                    var commonPath = string.IsNullOrEmpty(configName) ? $"{scriptDirPath}/ParamsAutoGenerated.cs" : $"{scriptDirPath}/{configName}_ParamsAutoGenerated.cs";
                    var newTemplateClassContent = sbParamsContent.ToString();
                    var commonContent = ParamsAnnotation.TemplateFileContent.Replace(ParamsAnnotation.TemplateClassContent, newTemplateClassContent);
                    NodeBaseAnnotation.WriteToFile(commonPath, commonContent, sbSaveInfo);
                }

                // TODO 清理废弃的cs代码
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        private void PresetParams()
        {
            Utils.SafeCall(() =>
            {
                foreach (var p in BaseConfigAnnos)
                {
                    // 自动生成代码 && 错误提示等
                    p.Check(sbError);
                }
                // 排序
                BaseConfigAnnos.Sort((a, b) =>
                {
                    var aName = string.IsNullOrEmpty(a.name) ? string.Empty : a.name;
                    var bName = string.IsNullOrEmpty(b.name) ? string.Empty : b.name;
                    return aName.CompareTo(bName);
                });

                // TODO 检测Excel是否有相关sheet
                if (isSavingScript)
                {
                    var partialContentDic = new Dictionary<string, (string, StringBuilder)>
                    {
                        { "TableDR_CS.NotHotfix.dll", (Constants.TablePartialPathNotHotfix, new StringBuilder() ) },
                        { "TableDR_CS.dll", (Constants.TablePartialPathHotfix, new StringBuilder() ) },
                    };
                    var sbAnnoContent = new StringBuilder();
                    foreach (var p in BaseConfigAnnos)
                    {
                        // 自动生成代码
                        var content = p.Save(sbSaveInfo);
                        sbAnnoContent.AppendLine(content);
                        var refType = TableHelper.GetTableType($"{Constants.TableNameSpace}.{p.Name}");
                        if (refType != null)
                        {
                            if (partialContentDic.TryGetValue(refType.Assembly.ManifestModule.Name, out var path2SB))
                            {
                                path2SB.Item2.AppendLine($"\tpublic partial class {p.Name} : {nameof(EditorBaseConfig)} {{ }}");
                            }
                        }
                    }
                    // 参数细化代码统一在一个文件，方便清理废弃数据，以及查看等
                    var scriptDirPath = Constants.NodesScriptDir;
                    var commonPath = $"{scriptDirPath}/BaseConfig_AutoGenerated.cs";
                    var newTemplateClassContent = sbAnnoContent.ToString();
                    var commonContent = ConfigAnnotation.TemplateFileContent.Replace(ConfigAnnotation.TemplateClassContent, newTemplateClassContent);
                    NodeBaseAnnotation.WriteToFile(commonPath, commonContent, sbSaveInfo);

                    // 生成部分类定义
                    Utils.SafeCall(() =>
                    {
                        var scriptContent = Utils.ReadAllText(Constants.TablePartialTemplatePath);
                        foreach ((string filePath, StringBuilder partialContent) in partialContentDic.Values)
                        {
                            // 基础config partial定义代码生成
                            var content = scriptContent.Replace("#TABLE_PARTIAL#", $"{partialContent}");
                            NodeBaseAnnotation.WriteToFile(filePath, content, sbSaveInfo);
                        }
                    });

                    // 生成csv说明
                    var contentCSV = string.Empty;
                    var annoInfos = new Dictionary<string, StringBuilder>();
                    foreach (var (paramsAnno, enumType, annoDesc) in GetParamsAnnotations())
                    {
                        if (!annoInfos.TryGetValue(annoDesc, out var sb))
                        {
                            annoInfos.Add(annoDesc, sb = new StringBuilder());
                        }
                        sb.AppendLine(paramsAnno.ToString());
                    }
                    int i = 0;
                    foreach (var annoInfo in annoInfos)
                    {
                        var csvPath = $"{Constants.AnnotationDirPath}{++i}_{annoInfo.Key}.csv";
                        //if (File.Exists(csvPath))
                        //{
                        //    File.Delete(csvPath);
                        //}
                        string directoryName = Path.GetDirectoryName(csvPath);
                        if (!Directory.Exists(directoryName))
                        {
                            Directory.CreateDirectory(directoryName);
                        }
                        var splitInfos = annoInfo.Value.ToString().Split(',');
                        using (StreamWriter stream = new StreamWriter(csvPath, false, System.Text.Encoding.UTF8))
                        {
                            stream.WriteLine("类型,说明,参数列表");
                            stream.Write(annoInfo.Value);
                        }
                    }

                    // 生成html说明
                    var htmlContent = Utils.ReadAllText(Constants.AnnotationHtmlTemplatePath);
                    var sbContent = new StringBuilder();
                    var sbTitle = new StringBuilder();
                    var lastTab = string.Empty;
                    // 1-生成所有节点相关说明
                    foreach (var (paramsAnno, enumType, annoDesc) in GetParamsAnnotations())
                    {
                        var keyTab = annoDesc;
                        if (lastTab != keyTab)
                        {
                            if (lastTab != string.Empty)
                            {
                                sbContent.AppendLine("        </div>");
                            }
                            sbTitle.AppendLine($"        <div class=\"toc-item\" data-target=\"{keyTab}\">{keyTab}</div>");
                            sbContent.AppendLine($"        <div class=\"content-section\" id=\"{keyTab}\">");
                            lastTab = keyTab;
                        }
                        var enumName = Enum.GetName(enumType, paramsAnno.EnumValue);
                        var nodeName = $"{paramsAnno.EnumValue}  {paramsAnno.name}  {enumName}";
                        var nodeDesc = paramsAnno.tips;
                        var paramsAnnInfos = paramsAnno.paramsAnn.Select((anno, index) => $"{index + 1}-{anno.ToString()}");
                        var paramsAnnoStr = $"{string.Join("\n", paramsAnnInfos)}";
                        var nodeType = Utils.GetEnumAllFlagsText(paramsAnno.NodeTypeFlags);
                        var contentNode = HtmlUtils.GenerateNodeHtml(nodeName, nodeDesc, paramsAnnoStr, nodeType, "            ");
                        sbContent.AppendLine(contentNode);
                    }
                    sbContent.AppendLine("        </div>");
                    // 2-生成所有模板相关说明
                    var titleTemplate = "战斗-模板列表";
                    sbTitle.AppendLine($"        <div class=\"toc-item\" data-target=\"{titleTemplate}\">{titleTemplate}</div>");
                    sbContent.AppendLine($"        <div class=\"content-section\" id=\"{titleTemplate}\">");
                    {
                        foreach (var (configName, dicInfos) in JsonGraphManager.Inst.Template2Nodes)
                        {
                            foreach (var (configID, nodeInfo) in dicInfos)
                            {
                                if (nodeInfo.Owner == null) continue;
                                var paramsAnnInfos = nodeInfo.TemplateParams.Select((anno, index) => $"{index + 1}-{anno.ToString()}");
                                var paramsAnnoStr = $"{string.Join("\n", paramsAnnInfos)}";
                                var nodeType = Utils.GetEnumAllFlagsText(nodeInfo.TemplateFlags);
                                var contentNode = HtmlUtils.GenerateNodeHtml(nodeInfo.Owner.graphName, nodeInfo.Desc, paramsAnnoStr, nodeType, "            ");
                                sbContent.AppendLine(contentNode);
                            }
                        }
                    }
                    sbContent.AppendLine("        </div>");
                    // 替换模板数据
                    htmlContent = htmlContent.
                        Replace("<!-- 内容 -->", sbContent.ToString()).
                        Replace("<!-- 标题 -->", sbTitle.ToString());
                    Utils.WriteAllText(Constants.AnnotationHelpPath, htmlContent);
                }
            });
        }
        #endregion

        #region 自动生成表格节点类型
        /// <summary>
        /// 生成技能编辑器用到的表格节点类型
        /// </summary>
        private void GeneratePortTypes()
        {
            Utils.SafeCall(() =>
            {
                var oldFileContent = File.ReadAllText(Constants.TablePortTypeScriptPath);
                var sbCode = new StringBuilder();
                var portTypeClass = new HashSet<string>();
                var portTypeInterface = new HashSet<string>();
                sbCode.AppendLine("// 注意！！此代码文件由工具自动生成！！ ");
                sbCode.AppendLine("// 相关基类见：TablePortTypesHelper.cs ");
                sbCode.AppendLine();
                sbCode.AppendLine("using TableDR;");
                sbCode.AppendLine();
                sbCode.AppendLine("namespace NodeEditor.PortType");
                sbCode.AppendLine("{");
                sbCode.AppendLine("    #region 自动生成类型代码");
                // 以BaseConfigAnnos为准
                foreach ( var BaseConfigAnno in BaseConfigAnnos)
                {
                    // 仅针对编辑器需要的数据
                    if (BaseConfigAnno.IsShowNode)
                    {
                        var configName = BaseConfigAnno.ConfigName;
                        var portClassName = TablePortTypesHelper.GetConfigPortTypeClassName(configName);
                        var portInterfaceName = TablePortTypesHelper.GetConfigInterfaceName(configName);
                        portTypeInterface.Add($"    public interface {portInterfaceName} {{ }}");
                        portTypeClass.Add($"    public class {portClassName} : {nameof(ConfigPortType)}, {portInterfaceName} {{ }}");
                    }
                }
                var paramsAll = new List<ParamsAnnotation>();
                paramsAll.AddRange(EffectParams);
                paramsAll.AddRange(ConditionParams);
                paramsAll.AddRange(SelectParams);
                paramsAll.AddRange(AITaskNodeParams);
                // 添加参数引用的类型声明
                foreach (var paramSingle in paramsAll)
                {
                    foreach (var paramAnn in paramSingle.paramsAnn)
                    {
                        if (!string.IsNullOrEmpty(paramAnn.RefPortTypeFullName))
                        {
                            var classCode = string.Empty;
                            for (int i = 0, length = paramAnn.RefPortTypeNameList.Count; i < length; i++)
                            {
                                var portTypeName = paramAnn.RefPortTypeNameList[i];
                                var portInterfaceName = TablePortTypesHelper.GetConfigInterfaceName(portTypeName);
                                portTypeInterface.Add($"    public interface {portInterfaceName} {{ }}");
                                classCode += $", {portInterfaceName}";
                            }
                            classCode = $"    public class {paramAnn.RefPortTypeName} : {nameof(ConfigPortType)}{classCode} {{ }}";
                            portTypeClass.Add(classCode);
                        }
                    }
                }
                // 添加基础节点类型声明
                foreach (var baseConfigAnnos in BaseConfigAnnos)
                {
                    if (baseConfigAnnos.Check(null))
                    {
                        var configName = baseConfigAnnos.name;
                        var portClassName = TablePortTypesHelper.GetConfigPortTypeClassName(configName);
                        var portInterfaceName = TablePortTypesHelper.GetConfigInterfaceName(configName);
                        portTypeInterface.Add($"    public interface {portInterfaceName} {{ }}");
                        // 添加基础节点类型的引用类型
                        var classCode = $"    public class {portClassName} : {nameof(ConfigPortType)}, {portInterfaceName} {{ }}";
                        portTypeClass.Add(classCode);
                    }
                }
                var interfaceList = portTypeInterface.ToList();
                interfaceList.Sort();
                foreach (var interfaceInfo in interfaceList)
                {
                    sbCode.AppendLine(interfaceInfo);
                }
                var classList = portTypeClass.ToList();
                classList.Sort();
                foreach (var classInfo in classList)
                {
                    sbCode.AppendLine(classInfo);
                }
                sbCode.AppendLine("    #endregion");
                sbCode.AppendLine("}");
                var newFileContent = sbCode.ToString();
                var appendInfo = string.Empty;
                if (newFileContent != oldFileContent)
                {
                    File.WriteAllText(Constants.TablePortTypeScriptPath, newFileContent);
                    appendInfo = $"【请上传对应文件({Constants.TablePortTypeScriptPath})】";
                }
                sbSaveInfo.Append(appendInfo);
            });
        }
        #endregion

        #region 文件变化监听
        // 表格文件变动监听
        private static FileSystemWatcher fileWatcherJson;
        [Conditional("UNITY_EDITOR")]
        private static void TurnFileWatcher(bool on)
        {
            if (fileWatcherJson == null)
            {
                var info = new FileInfo(Constants.SkillEditor.PathTableAnnotation);
                fileWatcherJson = new FileSystemWatcher(info.DirectoryName, info.Name)
                {
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Security,
                };
                fileWatcherJson.Created += OnFileChangedJson;
                fileWatcherJson.Changed += OnFileChangedJson;
                fileWatcherJson.Deleted += OnFileChangedJson;
                fileWatcherJson.Renamed += OnFileChangedJson;
            }
            fileWatcherJson.EnableRaisingEvents = on;
        }
        private static void OnFileChangedJson(object sender, FileSystemEventArgs e)
        {
            // json文件修改，单例重新获取
            if (!Inst.isSavingScript)
            {
                inst = null;
                _ = Inst;
            }
            onChangedJson?.Invoke();
        }
        #endregion
    }
}

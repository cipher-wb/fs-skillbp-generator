using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public interface IConfigEditorManager : ISetting
    {
        string Version { get; }
        string Name { get; }
        string Path { get; }
        string PathSaves { get; }
        string PathSavesJsons { get; }
        string PathSavesGraphs { get; }
        Type GraphType { get; }
        Type WindowType { get; }
        ConfigEditorSetting Setting { get; }
        void Init();
    }
    public abstract partial class ConfigEditorManager : IConfigEditorManager
    {
        #region interface
        [ShowInInspector, LabelText("版本号"), TitleGroup("基础信息", order: -10, indent: true), BoxGroup("基础信息/Box", false)]
        public virtual string Version => throw new NotImplementedException();

        [ShowInInspector, LabelText("名称"), TitleGroup("基础信息"), BoxGroup("基础信息/Box", false)]
        public virtual string Name => throw new NotImplementedException();

        [ShowInInspector, LabelText("路径"), TitleGroup("基础信息"), BoxGroup("基础信息/Box", false)]
        public virtual string Path => throw new NotImplementedException();

        [ShowInInspector, Button("打开编辑器目录"), TitleGroup("基础信息"), BoxGroup("基础信息/Box", false)]
        public void OpenDirectory()
        {
            Utils.OpenDirectory(Path);
        }

        /// <summary>
        /// 编辑器技能配置数据保存文件夹
        /// </summary>
        public string PathSaves => Path + "/Saves";

        /// <summary>
        /// 编辑器Json资源保存路径
        /// </summary>
        public string PathSavesJsons => PathSaves + "/Jsons";

        /// <summary>
        /// 编辑器Graphs资源保存路径
        /// </summary>
        public string PathSavesGraphs => PathSaves + "/Graphs";
        #endregion

        public abstract Type GraphType { get; }

        public abstract Type WindowType { get; }

        [TitleGroup("编辑器配置信息", indent: true), BoxGroup("编辑器配置信息/Box", false), HideLabel, ShowInInspector, HideReferenceObjectPicker]
        public ConfigEditorSetting Setting { get; protected set; }

        public virtual string PathSetting => throw new NotImplementedException();

        public virtual List<string> PathCommitSetting => throw new NotImplementedException();

        public abstract void Init();

        public virtual bool SaveSetting(StringBuilder saveInfo)
        {
            return Setting.SaveSetting(saveInfo);
        }

        public void GraphBatchProcessing(BatchFlags batchFlags, EditorFlag editorFlag = EditorFlag.DisplayDialog | EditorFlag.DisplayProcess, List<string> configNames = null)
        {
            if (batchFlags == BatchFlags.空)
            {
                editorFlag.DisplayDialog(Name, "未选择文件操作类型", "好的");
                return;
            }
            // 弹框二次确认下
            if (!editorFlag.DisplayDialog(Name, "是否执行？", "是√", "否×"))
            {
                return;
            }
            if (batchFlags.HasFlag(BatchFlags.清理无效ID) && !batchFlags.HasFlag(BatchFlags.导出Excel))
            {
                // 清理无效ID必须勾选导出Exce
                batchFlags |= BatchFlags.导出Excel;
            }
            var title = $"{Name}: 文件处理";
            Utils.DisplayProcess(title, (sbInfo) =>
            {
                var excelPath = Setting.PathExportExcel;
                var excelName = Setting.ExcelName;
                editorFlag.DisplayProgressBar(title, $"开始处理...[{excelName}]", 0.05f);
                // 如果导出表格，检查表格是否打开
                if (batchFlags.HasFlag(BatchFlags.导出Excel))
                {
                    while (Utils.IsFileOpened(excelPath))
                    {
                        var displayDialogInfo = $"【{excelName}】表格处于被打开状态/只读模式，请先关闭！";
                        if (editorFlag.DisplayDialog(Name, displayDialogInfo, "已关闭，继续导出", "不导出了", false))
                        {
                            continue;
                        }
                        else
                        {
                            return;
                        }
                    }
                    // 注意：可能存在id引用情况
                    if (batchFlags.HasFlag(BatchFlags.清理无效ID))
                    {
                        Utils.WatchTime($"清空Excel数据: {excelName}", () =>
                        {
                            editorFlag.DisplayProgressBar(title, $"清理Excel数据...", 0.1f);
                            ExcelManager.Inst.CleanSheets(excelPath, (sheetName) =>
                            {
                                foreach (var configName in DesignTable.EditorConfigNames)
                                {
                                    if (sheetName.Contains(configName) && (configNames == null || configNames.Count == 0 || configNames.Contains(configName)))
                                    {
                                        return true;
                                    }
                                }
                                return false;
                            });
                        });
                    }
                    // 如果不涉及Graph重新保存，优化导出，无需走打开Graph操作
                    if (!batchFlags.HasFlag(BatchFlags.保存Graph))
                    {
                        Utils.WatchTime($"导出Excel数据-解析Json: {excelName}", () =>
                        {
                            var graphFiles = Directory.GetFiles(PathSavesJsons, $"*.json", SearchOption.AllDirectories);
                            editorFlag.DisplayProgressBar(title, $"导出Excel...共{graphFiles.Length}个文件", 0.1f);

                            if (configNames?.Count > 0)
                            {
                                ExternalExportUtil.ExportNodeJson2Excel(graphFiles.ToList(), (config) =>
                                {
                                    if (config != null && configNames.Contains(config.GetType().Name))
                                    {
                                        return true;
                                    }
                                    return false;
                                });
                            }
                            else
                            {
                                ExternalExportUtil.ExportNodeJson2Excel(graphFiles.ToList());
                            }
                        });
                        return;
                    }
                    else
                    {
                        Utils.WatchTime($"导出Excel数据-打开Graph: {excelName}", () =>
                        {
                            var configs = new List<object>();
                            GraphHelper.ProcessGraphs(GraphType, (manager, graph, i, length) =>
                            {
                                if (editorFlag.DisplayCancelableProgressBar($"{title}, 收集数据...[{i}/{length}]", $"{graph.name}", i / (float)length * 0.7f + 0.1f, false))
                                {
                                    // 取消导出
                                    batchFlags = 0;
                                    return;
                                }
                                foreach (var node in graph.nodes)
                                {
                                    if (node is IConfigBaseNode configNode/* && node.CanExport()*/)
                                    {
                                        if (configNames == null || configNames.Count == 0 || configNames.Contains(configNode.GetConfigName()))
                                        {
                                            configs.Add(configNode.GetConfig());
                                        }
                                    }
                                }
                                if (batchFlags.HasFlag(BatchFlags.保存Graph)) graph.SaveGraphToDisk();
                            }, () => { return batchFlags == 0; });

                            if (configs.Count > 0 && batchFlags.HasFlag(BatchFlags.导出Excel))
                            {
                                editorFlag.DisplayProgressBar(title, $"导出Excel...{excelName}", 0.9f);

                                // Excel数据写入，数据排序，清理空行
                                ExcelManager.Inst.AddExcelMemberRefillableAction("NpcTemplateRuleConfig", (propName, configObj) =>
                                {
                                    if (propName == "RoleCommonProperty")
                                    {
                                        return false;
                                    }
                                    return true;
                                });

                                ExcelManager.Inst.WriteExcel(excelPath, configs);

                                ExcelManager.Inst.RemoveExcelMemberRefillableAction("NpcTemplateRuleConfig", default);
                            }
                        });
                    }
                }
            }, editorFlag: editorFlag);
            if (batchFlags.HasFlag(BatchFlags.保存Graph)) AssetDatabase.Refresh();
        }
    }
    public partial class ConfigEditorManager<TManager, TSetting, TGraph, TWindow> : ConfigEditorManager
        where TManager : IConfigEditorManager, new()
        where TSetting : ConfigEditorSetting, new()
        where TGraph : ConfigGraph
        where TWindow : ConfigGraphWindow
    {
        #region static
        private static bool hasInstanced = false;
        protected static TManager inst;
        public static TManager Inst
        {
            get
            {
                if (!hasInstanced)
                {
                    hasInstanced = true;
                    inst = Activator.CreateInstance<TManager>();
                    inst.Init();
                }
                return inst;
            }
        }
        public static bool IsInEditor { get { return inst != null; } }
        #endregion

        public override Type GraphType => typeof(TGraph);
        public override Type WindowType => typeof(TWindow);
        public ConfigEditorManager()
        {
            Setting = ConfigEditorSetting.Load<TSetting>(PathSetting);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            //设置默认的首个节点
            SetDefaultFirstNode();
        }

        #region 编辑器-保存
        [HideIf("@true")]
        public override string PathSetting => $"{PathSaves}/{ConfigEditorSetting.Name}";
        public override List<string> PathCommitSetting => new List<string> { PathSetting };
        #endregion

        #region 编辑器-新建graph
        protected virtual string GraphCreateInfo
        {
            get
            {
                var info = $"文件名：{GraphCreateFileName}";
                if (graphCreatedFirstNodeID == 0 && isNameAppendNodeID)
                {
                    info += "\n请选择首个节点！！";
                }
                if (string.IsNullOrEmpty(graphCreatedDesc))
                {
                    info += "\n请配置Graph描述！！";
                }
                return info;
            }
        }
        protected string GraphCreateFileName 
        {
            get
            {
                if (isNameAppendNodeID)
                {
                    return $"{GraphType.Name}_{graphCreatedFirstNodeID}_{graphCreatedDesc}.json";
                }
                else
                {
                    return $"{GraphType.Name}_{graphCreatedDesc}.json";
                }
            }
        }

        [InfoBox("$GraphCreateInfo", InfoMessageType.Warning)]
        [TitleGroup("编辑器操作", order: -5, indent: true), TitleGroup("编辑器操作/新建", order: -4, indent: true)]
        [LabelText("首个节点名 ："), ShowInInspector, ValueDropdown("GetNodeValidNames", DoubleClickToConfirm = true, ExpandAllMenuItems = true), OnValueChanged("OnValueChanged_NodeName")]
        protected string graphCreatedFirstNodeName = string.Empty;

        [TitleGroup("编辑器操作/新建")]
        [LabelText("首个节点ID ："), ShowInInspector, EnableIf("@false"), ShowIf("IsShowFirstNodeID")]
        private int graphCreatedFirstNodeID = 0;

        [TitleGroup("编辑器操作/新建")]
        [LabelText("名称是否拼接ID"), ShowInInspector, ShowIf("IsShowFirstNodeID")]
        private bool isNameAppendNodeID = true;

        /// <summary>
        /// 是否显示首个节点ID
        /// </summary>
        protected virtual bool IsShowFirstNodeID => true;

        [TitleGroup("编辑器操作/新建")]
        [LabelText("拷贝视图 ："), ShowInInspector, ValueDropdown("GetCopyGraphNames", DoubleClickToConfirm = true), ShowIf("IsShowGraphCopy")]
        protected string copyGraphName;

        /// <summary>
        /// 是否显示拷贝列表
        /// </summary>
        protected virtual bool IsShowGraphCopy => false;

        /// <summary>
        /// 是否拷贝节点
        /// </summary>
        private bool IsCopyGraph => !string.IsNullOrEmpty(copyGraphName);

        [TitleGroup("编辑器操作/新建")]
        [LabelText("Graph描述 ："), ShowInInspector, InlineButton("GenerateGraph", "点击创建")]
        protected string graphCreatedDesc;

        /// <summary>
        /// 获取需要拷贝的graph
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<ValueDropdownItem> GetCopyGraphNames()
        {
            yield return new ValueDropdownItem("空", string.Empty);

            var graghFiles = Directory.GetFiles(PathSavesJsons, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < graghFiles.Length; i++)
            {
                var graghFile = graghFiles[i];
                Utils.PathFormat(ref graghFile);
                var fileName = Utils.PathFull2SeparationFolder(graghFile, "Jsons/");
                yield return new ValueDropdownItem(fileName, graghFile);
            }
        }

        /// <summary>
        /// 设置默认的首个节点
        /// </summary>
        protected virtual void SetDefaultFirstNode()
        {
            graphCreatedFirstNodeName = string.Empty;
        }
        private Type GetGraphCreateFirstNodeType()
        {
            Type nodeType = null;
            if (!string.IsNullOrEmpty(graphCreatedFirstNodeName))
            {
                var type = typeof(ConfigBaseNode);
                nodeType = type.Assembly.GetType($"{type.Namespace}.{graphCreatedFirstNodeName}");
            }
            return nodeType;
        }
        protected virtual IEnumerable<ValueDropdownItem> GetNodeValidNames()
        {
            foreach (var anno in TableAnnotation.Inst.GetNodeBaseAnnotations())
            {
                if (!string.IsNullOrEmpty(anno.ConfigName) && !string.IsNullOrEmpty(anno.NodeName))
                {
                    if (anno.IsCompatibleWithGraph(GraphType))
                    {
                        if (anno.IsConfigDerive())
                        {
                            yield return new ValueDropdownItem($"{anno.ConfigName}/{anno.Name}", anno.NodeName);
                        }
                        else
                        {
                            yield return new ValueDropdownItem($"{anno.ConfigName}", anno.NodeName);
                        }
                    }
                }
            }
        }
        protected void OnValueChanged_NodeName()
        {
            graphCreatedFirstNodeID = 0;
            Type nodeType = GetGraphCreateFirstNodeType();
            if (nodeType != null)
            {
                foreach (var anno in TableAnnotation.Inst.GetNodeBaseAnnotations())
                {
                    if (anno.NodeName == graphCreatedFirstNodeName)
                    {
                        graphCreatedFirstNodeID = ConfigIDManager.Inst.GetNextConfigID(anno.ConfigName, isRecord: false);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 生成视图
        /// </summary>
        private void GenerateGraph()
        {
            if (IsCopyGraph)
            {
                CopyGraph();
            }
            else
            {
                CreateGraph();
            }
        }

        /// <summary>
        /// 新建视图
        /// </summary>
        private void CreateGraph()
        {
            // 刷新ID等
            OnValueChanged_NodeName();
            if (graphCreatedFirstNodeID == 0 || string.IsNullOrEmpty(graphCreatedDesc))
            {
                EditorUtility.DisplayDialog(Name, GraphCreateInfo, "好的");
                return;
            }
            var graphPath = GraphHelper.CreateGraph(GraphType, null, GraphCreateFileName);
            var graph = GraphHelper.LoadGraph(GraphType, graphPath);
            var win = GraphAssetCallbacks.InitializeGraph(graph, graphPath);
            if (win is ConfigGraphWindow configGraphWindow)
            {
                var graphView = configGraphWindow.GetGraphView();
                if (graphView != null)
                {
                    Type nodeType = GetGraphCreateFirstNodeType();
                    var node = GraphProcessor.BaseNode.CreateFromType(nodeType, Vector2.zero);
                    var nodeView = graphView.AddNode(node);
                    nodeView.FocusSelect();
                }
            }
        }

        /// <summary>
        /// 拷贝视图
        /// </summary>
        private void CopyGraph()
        {
            // 刷新ID等
            OnValueChanged_NodeName();

            if (graphCreatedFirstNodeID == 0 || string.IsNullOrEmpty(graphCreatedDesc))
            {
                EditorUtility.DisplayDialog(Name, GraphCreateInfo, "好的");
                return;
            }

            //获取要拷贝的视图
            GraphHelper.ProcessGraph(copyGraphName, (copyGraph) =>
            {
                //创建新的试图
                var createGraphPath = GraphHelper.CreateGraph(GraphType, null, GraphCreateFileName);
                var createGraph = GraphHelper.LoadGraph(GraphType, createGraphPath);
                var win = GraphAssetCallbacks.InitializeGraph(createGraph, createGraphPath);
                if (win is ConfigGraphWindow configGraphWindow)
                {
                    var graphView = configGraphWindow.GetGraphView();
                    if (graphView != null)
                    {
                        GraphHelper.CopyGraph(graphView, copyGraph, Vector2.zero, default);
                    }
                }

                copyGraphName = String.Empty;
            });
        }
        #endregion

        #region 编辑器-查找
        [TitleGroup("编辑器操作/查找", indent: true, order: -2), ShowInInspector, HideLabel, HideReferenceObjectPicker]
        private GraphDataSearch graphDataSearch = new GraphDataSearch();
        #endregion

        #region 编辑器-文件处理
        [LabelText("指定表格（如：SkillConfig|BuffConfig）"), TitleGroup("编辑器操作/文件处理", indent: true, order: -1)]
        public string ConfigNames = string.Empty;

        [LabelText("批处理选项"), TitleGroup("编辑器操作/文件处理", indent: true, order: -1), HorizontalGroup("编辑器操作/文件处理/1")]
        public BatchFlags GraphBatchFlags = BatchFlags.导出Excel;

        [Button("点击处理"), TitleGroup("编辑器操作/文件处理"), HorizontalGroup("编辑器操作/文件处理/1")]
        public void GraphBatchProcessing()
        {
            var configNames = string.IsNullOrEmpty(ConfigNames) ? null : ConfigNames.Split("|").ToList();
            GraphBatchProcessing(GraphBatchFlags, EditorFlag.DisplayDialog | EditorFlag.DisplayProcess, configNames);
        }
        #endregion

        #region Excel反导
        [InfoBox(
            "【Excel数据反导到json文件】\n" +
            "【注意】多语言字段需要单独导出，比如填：表格名：SkillConfig，字段名：SkillName\n" +
            "【注意】多语言字段导出，直接读取Excel字段，无需导表\n" +
            "【注意】非多语言字段/整表导出，需要保证一键全部导表，即保证多语言表正常导出，且SVN更到最新，保证Excel数据最新\n" +
            "【注意】导出时间可能较长，提交json前务必检查对比查看数据是否正确", InfoMessageType.Warning)]
        [LabelText("同步表格名"), TitleGroup("编辑器操作/Excel反导", indent: true, order: -1), ShowInInspector]
        private string excelReverseExport_ConfigName = "如：SkillConfig";

        [LabelText("同步表格列名/字段名"), TitleGroup("编辑器操作/Excel反导"), ShowInInspector]
        private string excelReverseExport_ColumnName = "如：技能名/SkillName，不填表示全表格替换";

        [Button("点击处理"), TitleGroup("编辑器操作/Excel反导")]
        private void ExcelReverseExport()
        {
            var configName = excelReverseExport_ConfigName;
            var memberNameOrColumnName = excelReverseExport_ColumnName; // 注意可能是列名/变量名
            if (string.IsNullOrEmpty(configName))
            {
                EditorUtility.DisplayDialog(Name, "查找信息为空，请检查", "好的");
                return;
            }
            var configType = TableHelper.GetTableType(TableHelper.ToTableFullName(configName));
            // 遍历所有表格配置
            var tableManager = DesignTable.GetTableManager(configName);
            if (tableManager == null)
            {
                EditorUtility.DisplayDialog(Name, $"未查到对应表格:{configName}", "好的");
                return;
            }
            if (!EditorUtility.DisplayDialog(Name, $"是否执行操作", "执行", "取消"))
            {
                return;
            }
            var title = "批处理Excel数据反向写入Json";
            var memberProperty = configType.GetProperty(memberNameOrColumnName);
            PropertyInfo memberEditorProperty = null;
            if (memberProperty == null)
            {
                // 没有按照变量名填写，再按照描述找下
                var properties = configType.GetProperties();
                foreach (var property in properties)
                {
                    var attr = property.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                    if (attr != null && attr.Description == memberNameOrColumnName)
                    {
                        memberProperty = property;
                        break;
                    }
                }
            }
            if (memberProperty == null && !string.IsNullOrEmpty(memberNameOrColumnName))
            {
                EditorUtility.DisplayDialog(title, $"未查到对应表格列:{memberNameOrColumnName}", "好的");
                return;
            }
            // 如果是多语言字段，直接读取Excel文件缓存编辑器数据
            CacheExcelData cacheExcelData = null;
            var excelPath = Setting.PathExportExcel;
            if (!string.IsNullOrEmpty(memberNameOrColumnName))
            {
                if (memberProperty == null)
                {
                    EditorUtility.DisplayDialog(title, $"未查到对应表格列:{memberNameOrColumnName}", "好的");
                    return;
                }
                else
                {
                    var memberPropertyName = memberProperty.Name;
                    var localizeKey = ExcelManager.Inst.ProjectConfig.LocalizeKey;
                    // 可能存在多语言字段
                    if (ExcelManager.Inst.ProjectConfig.IsMemberLocalize(configType.Name, memberPropertyName))
                    {
                        var memberNameReal = memberPropertyName.Remove(memberPropertyName.Length - localizeKey.Length, localizeKey.Length);
                        memberEditorProperty = configType.GetProperty(memberNameReal + "Editor");
                    }
                    else if (ExcelManager.Inst.ProjectConfig.IsMemberLocalize(configType.Name, memberPropertyName + localizeKey))
                    {
                        // 如果是多语言字段
                        memberEditorProperty = configType.GetProperty(memberPropertyName + "Editor");
                    }
                    if (memberEditorProperty != null)
                    {
                        cacheExcelData = ExcelManager.Inst.GetCacheExcelData(excelPath, configName);
                        if (cacheExcelData == null)
                        {
                            EditorUtility.DisplayDialog(Name, $"缓存表格数据失败:{excelPath}", "好的");
                            return;
                        }
                    }
                }
            }

            Utils.DisplayProcess(title, (sb) =>
            {
                //DesignTable.Reload();
                if (memberProperty == null)
                {
                    // 找到涉及到对应表格的编辑器文件
                    if (JsonGraphManager.Inst.TryGetGraphInfos(configName, null, out var graphInfos))
                    {
                        for (int i = 0, count = graphInfos.Count; i < count; i++)
                        {
                            var graphInfo = graphInfos[i];
                            var graphPath = graphInfo.graphPath;
                            var fileName = System.IO.Path.GetFileName(graphPath);
                            EditorUtility.DisplayProgressBar($"{title}[{i}/{count}]", $"开始处理...{fileName}", (float)i / count);
                            // 打开graph
                            GraphHelper.ProcessGraph(graphPath, (graph) =>
                            {
                                // 整体替换表格数据
                                foreach (var node in graph.nodes)
                                {
                                    if (node is IConfigBaseNode configNode && configNode.GetConfigName() == configName)
                                    {
                                        var config = configNode.GetConfig();
                                        var id = configNode.GetID();
                                        var configExcel = DesignTable.GetTableCell(configName, id);
                                        if (configExcel != null)
                                        {
                                            configNode.ExSetValue("Config", configExcel);
                                        }
                                    }
                                }
                                // 保存，注：更新表格数据已在ConfigBaseNode<T>.Deserialize()处理
                                graph.SaveGraphToDisk();
                                Log.Debug($"{title}[{i}/{count}]: {fileName}");
                            });
                        }
                    }
                }
                else
                {
                    // 优化逻辑，遍历文件保存
                    var graphInfos = JsonGraphManager.Inst.Path2JsonGraphInfo.Values.ToList();
                    int processCount = 0;
                    int processCountAll = graphInfos.Count;
                    foreach (var graphInfo in graphInfos)
                    {
                        processCount++;
                        if (graphInfo == null || graphInfo.Nodes == null)
                        {
                            continue;
                        }
                        var graphPath = graphInfo.graphPath;
                        var fileName = System.IO.Path.GetFileName(graphPath);
                        if (!fileName.StartsWith(GraphType.Name))
                        {
                            // 不是本编辑器的不处理
                            continue;
                        }
                        if (EditorUtility.DisplayCancelableProgressBar($"{title}[{processCount}/{processCountAll}]", $"开始处理...{fileName}", (float)processCount / processCountAll))
                        {
                            return;
                        }
                        // 测试
                        //if (!graphPath.Contains("SkillGraph_1360000_【测试】效果测试验证"))
                        //{
                        //    continue;
                        //}
                        // 查找是否包含对应节点
                        if (graphInfo.Nodes.Find((node) => node.Config2ID != null && node.Config2ID.StartsWith(configName)) == null)
                        {
                            continue;
                        }
                        // 打开graph
                        GraphHelper.ProcessGraph(graphPath, (graph) =>
                        {
                            // 仅修改当前字段
                            bool isDirty = false;
                            foreach (var node in graph.nodes)
                            {
                                if (node is IConfigBaseNode configNode && configNode.GetConfigName() == configName)
                                {
                                    var config = configNode.GetConfig();
                                    var id = configNode.GetID();
                                    var configExcel = DesignTable.GetTableCell(configName, id);
                                    if (configExcel != null)
                                    {
                                        object memberValue = default;
                                        object memberValueExcel = default;
                                        if (memberEditorProperty != null)
                                        {
                                            memberValue = memberEditorProperty.GetValue(config);
                                            memberValueExcel = cacheExcelData.GetCellData(configName, id.ToString(), memberProperty.Name);
                                        }
                                        else
                                        {
                                            memberValue = memberProperty.GetValue(config);
                                            memberValueExcel = memberProperty.GetValue(configExcel);
                                        }
                                        if (memberValue != memberValueExcel)
                                        {
                                            isDirty = true;
                                            if (memberEditorProperty != null)
                                            {
                                                memberEditorProperty.SetValue(config, memberValueExcel);
                                            }
                                            else
                                            {
                                                memberProperty.SetValue(config, memberValueExcel);
                                            }
                                        }
                                    }
                                }
                            }
                            // 保存，注：更新表格数据已在ConfigBaseNode<T>.Deserialize()处理
                            if (isDirty)
                            {
                                graph.SaveGraphToDisk();
                                Log.Debug($"{title}[{processCount + 1}/{processCountAll}]: {fileName}");
                            }
                        });
                    }
                }
            });
        }


        #endregion
    }
}

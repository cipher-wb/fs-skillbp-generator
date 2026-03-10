#if UNITY_EDITOR

using GameApp.Editor;
using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.NpcEventEditor;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Funny.Base.Utils;
using TableDR;
using UnityEditor;
using UnityEngine;

#pragma warning disable CS0414

namespace NodeEditor
{
    public sealed class NodeEditorTool : Singleton<NodeEditorTool>
    {
        public const string name = "批处理工具";
        private static StringBuilder sbInfo = new StringBuilder();

        #region 模板刷新
        public static void OpenNodeEditorSyncWindow(string templatePath)
        {
            var nodeEditersWindow = Utils.GetWindow<NodeEditorWindow>();

            if (nodeEditersWindow != null)
            {
                nodeEditersWindow.Focus();
            }
            else
            {
                NodeEditorWindow.OpenWindow();
            }

            EditorCoroutineRunner.StartEditorCoroutine(ChangeNodeEditorWindowToTab(templatePath));
        }

        public static IEnumerator ChangeNodeEditorWindowToTab(string templatePath)
        {
            yield return 1;
            var nodeEditersWindow = Utils.GetWindow<NodeEditorWindow>();
            if(nodeEditersWindow != null)
            {
                NodeEditorManager.Inst.SearchPathRef = templatePath;
                nodeEditersWindow.ChangeToTab(NodeEditorManager.name);
                NodeEditorManager.Inst.DoRefreshSearchPathRef();
                NodeEditorManager.Inst.DoSearchPathRef();
            }

        }

        public static void AutoSyncGraphPath(string oldPath, string newPath, bool isCommit)
        {
            string title = "自动同步引用文件名称";

            List<string> readyRefeshPaths = new List<string>();

            var windows = Utils.GetAllWindow<ConfigGraphWindow>();
            List<ITemplateReceiverNode> templates = new List<ITemplateReceiverNode>();

            foreach (var window in windows)
            {
                var nodes = Utils.GetGraphTeNodes<ITemplateReceiverNode>(window.configGraph);
                if (nodes != null && nodes.Count > 0)
                {
                    templates.AddRange(nodes);
                }

                readyRefeshPaths.Add(window.graphPath);
            }


            //刷新所有打开窗口的模板
            foreach (var template in templates)
            {
                string usingNodePath = template.TemplateNodeData.GetTemplatePath();

                if (!string.IsNullOrEmpty(usingNodePath) && usingNodePath == oldPath)
                {
                    template.TemplateNodeData.RefreshGraphRef(newPath);
                }
            }

            //刷新所有未打开的模板
            var graphFiles = GraphHelper.GetGraphFiles();
            //先移除已经刷新的
            foreach (var opnePath in readyRefeshPaths)
            {
                graphFiles.Remove(opnePath);
            }

            readyRefeshPaths.Clear();
            string oldInfo = $"\"TemplatePath\": \"{oldPath}\"";
            string newInfo = $"\"TemplatePath\": \"{newPath}\"";

            int count = graphFiles.Count;
            int i = 0;
            foreach (var path in graphFiles)
            {
                i++;
                EditorUtility.DisplayProgressBar($"{title}[{i}/{count}]", $"开始处理...{path}", (float)i / count);

                var fileContent = File.ReadAllText(path);

                if (fileContent.Contains(oldInfo))
                {
                    fileContent = fileContent.Replace(oldInfo, newInfo);
                    File.WriteAllText(path, fileContent);
                    AssetDatabase.Refresh();
                    readyRefeshPaths.Add(path);
                }
            }

            EditorUtility.ClearProgressBar();

            if (!isCommit)
            {
                return;
            }

            readyRefeshPaths.Add(newPath);
            readyRefeshPaths.Add(oldPath);
            DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.Commit(readyRefeshPaths, true);
        }


        /// <summary>
        /// 自动刷新所有引用模板
        /// </summary>
        /// <param name="templatePath">模板路径</param>
        /// <param name="newPath">重命名使用</param>
        /// <param name="isCommit">是否提交</param>
        /// <param name="changePaths">外部传入的修改路径</param>
        public static void AutoSyncTemlateParams(string templatePath, string newPath = null, bool isCommit = true, List<string> changePaths = null)
        {
            string title = "自动同步引用文件数据";

            Utils.DisplayProcess(title, (sb) =>
            {
                List<string> exportPaths = new List<string>() { templatePath };

                List<ITemplateReceiverNode> templates = new List<ITemplateReceiverNode>();
                List<string> readyRefeshPaths = new List<string>();

                var windows = Utils.GetAllWindow<ConfigGraphWindow>();

                foreach (var window in windows)
                {
                    if (changePaths != null && !changePaths.Contains(window.graphPath))
                    {
                        continue;
                    }
                    else
                    {
                        changePaths.Remove(window.graphPath);
                    }

                    var nodes = Utils.GetGraphTeNodes<ITemplateReceiverNode>(window.configGraph);
                    if (nodes != null && nodes.Count > 0)
                    {
                        templates.AddRange(nodes);
                        exportPaths.Add(window.graphPath);
                    }

                    readyRefeshPaths.Add(window.graphPath);
                }

                //刷新所有打开窗口的模板
                foreach (var template in templates)
                {
                    string usingNodePath = template.TemplateNodeData.GetTemplatePath();

                    if (!string.IsNullOrEmpty(usingNodePath) && usingNodePath == templatePath)
                        template.TemplateNodeData.RefreshGraphRef(templatePath);
                }

                if (changePaths == null)
                {
                    //刷新所有未打开的模板
                    var graphFiles = GraphHelper.GetGraphFiles();
                    //先移除已经刷新的
                    foreach (var opnePath in readyRefeshPaths)
                    {
                        graphFiles.Remove(opnePath);
                    }

                    readyRefeshPaths.Clear();

                    foreach (var path in graphFiles)
                    {
                        var fileContent = File.ReadAllText(path);
                        if (fileContent.Contains($"\"TemplatePath\": \"{templatePath}\""))
                        {
                            readyRefeshPaths.Add(path);
                        }
                    }
                }
                else
                {
                    readyRefeshPaths = changePaths;
                }

                for (int i = 0, count = readyRefeshPaths.Count; i < count; i++)
                {
                    var path = readyRefeshPaths[i];
                    var fileName = System.IO.Path.GetFileName(path);
                    EditorUtility.DisplayProgressBar($"{title}[{i}/{count}]", $"开始处理...{fileName}", (float)i / count);
                    // 打开graph
                    GraphHelper.ProcessGraph(path, (refeshGraph) =>
                    {
                        newPath = string.IsNullOrEmpty(newPath) ? templatePath : newPath;

                        // 整体替换表格数据
                        foreach (var node in refeshGraph.nodes)
                        {
                            if (node is ITemplateReceiverNode templateReceiverNode && 
                            templateReceiverNode.TemplateNodeData.GetTemplatePath() == templatePath)
                            {
                                templateReceiverNode.TemplateNodeData.RefreshGraphRef(newPath);
                                exportPaths.Add(refeshGraph.path);
                            }

                        }
                        // 保存，注：更新表格数据已在ConfigBaseNode<T>.Deserialize()处理
                        refeshGraph.SaveGraphToDisk();
                        Log.Debug($"{title}[{i}/{count}]: {fileName}");
                    });
                }

                EditorUtility.ClearProgressBar();

                if (!isCommit)
                {
                    return;
                }

                if (EditorUtility.DisplayDialog("注意", "自动同步完毕，确认进行SVN提交，取消查看已修改引用条目", "确认", "取消"))
                {
                    if (EditorUtility.DisplayDialog("文件查找", $"查找到文件：{exportPaths.Count}个\n是否提交全部文件?", "提交", "不提交"))
                    {
                        string pathInfo = string.Empty;
                        foreach(var path in exportPaths)
                        {
                            pathInfo = $"{pathInfo}\n{path}";
                        }

                        Log.Info($"即将提交文件{pathInfo}"); 

                        // 效率较低，改成直接解析json数据
                        //GraphHelper.ExportGraph2Excel(paths, false, EditorFlag.DisplayDialog | EditorFlag.DisplayProcess);
                        Utils.DisplayProcess($"提交文件，共{exportPaths.Count}个文件", (sbInfo) =>
                        {
                            sbInfo.AppendLine($"共处理文件: {exportPaths.Count} 个");

                            DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.Commit(exportPaths, true);
                            //ExternalExportUtil.ExportNodeJson2Excel(exportPaths);
                        });
                    }
                }
                else
                {
                    OpenNodeEditorSyncWindow(templatePath);
                }
            });
        } 


        #endregion

        #region 节点查找
        [InfoBox("查找所有编辑器json文件包含信息\n注：只检测文本是否包含", infoMessageType: InfoMessageType.Warning)]
        [TitleGroup("节点查找", indent: true), LabelText("查找内容，如：TSET_ORDER_EXECUTE"), PropertySpace(5, 10)]
        [InlineButton("BatchProcessing_SearchNode_DoProcess", "处理查找结果")]
        [InlineButton("BatchProcessing_SearchNode", "点击查找")]
        public string NodeDescription;

        [TitleGroup("节点查找"), Sirenix.OdinInspector.ShowInInspector, HideLabel, HideReferenceObjectPicker]
        private GraphDataSearchedList searchedList = new GraphDataSearchedList();
        private void BatchProcessing_SearchNode()
        {
            if (string.IsNullOrEmpty(NodeDescription))
            {
                return;
            }
            Utils.DisplayProcess("批量处理_节点类型查找", (_) =>
            {
                searchedList.Clear();
                var sbFiles = new StringBuilder();
                GraphHelper.ProcessEditor((manager) =>
                {
                    var graphFiles = Directory.GetFiles(manager.PathSavesJsons, $"*.json", SearchOption.AllDirectories);
                    for (int i = 0, length = graphFiles.Length; i < length; i++)
                    {
                        var graphFile = graphFiles[i];
                        var graphName = Path.GetFileName(graphFile);
                        var content = File.ReadAllText(graphFile);
                        if (EditorUtility.DisplayCancelableProgressBar($"批量处理_节点类型查找: {manager.Setting.ExcelName}", $"开始处理...{graphName}:{i}/{length}", i / (float)length))
                        {
                            return;
                        }
                        if (content.Contains(NodeDescription))
                        {
                            sbInfo.AppendLine($"{graphName}");
                            sbFiles.AppendLine(graphFile);
                            searchedList.Add(graphFile);
                        }
                        // 太慢
                        //var graph = SkillGraph.LoadGraph(graphFile);
                        //foreach (var node in graph.nodes)
                        //{
                        //    if (node.GetCustomName().Contains(NodeDescription))
                        //    {
                        //        sbInfo.AppendLine($"{graph.name}");
                        //    }
                        //}
                    }
                    if (sbInfo.Length == 0)
                    {
                        sbInfo.AppendLine($"找不到包含:【{NodeDescription}】的graph");
                    }
                    else
                    {
                        Log.Info(sbInfo.ToString());
                    }
                    File.WriteAllText(Constants.SkillEditor.PathSearchResult, sbFiles.ToString());
                });
            });
        }
        private void BatchProcessing_SearchNode_DoProcess()
        {
            // 以下逻辑自己处理
            return;
            Utils.DisplayProcess("批处理_处理查找结果", (_) =>
            {
                var graphFiles = File.ReadAllLines(Constants.SkillEditor.PathSearchResult);
                List<object> configs = new List<object>();
                for (int i = 0, length = graphFiles.Length; i < length; i++)
                {
                    var graphFile = graphFiles[i];
                    var fileName = Path.GetFileName(graphFile);
                    #region 处理查找graph
                    GraphHelper.ProcessGraph(graphFile, (graph) =>
                    {
                        if (EditorUtility.DisplayCancelableProgressBar("批处理_处理查找结果", $"处理数据...{graph.name}:{i}/{length}", i / (float)length))
                        {
                            return;
                        }
                        for (int j = graph.nodes.Count - 1; j >= 0; --j)
                        {
                            var node = graph.nodes[j];
                            try
                            {
                                if (node is RefConfigBaseNode configNode)
                                {
                                    //var configName = configNode.GetConfigName();
                                    //if (configName == nameof(SkillConditionConfig))
                                    //{
                                    //    // 删除节点，断开连线（为了断开连线后数据清空），移除节点
                                    //    var edges = node.GetAllEdges().ToList();
                                    //    foreach (var edge in edges)
                                    //    {
                                    //        graph.Disconnect(edge.GUID);
                                    //        if (edge.outputNode is SkillSelectConfigNode skillSelectConfigNode)
                                    //        {
                                    //            skillSelectConfigNode.Config.ExSetValue(nameof(skillSelectConfigNode.Config.SpecialSkillSelectFlag), TSpecialSkillSelectFlag.TSSSF_Common_DiJun);
                                    //        }
                                    //    }
                                    //    graph.RemoveNode(node);
                                    //}
                                    //configs.Add(configNode.GetConfig());
                                    if (configNode.Config == null)
                                    {
                                        Log.Error($"空引用节点: {configNode.ID}, {configNode.ManualID},{fileName}");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error($"处理错误:{graphFile}\n{ex}");
                            }
                        }
                        // 保存修改数据
                        graph.SaveGraphToDisk();
                    });
                    // 测试只处理一个
                    //return;
                    #endregion
                }
                //EditorUtility.ClearProgressBar();
                //// 导出修改数据
                //ExcelManager.Inst.WriteExcel(SkillEditorManager.Inst.Setting.PathExportExcel, configs);
                //ExcelManager.Inst.FormatExcelData(Constants.SkillEditor.PathExcel);
            });
        }
        #endregion

        #region 节点数据替换
        [TitleGroup("节点替换", indent: true), LabelText("查找内容："), PropertySpace(5, 5)]
        public string ReplaceFrom;

        [TitleGroup("节点替换", indent: true), LabelText("替换内容:"), PropertySpace(5, 5)]
        public string ReplaceTo;

        [InfoBox("注意：本操作会批处理替换所有Graph的json文件内容", infoMessageType: InfoMessageType.Warning)]
        [TitleGroup("节点替换", indent: true), Button("点击替换", ButtonSizes.Medium), PropertySpace(5, 10)]
        private void BatchProcessing_NodeFileReplace()
        {
            if (string.IsNullOrEmpty(ReplaceFrom))
            {
                return;
            }
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            Utils.DisplayProcess("批量处理_节点类型查找", (_) =>
            {
                var sbFiles = new StringBuilder();
                GraphHelper.ProcessEditor((manager) =>
                {
                    var graphFiles = Directory.GetFiles(manager.PathSavesJsons, $"*.json", SearchOption.AllDirectories);
                    for (int i = 0, length = graphFiles.Length; i < length; i++)
                    {
                        var graphFile = graphFiles[i];
                        var graphName = Path.GetFileName(graphFile);
                        if (EditorUtility.DisplayCancelableProgressBar("批量处理_节点类型查找", $"开始处理...{graphName}:{i}/{length}", i / (float)length))
                        {
                            return;
                        }
                        var content = File.ReadAllText(graphFile);
                        if (content.Contains(ReplaceFrom))
                        {
                            content = content.Replace(ReplaceFrom, ReplaceTo);
                            File.WriteAllText(graphFile, content);
                            sbInfo.AppendLine(graphFile);
                        }
                    }
                });
                if (sbInfo.Length == 0)
                {
                    sbInfo.AppendLine($"找不到包含:【{ReplaceFrom}】的graph");
                }
            });
        }
        #endregion

        #region 批处理SkillEffectConfig
        [InfoBox("批处理效果表数据：\n1-新增默认参数\n2-移除多余参数配置\n3-修改后同步数据修改到Excel")]
        [TitleGroup("批处理SkillEffectConfig", indent: true), LabelText("效果类型选择"), PropertySpace(5)]
        public TSkillEffectType SkillEffectType;

        [TitleGroup("批处理SkillEffectConfig", indent: true), Button("点击处理", ButtonSizes.Medium), PropertySpace(5)]
        private void BatchProcessing_CheckSkillEffectConfig()
        {
            if (SkillEffectType == TSkillEffectType.TSET_NULL) return;

            if (!LocalSettings.IsProgramer())
            {
                EditorUtility.DisplayDialog("批处理SkillEffectConfig", "仅限程序操作", "好的");
                return;
            }

            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            // TODO 后续可扩展表格数据校验，错误数据修改后直接同步到Excel，避免手动Excel查找错误数据，然后手动修改
            try
            {
                Utils.DisplayProcess("批处理_表格错误数据处理", (sb) =>
                {
                    // 确保表格加载
                    DesignTable.Reload();

                    // 收集需要修改的表数据，作为数据同步修改到Excel
                    var modifyID2Config = new Dictionary<int, object>();
                    float stepAll = 2;
                    float step = 0;

                    // 查找Excel相关数据
                    {
                        EditorUtility.DisplayProgressBar("批处理SkillEffectConfig", $"数据整理...{nameof(SkillEffectConfig)}", step / stepAll);
                        ++step;
                        for (int i = 0, length = SkillEffectConfigManager.Instance.ItemArray.Items.Count; i < length; i++)
                        {
                            var config = SkillEffectConfigManager.Instance.ItemArray.Items[i];
                            EditorUtility.DisplayProgressBar("批处理SkillEffectConfig", $"数据整理...{nameof(SkillEffectConfig)}:{config.ID}", i / (float)length * (step / stepAll));
                            if (TryModifyParams(config))
                            {
                                modifyID2Config[config.ID] = config;
                            }
                        }
                    }
                    // 查找编辑器Json数据，以编辑器数据为准
                    {
                        HashSet<string> modifyPaths = new HashSet<string>();
                        if (JsonGraphManager.Inst.Config2Nodes.TryGetValue(nameof(SkillEffectConfig), out var id2Nodes))
                        {
                            foreach (var item in id2Nodes)
                            {
                                var nodes = item.Value;
                                foreach (var node in nodes)
                                {
                                    if (node.SkillEffectType == (int)SkillEffectType)
                                    {
                                        if (node.Config is SkillEffectConfig config && TryModifyParams(config))
                                        {
                                            modifyID2Config[node.ID] = config;
                                            // 修改Json文件
                                            var path = node.Owner.graphPath;
                                            modifyPaths.Add(path);
                                            Log.Error($"[{config.GetType().Name}:{config.ID}:{SkillEffectType.GetDescription(false)}] 文件:{path}");
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var path in modifyPaths)
                        {
                            GraphHelper.ProcessGraph(path, (graph) =>
                            {
                                foreach(var node in graph.nodes)
                                {
                                    if (node is SkillEffectConfigNode configNode && modifyID2Config.TryGetValue(configNode.ID, out var moNode))
                                    {
                                        var moConfig = moNode as SkillEffectConfig;
                                        if (configNode.Config is SkillEffectConfig config)
                                        {
                                            var paramsList = config.Params;
                                            for(var i = 0; i < paramsList.Count; ++i)
                                            {
                                                (paramsList?.GetListRef())[i] = moConfig.Params[i];
                                            }
                                        }
                                    }
                                    //
                                }

                                graph.SaveGraphToDisk();
                                Log.Error($"刷新文件:{path}");
                            });
                        }
                    }
                    if (modifyID2Config.Count == 0)
                    {
                        EditorUtility.DisplayDialog("批处理SkillEffectConfig", $"无数据同步修改", "好的");
                        return;
                    }

                    // 需要修改的表格列表，空则全部处理
                    var excelFilter = SkillEffectConfigManager.Excels.ToList();
                    var errorInfo = string.Empty;
                    ExcelManager.Inst.ProcessExcels(
                        (excelFile, i, length) =>
                        {
                            var fileName = Path.GetFileName(excelFile);
                            EditorUtility.DisplayProgressBar("批处理SkillEffectConfig", $"修改表格数据...{fileName}", i / (float)length);
                            ExcelManager.Inst.ModifyExcelCell(excelFile, modifyID2Config.Values.ToList());
                        },
                        (excelFile) =>
                        {
                            var fileName = Path.GetFileName(excelFile);
                            if (excelFilter.Count > 0 && !excelFilter.Contains(fileName))
                            {
                                return false;
                            }
                            if (Utils.IsFileOpened(excelFile))
                            {
                                errorInfo += $"{fileName}\n";
                                return false;
                            }
                            return true;
                        });
                    if (!string.IsNullOrEmpty(errorInfo))
                    {
                        EditorUtility.DisplayDialog("批处理SkillEffectConfig", $"同步数据异常，表格处于不可读写状态\n需要svn lock以下excel文件:\n{errorInfo}", "好的");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("批处理SkillEffectConfig", $"同步数据成功，修改详细信息见Console-Error界面", "好的");
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        /// <summary>
        /// TODO SkillConditionConfig，SkillSelectConfig等
        /// </summary>
        private bool TryModifyParams(object config)
        {
            if (config is SkillEffectConfig skillEffectConfig && skillEffectConfig.SkillEffectType == SkillEffectType)
            {
                var id = skillEffectConfig.ID;
                var effectType = SkillEffectType;
                var paramsList = skillEffectConfig.Params;
                // 处理效果节点参数列表新增，默认值设置
                var anno = TableAnnotation.Inst.GetParamsAnnotation(effectType);
                if (anno != null)
                {
                    if (anno.IsArray)
                    {
                        // 检测数据是否是SkillEffectConfig.ID
                    }
                    else
                    {
                        var curCount = paramsList.Count;
                        var newCount = anno.paramsAnn.Count;
                        if (paramsList.Count < anno.paramsAnn.Count)
                        {
                            // 参数个数扩大，那么自动“向后”扩充下参数个数
                            // 特殊需求自行扩展展开
                            for (int j = curCount; j < newCount; j++)
                            {
                                // 按照预设默认值创建
                                var paramAnn = anno.paramsAnn[j];
                                var tParam = paramAnn.CopyDefaultParam();
                                paramsList?.GetListRef().Add(tParam);
                            }
                            // 给个提示信息
                            Log.Debug($"[{config.GetType().Name}:{id}:{effectType.GetDescription(false)}] 参数个数增多，原个数:{curCount}, 新个数:{newCount}");
                            return true;
                        }
                        else if (paramsList.Count > anno.paramsAnn.Count)
                        {
                            // 参数个数减少
                            Log.Debug($"[{config.GetType().Name}:{id}:{effectType.GetDescription(false)}] 参数个数减少，原个数:{curCount}, 新个数:{newCount}");
                            for (int j = paramsList.Count - 1; j >= anno.paramsAnn.Count; --j)
                            {
                                paramsList?.GetListRef().RemoveAt(j);
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        [TitleGroup("批处理工具", indent: true), ResponsiveButtonGroup("批处理工具/1", DefaultButtonSize = ButtonSizes.Large, UniformLayout = true), Button("表格错误数据处理", ButtonSizes.Large), PropertySpace(5)]
        private void BatchProcessing_CheckConfig()
        {
            // TODO 后续可扩展表格数据校验，错误数据修改后直接同步到Excel，避免手动Excel查找错误数据，然后手动修改
            try
            {
                if (!LocalSettings.IsProgramer())
                {
                    EditorUtility.DisplayDialog("批处理_表格错误数据处理", "仅限程序操作", "好的");
                    return;
                }
                bool isValid = false;
                if (!isValid)
                {
                    EditorUtility.DisplayDialog("注意", "注意：执行逻辑请自行修改代码，开启功能", "好的");
                    return;
                }
                if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
                {
                    return;
                }
                // 确保表格加载
                DesignTable.Reload();

                // 收集需要修改的表数据，作为数据同步修改到Excel
                var modifyConfigs = new List<object>();
                float stepAll = 2;
                float step = 0;

                // 检查SkillEffectConfig参数个数
                // 目前只处理参数个数增多，默认填充配置的默认值，其他处理可以自定义修改参数列表数据
                {
                    EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(SkillEffectConfig)}", step / stepAll);
                    ++step;
                    for (int i = 0, length = SkillEffectConfigManager.Instance.ItemArray.Items.Count; i < length; i++)
                    {
                        var config = SkillEffectConfigManager.Instance.ItemArray.Items[i];
                        var paramsList = config.Params;
                        var effectType = config.SkillEffectType;

                        //EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(SkillEffectConfig)}:{config.ID}", i / (float)length * (step / stepAll));

                        #region 屏蔽-批处理效果节点参数列表新增，默认值设置
                        //var anno = TableAnnotation.Inst.GetParamsAnnotation(effectType);
                        //if (anno != null)
                        //{
                        //    if (anno.IsArray)
                        //    {
                        //        // 检测数据是否是SkillEffectConfig.ID
                        //    }
                        //    else
                        //    {
                        //        var curCount = paramsList.Count;
                        //        var newCount = anno.paramsAnn.Count;
                        //        if (paramsList.Count < anno.paramsAnn.Count)
                        //        {
                        //            // 参数个数扩大，那么自动“向后”扩充下参数个数
                        //            // 特殊需求自行扩展展开
                        //            for (int j = curCount; j < newCount; j++)
                        //            {
                        //                // 按照预设默认值创建
                        //                var paramAnn = anno.paramsAnn[j];
                        //                var tParam = paramAnn.CopyDefaultParam();
                        //                paramsList.Add(tParam);
                        //            }
                        //            modifyConfigs.Add(config);
                        //            // 给个提示信息
                        //            Log.Error($"[{config.GetType().Name}:{config.ID}:{EnumUtility.GetDescription(effectType)}] 参数个数增多，原个数:{curCount}, 新个数:{newCount}");
                        //        }
                        //        else if (paramsList.Count > anno.paramsAnn.Count)
                        //        {
                        //            // 参数个数减少，暂时不处理，给个报错
                        //            Log.Error($"[{config.GetType().Name}:{config.ID}:{EnumUtility.GetDescription(effectType)}] 参数个数减少，原个数:{curCount}, 新个数:{newCount}");
                        //            //// 删除操作仅限程序，避免错误删除
                        //            //for (int j = paramsList.Count - 1; j >= anno.paramsAnn.Count; --j)
                        //            //{
                        //            //    paramsList.RemoveAt(j);
                        //            //}
                        //            //modifyConfigs.Add(config);
                        //        }
                        //    }
                        //}
                        #endregion

                        #region 屏蔽-批处理创建特效角度修正
                        //if (effectType == TSkillEffectType.TSET_CREATE_EFFECT)
                        //{
                        //    if (config == null)
                        //    {
                        //        Log.Error($"ID :{config.ID} config is null");
                        //        continue;
                        //    }
                        //    if (config.Params == null)
                        //    {
                        //        Log.Error($"ID :{config.ID} Params is null");
                        //        continue;
                        //    }
                        //    var angleParam = config.Params.ExGet(1);
                        //    if (angleParam != null && angleParam.ParamType == TParamType.TPT_NULL)
                        //    {
                        //        var valueNew = -(angleParam.Value - 90);
                        //        angleParam.ExSetValue(nameof(angleParam.Value), valueNew);
                        //        modifyConfigs.Add(config);
                        //    }
                        //}
                        #endregion

                        #region 屏蔽-检测屏幕效果濒死效果
                        //Utils.SafeCall(() =>
                        //{
                        //    if (effectType == TSkillEffectType.TSET_APPLY_SCREEN_EFFECT && config.Params[2].Value == 1)
                        //    {
                        //        Log.Error($"config:{config.ID}");
                        //    }
                        //});
                        #endregion

                        #region 屏蔽-检测属性模型缩放
                        //Utils.SafeCall(() =>
                        //{
                        //    if ((effectType == TSkillEffectType.TSET_MODIFY_ENTITY_ATTR_VALUE ||
                        //        effectType == TSkillEffectType.TSET_ADD_ENTITY_ATTR_VALUE) &&
                        //        (config.Params[1].Value == (int)TBattleNatureEnum.TBN_MODEL_SCALE || config.Params[1].Value == (int)TBattleNatureEnum.TBN_MODEL_BASE_SCALE_PERCENT))
                        //    {
                        //        Log.Error($"config:{config.ID}, {EnumUtility.GetDescription(effectType)}:{config.Params[1].Value}");
                        //    }
                        //});
                        #endregion

                        #region 屏蔽-检测播放角色动画
                        //Utils.SafeCall(() =>
                        //{
                        //    if (effectType == TSkillEffectType.TSET_PLAY_ROLE_ANIM)
                        //    {
                        //        var anno = TableAnnotation.Inst.GetParamsAnnotation(effectType);
                        //        if (anno != null)
                        //        {
                        //            var curCount = paramsList.Count;
                        //            var newCount = anno.paramsAnn.Count;
                        //            if (paramsList.Count < anno.paramsAnn.Count)
                        //            {
                        //                // 参数个数扩大，那么自动“向后”扩充下参数个数
                        //                // 特殊需求自行扩展展开
                        //                for (int j = curCount; j < newCount; j++)
                        //                {
                        //                    // 按照预设默认值创建
                        //                    var paramAnn = anno.paramsAnn[j];
                        //                    var tParam = paramAnn.GetDefaultParam();
                        //                    paramsList.Add(tParam);
                        //                }
                        //                modifyConfigs.Add(config);
                        //                // 给个提示信息
                        //                Log.Error($"[{config.GetType().Name}:{config.ID}:{EnumUtility.GetDescription(effectType)}] 参数个数增多，原个数:{curCount}, 新个数:{newCount}");
                        //            }
                        //            else if (paramsList.Count > anno.paramsAnn.Count)
                        //            {
                        //                // 参数个数减少
                        //                Log.Error($"[{config.GetType().Name}:{config.ID}:{EnumUtility.GetDescription(effectType)}] 参数个数减少，原个数:{curCount}, 新个数:{newCount}");
                        //                // 删除操作仅限程序，避免错误删除
                        //                for (int j = paramsList.Count - 1; j >= anno.paramsAnn.Count; --j)
                        //                {
                        //                    paramsList.RemoveAt(j);
                        //                }
                        //                modifyConfigs.Add(config);
                        //            }
                        //            var paramSpeed = paramsList[3];
                        //            //if (paramSpeed.Value == 0)
                        //            {
                        //                paramSpeed.ExSetValue(nameof(paramSpeed.Value), 200);
                        //                modifyConfigs.Add(config);
                        //            }
                        //        }
                        //    }
                        //});
                        #endregion
                    }
                }
                // TODO 其他表格，如SkillSelectConfig
                {
                    //++step;
                }
                // 屏蔽-检测BulletConfig
                //{
                //    EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(BulletConfig)}", step / stepAll);
                //    ++step;
                //    for (int i = 0, length = BulletConfigManager.Instance.ItemArray.Items.Count; i < length; i++)
                //    {
                //        var config = BulletConfigManager.Instance.ItemArray.Items[i];
                //        EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(BulletConfig)}:{config.ID}", i / (float)length * (step / stepAll));
                //        //if (config.TracePathParams?.Count > 0)
                //        //{
                //        //    Log.Error($"config:{config.ID}, {config.TracePathParams.Count}, {EnumUtility.GetDescription(config.TracePathType)}");
                //        //}

                //        #region 批处理子弹生命控制数据
                //        Utils.SafeCall(() =>
                //        {
                //            config.ExSetValue(nameof(config.LifeFlag), BulletConfig_TBulletLifeFlag.TBLT_NULL);
                //            modifyConfigs.Add(config);
                //        });
                //        #endregion
                //    }
                //}

                bool isExpor2Excel = false;
                if (!isExpor2Excel)
                {
                    return;
                }
                if (modifyConfigs.Count == 0) return;

                // 需要修改的表格列表，空则全部处理
                var excelFilter = new List<string>
                {
                    "1GamePlayEditor",
                    "1AIEditor",
                    "1SkillEditor",
                    "SkillConfig",
                };

                ExcelManager.Inst.ProcessExcels(
                    (excelFile, i, length) =>
                    {
                        var fileName = Path.GetFileName(excelFile);
                        EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"修改表格数据...{fileName}", i / (float)length);
                        ExcelManager.Inst.ModifyExcelCell(excelFile, modifyConfigs);
                    },
                    (excelFile) =>
                    {
                        var fileName = Path.GetFileNameWithoutExtension(excelFile);
                        if (excelFilter.Count > 0 && !excelFilter.Contains(fileName))
                        {
                            return false;
                        }
                        if (Utils.IsFileOpened(excelFile))
                        {
                            Log.Warning($"表格处于不可读写状态：{fileName}");
                            return false;
                        }
                        return true;
                    });
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        [TitleGroup("批处理工具", indent: true), ResponsiveButtonGroup("批处理工具/1", DefaultButtonSize = ButtonSizes.Large, UniformLayout = true), Button("代码批量处理数据到Json", ButtonSizes.Large), PropertySpace(5)]
        private void BatchProcessing_CheckConfigToJson()
        {
            try
            {
                if (!LocalSettings.IsProgramer())
                {
                    EditorUtility.DisplayDialog("代码批量处理数据到Json", "仅限程序操作", "好的");
                    return;
                }
                bool isValid = true;
                if (!isValid)
                {
                    EditorUtility.DisplayDialog("注意", "注意：执行逻辑请自行修改代码，开启功能", "好的");
                    return;
                }
                if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
                {
                    return;
                }
                // 确保表格加载
                DesignTable.Reload();

                // 收集需要修改的表数据，作为数据同步修改到Excel
                var modifyConfigs = new List<object>();
                float stepAll = 2;
                float step = 0;
                bool isReturn = false;

                // 检查威力系数获取节点，批量替换获取最终值
                bool checkSkillPamraNode = true;
                if (checkSkillPamraNode)
                {
                    EditorUtility.DisplayProgressBar("代码批量处理数据到Json", $"数据整理...{nameof(SkillSelectConfig)}", step / stepAll);
                    ++step;
                    // 收集数据
                    bool checkByConfig = false;
                    if (checkByConfig)
                    {
                        for (int i = 0, length = SkillEffectConfigManager.Instance.ItemArray.Items.Count; i < length; i++)
                        {
                            var config = SkillEffectConfigManager.Instance.ItemArray.Items[i];
                            var paramsList = config.Params;
                            var skillEffectType = config.SkillEffectType;

                            if (EditorUtility.DisplayCancelableProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(SkillSelectConfig)}:{config.ID}", i / (float)length * (step / stepAll)))
                            {
                                break;
                            }

                            #region 屏蔽-处理数据
                            //Utils.SafeCall(() =>
                            //{
                            //    switch (skillEffectType)
                            //    {
                            //        case TSkillEffectType.TSET_GET_SKILL_TAG_VALUE:
                            //            {
                            //                if (paramsList == null)
                            //                {
                            //                    Log.Error($"参数对象为空, ID:{config.ID}");
                            //                    return;
                            //                }
                            //                // 获取技能参数数据
                            //                if (paramsList.Count != 5)
                            //                {
                            //                    Log.Error($"参数数量错误, ID:{config.ID}，当前数量:{paramsList.Count}");
                            //                    return;
                            //                }
                            //                int skillTagConfigID = paramsList[2].Value;
                            //                if (skillTagConfigID == 3 ||
                            //                    skillTagConfigID == 5 ||
                            //                    skillTagConfigID == 1201 ||
                            //                    skillTagConfigID == 1211 ||
                            //                    skillTagConfigID == 55 ||
                            //                    skillTagConfigID == 56)
                            //                {
                            //                    // 替换为获取最终值
                            //                    if (paramsList[4].Value == 0)
                            //                    {
                            //                        paramsList[4].SetValue(1);
                            //                        //modifyConfigs.Add(config);
                            //                        Log.Error($"参数修改, ID:{config.ID}");
                            //                    }
                            //                }
                            //                break;
                            //            }
                            //    }
                            //    //isReturn = true;
                            //});
                            #endregion

                            if (isReturn)
                            {
                                return;
                            }
                        }
                    }

                    // 从json数据中搜集
                    bool checkByJson = true;
                    if (checkByJson)
                    {
                        ++step;
                        #region 处理音效节点
                        if (JsonGraphManager.Inst.TryGetGraphInfos(nameof(SkillEffectConfig), nameof(TableDR.TSkillEffectType.TSET_PLAY_SOUND), out var graphs))
                        {
                            // 找到所有音效节点
                            Utils.SafeCall(() =>
                            {
                                for (int i = 0, length = graphs.Count; i < length; i++)
                                {
                                    var graph = graphs[i];
                                    var graphPath = graph.graphPath;
                                    if (EditorUtility.DisplayCancelableProgressBar("批处理_表格错误数据处理", $"数据整理...{graph.graphName}", i / (float)length * (step / stepAll)))
                                    {
                                        break;
                                    }
                                    // 打开json文件修改数据
                                    GraphHelper.ProcessGraph(graphPath, (graph) =>
                                    {
                                        bool isNeedSave = false;
                                        foreach (var node in graph.nodes)
                                        {
                                            if (node is TSET_PLAY_SOUND myNode)
                                            {
                                                var config = myNode.Config;
                                                var paramsList = config.Params;
                                                if (paramsList == null)
                                                {
                                                    Log.Error($"参数对象为空, ID:{config.ID}");
                                                    return;
                                                }
                                                // 获取技能参数数据
                                                if (paramsList.Count < 8)
                                                {
                                                    Log.Error($"参数数量错误, ID:{config.ID}，当前数量:{paramsList.Count}");
                                                    return;
                                                }
                                                // 检查单位配置
                                                var paramEntity = paramsList[5];
                                                if (paramEntity.Value == 0 && paramEntity.ParamType == TableDR.TParamType.TPT_NULL)
                                                {
                                                    paramEntity.ExSetValue(nameof(paramEntity.Value), 1);
                                                    paramEntity.ExSetValue(nameof(paramEntity.ParamType), TableDR.TParamType.TPT_COMMON_PARAM);
                                                    modifyConfigs.Add(config);
                                                    isNeedSave = true;
                                                }
                                            }
                                        }
                                        if (isNeedSave)
                                        {
                                            graph.SaveGraphToDisk();
                                        }
                                    });
                                }
                            });
                        }
                    }
                    #endregion
                }

                // 检查隐藏节点
                bool checkHideNode = false;
                if (checkHideNode)
                {
                    EditorUtility.DisplayProgressBar("代码批量处理数据到Json", $"数据整理...{nameof(SkillSelectConfig)}", step / stepAll);
                    ++step;
                    // 收集所有技能连出去的筛选，去除隐藏状态
                    var skillSelectConfigs = new Dictionary<int, SkillSelectConfig>();
                    var changeConfigs = new Dictionary<int, SkillSelectConfig>();
                    foreach (var skillTable in SkillConfigManager.Instance.GetItems())
                    {
                        var skillConfig = skillTable as SkillConfig;
                        if (skillConfig != null && skillConfig.SkillEffectExecuteInfo != null)
                        {
                            var selectConfigID = skillConfig.SkillEffectExecuteInfo.SelectConfigID;
                            if (selectConfigID != 0 && !skillSelectConfigs.ContainsKey(selectConfigID))
                            {
                                var selectConfig = SkillSelectConfigManager.Instance.GetItem(selectConfigID);
                                if (selectConfig != null && selectConfig.SpecialSkillSelectFlag != 0)
                                {
                                    skillSelectConfigs.Add(selectConfigID, selectConfig);
                                }
                            }
                        }
                    }
                    for (int i = 0, length = SkillSelectConfigManager.Instance.ItemArray.Items.Count; i < length; i++)
                    {
                        var config = SkillSelectConfigManager.Instance.ItemArray.Items[i];
                        var paramsList = config.Params;
                        var selectType = config.SkillSelectType;

                        EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"数据整理...{nameof(SkillSelectConfig)}:{config.ID}", i / (float)length * (step / stepAll));

                        #region 检查状态-隐藏中
                        Utils.SafeCall(() =>
                        {
                            // 不存在隐身中的，加上
                            var flagOld = config.SpecialSkillSelectFlag;
                            var flag = flagOld;
                            if (flag != 0)
                            {
                                if (skillSelectConfigs.ContainsKey(config.ID))
                                {
                                    flag &= ~(TSpecialSkillSelectFlag.TSSSF_InState_Hide);
                                }
                                // 配了敌军，没配隐身的，加上
                                else if (!flag.HasFlag(TSpecialSkillSelectFlag.TSSSF_InState_Hide))
                                {
                                    flag |= TSpecialSkillSelectFlag.TSSSF_InState_Hide;
                                }
                            }
                            if (flagOld != flag)
                            {
                                config.ExSetValue(nameof(config.SpecialSkillSelectFlag), flag);
                                changeConfigs.Add(config.ID, config);
                                // 查找json，替换数据
                                if (JsonGraphManager.Inst.TryGetNodeInfo(nameof(SkillSelectConfig), config.ID, out var nodeInfo) && nodeInfo.Owner != null)
                                {
                                    var configJsonOld = nodeInfo.ConfigJson;
                                    var configJsonNew = JsonConvert.SerializeObject(config, Formatting.None, new JsonSerializerSettings
                                    {
                                        DefaultValueHandling = DefaultValueHandling.Include/*IgnoreAndPopulate*/,
                                        NullValueHandling = NullValueHandling.Ignore,
                                    });
                                    // 记录文件需要转义下
                                    configJsonOld = configJsonOld.Replace("\"", "\\\"");
                                    configJsonNew = configJsonNew.Replace("\"", "\\\"");
                                    if (Utils.ReplaceAllText(nodeInfo.Owner.graphPath, configJsonOld, configJsonNew))
                                    {
                                        Log.Info($"替换筛选数据-成功，ID：{config.ID}, {nodeInfo.Owner.graphName}");
                                    }
                                    else
                                    {
                                        Log.Error($"替换筛选数据-失败，ID：{config.ID}, {nodeInfo.Owner.graphName}");
                                    }
                                }
                                else
                                {
                                    Log.Error($"替换筛选数据失败，找不到对应的文件，需要删除数据，ID：{config.ID}");
                                    modifyConfigs.Add(config);
                                }
                                //isReturn = true;
                            }
                        });
                        #endregion
                        if (isReturn)
                        {
                            return;
                        }
                    }
                }
                // TODO 其他表格
                {
                    //++step;
                }

                bool isExpor2Excel = false;
                if (!isExpor2Excel)
                {
                    return;
                }
                if (modifyConfigs.Count == 0) return;

                // 需要修改的表格列表，空则全部处理
                var excelFilter = new List<string>
                {
                    "1GamePlayEditor",
                    "1AIEditor",
                    "1SkillEditor",
                };

                // 批处理数据导出到excel表
                ExcelManager.Inst.ProcessExcels(
                    (excelFile, i, length) =>
                    {
                        var fileName = Path.GetFileName(excelFile);
                        EditorUtility.DisplayProgressBar("批处理_表格错误数据处理", $"修改表格数据...{fileName}", i / (float)length);
                        ExcelManager.Inst.ModifyExcelCell(excelFile, modifyConfigs);
                    },
                    (excelFile) =>
                    {
                        var fileName = Path.GetFileNameWithoutExtension(excelFile);
                        if (excelFilter.Count > 0 && !excelFilter.Contains(fileName))
                        {
                            return false;
                        }
                        if (Utils.IsFileOpened(excelFile))
                        {
                            Log.Warning($"表格处于不可读写状态：{fileName}");
                            return false;
                        }
                        return true;
                    });
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        [TitleGroup("批处理工具", indent: true), ResponsiveButtonGroup("批处理工具/1"), Button("刷新已有ID记录", ButtonSizes.Large), PropertySpace(5)]
        public void BatchProcessing_LoadConfigID()
        {
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            Utils.DisplayProcess("刷新已有ID记录", (_) => { ConfigIDManager.Inst.LoadAllConfigID(EditorFlag.DisplayProcess); });
        }

        //[TitleGroup("批处理工具", indent: true), ButtonGroup("批处理工具/1"), Button("遍历检查节点", ButtonSizes.Large), PropertySpace(5)]
        public void BatchProcessing_CheckNodes()
        {
            if (!LocalSettings.IsProgramer())
            {
                EditorUtility.DisplayDialog("批处理_遍历检查节点", "仅限程序操作", "好的");
                return;
            }
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            #region 查找技能参数节点
            Utils.DisplayProcess("查找技能参数节点", (_) =>
            {
                GraphHelper.ProcessAllGraphs((manager, graph, i, length) =>
                {
                    int nodeCount = graph.nodes.Count;
                    if (EditorUtility.DisplayCancelableProgressBar("查找技能参数节点", $"开始处理...{graph.name}:{i}/{length}", i / (float)length))
                    {
                        return;
                    }
                    for (int j = nodeCount - 1; j >= 0; --j)
                    {
                        var node = graph.nodes[j];
                        if (node is IConfigBaseNode configNode && configNode.GetConfig() is SkillEffectConfig skillEffectConfig)
                        {
                            switch (skillEffectConfig.SkillEffectType)
                            {
                                case TSkillEffectType.TSET_ADD_SKILL_TAG_VALUE:
                                case TSkillEffectType.TSET_GET_SKILL_TAG_VALUE:
                                case TSkillEffectType.TSET_MODIFY_SKILL_TAG_VALUE:
                                    foreach (var edge in node.GetOutputEdges())
                                    {
                                        if (edge.outputPortIdentifier == "2")
                                        {
                                            Log.Error($"查找技能参数节点:{graph.name}");
                                            return;
                                        }
                                    }
                                    return;
                            }
                        }
                    }
                });
            });
            #endregion

            #region 屏蔽-批量替换（主体单位-目标单位）
            //DisplayProcess("批量替换（主体单位-目标单位）", () =>
            //{
            //    int deleteCount = 0;
            //    var deleteIDs = new HashSet<int>();
            //    var sbIDs = new StringBuilder();
            //    GraphHelper.LoadGraphs<SkillGraph>((graph, i, length) =>
            //    {
            //        // 测试
            //        //if (!graphFile.Contains("136_"))
            //        //{
            //        //    continue;
            //        //}
            //        int nodeCount = graph.nodes.Count;
            //        bool isDirty = false;
            //        EditorUtility.DisplayProgressBar("批量替换（主体单位-目标单位）", $"开始处理...{graph.name}:{i}/{length}", i / (float)length);
            //        for (int j = nodeCount - 1; j >= 0; --j)
            //        {
            //            var node = graph.nodes[j];
            //            if (node is IConfigBaseNode configNode && configNode.GetConfig() is SkillEffectConfig skillEffectConfig)
            //            {
            //                TParam paramObj = null;
            //                BaseNode nodeTarget = null;
            //                switch (skillEffectConfig.SkillEffectType)
            //                {
            //                    case TSkillEffectType.TSET_GET_MAIN_ENTITY_ID:
            //                    case TSkillEffectType.TSET_GET_TARGET_ENTITY_ID:
            //                        {
            //                            var outputNodeName = string.Empty;
            //                            var logInfo = $"{graph.name}:{node.GetCustomName()}:{node.GetType().Name}, outputNodeName:";
            //                            foreach (var edge in node.GetAllEdges())
            //                            {
            //                                if (edge.inputNode == node && edge.outputNode is SkillEffectConfigNode outputNode)
            //                                {
            //                                    nodeTarget = outputNode;
            //                                    outputNodeName += edge.outputNode.GetCustomName() + " : " + edge.outputPort.portData?.displayName ?? string.Empty;
            //                                    if (int.TryParse(edge.outputPort.portData.identifier, out var index))
            //                                    {
            //                                        paramObj = outputNode.Config.Params.ExGet(index, null);
            //                                        if (paramObj != null)
            //                                        {
            //                                            // 断开连接
            //                                            graph.Disconnect(edge.GUID);
            //                                            graph.UpdateComputeOrder();
            //                                        }
            //                                        else
            //                                        {
            //                                            sbIDs.AppendLine($"[ERROR index] {logInfo}{outputNodeName}");
            //                                        }
            //                                    }
            //                                    else
            //                                    {
            //                                        sbIDs.AppendLine($"[ERROR identifier] {logInfo}{outputNodeName}");
            //                                    }
            //                                    break;
            //                                }
            //                            }
            //                            sbIDs.AppendLine($"{logInfo}{outputNodeName}");

            //                            // 移除节点
            //                            graph.RemoveNode(node);
            //                            graph.UpdateComputeOrder();

            //                            isDirty = true;
            //                            ++deleteCount;
            //                            deleteIDs.Add(configNode.GetID());
            //                            break;
            //                        }
            //                }
            //                if (paramObj != null)
            //                {
            //                    switch (skillEffectConfig.SkillEffectType)
            //                    {
            //                        case TSkillEffectType.TSET_GET_MAIN_ENTITY_ID:
            //                            // 替换数据——获取主体单位ID
            //                            paramObj.ExSetValue(nameof(paramObj.Value), (int)TCommonParamType.TCPT_MAIN_ENTITY);
            //                            paramObj.ExSetValue(nameof(paramObj.ParamType), TParamType.TPT_COMMON_PARAM);
            //                            nodeTarget.Save();
            //                            break;
            //                        case TSkillEffectType.TSET_GET_TARGET_ENTITY_ID:
            //                            // 替换数据——获取目标单位ID
            //                            paramObj.ExSetValue(nameof(paramObj.Value), (int)TCommonParamType.TCPT_TARGET_ENTITY);
            //                            paramObj.ExSetValue(nameof(paramObj.ParamType), TParamType.TPT_COMMON_PARAM);
            //                            nodeTarget.Save();
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //        if (isDirty)
            //        {
            //            graph.SaveGraphToDisk();
            //            EditorUtility.SetDirty(graph);
            //            AssetDatabase.SaveAssetIfDirty(graph);
            //        }
            //    });
            //    //Log.Error(sbInfo.ToString());
            //    //Log.Error($"deleteCount:{deleteCount}");
            //    sbIDs.AppendLine("-------------------------------------------------------------");
            //    sbIDs.AppendLine("删除节点总数:" + deleteCount);
            //    sbIDs.AppendLine("删除节点总数（去重）:" + deleteIDs.Count);
            //    sbIDs.AppendLine("-------------------------------------------------------------");
            //    foreach (var id in deleteIDs)
            //    {
            //        sbIDs.AppendLine("删除ID:" + id);
            //    }
            //    File.WriteAllText(Constants.SkillEditor.PathSavesJsons + "/DelteInfo.txt", sbIDs.ToString());
            //    AssetDatabase.Refresh();
            //});
            #endregion
        }

        //[TitleGroup("批处理工具", indent: true), ResponsiveButtonGroup("批处理工具/1"), Button("数据转化Graph->Json", ButtonSizes.Large), PropertySpace(5)]
        public void BatchProcessing_DataConvert()
        {
            if (!LocalSettings.IsProgramer())
            {
                EditorUtility.DisplayDialog("批处理_数据转化Graph->Json", "仅限程序操作", "好的");
                return;
            }
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            Utils.DisplayProcess("查找特定类型节点所在Graph信息", (_) =>
            {
                GraphHelper.ProcessEditor((manager) =>
                {
                    // 清理老的文件
                    //if (Directory.Exists(manager.PathSavesJsons))
                    //{
                    //    Directory.Delete(manager.PathSavesJsons, true);
                    //    Log.Debug($"清理文件夹 : {manager.PathSavesJsons}");
                    //}
                    var graphFiles = Directory.GetFiles(manager.PathSavesGraphs, $"*.asset", SearchOption.AllDirectories);
                    for (int i = 0, length = graphFiles.Length; i < length; i++)
                    {
                        // 加载graph，CustomJsonConverter解析json数据
                        // 详情跳转CustomJsonConverter.cs
                        var graphFile = graphFiles[i];
                        var graphFileInfo = new FileInfo(graphFile);
                        EditorUtility.DisplayProgressBar("批处理_数据转化Graph->Json", $"{i}/{length} : {graphFileInfo.Name}", i / (float)length);
                        BaseGraph graph = null;
                        Utils.WatchTime($"LoadGraph : {graphFileInfo.Name}", () =>
                        {
                            graph = GraphHelper.LoadGraph(manager.GraphType, graphFile);
                        });
                        if (graph)
                        {
                            var jsonPath = graphFile.Replace(manager.PathSavesGraphs, manager.PathSavesJsons).Replace(".asset", ".json");
                            //var jsonData = JsonConvert.SerializeObject(graph, Formatting.Indented);
                            Utils.WatchTime($"ToJson : {jsonPath}", () =>
                            {
                                var jsonData = JsonUtility.ToJson(graph, true);
                                Utils.WriteAllText(jsonPath, jsonData);
                            });
                        }
                    }
                });
            });
        }

        [TitleGroup("批处理工具"), ResponsiveButtonGroup("批处理工具/1"), Button("Npc事件数据修复", ButtonSizes.Large), PropertySpace(5)]
        public void BatchProcessing_NpcEventFix()
        {
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            var graghFiles = Directory.GetFiles(NpcEventEditorManager.Inst.PathSavesJsons, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < graghFiles.Length; i++)
            {
                var graghFile = graghFiles[i];
                Utils.PathFormat(ref graghFile);

                GraphHelper.ProcessGraph(graghFile, (graph) =>
                {
                    foreach (var node in graph.nodes)
                    {
                        if (node is NpcEventActionConfigNode myNode)
                        {
                            foreach(var edge in myNode.GetOutputEdges())
                            {
                                var identifier = edge.outputPort.portData.identifier;
                                if (identifier == "AIParamsID")
                                {
                                    graph.Disconnect(edge);
                                    break;
                                }
                            }
                            myNode.Config.ExSetValue("AIParamsID", default);
                        }
                    }

                    //移除某个节点
                    //for (int i = graph.nodes.Count - 1; i >= 0; i--)
                    //{
                    //    var node = graph.nodes[i];
                    //    if (node is RefConfigBaseNode refNode )
                    //    {
                    //        if (refNode.TableName == "NpcEventPlayerConditionConfig" || refNode.TableName == "NpcEventConditionConfig")
                    //        {
                    //            graph.RemoveNode(node);
                    //        }
                    //    }
                    //}


                    //移除一条边
                    //for (int i = graph.edges.Count - 1; i >= 0; i--)
                    //{
                    //    var edge = graph.edges[i];
                    //    if (edge.IsNeedRemove())
                    //    {
                    //        graph.edges.Remove(edge);
                    //    }
                    //}

                    graph.SaveGraphToDisk();
                });
            }
        }

        //[TitleGroup("批处理工具"), ResponsiveButtonGroup("批处理工具/1"), Button("获取编辑器总代码量", ButtonSizes.Large), PropertySpace(5)]
        public void BatchProcessing_GetAllCodeLines()
        {
            if (!EditorUtility.DisplayDialog("注意", "是否执行操作", "确认", "取消"))
            {
                return;
            }
            Utils.DisplayProcess("获取代码总行数", (sb) =>
            {
                var codeFiles = Directory.GetFiles(NodeEditorManager.PathPrefix, "*.cs", SearchOption.AllDirectories);
                int lines = 0;
                for (int i = 0; i < codeFiles.Length; i++)
                {
                    var filePath = codeFiles[i];
                    lines += File.ReadAllLines(filePath).Length;
                }
                sb.AppendLine($"总代码行数: {lines}");

                var cppPath = $"{NodeEditorManager.PathPrefix}../../../../../Program/Client/FsBattle/";
                var h = Directory.GetFiles(cppPath, "*.h", SearchOption.AllDirectories);
                var cpp = Directory.GetFiles(cppPath, "*.cpp", SearchOption.AllDirectories);
                var list = new List<string>(h);
                list.AddRange(cpp);
                lines = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    var filePath = list[i];
                    lines += File.ReadAllLines(filePath).Length;
                }
                sb.AppendLine($"总代码行数C++: {lines}");
            });
        }
    }
}

#endif
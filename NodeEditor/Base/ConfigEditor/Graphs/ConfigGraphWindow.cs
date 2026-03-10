using GameApp;
using GameApp.Native.Battle;
using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NodeEditor.NpcEventEditor;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static GameApp.Battle.Battle;

namespace NodeEditor
{
    // TODO 尝试解决Path问题
    public abstract partial class ConfigGraphWindow : NPBehaveGraphWindow/*, ISerializationCallbackReceiver*/, INodeEditorWindow
    {
        #region Event
        private static Action<INodeEditorWindow> onOpenWindow;
        public static event Action<INodeEditorWindow> OnOpenWindow
        {
            add
            {
                onOpenWindow -= value;
                onOpenWindow += value;
            }
            remove { onOpenWindow -= value; }
        }
        private static Action<INodeEditorWindow> onCloseWindow;
        public static event Action<INodeEditorWindow> OnCloseWindow
        {
            add
            {
                onCloseWindow -= value;
                onCloseWindow += value;
            }
            remove { onCloseWindow -= value; }
        }
        #endregion
        public abstract string PathExportExcel { get; }
        public abstract string NameEditor { get; }
        public string graphPath;
        protected static StringBuilder errorInfo = new StringBuilder();
        public ConfigGraph configGraph;

        // 编辑器操作标记，暂用作文件修改弹窗标记
        protected EditorFlag saveFlags = EditorFlag.GraphSaveFlags;

        public EditorFlag SaveFlags { get => saveFlags; set => saveFlags = value; }

        private bool isPauseGame = false;

        public bool IsPauseGame
        {
            get => isPauseGame;
            set
            {
                var battle = AppFacade.BattleManager?.Battle;
                if (battle == null)
                {
                    isPauseGame = false;
                    return;
                }

                curDebugNode = null;

                isPauseGame = value;

                if (isPauseGame)
                {
                    battle.PauseGame();
                    battle.SetBattleLogicUpdateStop(true, BattleLogicUpdateStopType.Default);
                }
                else
                {
                    battle.ResumeGame();
                    battle.SetBattleLogicUpdateStop(false, BattleLogicUpdateStopType.Default);
                }
            }
        }

        // 记录所有打开得窗口
        private static HashSet<ConfigGraphWindow> cacheOpenedWindows = new HashSet<ConfigGraphWindow>();
        public static HashSet<ConfigGraphWindow> CacheOpenedWindows { get { return cacheOpenedWindows; } }

        protected override void OnEnable()
        {
            // 缓存数据提前记录处理，避免消息响应失败
            UpdateCacheWindows(true);

            base.OnEnable();

            _ = JsonGraphManager.Inst;

            if (configGraph == null && !string.IsNullOrEmpty(graphPath) && this != null)
            {
                // 编辑器关闭后，数据丢失，当前窗口直接关闭
                // 注：不做窗口恢复，避免打开编辑器耗时太大
                Utils.SafeCall(() => Close());
                Log.Info($"关闭编辑器窗口：{graphPath}");
            }
            else
            {
                if (configGraph)
                {
                    foreach (var node in configGraph.nodes)
                    {
                        if (node is ConfigBaseNode baseNode)
                        {
                            // 窗口刷新，数据全部标脏
                            baseNode.IsDirty2Sync = true;
                        }
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            //if (graph != null && graphView != null)
            //{
            //    saveFlags.ClrFlag();
            //}
            UpdateCacheWindows(false);
            base.OnDisable();
        }

        public void SetSavePath(string graphPath)
        {
            this.graphPath = graphPath;
            configGraph.path = graphPath;
            int index = graphPath.LastIndexOf('/'); ;
            string fileName = graphPath.Substring(index + 1, graphPath.Length - index - 1);
            fileName = fileName.Replace(".json", string.Empty);
            configGraph.name = fileName;
            TitleName = fileName;
            titleContent.text = fileName;
        }

        public void IgnoreFlagSave(bool reset = true)
        {
            errorInfo.Length = 0;
            if (graph is ConfigGraph configGraph)
            {
                configGraph.CheckConfigID(errorInfo);
            }
            DisplayError(graph.name);
            // 取消保存变化弹窗错误
            saveFlags.ClrFlag();
            saveFlags |= EditorFlag.Saving;
            graph.SaveGraphToDisk();
            if (reset)
                saveFlags |= EditorFlag.GraphSaveFlags;
            ShowNotification($"保存文件:{TitleName}");
        }


        public void IgnoreFlagClose(bool reset = true)
        {
            saveFlags.ClrFlag();
            this.Close();
            if (reset)
                saveFlags |= EditorFlag.GraphSaveFlags;
        }

        protected override void InitializeWindow(BaseGraph graph)
        {
            configGraph = graph as ConfigGraph;

            CreateGraphView();

            m_MiniMap = new MiniMap() { anchored = true };
            graphView.Add(m_MiniMap);

            CreateToolbarView();

            TitleName = graph.name;
            titleContent.text = GraphHelper.GetSimpleGraphName(graph.path);

            Log.Info($"打开窗口 : {TitleName}");

            AfterInitializeWindow();

            onOpenWindow?.Invoke(this);
        }
        
        protected override void InitializeGraphView(BaseGraphView view)
        {
            base.InitializeGraphView(view);
            // 默认选择第一个节点
            view.Focus();
            view.FrameFirstNode();
        }
        
        protected virtual void CreateGraphView()
        {
            graphView = new ConfigGraphView(this);
        }
        protected virtual void CreateToolbarView()
        {
            m_ToolbarView = new ConfigGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }
        protected virtual void AfterInitializeWindow()
        {
            AddEvent();
            // TODO 重新LoadGraph解决ID0问题？
            Utils.WatchTime($"{TitleName} 加载节点", () =>
            {
                graph.Load();
            }, false);
            EditorUtility.DisplayProgressBar(TitleName, "检查节点...", 0.7f);
            Utils.WatchTime($"{TitleName} 检查节点", () =>
            {
                errorInfo.Length = 0;
                configGraph.CheckConfigID(errorInfo);
                DisplayError(graph.name);
            }, false);
            EditorUtility.ClearProgressBar();
        }
        protected override void OnDestroy()
        {
            try
            {
                UpdateCacheWindows(false);
                //后面改添加事件，解耦
                EnableConsole(false);
                DelEvent();
                Log.Info($"关闭窗口 : {TitleName}");
                // TODO 保存确定
                EditorUtility.DisplayProgressBar(TitleName, "关闭编辑器...", 0.1f);
                if (configGraph)
                {
                    configGraph.Unload();

                    // 编辑graph assets资源
                    if (AssetDatabase.IsMainAsset(configGraph.GetInstanceID()))
                    {
                        Resources.UnloadAsset(configGraph);
                    }
                    else
                    {
                        DestroyImmediate(configGraph);
                    }
                    // 清理选择
                    if (Selection.activeObject == graphView?.nodeInspector)
                    {
                        Selection.activeObject = null;
                    }
                    // 清理undo
                    Undo.ClearUndo(configGraph);
                    configGraph = null;
                }
                onCloseWindow?.Invoke(this);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                base.OnDestroy();
            }
        }
        protected override void Update()
        {
            base.Update();
            if (IsPauseGame)
            {
                if(DebugingNode != null)
                {
                    var dtip = DebugingNode.debug ? "取消" : "设置";
                    ShowNotification($"断点:{DebugingNode.GetCustomName()}(F5:继续运行，F10:下一步，F9:{dtip}断点)", 1, LogLevel.None);
                }
                else if(curDebugNode != null)
                {
                    ShowNotification($"日志输出调试中，断点运行至{curDebugNode.nodeTarget.GetCustomName()}，按 Ctrl+R 继续运行", 1, LogLevel.None);
                }
                else
                {
                    ShowNotification($"暂停中，按 Ctrl+R 继续运行", 1, LogLevel.None);
                }
            }
            UpdateEdgeFlowPoint();
            UpdateBackup();
        }
        public override void RefreshWindow(bool reload = true)
        {
            // TODO 可能存在其他需要刷新数据
            //if (graph is SkillGraph skillGraph)
            //{
            //    skillGraph.Enable();
            //}
            base.RefreshWindow(reload);
        }
        public BaseGraph GetGraph()
        {
            return graph;
        }
        public BaseGraphView GetGraphView()
        {
            return graphView;
        }
        private void UpdateCacheWindows(bool isAdd)
        {
            if (isAdd)
            {
                cacheOpenedWindows.Add(this);
            }
            else
            {
                cacheOpenedWindows.Remove(this);
            }
            CheckRootResource();
        }
        private void CheckRootResource()
        {
            // 编辑器情况下，关闭资源检查
            if (AppFacade.GameEngine != null && AppFacade.Resource != null)
            {
                AppFacade.Resource.IsCheckRootResource = cacheOpenedWindows.Count > 0 ? false : true;
            }
        }
        #region Graph Save Event
        private void AddEvent()
        {
            if (configGraph != null)
            {
                configGraph.OnSaveGraphToDiskBefore += OnSaveToDiskBefore;
                configGraph.OnSaveGraphToDiskAfter += OnSaveToDiskAfter;
            }
            else
            {
                Log.Error($"{TitleName} AddEvent failed, configGraph is null");
            }
        }
        private void DelEvent()
        {
            if (configGraph != null)
            {
                configGraph.OnSaveGraphToDiskBefore -= OnSaveToDiskBefore;
                configGraph.OnSaveGraphToDiskAfter -= OnSaveToDiskAfter;
            }
        }
        private bool OnSaveToDiskBefore(ConfigGraph configGraph)
        {

            try
            {
                var graphPath = configGraph?.path;
                if (string.IsNullOrEmpty(graphPath))
                {
                    Log.Error($"OnSaveToDiskBefore failed, graph is null, {graphPath}");
                    return false;
                }

                var nameShow = configGraph?.name ?? graphPath;
                // 检查文件路径是否存在
                if (!File.Exists(graphPath))
                {
                    //string newPath = GraphHelper.FindGraphFile(graphPath);
                    if (string.IsNullOrEmpty(graphPath))
                    {
                        configGraph.SaveRet.AppendLine($"graph路径不存在:{graphPath}");
                    }
                    //else
                    //{
                    //    graphPath = newPath;
                    //    configGraph.path = graphPath;
                    //    int index = graphPath.LastIndexOf('/'); ;
                    //    string fileName = graphPath.Substring(index + 1, graphPath.Length - index - 1);
                    //    fileName = fileName.Replace(".json", string.Empty);
                    //    Debug.Log(configGraph.name + " " + fileName);
                    //    configGraph.name = fileName;
                    //    this.TitleName = fileName;
                    //    titleContent.text = fileName;
                    //}
                }

                saveFlags |= EditorFlag.Saving;
                string saveInfo = string.Empty;
                if (saveFlags.HasFlag(EditorFlag.GraphSaveFlags))
                {

                    //检查差异情况，询问是否保存
                    if (saveFlags.HasFlag(EditorFlag.DisplayChanged) && configGraph.IsDirty())
                    {
                        saveInfo = $"---文件数据已变动,是否保存？---\n";
                        //configGraph.SaveRet.AppendLine();
                    }

                    //// 检查Nodes连线有效性,现在取消连线会断开，此功能屏蔽
                    //foreach (var edge in configGraph.edges)
                    //{
                    //    // 查找连线node是否有效
                    //    if (!edge.IsValid())
                    //    {
                    //        configGraph.AddGraphFatalInfo($"连线异常:{edge.inputNode?.GetCustomName() ?? "输入"}-{edge.outputNode?.GetCustomName() ?? "输出"}", edge.inputNode, true);
                    //    }
                    //}

                    HashSet<int> newDefineNodeSet = new HashSet<int>();
                    int sKillConfigNodeCount = 0;

                    //检查连线问题
                    foreach (var node in configGraph.nodes)
                    {
                        if (node is SkillConfigNode) sKillConfigNodeCount++;
                        if (!(node is IConfigBaseNode configBaseNode) || node is SkillConfigNode)
                        {
                            continue;
                        }

                        newDefineNodeSet.Add(configBaseNode.GetID());

                        ////检测输入节点ID，是否都未连线
                        //if (node.GetAllEdges().Count() == 0)
                        //{
                        //    configGraph.SaveRet.AppendLine($"[ID:{configBaseNode.GetID()}_没有任何输入输出连线]");
                        //}

                        //foreach (var inputPort in node.inputPorts)
                        //{
                        //    var edges = inputPort.GetEdges();
                        //    if (inputPort.fieldName == "ID" && edges == null || edges.Count == 0)
                        //    {
                        //configGraph.SaveRet.AppendLine($"[ID:{configBaseNode.GetID()}_输入未连线]");
                        //    }
                        //}
                    }

                    //检查引用节点的有效性
                    foreach (var node in configGraph.nodes)
                    {
                        //优化操作，表格引用单独检查，检查自身即可
                        if (node is RefConfigBaseNode refConfigBaseNode)
                        {
                            if (refConfigBaseNode.Config == default)
                            {
                                configGraph.AddGraphFatalInfo($"[ID:{(node as IConfigBaseNode).GetID()}_表格引用无效]", node, true);
                                continue;
                            }
                        }

                        //检查模板节点是否有效

                        if (!(node is IParamsNode paramsNode) || !(node is IConfigBaseNode) || paramsNode.GetParamsAnnotation() == default)
                        {
                            continue;
                        }
                        IReadOnlyList<TParam> tParams = paramsNode.GetParamsList();
                        List<TParamAnnotation> paramsAnn = paramsNode.GetParamsAnnotation().paramsAnn;
                        int len = paramsAnn.Count;
                        if (paramsNode.GetParamsAnnotation().IsArray)
                        {
                            len = paramsNode.GetParamsAnnotation().ArrayStart;
                            if (len == 0)
                            {
                                continue;
                            }
                        }

                        if (tParams == default || tParams.Count <= 0)
                        {
                            continue;
                        }

                        for (var i = 0; i < len; i++)
                        {
                            if (i > tParams.Count - 1)
                            {
                                continue;
                            }

                            //WWYTODO这里注意下，之前有导致数据丢失的问题，提示一下
                            if (tParams[i] == default)
                            {
                                Log.Error($"$tParams index {i} Is Null");
                                continue;
                            }

                            TParam tParam = tParams[i];
                            TParamAnnotation paramAnn = paramsAnn[i];

                            //为默认值、非函数返回值类型或者在graph中能找得到
                            if (tParam.Value == 0 || tParam.ParamType != TParamType.TPT_FUNCTION_RETURN || newDefineNodeSet.Contains(tParam.Value))
                            {
                                continue;
                            }

                            var refTypes = paramAnn.GetValidRefTypeNames();
                            object table = default;
                            ITableManager tableManager = default;
                            //需要判断多个表，一般情况下单表
                            for (var j = 0; j < refTypes.Count; j++)
                            {
                                tableManager = DesignTable.GetTableManager(refTypes[j]);
                                if (tableManager == default)
                                {
                                    //说明该表结构没有定义
                                    continue;
                                }

                                //获取到表数据对象
                                table = tableManager.GetTableCell(tParam.Value);

                                if (table != default)
                                {
                                    break;
                                }
                            }

                            if (tableManager != default && table == default)
                            {
                                configGraph.AddGraphFatalInfo($"[ID:{(node as IConfigBaseNode).GetID()}_第{i + 1}个参数[{tParams[i].Value}]无该配置]", node, true);
                            }
                        }
                    }

                    ////WWYTODO根据技能节点的数量进行提示,暂时屏蔽，之后统一做
                    //if (sKillConfigNodeCount >1)
                    //{
                    //    configGraph.SaveRet.AppendLine($"[技能节点数量异常，存在{sKillConfigNodeCount}个]");
                    //}
                }

                //WWYTODO检查是否需要提示，是否该标志能完全屏蔽提示，有些提示是否必须提醒
                if ((configGraph.SaveRet.Length > 0 || !string.IsNullOrEmpty(saveInfo)) && saveFlags.HasFlag(EditorFlag.GraphSaveFlags))
                {
                    var info = configGraph.SaveRet.ToString(0, Math.Min(255, configGraph.SaveRet.Length));
                    info = $"{saveInfo}{info}";

                    if (configGraph.SaveRet.Length > 255)
                    {
                        info = $"{info}\n详情见Console界面...";
                    }
                    if (!EditorUtility.DisplayDialog($"是否保存文件:{nameShow}", info, "保存", "不保存"))
                    {
                        return false;
                    }
                }

                configGraph.graphChangeFlag = 0;

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                if (!EditorUtility.DisplayDialog("保存文件异常", ex.ToString(), "保存", "不保存"))
                {
                    return false;
                }
            }

            return true;
        }
        private void OnSaveToDiskAfter(ConfigGraph configGraph, bool result)
        {
            // 保存文件成功
            if (saveFlags.HasFlag(EditorFlag.ShowNotification))
            {
                if (result)
                {
                    ShowNotification($"保存成功:{TitleName}");
                }
                else
                {
                    ShowNotification($"保存失败:{TitleName}");
                }
            }
        }
        #endregion

        #region Help Function
        /// <summary>
        /// 定位节点
        /// </summary>
        public void FrameNode(string configName, int configID)
        {
            if (string.IsNullOrEmpty(configName) || configID == 0)
            {
                return;
            }
            graphView?.FrameNode((nodeView) =>
            {
                var node = nodeView?.nodeTarget;
                if (node is IConfigBaseNode configNode && !(node is IRefConfigBaseNode) &&
                    configNode.GetConfigName() == configName &&
                    configNode.GetConfigID() == configID)
                {
                    return true;
                }
                return false;
            });
        }
        /// <summary>
        /// 定位文件
        /// </summary>
        public bool PingObject()
        {
            return Utils.PingObject(graphPath);
        }

        /// <summary>
        /// 保存数据并操作SVN提交
        /// </summary>
        public void SaveAndSVNCommit()
        {
            // 保存
            configGraph?.SaveGraphToDisk();
            // 定位选择
            if (PingObject())
            {
                // 处理svn提交
                DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.CommitSelected();
            }
        }

        public static void SyncAllConfigData(ConfigGraphWindow self)
        {
            switch (Event.current.button)
            {
                case 1:
                    var winodws = Utils.GetAllWindow<ConfigGraphWindow>();
                    foreach(var window in winodws)
                    {
                        window.SyncConfigData();
                    }
                    break;
                default:
                    self.SyncConfigData();
                    break;
            }

            HotFix.Game.Data.GameUser.MapAnim.Clear(); // 清缓存
        }

        /// <summary>
        /// 同步表格数据
        /// </summary>
        public virtual void SyncConfigData()
        {
            if (!graph)
            {
                ShowNotification($"同步数据失败, graph 为空:{TitleName}");
                return;
            }
            try
            {
                CheckRootResource();
                Utils.WatchTime($"同步数据, 节点数量: {graph.nodes.Count}", () =>
                {
                    foreach (var node in graph.nodes)
                    {
                        //首先同步一下其他模板
                        if (node is ITemplateReceiverNode templateNode)
                        {
                            string path = templateNode.TemplateNodeData.GetTemplatePath();

                            foreach (var w in cacheOpenedWindows)
                            {
                                // 排除自身，避免死循环
                                if (w.graphPath == path && w != this)
                                {
                                    w.SyncConfigData();
                                }
                            }
                        }

                        if (!(node is IConfigBaseNode configNode))
                            continue;
                        if (!node.CanExport())
                            continue;
                        if (!(node is ConfigBaseNode configBaseNode))
                            continue;
                        // TODO 引用节点变动未标脏，暂时屏蔽待处理
                        // 性能优化，只同步脏标记数据
                        //if (!configBaseNode.IsDirty2Sync)
                        //    continue;
                        configBaseNode.IsDirty2Sync = false;
                        // 同步数据主要性能消耗点，屏蔽不必要Save，先观察观察
                        // 避免二进制数据未同步
                        //configBaseNode.Save();
                        var config = configNode.GetConfig();
                        DesignTable.UpdateConfigData(config);
                    }
                });
                if (enableConsole)
                    EnableConsole();
                ShowNotification($"同步数据成功:{TitleName}");
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        public void SaveData()
        {
            // 备份文件重置，刷新写入备份
            backupForce = true;
            // 编辑器下保存数据
            ExportData(configGraph, PathExportExcel, EditorFlag.SaveDefault);
        }

        /// <summary>
        /// 导出数据到Excel
        /// </summary>
        public void ExportData()
        {
            ExportData(configGraph, PathExportExcel);
        }
        public static bool ExportData<T>(T configGraph, string exportPath, EditorFlag flag = EditorFlag.ExportDefault) where T : ConfigGraph
        {
            if (configGraph == null) return false;

            errorInfo.Length = 0;

            var displayProcess = flag.HasFlag(EditorFlag.DisplayProcess);
            var displayDialog = flag.HasFlag(EditorFlag.DisplayDialog);
            var saveToExcel = flag.HasFlag(EditorFlag.SaveToExcel);
            var saveToAsset = flag.HasFlag(EditorFlag.SaveToAsset);
            var showNotification = flag.HasFlag(EditorFlag.ShowNotification);
            var titleName = "导出数据：" + configGraph.name;
            exportPath = Path.GetFullPath(exportPath);

            try
            {
                EditorUtility.DisplayProgressBar(titleName, "检查数据...", 0.0f);

                configGraph.CheckConfigID(errorInfo);

                //如果有错误，暂停导出
                if (displayDialog && DisplayError(titleName))
                {
                    return false;
                }

                if (displayProcess)
                {
                    EditorUtility.DisplayProgressBar(titleName, $"开始保存数据...[{configGraph.name}]", 0.5f);
                }

                if (saveToAsset)
                {
                    if (!configGraph.SaveGraphToDisk())
                    {
                        // 保存取消，直接返回跳过导出excel
                        return false;
                    }
                    else
                    {
                        // 保存成功，刷新下，Unity2022会报warning（https://developer.unity.cn/ask/question/65092ba3edbc2a7994abbca1）
                        AssetDatabase.Refresh();
                    }
                }

                if (saveToExcel)
                {
                    if (!File.Exists(exportPath))
                    {
                        var displayDialogInfo = $"\n未配置正确导出Excel路径:\n\n{exportPath}";
                        if (displayDialog)
                        {
                            EditorUtility.DisplayDialog(titleName, displayDialogInfo, "好的");
                        }
                        else
                        {
                            Log.Error(displayDialogInfo);
                        }
                        return false;
                    }
                    else
                    {
                        var exportFileName = Path.GetFileName(exportPath);
                        if (!displayDialog || EditorUtility.DisplayDialog(titleName, $"\n【建议测试后，再导出Excel】\n\n数据已保存完成，是否导出到{exportFileName} ?", "导出", "不导出"))
                        {
                            while (Utils.IsFileOpened(exportPath))
                            {
                                var displayDialogInfo = $"【{exportFileName}】表格处于被打开状态/只读模式，请先关闭！";
                                if (!displayDialog)
                                {
                                    Log.Error(displayDialogInfo);
                                }
                                if (displayDialog && EditorUtility.DisplayDialog(titleName, displayDialogInfo, "已关闭，继续导出", "不导出了"))
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (!Utils.IsFileOpened(exportPath))
                            {
                                if (displayProcess) EditorUtility.DisplayProgressBar(titleName, "开始保存数据->表格Excel...", 0.7f);
                                // 保存期间关闭监听
                                ExcelManager.TurnFileWatcher(false);
                                {
                                    var configs = new List<object>();
                                    foreach (var node in configGraph.nodes)
                                    {
                                        if (node is IConfigBaseNode configNode && node.CanExport())
                                        {
                                            // 收集需要导出的配置
                                            configs.Add(configNode.GetConfig());
                                        }
                                    }
                                    // Excel数据写入，数据排序，清理空行
                                    ExcelManager.Inst.AddExcelMemberRefillableAction("NpcTemplateRuleConfig", (propName, configObj) =>
                                    {
                                        if(propName == "RoleCommonProperty")
                                        {
                                            return false;
                                        }
                                        return true;
                                    });
                                    bool isWriteExcel = ExcelManager.Inst.WriteExcel(exportPath, configs);

                                    ExcelManager.Inst.RemoveExcelMemberRefillableAction("NpcTemplateRuleConfig", default);

                                    // TODO 清理废弃ID
                                    //if (isWriteExcel)
                                    //{
                                    //    ExcelManager.Inst.FormatExcelData(exportPath, (sheetName, id) =>
                                    //    {
                                    //        if (id == 0) return false;
                                    //        删除无效ID数据
                                    //        foreach (var info in ConfigIDManager.Inst.config2Infos)
                                    //        {
                                    //            // 找到记录的表格名的所有ID，检查是否包含本ID
                                    //            if (sheetName.Contains(info.Key))
                                    //            {
                                    //                // 存在ID
                                    //                if (info.Value.ContainsID(id))
                                    //                {
                                    //                    return true;
                                    //                }
                                    //                break;
                                    //            }
                                    //        }
                                    //        return false;
                                    //    });
                                    //}

                                    if (showNotification) ShowNotification<ConfigGraphWindow>($"导出表格完成:{configGraph.name}");
                                }
                                ExcelManager.TurnFileWatcher(true);
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                if (displayDialog) EditorUtility.DisplayDialog(titleName, $"\n保存数据失败\n详情见Console界面", "OK");
                Log.Exception(ex);

                return false;
            }
            finally
            {
                if (displayProcess)
                {
                    EditorUtility.ClearProgressBar();
                    GUIUtility.ExitGUI();
                }
            }
        }
        public static T CreateWindow<T>(ConfigGraph graph, string path) where T : ConfigGraphWindow
        {
            try
            {
                // 支持打开多个技能窗口
                var win = Utils.CreateWindow<T>((w) =>
                {
                    return graph.name == w.TitleName;
                });
                win.Focus();
                win.graphPath = path;
                win.InitializeGraph(graph);
                // 打开会触发两次初始化，限制下次数
                win.RefreshWindow(false);
                //if (isCreated)
                //{
                //    // 屏蔽，创建Graph可能只是想要实现SkillEffect
                //    //win.AddDefaultNode();
                //}
                return win;
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
        protected static bool DisplayError(string title)
        {
            if (errorInfo.Length > 0 && title != null)
            {
                EditorUtility.DisplayDialog(title, errorInfo.ToString(0, Math.Min(255, errorInfo.Length)), "好的");
                errorInfo.Length = 0;
                return true;
            }
            return false;
        }
        public void ShowNotification(string tips, double fadeoutWait = 1, LogLevel logType = LogLevel.Debug)
        {
            switch (logType)
            {
                case LogLevel.Fatal:
                    Log.Fatal($"{NameEditor}: {tips}");
                    break;
                case LogLevel.Error:
                    Log.Error($"{NameEditor}: {tips}");
                    break;
                case LogLevel.Warning:
                    Log.Warning($"{NameEditor}: {tips}");
                    break;
                case LogLevel.Info:
                case LogLevel.Debug:
                    Log.Info($"{NameEditor}: {tips}");
                    break;
                default:
                    // do nothing
                    break;
            }
            // 会导致窗口绘制卡顿，
            var content = new GUIContent(tips);
            ShowNotification(content, fadeoutWait);
        }
        public static void ShowNotification<T>(string tips, double fadeoutWait = 1, LogLevel logType = LogLevel.Debug) where T : ConfigGraphWindow
        {
            if (Utils.HasOpenWindow<T>())
            {
                var content = new GUIContent(tips);
                Utils.GetWindow<T>((w) =>
                {
                    return w.hasFocus;
                })?.ShowNotification(tips, fadeoutWait, logType);
            }
        }
        #endregion

        #region 自动被缓存数据，减轻闪退数据丢失
        // 备份间隔/秒
        private const float backupInterval = 60;
        // 备份路径
        private const string backupDir = "/../NodeEditor/Backups/";
        // 上次备份数据变化标识
        private long backupGraphChangeFlag = 0;
        // 上次备份时间/秒
        private float backupTime = 0;
        // 是否强制备份
        private bool backupForce = false;
        private void UpdateBackup()
        {
            // 强制备份 或者 编辑数据情况下，60秒备份一次
            if (configGraph && (backupForce || (backupGraphChangeFlag != configGraph.graphChangeFlag && Time.realtimeSinceStartup - backupTime > backupInterval)))
            {
                backupForce = false;
                backupGraphChangeFlag = configGraph.graphChangeFlag;
                backupTime = Time.realtimeSinceStartup;
                // 写入备份
                var jsonContent = configGraph.ToJson();
                var filePath = Application.dataPath + backupDir + Utils.PathFull2SeparationFolder(configGraph.path, "/NodeEditor/");
                Utils.WriteAllText(filePath, jsonContent);
                Log.Debug($"UpdateBackup: {filePath}");
            }
        }
        #endregion 自动被缓存数据，减轻闪退数据丢失

        #region 调试功能
        
        public void SetNodeDebugBreak(BaseNode debugNode)
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
                return;
            DebugingNode = debugNode;

            graphView.ClearSelection();
            var nodeView = graphView.GetNodeView(DebugingNode);
            if (nodeView != null) 
            {
                battle.PauseGame();
                IsPauseGame = true;
                nodeView.FocusSelect();
            }
        }

        public void NodeDebugRunNext()
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
                return;
            //var nextNode = GetNextDebugNode();
            //if (nextNode == null)
            //{
            //    BattleWrapper.UnityEditorDebug_Run();
            //    return;
            //}
            //var configNode = nextNode as IConfigBaseNode;
            //if (configNode == null)
            //    return;

            //var configName = configNode.GetConfigName();
            //var configType = Utils.GetConfigNodeType(configName);

            if (battle.IsGamePause)
                battle.ResumeGame();
            BattleWrapper.UnityEditorDebug_RunUntilNextConfig();
        }

        public void NodeDebugStateSwitch()
        {
            if (graphView.selection.Count == 1)
            {
                var nodeView = graphView.selection[0] as BaseNodeView;
                if (nodeView != null)
                {
                    nodeView.ToggleDebug();
                    var battle = AppFacade.BattleManager?.Battle;
                    if (battle != null)
                    {
                        var node = nodeView.nodeTarget as IConfigBaseNode;
                        if (node != null)
                        {
                            var configName = node.GetConfigName();
                            var configType = Utils.GetConfigNodeType(configName);

                            if (nodeView.nodeTarget.debug)
                            {
                                BattleWrapper.UnityEditorDebug_AddDebugConfigId(configType, node.GetConfigID());
                                var bpDic = nodeView.nodeTarget.GetDebugBreakpointDic();
                                foreach (var bpData in bpDic)
                                {
                                    if (bpData.Value == null)
                                        continue;
                                    BattleWrapper.UnityEditorDebug_RefreshDebugCondition(configType, node.GetConfigID(), bpData.Value.ParamIndex, bpData.Value.Enable, bpData.Value.OpType, bpData.Value.CValue);
                                }
                            }
                            else
                                BattleWrapper.UnityEditorDebug_RemoveDebugConfigId(configType, node.GetConfigID());
                        }
                    }
                    OnRefreshPinnedView?.Invoke();
                }
            }
        }

        public void NodeDebugContinue()
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
                return;
            if (battle.IsGamePause)
                battle.ResumeGame();
            IsPauseGame = false;
            BattleWrapper.UnityEditorDebug_Run();
        }
        #endregion
        
        public void UpdateNodeRefState()
        {
            graphView.graph.UpdateRefNodeInfo();
            
            foreach (var nodeView in graphView.nodeViews)
            {
                if (nodeView is RefConfigBaseNodeView)
                {
                    continue;
                }

                var node = nodeView.nodeTarget as IConfigBaseNode;
                if (node == null)
                {
                    continue;
                }
                
                var refCount = graphView.graph.GetRefInfo($"TableDR.{node.GetConfigName()}Manager", node.GetConfigID())?.RefCount ?? 0;
                nodeView.UpdateRefCounter(refCount);
            }
        }
    }
}

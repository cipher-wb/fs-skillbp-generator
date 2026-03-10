using GraphProcessor;
using System;
using System.Collections;
using UnityEditor;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TableDR;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

namespace NodeEditor
{

    [Serializable, HideReferenceObjectPicker]
    public partial class ConfigBaseNode : BaseNode
    {
        /// <summary>
        /// 统一表格节点输出
        /// </summary>
        [FoldoutGroup("基础信息", true, -11), Input(allowMultiple = false), LabelText("ID"), ReadOnly]
        public int ID;
        [FoldoutGroup("基础信息"), Button("复制ID", ButtonSizes.Medium)]
        public void ClickCopyID()
        {
            GUIUtility.systemCopyBuffer = ID.ToString();
        }
        [FoldoutGroup("基础信息"), LabelText("节点描述"), TextArea(), OnValueChanged("OnDescChanged")]
        public string Desc;

        [FoldoutGroup("基础信息"), LabelText("节点UI描述"), NonSerialized, HideInInspector, ShowNodeView, UnityEngine.Multiline(), VisibleIf("IsNodeViewDesc", true)]
        public string NodeViewDesc;
        protected bool IsNodeViewDesc;

        #region 模板效果-自定义参数列表-可扩展
        [FoldoutGroup("模板信息", false, -9), LabelText("是否模板"), GraphProcessor.ShowInInspector(false), OnValueChanged("OnIsTemplateChanged")]
        public bool IsTemplate;

        [FoldoutGroup("模板信息"), LabelText("模板标记"), ShowIf("IsTemplate"), GraphProcessor.ShowInInspector(false), OnValueChanged("OnTemplateFlagsChanged")]
        public TemplateFlagsType TemplateFlags;

        [FoldoutGroup("模板信息"), LabelText("模板参数列表"), DelayedProperty(), OnValueChanged("OnTemplateParamsChanged", true), ShowIf("IsTemplate"), GraphProcessor.ShowInInspector(false)]
        [ListDrawerSettings(CustomAddFunction = "CustomAddTemplateParams")]
        public List<TParamAnnotation> TemplateParams = new List<TParamAnnotation>();

        // TODO 需改成只读显示
        [FoldoutGroup("模板信息"), LabelText("模板参数列表"), VisibleIf("IsTemplate", true), HideIf("@true"), ShowNodeView]
        public List<string> TemplateParamsDesc = new List<string>();

        [FoldoutGroup("模板信息"), LabelText("模板参数是否可增加"), VisibleIf("IsTemplate", true), ShowIf("IsTemplate"), ShowNodeView]
        public bool TemplateParamsCustomAdd = false;

        [Button("开启同步数据", ButtonSizes.Medium), FoldoutGroup("引用信息", false, -9), ShowIf("IsHideSyncTemplate")]
        public void OpenSyncTemplate()
        {
            IsShowQuote = true;

            graphDataSearchList ??= new SyncGraphDataSearchedList();
            graphDataSearchList.Clear();

            if (IsShowQuote)
            {
                //刷新所有未打开的模板
                var graphFiles = GraphHelper.GetGraphFiles();

                foreach (var path in graphFiles)
                {
                    var fileContent = File.ReadAllText(path);
                    if (fileContent.Contains($"\"TemplatePath\": \"{graph.path}\""))
                    {
                        graphDataSearchList.Add(path);
                    }
                }

                graphDataSearchList.Path = graph.path;
            }

        }

        [Button("关闭同步数据", ButtonSizes.Medium), FoldoutGroup("引用信息"), ShowIf("IsShowSyncTemplate")]
        public void CloseSyncTemplate()
        {
            IsShowQuote = false;

            graphDataSearchList?.Clear();
        }

        [Button("开启查看引用", ButtonSizes.Medium), FoldoutGroup("引用信息"), ShowIf("IsHideQuoteNode")]
        public void OpenQuoteNode()
        {
            var iConfig = this as IConfigBaseNode;
            if (iConfig == null)
            {
                return;
            }

            IsShowQuote = true;

            graphDataSearchList ??= new GraphDataSearchedList();
            graphDataSearchList.Clear();

            if (IsShowQuote)
            {
                graph.UpdateRefNodeInfo();

                var refInfo = graph.GetRefInfo($"TableDR.{iConfig.GetConfigName()}Manager", iConfig.GetConfigID());
                if (refInfo == null)
                {
                    return;
                }

                foreach (var graphPath in refInfo.PathsList)
                {
                    graphDataSearchList.Add(graphPath);
                }
                graphDataSearchList.Path = graph.path;
            }
        }

        [Button("关闭查看引用", ButtonSizes.Medium), FoldoutGroup("引用信息"), ShowIf("IsShowQuoteNode")]
        public void CloseQuoteNode()
        {
            IsShowQuote = false;

            graphDataSearchList?.Clear();
        }

        protected bool IsShowQuote { get; set; } = false;
        protected bool IsShowSyncTemplate => IsShowQuote && IsTemplate;
        protected bool IsHideSyncTemplate => !IsShowQuote && IsTemplate;
        protected bool IsShowQuoteNode => IsShowQuote && !IsTemplate;
        protected bool IsHideQuoteNode => !IsShowQuote && !IsTemplate;

        [FoldoutGroup("引用数据信息", true), LabelText("引用数据"), ShowIf("IsShowQuote"), JsonIgnore, Sirenix.OdinInspector.ShowInInspector]
        protected BsaeGraphDataSearchedList graphDataSearchList;

        public TemplateParamsChangeType TemplateParamsChangeType { get; set; }
        /// <summary>
        /// 标记是否数据被修改，暂用作运行时同步数据判定
        /// </summary>
        [NonSerialized]
        public bool IsDirty2Sync;
        private TParamAnnotation CustomAddTemplateParams()
        {
            return new TParamAnnotation();
        }
        private void OnTemplateParamsChanged()
        {
            DoUpdateTemplateParams();
            onNodeChanged?.Invoke(nameof(TemplateParams));
            TemplateProvider.RefreshAllWindowTemplateNodes(this.graph.path, TemplateParams);
        }
        private void DoUpdateTemplateParams()
        {
            // TODO 刷新问题，TemplateParamsDesc是否在ConfigBaseNodeView处理？
            // 描述文件变化刷新面板节点显示
            TemplateParamsChangeType = TemplateParamsChangeType.None;

            //判断是否有修改
            if (this is IConfigBaseNode configDataNdoe)
            {
                //如果没有数据，那可能是新增的节点没有保存导致没有数据记录，也有可能是有异常，添加新增标记
                if (JsonGraphManager.Inst.TryGetTemplateNodeInfo(configDataNdoe.GetConfigName(), configDataNdoe.GetConfigID(), out var nodeInfo))
                {
                    if(nodeInfo != null)
                    {
                        if (!nodeInfo.IsTemplate)
                        {
                            Log.Error("JsonGraphManager记录数据异常，模板节点记录的信息不是模板，是否外部操作过");
                            return;
                        }

                        //是新增
                        if (TemplateParams.Count > nodeInfo.TemplateParams.Count)
                        {
                            TemplateParamsChangeType |= TemplateParamsChangeType.AddOrRename;
                        }
                        //减少
                        else if (TemplateParams.Count < nodeInfo.TemplateParams.Count)
                        {
                            TemplateParamsChangeType |= TemplateParamsChangeType.Remove;
                        }

                        int minCount = TemplateParams.Count < nodeInfo.TemplateParams.Count ? TemplateParams.Count : nodeInfo.TemplateParams.Count;

                        for (var i = 0; i < minCount; ++i)
                        {
                            var templateParam = TemplateParams[i];
                            var nodeInfoParams = nodeInfo.TemplateParams[i];
                            
                            //名字变了重命名标签添加
                            if(templateParam.Name != nodeInfoParams.Name)
                            {
                                TemplateParamsChangeType |= TemplateParamsChangeType.AddOrRename;
                            }

                            //数据类型变了，数据改变标签
                            if (templateParam.RefTypeName != nodeInfoParams.RefTypeName 
                                || templateParam.RefPortTypeNames != nodeInfoParams.RefPortTypeNames)
                            {
                                TemplateParamsChangeType |= TemplateParamsChangeType.ChangeType;
                                break;
                            }

                            //默认值变了，新增创建标签
                            if((templateParam.DefalutParam != null && !templateParam.DefalutParam.Compare(nodeInfoParams.DefalutParam)) 
                                || (templateParam.DefalutParam == null && templateParam.DefalutParam != null))
                            {
                                TemplateParamsChangeType |= TemplateParamsChangeType.ChangeDefaultParams;
                            }
                        }
                    }
                    else
                    {
                        TemplateParamsChangeType |= TemplateParamsChangeType.AddOrRename;

                    }
                }
            }

            TemplateParamsDesc.Clear();

            foreach (var p in TemplateParams)
            {
                TemplateParamsDesc.Add(p.Name == null ? "" : p.Name);
            }
            // 同步信息到输入节点
            foreach (var edge in GetInputEdges())
            {
                if (edge.inputPort.portData.displayName.StartsWith(nameof(ID)))
                {
                    SyncPortDatas();
                    break;
                }
            }
        }
        protected virtual void OnExportChanged() { }

        protected virtual void OnTemplateFlagsChanged()
        {
            JsonGraphManager.Inst.RecordOrReleaseTemplateInfo(graph.path, this, IsTemplate);
            onNodeChanged?.Invoke(nameof(IsTemplate));
        }

        protected virtual void OnIsTemplateChanged()
        {
            if (!IsTemplate)
            {
                TemplateFlags ^=  TemplateFlagsType.IsChildTemplate;
            }

            var windows = Utils.GetAllWindow<ConfigGraphWindow>();
            foreach (var window in windows)
            {
                if (window.GetGraph() != graph)
                {
                    continue;
                }

                var nodeViews = window.GetGraphView().nodeViews;
                foreach (var nodeView in nodeViews)
                {
                    if (nodeView.nodeTarget is ConfigBaseNode configBaseNode && configBaseNode.IsTemplate && configBaseNode != this)
                    {
                        if(configBaseNode != this)
                        {
                            IsTemplate = false;
                            if (EditorUtility.DisplayDialog("存在模板标识", $"一个Graph只能存在一个模板标识", "是哪个", "好的"))
                            {
                                nodeView.FocusSelect();
                            }
                        }
                        else
                        {
                            JsonGraphManager.Inst.RecordOrReleaseTemplateInfo(graph.path, this , false);
                            DoUpdateTemplateParams();
                            onNodeChanged?.Invoke(nameof(IsTemplate));
                        }

                        return;
                    }
                }
            }

            JsonGraphManager.Inst.RecordOrReleaseTemplateInfo(graph.path, this, true);
            DoUpdateTemplateParams();
            onNodeChanged?.Invoke(nameof(IsTemplate));
        }
        protected virtual void OnDescChanged()
        {
            DoOnNodeChanged(nameof(Desc));
        }
        protected virtual void DoOnNodeChanged(string memberName)
        {
            onNodeChanged?.Invoke(memberName);
            IsNodeViewDesc = !string.IsNullOrEmpty(NodeViewDesc);
        }
        protected virtual string GetCustomNodeDesc() => null;
        #endregion

        #region Log
        public string GetLogPrefix()
        {
            return $"[{graph?.name ?? "空graph"}:{GetCustomName()}] ";
        }
        public void AppendSaveRet(string info)
        {
            graph?.AddGraphFatalInfo($"[ID:{ID}_{info}]", this, true);
        }

        public void AppendSaveMapEventRet(string info)
        {
            graph?.AddGraphFatalInfo($"{ID}_{info}", this, true);
        }
        #endregion

        /// <summary>
        /// 过滤指定节点
        /// </summary>
        /// <returns></returns>
        public virtual bool SpecificNodeFiltering(PortView outputPort, string portTitle, Type portType)
        {
            return true;
        }

        public virtual void DeserializeFromJson(string configJson)
        {

        }
    }
    /// <summary>
    /// 技能节点基础节点，用于通用设置
    /// </summary>
    [Serializable]
    public partial class ConfigBaseNode<T> : ConfigBaseNode, IConfigBaseNode where T : class, new()
    {
        #region virtual
        public override void OnNodeCreated()
        {
            base.OnNodeCreated();
            Preset();
        }
        public override void OnNodeRecreated() 
        {
            base.OnNodeRecreated();
            Deserialize();
            OnPresetPropertyList();
        }

        protected override void Enable()
        {
            base.Enable();
            onAfterEdgeConnected += OnAfterEdgeConnected;
            onAfterEdgeDisconnected += OnAfterEdgeDisconnected;
            IsDirty2Sync = true;
            if (isCreateNode2GenerateID)
            {
                isCreateNode2GenerateID = false;
                GenerateID();
                OnConfigChanged();
            }
        }
        protected override void Disable()
        {
            onAfterEdgeConnected -= OnAfterEdgeConnected;
            onAfterEdgeDisconnected -= OnAfterEdgeDisconnected;
            base.Disable();
        }
        protected override void Destroy()
        {
            // 暂时屏蔽，本次打开阶段不做节点删除回收，收益低，容易出bug
            //ConfigIDManager.Inst.ReleaseConfigID(GetConfigName(), GetConfigID(), graph?.path, 0);
            base.Destroy();
        }
        protected override void OnSave()
        {
            base.OnSave();
            try
            {
                SyncExtraData();
                SyncPortDatas();
                OnSaveCheck();
                Serialize();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
        /// <summary>
        /// ID不允许重置
        /// </summary>
        protected override bool CanResetPort(NodePort port)
        {
            if (port.fieldName == nameof(ID))
            {
                return false;
            }
            return base.CanResetPort(port);
        }
        public override bool CanExport()
        {
            // ID为0不导出
            if (ID == 0)
            {
                return false;
            }
            return base.CanExport();
        }
        protected virtual void OnAfterEdgeConnected(SerializableEdge edge)
        {
            IsDirty2Sync = true;
            // 刷新映射数据
            edge.SyncDatas();
            UpdateAllPortsLocal();
            RefreshData();
        }
        protected virtual void OnAfterEdgeDisconnected(SerializableEdge edge)
        {
            IsDirty2Sync = true;
            // 刷新映射数据
            edge.SyncDatas();
            UpdateAllPortsLocal();
            RefreshData();
        }
        /// <summary>
        /// 表格数据变化响应
        /// </summary>
        protected virtual void OnConfigChanged()
        {
            IsDirty2Sync = true;
            // ID变化，刷新节点
            ID = GetConfigID();
            bool changed = true;
            if (cacheID != ID)
            {
                SyncPortDatas();
                cacheID = ID;
            }
            else
            {
                changed = UpdateAllPortsLocal();
            }
            RefreshData();
            // TODO没必要实时，保存处理即可
            // 表格数据修改，实时同步到序列化数据
            if(changed)
            {
                var configStr = ConfigJson;
                Serialize();
                onConfigNodeChanged?.Invoke(configStr, ConfigJson);
            }
            //监听操作
            onNodeChanged?.Invoke(nameof(ID));
        }
        protected virtual void OnRefreshCustomName()
        {
            SetCustomName($"[{name}:{ID}]");
        }
        public override void OnNodePostionUpdated()
        {
            IsDirty2Sync = true;
            // 如果父节点是多节点，调整位置需要刷新下
            foreach (var edge in GetInputEdges())
            {
                if (edge.outputPort.portData.acceptMultipleEdges == true)
                {
                    SyncExtraData();
                }
            }
            base.OnNodePostionUpdated();
        }
        //public override void GetChildNodesRecursive(List<BaseNode> childNodes, bool isIncludeNodeWithoutEdge = false)
        //{
        //    base.GetChildNodesRecursive(childNodes, isIncludeNodeWithoutEdge);

        //    var annos = GetParamsAnnotation();
        //    foreach (var outport in outputPorts)
        //    {
        //        var edges = outport.GetEdges();
        //        if (edges?.Count > 0) continue;
        //        // 如果是表格节点，那么需要计算下未连线节点
        //        var fieldValue = outport.fieldInfo.GetValue(outport.fieldOwner);
        //        //if (outport.portData.displayType is PortType.ConfigPortTypeBase)
        //        //{
        //        //    if (fieldValue is PackedMembersData packedMembersData && packedMembersData.propertyMap != null &&
        //        //        packedMembersData.propertyMap.TryGetValue(outport.portData.identifier, out var propertyInfo))
        //        //    {
        //        //        var value = propertyInfo.GetValue()
        //        //    }
        //        //    switch (outport.fieldName)
        //        //    {
        //        //        case nameof(PackedParamsOutput):
        //        //            if (int.TryParse(outport.portData.identifier, out var index))
        //        //            {
        //        //                var paramAnno = annos.paramsAnn.GetAt(index, null);
        //        //                if (true)
        //        //                {

        //        //                }
        //        //            }
        //        //            break;
        //        //        case nameof(PackedMembersOutput):
        //        //            break;
        //        //    }
        //        //}
        //    }
        //}
        #endregion

        #region protected
        /// <summary>
        /// 缓存序列化数据
        /// </summary>
        protected void Serialize()
        {
            try
            {
                ConfigJson = JsonConvert.SerializeObject(Config, Formatting.None, defaultJsonSetting);
                TableTash = TableHelper.GetTableTash(GetConfigName()) ?? null;
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()} 序列化数据失败!\n{ex}");
            }
        }
        /// <summary>
        /// 反序列化数据
        /// </summary>
        protected void Deserialize()
        {
            try
            {
                // 以保存数据为准，不依赖表格
                // 注：因为表格数据的更新程度没有asset/json高，只有在批量修改表格数据时候，需要反向把数据同步到技能编辑器数据
                // 版本检查
                var configName = GetConfigName();
                var newTableTash = TableHelper.GetTableTash(configName);
                var isTableTashChanged = TableTash != newTableTash;
                // 版本未变化  反序列化Config
                if (!string.IsNullOrEmpty(ConfigJson))
                {
                    if (isTableTashChanged)
                    {
                        try
                        {
                            // 版本变化  以存储的json数据为准
                            Config = JsonConvert.DeserializeObject<T>(ConfigJson, customJsonSetting);
                            Log.Warning($"[表结构变化，已自动处理:{configName}] {GetLogPrefix()}");
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                var data = DesignTable.GetTableCell<T>(ID);

                                if (data != null)
                                    Config = data;

                                Log.Error($"[表结构变化，自动处理异常，从配表回读成功:{GetLogPrefix()}\n{ex}] ");
                            }
                            catch (Exception ex2)
                            {
                                Log.Error($"表格回读失败:{GetLogPrefix()} \n{ex2}");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            // 版本未变化  反序列化Config
                            try
                            {
                                Config = JsonConvert.DeserializeObject<T>(ConfigJson);
                            }
                            catch
                            {
                                // 存在异常采用自定义设置，减少数据损失
                                Config = JsonConvert.DeserializeObject<T>(ConfigJson, customJsonSetting);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Log.Error($"{GetLogPrefix()} 反序列化数据失败!\n{ex}");
                        }
                    }
                    // 窗口打开情况下，代码更新重载，数据需要触发刷新
                    OnConfigChanged();
                }
                {
                    Utils.ConvertReadOnlyListsToLists(Config);
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()} 反序列化数据失败!\n{ex}");
            }
        }

        public override void DeserializeFromJson(string configJson)
        {
            if (!string.IsNullOrEmpty(configJson))
                ConfigJson = configJson;
            Deserialize();
            UpdateAllPortsLocal();
            RefreshData();
        }

        protected void ConfigSetValue(string propertyName, object value, bool withChangeEvent)
        {
            Config.ExSetValue(propertyName, value);
            if (withChangeEvent)
            {
                OnConfigChanged();
            }
        }
        protected void TryRepaint()
        {
            if (enumeratorRepaint != null)
            {
                return;
            }
            if (Selection.activeObject is NodeInspectorObject nodeInspector)
            {
                foreach (var nodeView in nodeInspector.selectedNodes)
                {
                    if (nodeView.nodeTarget == this)
                    {
                        enumeratorRepaint = EditorCoroutineRunner.StartEditorCoroutine(IEForceRepaint());
                        break;
                    }
                }
            }
        }
        #endregion

        #region private
        private void RefreshData()
        {
            //GetID(true);
            OnRefreshCustomName();
            RefreshParamsDisplayName();
            DoOnNodeChanged(null);
        }
        protected override void DoOnNodeChanged(string memberName)
        {
            NodeViewDesc = GetNodeViewDesc(Config, Desc);
            base.DoOnNodeChanged(memberName);
        }
        private IEnumerator IEForceRepaint()
        {
            var select = Selection.activeObject;
            Selection.activeObject = null;
            yield return 0;
            Selection.activeObject = select;
            enumeratorRepaint = null;
        }
        /// <summary>
        /// 同步额外数据保存-临时做法，后续按照Json解析处理，优化导出等性能
        /// </summary>
        private void SyncExtraData()
        {
            if (Config != null)
            {
                // 记录表格名_ID
                Config2ID = $"{GetConfigName()}_{GetConfigID()}";
                // 记录描述+所属Graph
                if (Config is EditorBaseConfig editorBaseConfig)
                {
                    editorBaseConfig.EditorDesc = Desc;
                    editorBaseConfig.EditorGraphName = graph?.FileName ?? string.Empty;
                }
            }
        }
        #endregion

        #region Interface
        //[FoldoutGroup("基础信息", true, -10), Button("重新分配ID")]
        //private void EditorGenerateID()
        //{
        //    GenerateID(true);
        //}

        /// <summary>
        /// 是否新建节点待分配ID
        /// </summary>
        private bool isCreateNode2GenerateID = false;
        public int GenerateID(bool reset = false)
        {
            int id;
            var graphPath = string.Empty;
            if (graph != null)
            {
                graphPath = graph.path;
            }
            else
            {
                Log.Error($"{GetLogPrefix()} GenerateID error, graph is null");
            }
            if (!reset)
            {
                id = ConfigIDManager.Inst.GetNextConfigID(GetConfigName(), graphPath);
            }
            else
            {
                // 修改ID
                if (!IsPostProcessed)
                {
                    OnPostProcessing();
                }
                id = ConfigIDManager.Inst.GetResetConfigID(graphPath, GetConfigName(), GetID());
            }
            ID = id;
            Config.ExSetValue(InputName, ID);
            if (reset)
            {
                OnConfigChanged();
            }
            return ID;
        }
        /// <summary>
        /// 获取表格ID
        /// </summary>
        public int GetConfigID()
        {
            // 【性能优化】避免反射调用
            if (Config is ITable table)
            {
                return (int)table.GetKey();
            }
            var prop = ConfigType.GetProperty(InputName);
            if (prop != null)
            {
                return Convert.ToInt32(prop.GetValue(Config));
            }
            return 0;
        }
        /// <summary>
        /// 获取缓存表格ID
        /// </summary>
        public int GetID() => ID;
        /// <summary>
        /// 获取表格对象
        /// </summary>
        /// <returns></returns>
        public object GetConfig() => Config;
        /// <summary>
        /// 获取表格名
        /// </summary>
        public string GetConfigName() => ConfigType.Name;
        public string GetConfigJson() => ConfigJson;
        public string GetTableTash() => TableTash;
        public override string GetNodeSearchName()
        {
            var searchName = GetCustomName();
            if (GetParamsList() != null)
            {
                searchName += "_{";
                foreach (var info in GetParamsList())
                {
                    searchName = $"{searchName}{info.GetDisplayName()}_{info.Value};";
                }
                searchName += "}";
            }
            searchName += $"_{NodeViewDesc}";
            return searchName.Replace("\n", "_");
        }

        /// <summary>
        /// 检查表格版本数据
        /// </summary>
        public virtual bool OnPostProcessing()
        {
            IsDirty2Sync = true;
            IsPostProcessed = true;
            Deserialize();
            CheckParams();
            // 刷新下数据
            SyncPortDatas();
            RefreshData();
            SyncExtraData();
            if (!string.IsNullOrEmpty(Desc))
            {
                OnDescChanged();
            }
            return true;
        }
        public virtual bool OnSaveCheck()
        {
            #region 检查表数据
            if (graph == null)
            {
                Log.Fatal($"[ID:{ID}_graph数据为空]");
                return false;
            }
            if (Config == null)
            {
                AppendSaveRet("表格数据为空");
                return false;
            }
            if (GetConfigID() == 0)
            {
                AppendSaveRet(" ");
                return false;
            }
            #endregion

            // 检查转化数据
            BaseTransData?.CheckError();

            #region 检查Param
            var anno = GetParamsAnnotation();
            var paramName = GetParamsName();
            if (anno != null && !string.IsNullOrEmpty(paramName))
            {
                var paramObj = Config.ExGetValue(paramName);
                if (paramObj is List<TParam> paramsList)
                {
                    var curCount = paramsList.Count;
                    var newCount = anno.paramsAnn.Count;
                    if (!anno.IsArray)
                    {
                        if (curCount != newCount)
                        {
                            AppendSaveRet($"参数个数不一致:{curCount}/{newCount}");
                            return false;
                        }
                    }
                    // 检查数值有效性
                    for (int i = 0; i < curCount; ++i)
                    {
                        var tParam = paramsList[i];
                        TParamAnnotation tParamAnno = null;
                        var annoIndex = i;
                        if (anno.IsArray && i >= anno.ArrayStart)
                        {
                            annoIndex = anno.ArrayStart;
                        }
                        tParamAnno = anno.paramsAnn[annoIndex];
                        if (!tParamAnno.CheckTParam(tParam, out var _, out var error, true))
                        {
                            AppendSaveRet($"参数{i + 1}_{error}");
                        }
                    }
                }
            }

            #endregion
            return true;
        }
        #endregion
    }
}

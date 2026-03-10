using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable, HideReferenceObjectPicker]
    public sealed class GraphDataSearch
    {
        public enum CheckType
        {
            不检查 = 0,
            相等 = 1,
            不相等 = 2,
            大于 = 3,
            小于 = 4,
        }
        public static bool IsCheckOK(int a, int b, CheckType checkType)
        {
            switch (checkType)
            {
                case CheckType.不检查: return true;
                case CheckType.相等: return a == b;
                case CheckType.不相等: return a != b;
                case CheckType.大于: return a > b;
                case CheckType.小于: return a < b;
            }
            return false;
        }
        [HideLabel, HideReferenceObjectPicker, Serializable]
        public struct SearchParamInfo
        {
            [HideLabel, HorizontalGroup("查找参数"), ReadOnly]
            public string Name;

            [HideLabel, HorizontalGroup("查找参数")]
            public int Value;

            [LabelText("参数检查类型"), HorizontalGroup("查找参数")]
            public CheckType CheckType;
        }

        [HideLabel, HideReferenceObjectPicker, Serializable]
        public struct SearchTParamCheckInfo
        {
            [LabelText("数值"), HorizontalGroup("数值")]
            public int Value;

            [HideLabel, HorizontalGroup("数值")]
            public CheckType Value_CheckType;

            [LabelText("数值类型"), HorizontalGroup("数值类型")]
            public global::TableDR.TParamType ParamType;

            [HideLabel, HorizontalGroup("数值类型")]
            public CheckType ParamType_CheckType;

            [LabelText("数值系数(万分比)"), HorizontalGroup("数值系数(万分比)")]
            public int Factor;

            [HideLabel, HorizontalGroup("数值系数(万分比)")]
            public CheckType Factor_CheckType;

            public bool IsNeedCheck()
            {
                return Value_CheckType != CheckType.不检查 || ParamType_CheckType != CheckType.不检查 || Factor_CheckType != CheckType.不检查;
            }

            public bool IsMatch(int value, TParamType paramType, int factor)
            {
                if (!IsNeedCheck()) return false;
                return IsCheckOK(value, Value, Value_CheckType)
                    && IsCheckOK((int)paramType, (int)ParamType, ParamType_CheckType)
                    && IsCheckOK(factor, Factor, Factor_CheckType);
            }

            public bool IsMatch(TParam tParam)
            {
                if (!IsNeedCheck() || tParam == null) return false;
                return IsMatch(tParam.Value, tParam.ParamType, tParam.Factor);
            }
            public bool IsMatch(TSkillBuffAttrValueParam param)
            {
                if (ParamType != TParamType.TPT_ATTR) return false;
                if (!IsNeedCheck() || param == null) return false;

                if ((int)param.AttrType == Value)
                {
                    return true;
                }
                return IsMatch(param.Param);
            }

            public bool IsMatch(TSkillBuffTagValueParam param)
            {
                if (ParamType != TParamType.TPT_SKILL_PARAM) return false;
                if (!IsNeedCheck() || param == null) return false;

                if (param.SkillTagID_ParamType == TParamType.TPT_NULL && param.SkillTagID_ParamValue == Value)
                {
                    return true;
                }
                return IsMatch(param.SkillTagID_ParamValue, param.SkillTagID_ParamType, 0);
            }            
        }

        #region 按表格名查找
        [InlineButton("SearchByConfig", "点击查找")]
        [LabelText("表格名:"), TabGroup("查找-按表格类型"), ValueDropdown("GetConfigNames", DoubleClickToConfirm = true)]
        public string ConfigName;

        [LabelText("表格ID:"), TabGroup("查找-按表格类型")]
        public int ConfigID;

        [LabelText("是否包含引用节点-不保证精准:"), TabGroup("查找-按表格类型")]
        public bool IsCheckRefConfig;

        private void SearchByConfig()
        {
            Utils.DisplayProcess("按表格类型查找", (sbInfo) =>
            {
                SearchedList.Clear();
                List<JsonGraphInfo> jsonGraphInfos = null;
                if (string.IsNullOrEmpty(ConfigName) || ConfigID <= 0)
                {
                    sbInfo?.AppendLine("错误：请填写<表格名>及<表格ID>");
                }
                else
                {
                    // 如果填了ID，优先按照ID查找
                    if (JsonGraphManager.Inst.TryGetGraphInfos(ConfigName, ConfigID, out jsonGraphInfos))
                    {
                        if (jsonGraphInfos.Count > 1)
                        {
                            // 存在多个Graph
                            sbInfo?.AppendLine($"注意：存在ID重复");
                        }
                    }
                }
                if (jsonGraphInfos != null)
                {
                    // 查找引用节点
                    if (IsCheckRefConfig)
                    {
                        if (JsonGraphManager.Inst.TryGetRefConfigIDNodeInfos(ConfigName, ConfigID, out var outNodeInfos))
                        {
                            foreach (var outNodeInfo in outNodeInfos)
                            {
                                if (JsonGraphManager.Inst.TryGetGraghInfo(outNodeInfo.Owner?.graphPath, out var outGraghInfo))
                                {
                                    jsonGraphInfos.Add(outGraghInfo);
                                }
                            }
                        }
                    }
                    foreach (var jsonGraphInfo in jsonGraphInfos)
                    {
                        sbInfo.AppendLine($"{jsonGraphInfo.graphName}");
                        SearchedList.Add(jsonGraphInfo.graphPath, ConfigName, ConfigID);
                    }
                }
                if (SearchedList.Count() == 0)
                {
                    sbInfo.AppendLine($"未找到对应graph");
                }
            });
        }
        #endregion 按表格名查找

        #region 按节点类型查找
        [InlineButton("SearchByNode", "点击查找")]
        [LabelText("@NodeAnno != null ? \"节点类型: \" + NodeAnno.Name : \"节点类型: \""), TabGroup("查找-按节点类型"), ValueDropdown("GetNodeNames", DoubleClickToConfirm = true), OnValueChanged("OnValueChanged_SearchNodeName")]
        public NodeBaseAnnotation NodeAnno;

        [LabelText("节点ID:"), TabGroup("查找-按节点类型"),]
        public int NodeID;

        [LabelText("节点参数校验列表:"), TabGroup("查找-按节点类型"), HideReferenceObjectPicker, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, ShowFoldout = true, DraggableItems = false)]
        public List<SearchParamInfo> NodeParamList = new List<SearchParamInfo>();

        private void SearchByNode()
        {
            Utils.DisplayProcess("按节点类型查找", (sbInfo) =>
            {
                SearchedList.Clear();
                List<JsonGraphInfo> jsonGraphInfos = null;
                if (NodeAnno == null)
                {
                    sbInfo?.AppendLine("错误：请选择节点类型");
                }
                else
                {
                    var nodeName = NodeAnno.NodeName;
                    var configName = NodeAnno.ConfigName;
                    var configID = NodeID;
                    var checkParam = NodeParamList.Count((p) => { return p.CheckType != CheckType.不检查; }) > 0;
                    if (JsonGraphManager.Inst.TryGetGraphInfos(configName, nodeName, out jsonGraphInfos))
                    {
                        for (int k = jsonGraphInfos.Count - 1; k >= 0; --k)
                        {
                            var jsonGraphInfo = jsonGraphInfos[k];
                            var valid = true;
                            if (configID > 0)
                            {
                                // 找到对应文件内表格数据
                                if (jsonGraphInfo.TryGetConfig(configName, configID, out var config))
                                {
                                    // 找到后，对比下检查的参数
                                    if (checkParam)
                                    {
                                        valid = CheckSearchParams(config);
                                    }
                                }
                                else
                                {
                                    valid = false;
                                }
                            }
                            else if (checkParam)
                            {
                                var result = false;
                                foreach (var node in jsonGraphInfo.Nodes)
                                {
                                    if ((string.IsNullOrEmpty(nodeName) || node.NodeName == nodeName) &&    // 相同节点
                                        (CheckSearchParams(node.Config)))                                   // 参数符合
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                                if (!result)
                                {
                                    // 没找到符合参数条件
                                    valid = false;
                                }
                            }
                            if (!valid)
                            {
                                jsonGraphInfos.RemoveAt(k);
                            }
                        }
                    }
                }
                if (jsonGraphInfos != null)
                {
                    foreach (var jsonGraphInfo in jsonGraphInfos)
                    {
                        sbInfo.AppendLine($"{jsonGraphInfo.graphName}");
                        SearchedList.Add(jsonGraphInfo.graphPath);
                    }
                }
                if (SearchedList.Count() == 0)
                {
                    sbInfo.AppendLine($"未找到对应graph");
                }
            });
        }
        #endregion 按节点类型查找

        #region 按参数查找
        [InlineButton("SearchByTParam", "点击查找")]
        [TabGroup("查找-按参数查找")]
        public SearchTParamCheckInfo tParamCheckInfo;
        private void SearchByTParam()
        {
            Utils.DisplayProcess("按节点类型查找", (sbInfo) =>
            {
                SearchedList.Clear();
                var jsonGraphInfos = new List<JsonGraphInfo>();
                if (!tParamCheckInfo.IsNeedCheck())
                {
                    sbInfo?.AppendLine("请选择检查类型");
                }
                else
                {
                    foreach (var kv in JsonGraphManager.Inst.Path2JsonGraphInfo)
                    {
                        var jsonGraphInfo = kv.Value;
                        var result = false;
                        foreach (var node in jsonGraphInfo.Nodes)
                        {
                            IReadOnlyList<TParam> paramList = null;
                            switch (node.Config)
                            {
                                case SkillEffectConfig skillEffectConfig: paramList = skillEffectConfig.Params; break;
                                case SkillConditionConfig skillConditionConfig: paramList = skillConditionConfig.Params; break;
                                case SkillSelectConfig skillSelectConfig: paramList = skillSelectConfig.Params; break;
                                case BuffConfig buffConfig:
                                    // buff的特殊处理
                                    switch (tParamCheckInfo.ParamType)
                                    {
                                        case TParamType.TPT_ATTR:
                                            if (buffConfig.Attrs?.Count > 0)
                                            {
                                                foreach (var attr in buffConfig.Attrs)
                                                {
                                                    if (tParamCheckInfo.IsMatch(attr))
                                                    {
                                                        result = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        case TParamType.TPT_SKILL_PARAM:
                                            if (buffConfig.SkillTags?.Count > 0)
                                            {
                                                foreach (var item in buffConfig.SkillTags)
                                                {
                                                    if (tParamCheckInfo.IsMatch(item))
                                                    {
                                                        result = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }
                            if (paramList != null)
                            {
                                foreach (var param in paramList)
                                {
                                    if (tParamCheckInfo.IsMatch(param))
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                            }
                            if (result)
                            {
                                jsonGraphInfos.Add(jsonGraphInfo);
                                break;
                            }
                        }
                    }
                }
                foreach (var jsonGraphInfo in jsonGraphInfos)
                {
                    sbInfo.AppendLine($"{jsonGraphInfo.graphName}");
                    SearchedList.Add(jsonGraphInfo.graphPath);
                }
                if (SearchedList.Count() == 0)
                {
                    sbInfo.AppendLine($"未找到对应graph");
                }
            });
        }
        #endregion 按参数查找

        #region 查找结果
        [ShowInInspector, HideLabel, HideReferenceObjectPicker]
        public GraphDataSearchedList SearchedList = new GraphDataSearchedList();
        #endregion

        #region 编辑器辅助函数
        private void OnValueChanged_SearchNodeName()
        {
            // 查找节点名变化，更新参数列表
            if (NodeAnno is ParamsAnnotation paramsAnno)
            {
                var paramsCount = paramsAnno.paramsAnn.Count;
                //paramList.Clear();
                NodeParamList.Clear();
                for (int i = 0; i < paramsCount; i++)
                {
                    // 参数名会实时变化，枚举名暂不支持，先屏蔽
                    //var tParam = new TParam();
                    //var anno = paramsAnno.paramsAnn[i];
                    //anno.CheckTParam(tParam, i, out _, out _);
                    //paramList.Add(tParam);
                    NodeParamList.Add(new SearchParamInfo
                    {
                        CheckType = CheckType.不检查,
                        Name = paramsAnno.paramsAnn[i].Name,
                        Value = 0,
                    });
                }
            }
            else
            {
                NodeParamList.Clear();
            }
        }
        private IEnumerable<ValueDropdownItem> GetNodeNames()
        {
            yield return new ValueDropdownItem("Null", null);
            foreach (var anno in TableAnnotation.Inst.GetNodeBaseAnnotations())
            {
                if (!string.IsNullOrEmpty(anno.ConfigName) && !string.IsNullOrEmpty(anno.NodeName))
                {
                    if (anno.IsConfigDerive())
                    {
                        yield return new ValueDropdownItem($"{anno.ConfigName}/{anno.Name}_{anno.NodeName}", anno);
                    }
                    else
                    {
                        yield return new ValueDropdownItem($"{anno.ConfigName}_{anno.NodeName}", anno);
                    }
                }
            }
        }
        private IEnumerable<ValueDropdownItem> GetConfigNames()
        {
            yield return new ValueDropdownItem("Null", string.Empty);
            foreach (var anno in TableAnnotation.Inst.GetNodeBaseAnnotations())
            {
                if (!string.IsNullOrEmpty(anno.ConfigName) && !string.IsNullOrEmpty(anno.NodeName))
                {
                    if (!anno.IsConfigDerive())
                    {
                        yield return new ValueDropdownItem(anno.ConfigName, anno.ConfigName);
                    }
                }
            }
        }
        #endregion 编辑器辅助函数
        private bool CheckSearchParams(object config)
        {
            if (config == null)
            {
                return false;
            }
            IReadOnlyList<TParam> paramList = null;
            switch (config)
            {
                case SkillEffectConfig skillEffectConfig: paramList = skillEffectConfig.Params; break;
                case SkillConditionConfig skillConditionConfig: paramList = skillConditionConfig.Params; break;
                case SkillSelectConfig skillSelectConfig: paramList = skillSelectConfig.Params; break;
            }
            if (paramList != null)
            {
                for (int i = 0, length = NodeParamList.Count; i < length; i++)
                {
                    var nodeParam = NodeParamList[i];
                    int nodeParamValueSearch = nodeParam.Value;
                    int nodeParamValue = paramList.GetAt(i, null)?.Value ?? 0;
                    if ((nodeParam.CheckType == CheckType.相等 && nodeParamValueSearch != nodeParamValue) ||
                        (nodeParam.CheckType == CheckType.不相等 && nodeParamValueSearch == nodeParamValue) ||
                        (nodeParam.CheckType == CheckType.大于 && nodeParamValueSearch <= nodeParamValue) ||
                        (nodeParam.CheckType == CheckType.小于 && nodeParamValueSearch >= nodeParamValue))
                    {
                        // 参数不满足
                        return false;
                    }
                }
            }
            return true;
        }
    }

    // 查找结果
    [Serializable, HideReferenceObjectPicker]
    public sealed class GraphDataSearched
    {
        public string TemplatePath { get; set; }

        public string Path { get; private set; }
        private string configName;
        private int configID;

        [HorizontalGroup("Graph 查找信息", 0, 0, 10), /*LabelText("文件名")*/HideLabel, ShowInInspector]
        public string FileName { get { return !string.IsNullOrEmpty(Path) ? System.IO.Path.GetFileNameWithoutExtension(Path) : "空文件"; } }


        [HorizontalGroup("Graph 查找信息", 50, 0, 10), Button("定位")]
        private void PingFile()
        {
            Utils.PingObject(Path);
        }

        [HorizontalGroup("Graph 查找信息", 50, 0, 10), Button("打开")]
        public void Open()
        {
            var win = GraphAssetCallbacks.OpenGraphWindow(Path, false);
            if (win is ConfigGraphWindow configWin)
            {
                configWin.FrameNode(configName, configID);
            }
        }


        [HorizontalGroup("Graph 查找信息", 50), Button("导出")]
        public void Export()
        {
            if (File.Exists(Path))
            {
                Utils.DisplayProcess($"导出文件:{System.IO.Path.GetFileName(Path)}", (sbInfo)=>
                {
                    ExternalExportUtil.ExportNodeJson2Excel(new List<string> { Path });
                });
            }
        }

        public void Init(string path, string configName, int configID)
        {
            this.Path = path;
            this.configName = configName;
            this.configID = configID;
        }
    }

    [Serializable, HideReferenceObjectPicker]
    public abstract class BsaeGraphDataSearchedList
    {
        [HideInInspector]
        protected string path;
        public string Path
        {
            get
            {
                return path;
            }

            set
            {
                path = value;

                if (GraphDataSearched != null && GraphDataSearched.Count > 0)
                {
                    foreach (var info in GraphDataSearched)
                    {
                        info.TemplatePath = value;
                    }
                }
            }
        }
        public virtual List<GraphDataSearched> GraphDataSearched { get; set; } = new List<GraphDataSearched>();

        public void Add(string graphFile, string configName = null, int configID = 0)
        {
            var graphPath = Utils.PathFull2Assets(graphFile);
            // 避免重复，做下检测
            foreach (var data in GraphDataSearched)
            {
                if (data.Path == graphPath)
                {
                    return;
                }
            }
            var graphDataSearch = PoolObject.Get<GraphDataSearched>();
            graphDataSearch.Init(graphPath, configName, configID);
            GraphDataSearched.Add(graphDataSearch);
        }
        public void Clear()
        {
            foreach (var GraphDataSearched in GraphDataSearched)
            {
                PoolObject.Release(GraphDataSearched);
            }
            GraphDataSearched.Clear();
        }
        public int Count() => GraphDataSearched.Count;
    }

    [Serializable, HideReferenceObjectPicker]
    public sealed class SyncGraphDataSearchedList : BsaeGraphDataSearchedList
    {
        [InlineButton("SVNCommoit", "一键提交"), InlineButton("AutoSyncData", "一键同步")]
        [LabelText("查找文件列表"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false, NumberOfItemsPerPage = 10, ShowFoldout = true)]
        public List<GraphDataSearched> graphDataSearched = new List<GraphDataSearched>();

        public override List<GraphDataSearched> GraphDataSearched { get => graphDataSearched; set => graphDataSearched = value; }

        private void AutoSyncData()
        {
            if (graphDataSearched == null)
            {
                Log.Error("没有需要一键刷新的数据");
            }

            var myWindow = Utils.GetWindow<ConfigGraphWindow>(w => w.GetGraph().path == Path);

            if (myWindow != null)
            {
                myWindow.IgnoreFlagSave();
            }

            List<string> refeshList = new List<string>();

            foreach (var serrchInfo in graphDataSearched)
            {
                refeshList.Add(serrchInfo.Path);
            }

            NodeEditorTool.AutoSyncTemlateParams(Path, string.Empty, false, refeshList);

        }

        private void SVNCommoit()
        {
            if (graphDataSearched == null)
            {
                Log.Error("没有需要一键刷新的数据");
            }

            List<string> commitList = new List<string>();

            foreach (var serrchInfo in graphDataSearched)
            {
                commitList.Add(serrchInfo.Path);
            }

            DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.Commit(commitList, true);

        }
    }

    [Serializable, HideReferenceObjectPicker]
    // 查找结果列表
    public sealed class GraphDataSearchedList : BsaeGraphDataSearchedList
    {
        [InlineButton("ExportSearchDatas", "一键导出")]
        [LabelText("查找文件列表"), ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = false, NumberOfItemsPerPage = 10, ShowFoldout = true)]
        public List<GraphDataSearched> graphDataSearched = new List<GraphDataSearched>();

        public override List<GraphDataSearched> GraphDataSearched { get => graphDataSearched; set => graphDataSearched = value; }

        private void ExportSearchDatas()
        {
            int option = EditorUtility.DisplayDialogComplex("文件查找", $"查找到文件：{graphDataSearched.Count}个\n选择批处理全部文件?", "导出", "保存", "不导出");
            switch (option)
            {
                case 0: // 导出编辑器文件
                    {
                        List<string> paths = new List<string>();
                        foreach (var item in graphDataSearched)
                        {
                            paths.Add(item.Path);
                        }
                        // 效率较低，改成直接解析json数据
                        //GraphHelper.ExportGraph2Excel(paths, false, EditorFlag.DisplayDialog | EditorFlag.DisplayProcess);
                        Utils.DisplayProcess($"导出文件，共{paths.Count}个文件", (sbInfo) =>
                        {
                            sbInfo.AppendLine($"共处理文件: {paths.Count} 个");
                            ExternalExportUtil.ExportNodeJson2Excel(paths);
                        });
                        break;
                    }
                case 1: // 重新保存文件
                    {
                        // 效率较低，改成直接解析json数据
                        //GraphHelper.ExportGraph2Excel(paths, false, EditorFlag.DisplayDialog | EditorFlag.DisplayProcess);
                        int countAll = graphDataSearched.Count;
                        if (countAll > 0)
                        {
                            var title = $"保存文件，共{countAll}个文件";
                            var paths = new List<string>();
                            Utils.DisplayProcess(title, (sbInfo) =>
                            {
                                int count = 0;
                                foreach (var item in graphDataSearched)
                                {
                                    ++count;
                                    EditorUtility.DisplayProgressBar(title, $"{count}/{countAll} {item.FileName}", count / countAll);
                                    GraphHelper.ProcessGraph(item.Path, (graph) =>
                                    {
                                        graph.SaveGraphToDisk();
                                    });
                                    paths.Add(item.Path);
                                }
                                sbInfo.AppendLine($"共处理文件: {countAll} 个");
                            });
                            // 顺便弹窗确定是否提交
                            DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.Commit(paths, true, true);
                        }
                        break;
                    }
                case 2:// 取消
                    {
                        break;
                    }
                default:
                    Log.Error($"ExportSearchDatas error, invalid option: {option}");
                    break;
            }
        }
    }
}

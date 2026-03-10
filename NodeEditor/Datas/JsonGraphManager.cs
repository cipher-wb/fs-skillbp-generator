using GraphProcessor;
using HotFix;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class JsonGraphInfo
    {
        // 数据版本号，注：数据格式变动务必变动版本号
        public const string Version = "1.3";

        [Serializable]
        public class NodeInfo
        {
            [JsonIgnore]
            public JsonGraphInfo Owner;
            private string configName; 
            public string ConfigName
            {
                get
                {
                    if (configName == null)
                    {
                        if (IsConfigNode)
                        {
                            configName = Config2ID.Split('_')[0];
                        }
                        else if (IsTableReference && !string.IsNullOrEmpty(TableManagerName))
                        {
                            var split = TableManagerName.Replace(Constants.TableManagerSuffix, "").Split('.');
                            configName = split.Length == 2 ? split[1] : string.Empty;
                        }
                        else
                        {
                            configName = string.Empty;
                        }
                    }
                    return configName;
                }
            }
            private Type configType;
            [JsonIgnore]
            public Type ConfigType
            {
                get
                {
                    if (configType == null && !string.IsNullOrEmpty(ConfigName))
                    {
                        configType = TableHelper.GetTableType($"{ExcelManager.Inst.PackegeName}.{ConfigName}");
                    }
                    return configType;
                }
            }
            [JsonIgnore]
            public bool IsValid
            {
                get
                {
                    // 表格节点 或者 表格引用节点都记录
                    return !string.IsNullOrEmpty(Config2ID) || IsTableReference;
                }
            }
            public bool IsConfigNode
            {
                get
                {
                    return !string.IsNullOrEmpty(Config2ID);
                }
            }
            private object config; 
            [JsonIgnore]
            public object Config
            {
                get
                {
                    if (config == null && ConfigType != null && !string.IsNullOrEmpty(ConfigJson))
                    {
                        config = JsonConvert.DeserializeObject(ConfigJson, ConfigType);
                    }
                    return config;
                }
            }

            // TODO 整合Node数据，暂时自定义
            // 以下直接json反序列化
            public string GUID;
            public int ID;
            public string Desc;
            public bool IsExport;
            public bool IsTemplate;
            public TemplateFlagsType TemplateFlags;
            public string TableTash;
            public string ConfigJson;
            public string Config2ID;
            public List<TParamAnnotation> TemplateParams = new List<TParamAnnotation>();

            // 引用节点信息
            public string TableManagerName;

            // 其他自定义字段
            public int SkillEffectType;
            public int SkillConditionType;
            public int SkillSelectType;
            public string NodeName;
            public bool IsTableReference;

            public void Init(JsonGraphInfo owner, string nodeName)
            {
                Owner = owner;
                NodeName = nodeName;
                IsTableReference = nodeName == nameof(RefConfigBaseNode);
                configType = null;
                config = null;
                // 引用节点为false
                _ = ConfigType;

                if (TemplateParams != null && TemplateParams.Count > 0)
                {
                    foreach (var param in TemplateParams)
                    {
                        param.Load();
                    }
                }
            }
        }

        // 文件名：Path.GetFileName
        public string graphName;
        // 路径格式：Utils.PathFull2Assets
        public string graphPath;
        // 最近修改时间
        public DateTime lastWriteTime;
        // 所有节点信息
        public List<NodeInfo> Nodes = new List<NodeInfo>();
        // 是否为模板
        public bool IsTemplate { get; set; }
        public TemplateFlagsType TemplateFlags { get; set; }
        public bool TryGetConfig(string configName, int id, out object config)
        {
            config = default;
            foreach (var node in Nodes)
            {
                if (node.ConfigName == configName && node.ID == id)
                {
                    config = node.Config;
                    return true;
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 编辑器Json文件数据管理
    /// </summary>
    [Serializable]
    public sealed partial class JsonGraphManager : Singleton<JsonGraphManager>
    {
        // 项目工程外路径，避免文件高频import异常
        public static readonly string CACHE_DIR = Application.dataPath + "/../NodeEditor/Cache/";

        private static readonly string cacheFullPath = Utils.GetFullPath($"{CACHE_DIR}{nameof(JsonGraphManager)}_v{JsonGraphInfo.Version}.json");

        /// <summary>
        /// key:文件路径, value:文件信息，如：Assets/xxx/xxx/xx.json，信息
        /// 编辑器Graph保存时 或者 新增、移除、修改Json文件时刷新
        /// </summary>
        private Dictionary<string, JsonGraphInfo> path2JsonGraphInfo = new Dictionary<string, JsonGraphInfo>();
        public Dictionary<string, JsonGraphInfo> Path2JsonGraphInfo => path2JsonGraphInfo;

        /// <summary>
        /// key:表格名_ID, value:所属文件，如：SkillConfig_xxx, {Assets/xxx/xxx/xx.json, ...}
        /// 编辑器Graph保存时 或者 移动、重命名（移除+新增）Json文件时刷新
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, List<JsonGraphInfo>> ConfigNameID2GraphInfos { get; private set; } = new Dictionary<string, List<JsonGraphInfo>>();
        /// <summary>
        /// 同上，仅记录引用节点
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, List<JsonGraphInfo>> RefConfigNameID2GraphInfos { get; private set; } = new Dictionary<string, List<JsonGraphInfo>>();

        /// <summary>
        /// key:表格名, value:ID_节点列表，如：SkillConfig, {xxx, {Assets/xxx/xxx/xx.json, ...}}
        /// 编辑器Graph保存时 或者 新增、移除、修改Json文件时刷新
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, Dictionary<int, List<JsonGraphInfo.NodeInfo>>> Config2Nodes { get; private set; } = new Dictionary<string, Dictionary<int, List<JsonGraphInfo.NodeInfo>>>();
        /// <summary>
        /// 同上，仅记录引用节点
        /// </summary>

        [JsonIgnore]
        public Dictionary<string, Dictionary<int, List<JsonGraphInfo.NodeInfo>>> RefConfig2Nodes { get; private set; } = new Dictionary<string, Dictionary<int, List<JsonGraphInfo.NodeInfo>>>();

        /// <summary>
        /// key:表格名(仅限Effect|Condition|Select)，value:ID_NodeInfo，如："SkillEffectConfig",{74000001_<NodeInfo>}
        /// 编辑器包含模板节点的Graph保存时 或者 移动、重命名（移除+新增）Json文件时刷新
        /// </summary>
        private Dictionary<string, Dictionary<int, JsonGraphInfo.NodeInfo>> template2Nodes = new Dictionary<string, Dictionary<int, JsonGraphInfo.NodeInfo>>();
        [JsonIgnore]
        public Dictionary<string, Dictionary<int, JsonGraphInfo.NodeInfo>> Template2Nodes => template2Nodes;

        #region 解析模块
        public JsonGraphManager()
        {
            // 初始化Json监听
            InitJsonListenter();
            // 初始化数据
            InitData();
        }

        private void InitData()
        {
            // 检查缓存文件
            if (Directory.Exists(CACHE_DIR))
            {
                var cacheFiles = Directory.GetFiles(CACHE_DIR, $"{nameof(JsonGraphManager)}*.json", SearchOption.AllDirectories);
                foreach (var cacheFile in cacheFiles)
                {
                    // 移除非本版本的缓存数据，避免版本变动数据不刷新
                    var fullPath = Utils.GetFullPath(cacheFile);
                    if (fullPath != cacheFullPath)
                    {
                        Utils.SafeCall(() =>
                        {
                            File.Delete(fullPath);
                            Log.Debug($"JsonGraphManager delete old version cache file: {Path.GetFileName(fullPath)}");
                        });
                    }
                }
            }

            bool isReadFromCache = false;
            // TODO 删除老数据，按版本记录
            if (File.Exists(cacheFullPath))
            {
                Utils.WatchTime("JsonGraphManager 初始化：读取缓存文件", () =>
                {
                    // 存在缓存，先按照缓存读取
                    var cacheGraphInfos = Utils.ReadFromJson<Dictionary<string, JsonGraphInfo>>(cacheFullPath);
                    if (cacheGraphInfos == null)
                    {
                        // 读取失败
                        return;
                    }
                    isReadFromCache = true;
                    this.path2JsonGraphInfo = cacheGraphInfos;

                    var paths = path2JsonGraphInfo.Keys.ToArray();
                    foreach (var path in paths)
                    {
                        // 检测文件如果不存在，那么清理下数据，避免一些文件重命名导致记录信息没有清理
                        if (!File.Exists(path))
                        {
                            Log.Debug($"JsonGraphManager remove invalid data, path: {path}");
                            path2JsonGraphInfo.Remove(path);
                        }
                    }

                    // 解析所有编辑器Json文件数据，仅更新变化的数据
                    GraphHelper.ProcessEditor((manager) =>
                    {
                        var dir = new DirectoryInfo(manager.PathSavesJsons);
                        var fileInfos = dir.GetFiles("*.json", SearchOption.AllDirectories);
                        foreach (var fileInfo in fileInfos)
                        {
                            var path = Utils.PathFull2Assets(fileInfo.FullName);
                            var lastWriteTime = fileInfo.LastWriteTime;
                            if (path2JsonGraphInfo.TryGetValue(path, out var graphInfo) && lastWriteTime == graphInfo.lastWriteTime)
                            {
                                // 文件修改时间与缓存数据一致，不做解析，仅处理数据缓存
                                foreach (var nodeInfo in graphInfo.Nodes)
                                {
                                    nodeInfo.Owner = graphInfo;
                                    CacheNodeInfo(graphInfo, nodeInfo);
                                }
                                continue;
                            }
                            // TODO 变动文件异步刷新，等当前处理方式稳定
                            // 重新刷新GraphInfo
                            RefreshDataByJsonFile(fileInfo.FullName);
                        }
                    });
                });
            }
            // 给个保险，缓存读取失败就重新全部读取
            if (!isReadFromCache)
            {
                Utils.WatchTime("JsonGraphManager初始化：解析所有编辑器Json文件", () =>
                {
                    path2JsonGraphInfo.Clear();
                    // 解析所有编辑器Json文件数据
                    GraphHelper.ProcessEditor((manager) =>
                    {
                        var jsonFiles = Directory.GetFiles(manager.PathSavesJsons, "*.json", SearchOption.AllDirectories);
                        Log.Debug($"JsonGraphManager ProcessEditor: {manager.Name}, file num : {jsonFiles.Length}");
                        //int restCount = jsonFiles.Length;
                        foreach (var jsonFilePath in jsonFiles)
                        {
                            // 同步读取
                            CreateGraphByJsonPath(jsonFilePath);

                            // 异步读取，打开功能 观察观察，应该没问题了，之前存在文件争夺原因可能是ConfigIDManager也在读取文件资源导致
                            //CreateGraphByJsonPathAsync(jsonFilePath, () =>
                            //{
                            //    restCount--;
                            //});
                        }
                        //while (restCount > 0) ;
                    });
                });
            }
            CheckError();
            CacheData2FileAsync();
        }

        private StringBuilder sbError = new StringBuilder();
        private void CheckError()
        {
            // 检查文件冲突
            var files = Directory.GetFiles(NodeEditor.Constants.NodeEditorPath, "*.json.mine", SearchOption.AllDirectories);
            if (files.Length > 0)
            {
                var info = string.Join("\n", files);
                var dialogInfo = $"文件有冲突,请先解决svn冲突!\n{info}";
                if (dialogInfo.Length > 255)
                {
                    dialogInfo = dialogInfo.Substring(0, 255);
                }
                dialogInfo += "\n详情见Console界面";
                Log.Fatal(dialogInfo);
                UnityEditor.EditorUtility.DisplayDialog("文件冲突", dialogInfo, "好的");
            }

            // TODO 检查表格缺失信息

            // 检查重复数据，信息输出
            Utils.SafeCall(() =>
            {
                sbError.Length = 0;
                // 检查所有ID重复问题，包括不同文件及同个文件
                foreach (var kv in ConfigNameID2GraphInfos)
                {
                    // 默认第一个为常规节点，后续应该都是引用节点
                    // 注：如果存在第二个为常规节点，那么就是有ID重复错误，需要检查
                    if (kv.Value.Count > 1)
                    {
                        if (sbError.Length == 0)
                        {
                            sbError.AppendLine("【严重错误-ID重复】详情如下:");
                        }
                        var graphNames = kv.Value.Select((v) => v.graphName);
                        sbError.AppendLine($"{kv.Key}: \n\t{string.Join("\n\t", graphNames)}");
                    }
                }
                if (sbError.Length != 0)
                {
                    Log.Error(sbError.ToString());
                }

                if (LocalSettings.IsProgramer())
                {
                    var maxCount = 0;
                    var maxGraphName = string.Empty;
                    foreach (var kv in Path2JsonGraphInfo)
                    {
                        var graphInfo = kv.Value;
                        var nodeCount = graphInfo.Nodes.Count;
                        if (maxCount < nodeCount)
                        {
                            maxGraphName = graphInfo.graphName;
                            maxCount = nodeCount;
                        }
                    }
                    Log.Debug($"【编辑器信息】 [文件总数: {Path2JsonGraphInfo.Count}], [节点最多文件: {maxGraphName}, 节点数: {maxCount}]");
                }
            });
        }

        private bool isDirty = true;
        private bool isWriting = false;
        /// <summary>
        /// 缓存数据，注：时机待优化，文件变动/间隔时间脏缓存等
        /// </summary>
        private void CacheData2FileAsync()
        {
            // 异步将数据缓存
            if (isDirty)
            {
                if (!isWriting)
                {
                    isDirty = false;
                    isWriting = true;
                    Log.Debug($"刷新写入缓存数据 JsonGraphManager path : {cacheFullPath}");
                    _ = Utils.WriteToJsonAsync(path2JsonGraphInfo, cacheFullPath, (result) =>
                    {
                        isWriting = false;
                        if (!result)
                        {
                            Log.Error($"刷新写入缓存数据错误 JsonGraphManager path : {cacheFullPath}");
                        }
                    });
                }
            }
        }

        private void ProcessJsonData(dynamic refData, string jsonFilePathFormat, string jsonFilePath)
        {
            var data = refData["data"];
            var type = refData["type"];
            if (data != null)
            {
                var nodeInfo = JsonConvert.DeserializeObject<JsonGraphInfo.NodeInfo>(refData["data"].ToString()) as JsonGraphInfo.NodeInfo;
                if (!path2JsonGraphInfo.TryGetValue(jsonFilePathFormat, out var jsonGraphInfo))
                {
                    jsonGraphInfo = new JsonGraphInfo
                    {
                        graphName = Path.GetFileName(jsonFilePath),
                        graphPath = jsonFilePathFormat,
                        lastWriteTime = File.GetLastWriteTime(jsonFilePath),
                    };
                    path2JsonGraphInfo.Add(jsonFilePathFormat, jsonGraphInfo);
                }
                jsonGraphInfo.Nodes.Add(nodeInfo);
                nodeInfo.Init(jsonGraphInfo, type["class"].ToString());
                CacheNodeInfo(jsonGraphInfo, nodeInfo);
            }
        }

        private void CacheNodeInfo(JsonGraphInfo jsonGraphInfo, JsonGraphInfo.NodeInfo nodeInfo)
        {
            var searchKey = GetSearchKey(nodeInfo.ConfigName, nodeInfo.ID);
            // 记录表格信息
            if (nodeInfo.IsValid)
            {
                // 处理常规节点 or 引用节点
                var tempConfig2GraphInfos = nodeInfo.IsTableReference ? RefConfigNameID2GraphInfos : ConfigNameID2GraphInfos;
                var tempConfig2Nodes = nodeInfo.IsTableReference ? RefConfig2Nodes : Config2Nodes;
                if (!tempConfig2GraphInfos.TryGetValue(searchKey, out var graphInfos))
                {
                    graphInfos = new();
                    tempConfig2GraphInfos.Add(searchKey, graphInfos);
                }
                if (!tempConfig2Nodes.TryGetValue(nodeInfo.ConfigName, out var id2Nodes))
                {
                    id2Nodes = new Dictionary<int, List<JsonGraphInfo.NodeInfo>>();
                    tempConfig2Nodes.Add(nodeInfo.ConfigName, id2Nodes);
                }
                if (!id2Nodes.TryGetValue(nodeInfo.ID, out var nodes))
                {
                    nodes = new List<JsonGraphInfo.NodeInfo>();
                    id2Nodes.Add(nodeInfo.ID, nodes);
                }
                nodes.Add(nodeInfo);
                if (!graphInfos.Contains(jsonGraphInfo))
                {
                    graphInfos.Add(jsonGraphInfo);
                }

                // 模板节点
                if (nodeInfo.IsTemplate)
                {
                    if (!template2Nodes.TryGetValue(nodeInfo.ConfigName, out var templates))
                    {
                        template2Nodes.Add(nodeInfo.ConfigName, templates = new Dictionary<int, JsonGraphInfo.NodeInfo>());
                    }
                    templates[nodeInfo.ID] = nodeInfo;
                    jsonGraphInfo.IsTemplate = true;
                    jsonGraphInfo.TemplateFlags = nodeInfo.TemplateFlags;
                }
            }
        }
        #endregion

        #region 外部获取

        public bool IsTemplate(string path)
        {
            if(!path2JsonGraphInfo.TryGetValue(path, out var jsonGraphInfo) || jsonGraphInfo == null)
            {
                //Log.Fatal($"JsonGraphManager获取模板信息时，没有找到改路径的记录:{path}");
                return false;
            }

            return jsonGraphInfo.IsTemplate;
        }

        public bool IsChildTemplate(string path)
        {
            if (!path2JsonGraphInfo.TryGetValue(path, out var jsonGraphInfo) || jsonGraphInfo == null)
            {
                //Log.Fatal($"JsonGraphManager获取模板信息时，没有找到改路径的记录:{path}");
                return false;
            }

            return jsonGraphInfo.TemplateFlags.HasFlag(TemplateFlagsType.IsChildTemplate);
        }

        public bool TryGetTemplateNodeInfo(string path, out JsonGraphInfo.NodeInfo nodeInfo)
        {
            nodeInfo = null;

            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!path2JsonGraphInfo.TryGetValue(path, out var jsonGraphInfo) || jsonGraphInfo == null)
            {
                Log.Error($"JsonGraphManager尝试获取一个不存在的模板：{path}");
                return false;
            }

            if (jsonGraphInfo.IsTemplate && jsonGraphInfo.Nodes != null || jsonGraphInfo.Nodes.Count != 0)
            {
                foreach (var node in jsonGraphInfo.Nodes)
                {
                    if (node.IsTemplate)
                    {
                        nodeInfo = node;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetGraphInfos(string configName, int id, out List<JsonGraphInfo> graphInfos)
        {
            graphInfos = null;
            if (string.IsNullOrEmpty(configName) || id <= 0)
            {
                return false;
            }
            var searchKey = GetSearchKey(configName, id);
            if (ConfigNameID2GraphInfos.TryGetValue(searchKey, out var cacheGraphInfos))
            {
                graphInfos = new List<JsonGraphInfo>(cacheGraphInfos);
                return graphInfos.Count > 0;
            }
            return false;
        }

        public bool TryGetNodeInfo(string configName, int id, out JsonGraphInfo.NodeInfo nodeInfo)
        {
            nodeInfo = null;
            if (string.IsNullOrEmpty(configName) || id <= 0)
            {
                return false;
            }
            var searchKey = GetSearchKey(configName, id);
            if (Config2Nodes.TryGetValue(configName, out var nodeInfoDic))
            {
                if(nodeInfoDic.TryGetValue(id, out var nodes) && nodes.Count > 0)
                {
                    nodeInfo = nodes[0];
                    if (!nodeInfo.IsTableReference)
                    {
                        return true;
                    }
                    nodeInfo = null;
                    Log.Error($"JsonGraphManager尝试获取节点信息，{searchKey}找不到对应常规节点");
                    return false;
                }
            }
            return false;
        }

        public bool TryGetGraphInfos(string configName, string nodeName, out List<JsonGraphInfo> graphInfos)
        {
            graphInfos = null;
            bool checkNodeName = !string.IsNullOrEmpty(nodeName);
            if (Config2Nodes.TryGetValue(configName, out var id2Nodes))
            {
                var path2GraphInfos = new Dictionary<string, JsonGraphInfo>();
                foreach (var item in id2Nodes)
                {
                    var nodes = item.Value;
                    foreach (var node in nodes)
                    {
                        if (!checkNodeName || node.NodeName == nodeName)
                        {
                            if (!path2GraphInfos.TryGetValue(node.Owner.graphPath, out var jsonGraphInfo))
                            {
                                jsonGraphInfo = node.Owner;
                                path2GraphInfos.Add(node.Owner.graphPath, jsonGraphInfo);
                            }
                        }
                    }
                }
                graphInfos = path2GraphInfos.Values.ToList();
                return graphInfos.Count > 0;
            }
            return false;
        }

        public bool TryGetNodeInfos(string configName, string nodeName, out List<JsonGraphInfo.NodeInfo> nodeInfos, Func<JsonGraphInfo.NodeInfo, bool> validCondition = null)
        {
            nodeInfos = default;
            bool checkNodeName = !string.IsNullOrEmpty(nodeName);
            if (Config2Nodes.TryGetValue(configName, out var id2Nodes))
            {
                nodeInfos = new List<JsonGraphInfo.NodeInfo>();
                foreach (var item in id2Nodes)
                {
                    var nodes = item.Value;
                    foreach (var node in nodes)
                    {
                        if (!checkNodeName || node.NodeName == nodeName)
                        {
                            if (validCondition == null || validCondition.Invoke(node))
                            {
                                nodeInfos.Add(node);
                            }
                        }
                    }
                }
                return nodeInfos.Count > 0;
            }
            return false;
        }

        public bool TryGetGraphs(string configName, int id, out List<JsonGraphInfo> graphInfos)
        {
            var searchKey = GetSearchKey(configName, id);
            return ConfigNameID2GraphInfos.TryGetValue(searchKey, out graphInfos);
        }

        public bool TryGetTemplateNodeInfo(string configName, int id, out JsonGraphInfo.NodeInfo nodeInfo)
        {
            nodeInfo = null;

            if (template2Nodes.TryGetValue(configName, out var templates))
            {
                if (templates.TryGetValue(id, out nodeInfo))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取引用ID的节点数据，包含引用节点，以及非节点直接填数值
        /// </summary>
        public bool TryGetRefConfigIDNodeInfos(string configName, int id, out List<JsonGraphInfo.NodeInfo> outNodeInfos)
        {
            if (string.IsNullOrEmpty(configName) || id == 0)
            {
                outNodeInfos = default;
                return false;
            }
            // 注：可能存在重复
            outNodeInfos = new List<JsonGraphInfo.NodeInfo>();

            // 先查找纯引用节点
            if (RefConfig2Nodes.TryGetValue(configName, out var id2Nodes))
            {
                if (id2Nodes.TryGetValue(id, out var nodeInfos))
                {
                    foreach (var nodeInfo in nodeInfos)
                    {
                        outNodeInfos.Add(nodeInfo);
                    }
                }
            }

            // 屏蔽先
            return outNodeInfos.Count > 0;


            // 再查找节点参数直接填写的数据，按照参数申明类型查找
            // 先找到符合条件的节点类型
            foreach (var (anno, _, _) in TableAnnotation.Inst.GetParamsAnnotations())
            {
                var nodeName = anno.NodeName;
                for (int i = 0; i < anno.paramsAnn.Count; ++i)
                {
                    var paramAnn = anno.paramsAnn[i];
                    var refTypeNames = paramAnn.GetValidRefTypeNames(); // TODO 优化效率
                    if (refTypeNames?.Contains(configName) == true)
                    {
                        // 满足节点约束声明，查找所有符合类型的节点，比较ID
                        var isValid = TryGetNodeInfos(configName, nodeName, out var validNodeInfos, (checkNodeInfo) =>
                        {
                            // 目前参数命名都是Params，暂时以命名来查找
                            var config = checkNodeInfo.Config;
                            var paramsListObject = config.ExGetValue("Params");
                            if (paramsListObject != null)
                            {
                                var paramsList = paramsListObject as List<TParam>;
                                var paramValue = paramsList.ExGet(i, null);
                                // 找到满足的节点
                                if (paramValue != null && paramValue.Value == id)
                                {
                                    return true;
                                }
                            }
                            return false;
                        });
                        if (isValid)
                        {
                            outNodeInfos.AddRange(validNodeInfos);
                        }
                    }
                }
            }
            return outNodeInfos.Count > 0;
        }

        public bool Find(Func<JsonGraphInfo, bool> conditionFilter, Action<JsonGraphInfo> actionMatched)
        {
            if (conditionFilter == null)
            {
                return false;
            }
            var result = false;
            foreach (var item in path2JsonGraphInfo)
            {
                var info = item.Value;
                if (conditionFilter.Invoke(info))
                {
                    result = true;
                    actionMatched?.Invoke(info);
                }
            }
            return result;
        }

        public bool TryGetGraghInfo(string jsonFileFullPath, out  JsonGraphInfo graghInfo)
        {
            if (string.IsNullOrEmpty(jsonFileFullPath))
            {
                graghInfo = null;
                return false;
            }
            var jsonFilePathFormat = Utils.PathFull2Assets(jsonFileFullPath);
            return path2JsonGraphInfo.TryGetValue(jsonFilePathFormat, out graghInfo);
        }

        public bool TryOpenGraph(string configName, int configID)
        {
            var key = GetSearchKey(configName, configID);
            if (ConfigNameID2GraphInfos.TryGetValue(key, out var graphInfos))
            {
                if (graphInfos.Count > 0)
                {
                    var ownerGraph = graphInfos[0];
                    var win = GraphAssetCallbacks.OpenGraphWindow(ownerGraph.graphPath, false);
                    if (win is ConfigGraphWindow configWin)
                    {
                        configWin.FrameNode(configName, configID);
                        return true;
                    }
                }
                else
                {
                    Log.Error($"TryOpenGraph error, not find config in graph, configName:{configName}, configID:{configID}");
                }
            }
            return false;
        }

        public bool TryOpenGraphWithProgressBar(string configName, int configID, bool includeUnSaveNode = true)
        {
            bool result = false;
            Utils.DisplayProcess($"尝试打开对应编辑器文件<{configName}:{configID}>", (_) =>
            {
                // 是否需要查找未保存/新建节点信息
                if (includeUnSaveNode)
                {
                    // 优先查找已打开的编辑器
                    var windows = Utils.GetAllWindow<ConfigGraphWindow>();
                    foreach (var window in windows)
                    {
                        if (window.configGraph != null)
                        {
                            var node = window.configGraph.GetNodeByConfigNameAndID(configName, configID);
                            if (node != null)
                            {
                                result = true;
                                // 切换窗口
                                window.Focus();
                                // 定位节点
                                window.FrameNode(configName, configID);
                            }
                        }
                    }
                }
                // 未找到，继续从所有数据中查找
                if (!result)
                {
                    result = TryOpenGraph(configName, configID);
                }
            }, LogLevel.Info, EditorFlag.DisplayProcess);
            if (!result)
            {
                EditorUtility.DisplayDialog("打开技能编辑器", $"\n打开失败,未找到对应文件\n{configName}:{configID}", "好的");
            }
            return result;
        }

        #endregion

        #region 刷新数据
        public static string GetSearchKey(string configName, int id)
        {
            configName ??= string.Empty;
            return $"{configName}_{id}";
        }

        /// <summary>
        /// 记录模板的信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="configBaseNode"></param>
        /// <param name="isTemplate"></param>
        public void RecordOrReleaseTemplateInfo(string path, ConfigBaseNode configBaseNode, bool isTemplate)
        {
            string fatalInfo = "JsonGraphManager操作模板信息有误";

            if (!(configBaseNode is IConfigBaseNode configBaseData))
            {
                Log.Error($"{fatalInfo}，目标节点不是一个配置数据节点{configBaseNode}");
                return;
            }

            if(!Config2Nodes.TryGetValue(configBaseData.GetConfigName(), out var nodeInfoDic))
            {
                Log.Error($"{fatalInfo}，没有该配置信息的数据{configBaseData.GetConfigName()}");
                return;
            }

            if(!nodeInfoDic.TryGetValue(configBaseNode.ID, out var nodeInfos) || nodeInfos == null || nodeInfos.Count == 0)
            {
                Log.Error($"{fatalInfo}，没有该配置信息的ID{configBaseNode.ID}");
                return;
            }

            var nodeInfo = nodeInfos[0];
            if(nodeInfo.Owner.graphPath != path)
            {
                Log.Error($"{fatalInfo}，不是该模板的数据路径，获取数据来源路径{nodeInfo.Owner.graphPath}，操作模板路径：{path}");
                return;
            }

            nodeInfo.IsTemplate = isTemplate;
            nodeInfo.TemplateFlags = configBaseNode.TemplateFlags;

            if (nodeInfo.IsTemplate)
            {
                nodeInfo.Owner.IsTemplate = isTemplate;
                nodeInfo.Owner.TemplateFlags = configBaseNode.TemplateFlags;
            }
        }


        /// <summary>
        /// 刷新相关数据
        /// </summary>
        /// <param name="graph"></param>
        public void RefreshDataByEditor(BaseGraph graph)
        {
            if (string.IsNullOrEmpty(graph.path))
            {
                Log.Error("JsonGraphManger保存刷新数据时GraphInfo找不到了 graph.path is null");
                return;
            }
            if (!File.Exists(graph.path))
            {
                Log.Error($"JsonGraphManger保存刷新数据时GraphInfo找不到了 graph.path not find:{graph.path}");
                return;
            }

            RefreshDataByJsonFile(graph.path);
        }

        /// <summary>
        /// 强制刷新记录数据，先清理，再新增
        /// </summary>
        /// <param name="jsonFilePath">真实路径</param>
        public void RefreshDataByJsonFile(string jsonFilePath)
        {
            //刷新的话先移除旧数据，在添加新数据，删除出问题的话
            if (DeleteGraphByJsonPath(jsonFilePath))
            {
                isDirty = true;
                CreateGraphByJsonPath(jsonFilePath);
                Log.Debug($"JsonGraphManager RefreshDataByJsonFile: {jsonFilePath ?? string.Empty}");
            }
        }

        /// <summary>
        /// 创建新Graph的信息记录
        /// </summary>
        /// <param name="jsonFilePath">真实路径</param>
        private void CreateGraphByJsonPath(string jsonFilePath)
        {
            var fullPath = Utils.GetFullPath(jsonFilePath);
            if (path2JsonGraphInfo.TryGetValue(fullPath, out var jsonGraphInfo) && 
                jsonGraphInfo.lastWriteTime == File.GetLastWriteTime(fullPath))
            {
                // 修改时间一致，那么不再重复处理逻辑
                return;
            }
            var jsonObject = Utils.ReadFromJson<dynamic>(jsonFilePath);
            DoParseJsonData(jsonFilePath, jsonObject);
        }

        /// <summary>
        /// 异步加载技能图表
        /// </summary>
        private static readonly object thisLock = new object();
        [Obsolete]
        private void CreateGraphByJsonPathAsync(string jsonFilePath, Action finishCallback)
        {
            _ = Utils.ReadFromJsonAsync<dynamic>(jsonFilePath, (dynamic jsonObject) =>
            {
                lock (thisLock)
                {
                    DoParseJsonData(jsonFilePath, jsonObject);
                }
                finishCallback?.Invoke();
            });
        }
        private void DoParseJsonData(string jsonFilePath, dynamic jsonObject)
        {
            try
            {
                if (jsonObject == null || jsonObject.references == null)
                {
                    // 解析json错误
                    Log.Fatal($"JsonGraphManager Init failed, {jsonFilePath}");
                    return;
                }
                var jsonFilePathFormat = Utils.PathFull2Assets(jsonFilePath);
                if (jsonObject.references.ContainsKey("RefIds"))
                {
                    if (jsonObject.references["version"] != 2)
                    {
                        Log.Fatal($"数据格式和json数据的版本不一致，检测下是否有错!");
                    }

                    dynamic refIds = jsonObject.references.RefIds;
                    foreach (var item in refIds)
                    {
                        ProcessJsonData(item, jsonFilePathFormat, jsonFilePath);
                    }
                }
                else
                {
                    foreach (var item in jsonObject.references)
                    {
                        if (item.Name == "version")
                        {
                            continue;
                        }
                        ProcessJsonData(item.Value, jsonFilePathFormat, jsonFilePath);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"JsonGraphManager.DoParseJsonData error, ex:{ex}");
            }
        }

        /// <summary>
        /// 删除Graph
        /// </summary>
        public bool DeleteGraphByJsonPath(string path)
        {
            path = Utils.PathFull2Assets(path);

            if (!Path2JsonGraphInfo.TryGetValue(path, out var jsonGraphInfo))
            {
                return true;
            }

            try
            {
                //把所有节点移除
                foreach (var nodeInfo in jsonGraphInfo.Nodes)
                {
                    RemoveNodeInfo(nodeInfo);
                }
                jsonGraphInfo.Nodes.Clear();

                Path2JsonGraphInfo.Remove(path);
            }
            catch (Exception e)
            {
                Log.Error($"JsonGraphManger DeleteGraphByJsonPath 删除旧数据时出现异常: {e}");
                return false;
            }


            return true;
        }

        public void RenameGraphByJsonPath(string oldPath, string newPath)
        {
            oldPath = Utils.PathFull2Assets(oldPath);
            newPath = Utils.PathFull2Assets(newPath);

            if (!Path2JsonGraphInfo.TryGetValue(oldPath, out var jsonGraphInfo))
            {
                Log.Error($"JsonGraphManger重命名或者移动Graph数据时GraphInfo找不到了:{oldPath}");
                return;
            }

            var graphName = Path.GetFileName(newPath);
            jsonGraphInfo.graphPath = newPath;
            jsonGraphInfo.graphName = graphName;

            Path2JsonGraphInfo.Remove(oldPath);
            Path2JsonGraphInfo.Add(newPath, jsonGraphInfo);

            if (isAutoSyncGraphNames)
            {
                NodeEditorTool.AutoSyncGraphPath(oldPath, newPath, false);
            }
            else
            {
                //假如该文件是模板的话，要刷新所有引用的Graph
                string content = $"检测到文件路径{oldPath}->{newPath}发生变化，准备刷新引用数据";
                int op = EditorUtility.DisplayDialogComplex("同步文件",
                            content, "同步", "不再提醒", "不同步");

                switch (op)
                {
                    //导出单个
                    case 0:
                        //自动刷新模板不走提交
                        NodeEditorTool.AutoSyncGraphPath(oldPath, newPath, true);
                        break;
                    //多个导出
                    case 1:
                        isAutoSyncGraphNames = true;
                        NodeEditorTool.AutoSyncGraphPath(oldPath, newPath, false);
                        break;

                    //取消
                    case 2:
                        break;
                }
            }
        }


        /// <summary>
        /// 移除一个节点
        /// </summary>
        /// <param name="tableName">表格名</param>
        /// <param name="nodeId">节点Id</param>
        /// <param name="GUID">节点GUID，用于区分引用节点</param>
        public void RemoveNodeInfo(JsonGraphInfo.NodeInfo nodeInfo)
        {
            if (nodeInfo == null)
            {
                return;
            }
            var tableName = nodeInfo.ConfigName;
            var nodeId = nodeInfo.ID;
            var key = GetSearchKey(tableName, nodeId);
            var isConfigNode = nodeInfo.IsConfigNode;

            // 先移除config2Paths
            var tempConfigNameID2GraphInfos = isConfigNode ? ConfigNameID2GraphInfos : RefConfigNameID2GraphInfos;
            if (tempConfigNameID2GraphInfos.TryGetValue(key, out var graphInfos))
            {
                if (graphInfos.Remove(nodeInfo.Owner))
                {
                    if (graphInfos.Count == 0)
                    {
                        tempConfigNameID2GraphInfos.Remove(key);
                    }
                }
            }

            // 再移除Config2Nodes
            var tempConfig2Nodes = isConfigNode ? Config2Nodes : RefConfig2Nodes;
            if (tempConfig2Nodes.TryGetValue(tableName, out var nodeDic))
            {
                if (nodeDic.TryGetValue(nodeId, out var nodeInfos))
                {
                    nodeInfos.Remove(nodeInfo);
                    //nodeInfo.Owner.Nodes.Remove(nodeInfo);
                }
            }
        }
        #endregion
    }
}

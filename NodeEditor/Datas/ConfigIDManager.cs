using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TableDR;
using UnityEditor;
using GraphProcessor;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

namespace NodeEditor
{
    // TODO 继承单例类，区分各个Graph ID 生成规则
    [Serializable]
    public partial class ConfigIDManager
    {
        public const string name = "表格ID管理";
        /// <summary>
        /// 通用表格ID增加倍数
        /// </summary>
        [LabelText("通用表格ID增加倍数"), Sirenix.OdinInspector.ShowInInspector, ReadOnly, BoxGroup("信息")]
        private const int defaultConfigIDAdd = 10000;

        private readonly Dictionary<string, int> configIDAddMap = new Dictionary<string, int>
        {
            { nameof(ModelConfig), 100000 },
            { nameof(SkillEffectConfig), 1000000 },
            { nameof(TextConfig), 1000000 },
            { nameof(GuideConfig), 1000000 },

            //npc事件相关
            { nameof(NpcEventConfig), 1000000 },
            { nameof(MapEventStoryConfig), 1000000 },
            { nameof(NpcEventLinkConfig), 1000000 },
            { nameof(NpcEventActionGroupIndexConfig), 1000000},
            { nameof(NpcEventActionGroupConfig), 1000000 },
            { nameof(NpcEventActionConfig), 1000000 },
            { nameof(NpcTalkGroupConfig), 1000000 },
            { nameof(NpcTalkConfig), 1000000 },
            { nameof(NpcTalkOptionConfig), 1000000 },
            { nameof(NpcEventModelConfig), 1000000 },

            { nameof(MapEventConditionConfig), 1000000 },
            { nameof(MapEventConditionGroupConfig), 1000000 },
            { nameof(MapEventFormulaConfig), 1000000 },

            { nameof(MapEventGeneralFuncConfig), 1000000 },
            { nameof(MapEventGeneralFuncGroupConfig), 1000000 },

            { nameof(NpcTemplateRuleConfig), 10000}
        };

        private bool isDirty;
        private StringBuilder errorInfo = new StringBuilder();

        [BoxGroup("表格版本号记录"), JsonProperty, Sirenix.OdinInspector.ShowInInspector, ReadOnly]
        [DictionaryDrawerSettings(KeyLabel = "表格名", ValueLabel = "版本号TableTash", IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.OneLine)]
        private SortedDictionary<string, string> tableName2TableTash = new SortedDictionary<string, string>();

        [BoxGroup("最大表格ID记录，如<IP, <SkillConfig, MaxID>>")/*, JsonProperty*/, Sirenix.OdinInspector.ShowInInspector, ReadOnly]
        [DictionaryDrawerSettings(KeyLabel = "IP", ValueLabel = "最大ID<表格名:ID>", IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.OneLine)]
        private SortedDictionary<int, SortedDictionary<string, int>> ip2ConfigMaxIDs = new SortedDictionary<int, SortedDictionary<string, int>>();

        // SkillConfig_xxxx
        [JsonIgnore]
        public HashSet<string> allNodeIdSet = new HashSet<string>();

        // 记录所有id及对应文件 key: SkillConfig_xxxx, value: List<String> jsonPaths
        [JsonIgnore]
        private Dictionary<string, List<string>> config2Paths = new Dictionary<string, List<string>>();

        private static ConfigIDManager inst;
        public static ConfigIDManager Inst
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
        /// <summary>
        /// 是否保存json
        /// </summary>
        private static bool isSaving = false;
        public ConfigIDManager()
        {
            try
            {
                isDirty = false;
                TurnFileWatcher(true);
                InitCSharpBytesWatcher();
                InitJsonFileWatcher();
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }


        /// <summary>
        /// 依据json文件实例化数据单例
        /// </summary>
        public static bool CreateInstance()
        {
            string jsonData;
            var pathConfigID = Constants.SkillEditor.PathConfigID;
            if (!File.Exists(pathConfigID))
            {
                var data = new ConfigIDManager();
                jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(pathConfigID, jsonData);
            }
            else
            {
                jsonData = File.ReadAllText(pathConfigID);
            }
            inst = JsonConvert.DeserializeObject<ConfigIDManager>(jsonData);
            if (inst == null)
            {
                Log.Error($"加载文件失败: {pathConfigID}");
                return false;
            }
            Utils.WatchTime("ConfigIDManager 缓存所有ID", () =>
            {
                // 加载所有ID
                inst.LoadAllConfigID();
            });
            inst.AddEvent();
            return true;
        }
        public void AddEvent()
        {
            ConfigGraphWindow.OnOpenWindow += OnOpenWindow;
            ConfigGraph.OnSaveGraphToDisk += OnSaveGraphToDisk;
            // 加个编辑器下更新自动保存ID变化到文件
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }
        public void DelEvent()
        {
            ConfigGraphWindow.OnOpenWindow -= OnOpenWindow;
            ConfigGraph.OnSaveGraphToDisk -= OnSaveGraphToDisk;
            EditorApplication.update -= OnUpdate;
        }
        private void Clear()
        {
            // 清理IP对应ID数据，重新生成
            ip2ConfigMaxIDs.Clear();
        }
        public void LoadConfigID(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return;
            }
            #region 读文件关键字解析
            var graphName = Path.GetFileName(filePath);
            var graphPath = Utils.PathFormat(Path.GetFullPath(filePath));
            var configGraph = ConfigDataUtils.GetSingleOpenEditorConfigGraph(graphName);

            if (configGraph == default)
            {
                if (JsonGraphManager.Inst.TryGetGraghInfo(graphPath, out var graphInfo))
                {
                    foreach (var nodeInfo in graphInfo.Nodes)
                    {
                        // 非表格节点记录信息忽略
                        //if (!nodeInfo.IsValid || nodeInfo.IsTableReference) continue;

                        var id = nodeInfo.ID; 
                        var configName = nodeInfo.ConfigName;
                        if (id == 0 || string.IsNullOrEmpty(configName)) continue;
                        RecordConfigInfo(graphPath, configName, id);
                        ParseConfigIDs(configName, id);
                    }
                }
            }
            else
            {
                foreach (var node in configGraph.nodes)
                {
                    if(!(node is IConfigBaseNode iConfigBaseNode) || node is RefConfigBaseNode)
                    {
                        continue;
                    }

                    var id = iConfigBaseNode.GetConfigID();
                    var configName = iConfigBaseNode.GetConfigName();
                    RecordConfigInfo(graphPath, configName, id);
                    ParseConfigIDs(configName, id);
                }
            }
            #endregion
        }
        public void LoadAllConfigID(EditorFlag editorFlag = EditorFlag.None)
        {
            try
            {
                // 清理缓存
                errorInfo.Length = 0;
                Clear();
                // 缓存所有graph内的ID
                foreach (var item in JsonGraphManager.Inst.Path2JsonGraphInfo)
                {
                    var graphInfo = item.Value;
                    LoadConfigID(graphInfo.graphPath);
                }
                if (errorInfo.Length > 0)
                {
                    Log.Error(errorInfo.ToString());
                }
            }
            catch (Exception ex)
            {
                Log.Error($"解析表格ID最大可用值异常\n{ex}");
            }
            finally
            {
                if (editorFlag.HasFlag(EditorFlag.DisplayProcess))
                {
                    EditorUtility.ClearProgressBar();
                }
            }
        }

        private static readonly Regex idRegex = new Regex(@"(\d+)_(\d+)");
        public int GetResetConfigID(string graphName, string configName, int idOld)
        {
            var addNum = GetConfigIDAddNum(configName);
            int ip = idOld / addNum;
            // TODO 细化命名
            var match = idRegex.Match(graphName);
            if (match.Success)
            {
                // 如果graph命名按照规则，那么ID的IP由graph确定
                var idSplit = match.Value.Split('_');
                var ipGraph = int.Parse(idSplit[0]);
                if (ipGraph > 0 && ipGraph <= 255)
                {
                    ip = ipGraph;
                }
            }
            // 修改ID
            var idNew = GetNextConfigID(configName, graphName, ip, true);
            return idNew;

        }
        /// <summary>
        /// 获取表格最大ID，作为自动ID生成
        /// TODO 后续需要考虑 ID删除回收复用情况
        /// </summary>
        public int GetNextConfigID(string configName, string graphPath = null , int ip = 0, bool isRecord = true)
        {
            if (ip == 0) ip = LocalSettings.Inst.ID;
            var configMaxIDs = GetConfigMaxIDs(ip);
            if (configMaxIDs.TryGetValue(configName, out var nextID))
            {
                nextID = nextID + 1;
            }
            else
            {
                nextID = GetMinConfigID(configName, ip) + 1;
            }
            var idMax = GetMaxConfigID(configName, ip);

            // 检测ID是否在非编辑器表格使用
            var configManager = DesignTable.GetTableManager(configName);

            //检测是否有该Id的使用（是否考虑有引用问题）
            if (allNodeIdSet != default && allNodeIdSet.Count > 0)
            {
                while ((allNodeIdSet.Contains(JsonGraphManager.GetSearchKey(configName, nextID)) || (configManager != null && configManager.GetTableCell(nextID) != null)) && nextID <= idMax)
                {
                    ++nextID;
                }
            }

            if (nextID > idMax)
            {
                Log.Fatal($"GetNextConfigID error, configName:{configName} ID分配自动分配已达最大, ID上限:{idMax}, 当前分配ID:{nextID}");
                return 0;
            }
            if (isRecord)
            {
                configMaxIDs[configName] = nextID;
                RecordConfigInfo(graphPath, configName, nextID);
                //isDirty = true;
            }
            //Log.Error($"需要添加表格类型[{configName}]对应ID段，请修改:{Path.GetFileName(Constants.TableAnnotationJsonPath)}");
            return nextID;
        }
        public void ReleaseConfigID(string configName, int id, string graphPath = null, int ip = 0)
        {
            // 先检查下回收ID是否存在多个
            if (graphPath != null)
            {
                var key = JsonGraphManager.GetSearchKey(configName, id);
                if (config2Paths.TryGetValue(key, out var paths))
                {
                    // 如果存在多个，那么不进行ID回收
                    if (paths.Count > 1)
                    {
                        // 清除记录信息
                        paths.Remove(graphPath);
                        return;
                    }
                }
            }

            if (ip == 0) ip = LocalSettings.Inst.ID;
            var configMaxIDs = GetConfigMaxIDs(ip);

            if (configMaxIDs.TryGetValue(configName, out var maxID))
            {
                //WWYTODO释放Id也要把maxId确定到数量为准
                //回收策略改为减减
                var minID = GetMinConfigID(configName, ip);
                configMaxIDs[configName] = id > minID ? maxID - 1 : minID;
                ReleaseConfigInfo(graphPath, configName, id);
                // 如果这个节点是当前最大ID，那么回收下
                //if (maxID == id)
                //{
                //    var minID = GetMinConfigID(configName, ip);
                //    configMaxIDs[configName] = id > minID ? maxID - 1 : minID;
                //    ReleaseConfigInfo(graphName, configName, id);
                //}
            }
        }
        /// <summary>
        /// 是否已存在节点数据（包括表格数据-DesignTable，以及记录文件数据及打开编辑器问剑数据-allNodeIdSet）
        /// </summary>
        public bool HasConfigData(string configName, int id)
        {
            // 检查已有表格数据
            var config = DesignTable.GetTableCell(configName, id);
            if (config == null)
            {
                // 检查文件记录数据
                var key = JsonGraphManager.GetSearchKey(configName, id);
                return allNodeIdSet.Contains(key);
            }
            return true;
        }

        public string GetCreatorName(string configName, int id)
        {
            if (string.IsNullOrEmpty(configName) || id == 0)
            {
                return null;
            }
            var ip = id / GetConfigIDAddNum(configName);
            if (TemplateManager.Inst.TryGetPersonnelName(ip, out var name))
            {
                return name;
            }
            return $"未识别IP_{ip}";
        }

        #region Private
        private void ParseConfigIDs(string configName, int id)
        {
            try
            {
                // 配置这个ID的所属IP值
                var ipOrign = id / GetConfigIDAddNum(configName);
                var configMaxIDs = GetConfigMaxIDs(ipOrign);
                // 解析最大ID
                var idMin = GetMinConfigID(configName, ipOrign);
                var idMax = GetMaxConfigID(configName, ipOrign);

                if (id >= idMin && id < idMax)
                {
                    //用数量去记录最大已用ID
                    if (configMaxIDs.TryGetValue(configName, out var maxID))
                    {
                        // 记录ID
                        configMaxIDs[configName]++;
                    }
                    else
                    {
                        configMaxIDs[configName] = idMin + 1;
                    }
                }
                if (!configMaxIDs.ContainsKey(configName))
                {
                    configMaxIDs[configName] = idMin;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"检查ID最大可用值异常：{configName}:{id}\n{ex}");
            }
        }
        private SortedDictionary<string, int> GetConfigMaxIDs(int ip)
        {
            if (!ip2ConfigMaxIDs.TryGetValue(ip, out var confgiMaxIDs))
            {
                confgiMaxIDs = new SortedDictionary<string, int>();
                ip2ConfigMaxIDs.Add(ip, confgiMaxIDs);
            }
            return confgiMaxIDs;
        }
        /// <summary>
        /// 获取表格自增基数
        /// </summary>
        private int GetConfigIDAddNum(string configName)
        {
            if (configIDAddMap.TryGetValue(configName, out var idAdd))
            {
                return idAdd;
            }
            return defaultConfigIDAdd;
        }
        /// <summary>
        /// 获取ID最小值
        /// </summary>
        private int GetMinConfigID(string configName, int ip)
        {
            return ip * GetConfigIDAddNum(configName);
        }
        /// <summary>
        /// 获取ID最大值
        /// </summary>
        private int GetMaxConfigID(string configName, int ip)
        {
            return (ip + 1) * GetConfigIDAddNum(configName) - 1;
        }
        private void RefreshConfigID(string filePath)
        {
            // TODO 文件变动，需要刷新记录
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }
            var fileFullPath = Utils.GetFullPath(filePath);
            foreach (var kv in config2Paths)
            {
                kv.Value.Remove(fileFullPath);
            }
            LoadConfigID(filePath);
        }
        private void RecordConfigInfo(string graphPath, string configName, int id)
        {
            var key = JsonGraphManager.GetSearchKey(configName, id);
            allNodeIdSet.Add(key);

            if (!config2Paths.TryGetValue(key, out var paths))
            {
                paths = new List<string>();
                config2Paths.Add(key, paths);
            }
            var graphFullPath = Utils.GetFullPath(graphPath);
            if (!paths.Contains(graphFullPath))
            {
                paths.Add(graphFullPath);
            }
        }
        private void ReleaseConfigInfo(string graphPath, string configName, int id)
        {
            if (configName == null || graphPath == null)
            {
                Log.Error($"ReleaseConfigInfo error, graphPath:{graphPath ?? "null"}, configName:{configName ?? "null"}, id:{id}");
                return;
            }

            var key = JsonGraphManager.GetSearchKey(configName, id);
            allNodeIdSet.Remove(key);

            if (config2Paths.TryGetValue(key, out var paths))
            {
                var graphFullPath = Utils.GetFullPath(graphPath);
                paths.Remove(graphFullPath);
            }
        }
        private bool isCheckingTableTash = false;
        private void OnOpenWindow(INodeEditorWindow w)
        {
            if (!(w is ConfigGraphWindow win))
            {
                return;
            }
            // 刷新graph id记录信息
            LoadConfigID(win.GetGraph()?.path);

            // 检查表格版本号
            Utils.SafeCall(() =>
            {
                if (isCheckingTableTash)
                {
                    return;
                }
                isCheckingTableTash = true;
                errorInfo.Length = 0;
                foreach (var kv in DesignTable.EditorConfigManagerName2TableTash)
                {
                    var tableName = kv.Key;
                    var tableTashNew = kv.Value;
                    if (tableName2TableTash.TryGetValue(tableName, out var tableTashOld))
                    {
                        // 存在记录，比较下版本号
                        if (tableTashOld != tableTashNew)
                        {
                            errorInfo.AppendLine($"[表结构变化] {tableName}");
                        }
                    }
                    else
                    {
                        // 不存在记录，记录下
                        tableName2TableTash.Add(tableName, tableTashNew);
                        isDirty = true;
                    }
                }
                if (errorInfo.Length > 0)
                {
                    //根据策划需求，暂时屏蔽该提示
                    if (LocalSettings.IsProgramer())
                    {
                        string info = "表结构有变化，请告知程序处理！";
                        errorInfo.AppendLine($"\n{info}");
                        // 限制显示得最大长度
                        var showLength = Math.Min(255, errorInfo.Length);
                        EditorUtility.DisplayDialog(name, errorInfo.ToString(0, showLength), "好的");
                        Log.Fatal(errorInfo.ToString());
                        errorInfo.Length = 0;

                        // 只允许程序刷新版本号
                        if (LocalSettings.IsProgramer() && EditorUtility.DisplayDialog(name, "是否刷新版本号", "是", "否"))
                        {
                            foreach (var kv in DesignTable.EditorConfigManagerName2TableTash)
                            {
                                var tableName = kv.Key;
                                var tableTashNew = kv.Value;
                                if (tableName2TableTash.TryGetValue(tableName, out var tableTashOld))
                                {
                                    // 存在记录，比较下版本号
                                    if (tableTashOld != tableTashNew)
                                    {
                                        tableName2TableTash[tableName] = tableTashNew;
                                        errorInfo.AppendLine($"[已刷新版本] {tableName}");
                                        isDirty = true;
                                    }
                                }
                            }
                            win.ShowNotification(errorInfo.ToString());
                            errorInfo.Length = 0;
                        }
                    }
                }
                OnUpdate();
                isCheckingTableTash = false;
            });
        }
        private void OnSaveGraphToDisk(ConfigGraph graph)
        {
            // TODO 刷新graphID,暂时没必要
            //Utils.SafeCall(() =>
            //{
            //    LoadConfigID(graph?.path);
            //});
        }
        public void Save()
        {
            isSaving = true;
            var jsonData = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(Constants.SkillEditor.PathConfigID, jsonData);
            isSaving = false;
        }
        private void OnUpdate()
        {
            // ID变化自动保存到文件，避免ID重复分配问题
            if (isDirty)
            {
                Save();
                isDirty = false;
            }
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
                var info = new FileInfo(Constants.SkillEditor.PathConfigID);
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
            // 转回主线程处理逻辑
            EditorCoroutineRunner.StartEditorCoroutine(DoOnFileChangedJson(e));
        }
        private static IEnumerator DoOnFileChangedJson(FileSystemEventArgs e)
        {
            yield return null;
            if (!isSaving)
            {
                inst.DelEvent();
                inst = null;
                _ = Inst;
            }
            onChangedJson?.Invoke();
        }
        #endregion
    }
}

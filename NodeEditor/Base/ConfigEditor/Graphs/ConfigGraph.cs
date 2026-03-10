using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class ConfigGraph : NPBehaveGraph
    {
        public virtual string Version => "0.0.0";

        protected Func<ConfigGraph, bool> onSaveGraphToDiskBefore;
        public event Func<ConfigGraph, bool> OnSaveGraphToDiskBefore
        {
            add
            {
                onSaveGraphToDiskBefore -= value;
                onSaveGraphToDiskBefore += value;
            }
            remove { onSaveGraphToDiskBefore -= value; }
        }
        protected Action<ConfigGraph, bool> onSaveGraphToDiskAfter;
        public event Action<ConfigGraph, bool> OnSaveGraphToDiskAfter
        {
            add
            {
                onSaveGraphToDiskAfter -= value;
                onSaveGraphToDiskAfter += value;
            }
            remove { onSaveGraphToDiskAfter -= value; }
        }
        protected static Action<ConfigGraph> onSaveGraphToDisk;
        public static event Action<ConfigGraph> OnSaveGraphToDisk
        {
            add
            {
                onSaveGraphToDisk -= value;
                onSaveGraphToDisk += value;
            }
            remove { onSaveGraphToDisk -= value; }
        }
        public override bool Load()
        {
            try
            {
                base.Load();
                for (int i = nodes.Count - 1; i >= 0; --i)
                {
                    var node = nodes[i];
                    if (node is IConfigBaseNode configNode)
                    {
                        configNode.OnPostProcessing();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        public override bool SaveGraphToDisk()
        {
            try
            {
                var baseRet = base.SaveGraphToDisk();
                var result = false;
                var isSave = onSaveGraphToDiskBefore?.Invoke(this) ?? true;
                if (SaveRet.Length > 0)
                {
                    Log.Error($"{name} SaveGraphToDisk failed, Details are as follows\n{SaveRet}");
                }
                if (isSave)
                {
                    // 保存数据
                    if (AssetDatabase.IsMainAsset(GetInstanceID()))
                    {
                        // assets保存
                        EditorUtility.SetDirty(this);
                        AssetDatabase.SaveAssetIfDirty(this);
                        onSaveGraphToDisk?.Invoke(this);
                        result = true;
                    }
                    else
                    {
                        // json保存
                        if (!string.IsNullOrEmpty(path))
                        {
                            if (File.Exists(path))
                            {
                                var jsonData = ToJson();
                                System.IO.File.WriteAllText(path, jsonData);
                                onSaveGraphToDisk?.Invoke(this);
                                result = true;
                            }
                        }
                        else
                        {
                            Log.Fatal($"保存数据失败 {name}，graph路径为空");
                        }
                    }
                }
                onSaveGraphToDiskAfter?.Invoke(this, result);
                if (result)
                {
                    onSaveGraphToDisk?.Invoke(this);
                }

                JsonGraphManager.Inst.RefreshDataByEditor(this);
                return result;
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"[保存数据失败] {name}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 只针对Graph的变动
        /// </summary>
        /// <returns></returns>
        public bool IsDirty()
        {
            if (AssetDatabase.IsMainAsset(GetInstanceID()))
            {
                return EditorUtility.IsDirty(this);
            }
            else
            {
                //if (!string.IsNullOrEmpty(path) && File.Exists(path))
                //{
                //    //string pattern = "\"graphChangeFlag\":(.+),";

                //    //左边发生变动不提示,必须要走一次，否则连线会报一个未知的空值错误
                //    //string ignorePattern = "\"x\":(.+),|\"y\":(.+),|\"SerializedFormat\":(.+),";
                //    var jsonData = JsonUtility.ToJson(this, true);
                //    var jsonDataOld = File.ReadAllText(path);
                //    //string changeFlag = Regex.Match(jsonData, pattern).Value;
                //    //string changeFlagOld = Regex.Match(jsonDataOld, pattern).Value;
                //    ////jsonData = Regex.Replace(jsonData,pattern,string.Empty);
                //    ////jsonDataOld = Regex.Replace(jsonDataOld, pattern, string.Empty);
                //    ////Debug.Log($"New   {jsonData}");
                //    ////Debug.Log($"Old   {jsonDataOld}");
                //    //return changeFlag != changeFlagOld;
                //}
                return this.graphChangeFlag != 0;
            }
        }

        /// <summary>
        /// 外部是否有改变文件
        /// </summary>
        /// <returns></returns>
        public bool IsOutSideChange()
        {
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                string ignorePattern = "\"x\":(.+),|\"y\":(.+),|\"SerializedFormat\":(.+),";
                var jsonData = JsonUtility.ToJson(this, true);
                var jsonDataOld = File.ReadAllText(path);
                jsonData = Regex.Replace(jsonData, ignorePattern, string.Empty);
                jsonDataOld = Regex.Replace(jsonDataOld, ignorePattern, string.Empty);
                return jsonData != jsonDataOld;
            }

            return false;
        }

        // TODO 首次性能消耗优化
        // TODO 整理检查，转移到ConfigIDManager
        /// <summary>
        /// 打开编辑器检查节点
        /// </summary>
        /// <param name="sbError"></param>
        /// <returns></returns>
        public bool CheckConfigID(StringBuilder sbError)
        {
            #region 重复ID检查——本graph内
            var validNodes = new Dictionary<string, List<int>>();
            var invalidNodes = new List<BaseNode>();
            foreach (var node in nodes)
            {
                if (node is RefConfigBaseNode || node is RefTemplateBaseNode) continue;
                if (node is IConfigBaseNode configNode)
                {
                    var configName = configNode.GetConfigName();
                    var id = configNode.GetID();
                    var condigID = configNode.GetConfigID();
                    if (id == 0 || condigID == 0 || id != condigID)
                    {
                        sbError.AppendLine($"[ID异常] {node.GetCustomName()}，ID：{id}，ConfigID：{condigID}");
                        continue;
                    }

                    if (node is ITemplateReceiverNode templateNode)
                    {
                        if (!templateNode.IsValidTemplate(out int templateID))
                        {
                            sbError.AppendLine($"{condigID}_模板引用无效:{templateID}");
                        }
                    }

                    if (!validNodes.TryGetValue(configName, out var ids))
                    {
                        ids = new List<int>();
                        validNodes.Add(configName, ids);
                    }
                    if (!ids.Contains(id))
                    {
                        ids.Add(id);
                    }
                    else
                    {
                        invalidNodes.Add(node);
                    }
                }
            }
            bool invalid = invalidNodes.Count > 0;
            if (invalid)
            {
                foreach (var invalidNode in invalidNodes)
                {
                    if (invalidNode is IConfigBaseNode configNode)
                    {
                        var idOld = configNode.GetID();
                        var idNew = configNode.GenerateID(true);
                        var logError = $"[{name}][ID重复-本graph], 已自动转化ID:{idOld}->{idNew}";
                        sbError?.AppendLine(logError);
                        Log.Error(logError);
                    }
                }
                if (AssetDatabase.IsMainAsset(this))
                {
                    EditorUtility.SetDirty(this);
                    AssetDatabase.SaveAssetIfDirty(this);
                }
            }
            #endregion

            return invalid;
        }

        public string ToJson()
        {
            var jsonData = JsonUtility.ToJson(this, true);
            // 避免保存数据SerializedFormat不一致，统一替换下
            jsonData = jsonData.Replace("\"SerializedFormat\": 2,", "\"SerializedFormat\": 0,");
            return jsonData;
        }

        /// <summary>
        /// 找到非引用节点的表格节点
        /// </summary>
        public BaseNode GetNodeByConfigNameAndID(string targetConfigName, int ID)
        {
            foreach (var node in nodes)
            {
                if (node is IConfigBaseNode configNode && !(node is IRefConfigBaseNode))
                {
                    string configName = configNode.GetConfigName();
                    int id = configNode.GetConfigID();
                    if (configName == targetConfigName && ID == id)
                    {
                        return node;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 找到非引用节点的表格数据
        /// </summary>
        public object GetConfigByConfigNameAndID(string configName, int ID)
        {
            var node  = GetNodeByConfigNameAndID(configName, ID);
            if (node is IConfigBaseNode configBaseNode)
            {
                return configBaseNode.GetConfig();
            }
            return null;
        }

        #region 引用节点查询
        public override void UpdateRefNodeInfo()
        {
            RefNodeInfoDict.Clear();
            
            var graphFiles = GraphHelper.GetGraphFiles(GetType());
            foreach (var graphFlie in graphFiles)
            {
                var fileContent = File.ReadAllText(graphFlie);
                var list = GetRefConfigBaseNodeIds(fileContent);
                foreach (var data in list)
                {
                    var refTuple = (data.tableName, data.configID);
                    if (!RefNodeInfoDict.TryGetValue(refTuple, out var refNodeInfo))
                    {
                        refNodeInfo = new RefNodeInfo(path,data.tableName);
                        RefNodeInfoDict[refTuple] = refNodeInfo;
                    }
                    refNodeInfo.AddRef(graphFlie);
                }
            }
        }
        
        public override List<(string tableName, int configID)> GetRefConfigBaseNodeIds(string jsonString)
        {
            var result = new List<(string tableName, int configID)>();
    
            if (string.IsNullOrEmpty(jsonString))
                return result;

            try
            {
                var jsonObject = JObject.Parse(jsonString);
                var references = jsonObject["references"] as JObject;
                if (references == null)
                {
                    return result;
                }
                
                var nodesArray = references["RefIds"] as JArray;
                if (nodesArray == null)
                    return result;

                foreach (JObject node in nodesArray.Children<JObject>())
                {
                    // 检查必要层级是否存在
                    if (node["type"] == null || node["data"] == null)
                        continue;

                    var typeObj = node["type"] as JObject;
                    var dataObj = node["data"] as JObject;
                    if (typeObj == null || dataObj == null)
                    {
                        continue;
                    }

                    // 类型检查
                    var classType = typeObj["class"]?.Value<string>();
                    if (classType != "RefConfigBaseNode")
                        continue;

                    // 表格类型
                    var tableName = dataObj["TableManagerName"]?.Value<string>();

                    // ID检查
                    var id = dataObj["ID"]?.Value<int>();
                    if (id != null)
                    {
                        result.Add((tableName, id.Value));
                    }
                }
            }
            catch (JsonReaderException)
            {
                // JSON格式错误时返回空列表
            }
            catch (Exception ex)
            {
                // 其他异常记录日志后返回空
                System.Diagnostics.Debug.WriteLine($"解析错误: {ex.Message}");
            }

            return result;
        }
        #endregion
    }
}

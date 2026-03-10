using GraphProcessor;
using System;
using System.IO;

namespace NodeEditor
{
    public enum NodeType
    {
        None,
        /// <summary>
        /// 一般的节点
        /// </summary>
        ConfigNode,
        /// <summary>
        /// 表格引用节点
        /// </summary>
        RefNode,
        /// <summary>
        /// 模板引用节点
        /// </summary>
        TemplateRefConfig
    }

    public static class ConfigDataUtils
    {
        /// <summary>
        /// 获取表格数据（包含所有编辑器编辑过程中的表格数据）
        /// </summary>
        public static object GetConfig(string configName, int id)
        {
            object config = null;
            // 从打开的窗口里找
            if (config == null)
            {
                var wins = ConfigGraphWindow.CacheOpenedWindows;
                foreach (var win in wins)
                {
                    if (win.configGraph != null)
                    {
                        var node = win.configGraph.GetNodeByConfigNameAndID(configName, id);
                        if (node is IConfigBaseNode configBaseNode)
                        {
                            var tmpConfig = configBaseNode.GetConfig();
                            if (tmpConfig != null)
                            {
                                config = tmpConfig;
                            }
                        }
                    }
                }
            }

            // 从记录的文件信息里找
            if (config == null)
            {
                if (JsonGraphManager.Inst.TryGetNodeInfo(configName, id, out var nodeInfo))
                {
                    config = nodeInfo.Config;
                }
            }

            // 从表格数据找
            if (config == null)
            {
                config = DesignTable.GetTableCell(configName, id);
            }
            return config;
        }
        public static NodeType GetNodeType(this BaseNode self)
        {
            //模板引用节点
            if (self is RefConfigBaseNode refNode)
            {
                return NodeType.RefNode;
            }
            //表格引用节点
            if (self is ITemplateReceiverNode iTemplateNode)
            {
                return NodeType.TemplateRefConfig;
            }
            //一般节点
            else if (self is ConfigBaseNode configNode)
            {
                return NodeType.ConfigNode;
            }
            //???啥也不是,有可能是其他的编辑器

            return NodeType.None;
        }

        public static BaseNode GetSingleEditorConfigNode(string typeName, int id)
        {
            BaseNode node = GetSingleOpenEditorConfigNode(typeName, id);
            if (node == default)
            {
                node = GetSingleCacheEditorConfigNode(typeName, id);
            }

            return node;
        }

        public static object GetSingleEditorConfigNodeData(string typeName, int id)
        {
            return (GetSingleEditorConfigNode(typeName, id) as IConfigBaseNode)?.GetConfig();
        }

        public static ConfigGraphWindow GetSingleOpenConfigGraphWindow(BaseNode node)
        {
            var configGraphWindows = ConfigGraphWindow.CacheOpenedWindows;
            foreach (var configGraphWindow in configGraphWindows)
            {
                ConfigGraph configGraph = configGraphWindow.GetGraph() as ConfigGraph;
                if (configGraph != default && configGraph.nodes.Contains(node))
                {
                    return configGraphWindow;
                }
            }
            return default;
           
        }
        public static ConfigGraph GetSingleOpenEditorConfigGraph(string name)
        {
            try
            {
                var configGraphWindows = ConfigGraphWindow.CacheOpenedWindows;
                foreach (var configGraphWindow in configGraphWindows)
                {
                    ConfigGraph configGraph = configGraphWindow.GetGraph() as ConfigGraph;
                    if (configGraph != default && configGraph.FileName == name)
                    {
                        return configGraph;
                    }
                }
                return default;
            }
            catch (Exception ex)
            {
                Log.Fatal($"GetSingleOpenEditorConfigGraph failed, {ex}");
                return default;
            }
        }

        public static BaseNode GetSingleOpenEditorConfigNode(string typeName , int id)
        {
            var configGraphWindows = ConfigGraphWindow.CacheOpenedWindows;
            foreach (var configGraphWindow in configGraphWindows)
            {
                ConfigGraph configGraph = configGraphWindow.GetGraph() as ConfigGraph;
                foreach (var node in configGraph.nodes)
                {
                    if (node is IConfigBaseNode configBaseNode && configBaseNode.GetConfigName() == typeName && configBaseNode.GetID() == id && !(node is RefConfigBaseNode))
                    {
                        return node;
                    }
                }
            }

            return default;
        }

        public static object GetSingleOpenEditorConfigNodeData(string typeName, int id)
        {
            return (GetSingleOpenEditorConfigNode(typeName, id) as IConfigBaseNode)?.GetConfig();
        }

        public static T GetSingleOpenEditorConfigNode<T>(int id)
        {
            var configGraphWindows = ConfigGraphWindow.CacheOpenedWindows;
            foreach (var configGraphWindow in configGraphWindows)
            {
                ConfigGraph configGraph = configGraphWindow.GetGraph() as ConfigGraph;
                foreach (var node in configGraph.nodes)
                {
                    if(node is IConfigBaseNode configBaseNode && configBaseNode.GetConfig() is T config && configBaseNode.GetID() == id && !(node is RefConfigBaseNode))
                    {
                        return config;
                    }
                }
            }

            return default;
        }

        public static BaseNode GetAndOpenEditorConfigNode(string typeName, int id)
        {
            string graphName = string.Empty;
            //可以做个优化，把Id和存储位置进行缓存
            GraphHelper.ProcessEditor((manager) =>
            {
                var graghFiles = Directory.GetFiles(manager.PathSavesJsons, "*.json", SearchOption.AllDirectories);
                for (int i = 0, length = graghFiles.Length; i < length; i++)
                {
                    var graghFile = graghFiles[i];
                    var fileName = Path.GetFileName(graghFile);

                    if (string.IsNullOrEmpty(graghFile))
                    {
                        continue;
                    }
                    var fileInfo = new FileInfo(graghFile);
                    if (fileInfo == null || !fileInfo.Exists)
                    {
                        continue;
                    }

                    string info = File.ReadAllText(fileInfo.FullName);

                    if (info.Contains($"\"{typeName}_{id}\""))
                    {
                        graphName = fileInfo.FullName;
                        break;
                    }
                }
            });

            if (!string.IsNullOrEmpty(graphName))
            {
                var win = GraphAssetCallbacks.OpenGraphWindow(graphName);
                if (win is ConfigGraphWindow configWin)
                {
                    foreach (var node in configWin.configGraph.nodes)
                    {
                        if (node is IConfigBaseNode configBaseNode && configBaseNode.GetConfigName() == typeName && configBaseNode.GetID() == id && !(node is RefConfigBaseNode))
                        {
                            return node;
                        }
                    }
                }
                
            }

            return default;
        }

        // TODO 优化获取
        public static BaseNode GetSingleCacheEditorConfigNode(string typeName,int id)
        {
            ConfigGraph configGraph = default;
            //可以做个优化，把Id和存储位置进行缓存
            GraphHelper.ProcessEditor((manager) =>
            {
                var graghFiles = Directory.GetFiles(manager.PathSavesJsons, "*.json", SearchOption.AllDirectories);
                for (int i = 0, length = graghFiles.Length; i < length; i++)
                {
                    var graghFile = graghFiles[i];
                    var fileName = Path.GetFileName(graghFile);

                    if (string.IsNullOrEmpty(graghFile))
                    {
                        continue;
                    }
                    var fileInfo = new FileInfo(graghFile);
                    if (fileInfo == null || !fileInfo.Exists)
                    {
                        continue;
                    }

                    string info = File.ReadAllText(fileInfo.FullName);

                    if (info.Contains($"\"{typeName}_{id}\""))
                    {
                        configGraph = GraphHelper.LoadGraph(fileInfo.FullName) as ConfigGraph;
                        break;
                    }
                }
            });
            
            if (configGraph != default)
            {
                foreach (var node in configGraph.nodes)
                {
                    if (node is IConfigBaseNode configBaseNode && configBaseNode.GetConfigName() == typeName && configBaseNode.GetID() == id && !(node is RefConfigBaseNode))
                    {
                        return node;
                    }
                }
            }

            return default;
        }

        public static object GetSingleCacheEditorConfigNodeData(string typeName, int id)
        {
            return (GetSingleCacheEditorConfigNode(typeName,id) as IConfigBaseNode)?.GetConfig();
        }

        public static T GetSingleCacheEditorConfigNodeData<T>(int id)
        {
            string typeName = typeof(T).Name;
            return (T)GetSingleCacheEditorConfigNodeData(typeName, id);
        }

    }

    public static class ConfigDataExp
    {


    }
}

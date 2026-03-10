using GraphProcessor;
using NodeEditor.AIEditor;
using NodeEditor.GamePlayEditor;
using NodeEditor.MapAnimEditor;
using NodeEditor.NpcEventEditor;
using NodeEditor.SkillEditor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public static class GraphHelper
    {
        public delegate void GraphProcessCallback(IConfigEditorManager manager, BaseGraph graph, int i, int length);
        public delegate void GraphFileProcessCallback(IConfigEditorManager manager, string graphPath, int i, int length);
        public static IEnumerable<IConfigEditorManager> GetEditorManagers()
        {
            yield return SkillEditorManager.Inst;
            yield return AIEditorManager.Inst;
            yield return GamePlayEditorManager.Inst;
            yield return NpcEventEditorManager.Inst;
            yield return MapAnimEditorManager.Inst;
        }

        public static string FindGraphFile(string path)
        {
            string newFilePath = default;
            //如果仅仅只是重命名， 那么文件夹路径不变，可以封装成函数
            int index = path.LastIndexOf("/");
            string direPath = path.Substring(0, index);

            if (Directory.Exists(direPath))
            {
                DirectoryInfo dir = new DirectoryInfo(direPath);
                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string filePath = file.FullName;//拿到了文件的完整路径
                    filePath = filePath.Substring(filePath.IndexOf("Assets"));
                    filePath = filePath.Replace('\\', '/');
                    if (!filePath.EndsWith(".json", StringComparison.Ordinal) && !filePath.EndsWith(".json", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string content = File.ReadAllText(filePath);

                    if (content.Contains(path) && content.Contains("SerializationNodes") && content.Contains("PrefabModificationsReferencedUnityObjects"))
                    {
                        newFilePath = filePath;
                        return newFilePath;
                    }
                }
            }

            //文件可能移动了，那就全局找
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

                    graghFile = graghFile.Replace('\\', '/');
                    if (!graghFile.EndsWith(".json", StringComparison.Ordinal) && !graghFile.EndsWith(".json", StringComparison.Ordinal))
                    {
                        continue;
                    }

                    string content = File.ReadAllText(graghFile);

                    if (content.Contains(path) && content.Contains("SerializationNodes") && content.Contains("PrefabModificationsReferencedUnityObjects"))
                    {
                        newFilePath = graghFile;
                        break;
                    }
                }
            });

            return newFilePath;
        }

        public static IConfigEditorManager FindEditorManager(Func<IConfigEditorManager, bool> condition)
        {
            if (condition == null) return null;
            foreach (var manager in GetEditorManagers())
            {
                if (condition?.Invoke(manager) == true)
                {
                    return manager;
                }
            }
            return null;
        }

        public static void ProcessEditor(Action<IConfigEditorManager> action)
        {
            foreach (var manager in GetEditorManagers())
            {
                Utils.SafeCall(() =>
                {
                    action?.Invoke(manager);
                });
            }
        }
        public static string CreateGraph(Type graphType, string panelPath = null, string panelFileName = null)
        {
            try
            {
                if (panelPath == null)
                {
                    panelPath = GetPathSaves(graphType);
                }
                if (panelFileName == null)
                {
                    panelFileName = graphType.Name;
                }
                Directory.CreateDirectory(panelPath);
                var path = EditorUtility.SaveFilePanelInProject($"创建{graphType.Name}", panelFileName, "json", "", panelPath);
                if (string.IsNullOrEmpty(path))
                {
                    Log.Debug("创建graph已取消");
                    return null;
                }
                var graph = SerializedScriptableObject.CreateInstance(graphType);
                var jsonContent = JsonUtility.ToJson(graph, true);
                SerializedScriptableObject.DestroyImmediate(graph);
                graph = null;
                File.WriteAllText(path, jsonContent);
                AssetDatabase.Refresh();
                return path;
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
                return string.Empty;
            }
            //finally
            //{
            //    GUIUtility.ExitGUI();
            //}
        }
        public static string CreateGraph<T>(string panelPath = null, string panelFileName = null) where T : ConfigGraph
        {
            return CreateGraph(typeof(T), panelPath, panelFileName);
        }
        public static string GetPathSaves(Type graphType)
        {
            foreach (var manager in GetEditorManagers())
            {
                if (manager.GraphType == graphType)
                {
                    return manager.PathSavesJsons;
                }
            }
            return string.Empty;
        }
        public static string GetPathSaves<T>() where T : BaseGraph
        {
            return GetPathSaves(typeof(T));
        }
        public static bool IsValidSavePath(string path)
        {
            path = Utils.PathFormat(path);
            foreach (var manager in GetEditorManagers())
            {
                if (path.Contains(manager.PathSaves))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 依据路径解析，处理graph数据，处理完清理graph对象
        /// </summary>
        public static void ProcessGraph(string path, Action<BaseGraph> action)
        {
            var graph = LoadGraph(path);
            if (graph)
            {
                action?.Invoke(graph);
                BaseGraph.DestroyImmediate(graph);
                graph = null;
            }
        }

        /// <summary>
        /// 依据graphType级路径，处理graph数据，处理完清理graph对象
        /// </summary>
        public static void ProcessGraph(Type graphType, string path, Action<BaseGraph> action)
        {
            var graph = LoadGraph(graphType, path);
            if (graph)
            {
                action?.Invoke(graph);
                BaseGraph.DestroyImmediate(graph);
                graph = null;
            }
        }
        /// <summary>
        /// 加载编辑器所有graph并处理
        /// </summary>
        /// <typeparam name="T">编辑器graph类型</typeparam>
        /// <param name="actionLoaded">加载完成回调</param>
        public static void ProcessGraphs<T>(GraphProcessCallback actionLoaded, Func<bool> cancel = null) where T : BaseGraph
        {
            ProcessGraphs(typeof(T), actionLoaded, cancel);
        }
        public static void ProcessGraphs(Type graphType, GraphProcessCallback actionLoaded, Func<bool> cancel = null)
        {
            if (actionLoaded == null) return;

            IConfigEditorManager manager = null;
            foreach (var m in GetEditorManagers())
            {
                if (m.GraphType == graphType)
                {
                    manager = m;
                    break;
                }
            }
            if (string.IsNullOrEmpty(manager?.PathSavesJsons)) return;

            var graphFiles = Directory.GetFiles(manager.PathSavesJsons, $"*.json", SearchOption.AllDirectories);
            for (int i = 0, length = graphFiles.Length; i < length; i++)
            {
                var graphFile = graphFiles[i];
                ProcessGraph(graphType, graphFile, (graph)=>
                {
                    Utils.SafeCall(() =>
                    {
                        actionLoaded.Invoke(manager, graph, i, length);
                    });
                });
                if (cancel?.Invoke() == true) return;
            }
        }
        public static void ProcessAllGraphs(GraphProcessCallback graphProcessCallback, Func<bool> cancel = null)
        {
            ProcessEditor((manager) =>
            {
                ProcessGraphs(manager.GraphType, (manager, graph, i, length) =>
                {
                    graphProcessCallback.Invoke(manager, graph, i, length);
                }, cancel);
            });
        }
        public static void ProcessAllGraphFiles(GraphFileProcessCallback graphFileProcessCallback, Func<bool> cancel = null)
        {
            ProcessEditor((manager) =>
            {
                if (cancel?.Invoke() == true) return;
                var pathSaves = manager.PathSaves;
                var graphFiles = Directory.GetFiles(pathSaves, "*.json", SearchOption.AllDirectories);
                for (int i = 0, length = graphFiles.Length; i < length; i++)
                {
                    var graphFile = graphFiles[i];
                    graphFileProcessCallback.Invoke(manager, graphFile, i, length);
                    if (cancel?.Invoke() == true) break;
                }
            });
        }
        public static List<string> GetGraphFiles(Type graphType = null)
        {
            var result = new List<string>();
            if (graphType != null)
            {
                var pathSaves = GetPathSaves(graphType);
                var graphFiles = Directory.GetFiles(pathSaves, "*.json", SearchOption.AllDirectories);
                foreach (var graphFile in graphFiles)
                {
                    result.Add(Utils.PathFormat(graphFile));
                }
            }
            else
            {
                // 获取所有编辑器文件
                ProcessEditor((manager) =>
                {
                    var pathSaves = manager.PathSaves;
                    var graphFiles = Directory.GetFiles(pathSaves, "*.json", SearchOption.AllDirectories);
                    foreach (var graphFile in graphFiles)
                    {
                        result.Add(Utils.PathFormat(graphFile));
                    }
                });
            }
            return result;
        }
        public static BaseGraph LoadGraph(string path)
        {
            var graphType = GetGraphType(path);
            return LoadGraph(graphType, path);
        }

        /// <summary>
        /// 加载graph
        /// </summary>
        public static T LoadGraph<T>(string path) where T : BaseGraph
        {
            return LoadGraph(typeof(T), path) as T;
        }

        private static string loadingGraphPath;

        /// <summary>
        /// 加载graph
        /// </summary>
        public static BaseGraph LoadGraph(Type graphType, string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return null;

                var pathNew = Utils.PathFull2Assets(path);
                if (loadingGraphPath == pathNew)
                {
                    Log.Fatal($"存在循环加载graph情况，请检查：{pathNew}");
                    return null;
                }
                var fileName = Path.GetFileName(path);

                // 确保表格加载
                DesignTable.Load();

                BaseGraph graph = null;
                Utils.WatchTime($"{fileName} 加载Graph", () =>
                {
                    loadingGraphPath = null;
                    if (pathNew.EndsWith(".json", StringComparison.Ordinal))
                    {
                        graph = SerializedScriptableObject.CreateInstance(graphType) as BaseGraph;
                        try
                        {
                            graph.Disable();
                            var jsonContent = Utils.ReadAllText(pathNew);
                            loadingGraphPath = pathNew;
                            JsonUtility.FromJsonOverwrite(jsonContent, graph);
                            loadingGraphPath = null;
                        }
                        catch (System.Exception ex)
                        {
                            if (graph)
                            {
                                ScriptableObject.DestroyImmediate(graph);
                                graph = null;
                            }
                            loadingGraphPath = null;
                            Log.Error($"GraphHelper.LoadGraph exception : {pathNew}\n{ex}");
                        }
                    }
                    else if (pathNew.EndsWith(".asset"))
                    {
                        graph = AssetDatabase.LoadAssetAtPath(pathNew, graphType) as BaseGraph;
                    }

                    if (!graph)
                    {
                        Log.Error($"GraphHelper.LoadGraph failed, path error : {path}, {pathNew}");
                    }
                    else
                    {
                        // 避免切换Play mode，graph被销毁
                        // TODO HideAndDontSave为了运行时不被销毁，不过会影响回退问题
                        graph.hideFlags = HideFlags.HideAndDontSave;/* HideFlags.HideInHierarchy;*/
                        graph.path = pathNew;
                        graph.name = Path.GetFileNameWithoutExtension(pathNew);
                        graph.Enable();
                        // TODO 暂时不需要
                        graph.Load();
                    }
                });
                return graph;
            }
            catch (Exception ex)
            {
                Log.Error($"GraphHelper.LoadGraph failed : {path}\n{ex}");
                return null;
            }
        }

        public static bool ReSaveGraph(string path)
        {
            var result = false;
            ProcessGraph(path, (graph) =>
            {
                if (graph != null)
                {
                    result = graph.SaveGraphToDisk();
                }
            });
            return result;
        }

        /// <summary>
        /// 依据路径文件名，获取对应graph类型
        /// </summary>
        public static Type GetGraphType(string path)
        {
            try
            {
                var fileName = Path.GetFileName(path);
                foreach (var manager in GetEditorManagers())
                {
                    if (fileName.StartsWith(manager.GraphType.Name))
                    {
                        return manager.GraphType;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            return null;
        }

        public static bool IsValidGraphPath(string path)
        {
            return GetGraphType(path) != null;
        }

        /// <summary>
        /// 拷贝的视图
        /// </summary>
        /// <param name="createGraphView"></param>
        /// <param name="copyGraph"></param>
        /// <param name="serializedData"></param>
        public static void CopyGraph(BaseGraphView createGraphView, BaseGraph copyGraph, Vector2 firstNodePos, PortView connectPort)
        {
            //转到对应数据
            var data = new CopyPasteHelper();

            var sortNode = copyGraph.nodes.OrderBy(n => n.computeOrder);
            //node
            foreach (var baseNode in sortNode)
            {
                baseNode.visible = true;
                baseNode.hideChildNodes = false;
                baseNode.hideCounter = 0;

                data.copiedNodes.Add(JsonSerializer.SerializeNode(baseNode));

                foreach (var port in baseNode.GetAllPorts())
                {
                    if (port.portData.vertical)
                    {
                        foreach (var edge in port.GetEdges())
                        {
                            edge.isVisible = true;
                            data.copiedEdges.Add(JsonSerializer.Serialize(edge));
                        }
                    }
                }
            }

            //group
            foreach (var group in copyGraph.groups)
            {
                data.copiedGroups.Add(JsonSerializer.Serialize(group));
            }

            //Edge
            foreach (var serializedEdge in copyGraph.edges)
            {
                serializedEdge.isVisible = true;
                data.copiedEdges.Add(JsonSerializer.Serialize(serializedEdge));
            }

            //stack
            foreach (var stackNode in copyGraph.stackNodes)
            {
                data.copiedStack.Add(JsonSerializer.Serialize(stackNode));
            }

            //反序列化Group
            var unserializedGroups = data.copiedGroups.Select(g => JsonSerializer.Deserialize<Group>(g)).ToList();

            //记录原GUID对应的Node
            var copiedNodesMap = new Dictionary<string, BaseNode>();

            //node
            var nodePosOffset = Vector2.zero;
            BaseNodeView firstNodeView = default;
            foreach (var serializedNode in data.copiedNodes)
            {
                var node = JsonSerializer.DeserializeNode(serializedNode);
                if (node == null)
                    continue;

                if(node.computeOrder == 0 && firstNodePos != Vector2.zero)
                {
                    nodePosOffset = firstNodePos - node.position.position;
                }

                string sourceGUID = node.GUID;

                //Call OnNodeCreated on the new fresh copied node
                node.createdFromDuplication = true;
                node.createdWithinGroup = unserializedGroups.Any(g => g.innerNodeGUIDs.Contains(sourceGUID));
                node.OnNodeCreated();
                node.position.position += nodePosOffset;
                node.OnNodePostionUpdated();

                var newNodeView = createGraphView.AddNode(node);
                if(firstNodeView == default)
                {
                    firstNodeView = newNodeView;
                }

                copiedNodesMap[sourceGUID] = node;
            }

            // 记录原GUID对应的StickyNote
            var copiedStickyNotesMap = new Dictionary<string, StickyNote>();
            // stickynote
            foreach (var serializedStickyNote in data.copiedStickyNotes)
            {
                var stickyNote = JsonSerializer.Deserialize<StickyNote>(serializedStickyNote);
                if (stickyNote == null)
                    continue;
                // 记录原始GUID
                copiedStickyNotesMap[stickyNote.GUID] = stickyNote;

                stickyNote.OnCreated();
                stickyNote.position.position += nodePosOffset;
                var stickyNoteView = createGraphView.AddStickyNote(stickyNote);
            }

            //group
            foreach (var group in unserializedGroups)
            {
                //Same than for node
                group.OnCreated();

                var oldGUIDList = group.innerNodeGUIDs.ToList();
                group.innerNodeGUIDs.Clear();
                foreach (var guid in oldGUIDList)
                {
                    if (copiedNodesMap.TryGetValue(guid, out var node))
                    {
                        group.innerNodeGUIDs.Add(node.GUID);
                    }
                }

                // copy stick note
                oldGUIDList = group.innerStickyNoteGUIDs.ToList();
                group.innerStickyNoteGUIDs.Clear();
                foreach (var guid in oldGUIDList)
                {
                    if (copiedStickyNotesMap.TryGetValue(guid, out var stickyNote))
                    {
                        group.innerStickyNoteGUIDs.Add(stickyNote.GUID);
                    }
                }

                group.position.position += nodePosOffset;
                createGraphView.AddGroup(group);
            }

            //edge
            foreach (var serializedEdge in data.copiedEdges)
            {
                var edge = JsonSerializer.Deserialize<SerializableEdge>(serializedEdge);
                edge.Deserialize(copyGraph);

                // Find port of new nodes:
                copiedNodesMap.TryGetValue(edge.inputNode.GUID, out var oldInputNode);
                copiedNodesMap.TryGetValue(edge.outputNode.GUID, out var oldOutputNode);

                // both new node
                bool valid = oldInputNode != null && oldOutputNode != null;
                // We avoid to break the graph by replacing unique connections:
                if (!valid && (oldInputNode == null && !edge.inputPort.portData.acceptMultipleEdges || !edge.outputPort.portData.acceptMultipleEdges))
                    continue;

                oldInputNode = oldInputNode ?? edge.inputNode;
                oldOutputNode = oldOutputNode ?? edge.outputNode;

                var inputPort = oldInputNode.GetPort(edge.inputPort.fieldName, edge.inputPortIdentifier);
                var outputPort = oldOutputNode.GetPort(edge.outputPort.fieldName, edge.outputPortIdentifier);

                var newEdge = SerializableEdge.CreateNewEdge(createGraphView.graph, inputPort, outputPort);

                if (createGraphView.nodeViewsPerNode.ContainsKey(oldInputNode) && createGraphView.nodeViewsPerNode.ContainsKey(oldOutputNode))
                {
                    var edgeView = createGraphView.CreateEdgeView();
                    edgeView.userData = newEdge;
                    edgeView.input = createGraphView.nodeViewsPerNode[oldInputNode].GetPortViewFromFieldName(newEdge.inputFieldName, newEdge.inputPortIdentifier);
                    edgeView.output = createGraphView.nodeViewsPerNode[oldOutputNode].GetPortViewFromFieldName(newEdge.outputFieldName, newEdge.outputPortIdentifier);

                    createGraphView.Connect(edgeView);
                }
            }

            //stack
            for(int i = 0; i < data.copiedStack.Count; i++)
            {
                var serializedStack = data.copiedStack[i];
                var stackNode = JsonSerializer.DeserializeStack(serializedStack);
                if (stackNode == null)
                    continue;

                var oldGUIDList = stackNode.nodeGUIDs.ToList();
                stackNode.nodeGUIDs.Clear();
                foreach (var guid in oldGUIDList)
                {
                    if (copiedNodesMap.TryGetValue(guid, out var node))
                    {
                        stackNode.nodeGUIDs.Add(node.GUID);
                    }
                }

                stackNode.position += nodePosOffset;
                createGraphView.AddStackNode(stackNode);
            }

            //主动连接第一个节点
            if (connectPort != default && firstNodeView != default)
            {
                var firstNodePort = firstNodeView.GetPortViewFromFieldName("ID", "0");
                if(firstNodePort != null)
                {
                    createGraphView.Connect(firstNodePort, connectPort);
                }
            }
        }

        #region 模板相关
        public static Type TryGetTemplateNodeType(BaseGraph graph, string pathOrName)
        {
            if (!graph || !JsonGraphManager.Inst.IsTemplate(pathOrName))
            {
                return null;
            }
            if (graph is SkillGraph ||
                graph is AIGraph ||
                graph is GamePlayGraph)
            {
                if (pathOrName.Contains("筛选"))
                {
                    return typeof(TSKILLSELECT_CUSTOM);
                }
                else if (pathOrName.Contains("条件"))
                {
                    return typeof(TSCT_GET_TEMPLATE_COND_RESULT);
                }
                else
                {
                    return typeof(TSET_RUN_SKILL_EFFECT_TEMPLATE);
                }
            }
            else if (graph is NpcEventGraph)
            {
                // TODO
            }
            return null;
        }
        /// <summary>
        /// 获取模板路径列表
        /// </summary>
        public static Dictionary<string, string> GetTemplatePaths(Type graphType)
        {
            var result = new Dictionary<string, string>();
            var pathSaves = GetPathSaves(graphType);
            if (!string.IsNullOrEmpty(pathSaves))
            {
                var jsonPaths = Directory.GetFiles(pathSaves, $"*.json", SearchOption.AllDirectories);
                for (int i = 0, length = jsonPaths.Length; i < length; i++)
                {
                    var jsonPath = jsonPaths[i];
                    Utils.PathFormat(ref jsonPath);
                    //if (jsonPath.Contains("模板"))
                    if (JsonGraphManager.Inst.IsTemplate(jsonPath))
                    {
                        var showPath = $"{jsonPath.Substring(pathSaves.Length + 1)}";
                        result[jsonPath] = showPath;
                    }
                }
                if (graphType == typeof(AIGraph) || graphType == typeof(GamePlayGraph))
                {
                    var resultSkill = GetTemplatePaths(typeof(SkillGraph));
                    foreach (var kv in resultSkill)
                    {
                        result[kv.Key] = $"{SkillEditorManager.Inst.Name}/{kv.Value}";
                    }
                }
            }
            return result;
        }

        #endregion

        #region 辅助接口
        // 获取graph简介命名，如:SkillGraph_11111_测试.json->测试
        // TODO 规范graph命名
        public static string GetSimpleGraphName(string graphPath)
        {
            try
            {
                if (string.IsNullOrEmpty(graphPath))
                {
                    return "空";
                }
                var fileName = Path.GetFileNameWithoutExtension(graphPath);
                var fileSimpleName = Utils.PathFull2SeparationFolder(fileName, "Graph_");
                return fileSimpleName;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return "异常";
            }
        }
        #endregion
    }
}
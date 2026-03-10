using UnityEngine;
using UnityEditor;
using GraphProcessor;
using UnityEditor.Callbacks;
using NodeEditor.SkillEditor;
using NodeEditor.AIEditor;
using NodeEditor.GamePlayEditor;
using NodeEditor.NpcEventEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using NodeEditor.MapAnimEditor;

namespace NodeEditor
{
    public class GraphAssetCallbacks
    {
        public const string GRAPH_ASSET_HEAD = "Assets/FS/节点编辑器-";
        public const int GRAPH_PRIORITY = 200;

        static List<string> validFiles = new List<string>();
        static StringBuilder sb = new StringBuilder();

        [MenuItem(GRAPH_ASSET_HEAD + "导出-选择内容", validate = true)]
        [MenuItem(GRAPH_ASSET_HEAD + "保存-选择内容", validate = true)]
        static bool ExoprtBySelectionValidation()
        {
            validFiles.Clear();
            var length = Selection.objects.Length;
            if (length == 0)
            {
                return false;
            }
            foreach (var selectionObject in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(selectionObject);
                path = Utils.PathFormat(path);
                if (!path.Contains(Constants.PathPrefix_NodeEditor))
                {
                    continue;
                }
                if (Directory.Exists(path))
                {
                    // 如果是文件夹，则遍历文件夹
                    var dirFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
                    foreach (var dirFile in dirFiles)
                    {
                        if (GraphHelper.IsValidGraphPath(dirFile))
                        {
                            validFiles.Add(dirFile);
                        }
                    }
                }
                else if (GraphHelper.IsValidGraphPath(path))
                {
                    validFiles.Add(path);
                }
            }
            if (validFiles.Count == 0)
            {
                return false;
            }
            return true;
        }

        [MenuItem(GRAPH_ASSET_HEAD + "导出-选择内容", priority = GRAPH_PRIORITY)]
        static void ExoprtBySelection()
        {
            var validFilesCount = validFiles.Count;
            if (validFilesCount == 0)
            {
                return;
            }
            sb.Clear();
            var maxCount = 10;
            sb.AppendLine($"导出文件总数: {validFilesCount}\n");
            for (int i = 0, length = validFiles.Count; i < length && i < maxCount; i++)
            {
                var validFile = validFiles[i];
                var fileName = Path.GetFileNameWithoutExtension(validFile);
                sb.AppendLine($"  {fileName}");
            }
            if (validFilesCount > maxCount)
            {
                sb.AppendLine("\n导出文件较多，是否导出？");
            }
            if (EditorUtility.DisplayDialog("导出文件确认", sb.ToString(), "导出", "再想想"))
            {
                // 优化导出效率，换个接口
                //GraphHelper.ExportGraph2Excel(validFiles);
                Utils.DisplayProcess($"导出文件，共{validFiles.Count}个文件", (sbInfo) =>
                {
                    sbInfo.AppendLine($"共处理文件: {validFiles.Count} 个");
                    ExternalExportUtil.ExportNodeJson2Excel(validFiles);
                });
            }
        }

        [MenuItem(GRAPH_ASSET_HEAD + "保存-选择内容", priority = GRAPH_PRIORITY)]
        static void ReSaveBySelection()
        {
            var validFilesCount = validFiles.Count;
            if (validFilesCount == 0)
            {
                return;
            }
            sb.Clear();
            var maxCount = 10;
            sb.AppendLine($"保存文件总数: {validFilesCount}\n");
            for (int i = 0, length = validFiles.Count; i < length && i < maxCount; i++)
            {
                var validFile = validFiles[i];
                var fileName = Path.GetFileNameWithoutExtension(validFile);
                sb.AppendLine($"  {fileName}");
            }
            if (validFilesCount > maxCount)
            {
                sb.AppendLine("\n保存文件较多，是否保存？");
            }
            if (EditorUtility.DisplayDialog("保存文件确认", sb.ToString(), "重新保存", "再想想"))
            {
                // 优化导出效率，换个接口
                //GraphHelper.ExportGraph2Excel(validFiles);
                Utils.DisplayProcess($"保存文件，共{validFiles.Count}个文件", (sbInfo) =>
                {
                    for (int i = 0, length = validFilesCount; i < length; i++)
                    {
                        var file = validFiles[i];
                        EditorUtility.DisplayProgressBar($"保存文件...[{i}/{validFilesCount}]", Path.GetFileName(file), (float)i / validFilesCount);
                        GraphHelper.ReSaveGraph(file);
                    }
                });
            }
        }

        //[MenuItem("Assets/Create/SkillGraph_Asset", false, 10)]
        public static void CreateGraphPorcessor_Skill_Asset()
        {
            var graph = ScriptableObject.CreateInstance<SkillGraph>();
            ProjectWindowUtil.CreateAsset(graph, "SkillGraph.asset");
        }

        private static bool IsValidPathSaves<T>() where T : BaseGraph
        {
            if (Selection.objects.Length != 1)
            {
                return false;
            }
            var graphPathSaves = GraphHelper.GetPathSaves<T>();
            if (string.IsNullOrEmpty(graphPathSaves))
            {
                return false;
            }
            var selectObject = Selection.objects[0];
            var selectPath = AssetDatabase.GetAssetPath(selectObject);
            selectPath = Utils.PathFormat(selectPath);
            return selectPath.Contains(graphPathSaves);
        }

        #region SkillGraph
        [MenuItem(GRAPH_ASSET_HEAD + "创建-SkillGraph", true, GRAPH_PRIORITY)]
        public static bool CreateGraphPorcessor_SkillGraph_Validation()
        {
            return IsValidPathSaves<SkillGraph>();
        }

        [MenuItem(GRAPH_ASSET_HEAD + "创建-SkillGraph", priority = GRAPH_PRIORITY)]
        public static void CreateGraphPorcessor_SkillGraph()
        {
            var graph = ScriptableObject.CreateInstance<SkillGraph>();
            var jsonContent = JsonUtility.ToJson(graph);
            ScriptableObject.DestroyImmediate(graph);
            ProjectWindowUtil.CreateAssetWithContent("SkillGraph.json", jsonContent);
        }
        #endregion SkillGraph

        #region AIGraph
        [MenuItem(GRAPH_ASSET_HEAD + "创建-AIGraph", true, GRAPH_PRIORITY)]
        public static bool CreateGraphPorcessor_AI_Validation()
        {
            return IsValidPathSaves<AIGraph>();
        }

        [MenuItem(GRAPH_ASSET_HEAD + "创建-AIGraph", priority = GRAPH_PRIORITY)]
        public static void CreateGraphPorcessor_AI()
        {
            var graph = ScriptableObject.CreateInstance<AIGraph>();
            var jsonContent = JsonUtility.ToJson(graph);
            ScriptableObject.DestroyImmediate(graph);
            ProjectWindowUtil.CreateAssetWithContent("AIGraph.json", jsonContent);
        }
        #endregion AIGraph

        #region GamePlayGraph
        [MenuItem(GRAPH_ASSET_HEAD + "创建-GamePlayGraph", true, GRAPH_PRIORITY)]
        public static bool CreateGraphPorcessor_GamePlayGraph_Validation()
        {
            return IsValidPathSaves<GamePlayGraph>();
        }

        [MenuItem(GRAPH_ASSET_HEAD + "创建-GamePlayGraph", priority = GRAPH_PRIORITY)]
        public static void CreateGraphPorcessor_GamePlayGraph()
        {
            var graph = ScriptableObject.CreateInstance<GamePlayGraph>();
            var jsonContent = JsonUtility.ToJson(graph);
            ScriptableObject.DestroyImmediate(graph);
            ProjectWindowUtil.CreateAssetWithContent("GamePlayGraph.json", jsonContent);
        }
        #endregion GamePlayGraph

        #region NpcEventGraph
        [MenuItem(GRAPH_ASSET_HEAD + "创建-NpcEventGraph", true, GRAPH_PRIORITY)]
        public static bool CreateGraphPorcessor_NpcEventGraph_Validation()
        {
            return IsValidPathSaves<NpcEventGraph>();
        }

        [MenuItem(GRAPH_ASSET_HEAD + "创建-NpcEventGraph", priority = GRAPH_PRIORITY)]
        public static void CreateGraphPorcessor_NpcEventGraph()
        {
            var graph = ScriptableObject.CreateInstance<NpcEventGraph>();
            var jsonContent = JsonUtility.ToJson(graph);
            ScriptableObject.DestroyImmediate(graph);
            ProjectWindowUtil.CreateAssetWithContent("NpcEventGraph.json", jsonContent);
        }
        #endregion NpcEventGraph

        //[OnOpenAsset(0)]
        [EditorMethodRegister(typeof(OnOpenAssetAttribute), "NodeGraph-技能编辑器相关")]
        public static bool OnBaseGraphOpened(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (!obj) return false;

            var path = AssetDatabase.GetAssetPath(obj);
            path = Utils.PathFormat(path);
            BaseGraphWindow win = null;
            if (obj is BaseGraph baseGraph)
            {
                win = InitializeGraph(baseGraph, path);
            }
            else if (obj is TextAsset textAsset)
            {
                // 如果是编辑器文件，检查下文件冲突
                if (path.EndsWith(".json") && GraphHelper.IsValidGraphPath(path))
                {
                    if (Utils.CheckSVNConflict(path))
                    {
                        // 直接跳过打开文件
                        return true;
                    }
                    else
                    {
                        win = OpenGraphWindow(path);
                    }
                }
            }
            return win != null;
        }

        public static BaseGraphWindow InitializeGraph(BaseGraph baseGraph, string path)
        {
            if (baseGraph == null || !File.Exists(path)) return null;

            BaseGraphWindow win = null;
            try
            {
                switch (baseGraph)
                {
                    case SkillGraph skillGraph:
                        win = ConfigGraphWindow.CreateWindow<SkillGraphWindow>(skillGraph, path);
                        break;
                    case AIGraph aiGraph:
                        win = ConfigGraphWindow.CreateWindow<AIGraphWindow>(aiGraph, path);
                        break;
                    case GamePlayGraph gamePlayGraph:
                        win = ConfigGraphWindow.CreateWindow<GamePlayGraphWindow>(gamePlayGraph, path);
                        break;
                    case NpcEventGraph npcEventGraph:
                        win = ConfigGraphWindow.CreateWindow<NpcEventGraphWindow>(npcEventGraph, path);
                        break;
                    case MapAnimGraph mapAnimGraph:
                        win = ConfigGraphWindow.CreateWindow<MapAnimGraphWindow>(mapAnimGraph, path);
                        break;
                    case NPBehaveGraph npBehaveGraph:
                        win = EditorWindow.GetWindow<NPBehaveGraphWindow>();
                        win?.InitializeGraph(npBehaveGraph);
                        break;
                    default:
                        //EditorWindow.GetWindow<FallbackGraphWindow>().InitializeGraph(baseGraph);
                        break;
                }
                return win;
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// 打开编辑器窗口
        /// </summary>
        /// <param name="filePath">json文件路径</param>
        /// <param name="loadFromFile">是否强制从文件重新打开窗口，默认focus不重载</param>
        /// <returns></returns>
        public static BaseGraphWindow OpenGraphWindow(string filePath, bool loadFromFile = false)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }
            var path = Utils.PathFull2Assets(filePath);
            if (path.EndsWith(".json") && !File.Exists(path) || !GraphHelper.IsValidGraphPath(path))
            {
                return null;
            }
            if (Utils.CheckSVNConflict(path))
            {
                return null;
            }
            var fileName = Path.GetFileName(path);
            var name = Path.GetFileNameWithoutExtension(path);

            // 检查下编辑器前置条件
            if (!NodeEditorManager.IsValid(out var errorMessage))
            {
                EditorUtility.DisplayDialog($"打开文件:{fileName}", $"编辑器存在异常，详情见Console界面\n{errorMessage}", "好的");
                return null;
            }
            BaseGraphWindow win = null;
            // 检查是否已经打开
            var winOpened = Utils.GetWindow<ConfigGraphWindow>((win) =>
            {
                return win.TitleName == name;
            });
            var openWinOld = winOpened != null && !loadFromFile;
            Utils.WatchTime($"{fileName} [{(openWinOld ? "旧" : "新")}]打开窗口-总耗时", () =>
            {
                if (openWinOld)
                {
                    winOpened.Focus();
                    win = winOpened;
                }
                else
                {
                    var graph = GraphHelper.LoadGraph(path);
                    win = InitializeGraph(graph, path);
                }
            });
            return win;
        }
    }
}

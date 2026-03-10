using GraphProcessor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;


namespace NodeEditor
{
    public partial class ConfigIDManager
    {
        private FileSystemWatcher jsonFileWatcher;

        static Queue<FileSystemEventArgs> curJsonChangeEventArgs = new Queue<FileSystemEventArgs>();
        public void InitJsonFileWatcher()
        {
            //if (isJsonWatcherInit) return;

            //curJsonChangeEventArgs.Clear();
            //jsonFileWatcher = new FileSystemWatcher(Constants.NodeEditorPath, "*.json");
            //jsonFileWatcher.IncludeSubdirectories = true;
            //jsonFileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size;
            //jsonFileWatcher.Renamed += OnJsonFileChanged;
            //jsonFileWatcher.Created += OnJsonFileChanged;
            //jsonFileWatcher.Deleted += OnJsonFileChanged;
            //jsonFileWatcher.Changed += OnJsonFileChanged;
            //EditorApplication.update += OnJsonFileWatcher;
            //jsonFileWatcher.EnableRaisingEvents = true;
            //isJsonWatcherInit = true;
        } 

        private bool enableJsonWatcher = true;
        private bool isJsonWatcherInit = false;

        private static bool isWait2Init = false;
        public void OnJsonFileWatcher()
        {
            if (!enableJsonWatcher)
            {
                curJsonChangeEventArgs.Clear();
                return;
            }

            if (curJsonChangeEventArgs == null || curJsonChangeEventArgs.Count == 0)
            {
                return;
            }

            FileSystemEventArgs curJsonChangeEventArg = curJsonChangeEventArgs.Dequeue();
            if (curJsonChangeEventArg == null)
            {
                return;
            }
            var changeType = curJsonChangeEventArg.ChangeType;
            if (!isWait2Init || changeType != WatcherChangeTypes.Changed)
            {
                Log.Debug($"OnJsonFileWatcher StartEditorCoroutine, {curJsonChangeEventArg.ChangeType}-{curJsonChangeEventArg.FullPath}");
                isWait2Init = true;
                //实际流程处理全部回归到主线程
                EditorCoroutineRunner.StartEditorCoroutine(OnRefeshFile(curJsonChangeEventArg));
            }
            else
            {
                // TODO Dequeue->Peek？
                Log.Debug($"OnJsonFileWatcher error, {changeType}-{curJsonChangeEventArg.FullPath}");
            }
        }

        public static IEnumerator OnRefeshFile(FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                if (!isWait2Init)
                {
                    yield break;
                } 

                yield return new WaitForSeconds(0.1f);
            }
            //防止重命名出现先删除再创建
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                yield return new WaitForSeconds(0.25f);
            }

            try
            {
                // TODO ConfigIDManager内记录文件节点映射数据需要刷新

                var configGraphWindows = Utils.GetAllWindow<ConfigGraphWindow>();
                var length = configGraphWindows.Length;

                //窗口相关
                if (length > 0)
                {
                    for (int i = 0; i < length; i++)
                    {
                        var window = configGraphWindows[i];
                        var winGraphPath = window.configGraph?.path;
                        var fileGraphPath = Utils.PathFull2Assets(e.FullPath);

                        //badcode （已提单）该部分是监听编辑器Json文件的核心代码，这部分代码效率有点低
                        //最大的问题在于重命名事件监听不生效，目前没有找到原因
                        //目前重命名会走三个流程Change Delete Create，重命名只能通过Create事件取巧的来实现

                        //实际重命名
                        if (e.ChangeType == WatcherChangeTypes.Created && File.Exists(e.FullPath))
                        {
                            string content = File.ReadAllText(e.FullPath);

                            string oldFullPath = $"{Application.dataPath}{winGraphPath.Replace("Assets", string.Empty)}";

                            //首先操作模板
                            if (content.Contains("\"IsTemplate\": true,"))
                            {
                                string headRegex = "\"path\": \"";
                                string endRegex = "\",";
                                Regex config2IDRegex = new Regex($"{headRegex}(.+){endRegex}");
                                var oldTemplatePath = config2IDRegex.Match(content).Value.Replace(headRegex,string.Empty).Replace(endRegex,string.Empty);

                                //json文件内路径相同了就说明已经保存了
                                if(oldTemplatePath != fileGraphPath)
                                {
                                    List<BaseNode> nodes = oldTemplatePath != winGraphPath ? window.GetGraph().nodes : default;

                                    if (TemplateProvider.RefreshAllWindowTemplateNodes(oldTemplatePath, fileGraphPath, nodes))
                                        window.ShowNotification("刷新模板路径成功");

                                    window.OnRefreshPinnedView?.Invoke();
                                }
                            }

                            //防止创建文件和原文件相同导致的重命名
                            if (File.Exists(oldFullPath))
                            {
                                window.RefreshWindow();
                                continue;
                            }

                            if (!string.IsNullOrEmpty(content) && content.Contains(winGraphPath))
                            {
                                window.SetSavePath(fileGraphPath);
                                window.IgnoreFlagSave(false);
                            }
                        }
                        //实际删除
                        else if (e.ChangeType == WatcherChangeTypes.Deleted)
                        {
                            window.RefreshWindow();
                            if (winGraphPath != fileGraphPath)
                            {
                                inst.RemoveAllOldNodeId(window, fileGraphPath);
                                continue;
                            }

                            var nameShow = window.configGraph?.name ?? winGraphPath;

                            string tips = "文件在外部被移除，即将关闭Graph";
                            if (EditorUtility.DisplayDialog($"是否保存文件:{nameShow}", tips, "是","否"))
                            {
                                inst.RemoveAllOldNodeId(window);
                                window.OnRefreshPinnedView?.Invoke();
                                window.IgnoreFlagClose(false);
                            }
                            //Utils.RefeshAllWindow(window);
                        }
                        //实际刷新
                        else if (e.ChangeType == WatcherChangeTypes.Changed)
                        {
                            if (fileGraphPath == winGraphPath)
                            {
                                //如果是手动保存
                                if (window.SaveFlags.HasFlag(EditorFlag.Saving))
                                {
                                    window.SaveFlags ^= EditorFlag.Saving;
                                    continue;
                                }

                                //防止重名名前的Change事件导致文件丢失
                                if (!File.Exists(e.FullPath))
                                {
                                    continue;
                                }

                                //否则就是被动保存，需要重新加载Graph
                                bool isChange = window.configGraph.IsOutSideChange();
                                if (!isChange)
                                {
                                    continue;
                                }

                                var nameShow = window.configGraph?.name ?? winGraphPath;
                                string tips = "文件在外部被修改（例如回滚、其他人改动），即将重新加载(注意注意！！！！！如果不是外部修改请点否)";
                                if (EditorUtility.DisplayDialog($"是否保存文件:{nameShow}", tips, "是","否"))
                                {
                                    window.InitializeGraph(GraphHelper.LoadGraph(e.FullPath),false);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                var name = Path.GetFileNameWithoutExtension(e.FullPath);
                Log.Debug($"文件变动-{e.ChangeType}: {name}");
                isWait2Init = false;
                EditorUtility.ClearProgressBar();
            }
        }

        public void RemoveAllOldNodeId(ConfigGraphWindow window,string oldGraphPath = null)
        {
            var nodes = window.configGraph?.nodes;
            var graphPath = oldGraphPath != null ? oldGraphPath : window.configGraph.path;
            foreach (var node in nodes)
            {
                if (node is RefConfigBaseNode || node is RefTemplateBaseNode) continue;
                if (node is IConfigBaseNode configNode)
                {
                    var configName = configNode.GetConfigName();
                    var id = configNode.GetID();

                    inst.ReleaseConfigID(configName, id, graphPath);
                }
            }
        }

        private void OnJsonFileChanged(object sender, FileSystemEventArgs e)
       {
            //WWYTODO会不会出现无效路径的问题
            if (!e.FullPath.Contains("Saves\\Jsons") || !jsonFileWatcher.EnableRaisingEvents)
            {
                return;
            }

            curJsonChangeEventArgs.Enqueue(e);
        }

    }
}

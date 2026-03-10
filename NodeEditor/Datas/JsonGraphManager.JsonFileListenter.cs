using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace NodeEditor
{
    public partial class JsonGraphManager
    {
        private FileSystemWatcher jsonFileWatcher;

        private bool isAutoSyncGraphNames = false;

        //路径：（监听类型：监听参数）
        private Dictionary<string, SortedDictionary<int, AssetModificationArgs>> allWatcherPath2ArgDic = 
            new Dictionary<string, SortedDictionary<int, AssetModificationArgs>>();

        private void InitJsonListenter()
        {
            EditorAssetModificationProcessor.IsOpenModification = true;

            EditorEventManager.Inst.AddListenter<AssetModificationArgs>(EventConstDefine.EditorEventManager_OnJsonAssetModification, OnJsonAssetHandler);

            jsonFileWatcher = new FileSystemWatcher(Constants.NodeEditorPath, "*.json");
            jsonFileWatcher.BeginInit();
            jsonFileWatcher.NotifyFilter = NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName |
                NotifyFilters.Size;
            jsonFileWatcher.IncludeSubdirectories = true;
            jsonFileWatcher.Changed += OnJsonFileChangedListenting;
            jsonFileWatcher.Deleted += OnJsonFileChangedListenting;
            jsonFileWatcher.Created += OnJsonFileChangedListenting;
            jsonFileWatcher.EnableRaisingEvents = true;
            jsonFileWatcher.EndInit();

            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;

            ConfigGraphWindow.OnOpenWindow += OnOpenWindow;
        }

        private bool IsValidPath(string path)
        {
            if(path.Contains(Constants.NodeEditorPath)
                && path.Contains(Constants.SavePartPath)
                && (path.EndsWith(".Json") || path.EndsWith(".json")))
            {
                return true;
            }

            return false;
        }

        private void OnUpdate()
        {
            // 暂时不用打开
            //CacheData2FileAsync();

            if (allWatcherPath2ArgDic.Count > 0)
            {
                var paths = allWatcherPath2ArgDic.Keys.ToList();

                for (var i = paths.Count - 1; i >= 0; i--)
                {
                    var path = paths[i];
                    //多线程情况下可能丢失
                    if (string.IsNullOrEmpty(path) || !allWatcherPath2ArgDic.TryGetValue(path, out var argsDic))
                    {
                        continue;
                    }

                    var keys = argsDic.Keys.ToList();

                    for (int j = keys.Count - 1; j >= 0; j--)
                    {
                        var key = keys[j];
                        var value = argsDic[key];
                        OnJsonAssetHandler(value);
                    }

                    allWatcherPath2ArgDic.Remove(path);
                }
            }
        }

        private void OnJsonAssetHandler(AssetModificationArgs args)
        {
            if (!IsValidPath(args.newPath))
            {
                return;
            }

            args.newPath = Utils.PathFull2Assets(args.newPath);

            args.oldPath = string.IsNullOrEmpty(args.oldPath) ? string.Empty : Utils.PathFull2Assets(args.oldPath);

            ConfigGraphWindow curWindow = GetWindowByPath(args.newPath);

            switch (args.changeTypes)
            {
                case System.IO.WatcherChangeTypes.Created:
                    OnCreateJsonAsset(args.newPath, curWindow);
                    break;
                case System.IO.WatcherChangeTypes.Deleted:
                    OnDeleteJsonAsset(args.newPath, RemoveAssetOptions.DeleteAssets, curWindow);
                    break;
                case System.IO.WatcherChangeTypes.Changed:
                    OnJsonAssetChange(args.newPath, curWindow);
                    break;
                case System.IO.WatcherChangeTypes.Renamed:
                    OnWillRenameJsonAsset(args.oldPath, args.newPath);
                    break;
            }
        }


        private void OnJsonAssetChange(string path, ConfigGraphWindow window = null)
        {
            Log.Info($"NodeEditor刷新了资源！路径：{path}");

            if (window != null)
            {
                //如果是手动保存
                if (window.SaveFlags.HasFlag(EditorFlag.Saving))
                {
                    window.SaveFlags ^= EditorFlag.Saving;
                    return;
                }

                window.OnRefreshPinnedView?.Invoke();

                //防止重名名前的Change事件导致文件丢失
                if (!File.Exists(path))
                {
                    return;
                }

                if (window.configGraph == null)
                {
                    return;
                }

                //否则就是被动保存，需要重新加载Graph
                bool isChange = window.configGraph.IsOutSideChange();
                if (!isChange)
                {
                    return;
                }

                var nameShow = window.configGraph?.name ?? path;
                string tips = "文件在外部被修改（例如回滚、其他人改动），即将重新加载(注意注意！！！！！如果不是外部修改请点否)";
                if (EditorUtility.DisplayDialog($"是否保存文件:{nameShow}", tips, "是", "否"))
                {
                    //确认了才刷新
                    RefreshDataByJsonFile(path);
                    window.InitializeGraph(GraphHelper.LoadGraph(path), false);
                }
            }
        }

        private void OnWillRenameJsonAsset(string oldPath, string newPath, ConfigGraphWindow window = null)
        {
            Log.Info($"NodeEditor移动资源！从旧路径:{oldPath}到新路径:{newPath}");

            //刷新JsonGraphManager的数据，只是改了名称和路径，不需要刷新节点数据

            window = GetWindowByPath(oldPath);

            if (window != null)
            {
                window.SetSavePath(newPath);
                window.IgnoreFlagSave(false);
                window.OnRefreshPinnedView?.Invoke();
            }

            RenameGraphByJsonPath(oldPath, newPath);
        }

        private void OnCreateJsonAsset(string path, ConfigGraphWindow window = null)
        {
            Log.Info($"NodeEditor创建资源！路径：{path}");

            //刷新JsonGraphManager的数据
            this.CreateGraphByJsonPath(path);
        }

        private void OnDeleteJsonAsset(string path, RemoveAssetOptions options = RemoveAssetOptions.DeleteAssets, ConfigGraphWindow window = null)
        {
            Log.Info($"NodeEditor删除资源！路径：{path}");

            //刷新JsonGraphManager的数据

            this.DeleteGraphByJsonPath(path);

            if (window != null)
            {
                var nameShow = window.configGraph?.name ?? path;

                string tips = "文件在外部被移除，即将关闭Graph";
                if (EditorUtility.DisplayDialog($"是否保存文件:{nameShow}", tips, "是", "否"))
                {
                    ConfigIDManager.Inst.RemoveAllOldNodeId(window);
                    window.IgnoreFlagClose(false);
                }
            }
        }

        private ConfigGraphWindow GetWindowByPath(string path)
        {
            path = Utils.PathFull2Assets(path);

            //刷新窗口和ConfigIDManager
            return Utils.GetEnoughWindow<ConfigGraphWindow>((w) =>
            {
                if (w.graphPath == path)
                    return true;

                return false;
            });
        }

        private void OnJsonFileChangedListenting(object sender, FileSystemEventArgs e)
        {
            string path = e.FullPath.Replace("\\", "/");

            var args = new AssetModificationArgs
            {
                changeTypes = e.ChangeType,
                newPath = path,
            };

            if (!IsValidPath(args.newPath))
            {
                return;
            }

            if(!allWatcherPath2ArgDic.TryGetValue(path, out var argsDic) || argsDic == null)
            {
                argsDic = new SortedDictionary<int, AssetModificationArgs>();
                allWatcherPath2ArgDic.Add(path, argsDic);
            }

            argsDic[(int)args.changeTypes] = args;
        }

        private void OnOpenWindow(INodeEditorWindow w)
        {
            if (w is ConfigGraphWindow win)
            {
                // 变动做下检查，检查频繁些，避免错误发现不及时
                if (isDirty)
                {
                    CheckError();
                }
            }
        }
    }
}

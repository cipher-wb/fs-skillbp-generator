#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


namespace NodeEditor
{
    public struct AssetModificationArgs
    {
        public string oldPath;

        public string newPath;

        public WatcherChangeTypes changeTypes;
    }

    /// <summary>
    /// 监听资源事件，包括资源的创建 删除 移动 等
    /// 保存和监听改变频率较高，不监听
    /// </summary>
    public class EditorAssetModificationProcessor : AssetModificationProcessor
    {
        public static bool IsOpenModification { get; set; } = false;

        /// <summary>
        /// 监听资源移动和重命名事件
        /// </summary>
        /// <param name="oldPath">旧路径</param>
        /// <param name="newPath">新路径</param>
        /// <returns></returns>
        public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        {
            if (!IsOpenModification)
            {
                return AssetMoveResult.DidNotMove;
            }

            var args = new AssetModificationArgs
            {
                changeTypes = WatcherChangeTypes.Renamed,
                oldPath = oldPath,
                newPath = newPath
            };

            EditorEventManager.Inst.EventTrigger(EventConstDefine.EditorEventManager_OnJsonAssetModification, args);
    
            return AssetMoveResult.DidNotMove;
        }

        ///// <summary>
        ///// 监听资源创建事件
        ///// </summary>
        ///// <param name="path">资源路径</param>
        //public static void OnWillCreateAsset(string path)
        //{
        //    if (!IsOpenModification)
        //    {
        //        return;
        //    }

        //    var args = new AssetModificationArgs
        //    {
        //        changeTypes = WatcherChangeTypes.Created,
        //        newPath = path
        //    };

        //    EditorEventManager.Inst.EventTrigger(EventConstDefine.EditorEventManager_OnJsonAssetModification, args);
        //}

        ///// <summary>
        ///// 监听资源即将被删除事件
        ///// </summary>
        ///// <param name="path">资源路径</param>
        ///// <param name="options"></param>
        ///// <returns></returns>
        //public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        //{
        //    if (!IsOpenModification)
        //    {
        //        return AssetDeleteResult.DidNotDelete;
        //    }

        //    //这才是删除
        //    if (File.Exists(path))
        //    {
        //        var args = new AssetModificationArgs
        //        {
        //            changeTypes = WatcherChangeTypes.Deleted,
        //            newPath = path
        //        };

        //        EditorEventManager.Inst.EventTrigger(EventConstDefine.EditorEventManager_OnJsonAssetModification, args);
        //    }
        //    //Unity的一个BUG，删除了一个的资源后,在创建和这个资源同路径同类型的资源，会走一遍这个监听，离谱！！！
        //    else
        //    {
        //        //Debug.LogFormat($"清空旧资源缓存！路径：{path}");
        //    }

        //    //AssetDeleteResult.DidNotDelete表示可以删除 DidDelete表示不可以删除  
        //    return AssetDeleteResult.DidNotDelete;
        //}

        //[InitializeOnLoadMethod]
        //static void ListenAssetEvent()
        //{
        //    //全局监听project面板下资源的变动

        //    EditorApplication.projectChanged += delegate ()
        //    {
        //        Debug.Log("改变资源!");
        //    };
        //}

        ///// <summary>
        ///// 监听点击或打开资源事件，每次点击project面板的资源都会执行这个方法
        ///// </summary>
        ///// <param name="assetPath">资源路径</param>
        ///// <param name="message"></param>
        ///// <returns></returns>
        //public static bool IsSelectForEdit(string assetPath, out string message)
        //{
        //    message = null;
        //    //Debug.Log($"选择了资源！资源路径：{assetPath}");
        //    //TRUE表示可以打开，FALSE表示不能在unity打开资源（但是我写false也能打开。。。）
        //    return true;
        //}

        ///// <summary>
        ///// 监听资源保存事件
        ///// </summary>
        ///// <param name="paths">资源路径</param>
        ///// <returns></returns>
        //public static string[] OnWillSaveAssets(string[] paths)
        //{
        //    if (paths != null)
        //    {
        //        //string.Join  连接多个字符串
        //        Debug.LogFormat("保存资源！路径：{0}", string.Join(",", paths));
        //    }
        //    return paths;
        //}
    }
}
#endif
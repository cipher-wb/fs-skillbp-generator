using GameApp.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public partial class ConfigIDManager
    {
        FileSystemWatcher cSharpBytesWatcher;

        static FileSystemEventArgs curBytesChangeEventArgs;
        bool isBytesWatcherInit = false;

        public void InitCSharpBytesWatcher()
        {
            if (isBytesWatcherInit) return;
            EditorApplication.update += OnCSharpBytesUpdateWatcher;
            curBytesChangeEventArgs = null;

            cSharpBytesWatcher = new FileSystemWatcher(Constants.CSharpTablePath, "*.bytes");
            cSharpBytesWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            cSharpBytesWatcher.EnableRaisingEvents = true;
            cSharpBytesWatcher.Created += OnCSharpBytesChanged;
            cSharpBytesWatcher.Changed += OnCSharpBytesChanged;
            cSharpBytesWatcher.Deleted += OnCSharpBytesChanged;
            isBytesWatcherInit = true;
        }

        //WWYTODO暂时采用Update监听，之后采用协程进行优化
        private static void OnCSharpBytesUpdateWatcher()
        {
            if (curBytesChangeEventArgs != null)
            {
                string filePath = curBytesChangeEventArgs.Name;
                curBytesChangeEventArgs = null;
                if (string.IsNullOrEmpty(filePath) || !filePath.Contains("\\") || !filePath.Contains(".bytes"))
                {
                    return;
                }

                string fileName = filePath.Split('\\').Last();
                fileName = fileName.Replace(".bytes", string.Empty);
                if (!DesignTable.EditorConfigManagerName2TableTash.ContainsKey($"{fileName}Manager"))
                {
                    return;
                }

                ITableManager manager = DesignTable.GetTableManager(fileName);
                if(manager == default)
                {
                    return;
                }
                int assetIndex = filePath.IndexOf("Assets");
                string assetPath = filePath.Substring(assetIndex, filePath.Length - assetIndex);
                var asset = (TextAsset)UnityEditor.AssetDatabase.LoadMainAssetAtPath(assetPath);
                if(asset == default)
                {
                    Log.Debug($"{fileName} FileChanged But Asset Load Fail");
                    return;
                }

                //manager?.ParseFromBytes(id, configData, out _);
                manager?.DeserializeData(asset.bytes);
                Log.Debug($"{fileName}数据文件变化");
            }
        }

        private void OnCSharpBytesChanged(object sender, FileSystemEventArgs e)
        {
            curBytesChangeEventArgs = e;
        }
    }
}

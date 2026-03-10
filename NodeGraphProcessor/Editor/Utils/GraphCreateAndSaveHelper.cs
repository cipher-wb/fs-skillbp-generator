using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GraphProcessor
{
    public static class GraphCreateAndSaveHelper
    {
        /// <summary>
        /// NodeGraphProcessor路径前缀
        /// </summary>
        public const string NodeGraphProcessorPathPrefix = "Assets/Thirds/NodeGraphProcessor";

        public static BaseGraph CreateGraph(Type graphType, string panelPath = null, string panelFileName = null)
        {
            BaseGraph baseGraph = ScriptableObject.CreateInstance(graphType) as BaseGraph;
            if (panelPath == null)
            {
                panelPath = $"{NodeGraphProcessorPathPrefix}/Examples/Saves/";
            }
            if (panelFileName == null)
            {
                panelFileName = graphType.Name;
            }
            Directory.CreateDirectory(panelPath);
            string path = EditorUtility.SaveFilePanelInProject("Save Graph Asset", panelFileName, "asset", "", panelPath);
            if (string.IsNullOrEmpty(path))
            {
                Debug.Log("创建graph已取消");
                return null;
            }
            AssetDatabase.CreateAsset(baseGraph, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return baseGraph;
        }
        
        public static void SaveGraphToDisk(BaseGraph baseGraphToSave)
        {
            EditorUtility.SetDirty(baseGraphToSave);
            AssetDatabase.SaveAssets();
        }
    }
}
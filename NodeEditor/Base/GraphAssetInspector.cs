using UnityEditor;
using GraphProcessor;
using UnityEngine.UIElements;
using NodeEditor.SkillEditor;
using UnityEngine;
using System;
using System.IO;

namespace NodeEditor
{
    //[CustomEditor(typeof(BaseGraph), true)]
    //public class GraphAssetInspector : GraphInspector
    //{
    //    protected override void CreateInspector()
    //    {
    //        //base.CreateInspector();

    //        if (target is SkillGraph skillGraph)
    //        {
    //            root.Add(new Button(() => EditorWindow.GetWindow<SkillGraphWindow>().InitializeGraph(skillGraph))
    //            {
    //                text = "打开技能编辑器"
    //            });
    //            root.Add(new Button(() => SkillGraphWindow.ExportData(skillGraph, EditorFlag.ExportInspector))
    //            {
    //                text = "导出数据"
    //            });
    //        }
    //    }
    //}
    [CustomEditor(typeof(TextAsset), true)]
    public class GraphAssetJsonInspector : Editor
    {
        protected VisualElement root;

        Editor editor;

        string targetPath;

        ConfigGraphWindow win;
        public override VisualElement CreateInspectorGUI()
        {
            root = base.CreateInspectorGUI();
            if (targetPath == null)
            {
                targetPath = AssetDatabase.GetAssetPath(target);
            }
            if (GraphHelper.IsValidGraphPath(targetPath))
            {
                root ??= new VisualElement();
                root.Add(new Button(() => { AssetDatabase.OpenAsset(target); })
                {
                    text = "打开编辑器"
                });
                root.Add(new Button(() => 
                {
                    var fileName = Path.GetFileName(targetPath);
                    Utils.DisplayProcess($"导出文件: {fileName}", (_) =>
                    {
                        ExternalExportUtil.ExportNodeJson2Excel(new System.Collections.Generic.List<string> { targetPath });
                    });
                })
                {
                    text = "导出数据"
                });
            }
            return root;
        }
    }
}
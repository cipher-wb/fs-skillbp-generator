using GameApp.Native.Battle;
using GameApp;
using GraphProcessor;
using NodeEditor.SkillEditor;
using System.Collections.Generic;
using TableDR;
using UnityEditor;
using System;

namespace NodeEditor.MapAnimEditor
{
    public class MapAnimGraphWindow : ConfigGraphWindow
    {
        public override string PathExportExcel { get { return MapAnimEditorManager.Inst.Setting.PathExportExcel; } }
        public override string NameEditor => MapAnimEditorManager.Inst.Name;

        static public Action OnSyncConfig;

        protected override void CreateToolbarView()
        {
            m_ToolbarView = new MapAnimGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }

        protected override void AfterInitializeWindow()
        {
            EditorUtility.DisplayProgressBar(TitleName, "初始化...", 0.1f);
            Utils.WatchTime($"{TitleName} 初始化", () =>
            {
                _ = MapAnimEditorManager.Inst;
            });
            base.AfterInitializeWindow();
        }

        public override void SyncConfigData()
        {
            base.SyncConfigData();

            OnSyncConfig?.Invoke();
        }
    }
}

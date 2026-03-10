using UnityEditor;

namespace NodeEditor.GamePlayEditor
{
    public class GamePlayGraphWindow : ConfigGraphWindow
    {
        public override string PathExportExcel { get { return GamePlayEditorManager.Inst.Setting.PathExportExcel; } }
        public override string NameEditor => GamePlayEditorManager.Inst.Name;
        protected override void CreateToolbarView()
        {
            m_ToolbarView = new GamePlayGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }
        protected override void AfterInitializeWindow()
        {
            EditorUtility.DisplayProgressBar(TitleName, "初始化...", 0.1f);
            Utils.WatchTime($"{TitleName} 初始化", () =>
            {
                _ = GamePlayEditorManager.Inst;
            });
            base.AfterInitializeWindow();
        }
    }
}

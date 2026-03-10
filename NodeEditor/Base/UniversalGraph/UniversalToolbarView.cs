using GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class UniversalToolbarView : ToolbarView
    {
        protected readonly MiniMap m_MiniMap;
        protected readonly BaseGraph m_BaseGraph;
        protected readonly BaseGraphView m_BaseGraphView;

        protected readonly Texture2D m_CreateNewToggleIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/CreateNew.png");

        protected readonly Texture2D m_MiniMapToggleIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/MiniMap.png");

        protected readonly Texture2D m_ConditionalToggleIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Run.png");

        protected readonly Texture2D m_ExposedParamsToggleIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Blackboard.png");

        protected readonly Texture2D m_GotoFileButtonIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/GotoFile.png");

        protected readonly Texture2D m_CommitButtonIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Commit.png");

        protected readonly Texture2D m_EyeButtonIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
    $"{GraphCreateAndSaveHelper.NodeGraphProcessorPathPrefix}/Editor/Eye.png");

        public UniversalToolbarView(BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) : base(graphView)
        {
            m_MiniMap = miniMap;
            // 默认隐藏小地图，防止Graph内容过多而卡顿
            m_MiniMap.visible = false;

            m_BaseGraph = baseGraph;
            m_BaseGraphView = graphView;
        }

        protected override void AddButtons()
        {
            // TODO 暂时屏蔽，待扩展成json
            //AddButton(new GUIContent("", m_CreateNewToggleIcon, "新建Graph资产"), () =>
            //{
            //    GenericMenu genericMenu = new GenericMenu();
            //    foreach (var graphType in TypeCache.GetTypesDerivedFrom<NPBehaveGraph>())
            //    {
            //        genericMenu.AddItem(new GUIContent($"{graphType.Name}"), false,
            //            data =>
            //            {
            //                var baseGraph = GraphCreateAndSaveHelper.CreateGraph(data as System.Type, Constants.GraphSavesPathPrefix);
            //                GraphAssetCallbacks.InitializeGraph(baseGraph);
            //            }, graphType);
            //    }

            //    genericMenu.ShowAsContext();
            //}, true);

            //AddSeparator(5);

            //AddToggle(new GUIContent("", m_ExposedParamsToggleIcon, "开/关参数面板"),
            //    graphView.GetPinnedElementStatus<ExposedParameterView>() != DropdownMenuAction.Status.Hidden,
            //    (v) => graphView.ToggleView<ExposedParameterView>());

            //AddSeparator(5);
            AddToggle(new GUIContent("", m_MiniMapToggleIcon, "开/关小地图"), m_MiniMap.visible,
                (v) => m_MiniMap.visible = v);

            //AddSeparator(5);
            //AddToggle(new GUIContent(m_ConditionalToggleIcon, "开/关运行的面板"),
            //    graphView.GetPinnedElementStatus<ConditionalProcessorView>() !=
            //    DropdownMenuAction.Status.Hidden, (v) => graphView.ToggleView<ConditionalProcessorView>());

            //AddSeparator(5);
            AddButton(new GUIContent("【重载界面】", "主动重载界面"), () => 
            {
                (this.m_BaseGraphView as UniversalGraphView)?.universalGraphWindow.RefreshWindow();
            },false);
        }
    }
}
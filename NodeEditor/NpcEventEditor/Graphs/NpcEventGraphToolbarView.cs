using GraphProcessor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor.NpcEventEditor
{
    public class NpcEventGraphToolbarView : ConfigGraphToolbarView
    {
        NpcEventGraphWindow npcEventGraphWindow;
        public NpcEventGraphToolbarView(NpcEventGraphWindow graphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) 
            : base(graphWindow, graphView, miniMap, baseGraph)
        {
            npcEventGraphWindow = graphWindow;
        }

        protected override void AddButtons()
        {
            //ID分配
            AddCustom(() =>
            {
                var idLocal = LocalSettings.Inst.ID;
                GUI.color = Color.white;
                GUILayout.Label($"ID分配：{idLocal}",
                    EditorGUIStyleHelper.GetGUIStyleByName(nameof(EditorStyles.toolbarButton)));
                GUI.color = Color.white;
            });

            //小地图开关
            AddToggle(new GUIContent("", m_MiniMapToggleIcon, "开/关小地图"), m_MiniMap.visible, (v) => m_MiniMap.visible = v);

            //定位选中文件
            AddButton(new GUIContent("", m_GotoFileButtonIcon, "定位至资产文件"), () =>
            {
                configGraphWindow.PingObject();
            });

            //打开日志
            AddButton(new GUIContent("", m_ExposedParamsToggleIcon, "打开日志目录"), () =>
            {
                Utils.OpenDirectory(Log.LOG_DIR);
            });

            //列表面板
            AddToggle(new GUIContent("", m_ExposedParamsToggleIcon, "列表面板"), false, (isToggle) => 
            {
                if (isToggle)
                {
                    graphView.OpenPinned<MapEventPinnedView>();
                }
                else
                {
                    graphView.ClosePinned<MapEventPinnedView>();
                }
            });

            //查看运行状态
            AddToggle(new GUIContent("", m_EyeButtonIcon, "查看运行状态"), false, (isToggle) =>
            {
                if (configGraphWindow is NpcEventGraphWindow graphWindow)
                {
                    if (isToggle)
                    {
                        graphWindow.OpenRuntimeMode();
                    }
                    else
                    {
                        graphWindow.CloseRuntimeMode();
                    }
                }
            });
            
            AddButton(new GUIContent("【节点引用刷新】", "刷新节点的引用状态"), () =>
            {
                if (configGraphWindow is NpcEventGraphWindow graphWindow)
                {
                    graphWindow.UpdateNodeRefState();
                } 
            }, false);
            
            AddButton(new GUIContent("【保存数据】", "保存数据到资源"), configGraphWindow.SaveData, false);
            AddButton(new GUIContent("【导出数据】", "保存并导出数据到表格"), configGraphWindow.ExportData, false);
            AddButton(new GUIContent("【SVN提交】", "SVN提交文件"), configGraphWindow.SaveAndSVNCommit, false);
            AddButton(new GUIContent("【导表到私服】", "导出当前配置表，并同步到服务器"), npcEventGraphWindow.SaveExportAndSyncData, false);
        }
    }
}

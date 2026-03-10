using GameApp.Battle;
using GraphProcessor;
using NodeEditor.NpcEventEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using TableDR;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace NodeEditor
{
    public class ConfigGraphToolbarView : NPBehaveToolbarView
    {
        protected ConfigGraphWindow configGraphWindow;
        public ConfigGraphToolbarView(ConfigGraphWindow graphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph)
            : base(graphView, miniMap, baseGraph)
        {
            configGraphWindow = graphWindow;
        }

        protected override void AddButtons()
        {
            // ID段分类按照IP来处理，显示提示下
            AddCustom(() =>
            {
                var idLocal = LocalSettings.Inst.ID;
                GUI.color = Color.white;
                GUILayout.Label($"ID:{idLocal}",
                    EditorGUIStyleHelper.GetGUIStyleByName(nameof(EditorStyles.toolbarButton)));
                GUI.color = Color.white;
            });

            base.AddButtons();

            //默认打开Pinned面板
            if (graphView.GetPinnedElementStatus<ConfigPinnedView>() == DropdownMenuAction.Status.Hidden)
            {
                graphView.ToggleView<ConfigPinnedView>();
            }

            AddToggle(new GUIContent(m_ExposedParamsToggleIcon, "开/关节点列表面板"),
                graphView.GetPinnedElementStatus<ConfigPinnedView>() !=
                DropdownMenuAction.Status.Hidden, (v) => graphView.ToggleView<ConfigPinnedView>());

            foreach (var graphType in TypeCache.GetTypesDerivedFrom<ConfigGraph>())
            {
                // 限制graph
                if (GetType().Name.StartsWith(graphType.Name))
                {
                    AddButton(new GUIContent("", m_CreateNewToggleIcon, "新建Graph资产"), () =>
                    {
                        GenericMenu genericMenu = new GenericMenu();
                        genericMenu.AddItem(new GUIContent($"{graphType.Name}"), false, data =>
                        {
                            var path = GraphHelper.CreateGraph(graphType);
                            var graph = GraphHelper.LoadGraph(graphType, path);
                            GraphAssetCallbacks.InitializeGraph(graph, path);
                        }, graphType);
                        genericMenu.ShowAsContext();
                    }, true);
                    break;
                }
            }

            AddButton(new GUIContent("", m_ExposedParamsToggleIcon, "打开日志目录"), () =>
            {
                Utils.OpenDirectory(Log.LOG_DIR);
            }, true);

            var fileName = System.IO.Path.GetFileNameWithoutExtension(graphView.graph.path);
            AddButton(new GUIContent($"【{fileName}】", "定位文件"), () =>
            {
                // 定位文件
                configGraphWindow.PingObject();
                // 拷贝文件名到剪贴板
                GUIUtility.systemCopyBuffer = fileName;
                // 给个提示
                configGraphWindow.ShowNotification($"已定位文件 & 复制文件名到剪贴板\n\n{fileName}");
            }, true);

            AddButton(new GUIContent("【同步数据(右击全同)】", "同步运行时数据，避免导表。右击一次性同步所有数据"), () => { ConfigGraphWindow.SyncAllConfigData(configGraphWindow); }, false);

            AddButton(new GUIContent("【保存数据】", "保存数据到资源"), configGraphWindow.SaveData, false);

            AddButton(new GUIContent("【导出数据】", "保存并导出数据到表格"), configGraphWindow.ExportData, false);

            AddButton(new GUIContent("【SVN提交】", "SVN提交文件"), configGraphWindow.SaveAndSVNCommit, false);

            // 测试技能
            if (this.GetType() != typeof(NpcEventGraphToolbarView))
            {
                AddCustom(() =>
                {
                    var battle = GameApp.AppFacade.BattleManager?.Battle;
                    if (battle != null)
                    {
                        GUILayout.Label("选择单位");
                        int index = configGraphWindow.Entitys.IndexOf(configGraphWindow.EntityID);
                        if (configGraphWindow.EntityID != 0 && index >= 0)
                        {
                            configGraphWindow.CurIndex = index;
                        }
                        configGraphWindow.CurIndex = EditorGUILayout.Popup(configGraphWindow.CurIndex, configGraphWindow.EntityNames, GUILayout.Width(80f));
                        configGraphWindow.OnSelectionChange();
                        if (configGraphWindow is SkillEditor.SkillGraphWindow skillGraphWindow)
                        {
                            // 装配技能
                            if (choicesMap == null)
                            {
                                choicesMap = new Dictionary<string, TSkillSlotType>();
                                var slotType = TSkillSlotType.TSST_TEMP_SKILL;
                                choicesMap[slotType.GetDescription(false)] = slotType;
                                foreach (var slotValue in Enum.GetValues(typeof(TableDR.TSkillSlotType)))
                                {
                                    slotType = (TSkillSlotType)slotValue;
                                    if (slotType.IsUISlotSkill())
                                    {
                                        choicesMap[slotType.GetDescription(false)] = slotType;
                                    }
                                }
                                choices = choicesMap.Keys.ToArray();
                            }
                            var skillSlotType = TSkillSlotType.TSST_TEMP_SKILL;
                            if (skillGraphWindow.HasNode<SkillConfigNode>())
                            {
                                var oldIndex = selectedIndex;
                                selectedIndex = EditorGUILayout.Popup("", selectedIndex, choices, GUILayout.Width(80));
                                if (!choicesMap.TryGetValue(choices[selectedIndex], out skillSlotType))
                                {
                                    skillSlotType = TSkillSlotType.TSST_TEMP_SKILL;
                                }
                                if (selectedIndex != oldIndex)
                                {
                                    // 切换技能
                                    skillGraphWindow.TestSKill(false, skillSlotType, false);
                                }
                            }
                            // 测试技能
                            if (GUILayout.Button(testSkillGUIContent))
                            {
                                skillGraphWindow.TestSKill(true);
                            }
                            GUILayout.Space(4);
                        }
                    }
                }, false);
            }

            #region 工具代码-重新分配ID
            //AddButton(new GUIContent("【重新分配ID】", "重新分配ID解决冲突"), ()=>
            //{
            //    var graph = configGraphWindow.GetGraph();
            //    foreach (var node in graph.nodes)
            //    {
            //        if (node is RefConfigBaseNode || node is RefTemplateBaseNode) continue;
            //        if (node is IConfigBaseNode configNode)
            //        {
            //            var id = configNode.GetConfigID();
            //            configNode.GenerateID(true);
            //            var idNew = configNode.GetID();
            //            Log.Error($"[{graph.name}] 存在相同ID节点, ID:{id}->{idNew}");
            //        }
            //    }
            //    graph.SaveGraphToDisk();
            //}, false);
            #endregion
        }

        #region 测试技能
        private GUIContent testSkillGUIContent = new GUIContent("【测试技能】", "同步数据到游戏，并释放技能");

        private int selectedIndex = 0;
        private static string[] choices;
        private static Dictionary<string, TableDR.TSkillSlotType> choicesMap;
        #endregion
    }
}

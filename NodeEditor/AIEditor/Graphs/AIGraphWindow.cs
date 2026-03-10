using GameApp.Native.Battle;
using GameApp;
using GraphProcessor;
using NodeEditor.SkillEditor;
using System.Collections.Generic;
using TableDR;
using UnityEditor;

namespace NodeEditor.AIEditor
{
    public class AIGraphWindow : ConfigGraphWindow
    {
        public override string PathExportExcel { get { return AIEditorManager.Inst.Setting.PathExportExcel; } }
        public override string NameEditor => AIEditorManager.Inst.Name;

        protected override void CreateToolbarView()
        {
            m_ToolbarView = new AIGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }

        protected override void AfterInitializeWindow()
        {
            EditorUtility.DisplayProgressBar(TitleName, "初始化...", 0.1f);
            Utils.WatchTime($"{TitleName} 初始化", () =>
            {
                _ = AIEditorManager.Inst;
            });
            base.AfterInitializeWindow();
        }

        public void TestAI(bool check)
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                ShowNotification("测试技能失败，需进战斗测试！");
                return;
            }
            if (check)
            {
                IgnoreFlagSave();
            }
            SyncConfigData();
            if (graph.nodes == null)
            {
                Log.Fatal($"Graph Nodes Is Null {graph.name}");
                return;
            }
            int aiNodeID = 0;
            foreach (var node in graph.nodes)
            {
                if (!(node is IConfigBaseNode iConfigNode))
                    continue;
                var id = iConfigNode.GetConfigID();

                var configName = iConfigNode.GetConfigName();

                if (aiNodeID == 0 && iConfigNode is AITaskNodeConfigNode aiNode && aiNode.CanExport())
                {
                    List<BaseNode> parents = new List<BaseNode>();
                    aiNode.GetParentNodes(parents);
                    if (parents.Count == 0 || (parents.Count == 1 && (parents[0] is ConfigBaseNode<BattleAIConfig>)))
                    {
                        aiNodeID = id;
                    }
                }
            }

            if (aiNodeID > 0)
            {
                string entity_id = "";
                if (battle.CurrControlFighter == null)
                {
                    entity_id = battle.BattlePlayerManager.GetSelfPlayerMainControlEntityId().ToString();
                }
                else
                {
                    entity_id = battle.CurrControlFighter.Entity.Id.ToString();
                }

                if (this.m_ToolbarView is AIGraphToolbarView toolbarView && EntityID > 0)
                {
                    var entity_id_select = EntityID.ToString();
                    entity_id = entity_id_select;
                }
                BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_ADD_AI, entity_id, aiNodeID.ToString(), "0");
                ShowNotification($"测试AI EntityID : {entity_id}, AiNodeID : {aiNodeID}");
            }
            else
            {
                ShowNotification("测试AI失败，找不到AI根节点！");
            }
        }

    }
}

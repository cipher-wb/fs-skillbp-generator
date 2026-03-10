using UnityEngine;
using UnityEditor;
using GraphProcessor;
using System.Reflection;
using GameApp.Native.Battle;
using TableDR;
using GameApp;
using GameApp.Components;

namespace NodeEditor.SkillEditor
{
    public class SkillGraphWindow : ConfigGraphWindow
    {
        public override string PathExportExcel { get { return SkillEditorManager.Inst.Setting.PathExportExcel; } }
        public override string NameEditor => SkillEditorManager.Inst.Name;
        protected override void CreateGraphView()
        {
            graphView = new SkillGraphView(this);
        }

        protected override void CreateToolbarView()
        {
            m_ToolbarView = new SkillGraphToolbarView(this, graphView, m_MiniMap, graph);
            graphView.Add(m_ToolbarView);
        }

        protected override void AfterInitializeWindow()
        {
            EditorUtility.DisplayProgressBar(TitleName, "初始化...", 0.1f);
            Utils.WatchTime($"{TitleName} 初始化", () =>
            {
                _ = SkillEditorManager.Inst;
            }, false);

            base.AfterInitializeWindow();
        }

        public bool HasNode<T>() where T: ConfigBaseNode
        {
            if (!graph) return false;
            foreach (var node in graph.nodes)
            {
                if (node is T) return true;
            }
            return false;
        }
        public void TestSKill(bool check, TSkillSlotType skillSlotType = TSkillSlotType.TSST_TEMP_SKILL, bool isUseSkill = true)
        {
            var battle = AppFacade.BattleManager?.Battle;
            if (battle == null)
            {
                ShowNotification($"测试技能失败，需进战斗测试！\n{TitleName}");
                return;
            }

            if (check)
            {
                IgnoreFlagSave();
            }

            SyncConfigData();
            SkillConfig skillConfig = null;

            if (graph.nodes == null)
            {
                Log.Fatal($"Graph Nodes Is Null: {TitleName}");
                return;
            }

            if (battle.IsGamePause)
                battle.ResumeGame();
            if (IsPauseGame)
                IsPauseGame = false;
            //BattleWrapper.UnityEditorDebug_ReInitConfig();

            foreach (var node in graph.nodes)
            {
                if (!(node is IConfigBaseNode iConfigNode))
                    continue;
                var id = iConfigNode.GetConfigID();

                var configName = iConfigNode.GetConfigName();
                //var configType = Utils.GetConfigNodeType(configName);

                if (iConfigNode is SkillConfigNode skillConfigNode && node.CanExport())
                {
                    if (skillConfig == null)
                    {
                        skillConfig = skillConfigNode.Config;
                    }
                    else
                    {
                        ShowNotification($"存在多个技能节点，请检查！\n{TitleName}");
                    }
                }

                //if (node.debug && configType != 0)
                //{
                //    BattleWrapper.UnityEditorDebug_AddDebugConfigId(configType, id);
                //}
            }

            if (skillConfig != null)
            {
                var skillID = skillConfig.ID;
                var entityID = battle.CurrControlFighter.Entity.Id;
                if (this.m_ToolbarView is SkillGraphToolbarView toolbarView && EntityID > 0)
                {
                    var entityIDSelect = EntityID;
                    //if (entity_id_select != entityID)
                    //{
                    //    // 更换主控单位
                    //    BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_CHANGE_MAIN_CONTROL_ROLE, entity_id_select, "0", "0");
                    //}
                    entityID = entityIDSelect;
                }
                var isHasSkill = false;
                var entity = AppFacade.BattleManager.Battle.BattleEntityProcessor.GetBattleEntity(entityID);
                if (entity != null)
                {
                    var skillComp = entity.GetComp<BattleSkillCollectComp>();
                    if (skillComp != null)
                    {
                        var skillInfo = skillComp.GetSkillByConfigID(skillID);
                        // 屏蔽下临时技能处理
                        if (skillInfo.IsValid && skillInfo.SlotType != TSkillSlotType.TSST_TEMP_SKILL)
                        {
                            skillSlotType = skillInfo.SlotType;
                        }
                        isHasSkill = skillComp.HasSkill((skillSlotType, skillID));
                    }
                }
                var skill_id = skillID.ToString();
                var entity_id = entityID.ToString();
                var slot = ((int)skillSlotType).ToString();

                // 更新槽位技能
                BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_CHANGE_ENTITY_SKILL, entity_id, slot, skill_id);
                // 更新技能数据
                if (isHasSkill)
                {
                    // 仅存在技能情况下，刷新数据，因为更换槽位技能针对已装配技能，不做数据更新
                    BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_UPDATE_SKILL_DATA, entity_id, skill_id, slot);
                }
                // 刷新技能CD
                BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_RESET_SKILL_CD, entity_id, slot, skill_id);
                // 使用技能
                if (isUseSkill)
                {
                    BattleWrapper.BattleNet_SendBattleCheatCmd((int)TCheatType.TCT_USE_SKILL, entity_id, skill_id, slot);
                }

                // 性能优化
                //ShowNotification($"测试技能 EntityID : {entity_id}, SkillID : {skillID}\n{TitleName}");
            }
            else
            {
                ShowNotification($"测试技能失败，找不到技能节点！\n{TitleName}");
            }
        }

        /// <summary>
        /// TODO 表格数据转编辑器
        /// </summary>
        public void ExcelData2SkillGraph()
        {
            int id = 46;
            var config = DesignTable.GetTableCell<SkillConfig>(id);
            if (config != null)
            {
                // 获取所有节点描述信息
                foreach (var nodeType in TypeCache.GetTypesDerivedFrom(typeof(ConfigBaseNode<>)))
                {
                    foreach (var field in nodeType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        //if (field.GetCustomAttribute<HideInInspector>() == null && field.GetCustomAttributes().Any(c => c is InputAttribute || c is OutputAttribute))
                        //    targetDescription.slotTypes.Add(field.FieldType);
                    }
                }
                // 添加节点到graph及连线
                // TODO 测试
                var node = BaseNode.CreateFromType<SkillConfigNode>(Vector2.zero);
                node.Config = config;
                graphView.AddNode(node);
            }
        }
    }
}
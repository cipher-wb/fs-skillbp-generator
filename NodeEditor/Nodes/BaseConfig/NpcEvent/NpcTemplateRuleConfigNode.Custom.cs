using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// MapEventConditionConfigNode自定义节点
    /// </summary>
    public partial class NpcTemplateRuleConfigNode
    {
        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][NPC模板]");
        }

        [ShowInInspectorAttribute(), Button("刷新数据")]
        private void OnUpdateConfig()
        {
            foreach (var input in inputPorts)
            {
                foreach (var edge in input.GetEdges())
                {
                    var conditionNode = edge.outputNode as MapEventConditionConfigNode;
                    if (conditionNode != default)
                    {
                        UpdateConfig(conditionNode);
                        return;
                    }
                }
            }
        }

        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            if (edges.Count != 1)
            {
                return;
            }

            var edge = edges.First();
            if (edge.outputNode is MapEventConditionConfigNode conditionNode)
            {
                UpdateConfig(conditionNode);
            }
        }

        /// <summary>
        /// 生成配置数据
        /// </summary>
        /// <param name="conditionNode"></param>
        private void UpdateConfig(MapEventConditionConfigNode conditionNode)
        {
            var formulaNodes = conditionNode?.GetFormulaList();
            if(formulaNodes == default)
            {
                return;
            }

            if(conditionNode.Config.GameEntityType == GameEntityType.ET_Npc)
            {
                UpdateNpcConfig(conditionNode, formulaNodes);
            }

            //传递动态npcid到link表
            TransferToLinkConfig(conditionNode);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="conditionNode"></param>
        /// <param name=""></param>
        private void UpdateNpcConfig(MapEventConditionConfigNode conditionNode, List<MapEventFormulaConfigNode> formulaNodes)
        {
            //1、随五行，随宗门(SectConfig）
            //2、指定境界、相对演员
            //3、男、女

            //五行
            List<ElementsProperty> elements = new List<ElementsProperty>();

            //境界
            var npcState = 1;

            //男、女
            var sex = (TSexEnum)UnityEngine.Random.Range(1, 3);

            foreach (var node in formulaNodes)
            {
                var conditionType = node.Config.ConditionType;
                if (conditionType == MapEventConditionType.MECT_WUXING)
                {
                    node.Config.FormulaC?.ForEach(element =>
                    {
                        elements.Add(element.Clone());
                    });
                }
                else if (conditionType == MapEventConditionType.MECT_SEX)
                {
                    if (node.Config.FormulaB?.Count > 0)
                    {
                        sex = (TSexEnum)(node.Config.FormulaB.FirstOrDefault());
                    }
                }
                else if (conditionType == MapEventConditionType.MECT_STATE_FIXED)
                {
                    var state = node.Config.FormulaA?.FirstOrDefault() ?? default;
                    if (state == default) { continue; }

                    npcState = state.Rule switch
                    {
                        TNpcMatchRuleType.TNMRT_EQUAL => state.Value,
                        TNpcMatchRuleType.TNMRT_SPACE => UnityEngine.Random.Range(state.Value, state.ValueEnd + 1),
                        _ => 1,
                    };
                }
                else if (conditionType == MapEventConditionType.MECT_STATE_WITHPLAYER)
                {

                }
                else if (conditionType == MapEventConditionType.MECT_STATE_WITHPLAYER_DYNAMIC)
                {

                }
                else if (conditionType == MapEventConditionType.MECT_LEVEL_FIXED)
                {

                }
            }

            if (elements.Count == 0)
            {
                var randomElement = UnityEngine.Random.Range((int)TElementsType.TELT_METAL, (int)TElementsType.TELT_EARTH + 1);
                elements.Add(new ElementsProperty((TElementsType)randomElement, 0));
            }

            //五行和宗门对应
            var sectID = elements.FirstOrDefault()?.Element switch
            {
                TElementsType.TELT_METAL => 1,
                TElementsType.TELT_WOOD => 2,
                TElementsType.TELT_WATER => 3,
                TElementsType.TELT_FIRE => 4,
                TElementsType.TELT_EARTH => 5,
                _ => 1,
            };

            if (Config.RoleCommonProperty == default)
            {
                SetConfigValue(nameof(Config.RoleCommonProperty), new TRoleCommonProperty());
            }

            //五行
            SetSpecifyConfigValue(Config.RoleCommonProperty, nameof(Config.RoleCommonProperty.ElementsType), elements);
            //宗门
            SetSpecifyConfigValue(Config.RoleCommonProperty, nameof(Config.RoleCommonProperty.SectID), sectID);
            //境界
            SetSpecifyConfigValue(Config.RoleCommonProperty, nameof(Config.RoleCommonProperty.ExpState), npcState);
            //性别
            SetSpecifyConfigValue(Config.RoleCommonProperty, nameof(Config.RoleCommonProperty.Sex), sex);
            //种族
            SetSpecifyConfigValue(Config.RoleCommonProperty, nameof(Config.RoleCommonProperty.RaceType), TRaceType.TRR_HUMAN);

            //标签
            SetConfigValue(nameof(Config.NpcModuleStaticFlags), new List<NpcModuleStaticFlag>() { NpcModuleStaticFlag.NMSFLAG_LIFE });

            //专用类型
            SetConfigValue(nameof(Config.TemplateType), NpcTemplateRuleConfig_TNpcTemplateType.TNT_MAPEVENT);
        }

        /// <summary>
        /// 把动态模板id，传递到link表
        /// </summary>
        private void TransferToLinkConfig(MapEventConditionConfigNode conditionNode)
        {
            var linkNode = conditionNode.GetLoopPreviousNode<NpcEventLinkConfigNode>();
            if(linkNode == default) { return; }

            SetSpecifyConfigValue(linkNode, nameof(linkNode.Config.NpcTemplateID), Config.ID);         
        }
    }
}

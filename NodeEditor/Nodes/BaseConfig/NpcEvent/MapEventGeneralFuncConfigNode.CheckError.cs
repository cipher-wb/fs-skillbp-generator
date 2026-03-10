using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 检视面板错误提示
    /// </summary>
    public partial class MapEventGeneralFuncConfigNode
    {
        public HashSet<int> Target1Links { get; private set; } = new HashSet<int>();

        public HashSet<int> Target2Links { get; private set; } = new HashSet<int>();

        /// <summary>
        /// 错误提示
        /// </summary>
        private bool IsExitInspectorError => !string.IsNullOrEmpty(InspectorError);

        public string InspectorError { get; set; }

        /// <summary>
        /// 检测好感度是否为0
        /// </summary>
        /// <param name="favorData"></param>
        public void AddInspectorErrorFavor(ChangeFavorData favorData)
        {
            if (favorData.ChangeType == SymbolType.Add)
            {
                if (favorData.ChangeValue == 0)
                {
                    InspectorError += "【好感度】不能为0\n";
                }
            }
        }

        /// <summary>
        /// 检测目标是否唯一
        /// </summary>
        /// <param name="targets"></param>
        public void AddInspectorErrorTargetOnlyOne(List<MapEventTarget> targets)
        {
            if (targets?.Count != 1)
            {
                InspectorError += "【对象列表】只能存在1个对象 \n";
            }
        }

        public void AddInspectorErrorTargetIsEmpty(List<MapEventTarget> targets)
        {
            if (targets.Count == 0)
            {
                InspectorError += "【对象列表】为空 \n";
            }
        }

        /// <summary>
        /// 检测两个目标是否相同
        /// </summary>
        /// <param name="target1s"></param>
        /// <param name="target2s"></param>
        public void AddInspectorErrorTargetSame(List<MapEventTarget> target1s, List<MapEventTarget> target2s)
        {
            var eventNode = GetLoopPreviousNode<NpcEventConfigNode>();
            var linkNode = GetLoopPreviousNode<NpcEventLinkConfigNode>();
            if (eventNode == null || linkNode == null)
            {
                return;
            }
            
            if(eventNode?.Config?.ID == 0 || linkNode?.Config?.ID == 0)
            {
                return;
            }

            AddTargetLinks(target1s, Target1Links, eventNode, linkNode);
            AddTargetLinks(target2s, Target2Links, eventNode, linkNode);

            foreach(var target1Link in Target1Links)
            {
                if (Target2Links.Contains(target1Link))
                {
                    InspectorError += "【不同对象列表】存在相同目标 \n";
                    return;
                }
            }
        }

        private void AddTargetLinks(List<MapEventTarget> targets, HashSet<int> targetLinks, NpcEventConfigNode eventNode, NpcEventLinkConfigNode linkNode)
        {
            //主角
            var leaderLinkID = eventNode?.Config?.LinkActor[0] ?? 0;
            //当前演员
            var mineLinkID = linkNode?.Config?.ID ?? 0;

            targetLinks.Clear();
            targets?.ForEach(target =>
            {
                if (target.TargetType == MapEventTargetType.MapEventTargetType_AllCostar)
                {
                    eventNode?.Config?.LinkActor?.ForEach(linkID =>
                    {
                        if (linkID != leaderLinkID)
                        {
                            targetLinks.Add(linkID);
                        }
                    });
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_Leader)
                {
                    targetLinks.Add(leaderLinkID);
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_SpecificActor)
                {
                    if (target.TargetIndex == 0)
                    {
                        targetLinks.Add(leaderLinkID);
                    }
                    else
                    {
                        if (eventNode?.Config?.LinkActor.Count > target.TargetIndex)
                        {
                            targetLinks.Add(eventNode.Config.LinkActor[target.TargetIndex]);
                        }
                    }
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_Player)
                {
                    targetLinks.Add(0);
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_MineActor)
                {
                    targetLinks.Add(mineLinkID);
                }
            });
        }

        /// <summary>
        /// 检测表格是否选择
        /// </summary>
        /// <param name="tables"></param>
        public void AddInspectorErrorTableNotSelect(List<TableSelectData> tables)
        {
            if(tables.Count == 0)
            {
                InspectorError += "【表格未选择】\n";
                return;
            }

            foreach (var table in tables)
            {
                if (table.ID == 0)
                {
                    InspectorError += "【表格未选择】\n";
                    return;
                }
            }
        }

        /// <summary>
        /// 检测表格是否选择
        /// </summary>
        /// <param name="table"></param>
        public void AddInspectorErrorTableNotSelect(TableSelectData table)
        {
            if (table == default || (table != default && table.ID == 0))
            {
                InspectorError += "【表格未选择】\n";
            }
        }

        /// <summary>
        /// 奇遇战斗奖励
        /// </summary>
        /// <param name="data"></param>
        /// <param name="title"></param>
        public void AddInspectorErrorEncounterBattleAwardData(EncounterBattleAwardData data, string title)
        {
            if (data.GradeType == TBattleMissionGradeType.TBMGT_NULL)
            {
                InspectorError += $"【{title} 分级选择错误】\n";
            }

            var dropConfig = DropConfigManager.Instance.GetGroupConfigs(data.DropID);
            if (dropConfig == default || dropConfig.Count == 0)
            {
                InspectorError += $"【{title} 掉落配置不存在】\n";
            }

            var itemConfig = ItemConfigManager.Instance.GetItem(data.ItemData.ID);
            if (itemConfig == default)
            {
                InspectorError += $"【{title} 道具展示ID错误】\n";
            }

            return;
        }

        /// <summary>
        /// 掉落类型
        /// </summary>
        /// <param name="dropIDs"></param>
        /// <param name="dropType"></param>
        public void AddInspectorErrorDropType(TDropInfoPushType dropType)
        {
            if (dropType == TDropInfoPushType.TD_NoTips)
            {
                InspectorError += $"【掉落类型错误】\n";
            }
        }

        public bool IsLastNode()
        {
            if (IsLastNodeInParent())
            {
                return true;
            }

            var edges = GetSiblingEdge();
            if (edges != null)
            {
                for (int i = edges.Count - 1; i >= 0; i--)
                {
                    var node = edges[i].inputNode as MapEventGeneralFuncConfigNode;
                    if(node == default) { continue; }

                    if (node.Config.GeneralFuncType != Config.GeneralFuncType)
                    {
                        return false;
                    }

                    if (node == this)
                    {
                        return true;
                    }
                }   
            }

            return false;
        }
    }
}

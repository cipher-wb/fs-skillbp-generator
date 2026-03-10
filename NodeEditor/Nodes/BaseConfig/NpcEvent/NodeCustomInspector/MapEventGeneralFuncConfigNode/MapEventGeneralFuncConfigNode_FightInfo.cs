using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Funny.Base.Utils;
using TableDR;
using UnityEngine;
using static NodeEditor.MapEventGeneralFuncConfigNode;

namespace NodeEditor
{
    public class FightInfo
    {
        private readonly MapEventGeneralFuncConfigNode baseNode;

        [ShowInInspector, HideReferenceObjectPicker, LabelText("友方"), ShowIf("IsFightNormal")]
        [ListDrawerSettings(CustomAddFunction = "OnFriendTargetsAdd")]
        public List<MapEventTarget> FriendTargets { get; private set; } = new List<MapEventTarget>();

        [ShowInInspector, HideReferenceObjectPicker, LabelText("敌方"), ShowIf("IsFightNormal")]
        [ListDrawerSettings(CustomAddFunction = "OnEnemyTargetsAdd")]
        public List<MapEventTarget> EnemyTargets { get; private set; } = new List<MapEventTarget>();

        [ShowInInspector, HideReferenceObjectPicker, LabelText("不参战对象")]
        [ListDrawerSettings(CustomAddFunction = "OnNoBattleTargetsAdd"), ShowIf("IsFightNormal")]
        public List<MapEventTarget> NoBattleTargets { get; private set; } = new List<MapEventTarget>();

        #region IntParams1
        [ShowInInspector, HideReferenceObjectPicker, LabelText("战斗类型"), GUIColor(0f, 1f, 0f, 1f)]
        public TMapEventFightType FightType { get; private set; }

        public bool IsFightNormal => FightType == TMapEventFightType.TMapEventFightType_Normal;

        [ShowInInspector, HideReferenceObjectPicker, LabelText("战斗表"), ShowIf("IsFightNormal")]
        public TableSelectData BattleTableDatas { get; private set; } = new TableSelectData(typeof(BattleConfig).FullName, 0);

        [ShowInInspector, HideReferenceObjectPicker, LabelText("怪物-可带入等阶"), ShowIf("IsFightNormal")]
        public TMonsterRankEnum TakeInMonsterRank { get; private set; }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("怪物-最大带入数量"), ShowIf("IsFightNormal")]
        public int TakeInMaxCount { get; private set; }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("敌方不死"), ShowIf("IsFightNormal")]
        public bool EnemyNeverDie { get; private set; }
        #endregion

        #region IntParams2
        [ShowInInspector, LabelText("停留当前行为类型"), HideReferenceObjectPicker]
        [ValueDropdown("GetDropdownFightInfoContinue"), ShowIf("IsFightNormal")]
        public List<EventActionChooseConditionType> ContinueActionType { get; private set; } = new List<EventActionChooseConditionType>();

        private IEnumerable<ValueDropdownItem> GetDropdownFightInfoContinue()
        {
            foreach (var funType in MapEventGeneralFuncConfigNode.VD_FightInfoContinue)
            {
                yield return new ValueDropdownItem(funType.GetDescription(false), funType);
            }
        }
        #endregion

        public FightInfo(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        private MapEventTarget OnFriendTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Leader);
        }

        private MapEventTarget OnEnemyTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Leader);
        }

        private MapEventTarget OnNoBattleTargetsAdd()
        {
            return new MapEventTarget(MapEventTargetType.MapEventTargetType_Null);
        }

        public List<int> ToIntParams1()
        {
            return new List<int> { BattleTableDatas.ID, (int)TakeInMonsterRank, TakeInMaxCount, EnemyNeverDie ? 1 : 0, (int)FightType };
        }

        public List<int> ToIntParams2()
        {
            var param2 = new List<int>();
            ContinueActionType?.ForEach(myType =>
            {
                param2.Add((int)myType);
            });
            return param2;
        }

        public void ConfigToData()
        {
            //Target1
            baseNode.RestoreTargets(baseNode.Config?.Target1, FriendTargets);

            //Target2
            baseNode.RestoreTargets(baseNode.Config?.Target2, EnemyTargets);

            //Target3
            baseNode.RestoreTargets(baseNode.Config?.Target3, NoBattleTargets);

            //IntParams1
            if (baseNode.Config?.IntParams1?.Count >= 1)
            {
                BattleTableDatas = new TableSelectData(typeof(BattleConfig).FullName, baseNode.Config?.IntParams1[0] ?? 0);
                BattleTableDatas.OnSelectedID();
            }

            if (baseNode.Config?.IntParams1?.Count >= 2)
            {
                TakeInMonsterRank = (TMonsterRankEnum)(baseNode.Config?.IntParams1[1] ?? 0);
            }

            if (baseNode.Config?.IntParams1?.Count >= 3)
            {
                TakeInMaxCount = baseNode.Config?.IntParams1[2] ?? 0;
            }

            if (baseNode.Config?.IntParams1?.Count >= 4)
            {
                EnemyNeverDie = (baseNode.Config?.IntParams1[3] ?? 0) == 0 ? false : true;
            }

            if (baseNode.Config?.IntParams1?.Count >= 5)
            {
                FightType = (TMapEventFightType)(baseNode.Config?.IntParams1[4] ?? 0);
            }

            //IntParam2
            baseNode.Config?.IntParams2?.ForEach(myType =>
            {
                ContinueActionType.Add((EventActionChooseConditionType)myType);
            });
        }

        public void SetDefault()
        {
            //Target1
            FriendTargets.Clear();
            baseNode.SaveConfigTarget1(FriendTargets);

            //Target2
            EnemyTargets.Clear();
            EnemyTargets.Add(new MapEventTarget(MapEventTargetType.MapEventTargetType_Leader));
            EnemyTargets.Add(new MapEventTarget(MapEventTargetType.MapEventTargetType_AllCostar));
            baseNode.SaveConfigTarget2(EnemyTargets);

            //Target3
            NoBattleTargets.Clear();
            baseNode.SaveConfigTarget3(NoBattleTargets);
        }
    }

    public class MapEventGeneralFuncConfigNode_FightInfo : INodeCustomInspector
    {
        /// <summary>
        /// 0 中立 1 敌方 2 友方
        /// </summary>
        [ShowInInspector, HideReferenceObjectPicker, LabelText("演员阵营"), EnableIf("@false"), FoldoutGroup("信息展示", 0)]
        public CampType Camp { get; private set; } = CampType.None;

        /// <summary>
        /// 演员索引
        /// </summary>
        [ShowInInspector, HideReferenceObjectPicker, LabelText("演员索引"), EnableIf("@false"), FoldoutGroup("信息展示", 0)]
        public int ActorIndex { get; private set; } = -1;

        private readonly MapEventGeneralFuncConfigNode baseNode;

        public MapEventGeneralFuncConfigNode_FightInfo(MapEventGeneralFuncConfigNode baseNode)
        {
            this.baseNode = baseNode;
        }

        [ShowInInspector, HideReferenceObjectPicker, LabelText("战斗信息")]
        [OnValueChanged("OnChangedFightInfo", true), DelayedProperty]
        public FightInfo FightInfo;

        public void OnChangedFightInfo()
        {
            baseNode.SaveConfigTarget1(FightInfo.FriendTargets);
            baseNode.SaveConfigTarget2(FightInfo.EnemyTargets);
            baseNode.SaveConfigTarget3(FightInfo.NoBattleTargets);

            baseNode.Config?.ExSetValue("IntParams1", FightInfo.ToIntParams1());
            baseNode.Config?.ExSetValue("IntParams2", FightInfo.ToIntParams2());

            UpdateTempData();

            CheckError();
        }

        public void CheckError()
        {
            baseNode.InspectorError = string.Empty;

            if (FightInfo != default && FightInfo.FightType == TMapEventFightType.TMapEventFightType_Normal)
            {
                baseNode.AddInspectorErrorTargetSame(FightInfo.FriendTargets, FightInfo.EnemyTargets);

                baseNode.AddInspectorErrorTableNotSelect(FightInfo.BattleTableDatas);
            }

            bool isContainB_V = false;
            bool isContainB_F = false;
            FightInfo.ContinueActionType?.Foreach(actionType =>
            {
                if (actionType == EventActionChooseConditionType.TCCT_BATTLE_V)
                {
                    isContainB_V = true;
                }
                else if (actionType == EventActionChooseConditionType.TCCT_BATTLE_F)
                {
                    isContainB_F = true;
                }
            });
            if (isContainB_V && isContainB_F)
            {
                baseNode.InspectorError += "停留当前行为冲突\n";
            }

            //停留当前阶段
            //战斗胜利 演员是友方=不能配演员战斗胜利 敌方=不能配演员战斗失败
            //战斗失败 演员是友方=不能配演员战斗失败 敌方=不能配演员战斗胜利
            if (FightInfo.ContinueActionType.Count > 0)
            {
                foreach (var childNode in baseNode.GetParentNodeAllChildren<MapEventGeneralFuncConfigNode>())
                {
                    if(childNode == default || childNode.Config == default || childNode == this.baseNode) {  continue; }

                    if(childNode.Config.GroupConditionType == default)
                    {
                        continue;
                    }

                    var actorKeepBattleState = EventActionChooseConditionType.TCCT_NULL;
                    //战斗胜利
                    if (FightInfo.ContinueActionType.Contains(EventActionChooseConditionType.TCCT_BATTLE_V))
                    {
                        actorKeepBattleState = EventActionChooseConditionType.TCCT_BATTLE_ACTOR_V;
                    }
                    //战斗失败
                    else if (FightInfo.ContinueActionType.Contains(EventActionChooseConditionType.TCCT_BATTLE_F))
                    {
                        actorKeepBattleState = EventActionChooseConditionType.TCCT_BATTLE_ACTOR_F;
                    }
                    else
                    {
                        continue;
                    }

                    if (childNode.Config.GroupConditionType.Contains(actorKeepBattleState))
                    {
                        if (actorKeepBattleState == EventActionChooseConditionType.TCCT_BATTLE_ACTOR_F)
                        {
                            baseNode.InspectorError += $"因为停留当前行为类型 节点{childNode.Config.ID} 只能设置成【{EventActionChooseConditionType.TCCT_BATTLE_ACTOR_V.GetDescription(false)}】\n";
                        }
                        else if (actorKeepBattleState == EventActionChooseConditionType.TCCT_BATTLE_ACTOR_V)
                        {
                            baseNode.InspectorError += $"因为停留当前行为类型 节点{childNode.Config.ID} 只能设置成【{EventActionChooseConditionType.TCCT_BATTLE_ACTOR_F.GetDescription(false)}】\n";
                        }
                    }
                }
            }
        }

        public void ConfigToData()
        {
            FightInfo = new FightInfo(baseNode);
            FightInfo.ConfigToData();

            UpdateTempData();
        }

        public void SetDefault()
        {
            FightInfo ??= new FightInfo(baseNode);
            FightInfo.SetDefault();
        }

        public bool IsInTarget(List<MapEventTarget> Targets)
        {
            foreach (var target in Targets)
            {
                if (target.TargetType == MapEventTargetType.MapEventTargetType_AllCostar && ActorIndex > 0)
                {
                    return true;
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_Leader && ActorIndex == 0)
                {
                    return true;
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_SpecificActor && ActorIndex == target.TargetIndex)
                {
                    return true;
                }
                else if (target.TargetType == MapEventTargetType.MapEventTargetType_MineActor)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateTempData()
        {
            var linkNode = baseNode.GetLoopPreviousNode<NpcEventLinkConfigNode>();
            ActorIndex = linkNode?.GetInParentNodeIndex() ?? -1;

            if (ActorIndex == -1) { return; }

            if (IsInTarget(FightInfo.EnemyTargets))
            {
                Camp = CampType.Enemy;
            }

            if (IsInTarget(FightInfo.FriendTargets))
            {
                Camp = CampType.Friend;
            }
        }
    }
}

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
    /// 通用功能节点
    /// </summary>
    public partial class MapEventGeneralFuncConfigNode
    {
        /// <summary>
        /// 类型映射
        /// </summary>
        private Dictionary<MapEventGeneralFuncType, Type> generalFuncTypeMapping = new Dictionary<MapEventGeneralFuncType, Type>()
        {
            { MapEventGeneralFuncType.MEGFT_Modify_GameTag, typeof(MapEventGeneralFuncConfigNode_Modify_GameTag) },
            { MapEventGeneralFuncType.MEGFT_ItemTransfer, typeof(MapEventGeneralFuncConfigNode_ItemTransfer) },
            { MapEventGeneralFuncType.MEGFT_AwardDrop, typeof(MapEventGeneralFuncConfigNode_AwardDrop) },
            { MapEventGeneralFuncType.MEGFT_Dead, typeof(MapEventGeneralFuncConfigNode_Dead) },
            { MapEventGeneralFuncType.MEGFT_ActorFavor, typeof(MapEventGeneralFuncConfigNode_ActorFavor) },
            { MapEventGeneralFuncType.MEGFT_HouseFavor, typeof(MapEventGeneralFuncConfigNode_CampFavor) },
            { MapEventGeneralFuncType.MEGFT_FightInfo, typeof(MapEventGeneralFuncConfigNode_FightInfo) },
            { MapEventGeneralFuncType.MEGFT_OpenWindow, typeof(MapEventGeneralFuncConfigNode_OpenWindow) },
            { MapEventGeneralFuncType.MEGFT_DEMONINVASION, typeof(MapEventGeneralFuncConfigNode_DemonInvasion) },
            { MapEventGeneralFuncType.MEGFT_MODIFY_GODEVIL, typeof(MapEventGeneralFuncConfigNode_ModifyGodEvil) },
            { MapEventGeneralFuncType.MEGFT_NpcNeedItemInfo, typeof(MapEventGeneralFuncConfigNode_NpcNeedItem) },
            { MapEventGeneralFuncType.MEGFT_EvolutionHeadline, typeof(MapEventGeneralFuncConfigNode_EvolutionHeadline) },
            { MapEventGeneralFuncType.MEGFT_SetNpcState, typeof(MapEventGeneralFuncConfigNode_SetNpcStatus) },
            { MapEventGeneralFuncType.MEGFT_PlayerAsset, typeof(MapEventGeneralFuncConfigNode_PlayerAssetType) },
            { MapEventGeneralFuncType.MEGFT_AddMapRes, typeof(MapEventGeneralFuncConfigNode_AddMapRes) },
            { MapEventGeneralFuncType.MEGFT_Encounter_Battle_Award, typeof(MapEventGeneralFuncConfigNode_EncounterBattleAward) },
            { MapEventGeneralFuncType.MEGFT_ENCOUNTER_AUCTION_INFO, typeof(MapEventGeneralFuncConfigNode_EncounterAuctionInfo) },
            { MapEventGeneralFuncType.MEGFT_OPEN_STORY_ARENA, typeof(MapEventGeneralFuncConfigNode_OpenStoryArena) },
            { MapEventGeneralFuncType.MEGFT_SET_VARIABLE, typeof(MapEventGeneralFuncConfigNode_SetVariable) },
            { MapEventGeneralFuncType.MEGFT_Set_StoryEnd, typeof(MapEventGeneralFuncConfigNode_SetStoryEnd) },
            { MapEventGeneralFuncType.MEGFT_ShoperDrop, typeof(MapEventGeneralFuncConfigNode_ShoperDrop) },
            { MapEventGeneralFuncType.MEGFT_Destiny, typeof(MapEventGeneralFuncConfigNode_Destiny) },
            { MapEventGeneralFuncType.MEGFT_RunBusinessItemChange, typeof(MapEventGeneralFuncConfigNode_RunBusinessItemChange) },
            { MapEventGeneralFuncType.MEGFT_Cost_OT, typeof(MapEventGeneralFuncConfigNode_CostOrinTreasure) },
            { MapEventGeneralFuncType.MEGFT_SetLifePath,  typeof(MapEventGeneralFuncConfigNode_LifePath) },
            { MapEventGeneralFuncType.MEGFT_YINQI_BATTLE_SETT, typeof(MapEventGeneralFuncConfigNode_YinQi_Battle_Set) },
            { MapEventGeneralFuncType.MEGFT_DEMONINVASION_POMUP, typeof(MapEventGeneralFuncConfigNode_DemonInvasionMopUp) },
            { MapEventGeneralFuncType.MEGFT_KanYu, typeof(MapEventGeneralFuncConfigNode_KanYu) },
            { MapEventGeneralFuncType.MEGFT_QiQiu, typeof(MapEventGeneralFuncConfigNode_QiQiu) },
            { MapEventGeneralFuncType.MEGFT_MapPosEntity, typeof(MapEventGeneralFuncConfigNode_MapPosEntity) },

        };

        [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, HideLabel, ShowIf("@generalFuncType != MapEventGeneralFuncType.MEGFT_Null")]
        [InfoBox("@InspectorError", InfoMessageType.Error, "IsExitInspectorError")]
        [FoldoutGroup("参数设置", true, order:1)]
        public INodeCustomInspector NodeCustomInspector { get; private set; }

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            var title = $"[{Config.ID}][功能][{EnumUtility.GetDescription(Config.GeneralFuncType, false)}]";
            
            var conditions = string.Empty;
            Config?.GroupConditionType?.ForEach(condtionType =>
            {
                conditions += $"{EnumUtility.GetDescription(condtionType, false)} ";
            });

            if (!string.IsNullOrEmpty(conditions))
            {
                title = $"{title}[{conditions}]";
            }

            SetCustomName(title);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            ConfigToData();

            ConfigToGroupConditionType();
        }

        public override bool OnSaveCheck()
        {
            if (!base.OnSaveCheck()) { return false; }

            NodeCustomInspector?.CheckError();

            if (IsExitInspectorError)
            {
                AppendSaveMapEventRet(InspectorError);
                return false;
            }

            return true;
        }

        #region 功能类型
        [Sirenix.OdinInspector.ShowInInspector, LabelText("类型"), HideReferenceObjectPicker]
        [FoldoutGroup("功能类型", true, 0), GUIColor(0f, 1f, 0f, 1f)]
        [ValueDropdown("GetFuncValueDropdown"), OnValueChanged("OnGeneralFuncTypeChanged"), DelayedProperty]
        private MapEventGeneralFuncType generalFuncType = MapEventGeneralFuncType.MEGFT_Null;

        private IEnumerable<ValueDropdownItem> GetFuncValueDropdown()
        {
            //for event
            var funGroupNode = GetPreviousNode<MapEventGeneralFuncGroupConfigNode>();
            var eventNode = funGroupNode?.GetPreviousNode<NpcEventConfigNode>();
            if(eventNode != default)
            {
                foreach (var funType in VD_GeneralFuncTypeForEventNode)
                {
                    yield return new ValueDropdownItem(funType.GetDescription(false), funType);
                }
            }
            else
            {
                foreach (var funType in VD_GeneralFuncTypeForOption)
                {
                    yield return new ValueDropdownItem(funType.GetDescription(false), funType);
                }
            }
        }

        private void OnGeneralFuncTypeChanged()
        {
            Config?.ExSetValue("GeneralFuncType", generalFuncType);

            OnRefreshCustomName();

            if(generalFuncTypeMapping.TryGetValue(generalFuncType, out var inspectorType))
            {
                NodeCustomInspector = Activator.CreateInstance(inspectorType, this) as INodeCustomInspector;
                NodeCustomInspector.SetDefault();
                NodeCustomInspector.CheckError();
            }
        }

        private void ConfigToData()
        {
            generalFuncType = Config?.GeneralFuncType ?? MapEventGeneralFuncType.MEGFT_Null;

            if (generalFuncTypeMapping.TryGetValue(generalFuncType, out var inspectorType))
            {
                NodeCustomInspector ??= Activator.CreateInstance(inspectorType, this) as INodeCustomInspector;
                NodeCustomInspector.ConfigToData();
                NodeCustomInspector.CheckError();
            }
        }
        #endregion

        #region 行为组条件
        [Sirenix.OdinInspector.ShowInInspector, LabelText("类型"), HideReferenceObjectPicker, ShowIf("@IsShowGroupConditionType")]
        [FoldoutGroup("条件组类型", true, 0)]
        [ValueDropdown("GetGroupValueDropdown"), OnValueChanged("OnGroupConditionTypeChanged", true), DelayedProperty]
        private List<EventActionChooseConditionType> groupConditionType = new List<EventActionChooseConditionType>();

        /// <summary>
        /// 是否显示条件组类型
        /// </summary>
        private bool IsShowGroupConditionType
        {
            get
            {
                if ((OptionType == TNpcEventDialogOptionType.TNEDOT_BATTLE || OptionType == TNpcEventDialogOptionType.TNEDOT_MINIGAME)
                    && generalFuncType != MapEventGeneralFuncType.MEGFT_FightInfo
                    && generalFuncType != MapEventGeneralFuncType.MEGFT_OPEN_STORY_ARENA
                    && generalFuncType != MapEventGeneralFuncType.MEGFT_NpcNeedItemInfo
                    && generalFuncType != MapEventGeneralFuncType.MEGFT_OpenWindow
                    && generalFuncType != MapEventGeneralFuncType.MEGFT_Null)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 获取条件组选项
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetGroupValueDropdown()
        {
            var optionType = OptionType;
            if (optionType == TNpcEventDialogOptionType.TNEDOT_BATTLE)
            {
                foreach (var item in VD_GroupTypeForFight)
                {
                    yield return new ValueDropdownItem(item.GetDescription(false), item);
                }
            }
            else if(optionType == TNpcEventDialogOptionType.TNEDOT_MINIGAME)
            {
                foreach (var item in VD_GroupTypeForMiniGame)
                {
                    yield return new ValueDropdownItem(item.GetDescription(false), item);
                }           
            }
        }

        /// <summary>
        /// 获取条件组类型
        /// </summary>
        /// <returns></returns>
        private TNpcEventDialogOptionType OptionType
        {
            get
            {
                var optionNode = GetLoopPreviousNode<NpcTalkOptionConfigNode>();
                return optionNode?.Config?.NpcEventDialogOptionType ?? TNpcEventDialogOptionType.TNEDOT_NULL;
            }
        }

        private void OnGroupConditionTypeChanged()
        {
            var newType = new List<EventActionChooseConditionType>();
            groupConditionType?.ForEach(conditionType =>
            {
                newType.Add(conditionType);
            });

            Config?.ExSetValue("GroupConditionType", newType);

            OnRefreshCustomName();
        }

        private void ConfigToGroupConditionType()
        {
            var newType = new List<EventActionChooseConditionType>();
            Config?.GroupConditionType?.ForEach(conditionType =>
            {
                newType.Add(conditionType);
            });

            groupConditionType = newType;
        }
        #endregion

        #region 目标
        public void RestoreTargets(IReadOnlyList<MapEventTarget> configTargets, List<MapEventTarget> inspectorTargets)
        {
            inspectorTargets.Clear();
            configTargets?.ForEach(target =>
            {
                inspectorTargets.Add(target.Clone());
            });
        }

        public void SaveConfigTarget1(List<MapEventTarget> inspectorTargets)
        {
            var targets = new List<MapEventTarget>();
            inspectorTargets?.ForEach(target =>
            {
                targets.Add(target.Clone());
            });
            SetConfigValue(nameof(Config.Target1), targets);
        }

        public void SaveConfigTarget2(List<MapEventTarget> inspectorTargets)
        {
            var targets = new List<MapEventTarget>();
            inspectorTargets?.ForEach(target =>
            {
                targets.Add(target.Clone());
            });
            SetConfigValue(nameof(Config.Target2), targets);
        }

        public void SaveConfigTarget3(List<MapEventTarget> inspectorTargets)
        {
            var targets = new List<MapEventTarget>();
            inspectorTargets?.ForEach(target =>
            {
                targets.Add(target.Clone());
            });
            SetConfigValue(nameof(Config.Target3), targets);
        }
        #endregion
    }
}

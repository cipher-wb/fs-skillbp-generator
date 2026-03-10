using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 定义其他结构
    /// </summary>
    public partial class MapEventGeneralFuncConfigNode
    {
        #region ValueDropdown
        public readonly static ValueDropdownList<int> VDL_State = new ValueDropdownList<int>
        {
            {"凡人境",1},{"炼气期",2},
            {"筑基前期",3},{"筑基中期",4},{"筑基后期",5},
            {"淬灵前期",6},{"淬灵中期",7},{"淬灵后期",8},
            {"金丹前期",9},{"金丹中期",10},{"金丹后期",11},
            {"元婴前期",12},{"元婴中期",13},{"元婴后期",14},
            {"化神前期",15},{"化神中期",16},{"化神后期",17},
        };

        public readonly static ValueDropdownList<TMapResType> VD_MapResType = new ValueDropdownList<TMapResType>()
        {
            {TMapResType.TMRT_BOX.GetDescription(false), TMapResType.TMRT_BOX},
        };

        public readonly static ValueDropdownList<TPlayerAssetType> VD_PlayerAsset = new ValueDropdownList<TPlayerAssetType>()
        {
            {TPlayerAssetType.TPAT_SILVER.GetDescription(false), TPlayerAssetType.TPAT_SILVER},
            {TPlayerAssetType.TPAT_STEMINA.GetDescription(false), TPlayerAssetType.TPAT_STEMINA},
        };

        public static ValueDropdownList<SymbolType> vd_ChangeFavorType = new ValueDropdownList<SymbolType>()
        {
            {"+",SymbolType.Add},
            {"=",SymbolType.Set},
        };

        public static ValueDropdownList<SymbolType> vd_RunBusinessItemType = new ValueDropdownList<SymbolType>()
        {
            {"+",SymbolType.Add},
            {"-",SymbolType.Reduce},
        };

        public static ValueDropdownList<SymbolType> vd_MapPosEntityOptType = new ValueDropdownList<SymbolType>()
        {
            {"+",SymbolType.Add},
            {"-",SymbolType.Reduce},
        };

        public static ValueDropdownList<SymbolType> vd_VariableType = new ValueDropdownList<SymbolType>()
        {
            {"=",SymbolType.Set},
            {"+",SymbolType.Add},
            {"-",SymbolType.Reduce},
            {"x",SymbolType.Multi},
            {"/",SymbolType.Div},
        };

        public static List<EventActionChooseConditionType> VD_GroupTypeForFight = new List<EventActionChooseConditionType>()
        {
            {EventActionChooseConditionType.TCCT_BATTLE_ACTOR_V},
            {EventActionChooseConditionType.TCCT_BATTLE_ACTOR_F},
            {EventActionChooseConditionType.TCCT_BATTLE_ACTOR_R},
        };

        public static List<EventActionChooseConditionType> VD_GroupTypeForMiniGame = new List<EventActionChooseConditionType>()
        {
            {EventActionChooseConditionType.TCCT_MINIGAME_FAIL},
            {EventActionChooseConditionType.TCCT_MINIGAME_SUC},
        };

        public static List<MapEventGeneralFuncType> VD_GeneralFuncTypeForEventNode = new List<MapEventGeneralFuncType>()
        {
            {MapEventGeneralFuncType.MEGFT_Null},
            {MapEventGeneralFuncType.MEGFT_EvolutionHeadline},
            {MapEventGeneralFuncType.MEGFT_SetLifePath},
            {MapEventGeneralFuncType.MEGFT_Dead},
            {MapEventGeneralFuncType.MEGFT_SET_VARIABLE},
        };

        public static List<EventActionChooseConditionType> VD_FightInfoContinue = new List<EventActionChooseConditionType>()
        {
            {EventActionChooseConditionType.TCCT_BATTLE_V},
            {EventActionChooseConditionType.TCCT_BATTLE_F},
        };

        public static List<MapEventGeneralFuncType> VD_GeneralFuncTypeForOption = new List<MapEventGeneralFuncType>()
        {
            {MapEventGeneralFuncType.MEGFT_Null},
            {MapEventGeneralFuncType.MEGFT_Modify_GameTag},
            {MapEventGeneralFuncType.MEGFT_ItemTransfer},
            {MapEventGeneralFuncType.MEGFT_AwardDrop},
            {MapEventGeneralFuncType.MEGFT_Dead},
            {MapEventGeneralFuncType.MEGFT_ActorFavor},
            {MapEventGeneralFuncType.MEGFT_HouseFavor},
            {MapEventGeneralFuncType.MEGFT_FightInfo},
            {MapEventGeneralFuncType.MEGFT_OpenWindow},
            {MapEventGeneralFuncType.MEGFT_DEMONINVASION},
            {MapEventGeneralFuncType.MEGFT_MODIFY_GODEVIL},
            {MapEventGeneralFuncType.MEGFT_NpcNeedItemInfo},
            {MapEventGeneralFuncType.MEGFT_SetNpcState},
            {MapEventGeneralFuncType.MEGFT_PlayerAsset},
            {MapEventGeneralFuncType.MEGFT_AddMapRes},
            {MapEventGeneralFuncType.MEGFT_Encounter_Battle_Award },
            {MapEventGeneralFuncType.MEGFT_ENCOUNTER_AUCTION_INFO },
            {MapEventGeneralFuncType.MEGFT_OPEN_STORY_ARENA },
            {MapEventGeneralFuncType.MEGFT_SET_VARIABLE },
            {MapEventGeneralFuncType.MEGFT_Set_StoryEnd },
            {MapEventGeneralFuncType.MEGFT_ShoperDrop },
            {MapEventGeneralFuncType.MEGFT_Destiny },
            {MapEventGeneralFuncType.MEGFT_RunBusinessItemChange },
            {MapEventGeneralFuncType.MEGFT_Cost_OT },
            {MapEventGeneralFuncType.MEGFT_YINQI_BATTLE_SETT},
            {MapEventGeneralFuncType.MEGFT_DEMONINVASION_POMUP},
            {MapEventGeneralFuncType.MEGFT_KanYu },
            {MapEventGeneralFuncType.MEGFT_QiQiu},
            {MapEventGeneralFuncType.MEGFT_MapPosEntity},
        };

        public static ValueDropdownList<TransferItemType> VD_TransferItemType = new ValueDropdownList<TransferItemType>()
        {
            {"指定Item", TransferItemType.SpecialItem},
            {"指定QuestSubmitItem", TransferItemType.SpecialQuestSubmitItem},
            {"全背包随机", TransferItemType.BagRandom},
        };

        public static HashSet<MapEventGeneralFuncType> AwardFuncType = new HashSet<MapEventGeneralFuncType>()
        {
            {MapEventGeneralFuncType.MEGFT_AwardDrop},
            {MapEventGeneralFuncType.MEGFT_Encounter_Battle_Award},
            {MapEventGeneralFuncType.MEGFT_SET_VARIABLE},
            {MapEventGeneralFuncType.MEGFT_ShoperDrop},
            {MapEventGeneralFuncType.MEGFT_AwardDrop},
            {MapEventGeneralFuncType.MEGFT_AwardDrop},

        };
        #endregion

        #region Struct Or Class
        [Serializable]
        public struct ModifyGodEvilData
        {
            [LabelText("设置")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.vd_ChangeFavorType")]
            public ModifyType ModifyType;

            [LabelText("数值")]
            public int ChangeValue;

            public ModifyGodEvilData(ModifyType type, int value)
            {
                ModifyType = type;
                ChangeValue = value;
            }
        }

        [Serializable]
        public struct ChangeFavorData
        {
            [LabelText("设置")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.vd_ChangeFavorType")]
            public SymbolType ChangeType;

            [LabelText("数值")]
            public int ChangeValue;

            public ChangeFavorData(SymbolType type, int value)
            {
                ChangeType = type;
                ChangeValue = value;
            }
        }

        [Serializable]
        public struct STPlayerAssetData
        {
            [LabelText("类型")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.VD_PlayerAsset")]
            public TPlayerAssetType AssetType;

            [LabelText("数值")]
            public int ChangeValue;

            public STPlayerAssetData(TPlayerAssetType assetType, int value)
            {
                AssetType = assetType;
                ChangeValue = value;
            }
        }

        public class AddMapResData
        {
            [LabelText("物产类型")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.VD_MapResType")]
            public TMapResType MapResType;

            [LabelText("物产表"), HideReferenceObjectPicker]
            public TableSelectData MapResTable;

            [LabelText("数量"), MinValue(0)]
            public int Count;

            /// <summary>
            /// 0 相对坐标 1 绝对坐标
            /// </summary>
            [LabelText("坐标类型")]
            public CoordType PosType;

            /// <summary>
            /// 相对坐标：MapEventTargetType
            /// 绝对坐标：MapID
            /// </summary>
            [LabelText("目标类型"), ShowIf("@PosType == CoordType.Relative")]
            public MapEventTarget Target = new MapEventTarget(MapEventTargetType.MapEventTargetType_MineActor);

            [LabelText("地图ID"), ShowIf("@PosType == CoordType.Absolute")]
            public int MapID;

            /// <summary>
            /// 相对坐标：OffsetX
            /// 绝对坐标：PosX
            /// </summary>
            [LabelText("@GetPosXTitle()")]
            public int PosX;

            /// <summary>
            /// 相对坐标：OffsetY
            /// 绝对坐标：PosY
            /// </summary>
            [LabelText("@GetPosYTitle()")]
            public int PosY;

            /// <summary>
            /// 相对坐标：随机半径
            /// 绝对坐标：随机半径
            /// </summary>
            [LabelText("随机半径"), PropertyTooltip("0：不随机 其他：X、Y取随机值")]
            public int Range;

            /// <summary>
            /// 转换成参数列表
            /// </summary>
            /// <returns></returns>
            public List<int> ToIntParams1()
            {
                var paramlist = new List<int>();
                paramlist.Add((int)MapResType);
                paramlist.Add(MapResTable.ID);
                paramlist.Add(Count);
                paramlist.Add((int)PosType);

                if (PosType == CoordType.Relative)
                {
                    paramlist.Add((int)Target.TargetType);
                    paramlist.Add(Target.TargetIndex);
                }
                else
                {
                    paramlist.Add(MapID);
                    paramlist.Add(0);
                }

                paramlist.Add(PosX);
                paramlist.Add(PosY);
                paramlist.Add(Range);
                return paramlist;
            }

            public void Update(IReadOnlyList<int> paramlist)
            {
                if(paramlist?.Count != 9) { return; }

                MapResType = (TMapResType)paramlist[0];

                if(MapResType == TMapResType.TMRT_BOX)
                {
                    MapResTable = new TableSelectData(typeof(MapBoxConfig).FullName, paramlist[1]);
                }
                MapResTable.OnSelectedID();

                Count = paramlist[2];
                PosType = (CoordType)paramlist[3];

                if(PosType == CoordType.Relative)
                {
                    Target.Update((MapEventTargetType)paramlist[4], paramlist[5]);
                }
                else
                {
                    MapID = paramlist[4];
                }

                PosX = paramlist[6];
                PosY = paramlist[7];
                Range = paramlist[8];
            }

            private string GetPosXTitle()
            {
                return PosType switch
                {
                    CoordType.Relative => "坐标X偏移",
                    CoordType.Absolute => "坐标X",
                    _ => "Error",
                };
            }

            private string GetPosYTitle()
            {
                return PosType switch
                {
                    CoordType.Relative => "坐标Y偏移",
                    CoordType.Absolute => "坐标Y",
                    _ => "Error",
                };
            }
        }

        public class EncounterBattleAwardData
        {
            private HashSet<int> dropIDSet = new HashSet<int>();

            [LabelText("分级"), EnableIf("@false")]
            public TBattleMissionGradeType GradeType;

            [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("掉落表")]
            [ValueDropdown("OnDropTableDataAdd")]
            public int DropID;

            [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("展示道具")]
            [ListDrawerSettings(CustomAddFunction = "OnAddItemTable")]
            public TableSelectData ItemData;

            public EncounterBattleAwardData(TBattleMissionGradeType gradeType, int dropID, int itemID)
            {
                GradeType = gradeType;

                DropID = dropID;

                ItemData = new TableSelectData(typeof(ItemConfig).FullName, itemID);
                ItemData.OnSelectedID();
            }

            private IEnumerable<ValueDropdownItem> OnDropTableDataAdd()
            {
                var dropItems = DropConfigManager.Instance.ItemArray.Items;
                dropIDSet.Clear();

                foreach (var dropItem in dropItems)
                {
                    if (!dropIDSet.Contains(dropItem.DropID))
                    {
                        dropIDSet.Add(dropItem.DropID);
                        yield return new ValueDropdownItem(dropItem.DropID.ToString(), dropItem.DropID);
                    }
                }
            }

            private TableSelectData OnAddItemTable()
            {
                return new TableSelectData(typeof(ItemConfig).FullName, 0);
            }

            public GFDynamic ToGFDynamic()
            {
                return new GFDynamic((int)GradeType, DropID, ItemData.ID);
            }
        }

        public class SetVariableData
        {
            [HideReferenceObjectPicker, LabelText("变量设置·")]
            [ListDrawerSettings(CustomAddFunction = "OnAddVariableTable")]
            public TableSelectData VariableData;

            [LabelText("数值")]
            public int Value;

            [LabelText("设置")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.vd_VariableType")]
            public SymbolType SymbolType;

            public SetVariableData(int variableID, int value, SymbolType symbolType)
            {
                Value = value;
                SymbolType = symbolType;

                VariableData = new TableSelectData(typeof(TagConfig).FullName, variableID);
                VariableData.OnSelectedID();
            }

            private TableSelectData OnAddVariable()
            {
                return new TableSelectData(typeof(TagConfig).FullName, 0);
            }

            public GFDynamic ToGFDynamic()
            {
                return new GFDynamic(VariableData.ID, Value, (int)SymbolType);
            }
        }

        public class SetRunBuisnessItemData
        {
            [LabelText("设置")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.vd_RunBusinessItemType")]
            public SymbolType SymbolType;

            [LabelText("类型")]
            public ValueType ValueType;

            [LabelText("最小值")]
            public int ValueMin;

            [LabelText("最大值")]
            public int ValueMax;

            [LabelText("掉落类型")]
            public TDropInfoPushType PushType;

            public SetRunBuisnessItemData()
            {
                SymbolType = SymbolType.Add;
                ValueType = ValueType.Value;
                ValueMin = 1;
                ValueMax = 100;
                PushType = TDropInfoPushType.TD_Toast;
            }

            public SetRunBuisnessItemData(IReadOnlyList<int> intParams1)
            {
                if(intParams1?.Count > 0)
                {
                    SymbolType = (SymbolType)intParams1[0];
                }
                if (intParams1?.Count > 1)
                {
                    ValueType = (ValueType)intParams1[1];
                }
                if (intParams1?.Count > 2)
                {
                    ValueMin = intParams1[2];
                }
                if (intParams1?.Count > 3)
                {
                    ValueMax = intParams1[3];
                }
                if (intParams1?.Count > 4)
                {
                    PushType = (TDropInfoPushType)intParams1[4];
                }
            }

            public List<int> ToIntParams1()
            {
                return new List<int>() { (int)SymbolType, (int)ValueType, ValueMin, ValueMax, (int)PushType };
            }
        }

        public class TransferItemData
        {
            private HashSet<int> itemMarqueGroupSet = new HashSet<int>();

            [LabelText("类型"), OnValueChanged("OnChangeTransferType")]
            [ValueDropdown("@MapEventGeneralFuncConfigNode.VD_TransferItemType")]
            public TransferItemType TransferType;

            [LabelText("道具"), HideReferenceObjectPicker, HideIf("@TransferType == TransferItemType.BagRandom")]
            public TableSelectData ItemTable;

            [LabelText("道具数量")]
            public int ItemCount;

            [Sirenix.OdinInspector.ShowInInspector, HideReferenceObjectPicker, LabelText("跑马灯GroupID")]
            [ValueDropdown("OnAddItemMarqueGroup")]
            public int ItemMarqueGroupID;

            public TransferItemData()
            {
                TransferType = TransferItemType.SpecialItem;
                ItemCount = 1;
                ItemMarqueGroupID = 0;

                OnChangeTransferType();
            }

            public TransferItemData(IReadOnlyList<int> intParams1)
            {
                if (intParams1?.Count > 0)
                {
                    TransferType = (TransferItemType)intParams1[0];
                }
                if (intParams1?.Count > 1)
                {
                    var itemID = intParams1[1];
                    if (TransferType == TransferItemType.SpecialItem)
                    {
                        ItemTable = new TableSelectData(typeof(ItemConfig).FullName, itemID);
                    }
                    else if (TransferType == TransferItemType.SpecialQuestSubmitItem)
                    {
                        ItemTable = new TableSelectData(typeof(CommonSubmitItemConfig).FullName, itemID);
                    }
                    else
                    {
                        ItemTable = default;
                    }
                    ItemTable?.OnSelectedID();
                }
                if (intParams1?.Count > 2)
                {
                    ItemCount = intParams1[2];
                }

                if (intParams1?.Count > 3)
                {
                    ItemMarqueGroupID = intParams1[3];
                }
            }

            public List<int> ToIntParams1()
            {
                return new List<int>() { (int)TransferType, ItemTable?.ID ?? 0, ItemCount, ItemMarqueGroupID };
            }

            private void OnChangeTransferType()
            {
                if (TransferType == TransferItemType.SpecialItem)
                {
                    ItemTable = new TableSelectData(typeof(ItemConfig).FullName, 0);
                }
                else if (TransferType == TransferItemType.SpecialQuestSubmitItem)
                {
                    ItemTable = new TableSelectData(typeof(CommonSubmitItemConfig).FullName, 0);
                }
                else
                {
                    ItemTable = default;
                }

                ItemTable?.OnSelectedID();
            }

            private IEnumerable<ValueDropdownItem> OnAddItemMarqueGroup()
            {
                itemMarqueGroupSet.Clear();

                var itemMarquees = ItemMarqueeConfigManager.Instance.ItemArray.Items;
                foreach (var itemMarque in itemMarquees)
                {
                    if (!itemMarqueGroupSet.Contains(itemMarque.GroupID))
                    {
                        itemMarqueGroupSet.Add(itemMarque.GroupID);
                        yield return new ValueDropdownItem(itemMarque.GroupID.ToString(), itemMarque.GroupID);
                    }
                }
            }
        }

        public class SetPathLifeData
        {
            private readonly MapEventGeneralFuncConfigNode_LifePath baseNode;

            [HideReferenceObjectPicker, LabelText("结束剧情ID")]
            [ValueDropdown("GetStoryEndID")]
            public int StoryEndID;

            [HideReferenceObjectPicker, LabelText("经历表")]
            public TableSelectData LiftPathTable;

            [HideReferenceObjectPicker, LabelText("道具表")]
            public TableSelectData ItemTable;

            public SetPathLifeData(MapEventGeneralFuncConfigNode_LifePath node, IReadOnlyList<int> intParams1 = default)
            {
                baseNode = node;

                //结束剧情ID
                if (intParams1?.Count > 0)
                {
                    StoryEndID = intParams1[0];
                }

                var lifePathID = intParams1?.Count > 1 ? intParams1[1] : 0;
                LiftPathTable = new TableSelectData(typeof(LifePathTextConfig).FullName, lifePathID);
                LiftPathTable.OnSelectedID();

                var itemID = intParams1?.Count > 2 ? intParams1[2] : 0;
                ItemTable = new TableSelectData(typeof(ItemConfig).FullName, itemID);
                ItemTable.OnSelectedID();
            }

            public List<int> ToIntParams1()
            {
                return new List<int>() { StoryEndID, LiftPathTable?.ID ?? 0, ItemTable?.ID ?? 0 };
            }

            private IEnumerable<ValueDropdownItem> GetStoryEndID()
            {
                if (baseNode.UsableEndStoryNodes == default)
                {
                    yield break;
                }

                foreach (var node in baseNode.UsableEndStoryNodes)
                {
                    yield return new ValueDropdownItem($"{node.Config.ID}-{node.Config.TitleEditor}", node.Config.ID);
                }
            }
        }

        #endregion

        #region Enum
        /// <summary>
        /// 常规符号
        /// </summary>
        public enum SymbolType
        {
            Add,
            Set,
            Reduce,
            Multi,
            Div,
        }

        /// <summary>
        /// 改变正魔值类型
        /// </summary>
        public enum ModifyType
        {
            Add,
            Set,
        }

        /// <summary>
        /// 功能条件组类型
        /// </summary>
        public enum GFGroupType
        {
            None,
            Fight,
            MiniGame,
        }

        public enum CoordType
        {
            [LabelText("相对坐标")]
            Relative,
            [LabelText("绝对坐标")]
            Absolute,
        }

        public enum AwardCountType
        {
            [LabelText("给一个")]
            One,
            [LabelText("给全部")]
            All,
        }

        /// <summary>
        /// 数值类型
        /// </summary>
        public enum ValueType
        {
            [LabelText("数值")]
            Value,

            [LabelText("百分比")]
            Percent,
        }

        public enum TransferItemType
        {
            [LabelText("指定Item")]
            SpecialItem,
            [LabelText("指定QuestSubmitItem")]
            SpecialQuestSubmitItem,
            [LabelText("全背包随机")]
            BagRandom,
        }

        public enum CampType
        {
            None = 0,
            Friend,
            Enemy,
        }
        #endregion
    }
}

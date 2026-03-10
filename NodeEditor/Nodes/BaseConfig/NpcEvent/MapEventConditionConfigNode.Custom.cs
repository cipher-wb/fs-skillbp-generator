using GraphProcessor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// MapEventConditionConfigNode自定义节点
    /// </summary>
    public partial class MapEventConditionConfigNode
    {
        /// <summary>
        /// 全局条件
        /// </summary>
        public bool IsPlayerCondition
        {
            get
            {
                var parentNode = GetPreviousNode<MapEventConditionGroupConfigNode>()?.GetPreviousNode<NpcEventConfigNode>();
                return parentNode != default;
            }
        }

        /// <summary>
        /// 自定义node名字
        /// </summary>
        protected override void OnRefreshCustomName()
        {
            SetCustomName($"[{Config.ID}][条件][{Utils.GetEnumDescription(Config.GameEntityType)}]");
        }

        /// 刷新数据
        /// </summary>
        /// <param name="edges"></param>
        protected override void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            base.OnCustomPortInput_ID(edges);

            gameEntityType = IsPlayerCondition ? GameEntityType.ET_StoryPlayer : Config.GameEntityType;
            SetConfigValue(nameof(Config.GameEntityType), gameEntityType);

            RestoreSubEntityType();

            OnRefreshCustomName();
        }

        private void RestoreSubEntityType()
        {
            if (IsShowMrtMetal) { mrtMetalSubType = (TMapMetalSubType)Config.SubGameEntityType; }
            else if (IsShowMrtPlant) { mrtPlantSubType = (TMapPlantSubType)Config.SubGameEntityType; }
            else if (IsShowMrtBox) { mrtBoxSubType = (TMapBoxSubType)Config.SubGameEntityType; }
            else if (IsShowMrtFish) { mrtFishSubType = (TMapFishSubType)Config.SubGameEntityType; }
            else if (IsShowMrtMonster) { mrtMonsterSubType = (TMapMonsterSubType)Config.SubGameEntityType; }
            else if (IsShowMrtLingQi) { mrtLingQiSubType = (TMapLingQiSubType)Config.SubGameEntityType; }
        }

        private void OnEnumChanged()
        {
            var tempSubType = 0;

            if (IsShowMrtMetal) { tempSubType = (int)mrtMetalSubType; }
            else if (IsShowMrtPlant) { tempSubType = (int)mrtPlantSubType; }
            else if (IsShowMrtBox) { tempSubType = (int)mrtBoxSubType; }
            else if (IsShowMrtFish) { tempSubType = (int)mrtFishSubType; }
            else if (IsShowMrtMonster) { tempSubType = (int)mrtMonsterSubType; }
            else if (IsShowMrtLingQi) { tempSubType = (int)mrtLingQiSubType; }

            SetConfigValue(nameof(Config.SubGameEntityType), tempSubType);

            OnRefreshCustomName();
        }

        private void OnGameEntityType()
        {
            SetConfigValue(nameof(Config.GameEntityType), gameEntityType);

            OnEnumChanged();
        }

        [BoxGroup("对象", centerLabel: true, order: 0), ShowIf("@IsPlayerCondition == false")]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("对象类型"), ValueDropdown("@VD_GameEntityType")]
        [OnValueChanged("OnGameEntityType", true)]
        private GameEntityType gameEntityType = GameEntityType.ET_Null;

        #region 子类型
        /// <summary>
        /// TET_MRT_MONSTER 怪物
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtMonster")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_MonsterSubType")]
        private TMapMonsterSubType mrtMonsterSubType = TMapMonsterSubType.TMMOST_LAND_MONSTER;
        private bool IsShowMrtMonster => Config?.GameEntityType == GameEntityType.TET_MRT_MONSTER;

        /// <summary>
        /// TET_MRT_PLANT 灵植
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtPlant")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_PlantSubType")]
        private TMapPlantSubType mrtPlantSubType = TMapPlantSubType.TMPST_PLANT;
        private bool IsShowMrtPlant => Config?.GameEntityType == GameEntityType.TET_MRT_PLANT;

        /// <summary>
        /// TET_MRT_METAL 矿物
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtMetal")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_MetalSubType")]
        private TMapMetalSubType mrtMetalSubType = TMapMetalSubType.TMMEST_METAL;
        private bool IsShowMrtMetal => Config?.GameEntityType == GameEntityType.TET_MRT_METAL;

        /// <summary>
        /// TET_MRT_FISH 鱼
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtFish")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_FishSubType")]
        private TMapFishSubType mrtFishSubType = TMapFishSubType.TMFST_FISH;
        private bool IsShowMrtFish => Config?.GameEntityType == GameEntityType.TET_MRT_FISH;

        /// <summary>
        /// TET_MRT_BOX 宝箱
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtBox")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_MapBoxSubType")]
        private TMapBoxSubType mrtBoxSubType = TMapBoxSubType.TMBST_WUXING_BOX;
        private bool IsShowMrtBox => Config?.GameEntityType == GameEntityType.TET_MRT_BOX;

        /// <summary>
        /// TET_MRT_LINGQITUAN 灵气团
        /// </summary>
        [BoxGroup("对象", centerLabel: true, order: 0)]
        [Sirenix.OdinInspector.ShowInInspector, LabelText("子类型"), ShowIf("@IsShowMrtLingQi")]
        [OnValueChanged("OnEnumChanged", true), ValueDropdown("@VD_LingQiTuanSubType")]
        private TMapLingQiSubType mrtLingQiSubType = TMapLingQiSubType.TMLQST_WUXING;
        private bool IsShowMrtLingQi => Config?.GameEntityType == GameEntityType.TET_MRT_LINGQITUAN;
        #endregion
    }
}

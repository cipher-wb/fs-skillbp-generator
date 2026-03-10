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
    /// NpcEventActionGroup自定义节点
    /// </summary>
    public partial class MapEventFormulaConfigNode
    {
        /// <summary>
        /// 转换列表
        /// </summary>
        private List<int> convertList = new List<int>();

        /// <summary>
        /// 道具指定类型
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("道具指定类型"), ValueDropdown("@TableDR.EnumUtility.VD_ItemSubType")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowItemPurify")]
        private List<ItemSubType> subItemTypeList = new List<ItemSubType>();
        private bool IsShowItemPurify => Config?.ConditionType == MapEventConditionType.MECT_ITEMPURIFY;

        /// <summary>
        /// 性别
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("性别"), ValueDropdown("@TableDR.EnumUtility.VD_TSexEnum")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowSex")]
        private List<TSexEnum> sexList = new List<TSexEnum>();
        private bool IsShowSex => Config?.ConditionType == MapEventConditionType.MECT_SEX;

        /// <summary>
        /// 品质
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("品质"), ValueDropdown("@TableDR.EnumUtility.VD_RankType")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowQuality")]
        private List<RankType> qualityList = new List<RankType>();
        private bool IsShowQuality => Config?.ConditionType == MapEventConditionType.MECT_QUALITY;

        /// <summary>
        /// 喜好
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("喜好"), ValueDropdown("@TableDR.EnumUtility.VD_TNpcGiftType")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowLike")]
        private List<TNpcGiftType> likeList = new List<TNpcGiftType>();
        private bool IsShowLike => Config?.ConditionType == MapEventConditionType.MECT_LIKE;

        /// <summary>
        /// NPC健康状态
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("NPC健康状态"), ValueDropdown("@TableDR.EnumUtility.VD_TNpcStatusType")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowNpcStatus")]
        private List<TNpcStatusType> npcStatusList = new List<TNpcStatusType>();
        private bool IsShowNpcStatus => Config?.ConditionType == MapEventConditionType.MECT_NPC_STATUS;

        /// <summary>
        /// NPC类型
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("NPC类型"), ValueDropdown("@TableDR.EnumUtility.VD_TNpcType")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowNpcType")]
        private List<TNpcType> npcTypeList = new List<TNpcType>();
        private bool IsShowNpcType => Config?.ConditionType == MapEventConditionType.MECT_NPC_TYPE;

        /// <summary>
        /// 区域玩法类型
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("区域玩法类型"), ValueDropdown("@TableDR.EnumUtility.VD_RegionGameplay")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowRegionGamePlay")]
        private List<RegionGameplay> regionGameplayList = new List<RegionGameplay>();
        private bool IsShowRegionGamePlay => Config?.ConditionType == MapEventConditionType.MECT_REGION_GAMEPLAY;

        /// <summary>
        /// 怪物等阶
        /// </summary>
        [Sirenix.OdinInspector.ShowInInspector, LabelText("怪物等阶"), ValueDropdown("@TableDR.EnumUtility.VD_TMonsterRankEnum")]
        [BoxGroup("公式", centerLabel: true, order: 0), HideReferenceObjectPicker]
        [OnValueChanged("OnEnumChanged", true), ShowIf("@IsShowMonsterRank")]
        private List<TMonsterRankEnum> monsterRankList = new List<TMonsterRankEnum>();
        private bool IsShowMonsterRank => Config?.ConditionType == MapEventConditionType.MECT_MONSTER_RANK;

        /// <summary>
        /// 切换类型 保存
        /// </summary>
        private void OnEnumChanged()
        {
            convertList.Clear();

            if (IsShowItemPurify) { subItemTypeList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowSex) { sexList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowQuality) { qualityList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowLike) { likeList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowNpcStatus) { npcStatusList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowNpcType) { npcTypeList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowRegionGamePlay) { regionGameplayList?.ForEach(converType => convertList.Add((int)converType)); }
            else if (IsShowMonsterRank) { monsterRankList?.ForEach(converType => convertList.Add((int)converType)); }

            SetConfigValue(nameof(Config.FormulaB), convertList);
        }

        /// <summary>
        /// 打开json，还原
        /// </summary>
        private void RestoreFormulaB_Enum()
        {
            if (IsShowItemPurify) { subItemTypeList.Clear(); Config?.FormulaB?.ForEach(converType => subItemTypeList.Add((ItemSubType)converType)); }
            else if (IsShowSex) { sexList.Clear(); Config?.FormulaB?.ForEach(converType => sexList.Add((TSexEnum)converType)); }
            else if (IsShowQuality) { qualityList.Clear(); Config?.FormulaB?.ForEach(converType => qualityList.Add((RankType)converType)); }
            else if (IsShowLike) { likeList.Clear(); Config?.FormulaB?.ForEach(converType => likeList.Add((TNpcGiftType)converType)); }
            else if (IsShowNpcStatus) { npcStatusList.Clear(); Config?.FormulaB?.ForEach(converType => npcStatusList.Add((TNpcStatusType)converType)); }
            else if (IsShowNpcType) { npcTypeList.Clear(); Config?.FormulaB?.ForEach(converType => npcTypeList.Add((TNpcType)converType)); }
            else if (IsShowRegionGamePlay) { regionGameplayList.Clear(); Config?.FormulaB?.ForEach(converType => regionGameplayList.Add((RegionGameplay)converType)); }
            else if (IsShowMonsterRank) { monsterRankList.Clear(); Config?.FormulaB?.ForEach(converType => monsterRankList.Add((TMonsterRankEnum)converType)); }
        }
    }
}

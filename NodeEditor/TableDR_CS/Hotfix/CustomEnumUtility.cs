#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TableDR
{
    public static class CustomEnumUtility
    {
        // 可修改的属性列表
        public static Sirenix.OdinInspector.ValueDropdownList<TBattleNatureEnum> VD_TBattleNatureEnum_Write = new ValueDropdownList<TBattleNatureEnum>();
        // 可获取的属性列表
        public static Sirenix.OdinInspector.ValueDropdownList<TBattleNatureEnum> VD_TBattleNatureEnum_Read = new ValueDropdownList<TBattleNatureEnum>();

        // 可修改的状态列表
        public static Sirenix.OdinInspector.ValueDropdownList<TEntityState> VD_TEntityState_Write = new ValueDropdownList<TEntityState>();
        // 可获取的状态列表
        public static Sirenix.OdinInspector.ValueDropdownList<TEntityState> VD_TEntityState_Read = new ValueDropdownList<TEntityState>();

        // 可获取技能槽列表
        public static Sirenix.OdinInspector.ValueDropdownList<TSkillSlotType> VD_TSkillSlotType_Read = new ValueDropdownList<TSkillSlotType>();
        // 可获取战斗子类型列表
        public static Sirenix.OdinInspector.ValueDropdownList<TEntitySubType> VD_TEntitySubType_Read = new ValueDropdownList<TEntitySubType>();

        // 可修改的常用参数列表
        public static Sirenix.OdinInspector.ValueDropdownList<TCommonParamType> VD_TCommonParamEnum_Write = new ValueDropdownList<TCommonParamType>();
        // 可获取的常用参数列表
        public static Sirenix.OdinInspector.ValueDropdownList<TCommonParamType> VD_TCommonParamEnum_Read = new ValueDropdownList<TCommonParamType>();

        // 可修改的常用参数列表
        public static Sirenix.OdinInspector.ValueDropdownList<TCommonSkillParamType> VD_TCommonSkillParamEnum_Write = new ValueDropdownList<TCommonSkillParamType>();
        // 可获取的常用参数列表
        public static Sirenix.OdinInspector.ValueDropdownList<TCommonSkillParamType> VD_TCommonSkillParamEnum_Read = new ValueDropdownList<TCommonSkillParamType>();
    }
}
#endif
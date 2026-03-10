#if UNITY_EDITOR

using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using Funny.Base.Utils;

namespace TableDR
{
    public partial class SkillSelectConfig
    {
        public static OnValueChangedAttribute Attribute_SecondSelectType = new OnValueChangedAttribute("OnValueChange_SecondSelectType");
        public static ListDrawerSettingsAttribute Attribute_SecondSelecrParams = new ListDrawerSettingsAttribute
        {
            OnBeginListElementGUI = "OnBeginListElement_SecondSelecrParams",
            OnEndListElementGUI = "OnEndListElement",
            HideAddButton = true,
            HideRemoveButton = true,
            ShowFoldout = true,
            DraggableItems = false,
        };

        private static readonly Dictionary<TSkillSecondSelectType, List<string>> paramDescMap = new Dictionary<TSkillSecondSelectType, List<string>>
        {
            {TSkillSecondSelectType.TSSST_ATTR_MAX, new List<string>{ "属性枚举", "是否百分比" } },
            {TSkillSecondSelectType.TSSST_ATTR_MIN, new List<string>{ "属性枚举", "是否百分比" } },
            {TSkillSecondSelectType.TSSST_ATTR_SORT, new List<string>{ "排序属性枚举: ", "是否百分比", "排序属性是否降序", "距离排序（0：优先近，1：优先远，其他不处理）" } },
        };

        private bool HideIf_EntityTypeFilters()
        {
            if (EntityTypeFilters == null)
            {
                return true;
            }
            return !(EntityTypeFilters.GetListRef())?.Contains(TEntityType.TENT_BATTLE_UNIT) ?? false;
        }
        private void OnValueChange_SecondSelectType()
        {
            SecondSelecrParams ??= new List<TParam>();
            SecondSelecrParams.GetListRef().Clear();

            switch (SecondSelectType)
            {
                case TSkillSecondSelectType.TSSST_ATTR_MAX:
                case TSkillSecondSelectType.TSSST_ATTR_MIN:
                    SecondSelecrParams.GetListRef().AddRange(new List<TParam>
                    {
                        new TParam(),   // 属性枚举
                        new TParam(),   // 是否百分比
                    });
                    break;
                case TSkillSecondSelectType.TSSST_ATTR_SORT:
                    SecondSelecrParams.GetListRef().AddRange(new List<TParam>
                    {
                        new TParam(),   // 属性枚举
                        new TParam(),   // 是否百分比
                        new TParam(),   // 是否降序
                        new TParam(),   // 距离排序（0：增序，1：降序，其他不处理）
                    });
                    break;
            }
        }
        private void OnBeginListElement_SecondSelecrParams(int index)
        {
            var boxInfo = string.Empty;
            var param = SecondSelecrParams.GetAt(index);
            if (paramDescMap.TryGetValue(SecondSelectType, out var descs))
            {
                boxInfo = descs.GetAt(index, string.Empty);
            }
            switch (SecondSelectType)
            {
                case TSkillSecondSelectType.TSSST_ATTR_MAX:
                case TSkillSecondSelectType.TSSST_ATTR_MIN:
                case TSkillSecondSelectType.TSSST_ATTR_SORT:
                    switch (index)
                    {
                        case 0:
                            if (param?.ParamType == TParamType.TPT_NULL)
                            {
                                if (Enum.IsDefined(typeof(TBattleNatureEnum), param.Value))
                                {
                                    boxInfo += ((TBattleNatureEnum)param.Value).GetDescription(false);
                                }
                            }
                            break;
                    }
                    break;
            }
            SirenixEditorGUI.BeginBox(boxInfo);
        }
        private void OnEndListElement(int index)
        {
            SirenixEditorGUI.EndBox();
        }
    }
}

#endif
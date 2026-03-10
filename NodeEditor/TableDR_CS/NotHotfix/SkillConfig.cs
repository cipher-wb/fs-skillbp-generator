#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using UnityEditor;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace TableDR
{
    public partial class SkillComboCD
    {
        public SkillComboCD()
        { }
        public SkillComboCD(int cd, int cast, int buffer, int baseDuration)
        {
            CDTime = cd;
            CastFrame = cast;
            BufferFrame = buffer;
            BaseDuration = baseDuration;
        }
    }
    public partial class SkillConfig
    {
        #region Odin编辑器相关
        public static ListDrawerSettingsAttribute Attribute_SkillDamageTagsList = new ListDrawerSettingsAttribute
        {
            HideAddButton = true,
            OnTitleBarGUI = "OnTitleBarGUI_SkillDamageTagsList",
            OnBeginListElementGUI = "OnBeginListElement_SkillDamageTagsList",
            OnEndListElementGUI = "OnEndListElement"
        };
        public static ListDrawerSettingsAttribute Attribute_SkillTagsList = new ListDrawerSettingsAttribute
        {
            HideAddButton = true,
            OnTitleBarGUI = "OnTitleBarGUI_SkillTagsList",
            OnBeginListElementGUI = "OnBeginListElement_SkillTagsList",
            OnEndListElementGUI = "OnEndListElement"
        };
        public static ListDrawerSettingsAttribute Attribute_SkillTipsConditionSkillTagsList = new ListDrawerSettingsAttribute
        {
            HideAddButton = true,
            OnTitleBarGUI = "OnTitleBarGUI_SkillTipsConditionSkillTagsList",
            OnBeginListElementGUI = "OnBeginListElement_SkillTipsConditionSkillTagsList",
            OnEndListElementGUI = "OnEndListElement"
        };
        public static OnValueChangedAttribute Attribute_TIndicatorType = new OnValueChangedAttribute("OnValueChange_TIndicatorType");
        public static ListDrawerSettingsAttribute Attribute_IndicatorParam = new ListDrawerSettingsAttribute
        {
            OnBeginListElementGUI = "OnBeginListElement_IndicatorParam",
            OnEndListElementGUI = "OnEndListElement",
            HideAddButton = true,
            HideRemoveButton = true,
            ShowFoldout = true,
            DraggableItems = false,
        };
        public static ListDrawerSettingsAttribute Attribute_SkillIndicatorParamTagConfigIds = new ListDrawerSettingsAttribute
        {
            OnBeginListElementGUI = "OnBeginListElement_SkillIndicatorParamTagConfigIds",
            OnEndListElementGUI = "OnEndListElement",
        };
        public static ListDrawerSettingsAttribute Attribute_IndicatorResParam = new ListDrawerSettingsAttribute
        {
            OnBeginListElementGUI = "OnBeginListElement_IndicatorResParam",
            OnEndListElementGUI = "OnEndListElement",
            HideAddButton = true,
            HideRemoveButton = true,
            ShowFoldout = true,
            DraggableItems = false,
        };
        public static ListDrawerSettingsAttribute Attribute_SkillIndicatorResParamTagConfigIds = new ListDrawerSettingsAttribute
        {
            OnBeginListElementGUI = "OnBeginListElement_SkillIndicatorResParamTagConfigIds",
            OnEndListElementGUI = "OnEndListElement",
        };
        public static OnValueChangedAttribute Attribute_CdType = new OnValueChangedAttribute("OnValueChange_CdType");
        public static ListDrawerSettingsAttribute Attribute_ComboCDList = new ListDrawerSettingsAttribute
        {
            CustomAddFunction = "CustomAddFunction_ComboCDList",
            //OnBeginListElementGUI = "OnBeginListElement_ComboCDList",
            //OnEndListElementGUI = "OnEndListElement",
            HideAddButton = false,
            HideRemoveButton = false,
            ShowFoldout = true,
            DraggableItems = false,
        };
        public static HideIfAttribute Attribute_HideIf_CDType = new HideIfAttribute("HideIf_CDType");
        public static ValueDropdownAttribute Attribute_GetCastTargetCondTemplateOptions = new ValueDropdownAttribute("GetCastTargetCondTemplateOptions");
        public static OnValueChangedAttribute Attribute_OnValueChange_SmartCastTargetCondTemplate = new OnValueChangedAttribute("OnValueChange_SmartCastTargetCondTemplate");
        public static OnValueChangedAttribute Attribute_OnValueChange_CastTargetCondTemplate = new OnValueChangedAttribute("OnValueChange_CastTargetCondTemplate");
        public static CustomValueDrawerAttribute Attribute_CustomValueDrawerAttribute_SkillRangeTagConfigId = new CustomValueDrawerAttribute("CustomValueDrawerAttribute_SkillRangeTagConfigId");
        public static CustomValueDrawerAttribute Attribute_CustomValueDrawerAttribute_SkillMinRangeTagConfigId = new CustomValueDrawerAttribute("CustomValueDrawerAttribute_SkillMinRangeTagConfigId");
        public static ValueDropdownAttribute Attribute_CustomValueDrawerAttribute_BDLabels = new ValueDropdownAttribute("CustomValueDrawerAttribute_BDLabels");
       
        public static OnValueChangedAttribute Attribute_OnValueChange_SkillMainType = new OnValueChangedAttribute("OnValueChange_SkillMainType");
        public static OnValueChangedAttribute Attribute_OnValueChange_SkillSubType = new OnValueChangedAttribute("OnValueChange_SkillSubType");
        public static ValueDropdownAttribute Attribute_CustomValueDrawerAttribute_SkillSubType = new ValueDropdownAttribute("CustomValueDrawerAttribute_SkillSubType");

        #endregion Odin编辑器相关

        private void OnTitleBarGUI_SkillTagsList()
        {
            // 插件bug，重写下Add处理
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                SkillTagsList.GetListRef().Add(new SkillTagInfo());
            }
        }
        private void OnBeginListElement_SkillTagsList(int index)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(SkillTagsList, index));
        }
        private void OnTitleBarGUI_SkillDamageTagsList()
        {
            // 插件bug，重写下Add处理
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                SkillDamageTagsList.GetListRef().Add(new SkillTagInfo());
            }
        }
        
        private void OnBeginListElement_SkillDamageTagsList(int index)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(SkillDamageTagsList, index));
        }
        private void OnTitleBarGUI_SkillTipsConditionSkillTagsList()
        {
            // 插件bug，重写下Add处理
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                SkillTipsConditionSkillTagsList.GetListRef().Add(new SkillTagInfo());
            }
        }
        private void OnBeginListElement_SkillTipsConditionSkillTagsList(int index)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(SkillTipsConditionSkillTagsList, index));
        }

        private void OnValueChange_TIndicatorType()
        {
            SkillIndicatorParam ??= new List<int>();
            SkillIndicatorParamTagConfigIds ??= new List<int>();
            SkillIndicatorParam.GetListRef().Clear();
            SkillIndicatorParamTagConfigIds.GetListRef().Clear();

            SkillIndicatorResParam ??= new List<int>();
            SkillIndicatorResParamTagConfigIds ??= new List<int>();
            SkillIndicatorResParam.GetListRef().Clear();
            SkillIndicatorResParamTagConfigIds.GetListRef().Clear();

            switch (SkillIndicatorType)
            {
                case TIndicatorType.TIRT_MULTIWAY:
                    SkillIndicatorParam.GetListRef().Add(100); //宽度
                    SkillIndicatorParam.GetListRef().Add(3); //数量
                    SkillIndicatorParam.GetListRef().Add(1000); //间隔角度
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int>{ 0, 0});
                    break;
                case TIndicatorType.TIRT_LINE:
                    SkillIndicatorParam.GetListRef().Add(100); //宽度
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int> { 0, 0});
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE:
                    SkillIndicatorParam.GetListRef().AddRange(new List<int>
                    {
                        500,    // 外圆半径
                        0,      // 内圆半径
                        0,      // 拖动矩形-长
                        0,      // 拖动矩形-宽
                        0,      // 智能施法坐标往主角侧偏移值
                    });
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int> 
                    { 
                        0,  // 技能范围圈
                        0,  // 拖动圈-外圈
                        0,  // 拖动圈-内圈
                        -1, // 连接外圈到脚下的矩形
                        -1, // 技能最小范围圈
                        -1, // 拖动矩形
                    });
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE_CAPTRUE:
                    SkillIndicatorParam.GetListRef().Add(500); //外圆半径
                    SkillIndicatorParam.GetListRef().Add(300); //内圆半径
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int> { 0, 0, 0 });
                    break;
                case TIndicatorType.TIRT_NO_TARGET:
                    SkillIndicatorParam.GetListRef().Add(500); //作用范围
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int> { 0 });
                    break;
                case TIndicatorType.TIRT_SINGLE_TARGET:
                    SkillIndicatorParam.GetListRef().Add(500); //作用范围
                    SkillIndicatorResParam.GetListRef().AddRange(new List<int> { 0 });
                    break;
                case TIndicatorType.TIRT_SECTOR:
                    ((List<int>)SkillIndicatorParam).Add(60); //扇形角度
                    ((List<int>)SkillIndicatorResParam).AddRange(new List<int> { 0, 0});
                    break;
            }
            // 刷新导致面板位置回到最上，暂无必要，屏蔽
            //NodeEditorHelper.OnValueChange(nameof(TIndicatorType));
            UpdateIndicatorTypeConfig();
        }

        // 根据指示器类型刷新相关数据 TODO 临时刷新智能施法数据
        public void UpdateIndicatorTypeConfig()
        {
            switch (SkillIndicatorType)
            {
                case TIndicatorType.TIRT_NO_TARGET:
                    // 无目标类型需要切换施法目标
                    CastTargetCondTemplate = 3;
                    OnValueChange_CastTargetCondTemplate();
                    break;
            }
        }

        private bool HideIf_TIndicatorType()
        {
            return SkillIndicatorType != TIndicatorType.TIRT_LINE;
        }

        private void OnBeginListElement_IndicatorParam(int index)
        {
            SirenixEditorGUI.BeginBox(GetIndicatorParamName(index));
        }

        private void OnBeginListElement_IndicatorResParam(int index)
        {
            SirenixEditorGUI.BeginBox(GetIndicatorResParamName(index));
        }

        private void OnBeginListElement_SkillIndicatorParamTagConfigIds(int index)
        {
            string paramName = GetIndicatorParamName(index);
            string tagDesc = SkillTagInfo.GetTagDesc(SkillIndicatorParamTagConfigIds, index);
            SirenixEditorGUI.BeginBox($"{paramName}:{tagDesc}");
        }
        private void OnBeginListElement_SkillIndicatorResParamTagConfigIds(int index)
        {
            string paramName = GetIndicatorResParamName(index);
            string tagDesc = SkillTagInfo.GetTagDesc(SkillIndicatorResParamTagConfigIds, index);
            SirenixEditorGUI.BeginBox($"{paramName}:{tagDesc}");
        }

        private int CustomValueDrawerAttribute_SkillRangeTagConfigId(int value, GUIContent label)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(SkillRangeTagConfigId));
            // 显示一个整数字段，并在旁边显示额外的信息
            value = EditorGUILayout.IntField(label, value);
            SirenixEditorGUI.EndBox();
            return value;
        }
        private int CustomValueDrawerAttribute_SkillMinRangeTagConfigId(int value, GUIContent label)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(SkillMinRangeTagConfigId));
            // 显示一个整数字段，并在旁边显示额外的信息
            value = EditorGUILayout.IntField(label, value);
            SirenixEditorGUI.EndBox();
            return value;
        }

        private ValueDropdownList<int> CustomValueDrawerAttribute_BDLabels()
        {
            ValueDropdownList<int> valueDropdownItems = new ValueDropdownList<int>();

            var bdLabels = SkillBDLabelConfigManager.Instance.ItemArray.Items;
            foreach (var bdLabelConfig in bdLabels)
            {
                if (bdLabelConfig.ElementType == ElementType)
                    valueDropdownItems.Add(new ValueDropdownItem<int>(bdLabelConfig.BDDesc, bdLabelConfig.ID));
            }

            return valueDropdownItems;
        }


        private void OnEndListElement(int index)
        {
            SirenixEditorGUI.EndBox();
        }

        private string GetIndicatorParamName(int index)
        {
            switch (SkillIndicatorType)
            {
                case TIndicatorType.TIRT_LINE:
                    if (index == 0)
                    {
                        return "宽度";
                    }
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE:
                    switch (index)
                    {
                        case 0:
                            return "拖动圈-外圆半径";
                        case 1:
                            return "拖动圈-内圆半径（无内圈配0）";
                        case 2:
                            return "拖动矩形-长";
                        case 3:
                            return "拖动矩形-宽";
                        case 4:
                            return "智能施法坐标往主角侧偏移值";
                    }
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE_CAPTRUE:
                    switch (index)
                    {
                        case 0:
                            return "拖动圈-外圆半径";
                        case 1:
                            return "拖动圈-内圆半径";
                    }
                    break;
                case TIndicatorType.TIRT_MULTIWAY:
                    switch (index)
                    {
                        case 0:
                            return "指示器宽度-厘米（注：半径长度请填施法距离）";
                        case 1:
                            return "指示器数量";
                        case 2:
                            return "间隔角度-百分比";
                    }
                    break;
                case TIndicatorType.TIRT_NO_TARGET:
                    if (index == 0)
                    {
                        return "作用范围";
                    }
                    break;
                case TIndicatorType.TIRT_SECTOR:
                    if (index == 0)
                    {
                        return "扇形角度";
                    }
                    break;
                default:
                    return "(无需配置)";
            }
            return "无效参数";
        }

        private string GetIndicatorResParamName(int index)
        {
            switch (SkillIndicatorType)
            {
                case TIndicatorType.TIRT_LINE:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                        case 1: return "直线资源-可缩放";
                    }
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                        case 1: return "拖动圈-外圈";
                        case 2: return "拖动圈-内圈";
                        case 3: return "连接外圈到脚下的矩形（默认-1）";
                        case 4: return "技能最小范围圈";
                        case 5: return "拖动矩形";
                    }
                    break;
                case TIndicatorType.TIRT_DOUBLE_CIRCLE_CAPTRUE:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                        case 1: return "拖动圈-外圈";
                        case 2: return "拖动圈-内圈";
                    }
                    break;
                case TIndicatorType.TIRT_MULTIWAY:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                        case 1: return "直线资源-可缩放";
                    }
                    break;
                case TIndicatorType.TIRT_NO_TARGET:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                    }
                    break;
                case TIndicatorType.TIRT_SINGLE_TARGET:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                    }
                    break;
                case TIndicatorType.TIRT_SECTOR:
                    switch (index)
                    {
                        case 0: return "技能范围圈（资源ID-ModelConfig，0:默认资源, -1:不显示，下同）";
                        case 1: return "扇形资源";
                    }
                    break;
                default:
                    return "(无需配置)";
            }
            return "无效参数";
        }


        private SkillComboCD CustomAddFunction_ComboCDList()
        {
            return new SkillComboCD(CdTime, SkillCastFrame, SkillBufferFrame, SkillBaseDuration);
        }

        private void OnBeginListElement_ComboCDList(int index)
        {
            SirenixEditorGUI.BeginBox();
        }

        private bool HideIf_CDType()
        {
            return CdType != TSkillColdType.TSCDT_COMBO && CdType != TSkillColdType.TSCDT_COMBO_SKILL_END;
        }
        
        private ValueDropdownList<int> GetCastTargetCondTemplateOptions()
        {
            ValueDropdownList<int> skillTargetCondTemplateList = new ValueDropdownList<int>();
            skillTargetCondTemplateList.Add("自定义", 0);
            if (SkillTargetCondTemplateConfigManager.Instance?.ItemArray?.Items != null)
            {
                foreach (var skillTargetCondTemplateConfig in SkillTargetCondTemplateConfigManager.Instance.ItemArray.Items)
                {
                    skillTargetCondTemplateList.Add(skillTargetCondTemplateConfig.Name, skillTargetCondTemplateConfig.ID);
                }
            }
            return skillTargetCondTemplateList;
        }

        public void OnValueChange_SmartCastTargetCondTemplate()
        {
            var skillTargetCondTemplateConfig =
                SkillTargetCondTemplateConfigManager.Instance.GetItem(SmartCastTargetCondTemplate);
            if (skillTargetCondTemplateConfig == null)
            {
                SmartCastTargetMonsterRankCond = new List<UnitType>();
                SmartCastTargetCampCond = new List<TSkillTargetCampType>();
            }
            else
            {
                SmartCastTargetMonsterRankCond = new List<UnitType>(skillTargetCondTemplateConfig.CastTargetMonsterRankCond);
                SmartCastTargetCampCond = new List<TSkillTargetCampType>(skillTargetCondTemplateConfig.CastTargetCampCond);
            }
        }

        public void OnValueChange_CastTargetCondTemplate()
        {
            var skillTargetCondTemplateConfig = SkillTargetCondTemplateConfigManager.Instance.GetItem(CastTargetCondTemplate);
            if (skillTargetCondTemplateConfig == null)
            {
                CastTargetMonsterRankCond = new List<UnitType>();
                CastTargetCampCond = new List<TSkillTargetCampType>();
            }
            else
            {
                CastTargetMonsterRankCond = new List<UnitType>(skillTargetCondTemplateConfig.CastTargetMonsterRankCond);
                CastTargetCampCond = new List<TSkillTargetCampType>(skillTargetCondTemplateConfig.CastTargetCampCond);
            }
        }
        
        private void OnValueChange_CdType()
        {
            if (CdType == TSkillColdType.TSCDT_COMBO || CdType == TSkillColdType.TSCDT_COMBO_SKILL_END)
            {
                if (ComboCdList == null)
                    ComboCdList = new List<SkillComboCD>();
                // 默认两段
                ((List<SkillComboCD>)ComboCdList).Add(new SkillComboCD(CdTime, SkillCastFrame, SkillBufferFrame, SkillBaseDuration));
                ((List<SkillComboCD>)ComboCdList).Add(new SkillComboCD(CdTime, SkillCastFrame, SkillBufferFrame, SkillBaseDuration));
            }
            else
            {
                if (ComboCdList != null)
                {
                    ((List<SkillComboCD>)ComboCdList).Clear();
                    ComboCdList = null;
                }
            }
        }

        private void OnValueChange_SkillMainType()
        {
            // 更新SkillSubType
            if (((int)SkillSubType / 100) != (int)SkillMainType)
            {
                if (SkillMainType == TBattleSkillMainType.TBSMT_NULL)
                    SkillSubType = TBattleSkillSubType.TBSST_NULL;
                else
                    SkillSubType = (TBattleSkillSubType)((int)SkillMainType * 100 + 1);
            }

            // 刷新SkillSubType的下拉
        }
        private void OnValueChange_SkillSubType()
        {
            // 更新SkillMainType
            if (((int)SkillSubType / 100) != (int)SkillMainType)
            {
                if (SkillSubType == TBattleSkillSubType.TBSST_NULL)
                    SkillMainType = TBattleSkillMainType.TBSMT_NULL;
                else
                    SkillMainType = (TBattleSkillMainType)((int)SkillSubType / 100);
            }
        }

        private ValueDropdownList<int> CustomValueDrawerAttribute_SkillSubType()
        {
            ValueDropdownList<int> valueDropdownItems = new ValueDropdownList<int>();
            foreach (TableDR.TBattleSkillSubType skillSubType in System.Enum.GetValues(typeof(TableDR.TBattleSkillSubType)))
            {
                if (SkillMainType == TBattleSkillMainType.TBSMT_NULL || ((int)skillSubType / 100) == (int)SkillMainType)
                {
                    valueDropdownItems.Add(skillSubType.GetDescription(), (int)skillSubType);
                }
            }
            return valueDropdownItems;
        }

        public bool EnableShowStorePropry()
        {
            if (CdType == TSkillColdType.TSCDT_STORAGE)
            {
                return true;
            }
            SkillFixCdTime = 0;
            CDMaxStoreCount = 0;
            return false;
        }
    }
}
#endif

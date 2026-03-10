#if UNITY_EDITOR
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TableDR
{
    public partial class TParam
    {
        private GUIStyle valueStyle;
        private string errorMessage;
        private string displayName;

        private Dictionary<string, string> CustomDescription = new Dictionary<string, string>()
        {
            { nameof(Value), "数值"},
            { nameof(ParamType), "数值类型"},
            { nameof(Factor), "数值系数(万分比)"}
        };

        private void InitDescription()
        {
            CustomDescription = new Dictionary<string, string>()
            {
                { nameof(Value), "数值"},
                { nameof(ParamType), "数值类型"},
                { nameof(Factor), "数值系数(万分比)"}
            };

        }

        public bool Compare(object obj)
        {
            if(this.GetType() != obj.GetType() || obj == null) { return false; }

            var comparer = obj as TParam;

            return this.Value == comparer.Value
                && this.ParamType == comparer.ParamType
                && this.Factor == comparer.Factor;
        }

        public string GetCustomDescription(string proptyName)
        {
            if (CustomDescription == null)
            {
                return string.Empty;
            }
            CustomDescription.TryGetValue(proptyName, out var ret);
            if (ret == null)
            {
                InitDescription();
            }
            return ret;
        }

        public void SetCustomDescription(string proptyName, string description)
        {
            if (CustomDescription == null)
            {
                CustomDescription = new Dictionary<string, string>();
            }
            CustomDescription[proptyName] = description;
        }

        private string GetErrorMessage() { return errorMessage; }
        private bool IsError() { return !string.IsNullOrEmpty(errorMessage); }
        public string GetDisplayName() { return displayName ?? string.Empty; }
        public void RefreshDisplay(string displayName, string errorMessage) { this.displayName = displayName; this.errorMessage = errorMessage; }

        private void OnValueChange_TParamType()
        {
            NodeEditorHelper.OnValueChange(nameof(TParamType));
        }
        private void OnInspectorGUI_Value()
        {
            if (valueStyle == null)
            {
                valueStyle = new GUIStyle();
            }
            valueStyle.normal.textColor = string.IsNullOrEmpty(errorMessage) ? Color.white : Color.red;
            if (!string.IsNullOrEmpty(errorMessage))
            {
                GUILayout.Label(displayName + errorMessage, valueStyle);
            }
            else
            {
                GUILayout.Label(displayName, valueStyle);
            }
        }

        /// <summary>
        /// 设置Value
        /// </summary>
        /// <param name="value"></param>
#if UNITY_EDITOR
        public void SetValue(int value)
        {
            Value = value;
        }
#endif
        //private int CustomValueDrawer_Value(int value, GUIContent label/*, Func<GUIContent, bool> callNextDrawer*/)
        //{
        //    // TODO 自定义GUI
        //    SirenixEditorGUI.BeginBox();
        //    //callNextDrawer(label);
        //    var result = EditorGUILayout.IntSlider(value, 1, 100);
        //    SirenixEditorGUI.EndBox();
        //    return result;
        //}
        //private Color GetGUIColor()
        //{
        //    //Sirenix.Utilities.Editor.GUIHelper.RequestRepaint();
        //    return string.IsNullOrEmpty(errorMessage) ? Color.green : Color.red;
        //}

        #region SkillTagsConfig下拉
        public static ValueDropdownAttribute VD_TagsValue = new ValueDropdownAttribute("ValueDropdown_TagsValue")
        {
            DropdownTitle = "选择技能参数：",
            ExpandAllMenuItems = false,
            AppendNextDrawer = true,
            DoubleClickToConfirm = true,
        };
        private IEnumerable<ValueDropdownItem> ValueDropdown_TagsValue()
        {
            // TODO 考虑未导出表格数据
            var tags = SkillTagsConfigManager.Instance.ItemArray.Items;
            foreach (var tagConfig in tags)
            {
                yield return new ValueDropdownItem($"{tagConfig.TagType.GetDescription()}/{tagConfig.ID}-{tagConfig.Desc}", tagConfig.ID);
            }
        }
        #endregion

        #region SkillEventConfig下拉
        public static ValueDropdownAttribute VD_EventValue = new ValueDropdownAttribute("ValueDropdown_EventValue")
        {
            DropdownTitle = "选择技能消息：",
            ExpandAllMenuItems = false,
            AppendNextDrawer = true,
            DoubleClickToConfirm = true,
        };
        private IEnumerable<ValueDropdownItem> ValueDropdown_EventValue()
        {
            // TODO 考虑未导出表格数据
            var eventConfigs = SkillEventConfigManager.Instance.ItemArray.Items;
            foreach (var eventConfig in eventConfigs)
            {
                yield return new ValueDropdownItem($"{eventConfig.ID}-{eventConfig.Name.Replace("/", "\\")}", eventConfig.ID);
            }
        }
        #endregion


        #region BattleCameraShakeConfig下拉
        public static ValueDropdownAttribute VD_CameraShakeValue = new ValueDropdownAttribute("ValueDropdown_CameraShakeValue")
        {
            DropdownTitle = "选择震屏配置：",
            ExpandAllMenuItems = false,
            AppendNextDrawer = true,
            DoubleClickToConfirm = true,
        };
        private IEnumerable<ValueDropdownItem> ValueDropdown_CameraShakeValue()
        {
            // TODO 考虑未导出表格数据
            var configs = BattleCameraShakeConfigManager.Instance.ItemArray.Items;
            foreach (var config in configs)
            {
                yield return new ValueDropdownItem($"{config.ID}-{config.Name.Replace("/", "\\")}", config.ID);
            }
        }
        #endregion
    }
}
#endif
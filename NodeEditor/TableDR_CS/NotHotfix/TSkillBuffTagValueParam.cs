#if UNITY_EDITOR
using UnityEngine;

namespace TableDR
{
    public partial class TSkillBuffTagValueParam
    {
        private GUIStyle valueStyle;
        private string errorMessage;
        private string displayName;
        private string GetErrorMessage() { return errorMessage; }
        private bool IsError() { return !string.IsNullOrEmpty(errorMessage); }
        public string GetDisplayName() { return displayName ?? string.Empty; }
        public void RefreshDisplay(string displayName, string errorMessage) { this.displayName = displayName; this.errorMessage = errorMessage; }

        private void OnValueChange_AddValueParamType()
        {
            NodeEditorHelper.OnValueChange(nameof(AddValue_ParamType));
        }
        private void OnValueChange_SkillIDParamType()
        {
            NodeEditorHelper.OnValueChange(nameof(SkillID_ParamType));
        }
        private void OnValueChange_SkillTagIDParamType()
        {
            NodeEditorHelper.OnValueChange(nameof(SkillTagID_ParamType));
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
        public string GetNodeViewDesc()
        {
            var skillDesc = SkillID_ParamType == TParamType.TPT_NULL ? $"{SkillID_ParamValue}" : $"{SkillID_ParamValue}_{SkillID_ParamType.GetDescription(false)}";
            var skillTagDesc = SkillTagID_ParamType == TParamType.TPT_NULL ? $"{SkillTagID_ParamValue}" : $"{SkillTagID_ParamValue}_{SkillTagID_ParamType.GetDescription(false)}";
            var skillTagValueDesc = AddValue_ParamType == TParamType.TPT_NULL ? $"{AddValue_ParamValue}" : $"{AddValue_ParamValue}_{AddValue_ParamType.GetDescription(false)}";
            return $"[²ÎÊý][{skillDesc}_{skillTagDesc}_{skillTagValueDesc}]";
        }
    }
}
#endif
#if UNITY_EDITOR

using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TableDR
{
    public partial class SkillConditionConfig
    {
        private Action paramsChanged;
        public event Action OnParamsChanged
        {
            add
            {
                paramsChanged -= value;
                paramsChanged += value;
            }
            remove { paramsChanged -= value; }
        }

        private void CustomAddFunction_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG()
        {
            ((List<TParam>)Params).Add(new TParam());
            this.paramsChanged?.Invoke();
        }
        private void CustomRemoveIndexFunction_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG(int index)
        {
            if (Params.Count <= 3)
            {
                Debug.LogError("条件-判断技能是否包含指定技能AI标签, 需保留最少3个参数!");
                return;
            }
            ((List<TParam>)Params).RemoveAt(index);
            this.paramsChanged?.Invoke();
        }
        private void OnTitleBarGUI_Params_TSCT_IS_SKILL_CONTAIN_SKILL_AI_TAG()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                // 添加两个
                if (this.Params?.Count >= 3)
                {
                    int changeNum = 2;
                    while (changeNum-- > 0)
                    {
                        //((List<TParam>)Params).Add(new TParam());
                        Sirenix.Utilities.ListExtensions.SetLength(((List<TParam>)Params), this.Params.Count + 1, () => new TParam());
                    }
                }
            }
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.X))
            {
                // 删除2个
                int changeNum = 2;
                while (changeNum-- > 0)
                {
                    if (this.Params?.Count > 3)
                    {
                        Sirenix.Utilities.ListExtensions.SetLength(((List<TParam>)Params), this.Params.Count - 1);
                    }
                }
            }
        }
    }
}

#endif

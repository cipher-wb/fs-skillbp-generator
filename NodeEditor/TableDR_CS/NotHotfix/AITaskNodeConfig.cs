#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funny.Base.Utils;
using UnityEngine;

namespace TableDR
{
    public partial class AITaskNodeConfig
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

        [System.ComponentModel.Description("AI参数列表")]
        [ShowInInspector, DelayedProperty, LabelText("AI参数列表"), Newtonsoft.Json.JsonProperty, ExcelIgnore]
        public List<global::TableDR.SkillTagInfo> SkillTagsList { get; private set; }

        private void OnTitleBarGUI_SkillTagsList()
        {
            // 插件bug，重写下Add处理
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                this.SkillTagsList.Add(new SkillTagInfo());
            }
        }
        private void OnBeginListElement_SkillTagsList(int index)
        {
            SirenixEditorGUI.BeginBox(GetTagDesc(SkillTagsList, index));
        }
        private void OnEndListElement_SkillTagsList(int index)
        {
            SirenixEditorGUI.EndBox();
        }
        private string GetTagDesc(List<SkillTagInfo> tagsList, int index)
        {
            var tagConfigID = tagsList.GetAt(index, null)?.SkillTagConfigID ?? 0;
            try
            {
                var tagConfig = SkillTagsConfigManager.Instance.GetItem(tagConfigID);
                return $"{index + 1}-{tagConfig?.Desc ?? "错误TagID"}";
            }
            catch
            {
                return $"{index + 1}-获取Tag值异常";
            }
        }


        private void CustomRemoveIndexFunction_Params_AI_TNT_SWITCH(int index)
        {
            if (index <= 3)
            {
                Debug.LogError("不允许删除!");
                return;
            }

            if (index % 2 == 0 || index != (this.Params.Count - 1))
            {
                Debug.LogError("只能从下往上逐个删除!");
                return;
            }

            Params.GetListRef().RemoveAt(index);
            Params.GetListRef().RemoveAt(index - 1);
            this.paramsChanged?.Invoke();
        }

        private void CustomAddFunction_Params_AI_TNT_SWITCH()
        {
            Params.GetListRef().Add(new TParam());
            Params.GetListRef().Add(new TParam());
            this.paramsChanged?.Invoke();
        }
    }
}

#endif
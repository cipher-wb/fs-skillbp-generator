/////////////////////////////////////
// 注意！！此代码文件由工具自动生成！！ 
// 扩展方法请新建文件扩展partial类实现 
// 如:#CONFIGNAME#.Custom.cs
/////////////////////////////////////

using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class AITaskNodeConfigNode : IParamsNode
    {
        [HideInInspector]
        public AITaskNodeType TaskNodeType;

        // TODO 需改成只读显示
        [LabelText("AI参数列表"), VisibleIf("ShowExtraParamInfo", true), ShowNodeView, HideIf("@true")]
        public List<string> AIParamsDesc = new List<string>();

        public void SyncAIParams()
        {
            AIParamsDesc.Clear();
            if (Config.SkillTagsList == null)
            {
                return;
            }
            foreach (var item in Config.SkillTagsList)
            {
                var tagConfig = SkillTagsConfigManager.Instance.GetItem(item.SkillTagConfigID);
                if (tagConfig != null)
                {
                    AIParamsDesc.Add($"{item.SkillTagConfigID}:{tagConfig.Desc}");
                }
                else
                {
                    AIParamsDesc.Add($"{item.SkillTagConfigID}:没找到SkillTagConfig");
                }
            }
            onNodeChanged?.Invoke(nameof(AIParamsDesc));
        }

        [LabelText("显示AI参数列表"), OnValueChanged("OnShowExtraParamInfoChanged", true)]
        public bool ShowExtraParamInfo = true;
        public void OnShowExtraParamInfoChanged()
        {
            if (ShowExtraParamInfo)
            {
                SyncAIParams();
            }
            else
            {
                onNodeChanged?.Invoke(nameof(ShowExtraParamInfo));
            }
        }

        public AITaskNodeConfigNode(AITaskNodeType nodeType) : base()
        {
            TaskNodeType = nodeType;
        }

        protected override void OnConfigChanged()
        {
            if (ShowExtraParamInfo)
            {
                SyncAIParams();
            }
            base.OnConfigChanged();
        }

        public void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes)
        {

        }

        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"{TaskNodeType.GetDescription(false)} [{name}:{ID}]");
        }
        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.TaskNodeType), TaskNodeType);
            Config.ExSetValue(nameof(Config.SkillTagsList), new List<TableDR.SkillTagInfo>());
        }

        public override ParamsAnnotation GetParamsAnnotation()
        {
            return TableAnnotation.Inst.GetParamsAnnotation(TaskNodeType);
        }
        public override string GetParamsName()
        {
            return nameof(Config.Params);
        }
        public override IReadOnlyList<TParam> GetParamsList()
        {
            return Config.Params;
        }

    }
}

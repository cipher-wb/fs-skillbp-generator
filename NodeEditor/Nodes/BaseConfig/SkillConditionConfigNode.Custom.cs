using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class SkillConditionConfigNode : IParamsNode
    {
        [HideInInspector]
        public TSkillConditionType SkillConditionType;
        public SkillConditionConfigNode(TSkillConditionType type) : base()
        {
            SkillConditionType = type;
        }
        public virtual void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes) { }
        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"{EnumUtility_NotHotfix.GetDescription(SkillConditionType, false)} [{name}:{ID}]");
        }
        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.SkillConditionType), SkillConditionType);
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            return TableAnnotation.Inst.GetParamsAnnotation(SkillConditionType);
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

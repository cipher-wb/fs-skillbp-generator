using System;
using System.Collections.Generic;
using System.Reflection;
using TableDR;
using UnityEngine;

namespace NodeEditor
{
    public partial class SkillSelectConfigNode : IParamsNode
    {
        [HideInInspector]
        public TSkillSelectType SkillSelectType;
        public SkillSelectConfigNode(TSkillSelectType type) : base()
        {
            SkillSelectType = type;
        }
        public virtual void ProcessParamAttributes(TParam param, MemberInfo member, List<Attribute> attributes) { }
        protected override void OnRefreshCustomName()
        {
            // 依据表格效果类型显示不同表现
            SetCustomName($"{SkillSelectType.GetDescription(false)} [{name}:{ID}]");
        }
        protected override void OnPreset()
        {
            base.OnPreset();
            Config.ExSetValue(nameof(Config.SkillSelectType), SkillSelectType);
            // 特殊筛选默认 敌军通用
            Config.ExSetValue(nameof(Config.SpecialSkillSelectFlag), TableDR.TSpecialSkillSelectFlag.TSSSF_Common_DiJun);
            // 单位类型筛选默认 战斗单位，场景物件
            Config.ExSetValue(nameof(Config.EntityTypeFilters), new List<TEntityType>() { TEntityType.TENT_BATTLE_UNIT, TEntityType.TENT_SCENE_OBJECT });
        }
        public override ParamsAnnotation GetParamsAnnotation()
        {
            return TableAnnotation.Inst.GetParamsAnnotation(SkillSelectType);
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

using System;

namespace NodeEditor
{
    /// <summary>
    /// 表格约束信息，如：SkillConditionConfig.ID|ID
    /// </summary>
    public class FromFiledAnnotation
    {
        public string Name;                     // 如：技能效果
        public bool isConfigId;                 // 是否约束表格ID
        public string RefDesc;                  // 非约束表格ID，如：Desc
        public string RefTableName;             // 引用表格名，如：SkillEffectConfig
        public string RefTableFullName;         // 如：TableDR.SkillEffectConfig
        public string RefTableManagerFullName;  // 如：TableDR.SkillEffectConfigManager
        //public object RefTableDataTarget;   // 如：TableDR.SkillEffectConfigManager对象
        public Type RefTableType { get { return TableHelper.GetTableType(RefTableFullName); } }
        public Type RefTableManagerType { get { return TableHelper.GetTableType(RefTableManagerFullName); } }
    }
}

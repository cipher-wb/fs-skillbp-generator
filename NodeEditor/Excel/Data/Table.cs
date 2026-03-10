using System.Collections.Generic;
using System.Linq;

namespace NodeEditor
{
    public class Table
    {
        public string Name;
        public string Desc;
        public bool EnableMerge;
        public string TemplateExpr;
        public List<Validation> Validations;
        public bool IsHotfix;
        public int MaxField;
        public List<ExcelMember> Members;
        public List<SubClass> Classes;
        public List<string> Excels;
        public List<TableExtSubClassConfig> tableExtSubClassConfigs;
        private List<ExcelMember> keyMembers;

        /// <summary>
        /// 获取所有的键值组合。
        /// </summary>
        /// <returns></returns>
        public List<ExcelMember> GetKeys()
        {
            keyMembers ??= Members.Where(x => x.Key).ToList();
            return keyMembers;
        }
        /// <summary>
        /// 获取单Key键值，组合键返回空
        /// </summary>
        /// <returns></returns>
        public ExcelMember GetKeySingle()
        {
            var keys = GetKeys();
            return keys.Count == 1 ? keys[0] : null;
        }
        public void Init() { }
    }
}

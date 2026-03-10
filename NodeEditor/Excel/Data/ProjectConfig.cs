using System.Collections.Generic;

namespace NodeEditor
{
    /// <summary>
    /// TableTool-config.json 工程配置数据
    /// 注：仅添加需要字段反序列化记录【按需添加】
    /// http://git.dianhun.cn/alanchen/TableToolFS
    /// </summary>
    public partial class ProjectConfig
    {
        public string PackegeName;
        public string ProtoExt;
        public string ProtoOption;
        public bool AllowAlias;
        public string DataExt;
        public string Placeholder;
        public string TableSeaperator;
        public int MaxColumneNum;
        public bool WriteEnumValue;
        public bool NeedAES;
        public string AESKey;
        public bool SortSheetByIndex;
        public bool CalcHashWithName;
        public string LocalizeTable;
        public string LocalizeName;
        public string LocalizeKey;
        public bool LocalizeRebuild;
        public List<FontInfoConfig> FontInfoConfigList;
        public bool ExportEnumNamePrefix;
        public bool ExportLocalizeStringKey;
        public string LocalizeTextExpr;
        public string CppExportKey;
        public bool Utf8WithBom;
        public bool IncludeSubDirectory;
        public string IgnoreDirectories;
        public bool IncludeCSV;
        public bool SplitConfig;
        public string SplitConfigDir;
        public bool CheckUniqueExpr;
        public bool TextToInteger;
        public List<SubClass> Classes;
        public SortedDictionary<string, Table> Tables;

        public Dictionary<string, ExportSetting> ExportSettings;

        public void Init()
        {
            foreach (var item in Tables)
            {
                item.Value.Init();
            }
            InitAnnotation();
        }

        public Table GetTable(string name)
        {
            Tables.TryGetValue(name, out Table table);
            return table;
        }

        /// <summary>
        /// 判断字段是否为本地化配置字段
        /// </summary>
        /// <param name="typeName">类型名，如：SkillConfig，子类型BubbleGameConfig_MinMaxRange</param>
        /// <param name="memberName">变量名，如：SkillNameKey</param>
        /// <returns></returns>
        public bool IsMemberLocalize(string typeName, string memberName)
        {
            if (string.IsNullOrEmpty(typeName) || string.IsNullOrEmpty(memberName))
            {
                return false;
            }
            // 如：SkillNameKey
            if (!memberName.EndsWith(LocalizeKey))
            {
                return false;
            }
            // 更新为：SkillName
            memberName = memberName.Remove(memberName.Length - LocalizeKey.Length, LocalizeKey.Length);
            // 查找-全局结构
            foreach (var item in Classes)
            {
                if (item.Name == typeName)
                {
                    foreach (var member in item.Members)
                    {
                        if (member.Name == memberName)
                        {
                            return member.Localize;
                        }
                    }
                }
            }
            // 查找-表结构
            foreach (var item in Tables)
            {
                var table = item.Value;
                // 如果是表格类型
                if (table.Name == typeName)
                {
                    foreach (var member in table.Members)
                    {
                        if (memberName == member.Name)
                        {
                            return member.Localize;
                        }
                    }
                }
                else
                {
                    // 再查找表格子类型
                    foreach (var subClass in table.Classes)
                    {
                        if (typeName == subClass.GetClassName(table.Name))
                        {
                            foreach(var member in table.Members)
                            {
                                if (memberName == member.Name)
                                {
                                    return member.Localize;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}

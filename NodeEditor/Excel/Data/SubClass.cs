using Newtonsoft.Json;
using System.Collections.Generic;

namespace NodeEditor
{
    /// <summary>
    /// 子类型
    /// </summary>
    public class SubClass
    {
        public string Name;
        public char Seaperator;
        public string Desc;
        public bool Localize;
        public string PropertyFontInfos;
        public bool IsHotfix;
        public bool Enum;
        public List<Member> Members;
        public List<EnumInfo> EnumInfos;
        [JsonIgnore]
        public bool HasSeaperater => Seaperator != '\0' && Seaperator != ' ';
        [JsonIgnore]
        public string AliasName => Name;

        public string GetClassName(string parentClassName)
        {
            return $"{parentClassName}_{Name}";
        }
    }
}

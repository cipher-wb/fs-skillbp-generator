using Newtonsoft.Json;

namespace NodeEditor
{
    /// <summary>
    /// 成员类型
    /// </summary>
    public class Member
    {
        public string Type;
        public string Name;
        public string DefaultValue;
        public char Seaperator;
        public string Desc;
        public string AliasName;
        public bool Localize;
        public bool LocalizeCombine;
        public bool NotNull;
        public string MaxValue;
        public string MinValue;
        public string FromFieldExpr;
        public bool SkipEmpty;
        public bool TrimValue;
        public string ResRef;
        public string PropertyFontInfos;
        [JsonIgnore]
        public bool HasSeaperater => Seaperator != '\0' && Seaperator != ' ';

        public virtual void Init() { }
    }
}

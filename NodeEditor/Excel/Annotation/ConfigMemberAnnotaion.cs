namespace NodeEditor
{
    /// <summary>
    /// 表格子结构类型
    /// </summary>
    public enum TableClassType
    {
        None,
        Table,
        Local,
        Global,
    }
    /// <summary>
    /// Config.json记录对象成员变量信息
    /// </summary>
    public class ConfigMemberAnnotaion
    {
        public string Colume;
        public int FieldIndex;
        public string Type;
        public string Name;
        public string Seaperator;
        public bool Enum;
        public bool Localize;
        public TableClassType ClassType;
        // 数据替代表达式
        public string FieldExpr;
        // 是否跳过空值
        public bool SkipEmpty;
        //与另一张表的关系
        public FromFiledAnnotation FromFiled;
    }
}

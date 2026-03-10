using System.Collections.Generic;

namespace NodeEditor
{
    /// <summary>
    /// Config.json记录对象信息，包括表结构及自定义子结构
    /// </summary>
    public class ConfigJsonAnnotation
    {
        public ConfigMemberAnnotaion Annotaion;
        public bool IsSubClass { get { return Annotaion != null; } }
        //是否可以显示为input output
        public List<RefPortAnnotation> RefPorts = new List<RefPortAnnotation>();
        public List<ConfigMemberAnnotaion> Members = new List<ConfigMemberAnnotaion>();
        public Dictionary<string, FromFiledAnnotation> FromField = new Dictionary<string, FromFiledAnnotation>();
    }
}

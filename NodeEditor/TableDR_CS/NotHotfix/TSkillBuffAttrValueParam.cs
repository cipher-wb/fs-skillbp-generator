#if UNITY_EDITOR

namespace TableDR
{
    public partial class TSkillBuffAttrValueParam
    {
        public string GetNodeViewDesc()
        {
            return Param.ParamType == TParamType.TPT_NULL ? 
                $"[属性][{(int)AttrType}_{AttrType.GetDescription(false)}_{Param.Value}]" : 
                $"[属性][{(int)AttrType}_{AttrType.GetDescription(false)}_{Param.Value}_{Param.ParamType.GetDescription(false)}]";
        }
    }
}
#endif
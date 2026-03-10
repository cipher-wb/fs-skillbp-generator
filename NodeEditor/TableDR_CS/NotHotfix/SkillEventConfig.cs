#if UNITY_EDITOR

using System.Collections.Generic;

namespace TableDR
{
    public partial class SkillEventConfig
    {
        // 方便获取参数，缓存下列表数据
        private List<string> paramNameList;
        public List<string> ParamNameList
        {
            get
            {
                if (paramNameList == null)
                {
                    paramNameList = new List<string>
                            {
                                ParamName1,
                                ParamName2,
                                ParamName3,
                                ParamName4,
                                ParamName5,
                                ParamName6,
                                ParamName7,
                                ParamName8,
                                ParamName9,
                                ParamName10,
                                ParamName11,
                                ParamName12,
                            };
                }
                return paramNameList;
            }
        }

        private string paramNames;
        public string ParamNames
        {
            get
            {
                if (paramNames == null)
                {
                    paramNames = $"事件名：{Name}";
                    for (int i = 0, length = ParamNameList.Count; i < length; i++)
                    {
                        var paramName = ParamNameList[i];
                        if (string.IsNullOrEmpty(paramName))
                        {
                            break;
                        }
                        paramNames += $"\n\t{i + 1}-{paramName}";
                    }
                }
                return paramNames;
            }
        }
        private string subTypeNames;
        public string SubTypeNames
        {
            get
            {
                if (subTypeNames == null)
                {
                    subTypeNames = string.Empty;
                    foreach (var type in this.EventSubType)
                    {
                        if (string.IsNullOrEmpty(subTypeNames))
                        {
                            subTypeNames = $"\n有效事件子类型：\n\t{type.GetDescription(false)}";
                        }
                        else
                        {
                            subTypeNames += $" | {type.GetDescription(false)}";
                        }
                    }
                }
                return subTypeNames;
            }
        }
    }
}

#endif
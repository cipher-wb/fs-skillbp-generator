namespace NodeEditor
{
    /// <summary>
    /// 参数数据转化中间数据结构接口
    /// </summary>
    public interface IConfigTransData
    {
        bool CheckError();
        bool ToConfig();
        bool ToData();
    }

    /// <summary>
    /// ConfigBaseNode对应的参数转化数据类型
    /// </summary>
    public abstract class ConfigBaseTransData : IConfigTransData
    {
        public ConfigBaseTransData(ConfigBaseNode baseNode)
        {
            BaseNode = baseNode;
        }

        public ConfigBaseNode BaseNode { get; protected set; }
        public abstract bool CheckError();
        public abstract bool ToData();
        public abstract bool ToConfig();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace NodeEditor.PortType
{
    #region 基类定义
    public interface IPortType { }
    public class ConfigPortType : IPortType { }
    #endregion

    public sealed class TablePortTypesHelper
    {
        //public static Type DefaultPortType = typeof(IPortType);

        /// <summary>
        /// key：表类型 value:port类型
        /// </summary>
        private static Dictionary<Type, Type> config2PortType = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> config2InputPortType = new Dictionary<Type, Type>();
        private static Dictionary<string, Type> nodeName2NodePortType = new Dictionary<string, Type>();

        private static Dictionary<string, Type> portName2Types;
        public static Dictionary<string, Type> PortName2Types
        {
            get
            {
                if (portName2Types == null)
                {
                    portName2Types = new Dictionary<string, Type>();
                    var baseType = typeof(IPortType);
                    var portTypes = typeof(TablePortTypesHelper).Assembly.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)).ToArray();
                    foreach (var portType in portTypes)
                    {
                        portName2Types[portType.Name] = portType;
                    }
                }
                return portName2Types;
            }
        }

        public static string NameSpace => typeof(TablePortTypesHelper).Namespace;

        /// <summary>
        /// 获取portType
        /// </summary>
        /// <param name="type">configType</param>
        /// <returns></returns>
        public static Type GetTypeInput(Type type)
        {
            if (type == null) return null;

            if (!config2InputPortType.TryGetValue(type, out var portType))
            {
                var typeHelper = typeof(TablePortTypesHelper);
                var typeName = type.Name;
                var interfaceName = GetConfigInterfaceName(typeName, true);
                portType = typeHelper.Assembly.GetType(interfaceName);
                if (portType == null)
                {
                    portType = TableHelper.GetTableTypeByName(typeName);
                    if (portType == null)
                    {
                        Log.Fatal($"{Constants.BaseConfigAnnotaionSheetName} : 缺少配置类型 {typeName}");
                    }
                }
                config2InputPortType.Add(type, portType);
            }
            return portType;
        }
        public static Type GetType(Type type)
        {
            if (type == null) return null;

            if (!config2PortType.TryGetValue(type, out var portType))
            {
                var typeHelper = typeof(TablePortTypesHelper);
                var typeName = type.Name;
                var fullTyepeName = GetConfigPortTypeClassName(typeName, true);
                portType = typeHelper.Assembly.GetType(fullTyepeName);
                if (portType == null)
                {
                    portType = TableHelper.GetTableTypeByName(typeName);
                    if (portType == null)
                    {
                        Log.Fatal($"{Constants.BaseConfigAnnotaionSheetName} : 缺少配置类型 {typeName}");
                    }
                }
                config2PortType.Add(type, portType);
            }
            return portType;
        }

        /// <summary>
        /// 获取编辑器相关节点类型
        /// </summary>
        /// <param name="name">类型名，如：TSET_RUN_SKILL_EFFECT_TEMPLATE，或者对应表格节点，如：SkillConfig</param>
        /// <returns></returns>
        public static Type GetType(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            var typeName = GetConfigPortTypeClassName(name);
            if (PortName2Types.TryGetValue(typeName, out var portType))
            {
                return portType;
            }
            return null;
        }

        public static string GetConfigInterfaceName(string configName, bool isFullName = false)
        {
            return isFullName ? $"{NameSpace}.I{configName}" : $"I{configName}";
        }

        public static string GetConfigPortTypeClassName(string configName, bool isFullName = false)
        {
            return isFullName ? $"{NameSpace}.{nameof(ConfigPortType)}_{configName}" : $"{nameof(ConfigPortType)}_{configName}";
        }
    }
}

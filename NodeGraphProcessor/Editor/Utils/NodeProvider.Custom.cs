using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace GraphProcessor
{
    public partial class NodeProvider
    {
        struct ScriptData
        {
            public string Path;
            public MonoScript MonoScript;
        }
        /// <summary>
        /// 自定义代码路径搜索
        /// </summary>
        static readonly List<string> scriptDirs = new List<string>
        {
            "Assets/Thirds/NodeEditor",
            "Assets/Thirds/NodeGraphProcessor",
        };
        static Dictionary<string, string> scriptName2Files;
        static readonly bool isUseCustomFind = true;
        static MonoScript FindScriptFromClassNameCustom(string className)
        {
            if (scriptName2Files == null)
            {
                scriptName2Files = new Dictionary<string, string>();
                foreach (var scriptDir in scriptDirs)
                {
                    var filePaths = Directory.GetFiles(scriptDir, $"*.cs", SearchOption.AllDirectories);
                    foreach (var filePath in filePaths)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(filePath);
                        scriptName2Files[fileName] = filePath;
                    }
                }
            }
            if (scriptName2Files.Count == 0)
                return null;

            if (scriptName2Files.TryGetValue($"{className}.Custom", out var scriptPath))
            {
                return AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            }
            else if (scriptName2Files.TryGetValue(className, out scriptPath))
            {
                return AssetDatabase.LoadAssetAtPath<MonoScript>(scriptPath);
            }
            return null;
        }
    }
    // TODO GetCustomAttribute有一定消耗
    public static class CustomAttributeHelper
    {
        /// <summary>
        /// Cache Data
        /// </summary>
        private static readonly Dictionary<string, string> cache = new Dictionary<string, string>();

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="T">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<T>(this Type sourceType, Func<T, string> attributeValueAction) where T : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, null);
        }

        /// <summary>
        /// 获取CustomAttribute Value
        /// </summary>
        /// <typeparam name="T">Attribute的子类型</typeparam>
        /// <param name="sourceType">头部标有CustomAttribute类的类型</param>
        /// <param name="attributeValueAction">取Attribute具体哪个属性值的匿名函数</param>
        /// <param name="name">field name或property name</param>
        /// <returns>返回Attribute的值，没有则返回null</returns>
        public static string GetCustomAttributeValue<T>(this Type sourceType, Func<T, string> attributeValueAction,
            string name) where T : Attribute
        {
            return GetAttributeValue(sourceType, attributeValueAction, name);
        }

        private static string GetAttributeValue<T>(Type sourceType, Func<T, string> attributeValueAction,
            string name) where T : Attribute
        {
            var key = BuildKey(sourceType, name);
            if (!cache.ContainsKey(key))
            {
                CacheAttributeValue(sourceType, attributeValueAction, name);
            }

            return cache[key];
        }

        /// <summary>
        /// 缓存Attribute Value
        /// </summary>
        private static void CacheAttributeValue<T>(Type type,
            Func<T, string> attributeValueAction, string name)
        {
            var key = BuildKey(type, name);

            var value = GetValue(type, attributeValueAction, name);

            lock (key + "_attributeValueLockKey")
            {
                if (!cache.ContainsKey(key))
                {
                    cache[key] = value;
                }
            }
        }

        private static string GetValue<T>(Type type,
            Func<T, string> attributeValueAction, string name)
        {
            object attribute = null;
            if (string.IsNullOrEmpty(name))
            {
                attribute =
                    type.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            }
            else
            {
                var propertyInfo = type.GetProperty(name);
                if (propertyInfo != null)
                {
                    attribute =
                        propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }

                var fieldInfo = type.GetField(name);
                if (fieldInfo != null)
                {
                    attribute = fieldInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                }
            }

            return attribute == null ? null : attributeValueAction((T)attribute);
        }

        /// <summary>
        /// 缓存Collection Name Key
        /// </summary>
        private static string BuildKey(Type type, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return type.FullName;
            }

            return type.FullName + "." + name;
        }
    }
}

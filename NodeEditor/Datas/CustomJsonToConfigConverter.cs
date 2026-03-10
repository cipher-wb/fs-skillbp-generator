using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    public class CustomJsonConverter<TConfig> : JsonConverter where TConfig : class, new()
    {
        private Type configType;
        private TConfig config = new TConfig();
        public CustomJsonConverter()
        {
            configType = typeof(TConfig);
        }

        //是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        public override bool CanRead => true;
        //是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        public override bool CanWrite => false;

        private bool TryCustomReadBefore(object propParant, PropertyInfo prop, JToken propJObject)
        {
            #region 处理多语言文本替换 屏蔽
            //try
            //{
            //    var localizeKey = ExcelManager.Inst.ProjectConfig.LocalizeKey;
            //    var editorName = "Editor";
            //    if (propParant is ITable && prop.Name.EndsWith(editorName))
            //    {
            //        var localizePropName = prop.Name.Substring(0, prop.Name.Length - editorName.Length) + localizeKey;
            //        if (propParant.GetType().GetProperty(localizePropName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) != null)
            //        {
            //            // 忽略编辑器文本设置
            //            return true;
            //        }
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error($"TryCustomReadBefore error, ex:{ex}");
            //}
            #endregion

            return false;
        }

        /// <summary>
        /// 自定义处理表结构变化
        /// 常规新增可不用自定义处理，结构修改需要自行处理下数据
        /// 如：TEntityType -> List<TEntityType>
        /// </summary>
        private bool TryCustomRead(object propParant, PropertyInfo prop, JToken propJObject)
        {
            #region 处理多语言文本替换 屏蔽
            //try
            //{
            //    var localizeKey = ExcelManager.Inst.ProjectConfig.LocalizeKey;
            //    if (propParant is ITable && prop.Name.EndsWith(localizeKey))
            //    {
            //        var localizeEditorPropName = prop.Name.Substring(0, prop.Name.Length - localizeKey.Length) + "Editor";
            //        propParant.ExSetValue(localizeEditorPropName, propJObject.ToString());
            //        Log.Debug($"TryCustomRead success, localizeEditorPropName : {localizeEditorPropName}, {propJObject}");
            //        return true;
            //    }
            //    return false;
            //}
            //catch (Exception ex)
            //{
            //    Log.Error($"TryCustomRead error, ex:{ex}");
            //}
            #endregion

            return false;
        }

        public void ReadJsonRecursive(JObject jObject, object propParant)
        {
            if (jObject == null || propParant == null)
            {
                return;
            }
            foreach (var prop in propParant.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
            {
                var propJObject = jObject[prop.Name];
                if (propJObject == null)
                {
                    continue;
                }
                var propType = prop.PropertyType;
                try
                {
                    var propValue = propJObject.ToObject(propType);
                    if (propValue is JObject subJObjct)
                    {
                        var subObject = prop.GetValue(propParant);
                        if (subObject == null)
                        {
                            subObject = Activator.CreateInstance(propType);
                        }
                        prop.SetValue(propParant, subObject);
                        ReadJsonRecursive(subJObjct, subObject);
                    }
                    else if (propValue != null)
                    {
                        var subProps = propType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                        if (propType.Namespace == configType.Namespace && subProps.Length > 0)
                        {
                            // 递归解析表格子类型
                            if (!(propJObject is JObject subObj))
                            {
                                subObj = JObject.Parse(propJObject.ToString());
                            }
                            ReadJsonRecursive(subObj, propValue);
                        }
                        else if (propType.IsGenericType &&
                            (propType.GetGenericTypeDefinition() == typeof(List<>) || 
                             propType.GetGenericTypeDefinition() == typeof(IReadOnlyList<>) || 
                             propType.GetGenericTypeDefinition() == typeof(ReadOnlyCollection<>)) 
                            && propValue is IEnumerable collection)
                        {
                            var itemType = propType.GetGenericArguments()[0];
                            if (itemType.IsClass)
                            {
                                // 递归处理List数据
                                var enu = collection.GetEnumerator();
                                var enuJ = propJObject.AsJEnumerable().GetEnumerator();
                                while (enu.MoveNext() && enuJ.MoveNext())
                                {
                                    if (!(enuJ.Current is JObject subObj))
                                    {
                                        subObj = JObject.Parse(enuJ.Current.ToString());
                                    }
                                    ReadJsonRecursive(subObj, enu.Current);
                                }
                            }
                        }
                        if (!TryCustomReadBefore(propParant, prop, propJObject))
                        {
                            // 可能存在自定义参数，这边限制下
                            if (prop.SetMethod != null)
                            {
                                prop.SetValue(propParant, propValue);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!TryCustomRead(propParant, prop, propJObject))
                    {
                        throw new System.Exception($"[表结构变化] {configType.Name}.{prop.Name} 新类型:{propType.Name}\n{ex.ToString()}");
                    }
                }
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(TConfig) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //获取JObject对象，该对象对应着我们要反序列化的json
            var jObject = serializer.Deserialize<JObject>(reader);
            ReadJsonRecursive(jObject, config);
            return config;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    public class ForceMutableContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (member is PropertyInfo property)
            {
                // 检查是否有私有 setter
                var hasPrivateSetter = property.GetSetMethod(true) != null;
                prop.Writable = hasPrivateSetter; // 强制设为可写
            }
            return prop;
        }
    }
}

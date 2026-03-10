using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using TableDR;

namespace NodeEditor
{
    /// <summary>
    /// 表格解析数据
    /// </summary>
    [Serializable]
    public class ConfigParseData
    {
        public static readonly ConfigParseData Empty = new ConfigParseData();
        [NonSerialized]
        public bool Valid = false;
        /// <summary>
        /// 表格名
        /// </summary>
        public string ConfigName { get; set; }
        /// <summary>
        /// 表格版本号
        /// </summary>
        public string TableTash { get; set; }
        /// <summary>
        /// 表格ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 单条表格数据映射
        /// </summary>
        public Dictionary<string, string> Data { get; set; }
        /// <summary>
        /// 单条表格数据
        /// </summary>
        public object Config { get; set; }

        public Type GetConfigType()
        {
            var type = typeof(SkillConfig);
            return TableHelper.GetTableType($"{type.Namespace}.{ConfigName}");
        }
        public void Deconstruct(out string id, out Dictionary<string, string> memberDataMap)
        {
            memberDataMap = this.Data;
            id = this.ID;
        }

        public bool WriteJson(string dir)
        {
            if (!Valid)
            {
                return false;
            }
            var path = $"{dir}/{ConfigName}_{ID}.json";
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
            return true;
        }
    }

    public class CustomJsonConverter<TObject, TConfig> : JsonConverter where TObject : NodeEditor.ConfigParseData, new() where TConfig : class
    {
        private Type configType = typeof(TConfig);
        private TObject configParseData = new TObject();

        //是否开启自定义反序列化，值为true时，反序列化时会走ReadJson方法，值为false时，不走ReadJson方法，而是默认的反序列化
        public override bool CanRead => true;
        //是否开启自定义序列化，值为true时，序列化时会走WriteJson方法，值为false时，不走WriteJson方法，而是默认的序列化
        public override bool CanWrite => false;

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
                var propValue = propJObject.ToObject(prop.PropertyType);
                if (propValue is JObject subJObjct)
                {
                    var propType = prop.PropertyType;
                    if (prop.Name == nameof(configParseData.Config))
                    {
                        propType = configType;
                    }
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
                    if (prop.PropertyType.Namespace == configType.Namespace &&
                        prop.PropertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).Length > 0)
                    {
                        var subJObjct2 = JObject.Parse(propJObject.ToString());
                        ReadJsonRecursive(subJObjct2, propValue);
                    }
                    prop.SetValue(propParant, propValue);
                }
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ConfigParseData) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //获取JObject对象，该对象对应着我们要反序列化的json
            var jObject = serializer.Deserialize<JObject>(reader);
            ReadJsonRecursive(jObject, configParseData);
            return configParseData;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GraphProcessor;
using System.Linq;
using NodeEditor.PortType;
using System.Reflection;
using System.ComponentModel;

namespace NodeEditor
{
    public partial class ConfigBaseNode<T>
    {
        /// <summary>
        /// 依据表格配置信息，预处理数据，参数预生成、类型预设置
        /// 注：预设期间graph未指定，相关操作应放在enable之后处理
        /// </summary>
        private void Preset()
        {
            if (createdFromDuplication)
            {
                // 复制的节点需要对config反序列化
                Deserialize();
            }
            else
            {
                // 不是复制的节点，需要预设信息
                OnPreset();
            }
            OnPresetPropertyList();
            // 依据规定自动生成ID
            isCreateNode2GenerateID = true;
            //GenerateID();
            //Serialize();
            //RefreshData();
        }

        // 预生成List成员，Odin存在bug，list新建选择可能存在错误
        protected void OnPresetPropertyList()
        {
            Utils.SafeCall(() =>
            {
                var subProps = ConfigType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty);
                foreach (var subProp in subProps)
                {
                    if (subProp.SetMethod != null && 
                        (subProp.PropertyType.HasImplementedRawGeneric(typeof(List<>)) ||
                         subProp.PropertyType.HasImplementedRawGeneric(typeof(IReadOnlyList<>)) ||
                         subProp.PropertyType.HasImplementedRawGeneric(typeof(ReadOnlyCollection<>))) && 
                        subProp.GetValue(Config) == null)
                    {
                        object value = null;
                        if (subProp.PropertyType.Name.Contains("IReadOnlyList"))
                        {
                            Type genericArgument = subProp.PropertyType.GetGenericArguments()[0]; // 得到泛型参数 T (int)
                            Type newListType = typeof(List<>).MakeGenericType(genericArgument);
                            value = Activator.CreateInstance(newListType);
                        }
                        else
                        {
                            value = Activator.CreateInstance(subProp.PropertyType);
                        }
                        subProp.SetValue(Config, value);
                    }

                    //添加默认值处理
                    var configTableKeys = ExcelManager.Inst.ProjectConfig.GetTable(name).Members;
                    configTableKeys?.ForEach(member =>
                    {
                        if (string.IsNullOrEmpty(member.DefaultValue)) { return; }

                        //Int
                        if (member.Type == typeof(int).Name && !member.HasSeaperater)
                        {
                            if (int.TryParse(member.DefaultValue, out int defaultValue))
                            {
                                var value = (int)Config.ExGetValue(member.Name);
                                if (defaultValue != value)
                                {
                                    Config.ExSetValue(member.Name, defaultValue);
                                }
                            }
                        }
                        //bool
                        else if (member.Type == typeof(bool).Name && !member.HasSeaperater)
                        {
                            if (bool.TryParse(member.DefaultValue, out bool defaultValue))
                            {
                                var value = (bool)Config.ExGetValue(member.Name);
                                if (defaultValue != value)
                                {
                                    Config.ExSetValue(member.Name, defaultValue);
                                }
                            }
                        }
                    });
                }
            });
        }
        protected virtual void OnPreset()
        {
            DoPresetParams();
        }

        /// <summary>
        /// 节点排序规则，从上到下排序
        /// </summary>
        public int SortInputNodes(SerializableEdge a, SerializableEdge b)
        {
            if (a == null || b == null || a.inputNode == null || b.inputNode == null)
            {
                return 0;
            }
            return a.inputNode.position.y.CompareTo(b.inputNode.position.y);
        }

        #region Input Port Custom ID
        [CustomPortBehavior(nameof(ID))]
        public IEnumerable<PortData> CustomPortBehavior_ID(List<SerializableEdge> edges)
        {
            yield return CustomPortBehavior_ID();
        }

        protected virtual PortData CustomPortBehavior_ID()
        {
            // TODO
            var refPortType = TablePortTypesHelper.GetTypeInput(ConfigType);
            if (refPortType == null)
            {
                refPortType = ConfigType;
                Log.Error($"InputPortBehavior_ID failed, {ConfigType.Name}");
            }
            return new PortData
            {
                displayName = $"{nameof(ID)}:{ID}",
                displayType = refPortType,
                identifier = "0",
                acceptMultipleEdges = false,
                portColor = TableAnnotation.Inst.GetNodeColor(ConfigType),
            };
        }

        /// <summary>
        /// 节点输入输出与理解上不同，是输入几点数据传到输出节点，与默认相反
        /// </summary>
        /// <param name="edges"></param>
        [CustomPortInput(nameof(ID), typeof(int), true)]
        public void CustomPortInput_ID(List<SerializableEdge> edges)
        {
            OnCustomPortInput_ID(edges);
        }
        protected virtual void OnCustomPortInput_ID(List<SerializableEdge> edges)
        {
            if (BaseTransData != null)
            {
                BaseTransData.ToData();
                BaseTransData.CheckError();
            }

            if (edges.Count != 1)
            {
                return;
            }
            var edge = edges.First();
            var outputNodeType = edge.outputNode.GetType();
            if (outputNodeType.HasImplementedRawGeneric(typeof(ConfigBaseNode<>)))
            {
                edges.First().passThroughBuffer = ID;
            }
            else
            {
                Log.Error($"{GetLogPrefix()}GetInputs failed, {outputNodeType.Name}");
            }
        }

        // TODO 后面优化
        private string inputName;
        protected string InputName
        {
            get
            {
                if (inputName == null)
                {
                    inputName = ExcelManager.Inst.GetTableKey(GetConfigName());
                }
                return inputName;
            }
        }
        #endregion

        #region   Port Custom Members
        /// <summary>
        /// 可扩展暴露接口
        /// </summary>
        protected virtual List<PortData> DoGetMembersAnnotationPortDatas()
        {
            var cacheList = new List<PortData>();
            var configName = GetConfigName();
            var configJson = ExcelManager.Inst.ProjectConfig.GetConfigJson(configName);

            if (configJson != null)
            {
                foreach (var member in configJson.RefPorts)
                {
                    var membersLink = member.identifier.Split('.');
                    var memberValue = 0;
                    object memberObject = Config;
                    var memberName = string.Empty;
                    for (int i = 0, length = membersLink.Length; i < length; i++)
                    {
                        if (memberObject == null)
                        {
                            break;
                        }
                        memberName = membersLink[i];
                        var subProp = memberObject.GetType().GetProperty(memberName);
                        if (subProp  == null)
                        {
                            Log.Fatal($"注意！表格：{configName}存在结构不统一情况，检查表目录config.json与C#代码是不是匹配！建议整体更新再打开编辑器！\n如果已更新，过一会儿等自动化导表再更新尝试！");
                            continue;
                        }
                        // TODO 注意config.json更新，但是c#代码未更新情况，给个报错
                        var subObjectValue = subProp.GetValue(memberObject);
                        // 如果是最后变量
                        if (i == length - 1)
                        {
                            if (subObjectValue is int subValue)
                            {
                                memberValue = subValue;
                            }
                        }
                        else
                        {
                            memberObject = subObjectValue;
                        }
                    }
                    var displayName = member.displayName;
                    if (!string.IsNullOrEmpty(memberName))
                    {
                        if (member.displayType.IsEnum)
                        {
                            // 枚举，直接显示枚举的描述
                            var desc = Utils.GetEnumDescription(member.displayType, memberValue);
                            displayName = $"{displayName}: [{desc}:{memberValue}]";
                        }
                        else
                        {
                            displayName = $"{member.displayName}: [{memberValue}]";
                        }
                    }
                    cacheList.Add(new PortData
                    {
                        displayName = displayName,
                        displayType = member.displayType,
                        identifier = member.identifier,
                        acceptMultipleEdges = member.acceptMultipleEdges,
                        portColor = member.portColor,
                    });
                }
            }
            cacheList.AddRange(CustomPortDatas);
            return cacheList;
        }
        private List<PortData> GetMembersAnnotationPortDatas()
        {
            //if (membersPortDataCache == null)
            {
                membersPortDataCache = DoGetMembersAnnotationPortDatas();
            }
            return membersPortDataCache;
        }

        [CustomPortBehavior(nameof(PackedMembersOutput))]
        public IEnumerable<PortData> CustomPortBehavior_PackedMembersOutput(List<SerializableEdge> edges)
        {
            var portDatas = GetMembersAnnotationPortDatas();
            foreach (var portData in portDatas)
            {
                yield return portData;
            }
            yield break;
        }

        [CustomPortOutput(nameof(PackedMembersOutput), typeof(int), true)]
        public void CustomPortOutput_PackedMembersOutput(List<SerializableEdge> edges, NodePort outputPort)
        {
            OnCustomPortOutput_PackedMembersOutput(edges, outputPort);
        }

        /// <summary>
        /// 当一个节点没有连线时要不要设置为null
        /// </summary>
        public virtual bool IsPortNoEdgeSetNull => true;

        private HashSet<string> hasChangeCustomPorts = new HashSet<string>();
        protected virtual void OnCustomPortOutput_PackedMembersOutput(List<SerializableEdge> edges, NodePort outputPort)
        {
            try
            {
                if (!this.graph.isEnabled)
                {
                    // 初始化期间不做数据处理
                    return;
                }
                var memberNameAnnotation = outputPort.portData.identifier;
                var propertyMap = PackedMembersOutput.propertyMap;
                if (propertyMap == null)
                {
                    propertyMap = PackedMembersOutput.propertyMap = new Dictionary<string, PropertyInfo>();
                }
                propertyMap.Clear();
                if (outputPort.portData.acceptMultipleEdges)
                {
                    var prop = ConfigType.GetProperty(memberNameAnnotation);
                    if (prop != null)
                    {
                        if (edges.Count == 0)
                        {
                            if (IsPortNoEdgeSetNull)
                            {
                                prop.SetValue(Config, null);
                            }
                        }
                        else
                        {
                            edges.Sort(SortInputNodes);
                            var propValue = prop.GetValue(Config);
                            if (propValue == null)
                            {
                                if (prop.PropertyType.Name.Contains("IReadOnlyList"))
                                {
                                    Type genericArgument = prop.PropertyType.GetGenericArguments()[0]; // 得到泛型参数 T (int)
                                    Type newListType = typeof(List<>).MakeGenericType(genericArgument);
                                    propValue = Activator.CreateInstance(newListType);
                                }
                                else
                                {
                                    propValue = Activator.CreateInstance(prop.PropertyType);
                                }
                                prop.SetValue(Config, propValue);
                            }
                            switch (propValue)
                            {
                                case List<int> propListInt:
                                    propListInt.Clear();
                                    foreach (var edge in edges)
                                    {
                                        if (edge.passThroughBuffer is int intValue)
                                        {
                                            propListInt.Add(intValue);
                                        }
                                    }
                                    break;
                            }
                        }
                        propertyMap[memberNameAnnotation] = prop;
                    }
                    else
                    {
                        Log.Error($"PushOutputs_PackedMembersOutput 找不到成员类型 ：{memberNameAnnotation}");
                    }
                }
                else
                {
                    var membersLink = memberNameAnnotation.Split('.');
                    object memberObject = Config;
                    var memberName = string.Empty;
                    for (int i = 0, length = membersLink.Length; i < length; i++)
                    {
                        memberName = membersLink[i];
                        var subProp = memberObject.GetType().GetProperty(memberName);
                        var subObjectValue = subProp.GetValue(memberObject);
                        if (subObjectValue == null)
                        {
                            if (subProp.PropertyType.Name.Contains("IReadOnlyList"))
                            {
                                Type genericArgument = subProp.PropertyType.GetGenericArguments()[0]; // 得到泛型参数 T (int)
                                Type newListType = typeof(List<>).MakeGenericType(genericArgument);
                                subObjectValue = Activator.CreateInstance(newListType);
                            }
                            else
                            {
                                subObjectValue = Activator.CreateInstance(subProp.PropertyType);
                            }
                            memberObject.ExSetValue(memberName, subObjectValue);
                        }
                        if (i != length - 1)
                        {
                            memberObject = subObjectValue;
                        }
                    }
                    var prop = memberObject?.GetType().GetProperty(memberName);
                    if (prop != null)
                    {
                        propertyMap[memberNameAnnotation] = prop;
                        // 依据连线设置数值
                        if (edges.Count == 1 && edges.First().passThroughBuffer is int intValue)
                        {
                            prop.SetValue(memberObject, intValue);
                            if (!hasChangeCustomPorts.Contains(memberNameAnnotation))
                            {
                                hasChangeCustomPorts.Add(memberNameAnnotation);
                            }

                        }
                        else if (edges.Count == 0)
                        {
                            if (hasChangeCustomPorts.Contains(memberNameAnnotation))
                            {
                                hasChangeCustomPorts.Remove(memberNameAnnotation);
                                // 不重置数据，如，手填数据
                                prop.SetValue(memberObject, 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()}成员类型解析异常\n{ex}");
            }
        }
        #endregion

        /// <summary>
        /// 向配置表写入数据
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetConfigValue(string propertyName, object value, bool isDoConfigChange = false)
        {
            try
            {
                ConfigType?.GetProperty(propertyName)?.SetValue(Config, value);
                if (isDoConfigChange)
                {
                    OnConfigChanged();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }

        /// <summary>
        /// 获取配置表数据
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object GetConfigValue(string propertyName)
        {
            try
            {
                return ConfigType?.GetProperty(propertyName)?.GetValue(Config) ?? null;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// 获取配置表描述
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetConfigDecription(string propertyName)
        {
            return Utils.GetDescription(Config, propertyName);
        }

        /// <summary>
        /// 向配置表子类型写入数据
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public void SetSpecifyConfigValue(object specifyData, string specifyPropertyName, object value)
        {
            try
            {
                var childType = specifyData.GetType();
                childType?.GetProperty(specifyPropertyName)?.SetValue(specifyData, value);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
        }
    }
}

using GraphProcessor;
using NodeEditor.PortType;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;
using TableDR;

namespace NodeEditor
{
    public partial class ConfigBaseNode<T>
    {
        public override string GetNodeAnnotation()
        {
            string nodeAnno = null;
            try
            {
                nodeAnno = base.GetNodeAnnotation();
                if (GetConfig() == null)
                {
                    return nodeAnno;
                }
                else
                {
                    var annoTips = string.Empty;
                    var anno = GetParamsAnnotation();
                    if (anno != null && !string.IsNullOrEmpty(anno.tips))
                    {
                        annoTips = $"\n\n{anno.tips}";
                    }
                    var name = ConfigIDManager.Inst.GetCreatorName(GetConfigName(), GetConfigID());
                    return $"【 节点负责人: {name} 】\n\n{nodeAnno}{annoTips}";
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"GetNodeAnnotation error, {nodeAnno}, ex:{ex}");
                return nodeAnno;
            }
        }

        #region TParam params
        public virtual ParamsAnnotation GetParamsAnnotation() { return null; }
        public virtual string GetParamsName() { return null; }
        public virtual IReadOnlyList<TParam> GetParamsList() { return null; }
        public void RefreshParamsDisplayName()
        {
            try
            {
                var annos = GetParamsAnnotation();
                var paramList = GetParamsList();
                if (paramList != null && annos != null && annos.paramsAnn.Count <= paramList.Count)
                {
                    TParamAnnotation anno = null;
                    for (int i = 0, length = paramList.Count; i < length; i++)
                    {
                        var tParam = paramList[i];
                        if (!annos.IsArray || annos.ArrayStart > i)
                        {
                            anno = annos.paramsAnn.ExGet(i);
                        }
                        else
                        {
                            anno = annos.paramsAnn.ExGet(annos.ArrayStart);
                        }
                        var annoOrign = anno;
                        var isTemplate = TryGetTemplateAnntation(anno, tParam, out var annoT, out var errorT);
                        if (isTemplate)
                        {
                            anno = annoT;
                        }
                        if (anno != null)
                        {
                            anno.CheckTParam(tParam, out var displayName, out var error);
                            if (!string.IsNullOrEmpty(errorT))
                            {
                                error = errorT;
                            }
                            if (annos.IsArray && i >= annos.ArrayStart)
                            {
                                var nodeName = string.Empty;
                                foreach (var edge in GetOutputEdges())
                                {
                                    if (edge.inputNode is ConfigBaseNode inputNode && inputNode.ID == tParam.Value)
                                    {
                                        nodeName = inputNode.GetCustomName();
                                        break;
                                    }
                                }
                                displayName = $"{displayName}-{nodeName}";
                            }
                            else
                            {
                                if (isTemplate)
                                {
                                    // 正常情况，拼下原始描述
                                    displayName = $"{i + 1}-{annoOrign.GetName()}-{displayName}";
                                }
                                else
                                {
                                    displayName = $"{i + 1}-{displayName}";
                                }
                            }
                            tParam?.RefreshDisplay(displayName, error);
                        }
                        else
                        {
                            Log.Error($"{GetLogPrefix()}RefreshParamsDisplayName failed, i: {i}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Fatal($"{GetLogPrefix()}RefreshParamsDisplayName failed\n{ex}");
            }
        }
        private bool TryGetTemplateAnntation(TParamAnnotation anno, TParam tParam, out TParamAnnotation annoTemplate, out string error)
        {
            annoTemplate = null;
            error = null;
            if (tParam?.ParamType == TParamType.TPT_EXTRA_PARAM)
            {
                // 额外参数，父节点是模板节点的话，显示模板参数描述
                var node = FindParentTemplateNode(this);
                if (node != null)
                {
                    // 逻辑默认从1开始
                    annoTemplate = node.TemplateParams.ExGet(tParam.Value - 1, null);
                    if (annoTemplate != null)
                    {
                        return true;
                    }
                    else
                    {
                        error = $"模板额外参数索引错误:{tParam.Value}";
                        return false;
                    }
                }
            }
            else if (tParam?.ParamType == TParamType.TPT_EVENT_PARAM)
            {
                // 技能消息参数，父节点是技能消息节点的话，显示技能消息参数描述
                var node = FindParentSkillEventNode(this);
                if (node != null)
                {
                    var paramsList = node.GetParamsList();
                    if (paramsList != null && paramsList.Count > 0)
                    {
                        var skillEventConfig = DesignTable.GetTableCell<SkillEventConfig>(paramsList[0].Value);
                        if (skillEventConfig != null)
                        {
                            // 技能消息参数名列表
                            var paramNameList = skillEventConfig.ParamNameList;

                            string paramValue = string.Empty;
                            if (tParam.Value > 0 && tParam.Value <= paramNameList.Count)
                            {
                                paramValue = paramNameList[tParam.Value - 1];
                            }

                            if (string.IsNullOrEmpty(paramValue))
                            {
                                error = $"技能消息参数索引错误:{tParam.Value}";
                                return false;
                            }

                            // 逻辑默认从1开始
                            annoTemplate = new TParamAnnotation()
                            {
                                Name = paramValue,
                                DefaultValueDesc = ""
                            };
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取第一个父节点为模版节点
        /// </summary>
        private ConfigBaseNode FindParentTemplateNode(ConfigBaseNode findNode)
        {
            foreach (var edge in findNode.GetInputEdges())
            {
                if (edge.inputFieldName == nameof(ID))
                {
                    if (edge.outputNode is ConfigBaseNode configBaseNode)
                    {
                        if (configBaseNode.IsTemplate)
                        {
                            // 找到模板节点
                            return configBaseNode;
                        }
                        else
                        {
                            return FindParentTemplateNode(configBaseNode);
                        }
                    }
                    break;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取第一个父节点为注册技能消息节点
        /// </summary>
        private TSET_REGISTER_SKILL_EVENT FindParentSkillEventNode(ConfigBaseNode findNode)
        {
            foreach (var edge in findNode.GetInputEdges())
            {
                if (edge.inputFieldName == nameof(ID))
                {
                    // 技能消息也可能存在额外参数调用，这边屏蔽
                    if (edge.outputNode is TSET_REGISTER_SKILL_EVENT skillEventNode)
                    {
                        // 注册技能消息连出去的节点，只有某几个才能获取“技能消息参数”
                        bool bFind = false;
                        int index = 0;
                        foreach (var port in skillEventNode.outputPorts)
                        {
                            // 1技能效果 、 3筛选 、 4条件
                            if (index == 1 || index == 3 || index == 4)
                            {
                                foreach (var portEdge in port.GetEdges())
                                {
                                    if (portEdge == edge)
                                    {
                                        bFind = true;
                                        break;
                                    }
                                }
                            }
                            if (bFind)
                            {
                                break;
                            }
                            index++;
                        }

                        if (bFind)
                            return skillEventNode;
                        else
                            return FindParentSkillEventNode(skillEventNode);
                    }
                    if (edge.outputNode is ConfigBaseNode configBaseNode)
                    {
                        return FindParentSkillEventNode(configBaseNode);
                    }
                    break;
                }
            }
            return null;
        }

        /// <summary>
        /// 预生产参数列表，个数类型等
        /// </summary>
        private void DoPresetParams()
        {
            var anno = GetParamsAnnotation();
            var paramName = GetParamsName();
            if (anno != null && !string.IsNullOrEmpty(paramName))
            {
                var paramsList = new List<TParam>();
                if (!anno.IsArray)
                {
                    // 非数组类型自动扩展参数数量
                    for (int i = 0, count = anno.paramsAnn.Count; i < count; i++)
                    {
                        // 按照预设默认值创建
                        var paramAnn = anno.paramsAnn[i];
                        var tParam = paramAnn.CopyDefaultParam();
                        paramsList.Add(tParam);
                    }
                }
                else
                {
                    for (int i = 0, count = (anno.ArrayStart + 1); i < count; i++)
                    {
                        // 按照预设默认值创建
                        var paramAnn = anno.paramsAnn[i];
                        var tParam = paramAnn.CopyDefaultParam();
                        paramsList.Add(tParam);
                    }

                }
                Config.ExSetValue(paramName, paramsList);
            }
        }
        /// <summary>
        /// 默认参数检查
        /// 1-参数定义变多，自动扩展
        /// 2-参数减少，默认给个提示，不做处理（需要CustomParamsProcess实现对应参数变化逻辑）
        /// </summary>
        private void CheckParams()
        {
            if (CustomParamsPostProcessing())
            {
                return;
            }
            var anno = GetParamsAnnotation();
            var paramName = GetParamsName();
            if (anno != null && !anno.IsArray && !string.IsNullOrEmpty(paramName))
            {
                var paramObj = Config.ExGetValue(paramName);
                if (paramObj is List<TParam> paramsList)
                {
                    var curCount = paramsList.Count;
                    var newCount = anno.paramsAnn.Count;
                    if (paramsList.Count < anno.paramsAnn.Count)
                    {
                        // 参数个数扩大，那么自动“向后”扩充下参数个数
                        // 特殊需求自行扩展展开
                        for (int i = curCount; i < newCount; i++)
                        {
                            // 按照预设默认值创建
                            var paramAnn = anno.paramsAnn[i];
                            var tParam = paramAnn.CopyDefaultParam();
                            paramsList.Add(tParam);
                        }
                        // warning提示，默认扩增按照默认值
                        Log.Warning($"{GetLogPrefix()}参数个数增多，原个数:{curCount}, 新个数:{newCount}");
                    }
                    else if (paramsList.Count > anno.paramsAnn.Count)
                    {
                        // 参数个数减少，非程序暂时不处理，给个报错，避免错误处理数据
                        Log.Fatal($"{GetLogPrefix()}参数个数减少，原个数:{curCount}, 新个数:{newCount}");
                        if (LocalSettings.IsProgramer())
                        {
                            // 删除操作仅限程序，避免错误删除
                            for (int i = paramsList.Count - 1; i >= anno.paramsAnn.Count; --i)
                            {
                                paramsList.RemoveAt(i);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 参数变化处理逻辑
        /// 如：参数个数减少，对应数据需要变化处理
        /// </summary>
        /// <returns>是否自定义处理</returns>
        protected virtual bool CustomParamsPostProcessing() { return false; }
        protected virtual List<PortData> DoGetParamsAnnotationPortDatas(List<SerializableEdge> edges)
        {
            var customPorts = new List<PortData>();
            var anno = GetParamsAnnotation();
            var tempStr = GetParamsName();

            if (anno != null)
            {
                List<TParam> paramsList = null;

                if (!string.IsNullOrEmpty(tempStr))
                {
                    paramsList = Config.ExGetValue(tempStr) as List<TParam>;
                }

                if (anno.IsArray)
                {
                    // 如果是不定长数组，先处理前面的固定参数，后面不固定的就按照顺序塞
                    if (anno.paramsAnn.Count > 0)
                    {
                        for (int i = 0, n = anno.paramsAnn.Count; i < n; i++)
                        {
                            TParamAnnotation paramAnno = null;
                            Type refPortType = null;
                            Type refType = null;
                            string displayName = string.Empty;
                            bool multipleEdges = true;
                            if (i < anno.ArrayStart)
                            {
                                paramAnno = anno.paramsAnn[i];
                                refPortType = paramAnno.GetRefPortType(TParamAnnotation.DefaultPortType);
                                var tParam = paramsList?.ExGet(i, null);
                                var paramAnnoOrign = paramAnno;
                                var isTemplate = TryGetTemplateAnntation(paramAnno, tParam, out var annoT, out _);
                                if (isTemplate)
                                {
                                    paramAnno = annoT;
                                }
                                displayName = paramAnno.GetDisplayName(tParam, true);
                                if (isTemplate)
                                {
                                    displayName = $"{paramAnnoOrign.GetName()}-{displayName}";
                                }
                                refType = paramAnno.GetRefType(null);
                                multipleEdges = false;
                            }
                            else
                            {
                                paramAnno = anno.paramsAnn[anno.ArrayStart];
                                refPortType = paramAnno.GetRefPortType(TParamAnnotation.DefaultPortType);
                                refType = paramAnno.GetRefPortType(TParamAnnotation.DefaultRefType);
                                displayName = paramAnno.GetName();
                            }
                            customPorts.Add(new PortData
                            {
                                displayName = displayName,
                                displayType = refPortType,
                                identifier = i.ToString(),
                                acceptMultipleEdges = multipleEdges,
                                portColor = TableAnnotation.Inst.GetNodeColor(refType),
                            });
                            if (multipleEdges)
                            {
                                break;
                            }
                        }
                    }
                }
                else
                {
                    int count = anno.paramsAnn.Count;
                    if (count > 0)
                    {
                        // 固定规则参数，按照顺序匹配规则设置数值
                        for (int i = 0; i < count; i++)
                        {
                            var paramAnno = anno.paramsAnn[i];
                            var refPortType = paramAnno.GetRefPortType(TParamAnnotation.DefaultPortType);
                            var tParam = paramsList?.ExGet(i, null);
                            var paramAnnoOrign = paramAnno;
                            var isTemplate = TryGetTemplateAnntation(paramAnno, tParam, out var annoT, out _);
                            if (isTemplate)
                            {
                                paramAnno = annoT;
                            }
                            var displayName = paramAnno.GetDisplayName(tParam, true);
                            if (isTemplate)
                            {
                                displayName = $"{paramAnnoOrign.GetName()}-{displayName}";
                            }
                            var refType = paramAnno.GetRefType(null);
                            customPorts.Add(new PortData
                            {
                                displayName = displayName,
                                displayType = refPortType,
                                identifier = i.ToString(),
                                acceptMultipleEdges = false,
                                portColor = TableAnnotation.Inst.GetNodeColor(refType),
                            });
                        }
                    }
                }
            }
            return customPorts;
        }
        protected List<PortData> GetParamsAnnotationPortDatas(List<SerializableEdge> edges)
        {
            //if (paramsPortDataCache == null)
            {
                paramsPortDataCache = DoGetParamsAnnotationPortDatas(edges);
            }
            return paramsPortDataCache;
        }
        #endregion

        #region Output Port Custom Params

        [CustomPortOutput(nameof(PackedParamsOutput), typeof(int), true)]
        public void PushOutputs_PackedParamsOutput(List<SerializableEdge> edges, NodePort outputPort)
        {
            UpdatePackedParamsOutput(edges, outputPort);
        }


        int[] hasChangePorts = default;
        /// <summary>
        /// 刷新表格数据
        /// </summary>
        /// <param name="edges"></param>
        /// <param name="outputPort"></param>
        protected virtual void UpdatePackedParamsOutput(List<SerializableEdge> edges, NodePort outputPort)
        {
            try
            {
                if (graph && !graph.isEnabled)
                {
                    // 初始化期间不做数据处理
                    return;
                }
                // 按照pos排序，如，顺序执行需求
                edges.Sort(SortInputNodes);
                // 依据输入缓存数据
                //PackedParamsOutput.ids = edges.Select(e => { return (int)e.passThroughBuffer; }).ToList();
                //PackedParamsOutput.names = edges.Select(e => { return (e.inputNode as IConfigBaseNode).GetConfigName(); }).ToList();
                //PackedParamsOutput.nodeGUIDs = edges.Select(e => { return e.inputNode.GUID; }).ToList();

                var anno = GetParamsAnnotation();
                var paramName = GetParamsName();
                var paramsList = GetParamsList();
                if (anno != null && !string.IsNullOrEmpty(paramName))
                {
                    int index = int.Parse(outputPort.portData.identifier);

                    if (anno.IsArray && index >= anno.ArrayStart)
                    {
                        if (paramsList == null)
                        {
                            paramsList = new List<TParam>();
                            Config.ExSetValue(paramName, paramsList);
                        }
                        var edgesCount = edges.Count;
                        var paramsCount = paramsList.Count - anno.ArrayStart;

                        if (paramsCount > edgesCount)
                        {
                            paramsList.GetListRef().RemoveRange(anno.ArrayStart + edgesCount, paramsCount - edgesCount);
                        }
                        else if (paramsCount < edgesCount)
                        {
                            for (int i = 0; i < edgesCount - paramsCount; i++)
                            {
                                paramsList.GetListRef().Add(new TParam());
                            }
                        }
                        // 如果是不定长数组，那么就按照顺序塞
                        if (anno.paramsAnn.Count > 0)
                        {
                            var annoParam = anno.paramsAnn[anno.ArrayStart];
                            var refType = annoParam.GetRefType(TParamAnnotation.DefaultRefType);
                            var configName = refType.Name;
                            string portTypes = annoParam.RefPortTypeFullName;
                            if (string.IsNullOrEmpty(portTypes))
                            {
                                portTypes = configName;
                            }
                            // TODO 手动配置数据丢失，需要支持下
                            for (int i = 0; i < edgesCount; i++)
                            {
                                var edge = edges[i];
                                var tParam = paramsList.ExGet(anno.ArrayStart + i);
                                if (edge.inputNode is IConfigBaseNode configNode &&
                                    (configName == configNode?.GetConfigName() ||
                                    portTypes.Contains(configNode?.GetConfigName())))
                                {
                                    if (edge.passThroughBuffer != null)
                                    {
                                        var id = edge.passThroughBuffer;
                                        var type = tParam.ParamType;
                                        if ((annoParam.isConfigId || annoParam.isEnum) && !annoParam.IsFunctionReturn)
                                        {
                                            // 表格、枚举并且节点类型未自定义，那么设置为TPT_NULL
                                            type = TParamType.TPT_NULL;
                                        }
                                        else if (/*annoParam.IsFunctionReturn && */edge.inputPort.portData.displayType == typeof(ISkillEffectConfig))
                                        {
                                            // 如果是连了效果节点，那么作为返回值处理
                                            type = TParamType.TPT_FUNCTION_RETURN;
                                        }
                                        tParam.ExSetValue(nameof(tParam.Value), id);
                                        // 连接默认认为，参数类型是函数返回值
                                        tParam.ExSetValue(nameof(tParam.ParamType), type);
                                        // TODO Factor手动修改
                                        //tParam.ExSetValue(nameof(tParam.Factor), 0);
                                    }
                                    else
                                    {
                                        Log.Error($"{GetLogPrefix()}连线数据丢失，参数{anno.ArrayStart + i + 1})");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (paramsList != null)
                        {
                            if (paramsList.Count > 0)
                            {
                                if (hasChangePorts == default || hasChangePorts.Length == 0)
                                {
                                    if (anno.IsArray)
                                    {
                                        hasChangePorts = new int[anno.ArrayStart];
                                    }
                                    else
                                    {
                                        hasChangePorts = new int[paramsList.Count];
                                    }
                                }

                                var tParam = paramsList.ExGet(index);
                                var annoParam = anno.paramsAnn.ExGet(index);
                                if (tParam != null)
                                {
                                    int value = tParam.Value;
                                    var type = tParam.ParamType;
                                    if (edges.Count == 1)
                                    {
                                        var edge = edges[0];
                                        if (edge.passThroughBuffer is int intValue)
                                        {
                                            value = intValue;
                                        }
                                        if (annoParam != null && (annoParam.isConfigId || annoParam.isEnum) && !annoParam.IsFunctionReturn)
                                        {
                                            // 表格、枚举并且节点类型未自定义，那么设置为TPT_NULL
                                            type = TParamType.TPT_NULL;
                                        }
                                        else if (/*annoParam.IsFunctionReturn && */edge.inputPort.portData.displayType == typeof(ISkillEffectConfig))
                                        {
                                            // 如果是连了效果节点，那么作为返回值处理
                                            type = TParamType.TPT_FUNCTION_RETURN;
                                        }
                                    }
                                    else if (hasChangePorts.Length > index && hasChangePorts[index] != edges.Count && edges.Count == 0)
                                    {
                                        value = 0;
                                        type = TParamType.TPT_NULL;
                                    }
                                    tParam.ExSetValue(nameof(tParam.Value), value);
                                    // 连接默认认为，参数类型是函数返回值
                                    tParam.ExSetValue(nameof(tParam.ParamType), type);
                                }
                                //else
                                //{
                                //    Log.Error($"{GetLogPrefix()}参数节点数据索引错误，identifier:{outputPort.portData.identifier}, paramsList.Count:{paramsList.Count}");
                                //}

                                if (hasChangePorts.Length > index)
                                    hasChangePorts[index] = edges.Count;
                            }
                            else
                            {
                                // TODO 暂时屏蔽 异常情况暂不影响
                                //Log.Error($"{GetLogPrefix()}参数节点数据索引解析错误，identifier:{outputPort.portData.identifier}, paramsList.Count:{paramsList.Count}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{GetLogPrefix()}参数类型解析异常\n{ex}");
            }
        }

        [CustomPortBehavior(nameof(PackedParamsOutput))]
        public IEnumerable<PortData> OutputPortBehavior(List<SerializableEdge> edges)
        {
            var portDatas = GetParamsAnnotationPortDatas(edges);
            if (portDatas != null)
            {
                foreach (var portData in portDatas)
                {
                    yield return portData;
                }
            }
            yield break;
        }
        #endregion
    }
}

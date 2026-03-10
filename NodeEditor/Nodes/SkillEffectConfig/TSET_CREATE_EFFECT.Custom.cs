using GraphProcessor;
using System;
using System.Collections.Generic;
using TableDR;

namespace NodeEditor
{
    public partial class TSET_CREATE_EFFECT
    {
        //protected override bool CustomParamsPostProcessing()
        //{
        //    try
        //    {
        //        // 处理下创建特效参数删除
        //        var anno = GetParamsAnnotation();
        //        var paramName = GetParamsName();
        //        if (anno != null && !anno.IsArray && !string.IsNullOrEmpty(paramName))
        //        {
        //            var paramObj = Config.ExGetValue(paramName);
        //            if (paramObj is List<TParam> paramsList)
        //            {
        //                var curCount = paramsList.Count;
        //                var newCount = anno.paramsAnn.Count;
        //                if (curCount - newCount == 1)
        //                {
        //                    // 删除第二个参数
        //                    int deleteIndex = 1;
        //                    // 按顺序数值往上覆盖处理
        //                    for (int i = deleteIndex; i < curCount - 1; i++)
        //                    {
        //                        // 更新数值
        //                        paramsList[i] = paramsList[i + 1];
        //                        // 更新连线
        //                        var outport1 = outputPorts.ExGet(i);
        //                        // 如果存在连线节点，那么更新连线
        //                        var edges1 = new List<SerializableEdge>(outport1.GetEdges());
        //                        foreach (var edge1 in edges1)
        //                        {
        //                            outport1.Remove(edge1);
        //                        }
        //                        var outport2 = outputPorts.ExGet(i + 1);
        //                        if (outport2 != null)
        //                        {
        //                            var edges2 = outport2.GetEdges();
        //                            foreach (var edge2 in edges2)
        //                            {
        //                                edge2.outputPort = outport1;
        //                                edge2.outputPortIdentifier = i.ToString();
        //                                outport1.Add(edge2);
        //                                edge2.SyncDatas();
        //                            }
        //                        }
        //                    }
        //                    // 删除最后一个
        //                    int lastIndex = curCount - 1;
        //                    paramsList.RemoveAt(lastIndex);
        //                    var outportLast = outputPorts.ExGet(lastIndex);
        //                    if (outportLast != null)
        //                    {
        //                        var edgesLast = outportLast.GetEdges();
        //                        if (edgesLast.Count > 0)
        //                        {
        //                            var edgesLastCopy = new List<SerializableEdge>(edgesLast);
        //                            foreach (var edgeLastCopy in edgesLastCopy)
        //                            {
        //                                outportLast.Remove(edgeLastCopy);
        //                            }
        //                        }
        //                    }
        //                    UpdateAllPortsLocal();
        //                    Log.Error($"{GetLogPrefix()}自适应删除第{deleteIndex}个参数，{curCount}->{newCount}");
        //                    return true;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Exception(ex);
        //    }
        //    return base.CustomParamsPostProcessing();
        //}
    }
}

using System.Collections.Generic;
using Funny.Base.Utils;
using GraphProcessor;
using TableDR;

namespace NodeEditor
{
    public interface ITemplateReceiverNode
    {
        /// <summary>
        /// 该模板节点是否有效
        /// </summary>
        /// <returns></returns>
        bool IsValidTemplate(out int templateId);

        /// <summary>
        /// 模板参数描述文件缓存
        /// </summary>
        ParamsAnnotation TemplateParamsAnnotation { get; }

        /// <summary>
        /// 模板开始参数索引
        /// </summary>
        int TemplateBeginIndex { get; }

        /// <summary>
        /// 模板端口索引
        /// </summary>
        int TemplatePortIndex { get; }

        ///// <summary>
        ///// 模板引用路径
        ///// </summary>
        //string TemplatePath { get; set; }

        ///// <summary>
        ///// 模板节点
        ///// </summary>
        //BaseNode TemplateNode { get; set; }
        ITemplateNodeData TemplateNodeData { get; }
    }
    public static class TemplateNodeHelper
    {
        public static ParamsAnnotation GetParamsAnnotationTemplate<T>(this T node) 
            where T : ConfigBaseNode, IConfigBaseNode, IParamsNode, ITemplateReceiverNode
        {
            if (node == null) return null;

            var anno = node.TemplateParamsAnnotation;
            if (anno != null)
            {
                for (int i = anno.paramsAnn.Count - 1; i >= node.TemplateBeginIndex; --i)
                {
                    anno.paramsAnn.RemoveAt(i);
                }
                foreach (var edge in node.GetOutputEdges())
                {
                    if (edge.outputPortIdentifier == node.TemplatePortIndex.ToString())
                    {
                        if (node.IsEdgeTemplateWithParams(edge, out var templateNode))
                        {
                            foreach (var param in templateNode.TemplateParams)
                            {
                                anno.paramsAnn.Add(param);
                            }
                        }
                        break;
                    }
                }
            }
            return anno;
        }
        /// <summary>
        /// 自定义模板参数同步
        /// </summary>
        /// <returns></returns>
        public static bool CustomParamsPostProcessingTemplate<T>(this T node) 
            where T : ConfigBaseNode, IConfigBaseNode, IParamsNode, ITemplateReceiverNode
        {
            if (node == null) return true;

            // TODO 需注意参数顺序改变问题
            var paramsList = node.GetParamsList();
            var anno = node.GetParamsAnnotation();
            if (paramsList == null || anno == null)
            {
                return true;
            }
            var curCount = paramsList.Count;
            var newCount = anno.paramsAnn.Count;
            if (paramsList.Count < anno.paramsAnn.Count)
            {
                for (int i = curCount; i < newCount; i++)
                {
                    // 按照预设默认值创建
                    var paramAnn = anno.paramsAnn[i];
                    var tParam = paramAnn.CopyDefaultParam();
                    paramsList.GetListRef().Add(tParam);
                }
            }
            else if (paramsList.Count > anno.paramsAnn.Count)
            {
                for (int i = paramsList.Count - 1; i >= anno.paramsAnn.Count; --i)
                {
                    paramsList.GetListRef().RemoveAt(i);
                }
            }
            //数量相同，设置一下默认值
            //else if(paramsList.Count > 3 && curCount == newCount)
            //{
            //    for (int i = 2; i < curCount; i++)
            //    {
            //        // 按照预设默认值创建
            //        var paramAnn = anno.paramsAnn[i];
            //        var tParam = paramsList[i];

            //        if(tParam?.Value == 0 && paramAnn?.DefalutParam?.Value != 0)
            //        {
            //            tParam.SetValue(paramAnn.DefalutParam.Value);
            //        }
            //    }
            //}
            return true;
        }
        /// <summary>
        /// 是否连上模板节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="edge"></param>
        /// <param name="templateNode"></param>
        /// <returns></returns>
        private static bool IsEdgeTemplate<T>(this T node, SerializableEdge edge, out ConfigBaseNode templateNode) 
            where T : ConfigBaseNode, IConfigBaseNode, IParamsNode, ITemplateReceiverNode
        {
            if (node == edge.outputNode)
            {
                if (edge.inputNode is ConfigBaseNode configBaseNode && configBaseNode.IsTemplate)
                {
                    templateNode = configBaseNode;
                    return true;
                }
                if (edge.inputNode is RefTemplateBaseNode refTemplateBaseNode && refTemplateBaseNode.NodeRef is ConfigBaseNode configBaseNodeRef && configBaseNodeRef.IsTemplate)
                {
                    templateNode = configBaseNodeRef;
                    return true;
                }
            }
            templateNode = null;
            return false;
        }

        private static bool IsEdgeTemplateWithParams<T>(this T node, SerializableEdge edge, out ConfigBaseNode templateNode)
            where T : ConfigBaseNode, IConfigBaseNode, IParamsNode, ITemplateReceiverNode
        {
            if (node.IsEdgeTemplate(edge, out templateNode))
            {
                if (templateNode.TemplateParams?.Count > 0)
                {
                    return true;
                }
            }
            templateNode = null;
            return false;
        }
    }
}

using GraphProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Graphs;

namespace NodeEditor
{
    public struct TemplateGraphDescription
    {
        public static readonly TemplateGraphDescription Empty = new TemplateGraphDescription();
        public string Path;
        public ConfigBaseNode Node;
        //public BaseGraph Graph;

        public bool Valid { get { return Node != null; } }
        public int ID { get { return Node?.ID ?? 0; } }
        public List<TParamAnnotation> TemplateParams { get { return Node?.TemplateParams ?? null; } }

        public static TemplateGraphDescription Create(BaseGraph graph)
        {
            var desc = new TemplateGraphDescription();
            //desc.Graph = graph;
            if (graph != null)
            {
                desc.Path = graph.path;
                desc.Node = graph.nodes.Find((n) => { return n is ConfigBaseNode configBaseNode && configBaseNode.IsTemplate; }) as ConfigBaseNode;
            }
            return desc;
        }
    }

    public static class TemplateProvider
    {
        // TODO 可能存在未卸载模板情况，即会导致Reload Script后触发Graph OnEnable逻辑，耗时较大，需要优化处理下
        static Dictionary<Type, Dictionary<string, TemplateGraphDescription>> cacheTemplate = new Dictionary<Type, Dictionary<string, TemplateGraphDescription>>();
        static TemplateProvider()
        {
            BuildTemplateCache();
        }
        static void BuildTemplateCache()
        {
            // TODO 缓存模板
        }

        public static TemplateGraphDescription Get(Type graphType, string path, bool forceRfresh = false)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                return TemplateGraphDescription.Empty;
            }
            TemplateGraphDescription desc;
            if (!cacheTemplate.TryGetValue(graphType, out var descMap))
            {
                descMap = new Dictionary<string, TemplateGraphDescription>();
                cacheTemplate.Add(graphType, descMap);
            }
            if (!forceRfresh && descMap.TryGetValue(path, out desc))
            {
                return desc;
            }
            else
            {
                var graph = GraphHelper.LoadGraph(graphType, path);
                desc = TemplateGraphDescription.Create(graph);
                if(desc.Node != null)
                    descMap[path] = desc;
                return desc;
            }
        }

        /// <summary>
        /// 避免频繁使用，使用会有卡顿的问题，频繁使用使用ConfigIDManager.Inst.IsTemplate接口
        /// </summary>
        /// <param name="graphType"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsTemplate(Type graphType,string path)
        {
            var desc = Get(graphType, path);
            if(desc.Node == null) 
                return false;
            return true;
        }

        /// <summary>
        /// 获取所有已经打开且有该模板节点的Node集合,如果要优化可以把这些节点监听记录下来
        /// </summary>
        /// <param name="templateType">模板节点类型</param>
        /// <returns></returns>
        public static List<ITemplateReceiverNode> GetGraphTemplateNodes(string oldPath,List<BaseNode> baseNodes = null)
        {
            var templates = new List<ITemplateReceiverNode>();
            if (baseNodes != null && baseNodes.Count> 0)
            {
                SetTemplates(baseNodes);
            }
            else
            {
                ConfigGraphWindow[] windows = Utils.GetAllWindow<ConfigGraphWindow>();

                for (int i = windows.Length - 1; i >= 0; i--)
                {
                    var window = windows[i];
                    var graph = window.GetGraph();
                    if (graph == null)
                    {
                        Log.Error($"GetGraphTemplateNodes error, window name:{window.name}");
                        continue;
                    }
                    SetTemplates(graph.nodes);
                }
            }

            return templates;

            void SetTemplates(List<BaseNode> nodes)
            {
                if(nodes.Count== 0) return;

                for (int j = nodes.Count - 1; j >= 0; j--)
                {
                    var node = nodes[j];

                    if (!(node is ITemplateReceiverNode templateNode) || templateNode.TemplateNodeData == null)
                        continue;


                    if (templateNode.TemplateNodeData.GetTemplatePath() == oldPath)
                        templates.Add(templateNode);
                }
            }
        }

        public static bool RefreshAllWindowTemplateNodes(string oldPath, List<TParamAnnotation> paramAnnotations = null)
        {
            if (string.IsNullOrEmpty(oldPath))
                return false;

            var templates = GetGraphTemplateNodes(oldPath);

            if (templates.Count == 0) return false;

            for (int i= templates.Count - 1; i >= 0; i--)
            {
                var templateNode = templates[i];

                if (paramAnnotations != null)
                {
                    var templateGraphDescription = templateNode.TemplateNodeData.GetTemplateGraphInfo();
                    //templateGraphDescription.Node.TemplateParams = paramAnnotations;
                    //templateNode.TemplateNodeData.SetTemplateGraphDescription(templateGraphDescription);
                }

                templateNode.TemplateNodeData.RefreshGraphRef(oldPath);
            }

            return true;
        }

        public static bool RefreshAllWindowTemplateNodes(string oldPath,string newPath,List<BaseNode> baseNodes = null)
        {
            if (string.IsNullOrEmpty(oldPath) || string.IsNullOrEmpty(newPath))
                return false;

            var templates = GetGraphTemplateNodes(oldPath, baseNodes);

            if(templates.Count == 0) return false;

            for (int i = templates.Count - 1; i >= 0; i--)
            {
                var templateNode = templates[i];

                templateNode.TemplateNodeData.RefreshGraphRef(newPath);
            }

            return true;
        }
    }
}

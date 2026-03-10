using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GraphProcessor
{
    public partial class BaseNode
    {
        [HideInInspector]
        public Vector2 hidePos;

        [HideInInspector]
        public int hideCounter;

        public virtual string GetNodeAnnotation() => GetCustomName();
        public virtual string GetNodeSearchName() => GetCustomName();

        /// <summary>
        /// Triggered after node position is updated
        /// </summary>
        public virtual void OnNodePostionUpdated() { }

        public void SetPosition(Rect newPos) { position = newPos; OnNodePostionUpdated(); }

        /// <summary>
        ///  获取所有OutPort的所有孩子节点，仅当前，不会遍历寻找
        /// </summary>
        /// <param name="childNodes"></param>
        public void GetChildNodes(List<BaseNode> childNodes)
        {
            foreach (var outport in outputPorts)
            {
                foreach (var edge in outport.GetEdges())
                {
                    var inputNode = edge.inputNode;
                    childNodes.Add(inputNode);
                }
            }
        }

        /// <summary>
        /// 获取某个OutPort的所有孩子节点，仅当前，不会遍历寻找
        /// </summary>
        /// <typeparam name="TPort"></typeparam>
        /// <typeparam name="TNode"></typeparam>
        /// <returns></returns>
        public List<TNode> GetChildNodes<TPort, TNode>(string portIdentifier = "") where TNode : BaseNode 
        {
            List<TNode> childNodes = new List<TNode>();
            foreach (var outport in outputPorts)
            {
                var portType = typeof(TPort);
                if (outport.portData.displayType != portType)
                {
                    continue;
                }
                if(!string.IsNullOrEmpty(portIdentifier) && !outport.portData.identifier.Equals(portIdentifier))
                {
                    continue;
                }
                foreach (var edge in outport.GetEdges())
                {
                    var inputNode = edge.inputNode;
                    if(inputNode is TNode myNode)
                    {
                        childNodes.Add(myNode);
                    }
                }
            }
            return childNodes;
        }
    }
}

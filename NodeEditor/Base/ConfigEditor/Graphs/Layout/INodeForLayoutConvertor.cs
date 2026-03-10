using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NodeEditor
{
    public interface INodeForLayoutConvertor
    {
        /// <summary>
        /// 节点间的距离
        /// </summary>
        float SiblingDistance { get; }
        /// <summary>
        /// 自定义的Node实例引用
        /// </summary>
        object PrimRootNode { get; }

        /// <summary>
        /// 算法中的Node结构
        /// </summary>
        NodeAutoLayouter.TreeNode LayoutRootNode { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="primRootNode"></param>
        /// <returns></returns>
        INodeForLayoutConvertor Init(object primRootNode);

        /// <summary>
        /// 将自定义节点树转换为算法中的节点树
        /// </summary>
        /// <returns></returns>
        NodeAutoLayouter.TreeNode PrimNode2LayoutNode();

        /// <summary>
        /// 将算法中的节点树转换为自定义节点树
        /// </summary>
        void LayoutNode2PrimNode();
    }
}

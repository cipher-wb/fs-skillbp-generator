using GraphProcessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GraphProcessor
{
    public static class BaseStackExt
    {
        /// <summary>
        /// 获取stack
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nodeView"></param>
        /// <returns></returns>
        public static BaseStackNodeView GetStackByNode(this BaseGraphView self, BaseNodeView nodeView)
        {
            return self.stackNodeViews.Find(x => x.Contains(nodeView));
        }

        public static void AddNodeToStack(this BaseGraphView self, BaseStackNodeView groupView, BaseNodeView nodeView)
        {
            var oldGroupView = GetStackByNode(self, nodeView);
            if (oldGroupView != default)
            {
                return;
            }

            groupView.AddElement(nodeView);
        }

        public static BaseStackNodeView RemoveNodeToStack(this BaseGraphView self, BaseNodeView nodeView)
        {
            var stackView = GetStackByNode(self, nodeView);
            if (stackView == default)
            {
                return default;
            }
            stackView.RemoveElement(nodeView);

            return stackView;
        }
    }
}

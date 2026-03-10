using UnityEditor.Experimental.GraphView;

namespace GraphProcessor
{
    public static class BaseGraphExt
    {

        public static GroupView RemoveNodeToGroup(this BaseGraphView self, GraphElement graphElement)
        {
            GroupView groupView = GetGroupByNode(self, graphElement);
            if (groupView == default)
            {
                return default;
            }
            groupView.RemoveElement(graphElement);

            return groupView;
        }

        public static GroupView GetGroupByNode(this BaseGraphView self, GraphElement graphElement)
        {
            return self.groupViews.Find(x => x.ContainsElement(graphElement));
        }

        public static void AddNodeToGroup(this BaseGraphView self, GroupView groupView, GraphElement graphElement)
        {
            GroupView oldGroupView = GetGroupByNode(self, graphElement);
            if (oldGroupView != default)
            {
                return;
            }

            groupView.AddElement(graphElement);
        }
    }
}

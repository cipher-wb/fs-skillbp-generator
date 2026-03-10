using GraphProcessor;
using UnityEngine.UIElements;

namespace NodeEditor
{
    [NodeCustomEditor(typeof(RefConfigBaseNode))]
    public class RefConfigBaseNodeView : BaseNodeView
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            BuildConfigNodeMenu(evt);
            base.BuildContextualMenu(evt);
        }
        protected void BuildConfigNodeMenu(ContextualMenuPopulateEvent evt)
        {
            if (nodeTarget is IRefConfigBaseNode refNode)
            {
                evt.menu.AppendAction("定位引用节点", (e) =>
                {
                    JsonGraphManager.Inst.TryOpenGraphWithProgressBar(refNode.RefConfigName, refNode.RefConfigID);
                });
            }
            evt.menu.AppendSeparator();
        }
    }
}

using GraphProcessor;
using UnityEditor.Experimental.GraphView;

namespace NodeEditor.SkillEditor
{
    public class SkillGraphToolbarView : ConfigGraphToolbarView
    {
        private SkillGraphWindow skillGraphWindow;

        public SkillGraphToolbarView(SkillGraphWindow skillGraphWindow, BaseGraphView graphView, MiniMap miniMap, BaseGraph baseGraph) 
            : base(skillGraphWindow, graphView, miniMap, baseGraph)
        {
            this.skillGraphWindow = skillGraphWindow;
        }
        protected override void AddButtons()
        {
            base.AddButtons();
        }
    }
}
using UnityEditor;

namespace NodeEditor.SkillEditor
{
    public class SkillGraphView : ConfigGraphView
    {
        public SkillGraphWindow skillGraphWindow;
        public SkillGraphView(EditorWindow window) : base(window)
        {
            skillGraphWindow = window as SkillGraphWindow;
        }
    }
}

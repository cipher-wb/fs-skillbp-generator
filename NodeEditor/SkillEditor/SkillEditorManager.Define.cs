using System.Collections;

namespace NodeEditor.SkillEditor
{
    public partial class SkillEditorManager
    {
        public bool IsInit { get; private set; } = false;
        private static IEnumerator enumeratorRepaint;
    }
}

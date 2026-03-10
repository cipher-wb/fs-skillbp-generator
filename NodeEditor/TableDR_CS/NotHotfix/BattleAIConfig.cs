#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.Linq;
using Funny.Base.Utils;

namespace TableDR
{
    public partial class BattleAIConfig
    {
        private void OnTitleBarGUI_AISkillTagsList()
        {
            // 插件bug，重写下Add处理
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Plus))
            {
                AISkillTagsList.GetListRef().Add(new SkillTagInfo());
            }
        }
        private void OnBeginListElement_AISkillTagsList(int index)
        {
            SirenixEditorGUI.BeginBox(SkillTagInfo.GetTagDesc(AISkillTagsList, index));
        }
        private void OnEndListElement_AISkillTagsList(int index)
        {
            SirenixEditorGUI.EndBox();
        }
    }
}
#endif
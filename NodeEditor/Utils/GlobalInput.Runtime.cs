using UnityEditor;

namespace NodeEditor
{
    public partial class GlobalInput
    {

        #region 运行时全局按键监听

        // Hook与C++调试冲突，屏蔽
        //[UnityEditor.Callbacks.DidReloadScripts]
        //private static void OnScriptsReoload()
        //{
        //    EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
        //    EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        //}
        //private static void OnPlaymodeStateChanged(PlayModeStateChange stateChange)
        //{
        //    switch (stateChange)
        //    {
        //        case PlayModeStateChange.EnteredPlayMode:
        //            SetHook();
        //            break;
        //        case PlayModeStateChange.ExitingPlayMode:
        //            UnsetHook();
        //            break;
        //    }
        //}
        #endregion

    }
}

using System;

namespace NodeEditor
{
    [System.Flags]
    public enum EditorFlag
    {
        None                = 0,
        SaveToAsset         = 1 << 0,
        SaveToExcel         = 1 << 1,
        DisplayProcess      = 1 << 2,
        DisplayDialog       = 1 << 3,
        ShowNotification    = 1 << 4,
        CheckVersion        = 1 << 5,
        DisplayChanged      = 1 << 6,   // 显示文件修改弹窗
        Saving              = 1 << 7,   

        SaveDefault = SaveToAsset | DisplayProcess | DisplayDialog | ShowNotification,

        ExportInspector = SaveToAsset | CheckVersion | SaveToExcel | DisplayProcess | DisplayDialog,
        ExportDefault = SaveToAsset | SaveToExcel | DisplayProcess | DisplayDialog | ShowNotification,
        ExportCheckVersion = SaveToAsset | CheckVersion,
        ExportAll = SaveToAsset | CheckVersion | SaveToExcel | DisplayProcess | DisplayDialog | ShowNotification,
        ExportSilence = SaveToAsset | SaveToExcel,

        GraphSaveFlags = DisplayChanged | ShowNotification,
    }

    public static partial class EnumExtension
    {
        public static void AddFlag(this ref EditorFlag self, EditorFlag flag)
        {
            self |= flag;
        }
        public static void DelFlag(this ref EditorFlag self, EditorFlag flag)
        {
            self &= ~flag;
        }
        public static void ClrFlag(this ref EditorFlag self)
        {
            self = EditorFlag.None;
        }
        public static void SetFlag(this ref EditorFlag self, EditorFlag flag)
        {
            self = flag;
        }
        public static bool DisplayDialog(this EditorFlag self, string title, string message, string ok, string cancel = "", bool defaultResult = true)
        {
            var result = defaultResult;
#if UNITY_EDITOR
            if (self.HasFlag(EditorFlag.DisplayDialog))
            {
                result = UnityEditor.EditorUtility.DisplayDialog(title, message, ok, cancel);
            }
#endif
            Log.Debug($"DisplayDialog[{result}] title:{title}, message:{message}, ok:{ok}, cancel:{cancel}");
            return result;
        }
        public static void DisplayProgressBar(this EditorFlag self, string title, string info, float progress)
        {
#if UNITY_EDITOR
            if (self.HasFlag(EditorFlag.DisplayDialog))
            {
                UnityEditor.EditorUtility.DisplayProgressBar(title, info, progress);
            }
#endif
            Log.Debug($"DisplayProgressBar[{progress.ToString("P")}] title:{title}, info:{info}");
        }
        public static bool DisplayCancelableProgressBar(this EditorFlag self, string title, string info, float progress, bool defaultResult)
        {
            var result = defaultResult;
#if UNITY_EDITOR
            if (self.HasFlag(EditorFlag.DisplayDialog))
            {
                result = UnityEditor.EditorUtility.DisplayCancelableProgressBar(title, info, progress);
            }
#endif
            Log.Debug($"DisplayProgressBar[{progress.ToString("P")}][{result}] title:{title}, info:{info}");
            return result;
        }
    }

    [Flags]
    public enum BatchFlags
    {
        空 = 0,
        保存Graph = 1 << 0,
        导出Excel = 1 << 1,
        清理无效ID = 1 << 2,
    }
}

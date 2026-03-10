using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// 日志重定向相关的实用函数。
    /// </summary>
    internal static class LogRedirection
    {
        private static readonly Regex logRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");
        private const string logPath = "Assets/Thirds/NodeEditor/Log/Log.cs";

        //[OnOpenAsset(0)]
        [EditorMethodRegister(typeof(OnOpenAssetAttribute), "NodeGraph-技能编辑器相关")]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            string selectedStackTrace = GetSelectedStackTrace();
            if (string.IsNullOrEmpty(selectedStackTrace))
            {
                return false;
            }

            if (!selectedStackTrace.Contains(logPath))
            {
                return false;
            }

            Match match = logRegex.Match(selectedStackTrace);
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups[1].Value.Contains("Log.cs") && line == int.Parse(match.Groups[2].Value))
            {
                // 过滤Log
                while (match.Success && match.Groups[1].Value.Contains("Log.cs"))
                {
                    match = match.NextMatch();
                }
                if (!match.Success)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            try
            {
                return InternalEditorUtility.OpenFileAtLineExternal(Path.Combine(Application.dataPath, match.Groups[1].Value.Substring(7)), int.Parse(match.Groups[2].Value));
            }
            catch
            {
                return false;
            }
        }

        private static string GetSelectedStackTrace()
        {
            Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;
            if (editorWindowAssembly == null)
            {
                return null;
            }

            System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
            {
                return null;
            }

            FieldInfo consoleWindowFieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            if (consoleWindowFieldInfo == null)
            {
                return null;
            }

            EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
            if (consoleWindow == null)
            {
                return null;
            }

            if (consoleWindow != EditorWindow.focusedWindow)
            {
                return null;
            }

            FieldInfo activeTextFieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (activeTextFieldInfo == null)
            {
                return null;
            }

            return (string)activeTextFieldInfo.GetValue(consoleWindow);
        }
    }
}

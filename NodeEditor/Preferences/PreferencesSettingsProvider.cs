#if !NodeExport
using System;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    internal class PreferencesSettingsProvider : SettingsProvider
    {
        private const string providerName = "封神/节点编辑器";
        private readonly static string[] providerKeywords = new[] { "Node", "NodeEditor", "节点", "编辑器", "技能", "战斗" };

        private PreferencesSettingsProvider(string path, SettingsScope scope)
            : base(path, scope)
        {

        }

        [SettingsProvider]
        public static SettingsProvider CreateUserInstance()
        {
            return new PreferencesSettingsProvider("Preferences/" + providerName, SettingsScope.User)
            {
                keywords = providerKeywords
            };
        }

        [SettingsProvider]
        public static SettingsProvider CreateProjectInstance()
        {
            return new PreferencesSettingsProvider("Project/" + providerName, SettingsScope.Project)
            {
                keywords = providerKeywords
            };
        }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            if (GUILayout.Button("打开-节点编辑器配置", GUILayout.ExpandWidth(false), GUILayout.Height(30f)))
            {
                NodeEditorWindow.OpenWindow();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            DrawHelpAbout();
        }

        private void DrawHelpAbout()
        {
            EditorGUILayout.LabelField("说明文档:", EditorStyles.boldLabel);

            if (GUILayout.Button("TAPD wiki", GUILayout.MaxWidth(EditorGUIUtility.labelWidth)))
            {
                Application.OpenURL("https://www.tapd.cn/49090545/markdown_wikis/show/#1149090545001003169");
            }

            EditorGUILayout.LabelField("前言:", EditorStyles.boldLabel);
            {
                var urlStyle = new GUIStyle(EditorStyles.label);
                urlStyle.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1.00f, 0.65f, 0.00f) : Color.blue;
                urlStyle.active.textColor = Color.red;

                GUILayout.Label("-主要功能", EditorStyles.boldLabel);
                GUILayout.Label("   逻辑数据可视化");
                GUILayout.Label("   逻辑数据即改即生效");
                EditorGUILayout.Space();

                GUILayout.Label("-环境/插件", EditorStyles.boldLabel);
                if (GUILayout.Button("   Unity2020.3.25f1", urlStyle, GUILayout.ExpandWidth(false)))
                {
                    Application.OpenURL("https://www.tapd.cn/49090545/markdown_wikis/show/#1149090545001002920");
                }
                if (GUILayout.Button("   Odin", urlStyle, GUILayout.ExpandWidth(false)))
                {
                    Application.OpenURL("https://odininspector.com/");
                }
                if (GUILayout.Button("   NodeGraphProcessor 【SHA：bc71d48ba9dfc7e7062779d6d12b9ef269e521b1】", urlStyle, GUILayout.ExpandWidth(false)))
                {
                    Application.OpenURL("https://github.com/alelievr/NodeGraphProcessor");
                }
                if (GUILayout.Button("   TableTool", urlStyle, GUILayout.ExpandWidth(false)))
                {
                    Application.OpenURL("http://git.dianhun.cn/alanchen/TableToolFS");
                }
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
    }
}

#endif
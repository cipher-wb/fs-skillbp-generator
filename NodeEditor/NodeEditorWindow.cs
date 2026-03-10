using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace NodeEditor
{
    public class NodeEditorWindow : OdinMenuEditorWindow, INodeEditorWindow
    {
        #region Event
        private static Action<INodeEditorWindow> onOpenWindow;
        public static event Action<INodeEditorWindow> OnOpenWindow
        {
            add
            {
                onOpenWindow -= value;
                onOpenWindow += value;
            }
            remove { onOpenWindow -= value; }
        }
        private static Action<INodeEditorWindow> onCloseWindow;
        public static event Action<INodeEditorWindow> OnCloseWindow
        {
            add
            {
                onCloseWindow -= value;
                onCloseWindow += value;
            }
            remove { onCloseWindow -= value; }
        }
        #endregion

        private string windowName = Constants.NodeEditorConfigTitle;
        private HashSet<ISetting> settings = new HashSet<ISetting>();
        private StringBuilder sbSaveInfo = new StringBuilder();
        private OdinMenuTree menuTree;

        protected override OdinMenuTree BuildMenuTree()
        {
            menuTree = new OdinMenuTree(supportsMultiSelect: false);

            menuTree.Config.DrawSearchToolbar = true;
            menuTree.Selection.SupportsMultiSelect = true;

            // 本地配置信息
            menuTree.Add(LocalSettings.name, LocalSettings.Inst);
            settings.Add(LocalSettings.Inst);

            // 枚举定义配置
            var enumConfigMenuItem = new OdinMenuItem(menuTree, Constants.EnumConfigTitle, null) { DefaultToggledState = true };
            {
                menuTree.MenuItems.Add(enumConfigMenuItem);
                enumConfigMenuItem.ChildMenuItems.Add(new OdinMenuItem(enumConfigMenuItem.MenuTree, BattleNatureAnnotation.name, TableAnnotation.Inst.BattleNatureAnno));
                enumConfigMenuItem.ChildMenuItems.Add(new OdinMenuItem(enumConfigMenuItem.MenuTree, EntityStateAnnotation.name, TableAnnotation.Inst.EntityStateAnno));
                enumConfigMenuItem.ChildMenuItems.Add(new OdinMenuItem(enumConfigMenuItem.MenuTree, CommonParamAnnotation.name, TableAnnotation.Inst.CommonParamAnnotation));
                enumConfigMenuItem.ChildMenuItems.Add(new OdinMenuItem(enumConfigMenuItem.MenuTree, CommonSkillParamAnnotation.name, TableAnnotation.Inst.CommonSkillParamAnnotation));
            }

            // 节点说明配置
            menuTree.Add(TableAnnotation.name, TableAnnotation.Inst);
            settings.Add(TableAnnotation.Inst);

            // 节点编辑器配置
            var menuNodeEditor = new OdinMenuItem(menuTree, NodeEditorManager.name, NodeEditorManager.Inst) { DefaultToggledState = true };
            {
                menuTree.MenuItems.Add(menuNodeEditor);
                foreach (var manager in GraphHelper.GetEditorManagers())
                {
                    settings.Add(manager.Setting);
                    var menuEditor = new OdinMenuItem(menuNodeEditor.MenuTree, manager.Name, manager);
                    {
                        menuNodeEditor.ChildMenuItems.Add(menuEditor);
                    }
                }
            }

            // 程序操作列表
            var programmerMenuItem = new OdinMenuItem(menuTree, Constants.ProgrammerUseTitle, null) { DefaultToggledState = true };
            {
                menuTree.MenuItems.Add(programmerMenuItem);
                programmerMenuItem.ChildMenuItems.Add(new OdinMenuItem(programmerMenuItem.MenuTree, NodeEditorTool.name, NodeEditorTool.Inst));
            }

            // 排序
            menuTree.EnumerateTree().AddThumbnailIcons().SortMenuItemsByName();

            return menuTree;
        }
        protected override void OnBeginDrawEditors()
        {
            var selectedValue = MenuTree?.Selection.SelectedValue;
            SirenixEditorGUI.BeginHorizontalToolbar(26);
            try
            {
                if (selectedValue is ISetting selectedSetting)
                {
                    var savePath = selectedSetting.PathSetting;
                    if (SirenixEditorGUI.ToolbarButton($"【{Path.GetFileName(savePath)}】"))
                    {
                        Utils.PingObject(savePath);
                    }
                }
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("【SVN提交】"))
                {
                    SVNCommit();
                }
                if (SirenixEditorGUI.ToolbarButton("【保存】"))
                {
                    Save();
                }
                if (SirenixEditorGUI.ToolbarButton("【帮助】"))
                {
                    ShowHelp();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"OnBeginDrawEditors error, exception:{ex}");
            }
            finally
            {
                SirenixEditorGUI.EndHorizontalToolbar();
            }
            base.OnBeginDrawEditors();
        }

        public void ChangeToTab(string name)
        {
            if(menuTree == null)
            {
                BuildMenuTree();
            }
           var item = menuTree.GetMenuItem(name);
            item.Select();
        }

        private void SVNCommit()
        {
            // 按照目录提交，避免文件遗漏
            var paths = new HashSet<string> { Utils.PathFull2Assets(Constants.NodeEditorPath) };
            //foreach (var settingItem in settings)
            //{
            //    foreach (var commitPath in settingItem.PathCommitSetting)
            //    {
            //        var path = Utils.PathFull2Assets(commitPath);
            //        paths.Add(path);
            //    }
            //}
            // 处理svn提交
            DevLocker.VersionControl.WiseSVN.ContextMenus.SVNContextMenusManager.Commit(paths, true);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            DoOnEnable();
        }

        protected override void OnGUI()
        {
            // 避免无效的报错提示，catch
            try { base.OnGUI(); } 
            catch(ExitGUIException) { GUIUtility.ExitGUI(); }
            catch (Exception ex) { Log.Error($"OnGUI error ex:{ex}"); }

            var e = UnityEngine.Event.current;
            if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.S)
            {
                // 保存数据
                Save();
            }
        }
        private void DoOnEnable()
        {
            try
            {
                onOpenWindow?.Invoke(this);

                EditorUtility.DisplayProgressBar(windowName, "加载...TableAnnotation.json", 0.1f);
                _ = TableAnnotation.Inst;

                EditorUtility.DisplayProgressBar(windowName, "同步...TableAnnotation.json", 0.7f);
                TableAnnotation.Inst.SyncParams();

                titleContent = new GUIContent(windowName);
                //ShowNotification(new GUIContent($"{windowName} 已打开"));
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
        public void Save()
        {
            if (!EditorUtility.DisplayDialog(name, "是否保存", "保存", "不保存"))
            {
                ShowNotification(new GUIContent("取消保存"));
                return;
            }
            sbSaveInfo.Length = 0;
            var result = false;
            foreach (var setting in settings)
            {
                result = result || setting.SaveSetting(sbSaveInfo);
            }
            if (sbSaveInfo.Length == 0)
            {
                ShowNotification(new GUIContent("保存完成, 无数据变化"));
            }
            else
            {
                if (EditorUtility.DisplayDialog(name, sbSaveInfo.ToString(), "SVN提交", "不提交"))
                {
                    SVNCommit();
                    ShowNotification(new GUIContent("保存成功"));
                }
                else
                {
                    ShowNotification(new GUIContent("保存成功，不提交SVN"));
                }
            }
            Log.Info($"NodeEditorWindow.Save()\n{sbSaveInfo.ToString().ToSafe()}");
        }

        public static void ShowHelp()
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(NodeEditor.Constants.AnnotationHelpPath);
            AssetDatabase.OpenAsset(asset);
        }
        protected override void OnDestroy()
        {
            Save();
            onCloseWindow?.Invoke(this);
            base.OnDestroy();
        }

        //[OnOpenAsset(0)]
        [EditorMethodRegister(typeof(OnOpenAssetAttribute), "NodeGraph-技能编辑器相关")]
        private static bool OpenAsset(int instanceId, int line)
        {
            var path = AssetDatabase.GetAssetPath(instanceId);
            path = Utils.PathFormat(path);
            if (path == Constants.SkillEditor.PathTableAnnotation)
            {
                OpenWindow()?.TrySelectMenuItemWithObject(TableAnnotation.Inst);
                return true;
            }
            else if (path == Constants.SkillEditor.PathConfigID)
            {
                OpenWindow()?.TrySelectMenuItemWithObject(ConfigIDManager.Inst);
                return true;
            }
            else
            {
                if (path.StartsWith(Constants.PathPrefix_NodeEditor))
                {
                    foreach (var manager in GraphHelper.GetEditorManagers())
                    {
                        if (path == manager.PathSetting)
                        {
                            OpenWindow()?.TrySelectMenuItemWithObject(manager);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        [MenuItem("Tools/节点编辑器配置", priority = 20)]
        public static NodeEditorWindow OpenWindow()
        {
            var win = GetWindow<NodeEditorWindow>();
            win.Show();
            return win;
        }
    }
}
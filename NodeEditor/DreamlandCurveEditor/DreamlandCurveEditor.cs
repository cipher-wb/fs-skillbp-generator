using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using System.IO;
using TableDR;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    /// <summary>
    /// 战斗曲线编辑器
    /// </summary>
    public class DreamlandCurveEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Battle/战斗曲线编辑器", priority = 13)]
        public static DreamlandCurveEditor OpenWindow()
        {
            var win = GetWindow<DreamlandCurveEditor>();
            win.titleContent = new GUIContent(windowName);
            win.Show();
            return win;
        }
        /// <summary>
        /// 只读变量
        /// </summary>
        private const string windowName = "战斗曲线编辑器";
        private readonly string excelName = "../../../../Design/Excel/excel/BattleCameraShake.xlsx";
        private readonly string configName = "BattleCameraShakeConfig";

        /// <summary>
        /// 内置变量
        /// </summary>
        private List<DreamlandCurveEditorPanel> arrAllData = new List<DreamlandCurveEditorPanel>();
        private int iMaxID = 1;
        private OdinMenuTree odinTreeMenu = null;
        private bool bDirty = false;

        protected override OdinMenuTree BuildMenuTree()
        {
            odinTreeMenu = new OdinMenuTree(supportsMultiSelect: false);
            odinTreeMenu.Config.DrawSearchToolbar = true;
            odinTreeMenu.Selection.SupportsMultiSelect = false;

            UpdatePanel();

            return odinTreeMenu;
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                if (SirenixEditorGUI.ToolbarButton("添加"))
                {
                    OnClickAddNew();
                }

                if (SirenixEditorGUI.ToolbarButton("删除当前"))
                {
                    OnClickDelete();
                }

                if (SirenixEditorGUI.ToolbarButton("保存"))
                {
                    OnClickSave();
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            // 这里是解析数据表
            ReloadFromExcel();
        }

        public void Open()
        {
        }

        private void OnClickAddNew()
        {
            iMaxID++;
            DreamlandCurveEditorPanel inst = new DreamlandCurveEditorPanel();
            inst.id = iMaxID;
            inst.strName = $"震动曲线_{inst.id}";
            arrAllData.Add(inst);
            UpdatePanel();
        }

        private void OnClickDelete()
        {
            if (odinTreeMenu.Selection == null)
            {
                return;
            }
            DreamlandCurveEditorPanel currentSelect = odinTreeMenu.Selection.SelectedValue as DreamlandCurveEditorPanel;
            if (currentSelect == null)
            {
                return;
            }

            arrAllData.Remove(currentSelect);
            UpdatePanel();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void OnClickSave()
        {
            // 这里保存
            string path = Path.GetFullPath(Application.dataPath + excelName);
            var excelData = ExcelHelper.ReadExcelXml(path, configName, 0);
            List<object> configs = new List<object>();
            for (int i = 0; i < arrAllData.Count; ++i)
            {
                var data = arrAllData[i];
                BattleCameraShakeConfig inst = data.ConvertToConfig();
                configs.Add(inst);
            }
            ExcelManager.Inst.CleanSheets(path, (sheetName) =>
            {
                return sheetName == nameof(BattleCameraShakeConfig);
            });
            ExcelManager.Inst.WriteExcel(path, configs);

            // 结束后需要重新加载一次
            UpdatePanel();
        }

        /// <summary>
        /// 从excel中重新加载
        /// </summary>
        private void ReloadFromExcel()
        {
            arrAllData.Clear();
            iMaxID = 0;

            // 这里读取excel
            string path = Path.GetFullPath(Application.dataPath + excelName);
            var excelData = ExcelHelper.ReadExcelXml(path, configName, 0);

            var length0 = excelData.GetLength(0);
            var length1 = excelData.GetLength(1);

            for (int i = 2 + 1; i <= length0; i++)
            {
                var ID = excelData[i, 1]?.ToString().Trim() ?? null;
                if (!string.IsNullOrEmpty(ID))
                {
                    DreamlandCurveEditorPanel inst = new DreamlandCurveEditorPanel();
                    inst.ReloadFromExcelData(excelData, i);
                    arrAllData.Add(inst);

                    if (iMaxID < inst.id) iMaxID = inst.id;
                }
            }

            UpdatePanel();
        }

        private void UpdatePanel()
        {
            if (odinTreeMenu == null || odinTreeMenu.MenuItems == null) return;
            odinTreeMenu.MenuItems.Clear();

            for (int i = 0; i < arrAllData.Count; ++i)
            {
                odinTreeMenu.Add(arrAllData[i].strName, arrAllData[i]);
            }
            odinTreeMenu.UpdateMenuTree();
        }
    }
}
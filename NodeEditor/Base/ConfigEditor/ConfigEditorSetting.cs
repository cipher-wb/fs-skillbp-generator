using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;

namespace NodeEditor
{
    [Serializable]
    public class ConfigEditorSetting : ISetting
    {
        public const string Name = "EditorSetting.json";

        [LabelText("配置文件路径"), ReadOnly]
        public string Path;

        [LabelText("导出Excel名"), ValueDropdown("GetExcelPaths")]
        public string ExcelName;

        [JsonIgnore]
        public string PathExportExcel { get { return $"{Constants.ExcelPathPrefix}/{ExcelName}"; } }

        [LabelText("模块配置")]
        public List<string> ModuleAnnos = new List<string>();

        public bool IsDirty()
        {
            ModuleAnnos.Sort();
            return Utils.IsDirtyJson(this, Path);
        }
        public static TS Load<TS>(string path) where TS : ConfigEditorSetting, new()
        {
            var setting = Utils.ReadFromJson<TS>(path);
            if (setting == null)
            {
                setting = new TS();
                setting.Path = path;
            }
            return setting;
        }
        public bool SaveSetting(StringBuilder saveInfo)
        {
            if (IsDirty())
            {
                Utils.WriteToJson(this, Path);
                saveInfo.AppendLine($"保存编辑器配置：{Path}");
                return true;
            }
            return false;
        }

        public string PathSetting => Path;
        public List<string> PathCommitSetting => new List<string> { PathSetting };

        private IEnumerable<ValueDropdownItem> GetExcelPaths()
        {
            var excelFiles = Directory.GetFiles(Constants.ExcelPathPrefix, $"*.xlsx", SearchOption.TopDirectoryOnly);
            foreach (var excelFile in excelFiles)
            {
                var excelName = System.IO.Path.GetFileName(excelFile);
                yield return new ValueDropdownItem(excelName, excelName);
            }
        }

        // TODO 文件变动监听
    }
}

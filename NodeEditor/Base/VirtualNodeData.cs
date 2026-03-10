using GraphProcessor;
using Newtonsoft.Json;
using NodeEditor.AIEditor;
using NodeEditor.GamePlayEditor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NodeEditor
{
    [Serializable]
    public sealed class VirtualNodeData
    {
        public const string DefaultGraphJsonPath = "选择模板文件";

        public const string JsonAssetPath = "Assets/Thirds/NodeEditor/NpcEventEditor/Saves/Jsons/节点模板/";

        [LabelText("模板文件"), ValueDropdown("GetGraphJsonPath")]
        public string GraphJsonPath = DefaultGraphJsonPath;

        [LabelText("模板显示名"), ValidateInput("CheckShowName", "必须为中文", InfoMessageType.Error)]
        public string ShowName;

        [LabelText("模板代码名"), ValidateInput("CheckCodeName", "必须为英文+数字", InfoMessageType.Error)]
        public string CodeName;

        /// <summary>
        /// 有效性
        /// </summary>
        public bool IsValid => CheckCodeName(CodeName) && CheckShowName(ShowName) && GraphJsonPath.Contains(".json");

        /// <summary>
        /// 检测VirturalNodeShowName输入
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckShowName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            name = name.Trim().Replace(" ", "").Replace("   ", "");

            for (int i = 0; i < name.Length; i++)
            {
                Regex reg = new Regex(@"[\u4e00-\u9fa5]");
                if (!reg.IsMatch(name[i].ToString()))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检测VirturalNodeCodeName输入
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckCodeName(string name)
        {
            if (string.IsNullOrEmpty(name)) { return false; }

            name = name.Trim().Replace(" ", "").Replace("   ", "");

            string pattern = @"^[A-Za-z0-9_]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(name);
        }

        /// <summary>
        /// 获取需要拷贝的graph列表
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValueDropdownItem> GetGraphJsonPath()
        {
            yield return new ValueDropdownItem("空", string.Empty);

            var graghFiles = Directory.GetFiles(JsonAssetPath, "*.json", SearchOption.AllDirectories);
            for (int i = 0; i < graghFiles.Length; i++)
            {
                var graghFile = graghFiles[i];
                Utils.PathFormat(ref graghFile);
                var fileName = Utils.PathFull2SeparationFolder(graghFile, "Jsons/");
                yield return new ValueDropdownItem(fileName, graghFile);
            }
        }
    }
}

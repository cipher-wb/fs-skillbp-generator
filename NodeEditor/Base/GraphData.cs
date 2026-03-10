using GraphProcessor;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace NodeEditor
{
    [Serializable]
    public sealed class GraphData
    {
        public const string DefaultCompatibleGraphName = "未配置-所属编辑器";
        public const string DefaultModuleName = "未配置-模块";

        [LabelText("所属编辑器"), ValueDropdown("GetCompatibleGraphs")]
        public string CompatibleGraphName = DefaultCompatibleGraphName;

        [LabelText("所属模块名"), ValueDropdown("GetModuleNames", DoubleClickToConfirm = true)]
        public string ModuleName = DefaultModuleName;

        [HideIf("@true"), NonSerialized]
        [LabelText("所属模块名-前缀"), ValueDropdown("GetModuleNames", DoubleClickToConfirm = true)]
        public string ModuleNamePerfix = string.Empty;

        public Type GetGraphType()
        {
            if (string.IsNullOrEmpty(CompatibleGraphName) || CompatibleGraphName == DefaultCompatibleGraphName)
            {
                return null;
            }
            var types = TypeCache.GetTypesDerivedFrom<BaseGraph>();
            foreach (var type in types)
            {
                if (type.Name == CompatibleGraphName)
                {
                    return type;
                }
            }
            return null;
        }
        public string GetModuleName(string defalutName = DefaultModuleName)
        {
            if (string.IsNullOrEmpty(ModuleName) || ModuleName == DefaultModuleName)
            {
                return defalutName;
            }
            return ModuleName;
        }
        public string GetModuleNamePerfix()
        {
            if (string.IsNullOrEmpty(ModuleNamePerfix))
            {
                return string.Empty;
            }
            return ModuleNamePerfix + "/";
        }
        public string GetEditorName()
        {
            foreach (var manager in GraphHelper.GetEditorManagers())
            {
                if (CompatibleGraphName == manager.GraphType.Name)
                {
                    return manager.Name;
                }
            }
            return CompatibleGraphName;
        }
        private IEnumerable<ValueDropdownItem> GetCompatibleGraphs()
        {
            yield return new ValueDropdownItem(DefaultCompatibleGraphName, DefaultCompatibleGraphName);
            foreach (var manager in GraphHelper.GetEditorManagers())
            {
                yield return new ValueDropdownItem(manager.Name, manager.GraphType.Name);
            }
        }
        private IEnumerable<ValueDropdownItem> GetModuleNames()
        {
            var manager = GraphHelper.FindEditorManager((manager) =>
            {
                return CompatibleGraphName == manager.GraphType.Name;
            });
            if (manager == null)
            {
                yield return new ValueDropdownItem("请先选择所属编辑器", DefaultModuleName);
                yield break;
            }
            yield return new ValueDropdownItem(DefaultModuleName, DefaultModuleName);
            foreach (var moduleAnno in manager.Setting.ModuleAnnos)
            {
                yield return new ValueDropdownItem(moduleAnno, moduleAnno);
            }
        }
    }
}

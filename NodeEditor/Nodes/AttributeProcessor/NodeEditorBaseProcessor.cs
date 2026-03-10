using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace NodeEditor
{
    internal class NodeEditorBaseProcessor<T> : OdinAttributeProcessor<T> where T : class, new()
    {
        protected static class DefaultAttributes
        {
            public static HideReferenceObjectPickerAttribute HideReferenceObjectPickerAttribute = new HideReferenceObjectPickerAttribute();
            public static EnableIfAttribute EnableIfAttribute_False = new EnableIfAttribute("@false");
            public static HideIfAttribute HideIfAttribute_True = new HideIfAttribute("@true");
            public static TextAreaAttribute TextAreaAttribute = new TextAreaAttribute(4, 4);
            public static IndentAttribute IndentAttribute = new IndentAttribute();
            public static ListDrawerSettingsAttribute ListDrawerSettingsAttribute_Hide = new ListDrawerSettingsAttribute
            {
                HideAddButton = true,
                HideRemoveButton = true,
                ShowFoldout = true,
                DraggableItems = false,
                NumberOfItemsPerPage = 50,
                IsReadOnly = false,
            };
        }
        #region 自定义显示
        private static T tConfig;
        public static T TConfig {  get { if (tConfig == null) tConfig = new(); return tConfig; } }
        protected virtual bool IsAddColorIf => true;
        public NodeEditorBaseProcessor() { }

        protected HashSet<string> hideMembers;

        // TODO 抽出统一设置
        protected Dictionary<(string Title, int order), HashSet<string>> groupInfo;

        protected virtual void ProcessHideIf(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (hideMembers?.Count > 0)
            {
                if (hideMembers.Contains(member.Name))
                {
                    foreach (var attribute in attributes)
                    {
                        if (attribute is Sirenix.OdinInspector.ShowInInspectorAttribute)
                        {
                            attributes.Remove(attribute);
                            break;
                        }
                    }
                }
            }
        }
        #endregion
        public override bool CanProcessSelfAttributes(InspectorProperty property)
        {
            if (!NodeEditorManager.IsInEditor)
            {
                return false;
            }
            return base.CanProcessSelfAttributes(property);
        }
        public override bool CanProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member)
        {
            if (!NodeEditorManager.IsInEditor || member.MemberType != MemberTypes.Property)
            {
                return false;
            }

            return base.CanProcessChildMemberAttributes(parentProperty, member);
        }

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            base.ProcessChildMemberAttributes(parentProperty, member, attributes);

            if (parentProperty.ParentValues[0] is IConfigBaseNode configBaseNode)
            {
                if (IsAddColorIf)
                {
                    attributes.Add(new ColorIfAttribute(ColorIfConditionAction, true));
                }
                var configName = configBaseNode.GetConfigName();
                var configAnno = TableAnnotation.Inst.BaseConfigAnnos.Find((n) => { return n.Name == configName; });
                if (configAnno != null)
                {
                    var desc = configAnno.configMemberDescs.Find((d) => { return d.Name == member.Name; });
                    if (configAnno.MemberName2Tips.TryGetValue(member.Name, out var tipsAttr))
                    {
                        attributes.Add(tipsAttr);
                    }
                }
            }

            ProcessHideIf(parentProperty, member, attributes);
        }

        /// <summary>
        /// 错误提示回调
        /// </summary>
        /// <param name="obj">提示对象</param>
        protected virtual bool ColorIfConditionAction(object obj, string propertyName) => false;

        protected T GetConfig(InspectorProperty parentProperty)
        {
            var configNode = parentProperty.ParentValues[0] as IConfigBaseNode;
            var config = configNode?.GetConfig();
            if (config != null)
            {
                return config as T;
            }
            return default(T);
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="attributes"></param>
        protected void ProcessGroupInfo(string memberName, List<Attribute> attributes, Dictionary<(string Title, int order), HashSet<string>> groupInfo)
        {
            foreach (var kvp in groupInfo)
            {
                if (kvp.Value.Contains(memberName))
                {
                    attributes.Add(new FoldoutGroupAttribute(kvp.Key.Title, order: kvp.Key.order));

                    //外部引用，不可编辑
                    if (kvp.Key.order == 99)
                    {
                        attributes.Add(DefaultAttributes.EnableIfAttribute_False);
                    }
                }
            }
        }
    }
}

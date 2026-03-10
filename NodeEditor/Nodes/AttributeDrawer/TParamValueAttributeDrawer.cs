using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Serialization;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TableDR;
using UnityEditor;
using UnityEngine;
using SerializationUtility = Sirenix.Serialization.SerializationUtility;

namespace NodeEditor
{
    public class TParamIValueDropdownEqualityComparer : IEqualityComparer<object>
    {
        private bool isTypeLookup;

        public TParamIValueDropdownEqualityComparer(bool isTypeLookup)
        {
            this.isTypeLookup = isTypeLookup;
        }

        public new bool Equals(object x, object y)
        {
            if (x is ValueDropdownItem)
            {
                x = ((ValueDropdownItem)x).Value;
            }

            if (y is ValueDropdownItem)
            {
                y = ((ValueDropdownItem)y).Value;
            }

            if (EqualityComparer<object>.Default.Equals(x, y))
            {
                return true;
            }

            if (x == null != (y == null))
            {
                return false;
            }

            if (isTypeLookup)
            {
                Type type = (x as Type) ?? x.GetType();
                Type type2 = (y as Type) ?? y.GetType();
                if ((object)type == type2)
                {
                    return true;
                }
            }

            return false;
        }

        public int GetHashCode(object obj)
        {
            if (obj == null)
            {
                return -1;
            }

            if (obj is ValueDropdownItem)
            {
                obj = ((ValueDropdownItem)obj).Value;
            }

            if (obj == null)
            {
                return -1;
            }

            if (isTypeLookup)
            {
                Type type = (obj as Type) ?? obj.GetType();
                return type.GetHashCode();
            }

            return obj.GetHashCode();
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("UNITY_EDITOR")]
    public class TParamValueAttribute : Attribute
    {
        public string MemberName;

        public int NumberOfItemsBeforeEnablingSearch;

        public bool IsUniqueList;

        public bool DrawDropdownForListElements;

        public bool DisableListAddButtonBehaviour;

        public bool ExcludeExistingValuesInList;

        public bool ExpandAllMenuItems;

        public bool AppendNextDrawer;

        public bool DisableGUIInAppendedDrawer;

        public bool DoubleClickToConfirm;

        public bool FlattenTreeView;

        public int DropdownWidth;

        public int DropdownHeight;

        public string DropdownTitle;

        public bool SortDropdownItems;

        public bool HideChildProperties;

        public TParam Param;

        public object ParentValue;

        public TParamAnnotation ParamAnnotation;

        public TParamValueAttribute(TParam Param,TParamAnnotation ParamAnnotation,object ParentValue)
        {
            NumberOfItemsBeforeEnablingSearch = 10;
            DrawDropdownForListElements = true;
            this.Param = Param;
            this.ParamAnnotation = ParamAnnotation;
            this.ParentValue = ParentValue;
        }
    }

    [DrawerPriority(0.0, 0.0, 2002.0)]
    public class TParamValueAttributeDrawer : OdinAttributeDrawer<TParamValueAttribute>
    {
        public static Func<TParamValueAttributeDrawer,bool> OnReloadDropdownCollections;
        public static Action<TParamValueAttributeDrawer> OnInit;

        private string error;

        private GUIContent label;

        private bool isList;

        private bool isListElement;

        private Func<IEnumerable<ValueDropdownItem>> getValues;

        private Func<IEnumerable<object>> getSelection;

        private IEnumerable<object> result;

        private bool enableMultiSelect;

        private Dictionary<object, string> nameLookup;

        private InspectorPropertyValueGetter<object> rawGetter;

        private LocalPersistentContext<bool> isToggled;

        private GenericSelector<object> inlineSelector;

        private IEnumerable<object> nextResult;

        public bool ForceDrawDropdown;

        //
        // 摘要:
        //     Initializes this instance.
        protected override void Initialize()
        {
            if(TParamValueAttributeDrawer.OnInit != null)
            {
                TParamValueAttributeDrawer.OnInit.Invoke(this);
            }
            ReloadDropdownCollections();
        }

        public void ReloadDropdownCollections()
        {
            bool setAttrMember = true;
            if (TParamValueAttributeDrawer.OnReloadDropdownCollections != null)
            {
                if(TParamValueAttributeDrawer.OnReloadDropdownCollections.Invoke(this))
                {
                    setAttrMember = false;
                }
            }

            if (setAttrMember)
            {
                TParamAnnotation paramAnn = base.Attribute.ParamAnnotation;
                TParam param = base.Attribute.Param;
                object parentValue = base.Attribute.ParentValue;
                if (paramAnn != null && paramAnn.isEnum && param.ParamType == TParamType.TPT_NULL)
                {
                    switch (paramAnn.RefTypeName)
                    {
                        // 属性下拉接管
                        case nameof(TBattleNatureEnum):
                            {
                                // 修改属性，下拉为可写列表
                                if (parentValue is TSET_MODIFY_ENTITY_ATTR_VALUE
                                    || parentValue is TSET_ADD_ENTITY_ATTR_VALUE)
                                {
                                    base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Write";
                                }
                                // 获取属性，下拉为可读列表
                                else
                                {
                                    base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Read";
                                }
                                break;
                            }
                        // 状态下拉接管
                        case nameof(TEntityState):
                            {
                                // 修改状态，下拉为可写列表
                                if (parentValue is TSET_MODIFY_ENTITY_STATE
                                    || parentValue is TSET_ADD_ENTITY_STATE)
                                {
                                    base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TEntityState_Write";
                                }
                                // 获取状态，下拉为可读列表
                                else
                                {
                                    base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TEntityState_Read";
                                }
                                break;
                            }
                        // 动作根据要求按照字母顺序从大到小排序
                        case nameof(TRoleAnimType):
                            {
                                base.Attribute.MemberName = $"{Constants.EnumVDPefix}{paramAnn.RefTypeName}";
                                break;
                            }
                        default:
                            {
                                base.Attribute.MemberName = $"{Constants.EnumVDPefix}{paramAnn.RefTypeName}";
                                break;
                            }
                    }
                }

                var desc = param.ParamType.GetDescription(false);
                // 如果是属性类型，那么显示成下拉框
                switch (param.ParamType)
                {
                    case TParamType.TPT_ATTR:
                        base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TBattleNatureEnum_Read";
                        base.Attribute.DropdownTitle = $"请选择 {desc}...";
                        break;
                    case TParamType.TPT_COMMON_PARAM:
                        base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TCommonParamEnum_Read";
                        //base.Attribute.MemberName = $"{Constants.EnumVDPefix}{nameof(TCommonParamType)}";
                        base.Attribute.DropdownTitle = $"请选择 {desc}...";
                        break;
                    case TParamType.TPT_COMMON_SKILL_PARAM:
                        base.Attribute.MemberName = $"@TableDR.CustomEnumUtility.VD_TCommonSkillParamEnum_Read";
                        //base.Attribute.MemberName = $"{Constants.EnumVDPefix}{nameof(TCommonParamType)}";
                        base.Attribute.DropdownTitle = $"请选择 {desc}...";
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrEmpty(base.Attribute.MemberName))
            {
                return;
            }

            rawGetter = new InspectorPropertyValueGetter<object>(base.Property, base.Attribute.MemberName);
            isToggled = this.GetPersistentValue("Toggled", SirenixEditorGUI.ExpandFoldoutByDefault);
            error = rawGetter.ErrorMessage;
            isList = base.Property.ChildResolver is IOrderedCollectionResolver;
            isListElement = (object)base.Property.Info.GetMemberInfo() == null;
            getSelection = () => base.Property.ValueEntry.WeakValues.Cast<object>();
            getValues = delegate
            {
                object value = rawGetter.GetValue();
                return (value != null) ? (from object x in value as IEnumerable
                                          where x != null
                                          select x).Select(delegate (object x)
                                          {
                                              if (x is ValueDropdownItem)
                                              {
                                                  return (ValueDropdownItem)x;
                                              }

                                              if (x is IValueDropdownItem)
                                              {
                                                  IValueDropdownItem valueDropdownItem = x as IValueDropdownItem;
                                                  return new ValueDropdownItem(valueDropdownItem.GetText(), valueDropdownItem.GetValue());
                                              }

                                              return new ValueDropdownItem(null, x);
                                          }) : null;
            };

            if (error != null)
            {
                return;
            }

            object obj = null;
            object value = rawGetter.GetValue();
            if (value != null)
            {
                obj = (value as IEnumerable).Cast<object>().FirstOrDefault();
            }

            if (obj is IValueDropdownItem)
            {
                IEnumerable<ValueDropdownItem> enumerable = getValues();
                nameLookup = new Dictionary<object, string>(new TParamIValueDropdownEqualityComparer(isTypeLookup: false));
                foreach (ValueDropdownItem item in enumerable)
                {
                    nameLookup[item] = item.Text;
                }
            }
            else
            {
                nameLookup = null;
            }
        }

        private static IEnumerable<ValueDropdownItem> ToValueDropdowns(IEnumerable<object> query)
        {
            return query.Select(delegate (object x)
            {
                if (x is ValueDropdownItem)
                {
                    return (ValueDropdownItem)x;
                }

                if (x is IValueDropdownItem)
                {
                    IValueDropdownItem valueDropdownItem = x as IValueDropdownItem;
                    return new ValueDropdownItem(valueDropdownItem.GetText(), valueDropdownItem.GetValue());
                }

                return new ValueDropdownItem(null, x);
            });
        }

        private TParamType lastParamType;
        protected override void DrawPropertyLayout(GUIContent label)
        {
            this.label = label;
            if (base.Property.ValueEntry == null)
            {
                CallNextDrawer(label);
            }
            else if (error != null)
            {
                SirenixEditorGUI.ErrorMessageBox(error);
                CallNextDrawer(label);
            }
            else if (isList)
            {
                if (base.Attribute.DisableListAddButtonBehaviour)
                {
                    CallNextDrawer(label);
                    return;
                }

                CollectionDrawerStaticInfo.NextCustomAddFunction = OpenSelector;
                CallNextDrawer(label);
                if (result != null)
                {
                    AddResult(result);
                    result = null;
                }
            }
            else if (base.Attribute.DrawDropdownForListElements || !isListElement)
            {
                if (lastParamType != base.Attribute.Param.ParamType)
                {
                    ReloadDropdownCollections();
                    lastParamType = base.Attribute.Param.ParamType;
                }

                var isDrawAsDropDown = base.Attribute.Param.ParamType == TableDR.TParamType.TPT_ATTR
                    || base.Attribute.Param.ParamType == TableDR.TParamType.TPT_COMMON_PARAM
                    || base.Attribute.Param.ParamType == TableDR.TParamType.TPT_COMMON_SKILL_PARAM
                    || (base.Attribute.ParamAnnotation?.isEnum == true && base.Attribute.Param.ParamType == TParamType.TPT_NULL)
                    || ForceDrawDropdown;
                if (isDrawAsDropDown)
                {
                    DrawDropdown(label);
                }
                else
                {
                    if (base.Attribute.Param.ParamType == TParamType.TPT_FUNCTION_RETURN)
                    {
                        // 函数返回值类型，添加按钮
                        DrawPropertyLayoutWithButton(label);
                    }
                    else
                    {
                        CallNextDrawer(label);
                    }
                }
            }
            else
            {
                CallNextDrawer(label);
            }
        }

        private void AddResult(IEnumerable<object> query)
        {
            if (isList)
            {
                IOrderedCollectionResolver orderedCollectionResolver = base.Property.ChildResolver as IOrderedCollectionResolver;
                if (enableMultiSelect)
                {
                    orderedCollectionResolver.QueueClear();
                }

                foreach (object item in query)
                {
                    object[] array = new object[base.Property.ParentValues.Count];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = SerializationUtility.CreateCopy(item);
                    }

                    orderedCollectionResolver.QueueAdd(array);
                }
            }
            else
            {
                object obj = query.FirstOrDefault();
                for (int j = 0; j < base.Property.ValueEntry.WeakValues.Count; j++)
                {
                    base.Property.ValueEntry.WeakValues[j] = SerializationUtility.CreateCopy(obj);
                }
            }
        }
        private bool DrawEnumFlagsPropertyLayout(GUIContent label)
        {
            if (!(base.Attribute.ParamAnnotation?.isEnum == true && base.Attribute.Param?.ParamType == TParamType.TPT_NULL))
            {
                return false;
            }

            var enumType = base.Attribute.ParamAnnotation.GetRefType(null);
            if (enumType == null || !enumType.IsEnum /*|| !enumType.IsDefined(typeof(FlagsAttribute))*/)
            {
                return false;
            }

            var property = base.Property;
            // 获取当前值
            var entry = property.ValueEntry;
            var currentValue = (int)entry.WeakSmartValue;
            // 绘制Enum
            EditorGUI.BeginChangeCheck();
            var currentEnumValue = (Enum)Enum.ToObject(enumType, entry.WeakSmartValue);
            var enumText = Utils.GetEnumDescription(enumType, currentValue);
            var newEnumValue = DraweHelper.DrawEnumField(label, new GUIContent(enumText), currentEnumValue, enumType);
            // 处理值变更
            if (EditorGUI.EndChangeCheck())
            {
                entry.WeakSmartValue = Convert.ToInt32(newEnumValue);
            }
            return true;
        }

        private void DrawDropdown(GUIContent label)
        {
            if (DrawEnumFlagsPropertyLayout(label))
            {
                return;
            }
            IEnumerable<object> enumerable = null;
            if (base.Attribute.AppendNextDrawer && !isList)
            {
                GUILayout.BeginHorizontal();
                float num = 15f;
                if (label != null)
                {
                    num += GUIHelper.BetterLabelWidth;
                }

                enumerable = OdinSelector<object>.DrawSelectorDropdown(label, GUIContent.none, ShowSelector, GUIStyle.none, GUILayoutOptions.Width(num));
                if (UnityEngine.Event.current.type == EventType.Repaint)
                {
                    Rect position = GUILayoutUtility.GetLastRect().AlignRight(15f);
                    position.y += 4f;
                    SirenixGUIStyles.PaneOptions.Draw(position, GUIContent.none, 0);
                }

                GUILayout.BeginVertical();
                bool disableGUIInAppendedDrawer = base.Attribute.DisableGUIInAppendedDrawer;
                if (disableGUIInAppendedDrawer)
                {
                    GUIHelper.PushGUIEnabled(enabled: false);
                }

                CallNextDrawer(null);
                if (disableGUIInAppendedDrawer)
                {
                    GUIHelper.PopGUIEnabled();
                }

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
            else
            {
                string currentValueName = GetCurrentValueName();
                if (!base.Attribute.HideChildProperties && base.Property.Children.Count > 0)
                {
                    isToggled.Value = SirenixEditorGUI.Foldout(isToggled.Value, label, out var valueRect);
                    enumerable = OdinSelector<object>.DrawSelectorDropdown(valueRect, currentValueName, ShowSelector);
                    if (SirenixEditorGUI.BeginFadeGroup(this, isToggled.Value))
                    {
                        EditorGUI.indentLevel++;
                        for (int i = 0; i < base.Property.Children.Count; i++)
                        {
                            InspectorProperty inspectorProperty = base.Property.Children[i];
                            inspectorProperty.Draw(inspectorProperty.Label);
                        }

                        EditorGUI.indentLevel--;
                    }

                    SirenixEditorGUI.EndFadeGroup();
                }
                else
                {
                    enumerable = OdinSelector<object>.DrawSelectorDropdown(label, currentValueName, ShowSelector, null);
                }
            }

            if (enumerable != null && enumerable.Any())
            {
                AddResult(enumerable);
            }
        }

        private void DrawPropertyLayoutWithButton(GUIContent label)
        {
            // 绘制Label
            EditorGUILayout.BeginHorizontal();
            CallNextDrawer(label);

            // 添加按钮
            var paramValue = base.Attribute.Param?.Value ?? 0;
            if (paramValue > 0 && GUILayout.Button("定位"))
            {
                // 查找节点并定位
                JsonGraphManager.Inst.TryOpenGraphWithProgressBar(nameof(SkillEffectConfig), paramValue);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OpenSelector()
        {
            ReloadDropdownCollections();
            Rect rect = new Rect(UnityEngine.Event.current.mousePosition, Vector2.zero);
            OdinSelector<object> odinSelector = ShowSelector(rect);
            odinSelector.SelectionConfirmed += delegate (IEnumerable<object> x)
            {
                result = x;
            };
        }

        private OdinSelector<object> ShowSelector(Rect rect)
        {
            GenericSelector<object> genericSelector = CreateSelector();
            rect.x = (int)rect.x;
            rect.y = (int)rect.y;
            rect.width = (int)rect.width;
            rect.height = (int)rect.height;
            if (base.Attribute.AppendNextDrawer && !isList)
            {
                rect.xMax = GUIHelper.GetCurrentLayoutRect().xMax;
            }

            genericSelector.ShowInPopup(rect, new Vector2(base.Attribute.DropdownWidth, base.Attribute.DropdownHeight));
            return genericSelector;
        }

        private GenericSelector<object> CreateSelector()
        {
            if (getValues == null)
            {
                ReloadDropdownCollections();
            }
            base.Attribute.IsUniqueList = base.Attribute.IsUniqueList || base.Attribute.ExcludeExistingValuesInList;
            IEnumerable<ValueDropdownItem> enumerable = getValues?.Invoke() ?? Enumerable.Empty<ValueDropdownItem>();
            if (enumerable.Any())
            {
                if ((isList && base.Attribute.ExcludeExistingValuesInList) || (isListElement && base.Attribute.IsUniqueList))
                {
                    List<ValueDropdownItem> list = enumerable.ToList();
                    InspectorProperty inspectorProperty = base.Property.FindParent((InspectorProperty x) => x.ChildResolver is IOrderedCollectionResolver, includeSelf: true);
                    TParamIValueDropdownEqualityComparer comparer = new TParamIValueDropdownEqualityComparer(isTypeLookup: false);
                    inspectorProperty.ValueEntry.WeakValues.Cast<IEnumerable>().SelectMany((IEnumerable x) => x.Cast<object>()).ForEach(delegate (object x)
                    {
                        list.RemoveAll((ValueDropdownItem c) => comparer.Equals(c, x));
                    });
                    enumerable = list;
                }

                if (nameLookup != null)
                {
                    foreach (ValueDropdownItem item in enumerable)
                    {
                        if (item.Value != null)
                        {
                            nameLookup[item.Value] = item.Text;
                        }
                    }
                }
            }

            bool drawSearchToolbar = base.Attribute.NumberOfItemsBeforeEnablingSearch == 0 || (enumerable != null && enumerable.Take(base.Attribute.NumberOfItemsBeforeEnablingSearch).Count() == base.Attribute.NumberOfItemsBeforeEnablingSearch);
            GenericSelector<object> genericSelector = new GenericSelector<object>(base.Attribute.DropdownTitle, supportsMultiSelect: false, enumerable.Select((ValueDropdownItem x) => new GenericSelectorItem<object>(x.Text, x.Value)));
            enableMultiSelect = isList && base.Attribute.IsUniqueList && !base.Attribute.ExcludeExistingValuesInList;
            if (base.Attribute.FlattenTreeView)
            {
                genericSelector.FlattenedTree = true;
            }

            if (isList && !base.Attribute.ExcludeExistingValuesInList && base.Attribute.IsUniqueList)
            {
                genericSelector.CheckboxToggle = true;
            }
            else if (!base.Attribute.DoubleClickToConfirm && !enableMultiSelect)
            {
                genericSelector.EnableSingleClickToSelect();
            }

            if (isList && enableMultiSelect)
            {
                genericSelector.SelectionTree.Selection.SupportsMultiSelect = true;
                genericSelector.DrawConfirmSelectionButton = true;
            }

            genericSelector.SelectionTree.Config.DrawSearchToolbar = drawSearchToolbar;
            IEnumerable<object> selection = Enumerable.Empty<object>();
            if (!isList)
            {
                selection = getSelection();
            }
            else if (enableMultiSelect)
            {
                selection = getSelection().SelectMany((object x) => (x as IEnumerable).Cast<object>());
            }

            genericSelector.SetSelection(selection);
            genericSelector.SelectionTree.EnumerateTree().AddThumbnailIcons(preferAssetPreviewAsIcon: true);
            if (base.Attribute.ExpandAllMenuItems)
            {
                genericSelector.SelectionTree.EnumerateTree(delegate (OdinMenuItem x)
                {
                    x.Toggled = true;
                });
            }

            if (base.Attribute.SortDropdownItems)
            {
                genericSelector.SelectionTree.SortMenuItemsByName();
            }

            return genericSelector;
        }

        private string GetCurrentValueName()
        {
            if (!EditorGUI.showMixedValue)
            {
                object weakSmartValue = base.Property.ValueEntry.WeakSmartValue;
                string value = null;
                if (nameLookup != null && weakSmartValue != null)
                {
                    nameLookup.TryGetValue(weakSmartValue, out value);
                }

                return new GenericSelectorItem<object>(value, weakSmartValue).GetNiceName();
            }

            return "—";
        }
    }
}

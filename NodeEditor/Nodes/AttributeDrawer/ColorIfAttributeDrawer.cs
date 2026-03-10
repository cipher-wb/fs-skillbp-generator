using GraphProcessor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Reflection;
using UnityEngine;

namespace NodeEditor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ColorIfAttribute : Attribute
    {
        public Color color = Color.red;
        public string colorMember;
        public string conditionMember;
        public bool IgnoreError = false;
        public Func<object, string, bool> conditionFunc;
        public ColorIfAttribute(string colorMember, string conditionMember, bool IgnoreError = false)
        {
            this.colorMember = colorMember;
            this.conditionMember = conditionMember;
            this.IgnoreError = IgnoreError;
        }
        public ColorIfAttribute(Color color, string conditionMember, bool IgnoreError = false)
        {
            this.color = color;
            this.conditionMember = conditionMember;
            this.IgnoreError = IgnoreError;
        }
        public ColorIfAttribute(string conditionMember, bool IgnoreError = false)
        {
            this.conditionMember = conditionMember;
            this.IgnoreError = IgnoreError;
        }
        public ColorIfAttribute(Func<object, string, bool> conditionFunc, bool IgnoreError = false)
        {
            this.IgnoreError = IgnoreError;
            this.conditionFunc = conditionFunc;
        }
    }
    public sealed class ColorIfAttributeDrawer : OdinAttributeDrawer<ColorIfAttribute>
    {
        private MethodInfo conditionMethod;
        private MethodInfo colorMethod;

        protected override bool CanDrawAttributeProperty(InspectorProperty property)
        {
            var node = property.Tree.WeakTargets[0] as NodeInspectorObject;
            return node != null;
        }

        protected override void Initialize()
        {
            colorMethod = null;
            conditionMethod = null;
            var parentValue = Property.ParentValues[0];
            if (parentValue != null && !parentValue.GetType().IsGenericType)
            {
                if (!string.IsNullOrEmpty(Attribute.colorMember))
                {
                    var method = parentValue.GetType().ExGetMethod(Attribute.colorMember);
                    if (method != null)
                    {
                        if (method.ReturnType == typeof(Color))
                        {
                            colorMethod = method;
                        }
                        else if (!Attribute.IgnoreError)
                        {
                            Log.Error($"ColorIfAttributeDrawer failed, colorMember is not return Color : {Attribute.colorMember}");
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Attribute.conditionMember))
                {
                    var method = parentValue.GetType().ExGetMethod(Attribute.conditionMember, typeof(string));
                    if (method != null && method.ReturnType == typeof(bool))
                    {
                        conditionMethod = method;
                    }
                    if (conditionMethod == null && !Attribute.IgnoreError)
                    {
                        Log.Error($"ColorIfAttributeDrawer failed, not find conditionMethod {Attribute.conditionMember}");
                    }
                }
            }
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var condition = false;
            var color = Color.white;
            var parentValue = Property.ParentValues[0];
            if (parentValue != null)
            {
                var result = conditionMethod?.Invoke(parentValue, new object[] { Property.Name }) ?? null;
                condition = result is true;

                condition |= Attribute.conditionFunc?.Invoke(parentValue, Property.Name) ?? false;

                result = colorMethod?.Invoke(parentValue, null) ?? Attribute.color;

                if (result is Color resultColor)
                {
                    color = resultColor;
                }
            }

            if (condition)
            {
                GUIHelper.PushColor(color);
            }

            this.CallNextDrawer(label);

            if (condition)
            {
                GUIHelper.PopColor();
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GraphProcessor
{
    public static class Utils
    {
        public static readonly StyleColor DefaultLabelColor = new StyleColor(new Color(255, 242, 153, 255));
        public static readonly float DefaultLabelFontSize = 15;
        public static Action<string,object[]> LogError { get; set; }

        //根据类型创建对应类型的Elem
        public static VisualElement CreateElemByFieldType(Type fieldType, string label = null, bool isMulti = false)
        {
            if (fieldType == null)
                return default;

            VisualElement element = null;

            //默认样式
            if (fieldType == typeof(string))
            {
                TextField textField = new TextField();
                textField.isReadOnly = true;
                textField.label = string.IsNullOrEmpty(label) ? "" : label;
                var input = textField.Q("unity-text-input");
                if (input != null)
                {
                    input.style.fontSize = DefaultLabelFontSize;
                    //input.style.color= DefaultLabelColor;
                    //input.RegisterCallback<GeometryChangedEvent>(evt =>
                    //{
                    //    input.style.color = DefaultLabelColor;
                    //});
                }
                if (isMulti)
                {
                    textField.multiline = true;
                    textField.style.flexDirection = FlexDirection.Column;
                }
                //textField.MarkDirtyRepaint();

                element = textField;
            }
            else if (typeof(IList).IsAssignableFrom(fieldType))
            {
                Foldout foldout = new Foldout();
                foldout.text = string.IsNullOrEmpty(label) ? "" : label;
                element = foldout;
            } 

            return element;
        }

        public static void Error(string info,params object[] param)
        {
            //所有的移除都会被捕获
            if (LogError != null)
            {
                LogError.Invoke(info.ToString(), null);
            }
            else
            {
                Debug.LogError(info.ToString());
            }
        }
    }
}

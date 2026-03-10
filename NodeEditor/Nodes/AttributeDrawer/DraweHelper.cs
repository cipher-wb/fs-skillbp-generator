using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEngine;

namespace NodeEditor
{
    public static class DraweHelper
    {
        public static object DrawEnumField(GUIContent label, GUIContent contentLabel, object value, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                Debug.LogError("Provided type is not an enum.");
                return default;
            }

            // 使用反射调用泛型方法
            //EnumSelector<T>.DrawEnumField()
            var method = typeof(EnumSelector<>)
                .MakeGenericType(enumType)
                .GetMethod("DrawEnumField", new Type[] { typeof(GUIContent), typeof(GUIContent), enumType, typeof(GUIStyle), typeof(SdfIconType) });

            if (method == null)
            {
                Debug.LogError("Method DrawEnumField not found.");
                return default;
            }

            return method.Invoke(null, new object[] { label, contentLabel, value, null, SdfIconType.None });
        }
    }
}

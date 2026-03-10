
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if !NodeExport
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
#endif

namespace NodeEditor
{
    public static class Extensions
    {
#if !NodeExport
        public static void TryRemove(this VisualElement self, VisualElement element)
        {
            if (self.Contains(element))
            {
                self.Remove(element);
            }
        }

        /// <summary>
        /// 包围盒检测，判断content是否位于self内部
        /// </summary>
        /// <param name="self"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool BoundingBoxTest(this VisualElement self, VisualElement content)
        {
            Vector2 sPos = self.worldBound.center;
            float sWidthRadius = self.worldBound.width / 2;
            float sHeghtRadius = self.worldBound.height / 2;

            Vector2 cPos = content.worldBound.center;
            float cWidthRadius = content.worldBound.width / 2;
            float cHeghtRadius = content.worldBound.height / 2;

            bool top = sPos.y + sHeghtRadius > cPos.y + cHeghtRadius;
            bool bottom = sPos.y - sHeghtRadius < cPos.y - cHeghtRadius;
            bool right = sPos.x + sWidthRadius > cPos.x + cWidthRadius;
            bool left = sPos.x - sWidthRadius < cPos.x - cWidthRadius;

            return top && bottom && right && left;
        }

        /// <summary>
        /// 判断指定的类型 <paramref name="type"/> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
        /// </summary>
        /// <param name="type">需要测试的类型。</param>
        /// <param name="generic">泛型接口类型，传入 typeof(IXxx<>)</param>
        /// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
        public static bool HasImplementedRawGeneric([NotNull] this Type type, [NotNull] Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // 测试接口。
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

            // 测试某个类型是否是指定的原始接口。
            bool IsTheRawGenericType(Type test)
            => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }
#endif
        public static bool IsNullOrDefault<T>(this T argument)
        {
            // deal with normal scenarios
            if (argument == null) return true;
            if (object.Equals(argument, default(T))) return true;

            // deal with non-null nullables
            Type methodType = typeof(T);
            if (Nullable.GetUnderlyingType(methodType) != null) return false;

            // deal with boxed value types
            Type argumentType = argument.GetType();
            if (argumentType.IsValueType && argumentType != methodType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        /// <summary>
        /// 设置成员数据
        /// </summary>
        public static bool ExSetValue(this object self, string propertyName, object value)
        {
            try
            {
                self.GetType().
                    GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).
                    SetValue(self, value);
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
        /// <summary>
        /// 设置成员数据
        /// </summary>
        public static object ExGetValue(this object self, string propertyName)
        {
            try
            {
                return self.GetType().
                    GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).
                    GetValue(self);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }

        /// <summary>
        /// 设置变量数据
        /// </summary>
        public static bool ExSetValueField(this object self, string propertyName, object value)
        {
            try
            {
                self.GetType().
                    GetField(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).
                    SetValue(self, value);
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// 获取成员数据
        /// </summary>
        public static object ExGetValueField(this object self, string propertyName)
        {
            try
            {
                return self.GetType().
                    GetField(propertyName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).
                    GetValue(self);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
        public static object ExGetValue(this MemberInfo memberInfo, object forObject)
        {
            switch (memberInfo.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo)memberInfo).GetValue(forObject);
                case MemberTypes.Property:
                    return ((PropertyInfo)memberInfo).GetValue(forObject);
                default:
                    return null;
            }
        }
        public static T ExGet<T>(this List<T> self, int index, T defaultValue = default(T))
        {
            if (index >= 0 && index < self.Count)
            {
                return self[index];
            }
            return defaultValue;
        }
        public static T ExGet<T>(this IReadOnlyList<T> self, int index, T defaultValue = default(T))
        {
            if (index >= 0 && index < self.Count)
            {
                return self[index];
            }
            return defaultValue;
        }
        /// <summary>
        /// 条件添加属性
        /// </summary>
        public static void ExAdd(this List<Attribute> self, Attribute attribute, Func<Attribute, bool> condition)
        {
            if (condition == null)
            {
                self.Add(attribute);
                return;
            }
            bool valid = false;
            for (int i = 0, length = self.Count; i < length; i++)
            {
                var attr = self[i];
                if (!condition.Invoke(attr))
                {
                    valid = false;
                    break;
                }
            }
            if (valid)
            {
                self.Add(attribute);
            }
        }
        public static bool ExIsSorted<T>(List<T> list) where T : IComparable<T>
        {
            if (list == null || list.Count <= 1)
            {
                return true;
            }

            for (int i = 0, count = list.Count - 1; i < count; i++)
            {
                if (list[i].CompareTo(list[i + 1]) > 0)
                {
                    return false;
                }
            }

            return true;
        }

        #region Method
        /// <summary>
        /// Search for a method by name and parameter types.  
        /// Unlike GetMethod(), does 'loose' matching on generic
        /// parameter types, and searches base interfaces.
        /// </summary>
        /// <exception cref="AmbiguousMatchException"/>
        public static MethodInfo ExGetMethod(this Type thisType,
                                                string name,
                                                params Type[] parameterTypes)
        {
            return ExGetMethod(thisType,
                                name,
                                BindingFlags.Instance
                                | BindingFlags.Static
                                | BindingFlags.Public
                                | BindingFlags.NonPublic
                                | BindingFlags.FlattenHierarchy,
                                parameterTypes);
        }

        /// <summary>
        /// Search for a method by name, parameter types, and binding flags.  
        /// Unlike GetMethod(), does 'loose' matching on generic
        /// parameter types, and searches base interfaces.
        /// </summary>
        /// <exception cref="AmbiguousMatchException"/>
        public static MethodInfo ExGetMethod(this Type thisType,
                                                string name,
                                                BindingFlags bindingFlags,
                                                params Type[] parameterTypes)
        {
            MethodInfo matchingMethod = null;

            // Check all methods with the specified name, including in base classes
            ExGetMethod(ref matchingMethod, thisType, name, bindingFlags, parameterTypes);

            // If we're searching an interface, we have to manually search base interfaces
            if (matchingMethod == null && thisType.IsInterface)
            {
                foreach (Type interfaceType in thisType.GetInterfaces())
                    ExGetMethod(ref matchingMethod,
                                 interfaceType,
                                 name,
                                 bindingFlags,
                                 parameterTypes);
            }

            return matchingMethod;
        }

        private static void ExGetMethod(ref MethodInfo matchingMethod,
                                            Type type,
                                            string name,
                                            BindingFlags bindingFlags,
                                            params Type[] parameterTypes)
        {
            // Check all methods with the specified name, including in base classes
            foreach (MethodInfo methodInfo in type.GetMember(name,
                                                             MemberTypes.Method,
                                                             bindingFlags))
            {
                // Check that the parameter counts and types match,
                // with 'loose' matching on generic parameters
                ParameterInfo[] parameterInfos = methodInfo.GetParameters();
                if (parameterInfos.Length == parameterTypes.Length)
                {
                    int i = 0;
                    for (; i < parameterInfos.Length; ++i)
                    {
                        if (!parameterInfos[i].ParameterType
                                              .IsSimilarType(parameterTypes[i]))
                            break;
                    }
                    if (i == parameterInfos.Length)
                    {
                        if (matchingMethod == null)
                            matchingMethod = methodInfo;
                        else
                            throw new AmbiguousMatchException(
                                  "More than one matching method found!");
                    }
                }
            }
        }
        /// <summary>
        /// Special type used to match any generic parameter type in ExGetMethod().
        /// </summary>
        public class T
        { }

        /// <summary>
        /// Determines if the two types are either identical, or are both generic
        /// parameters or generic types with generic parameters in the same
        ///  locations (generic parameters match any other generic paramter,
        /// but NOT concrete types).
        /// </summary>
        private static bool IsSimilarType(this Type thisType, Type type)
        {
            // Ignore any 'ref' types
            if (thisType.IsByRef)
                thisType = thisType.GetElementType();
            if (type.IsByRef)
                type = type.GetElementType();

            // Handle array types
            if (thisType.IsArray && type.IsArray)
                return thisType.GetElementType().IsSimilarType(type.GetElementType());

            // If the types are identical, or they're both generic parameters
            // or the special 'T' type, treat as a match
            if (thisType == type || ((thisType.IsGenericParameter || thisType == typeof(T))
                                 && (type.IsGenericParameter || type == typeof(T))))
                return true;

            // Handle any generic arguments
            if (thisType.IsGenericType && type.IsGenericType)
            {
                Type[] thisArguments = thisType.GetGenericArguments();
                Type[] arguments = type.GetGenericArguments();
                if (thisArguments.Length == arguments.Length)
                {
                    for (int i = 0; i < thisArguments.Length; ++i)
                    {
                        if (!thisArguments[i].IsSimilarType(arguments[i]))
                            return false;
                    }
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Enum Extension
        public static int ToInt(this System.Enum e)
        {
            return e.GetHashCode();
        }
        #endregion

        #region string Extension
        public static string ToSafe(this string self)
        {
            self = self.Replace("{", "{{").Replace("}", "}}");
            return self;
        }

        private static char[] s_fuzzySpaceArray = new[] { ' ' };
        public static bool ContainsFuzzy(this string self, string query, bool ignoreCase = true)
        {
            if (string.IsNullOrEmpty(query))
            {
                return false;
            }
            var keywords = query.Split(s_fuzzySpaceArray, StringSplitOptions.RemoveEmptyEntries);
            if (ignoreCase)
            {
                return keywords.All(keyword => self.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else
            {
                return keywords.All(keyword => self.IndexOf(keyword) >= 0);
            }
        }
        #endregion

        #region Time
        public static string ToStringEx(this TimeSpan timeSpan)
        {
            var reuslt = string.Empty;
            if (timeSpan.Days > 0) reuslt += $"{timeSpan.Days}天";
            if (timeSpan.Hours > 0) reuslt += $"{timeSpan.Hours}时";
            if (timeSpan.Minutes > 0) reuslt += $"{timeSpan.Minutes}分";
            if (timeSpan.Seconds > 0) reuslt += $"{timeSpan.Seconds}秒";
            if (timeSpan.Milliseconds >= 0) reuslt += $"{timeSpan.Milliseconds}毫秒";
            return reuslt;
        }
        #endregion

        #region Array
        /// <summary>
        /// 简单的foreach操作。
        /// </summary>
        public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }
        #endregion
    }
}

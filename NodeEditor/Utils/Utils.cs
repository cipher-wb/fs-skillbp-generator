using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Concurrent;



#if !NodeExport
using GraphProcessor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
#endif

namespace NodeEditor
{
    public static partial class Utils
    {
#if !NodeExport
        #region UI组件相关
        public static Button CreateTextBtn(string text = null, Action action = null)
        {
            Button textBtn = new Button();
            //Button textBtn = PoolObject.Get<Button>();

            if (!string.IsNullOrEmpty(text))
                textBtn.text = text;

            if (action != null)
                textBtn.clickable.clicked += action;

            textBtn.style.flexWrap = Wrap.Wrap;
            StyleColor styleColor = new StyleColor(new Color(0, 0, 0, 0));
            textBtn.style.backgroundColor = styleColor;
            textBtn.style.borderBottomColor = styleColor;
            textBtn.style.unityTextAlign = TextAnchor.UpperLeft;
            return textBtn;
        }

        public static void OnSelectCilcked(this VisualElement self, bool isSelect = true)
        {
            if (self != null)
                self.style.opacity = isSelect ? 1f : 0.7f;
        }

        #endregion
#endif
        #region 模拟键盘输入
        [DllImport("User32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
            byte bVk, //虚拟键值 对应按键的ascll码十进制值  
            byte bScan, //0
            int dwFlags, //0 为按下，1按住，2为释放 
            int dwExtraInfo //0
        );
        #endregion

        #region 判断文件是否被占用
        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
        public static bool IsFileOpened(string path)
        {
            try
            {
                IntPtr vHandle = _lopen(path, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    return true;
                }

                CloseHandle(vHandle);
                return false;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return true;
            }
        }

        #endregion

        #region IP地址
        public enum AddressType
        {
            IPv4, IPv6
        }
        /// <summary>
        /// 获取本机IP
        /// TODO 本地虚拟机情况获取有问题
        /// </summary>
        /// <param name="Addfam">要获取的IP类型</param>
        /// <returns></returns>
        public static string IP(AddressType fam)
        {
            if (fam == AddressType.IPv6 && !Socket.OSSupportsIPv6)
            {
                return null;
            }
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
                NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;
                if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (fam == AddressType.IPv4)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                        else if (fam == AddressType.IPv6)
                        {
                            if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                output = ip.Address.ToString();
                            }
                        }
                    }
                }
            }
            return output;
        }
        public static string IP_End()
        {
            return IP(AddressType.IPv4).Split('.')[3];
        }

        #endregion

        #region 表格-辅助函数
        // 缓存枚举位组定义
        private static readonly Dictionary<Type, int> enumValidBitMasks = new Dictionary<Type, int>();
        public static string GetEnumDescription<T>(T obj)
        {
            var type = obj?.GetType();
            if (type?.IsEnum == true)
            {
                return GetEnumDescription(type, obj.GetHashCode(), string.Empty);
            }
            else
            {
                Log.Error("GetEnumDescription error, obj is not enum type");
                return string.Empty;
            }
        }
        public static string GetEnumDescription(Type enumType, int obj, string defaultDesc = null)
        {
            try
            {
                if (enumType == null)
                {
                    Log.Error("GetEnumDescription error, type is null");
                    return defaultDesc;
                }
                if (!enumType.IsEnum)
                {
                    Log.Error($"GetEnumDescription error, type is not enum: {enumType.Name}");
                    return defaultDesc;
                }

                if (!enumType.IsDefined(typeof(FlagsAttribute)))
                {
                    var name = Enum.GetName(enumType, obj);
                    if (name == null)
                    {
                        Log.Error($"GetEnumDescription error, name is null, type: {enumType.Name}, obj: {obj}");
                        return defaultDesc ?? enumType.Name;
                    }
                    return GetDescriptionField(enumType, name);
                }
                else
                {
                    // 位组，需要遍历
                    var names = string.Empty;
                    foreach (var value in Enum.GetValues(enumType))
                    {
                        if ((obj & (int)value) != 0)
                        {
                            var name = Enum.GetName(enumType, value);
                            var desc = GetDescriptionField(enumType, name);
                            names += string.IsNullOrEmpty(names) ? desc : $",{desc}";
                        }
                    }
                    if (string.IsNullOrEmpty(names))
                    {
                        names = defaultDesc;
                    }
                    return names;
                }

            }
            catch (Exception ex)
            {
                Log.Fatal($"GetEnumDescription exception, {ex}");
                return defaultDesc;
            }
        }
        // 判断枚举是否被定义（包括位组Flags枚举）
        public static bool IsEnumDefined(Type enumType, object value)
        {
            if (!enumType.IsEnum)
            {
                return false;
            }
            if (!enumType.IsDefined(typeof(FlagsAttribute))) return Enum.IsDefined(enumType, value);

            if (!enumValidBitMasks.TryGetValue(enumType, out int validMask))
            {
                validMask = 0;
                foreach (var val in Enum.GetValues(enumType))
                {
                    if (Enum.IsDefined(enumType, val))
                        validMask |= (int)(object)val;
                }
                enumValidBitMasks[enumType] = validMask;
            }

            return ((int)(object)value & ~validMask) == 0;
        }


        public static string GetDescription<T>(T obj, string propName)
        {
            return GetDescription(obj?.GetType(), propName);
        }
        public static string GetDescription(Type type, string propertyName)
        {
            var prop = type?.GetProperty(propertyName);
            if (prop != null)
            {
                var descAttr = prop.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                return descAttr?.Description ?? propertyName;
            }
            return propertyName;
        }
        public static string GetDescriptionField(Type type, string fieldName)
        {
            var field = type?.GetField(fieldName);
            if (field != null)
            {
                var descAttr = field.GetCustomAttribute(typeof(DescriptionAttribute), false) as DescriptionAttribute;
                return descAttr?.Description ?? fieldName;
            }
            return fieldName;
        }

        public static T ObjectToEnum<T>(object o) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), o.ToString(), true);
            }
            catch
            {
                return default(T);
            }
        }
        #endregion

        #region 耗时-辅助函数
        public static long WatchTime(string info, Action action, bool enableLog = true)
        {
            long elapsedMilliseconds = 0;
            var stopwatch = PoolObject.Get<Stopwatch>();
            {
                stopwatch.Restart();
                SafeCall(action);
                stopwatch.Stop();
                elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                if (enableLog)
                {
                    Log.Debug($"【耗时 {stopwatch.Elapsed.ToStringEx()}】\t: {info}");
                }
            }
            PoolObject.Release(stopwatch);
            return elapsedMilliseconds;
        }

        public static string TimeMilliseconds2String(long milliseconds)
        {
            var timeSpan = TimeSpan.FromMilliseconds(milliseconds);
            return timeSpan.ToStringEx();
        }
        #endregion

        #region 安全性-辅助函数
        public static bool SafeCall(Action a)
        {
            try
            {
                a?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                Log.Exception(e);
                return false;
            }
        }

        #endregion

#if !NodeExport

        #region EditorWindow-辅助函数-注意<窗口函数只能主线程调用>
        public static T CreateWindow<T>(Func<T, bool> condition) where T : EditorWindow
        {
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));
            if (condition != null)
            {
                foreach (var win in windows)
                {
                    if (condition(win))
                    {
                        return win;
                    }
                }
            }
            return EditorWindow.CreateWindow<T>(typeof(T));
        }
        public static bool HasOpenWindow<T>(Func<T, bool> condition = null) where T : EditorWindow
        {
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));
            var validWindows = windows.Where(w =>
            {
                return condition?.Invoke(w) ?? true;
            }).ToArray();
            return validWindows.Length > 0;
        }
        public static T[] GetAllWindow<T>(Func<T, bool> condition = null) where T : EditorWindow
        {
            // TODO 性能优化FindObjectsOfTypeAll
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));
            var validWindows = windows.Where(w =>
            {
                return condition?.Invoke(w) ?? true;
            }).ToArray();
            return validWindows;
        }
        public static bool CloseAllWindow<T>(Func<T, bool> condition = null) where T : EditorWindow
        {
            try
            {
                var windows = GetAllWindow<T>(condition);
                if (windows != null)
                {
                    foreach (var win in windows)
                    {
                        win.Close();
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        public static T GetEnoughWindow<T>(Func<T, bool> condition = null) where T : EditorWindow
        {
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));
            var validWindows = windows.Where(w =>
            {
                return condition?.Invoke(w) ?? true;
            }).ToArray().FirstOrDefault();
            return validWindows;
        }

        public static T GetWindow<T>(Func<T, bool> condition = null) where T : EditorWindow
        {
            var windows = (T[])Resources.FindObjectsOfTypeAll(typeof(T));

            var validWindow = windows.FirstOrDefault(w =>
            {
                return condition?.Invoke(w) ?? true;
            });
            return validWindow;
        }

        #endregion

        public static string GetEnumAllFlagsText<TEnum>(TEnum enumObj) where TEnum : Enum
        {
            try
            {
                var enumType = typeof(TEnum);
                var text = string.Empty;
                foreach (TEnum value in Enum.GetValues(enumType))
                {
                    if (!enumObj.HasFlag(value)) continue;
                    var name = Enum.GetName(enumType, value);
                    if (name == null) continue;

                    var field = enumType.GetField(name);
                    var descAttr = field.GetCustomAttribute(typeof(LabelTextAttribute), false) as LabelTextAttribute;
                    if (descAttr != null && !string.IsNullOrEmpty(descAttr.Text))
                    {
                        if (string.IsNullOrEmpty(text))
                        {
                            text = descAttr.Text;
                        }
                        else
                        {
                            text += $"|{descAttr.Text}";
                        }
                    }
                }
                return text;
            }
            catch
            {
                return string.Empty;
            }
        }
#endif

        #region 拷贝-辅助函数
        /// <summary>
        /// 有死循环的风险，用下面那个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopyByReflection<T>(T obj)
        {
            if (obj is string || obj.GetType().IsValueType)
                return obj;
            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var field in fields)
            {
                try
                {
                    field.SetValue(retval, DeepCopyByReflection(field.GetValue(obj)));
                }
                catch { }
            }
            return (T)retval;
        }
        public static T DeepCopyByBinary<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }
        #endregion

        #region 格式化路径
        private const string assetsFolder = "Assets/";
        public static string PathFull2Assets(string fullPath)
        {
            fullPath = PathFormat(fullPath);
            var index = fullPath.IndexOf(assetsFolder);
            if (index == -1)
            {
                return fullPath;
            }
            return fullPath.Substring(index);
        }
        public static string PathFull2AssetsFolder(string fullPath)
        {
            fullPath = PathFormat(fullPath);
            var index = fullPath.IndexOf(assetsFolder);
            if (index == -1)
            {
                return fullPath;
            }
            return fullPath.Substring(index + assetsFolder.Length);
        }

        public static string PathFull2SeparationFolder(string fullPath, string separationPath)
        {
            fullPath = PathFormat(fullPath);
            var index = fullPath.IndexOf(separationPath);
            if (index == -1)
            {
                return fullPath;
            }
            return fullPath.Substring(index + separationPath.Length);
        }

        public static string PathFormat(string path)
        {
            return path.Replace("\\", "/");
        }
        public static void PathFormat(ref string path)
        {
            path = path.Replace("\\", "/");
        }
        public static string GetFullPath(string path)
        {
            var fullPath = Path.GetFullPath(path);
            PathFormat(ref fullPath);
            return fullPath;
        }
        #endregion

        #region File
        public static bool DeleteFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
        public static bool WriteAllText(string path, string contents)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }
                string directoryName = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                File.WriteAllText(path, contents);
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
        public static string ReadAllText(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return string.Empty;
            }
        }
        public static bool ReplaceAllText(string path, string oldValue, string newValue)
        {
            try
            {
                var content = ReadAllText(path);
                content = content.Replace(oldValue, newValue, StringComparison.Ordinal);
                WriteAllText(path, content);
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
        public static T ReadFromJson<T>(string path) where T : class
        {
            try
            {
                if (!File.Exists(path)) return null;

                var jsonContent = ReadAllText(path);
                return JsonConvert.DeserializeObject<T>(jsonContent);
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
        public static async Task ReadFromJsonAsync<T>(string path, Action<T> action) where T : class
        {
            try
            { 
                if (!File.Exists(path))
                {
                    action.Invoke(null);
                    return;
                }
                await Task.Run(() =>
                { 
                    var jsonContent = ReadAllText(path);
                    T jsonObject = JsonConvert.DeserializeObject<T>(jsonContent);
                    action.Invoke(jsonObject);
                });
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                action.Invoke(null);
            }
        }

        public static bool WriteToJson<T>(T instance, string path) where T : class
        {
            try
            {
                var jsonContent = JsonConvert.SerializeObject(instance, Formatting.Indented);
                WriteAllText(path, jsonContent);
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }
        public static async Task<bool> WriteToJsonAsync<T>(T instance, string path, Action<bool> action) where T : class
        {
            try
            {
                await Task.Run(() =>
                {
                    var jsonContent = JsonConvert.SerializeObject(instance, Formatting.Indented);
                    WriteAllText(path, jsonContent);
                    action?.Invoke(true);
                });
                return true;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                action?.Invoke(false);
                return false;
            }
        }

        public static T GetOrCreateInstanceFromJson<T>(string path) where T : class
        {
            try
            {
                var inst = ReadFromJson<T>(path);
                if (inst == null)
                {
                    inst = Activator.CreateInstance<T>();
                    var content = JsonConvert.SerializeObject(inst);
                    WriteAllText(path, content);
                }
                return inst;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }
            return null;
        }

        public static bool IsDirtyJson(object instance, string path)
        {
            try
            {
                var fileExists = File.Exists(path);
                if (instance == null) return false;
                if (!fileExists) return true;

                var jsonInstance = JsonConvert.SerializeObject(instance, Formatting.Indented);
                var jsonFile = File.ReadAllText(path);
                return jsonInstance != jsonFile;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return false;
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        public static void OpenDirectory(string path)
        {
            Utils.SafeCall(() =>
            {
                if (string.IsNullOrEmpty(path)) return;

                path = path.Replace("/", "\\");
                if (!System.IO.Directory.Exists(path))
                {
                    Log.Error("No Directory: " + path);
                    return;
                }
                Process.Start("explorer.exe", path);
            });
        }
        #endregion

        #region String
        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="sta">开始字符</param>
        /// <param name="end">结束字符</param>
        /// <returns></returns> 
        public static string GetMiddleString(string str, string sta, string end)
        {
            Regex rg = new Regex("(?<=(" + sta + "))[.\\s\\S]*?(?=(" + end + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }
        #endregion

        #region PropertyConverter
        public static List<T> ConvertToTypedList<T>(PropertyInfo property, object obj)
        {
            var value = property.GetValue(obj);
            return value is IReadOnlyList<T> readOnlyList
                ? new List<T>(readOnlyList)
                : throw new InvalidCastException();
        }

        public static IList ConvertToList(PropertyInfo property, object obj)
        {
            var value = property.GetValue(obj);

            if (value is IEnumerable enumerable)
            {
                Type elementType = property.PropertyType.GenericTypeArguments[0];
                Type listType = typeof(List<>).MakeGenericType(elementType);
                IList list = (IList)Activator.CreateInstance(listType);

                foreach (var item in enumerable)
                {
                    list.Add(item);
                }
                return list;
            }

            throw new InvalidCastException();
        }

        // 缓存字段信息，避免重复反射耗时
        private static readonly ConcurrentDictionary<Type, FieldInfo[]> FieldCache = new();
        // 强制转换对象及其嵌套成员中所有的 IReadOnlyList 为 List
        public static void ConvertReadOnlyListsToLists(object obj)
        {
            var processed = new HashSet<object>(ReferenceEqualityComparer.Instance);
            RecursiveConvertProcess(obj, processed);
        }
        private static void RecursiveConvertProcess(object obj, HashSet<object> processed)
        {
            if (obj == null || !processed.Add(obj)) return;

            Type type = obj.GetType();
            if (type.IsValueType || type == typeof(string)) return;

            // 从缓存中读取字段，大幅提升大数据量下的速度
            if (!FieldCache.TryGetValue(type, out var fields))
            {
                fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                FieldCache.TryAdd(type, fields);
            }

            foreach (var field in fields)
            {
                object value = field.GetValue(obj);
                if (value == null) continue;

                Type fieldType = field.FieldType;

                // 命中 IReadOnlyList 接口
                if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(IReadOnlyList<>))
                {
                    // 如果已经是 List<T> 了，直接递归内部元素即可，无需替换
                    if (value is IList && value.GetType().IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                    {
                        foreach (var item in (IEnumerable)value) RecursiveConvertProcess(item, processed);
                        continue;
                    }

                    // 执行替换逻辑
                    Type elementType = fieldType.GetGenericArguments()[0];
                    Type listType = typeof(List<>).MakeGenericType(elementType);
                    var newList = (IList)Activator.CreateInstance(listType);

                    foreach (var item in (IEnumerable)value)
                    {
                        newList.Add(item);
                        RecursiveConvertProcess(item, processed); // 递归处理元素
                    }

                    field.SetValue(obj, newList);
                }
                else
                {
                    // 普通对象或集合的递归处理
                    if (value is IEnumerable enumerable && !(value is string))
                    {
                        foreach (var item in enumerable) RecursiveConvertProcess(item, processed);
                    }
                    else
                    {
                        RecursiveConvertProcess(value, processed);
                    }
                }
            }
        }
        #endregion

#if UNITY_EDITOR
        #region Editor
        public static void RefeshAllWindow(EditorWindow ignoreWindow = null)
        {
            var windows = Utils.GetAllWindow<ConfigGraphWindow>();

            foreach(var window in windows)
            {
                if (null != ignoreWindow && window == ignoreWindow)
                    continue;

                window.RefreshWindow();
            }
        }

        public static List<T> GetGraphTeNodes<T> (BaseGraph graph)
        {
            List<T> nodes = new List<T>();

            foreach(var node in graph.nodes)
            {
                if(node is T tNode)
                {
                    nodes.Add(tNode);
                }
            }

            return nodes;
        }

        private static StringBuilder sbProcessInfo = new StringBuilder();
        public static void DisplayProcess(string title, Action<StringBuilder> action, LogLevel logLevel = LogLevel.Info, EditorFlag editorFlag = EditorFlag.DisplayDialog | EditorFlag.DisplayProcess)
        {
            try
            {
                sbProcessInfo.Length = 0;
                editorFlag.DisplayProgressBar(title, "开始处理...", 0);
                long elapsedMilliseconds = WatchTime(null, () =>
                {
                    action?.Invoke(sbProcessInfo);
                }, false);

                var timeInfo = $"{title}, 处理完毕, 耗时: {TimeMilliseconds2String(elapsedMilliseconds)}\n\n";
                var info = $"{timeInfo}{sbProcessInfo.ToString(0, Math.Min(sbProcessInfo.Length, 255))}";
                if (sbProcessInfo.Length > 255)
                {
                    info += $"\n\n详情见Console界面...";
                }
                editorFlag.DisplayDialog(title, info, "好的");
                Log.EnqueueLog(timeInfo + sbProcessInfo.ToString(), logLevel);
            }
            catch (System.Exception ex)
            {
                Log.Exception(ex);
            }
            finally
            {
                sbProcessInfo.Length = 0;
                EditorUtility.ClearProgressBar();
                //try { GUIUtility.ExitGUI(); } catch { }
            }
        }

        public static string GetConfigNodeTypeName(int iType)
        {
            switch(iType)
            { 
                case 1:
                    return "SkillEffectConfig";
                case 2:
                    return "SkillConditionConfig";
                case 3:
                    return "SkillSelectConfig";
                default:
                    return string.Empty;
            }
           
        }
        public static int GetConfigNodeType(string typeName)
        {
            switch (typeName)
            {
                case "SkillEffectConfig":
                    return 1;
                case "SkillConditionConfig":
                    return 2;
                case "SkillSelectConfig":
                    return 3;
                default:
                    return 0;
            }
        }

        public static bool PingObject(string path, Type type = null)
        {
            // 默认定位文本
            if (string.IsNullOrEmpty(path)) return false;
            type ??= typeof(TextAsset);
            var assetObject = AssetDatabase.LoadAssetAtPath(path, type);
            if (assetObject != null)
            {
                EditorGUIUtility.PingObject(assetObject);
                Selection.activeObject = assetObject;
                return true;
            }
            return false;
        }

        public static bool CheckSVNConflict(string path, Action<bool> action = null)
        {
            var isConflict = false;
            if (string.IsNullOrEmpty(path)) return isConflict;
            isConflict = File.Exists(path + ".mine");
            if (isConflict)
            {
                UnityEditor.EditorUtility.DisplayDialog("文件冲突", $"文件有冲突,请先解决svn冲突!\n{path}", "好的");
                PingObject(path);
            }
            action?.Invoke(isConflict);
            return isConflict;
        }

        #endregion
#endif
    }
}

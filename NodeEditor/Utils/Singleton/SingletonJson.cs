using System;
using System.Linq;

namespace NodeEditor
{
    public class SingletonJson<T> where T : class
    {
        private static T inst;
        public static T Inst
        {
            get
            {
                if (inst == null)
                {
                    LoadOrCreate();
                }
                return inst;
            }
        }
        private static T LoadOrCreate()
        {
            var filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                inst = Utils.ReadFromJson<T>(filePath);
                if (inst == null)
                {
                    inst = Activator.CreateInstance<T>();
                    Log.Debug($"{nameof(SingletonJson<T>)}: Create Instance: {filePath}");
                }
                else
                {
                    Log.Debug($"{nameof(SingletonJson<T>)}: Load Instance: {filePath}");
                }
            }
            else
            {
                Log.Error($"{nameof(SingletonJson<T>)}: Load Instance failed, can not find filePath");
            }
            return inst;
        }

        public static void Save()
        {
            if (inst == null)
            {
                Log.Error("Cannot save SingletonJson: no instance!");
                return;
            }

            var filePath = GetFilePath();
            if (!string.IsNullOrEmpty(filePath))
            {
                Utils.WriteToJson(inst, filePath);
            }
        }
        protected static string GetFilePath()
        {
            try
            {
                return typeof(T).GetCustomAttributes(typeof(JsonFilePathAttribute), true)
                      .Cast<JsonFilePathAttribute>()
                      .FirstOrDefault(v => v != null)
                      ?.filePath;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return null;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class JsonFilePathAttribute : Attribute
    {
        internal string filePath;
        public JsonFilePathAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new NodeEditorException("Invalid relative path (it is empty)");
            }
            if (path[0] == '/')
            {
                path = path.Substring(1);
            }
            filePath = path;
        }
    }
}

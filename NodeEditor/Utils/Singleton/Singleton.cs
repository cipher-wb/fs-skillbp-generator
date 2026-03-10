using System;

namespace NodeEditor
{
    public class Singleton<T> where T : class, new()
    {
        private static bool hasInstanced = false;
        private static T inst;

        public static T Inst
        {
            get
            {
                CreateInstance();
                return inst;
            }
        }
        public static void CreateInstance()
        {
            if (!hasInstanced)
            {
                hasInstanced = true;
                inst = Activator.CreateInstance<T>();
            }
        }
        public static void ShutdownInstance()
        {
            hasInstanced = false;
            inst = null;
        }
        protected Singleton() { }
    }
}

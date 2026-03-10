using System;
using System.Collections.Generic;

namespace NodeEditor
{
    public static class PoolObject
    {
        static Dictionary<Type, Queue<object>> poolObjects = new Dictionary<Type, Queue<object>>();

        public static T Get<T>(bool createIfNeed = true)
        {
            return (T)Get(typeof(T), createIfNeed);
        }

        public static object Get(Type t, bool createIfNeed = true)
        {
            Queue<object> pool = GetPool(t);
            if (pool.Count == 0)
            {
                if (createIfNeed)
                    return Activator.CreateInstance(t);
                else
                    return null;
            }
            else
            {
                return pool.Dequeue();
            }
        }

        public static void Release(object o)
        {
            Queue<object> pool = GetPool(o.GetType());
            pool.Enqueue(o);
        }

        private static Queue<object> GetPool(Type t)
        {
            Queue<object> pool;
            if (poolObjects.TryGetValue(t, out pool) == false)
            {
                pool = new Queue<object>();
                poolObjects.Add(t, pool);
            }
            return pool;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NodeEditor
{
    /// <summary>
    /// 简单的事件中心，用法比较自由
    /// 不用定义事件参数，声明一个常量作为事件名即可
    /// </summary>
    public class EditorEventManager : Singleton<EditorEventManager>
    {
        public Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

        public void AddListenter(string name, Action action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo).Actions += action;
            else this.eventDic.Add(name, new EventInfo(action));
        }

        public void RemoveListenter(string name, Action action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo).Actions -= action;
        }

        public void EventTrigger(string name)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo).EventTrigger();
        }
        public void AddListenter<T1>(string name, Action<T1> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1>).Actions += action;
            else this.eventDic.Add(name, new EventInfo<T1>(action));
        }

        public void RemoveListenter<T1>(string name, Action<T1> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1>).Actions -= action;
        }

        public void EventTrigger<T1>(string name, T1 info)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1>).EventTrigger(info);
        }

        public void AddListenter<T1, T2>(string name, Action<T1, T2> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2>).Actions += action;
            else this.eventDic.Add(name, new EventInfo<T1, T2>(action));
        }

        public void RemoveListenter<T1, T2>(string name, Action<T1, T2> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2>).Actions -= action;
        }

        public void EventTrigger<T1, T2>(string name, T1 info1, T2 info2)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2>).EventTrigger(info1, info2);
        }


        public void AddListenter<T1, T2, T3>(string name, Action<T1, T2, T3> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3>).Actions += action;
            else this.eventDic.Add(name, new EventInfo<T1, T2, T3>(action));
        }

        public void RemoveListenter<T1, T2, T3>(string name, Action<T1, T2, T3> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3>).Actions -= action;
        }

        public void EventTrigger<T1, T2, T3>(string name, T1 info1, T2 info2, T3 info3)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3>).EventTrigger(info1, info2, info3);
        }

        public void AddListenter<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3, T4>).Actions += action;
            else this.eventDic.Add(name, new EventInfo<T1, T2, T3, T4>(action));
        }

        public void RemoveListenter<T1, T2, T3, T4>(string name, Action<T1, T2, T3, T4> action)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3, T4>).Actions -= action;
        }

        public void EventTrigger<T1, T2, T3, T4>(string name, T1 info1, T2 info2, T3 info3, T4 info4)
        {
            if (this.eventDic.TryGetValue(name, out IEventInfo eventInfo)) (eventInfo as EventInfo<T1, T2, T3, T4>).EventTrigger(info1, info2, info3, info4);
        }


        public void Clear()
        {
            this.eventDic.Clear();
        }
    }

    public interface IEventInfo { }

    public class EventInfo : IEventInfo
    {
        public event Action Actions;

        public EventInfo(Action action)
        {
            Actions += action;
        }

        public void EventTrigger()
        {
            Actions?.Invoke();
        }
    }

    public class EventInfo<T1> : IEventInfo
    {
        public event Action<T1> Actions;

        public EventInfo(Action<T1> action)
        {
            Actions += action;
        }

        public void EventTrigger(T1 Info)
        {
            Actions?.Invoke(Info);
        }
    }

    public class EventInfo<T1, T2> : IEventInfo
    {
        public event Action<T1, T2> Actions;

        public EventInfo(Action<T1, T2> action)
        {
            Actions += action;
        }

        public void EventTrigger(T1 Info1, T2 info2)
        {
            Actions?.Invoke(Info1, info2);
        }
    }

    public class EventInfo<T1, T2, T3> : IEventInfo
    {
        public event Action<T1, T2, T3> Actions;

        public EventInfo(Action<T1, T2, T3> action)
        {
            Actions += action;
        }

        public void EventTrigger(T1 Info1, T2 info2, T3 info3)
        {
            Actions?.Invoke(Info1, info2, info3);
        }
    }

    public class EventInfo<T1, T2, T3, T4> : IEventInfo
    {
        public event Action<T1, T2, T3, T4> Actions;

        public EventInfo(Action<T1, T2, T3, T4> action)
        {
            Actions += action;
        }

        public void EventTrigger(T1 Info1, T2 info2, T3 info3, T4 info4)
        {
            Actions?.Invoke(Info1, info2, info3, info4);
        }
    }
}

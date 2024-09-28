using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemCenter : MonoSingleton<EventSystemCenter>
{
    private Dictionary<string,IeventInfo> events = new Dictionary<string, IeventInfo>();
    public void AddEventListener(string name, UnityAction action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo).actions += action;
        }
        else
        {
            events.Add(name,new EventInfo(action));
        }
    }
    public void RemoveEventListener(string name,UnityAction action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo).actions -= action;
        }
    }
    public void ClearAllListeners()
    {
        events.Clear();
    }
    public void EventTrigger(string name)
    {
        if (events.ContainsKey(name))
        {
            if ((events[name] as EventInfo).actions != null)
            {
                (events[name] as EventInfo).actions.Invoke();
            }
        }
    }

    public void AddEventListener<T>(string name, UnityAction<T> action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo<T>).actions += action;
        }
        else
        {
            events.Add(name, new EventInfo<T>(action));
        }
    }
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if (events.ContainsKey(name))
        {
            (events[name] as EventInfo<T>).actions -= action;
        }
    }

    public void EventTrigger<T>(string name, T info)
    {
        if (events.ContainsKey(name))
        {
            if ((events[name] as EventInfo<T>).actions != null)
            {
                (events[name] as EventInfo<T>).actions.Invoke(info);
            }
        }
    }
}
public interface IeventInfo
{ }
public class EventInfo : IeventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        actions += action;
    }
}
public class EventInfo<T> : IeventInfo
{
    public UnityAction<T> actions;
    public EventInfo (UnityAction<T> action)
    {
        actions += action;
    }
}
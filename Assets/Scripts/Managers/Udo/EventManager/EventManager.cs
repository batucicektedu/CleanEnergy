using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace udoEventSystem
{
    public static class EventManager
    {
        private static readonly EventHub _eventHub = new EventHub();

        public static T Get<T>() where T : IEvent,
        new()
        {
            return _eventHub.Get<T>();
        }
    }

    public class EventHub
    {
        private Dictionary<Type, IEvent> _events = new Dictionary<Type, IEvent>();

        public T Get<T>() where T : IEvent,
        new()
        {
            Type eType = typeof(T);
            IEvent eventToReturn;
            if (_events.TryGetValue(eType, out eventToReturn))
                return (T)eventToReturn;

            eventToReturn = (AEvent)Activator.CreateInstance(eType);
            _events.Add(eType, eventToReturn);
            return (T)eventToReturn;
        }
    }

    public interface IEvent
    {
    }

    public abstract class AEvent : IEvent
    {
        private Action callback;
        private Dictionary<GameObject, Action> _subscriberHandlerDictionary = new Dictionary<GameObject, Action>();

        public virtual void AddListener(Action handler, GameObject subscriber = null)
        {
            if (subscriber == null) callback += handler;
            else
            {
                if (!_subscriberHandlerDictionary.ContainsKey(subscriber))
                {
                    _subscriberHandlerDictionary.Add(subscriber, handler);
                    callback += handler;
                }
            }
        }

        public void RemoveListener(Action handler, GameObject subscriber = null)
        {
            if (subscriber == null) callback -= handler;
            else
            {
                if (_subscriberHandlerDictionary.ContainsKey(subscriber))
                {
                    _subscriberHandlerDictionary.Remove(subscriber);
                    callback -= handler;
                }
            }
        }

        public void Execute()
        {
            var subscribersToRemove = _subscriberHandlerDictionary.Where(x => x.Key == null || !x.Key.activeInHierarchy).ToArray();
            foreach (var subscriber in subscribersToRemove)
            {
                callback -= _subscriberHandlerDictionary[subscriber.Key];
                _subscriberHandlerDictionary.Remove(subscriber.Key);
            }

            callback?.Invoke();
        }
    }

    public abstract class AEvent<T1> : AEvent
    {
        private Action<T1> callback;

        public void AddListener(Action<T1> handler)
        {
            callback += handler;
        }

        public void RemoveListener(Action<T1> handler)
        {
            callback -= handler;
        }

        public void Execute(T1 arg1)
        {
            callback?.Invoke(arg1);
        }
    }

    public abstract class AEvent<T1, T2> : AEvent
    {
        private Action<T1, T2> callback;

        public void AddListener(Action<T1, T2> handler)
        {
            callback += handler;
        }

        public void RemoveListener(Action<T1, T2> handler)
        {
            callback -= handler;
        }

        public void Execute(T1 arg1, T2 arg2)
        {
            callback?.Invoke(arg1, arg2);
        }
    }


}
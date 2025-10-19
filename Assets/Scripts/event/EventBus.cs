using System;
using System.Collections.Generic;

namespace EventBusEx.@event
{
    public class EventBus
    {
        public static EventBus Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventBus();
                }

                return _instance;
            }
        }

        private static EventBus _instance;
        private EventBus() {}

        private readonly Dictionary<Type, List<Action<IEvent>>> _subscribers = new();

        public void Subscribe<T>(Action<T> callback) where T : IEvent
        {
            var eventType = typeof(T);

            if (!_subscribers.ContainsKey(eventType))
            {
                _subscribers[eventType] = new List<Action<IEvent>>();
            }
            
            _subscribers[eventType].Add(e => callback((T) e));
        }

        public void Unsubscribe<T>(Action<T> callback) where T : IEvent
        {
            if (_subscribers.TryGetValue(typeof(T), out var actions))
            {
                actions.RemoveAll(action => action.Target == callback.Target);
            }
        }
        
        public void NotifyAll<T>(T @event) where T : IEvent
        {
            var eventType = typeof(T);

            if (_subscribers.TryGetValue(eventType, out var actions))
            {
                actions.ForEach(callback => callback(@event));
            }
        }
        
        public void Clear()
        {
            _subscribers.Clear();
        }
    }
}

using System;
using System.Collections.Generic;

namespace EventBusEx.@event
{
    public sealed class EventBus
    {
        public static EventBus Instance => _instance.Value;
        private static readonly Lazy<EventBus> _instance =
            new Lazy<EventBus>(() => new EventBus());

        private readonly object _lock = new();
        private readonly Dictionary<Type, List<Action<IEvent>>> _subscribers = new();

        private EventBus() {}

        /// <summary>
        /// Subscribe to an event of type T
        /// </summary>
        /// <typeparam name="T">Type of the event</typeparam>
        /// <param name="callback">The callback to invoke when the event is published</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Subscribe<T>(Action<T> callback) where T : IEvent
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var eventType = typeof(T);

            lock (_lock)
            {
                if (!_subscribers.ContainsKey(eventType))
                {
                    _subscribers[eventType] = new List<Action<IEvent>>();
                }

                _subscribers[eventType].Add(e => callback((T)e));
            }
        }

        /// <summary>
        /// Unsubscribe from an event of type T
        /// </summary>
        /// <typeparam name="T">Type of the event</typeparam>
        /// <param name="callback">The callback to invoke when the event is published</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Unsubscribe<T>(Action<T> callback) where T : IEvent
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }
            var eventType = typeof(T);
            lock (_lock)
            {
                if (_subscribers.TryGetValue(typeof(T), out var actions))
                {
                    actions.RemoveAll(action => action.Target == callback.Target);
                    if (actions.Count == 0)
                    {
                        _subscribers.Remove(eventType);
                    }
                }
            }
        }
        
        /// <summary>
        /// Notify all subscribers of an event of type T
        /// </summary>
        /// <typeparam name="T">Type of the event</typeparam>
        /// <param name="event">The event to publish</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void NotifyAll<T>(T @event) where T : IEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            var eventType = typeof(T);
            List<Action<IEvent>> actionsCopy;

            lock (_lock)
            {
                if (!_subscribers.TryGetValue(eventType, out var actions) || actions.Count == 0)
                {
                    return;
                }

                actionsCopy = new(actions);
            }

            foreach (var callback in actionsCopy)
            {
                try
                {
                    callback(@event);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogException(ex);
                }
            }
        }

        /// <summary>
        /// Clear all subscribers
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _subscribers.Clear();
            }
        }
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Code.Scripts.Support
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Subscriptions = new();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);

            if (!Subscriptions.ContainsKey(type))
            {
                Subscriptions[type] = new List<Delegate>();
            }

            Subscriptions[type].Add(handler);
        }

        public static void Publish<T>(T evt)
        {
            if (!Subscriptions.TryGetValue(typeof(T), out var handlers))
            {
                return;
            }

            foreach (var handler in handlers.Cast<Action<T>>())
            {
                handler(evt);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace CODE.Scripts.Core
{
	public class EventBus
	{
		private readonly Dictionary<Type, Action<object>> _subscribers = new();

		public void Subscribe<T>(Action<T> callback)
		{
			if (_subscribers.TryGetValue(typeof(T), out var existing))
			{
				_subscribers[typeof(T)] = existing + (e => callback((T)e));
			}
			else
			{
				_subscribers[typeof(T)] = e => callback((T)e);
			}
		}

		public void Publish<T>(T evt)
		{
			if (_subscribers.TryGetValue(typeof(T), out var callback))
			{
				callback.Invoke(evt);
			}
		}
	}
}
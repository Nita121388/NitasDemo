using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nita.ToolKit.NAudio.Util
{
    public class MessageBus
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public void Subscribe<TMessage>(Action<TMessage> action)
        {
            if (!_subscribers.ContainsKey(typeof(TMessage)))
            {
                _subscribers[typeof(TMessage)] = new List<Delegate>();
            }
            _subscribers[typeof(TMessage)].Add(action);
        }

        public void Unsubscribe<TMessage>(Action<TMessage> action)
        {
            if (_subscribers.TryGetValue(typeof(TMessage), out List<Delegate> subscribers))
            {
                subscribers.Remove(action);
            }
        }

        public void Publish<TMessage>(TMessage message)
        {
            if (_subscribers.TryGetValue(typeof(TMessage), out List<Delegate> subscribers))
            {
                foreach (var subscriber in subscribers)
                {
                    if (subscriber is Action<TMessage> typedSubscriber)
                    {
                        typedSubscriber(message);
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace CrunchApiExplorer.Framework.MVVM
{
    public class Messenger : IMessenger
    {
        private readonly Dictionary<Type, object> _channels = new Dictionary<Type, object>(); 
        private readonly object _channelsLock = new object();

        public IObservable<T> Messages<T>()
        {
            var channel = GetChannel<T>();
            return channel ;
        } 

        public void Publish<T>(T message)
        {
            var channel = GetChannel<T>();
            channel.OnNext(message);
        }

        private Subject<T> GetChannel<T>()
        {
            lock (_channelsLock)
            {
                object channel;
                if (!_channels.TryGetValue(typeof(T), out channel))
                {
                    channel = new Subject<T>();
                    _channels.Add(typeof(T), channel);
                }

                return (Subject<T>) channel;
            }
        }
    }
}

using System;

namespace CrunchApiExplorer.Framework.MVVM
{
    public interface IMessenger
    {
        IObservable<T> Messages<T>();
        void Publish<T>(T message);
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#region  Client Code

var subject = new FilteredObservable<string>(s => s.StartsWith("[IMPORTANT]"));
        
var observer1 = new Observer("Observer 1");
var observer2 = new Observer("Observer 2");

using var subscription1 = subject.Subscribe(observer1);
using var subscription2 = subject.Subscribe(observer2);

// 这些消息将被过滤
subject.Notify("Normal Message 1");
subject.Notify("Normal Message 2");

// 这些消息将被传递
subject.Notify("[IMPORTANT] Message 1");
subject.Notify("[IMPORTANT] Message 2");

// 取消订阅 observer2
subscription2.Dispose();

// 再次通知
subject.Notify("[IMPORTANT] Message 3");  // 只有 observer1 收到

// 错误通知（不过滤）
subject.NotifyError(new Exception("Critical Error"));

// 完成通知（不过滤）
subject.OnCompleted();

#endregion

#region Subject


public class FilteredObservable<T> : IObservable<T>
{
    private readonly List<IObserver<T>> _observers = new();
    private readonly Func<T, bool> _filter;
    private readonly object _lock = new();

    public FilteredObservable(Func<T, bool> filter)
    {
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        lock (_lock)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }
        return new Unsubscriber(() =>
        {
            lock (_lock)
            {
                _observers.Remove(observer);
            }
        });
    }

    public async Task Notify(T value)
    {
        if (!_filter(value)) return;

        IObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
        }

        foreach (var observer in observersCopy)
        {
            try
            {
                observer.OnNext(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification failed: {ex.Message}");
            }
        }
    }

    public async Task NotifyError(Exception error)
    {
        IObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
            _observers.Clear();
        }

        foreach (var observer in observersCopy)
        {
            try
            { 
                observer.OnError(error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error notification failed: {ex.Message}");
            }
        }
    }

    public async Task OnCompleted()
    {
        IObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
            _observers.Clear();
        }

        foreach (var observer in observersCopy)
        {
            try
            {
                observer.OnCompleted();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Completion notification failed: {ex.Message}");
            }
        }
    }

    private class Unsubscriber : IDisposable
    {
        private readonly Action _unsubscribeAction;

        public Unsubscriber(Action unsubscribeAction) => _unsubscribeAction = unsubscribeAction;

        public void Dispose() => _unsubscribeAction?.Invoke();
    }
}

#endregion

#region Observer

public class Observer : IObserver<string>
{
    private readonly string _name;

    public Observer(string name)
    {
        _name = name;
    }

    public void OnCompleted()
    {
        Console.WriteLine($"{_name} completed.");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"{_name} error: {error.Message}");
    }

    public void OnNext(string value)
    {
        Console.WriteLine($"{_name} received: {value}");
    }
}
#endregion

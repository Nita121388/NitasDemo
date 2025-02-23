using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

#region Client Code
var subject = new AsyncSubject<string>();
        
var observer1 = new AsyncObserver("Observer 1");
var observer2 = new AsyncObserver("Observer 2");

// 订阅观察者
using var subscription1 = subject.Subscribe(observer1);
using var subscription2 = subject.Subscribe(observer2);

// 异步通知
await subject.NotifyAsync("First Message");
        
// 取消订阅 observer2
subscription2.Dispose();

// 再次通知
await subject.NotifyAsync("Second Message");
        
// 错误通知
await subject.NotifyErrorAsync(new Exception("Test Error"));
        
// 完成通知
await subject.OnCompletedAsync();

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
#endregion

#region Interfaces
public interface IAsyncObserver<in T>
{
    Task OnNextAsync(T value);
    Task OnErrorAsync(Exception exception);
    Task OnCompletedAsync();
}

public interface IAsyncObservable<out T>
{
    IDisposable Subscribe(IAsyncObserver<T> observer);
}
#endregion

#region Async Subject
public class AsyncSubject<T> : IAsyncObservable<T>
{
    private readonly List<IAsyncObserver<T>> _observers = new();
    private readonly object _lock = new();

    public IDisposable Subscribe(IAsyncObserver<T> observer)
    {
        lock (_lock)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }
        return new Unsubscriber(() =>
        {
            lock (_lock)
            {
                _observers.Remove(observer);
            }
        });
    }

    public async Task NotifyAsync(T value)
    {
        IAsyncObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
        }

        foreach (var observer in observersCopy)
        {
            try
            {
                await observer.OnNextAsync(value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification failed: {ex.Message}");
            }
        }
    }

    public async Task NotifyErrorAsync(Exception error)
    {
        IAsyncObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
            _observers.Clear();
        }

        foreach (var observer in observersCopy)
        {
            try
            {
                await observer.OnErrorAsync(error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error notification failed: {ex.Message}");
            }
        }
    }

    public async Task OnCompletedAsync()
    {
        IAsyncObserver<T>[] observersCopy;
        lock (_lock)
        {
            observersCopy = _observers.ToArray();
            _observers.Clear();
        }

        foreach (var observer in observersCopy)
        {
            try
            {
                await observer.OnCompletedAsync();
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

        public Unsubscriber(Action unsubscribeAction)
        {
            _unsubscribeAction = unsubscribeAction;
        }

        public void Dispose() => _unsubscribeAction?.Invoke();
    }
}
#endregion

#region Async Observer
public class AsyncObserver : IAsyncObserver<string>
{
    private readonly string _name;

    public AsyncObserver(string name) => _name = name;

    public async Task OnNextAsync(string value)
    {
        await Task.Delay(100); // 模拟异步处理
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {_name} received: {value}");
    }

    public async Task OnErrorAsync(Exception exception)
    {
        await Task.Delay(100); // 模拟异步处理
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {_name} error: {exception.Message}");
    }

    public async Task OnCompletedAsync()
    {
        await Task.Delay(100); // 模拟异步处理
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {_name} completed");
    }
}
#endregion
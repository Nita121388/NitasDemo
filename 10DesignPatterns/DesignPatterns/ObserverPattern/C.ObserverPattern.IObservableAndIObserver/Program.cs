// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Generic;

#region  Client Code

// 1. 创建被观察者和观察者
Subject subject = new Subject();
ConcreteObserver observer1 = new ConcreteObserver("Observer 1");
ConcreteObserver observer2 = new ConcreteObserver("Observer 2");

// 2. 订阅观察者
IDisposable subscription1 = subject.Subscribe(observer1);
IDisposable subscription2 = subject.Subscribe(observer2);

// 状态通知
subject.NotifyObservers("Hello, World!");
            
// 取消订阅 observer2
subscription2.Dispose();
            
// 再次设置状态，观察者1会收到通知，观察者2不会收到
subject.NotifyObservers("Hello again!");
            
// 模拟错误 此时会清空观察者列表
subject.OnError(new Exception("Something went wrong!"));
            
// 再次设置状态，观察者1和观察者2都不会收到通知
subject.NotifyObservers("Hello again!");

// 再次订阅观察者
ConcreteObserver observer3 = new ConcreteObserver("Observer 3");
IDisposable subscription3 = subject.Subscribe(observer3);
            
// 再次设置状态，观察者3收到通知
subject.NotifyObservers("Hello again!");
            
// 完成通知
subject.OnCompleted();
            
//再次设置状态，都不会收到通知
subject.NotifyObservers("Hello again!");

// 等待用户输入后退出
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
#endregion

#region Subject

// Subject 类实现了 IObservable<string> 接口，用于管理观察者并通知状态变化
public class Subject : System.IObservable<string>
{
    // 用于存储所有订阅的观察者
    private List<IObserver<string>> _observers = new();
    // 用于线程安全的锁对象
    private readonly object _lock = new();

    // 订阅方法，允许观察者订阅状态变化
    public IDisposable Subscribe(IObserver<string> observer)
    {
        lock (_lock) // 确保线程安全
        {
            if (!_observers.Contains(observer)) // 防止重复订阅
            {
                _observers.Add(observer);
            }
        }
        // 返回一个 Unsubscriber 对象，用于取消订阅
        return new Unsubscriber(_observers, observer, _lock);
    }

    // SetState 方法用于设置状态并通知所有观察者
    public void NotifyObservers(string state)
    {
        lock (_lock) // 确保线程安全
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(state); // 调用观察者的 OnNext 方法通知状态变化
            }
        }
    }

    // OnCompleted 方法用于通知所有观察者完成事件
    public void OnCompleted()
    {
        lock (_lock) // 确保线程安全
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted(); // 调用观察者的 OnCompleted 方法通知完成事件
            }
            _observers.Clear(); // 清空观察者列表
        }
    }

    // OnError 方法用于通知所有观察者发生错误
    public void OnError(Exception error)
    {
        lock (_lock) // 确保线程安全
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error); // 调用观察者的 OnError 方法通知错误事件
            }
            _observers.Clear(); // 清空观察者列表
        }
    }
    
    
    // Unsubscriber 类实现了 IDisposable 接口，用于取消观察者的订阅
    private class Unsubscriber : IDisposable
    {
        private System.Collections.Generic.List<IObserver<string>> _observers;
        private IObserver<string> _observer;
        private readonly object _lock;

        // 构造函数，初始化观察者列表、当前观察者和锁对象
        public Unsubscriber(List<IObserver<string>> observers, IObserver<string> observer, object lockObj)
        {
            _observers = observers;
            _observer = observer;
            _lock = lockObj;
        }

        // Dispose 方法用于取消订阅
        public void Dispose()
        {
            lock (_lock) // 确保线程安全
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _observers.Remove(_observer); // 从观察者列表中移除当前观察者
                    _observer = null; // 清空当前观察者引用
                }
            }
        }
    }

}



#endregion

#region ConcreteObserver

// ConcreteObserver 类实现了 IObserver<string> 接口，用于接收状态变化通知
public class ConcreteObserver : IObserver<string>
{
    // 观察者的名称，用于区分不同的观察者
    private readonly string _name;

    // 构造函数，初始化观察者名称
    public ConcreteObserver(string name)
    {
        _name = name;
    }

    // OnNext 方法用于接收状态变化通知
    public void OnNext(string value)
    {
        Console.WriteLine($"{_name} received: {value}"); // 输出接收到的状态信息
    }

    // OnError 方法用于接收错误通知
    public void OnError(Exception error)
    {
        Console.WriteLine($"{_name} received an error: {error.Message}"); // 输出错误信息
    }

    // OnCompleted 方法用于接收完成通知
    public void OnCompleted()
    {
        Console.WriteLine($"{_name} received completion notification."); // 输出完成通知
    }
}

#endregion


using System;
using System.Collections.Generic;

#region  Client Code

// 创建目标对象
Subject subject = new Subject();

// 创建观察者
Observer observer1 = new Observer("Observer1");
Observer observer2 = new Observer("Observer2");

// 观察者1注册对所有方面的兴趣
subject.RegisterObserver(Aspect.StateChange, observer1.OnStateChange);
subject.RegisterObserver(Aspect.DataUpdate, observer1.OnDataUpdate);
subject.RegisterObserver(Aspect.ErrorOccurred, observer1.OnError);

// 观察者2仅注册对错误方面的兴趣
subject.RegisterObserver(Aspect.ErrorOccurred, observer2.OnError);

// 模拟目标对象状态变化
subject.ChangeState();
subject.UpdateData();
subject.ErrorOccurred();

// 观察者2取消对错误方面的兴趣
subject.UnregisterObserver(Aspect.ErrorOccurred, observer2.OnError);

// 再次触发错误事件，观察者2不再接收通知
subject.ErrorOccurred();
#endregion

#region Aspect

// 定义方面（Aspect）枚举，表示目标对象可能的状态变化类型
public enum Aspect
{
    StateChange,
    DataUpdate,
    ErrorOccurred
}

#endregion

#region Subject

// 目标对象类
public class Subject
{
    // 用于存储观察者订阅的方面
    private Dictionary<Aspect, List<Action<string>>> observers = new Dictionary<Aspect, List<Action<string>>>();

    public Subject()
    {
        // 初始化方面列表
        foreach (Aspect aspect in Enum.GetValues(typeof(Aspect)))
        {
            observers[aspect] = new List<Action<string>>();
        }
    }

    // 注册观察者
    public void RegisterObserver(Aspect aspect, Action<string> observer)
    {
        observers[aspect].Add(observer);
        Console.WriteLine($"Observer registered for aspect: {aspect}");
    }

    // 取消注册观察者
    public void UnregisterObserver(Aspect aspect, Action<string> observer)
    {
        observers[aspect].Remove(observer);
        Console.WriteLine($"Observer unregistered from aspect: {aspect}");
    }

    // 通知观察者
    public void NotifyObservers(Aspect aspect, string message)
    {
        if (observers[aspect].Count > 0)
        {
            Console.WriteLine($"Notifying observers for aspect: {aspect}");
            foreach (var observer in observers[aspect])
            {
                observer(message);
            }
        }
        else
        {
            Console.WriteLine($"No observers registered for aspect: {aspect}");
        }
    }

    // 模拟目标对象状态变化
    public void ChangeState()
    {
        Console.WriteLine("Subject state changed.");
        NotifyObservers(Aspect.StateChange, "State has changed.");
    }

    public void UpdateData()
    {
        Console.WriteLine("Subject data updated.");
        NotifyObservers(Aspect.DataUpdate, "Data has been updated.");
    }

    public void ErrorOccurred()
    {
        Console.WriteLine("Error occurred in the subject.");
        NotifyObservers(Aspect.ErrorOccurred, "An error has occurred.");
    }
}

#endregion

#region Observer
// 观察者类
public class Observer
{
    private string name;

    public Observer(string name)
    {
        this.name = name;
    }

    public void OnStateChange(string message)
    {
        Console.WriteLine($"{name} received state change notification: {message}");
    }

    public void OnDataUpdate(string message)
    {
        Console.WriteLine($"{name} received data update notification: {message}");
    }

    public void OnError(string message)
    {
        Console.WriteLine($"{name} received error notification: {message}");
    }
}
#endregion
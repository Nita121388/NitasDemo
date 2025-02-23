
using System;
using System.Collections.Generic;

#region Client Code

Subject subject = new Subject();

// 创建观察者并注册到被观察者
ConcreteObserver observer1 = new ConcreteObserver(subject);
ConcreteObserver observer2 = new ConcreteObserver(subject);

// 改变被观察者的状态
Console.WriteLine("Setting state to 5:");
subject.State = 5;  // 输出基础状态

Console.WriteLine("\nSetting state to 15:");
subject.State = 15; // 输出基础状态和补充信息

#endregion

#region Interface Code

public interface IObserver
{
    void Update(int state);  // 接收基础状态
}

#endregion

#region Subject
public class Subject
{
    private List<IObserver> observers = new List<IObserver>();
    private int state;  // 被观察者的状态

    public int State
    {
        get { return state; }
        set
        {
            state = value;
            NotifyObservers();  // 状态改变时通知观察者
        }
    }

    // 注册观察者
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    // 移除观察者
    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    // 通知观察者
    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(state);  // 推送基础状态
        }
    }
}

#endregion

#region ConcreteObserver


public class ConcreteObserver : IObserver
{
    private Subject subject;  // 持有被观察者的引用

    public ConcreteObserver(Subject subject)
    {
        this.subject = subject;
        subject.Attach(this);  // 注册到被观察者
    }

    public void Update(int state)
    {
        Console.WriteLine($"Received base state: {state}");

        // 根据需要拉取补充信息
        if (state > 10)
        {
            int additionalInfo = subject.State;  // 拉取补充信息
            Console.WriteLine($"Additional info: {additionalInfo}");
        }
    }
}

#endregion

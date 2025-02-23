using System;
using System.Collections.Generic;

#region  Client Code

// 自动触发演示
Console.WriteLine("=== Automatic Trigger Demo ===");
var autoSubject = new AutoTriggerSubject();
var obs1 = new StateObserver(autoSubject);
autoSubject.Attach(obs1);

autoSubject.SetMainState(10);  // 触发通知
autoSubject.SetMainState(20);  // 再次触发

// 手动触发演示
Console.WriteLine("\n=== Manual Trigger Demo ===");
var manualSubject = new ManualTriggerSubject();
var obs2 = new StateObserver(manualSubject);
manualSubject.Attach(obs2);

manualSubject.CompleteUpdate(1, 100);
manualSubject.CompleteUpdate(2, 200);
manualSubject.CompleteUpdate(3, 300);
manualSubject.Notify();  // 单次触发

#endregion

#region abstract Subject

// 抽象目标对象
abstract class Subject
{
    protected HashSet<IObserver> observers = new HashSet<IObserver>();
    protected int mainState;
    protected int secondaryState;

    public void Attach(IObserver observer) => observers.Add(observer);
    public void Detach(IObserver observer) => observers.Remove(observer);
    public abstract void Notify();

    protected void PrepareUpdate(int main, int secondary)
    {
        mainState = main;
        secondaryState = secondary;
    }

    public void ShowState()
    {
        Console.WriteLine($"State: [{mainState}, {secondaryState}]");
    }
}

#endregion

#region IObserver

// 观察者接口
interface IObserver
{
    void Update();
}

#endregion

#region ConcreteObservers

// 具体观察者
class StateObserver : IObserver
{
    private readonly Subject subject;

    public StateObserver(Subject subject)
    {
        this.subject = subject;
    }

    public void Update()
    {
        Console.Write("Observer received update: ");
        subject.ShowState();
    }
}

#endregion

#region AutoTriggerSubject

// 自动触发实现
class AutoTriggerSubject : Subject
{
    public void SetMainState(int value)
    {
        mainState = value;
        Notify(); // 自动触发
    }

    public override void Notify()
    {
        Console.WriteLine("[AutoTrigger] Notifying observers...");
        foreach (var observer in observers)
        {
            observer.Update();
        }
    }
}

#endregion

#region ManualTriggerSubject

// 手动触发实现
class ManualTriggerSubject : Subject
{
    public void CompleteUpdate(int main, int secondary)
    {
        PrepareUpdate(main, secondary);
        // 不自动触发
    }

    public override void Notify()
    {
        Console.WriteLine("[ManualTrigger] Notifying observers...");
        foreach (var observer in observers)
        {
            observer.Update();
        }
    }
}
#endregion


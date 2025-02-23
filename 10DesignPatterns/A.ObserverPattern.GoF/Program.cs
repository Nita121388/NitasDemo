
#region  Client Code

// 创建具体目标对象
ConcreteSubject subject = new ConcreteSubject();

// 创建多个具体观察者对象
ConcreteObserver observer1 = new ConcreteObserver();
ConcreteObserver observer2 = new ConcreteObserver();
ConcreteObserver observer3 = new ConcreteObserver();

// 将观察者对象注册到目标对象中
subject.Attach(observer1);
subject.Attach(observer2);
subject.Attach(observer3);

// 改变目标对象的状态，触发通知
Console.WriteLine("第一次状态更新：");
subject.SetState("Hello, Observers!");

// 移除一个观察者
subject.Detach(observer2);

// 再次改变目标对象的状态，触发通知
Console.WriteLine("\n第二次状态更新：");
subject.SetState("State has changed!");

#endregion

#region Interface Code

// 观察者接口
public interface IObserver
{
    void Update(string message);
}

// 目标接口
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

#endregion

#region Implementation of Interfaces
// 具体目标
public class ConcreteSubject : ISubject
{
    // 观察者列表
    private List<IObserver> _observers = new();
    // 目标状态
    private string _state;

    // 注册观察者: 将观察者对象注册到目标对象中
    public void Attach(IObserver observer) => _observers.Add(observer);
    // 注销观察者: 移除一个观察者
    public void Detach(IObserver observer) => _observers.Remove(observer);
    
    // 通知观察者: 改变目标对象的状态，触发通知
    public void Notify()
    {
        foreach (var observer in _observers)
            observer.Update(_state);
    }

    // 设置目标状态: 改变目标对象的状态
    public void SetState(string state)
    {
        _state = state;
        Notify();
    }
}

// 具体观察者
public class ConcreteObserver : IObserver
{
    // 接收通知并处理
    public void Update(string message) => Console.WriteLine($"Received: {message}");
}
#endregion

#region Client Code

Subject subject = new Subject();

IObserver observer1 = new ConcreteObserver("Observer 1", subject);
IObserver observer2 = new ConcreteObserver("Observer 2", subject);

subject.Attach(observer1);
subject.Attach(observer2);

subject.Notify("Hello Observers!");

// 在销毁目标对象之前，通知所有观察者解除订阅
subject.Dispose();

// 尝试再次通知（应该不会有任何效果，因为观察者已被移除）
subject?.Notify("This should not be received.");

#endregion

#region Interface

// 定义观察者接口
public interface IObserver
{
    void Update(string message);
    void Unsubscribe();
}

// 定义目标（主题）接口
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string message);
}

#endregion

#region 定义具体目标（主题）类 : Subject

// 定义具体目标（主题）类
public class Subject : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in _observers)
        {
            observer.Update(message);
        }
    }

    private bool _disposed = false;

    public void Dispose()
    {
        if (!_disposed)
        {
            var observersCopy = new List<IObserver>(_observers);

            foreach (var observer in observersCopy)
            {
                observer.Unsubscribe();
            }

            _observers.Clear();
            _disposed = true;
        }
    }
}


#endregion

#region 具体观察者类 ： ConcreteObserver
// 定义具体观察者类
public class ConcreteObserver : IObserver
{
    private string _name;
    private Subject _subject;

    public ConcreteObserver(string name, Subject subject)
    {
        _name = name;
        _subject = subject;
    }

    public void Update(string message)
    {
        Console.WriteLine($"{_name} received message: {message}");
    }

    public void Unsubscribe()
    {
        if (_subject != null)
        {
            _subject.Detach(this);
            _subject = null;
        }
    }
}
#endregion

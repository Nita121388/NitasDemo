// See https://aka.ms/new-console-template for more information

#region  Client Code

// 创建 EventSubject 和 EventObserver 对象
EventSubject subject = new EventSubject();
EventObserver observer = new EventObserver();

// 订阅事件
observer.Subscribe(subject);
Console.WriteLine("Observer has subscribed to the subject.");

// 改变状态，触发事件
subject.SetState("State 1");
subject.SetState("State 2");

// 取消订阅
observer.Unsubscribe(subject);
Console.WriteLine("Observer has unsubscribed from the subject.");

// 再次改变状态，观察是否还会触发事件
subject.SetState("State 3"); // 不会触发事件，因为已取消订阅
#endregion

#region EventSubject

public class EventSubject
{
    public event EventHandler<string> StateChanged;

    private string _state;
    public void SetState(string state)
    {
        _state = state;
        StateChanged?.Invoke(this, _state);
    }
}

#endregion

#region EventObserver

public class EventObserver
{
    public void Subscribe(EventSubject subject)
    {
        subject.StateChanged += HandleStateChange;
    }

    private void HandleStateChange(object sender, string message)
    {
        Console.WriteLine($"Event received: {message}");
    }

    public void Unsubscribe(EventSubject subject)
    {
        subject.StateChanged -= HandleStateChange;
    }
}

#endregion
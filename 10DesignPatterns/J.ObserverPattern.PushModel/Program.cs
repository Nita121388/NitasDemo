
#region Client Code

// 创建一个天气站对象
WeatherStation weatherStation = new WeatherStation();

// 创建两个观察者对象
Display display1 = new Display();
Display display2 = new Display();

// 将观察者添加到天气站的观察者列表
weatherStation.Attach(display1);
weatherStation.Attach(display2);

// 模拟天气站更新数据
Console.WriteLine("天气站更新温度为 25.5℃：");
weatherStation.SetMeasurements(25.5f);

// 移除一个观察者
weatherStation.Detach(display1);

// 再次更新数据
Console.WriteLine("\n天气站更新温度为 28.0℃：");
weatherStation.SetMeasurements(28.0f);

Console.ReadLine();

#endregion

#region IObserver 接口
// 观察者接口
public interface IObserver
{
    void Update(WeatherData data);
}
#endregion

#region 具体观察者 ：Display 类
// 具体观察者
public class Display : IObserver
{
    public void Update(WeatherData data)
    {
        Console.WriteLine($"当前温度：{data.Temperature}℃");
    }
}
#endregion

#region  Subject 类: WeatherStation 类

// 目标对象
public class WeatherStation
{
    private List<IObserver> _observers = new List<IObserver>();
    private float _temperature;

    public void SetMeasurements(float temp)
    {
        _temperature = temp;
        NotifyObservers(new WeatherData(temp));
    }

    private void NotifyObservers(WeatherData data)
    {
        foreach (var observer in _observers)
        {
            observer.Update(data);
        }
    }

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
}

// 数据传输对象
public class WeatherData
{
    public float Temperature { get; }

    public WeatherData(float temperature)
    {
        Temperature = temperature;
    }
}

#endregion


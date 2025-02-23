using System;
using System.Collections.Generic;

#region Client Code

// 创建股票市场对象
StockMarket stockMarket = new StockMarket();

// 创建投资者
Investor investor1 = new Investor("Alice", stockMarket);
Investor investor2 = new Investor("Bob", stockMarket);

// 将投资者添加到股票市场的观察者列表
stockMarket.Attach(investor1);
stockMarket.Attach(investor2);

// 模拟股票价格变化
Console.WriteLine("Stock price changes to $100.");
stockMarket.StockPrice = 100;

Console.WriteLine("Stock price changes to $120.");
stockMarket.StockPrice = 120;

// 移除一个投资者
stockMarket.Detach(investor1);

Console.WriteLine("Stock price changes to $150.");
stockMarket.StockPrice = 150;

Console.ReadLine();
#endregion

#region Interface Code

// 观察者接口
public interface IObserver
{
    void Update();
}

// 目标接口
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}

#endregion

#region Concrete Classes

#region Concrete Subject : StockMarket

// 具体目标类
public class StockMarket : ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private decimal stockPrice;

    public decimal StockPrice
    {
        get { return stockPrice; }
        set
        {
            stockPrice = value;
            Notify();
        }
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update();
        }
    }
}

#endregion

#region Concrete Observer : Investor
// 具体观察者类
public class Investor : IObserver
{
    private string name;
    private ISubject stockMarket;

    public Investor(string name, ISubject stockMarket)
    {
        this.name = name;
        this.stockMarket = stockMarket;
    }

    public void Update()
    {
        decimal currentPrice = ((StockMarket)stockMarket).StockPrice;
        Console.WriteLine($"{name} received notification. Current stock price: {currentPrice:C}");
    }
}

#endregion

#endregion

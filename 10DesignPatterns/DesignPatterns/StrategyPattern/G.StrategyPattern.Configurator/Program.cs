#region Client Code


#region 初始化注册
// 初始化注册（通常在程序启动时）
// 注册策略
PaymentStrategyFactory.RegisterStrategy("CreditCardPayment", new CreditCardPayment());
PaymentStrategyFactory.RegisterStrategy("PayPalPayment", new PayPalPayment());

// 测试客户端
var client = new OptimizedClient();
client.Checkout("CreditCard");
client.Checkout("PayPal");
//client.Checkout("UnknownType"); // 使用默认策略
#endregion

class OptimizedClient
{
    public void Checkout(string paymentType)
    {
        // 通过配置映射获取实际策略key
        var strategyKey = AppConfig.PaymentStrategies.TryGetValue(paymentType, out var key) 
            ? key 
            : AppConfig.PaymentStrategies["Default"];

        var strategy = PaymentStrategyFactory.GetStrategy(strategyKey);
        strategy.ProcessPayment(100);
    }
}


#endregion

#region 策略接口
public interface IPaymentStrategy
{
    void ProcessPayment(decimal amount);
}
#endregion

#region 具体策略类
// 示例策略类：信用卡支付
public class CreditCardPayment : IPaymentStrategy
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing Credit Card payment for amount: {amount}");
    }
}

// 示例策略类：PayPal支付
public class PayPalPayment : IPaymentStrategy
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing PayPal payment for amount: {amount}");
    }
}
#endregion

#region 策略工厂与注册中心
public class PaymentStrategyFactory
{
    private static readonly Dictionary<string, IPaymentStrategy> _strategies = new();

    // 注册策略
    public static void RegisterStrategy(string key, IPaymentStrategy strategy)
    {
        if (!_strategies.ContainsKey(key))
        {
            _strategies.Add(key, strategy);
        }
    }

    // 获取策略
    public static IPaymentStrategy GetStrategy(string key)
    {
        if (_strategies.TryGetValue(key, out var strategy))
        {
            return strategy;
        }
        throw new KeyNotFoundException($"未找到支付策略：{key}");
    }
}
#endregion

#region 配置中心
public static class AppConfig
{
    public static readonly Dictionary<string, string> PaymentStrategies = new()
    {
        { "CreditCard", "CreditCardPayment" },
        { "PayPal", "PayPalPayment" },
        { "Default", "CreditCard" }
    };
}
#endregion

#region 动态策略注册扩展
// 自动注册所有实现IPaymentStrategy的类型
public static class DynamicStrategyRegistration
{
    public static void RegisterAllStrategies()
    {
        var strategyTypes = System.Reflection.Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IPaymentStrategy).IsAssignableFrom(t) && !t.IsInterface);

        foreach (var type in strategyTypes)
        {
            var instance = System.Activator.CreateInstance(type) as IPaymentStrategy;
            if (instance != null)
            {
                PaymentStrategyFactory.RegisterStrategy(type.Name, instance);
            }
        }
    }
}
#endregion
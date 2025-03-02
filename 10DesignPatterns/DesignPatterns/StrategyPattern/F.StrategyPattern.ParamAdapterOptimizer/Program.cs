#region 使用示例
var context = new PaymentContext();

// 信用卡支付
var creditCardParams = new CreditCardAdapter("1234-5678-9012-3456", "123");
context.SetStrategy(new CreditCardStrategy());
context.ExecutePayment(100.0, creditCardParams);

// 支付宝支付
var alipayParams = new AlipayAdapter("13812345678");
context.SetStrategy(new AlipayStrategy());
context.ExecutePayment(200.0, alipayParams);
#endregion

#region 接口定义
// 通用参数接口
public interface IPaymentParameters
{
    T GetParameter<T>(string key);
}

// 策略接口
public interface IPaymentStrategy
{
    void ProcessPayment(double amount, IPaymentParameters parameters);
}
#endregion

#region 参数适配器
// 信用卡参数适配器
public class CreditCardAdapter : IPaymentParameters
{
    private readonly string _cardNumber;
    private readonly string _securityCode;

    public CreditCardAdapter(string cardNumber, string securityCode)
    {
        _cardNumber = cardNumber;
        _securityCode = securityCode;
    }

    public T GetParameter<T>(string key)
    {
        switch (key)
        {
            case "CardNumber": return (T)(object)_cardNumber;
            case "SecurityCode": return (T)(object)_securityCode;
            default: throw new KeyNotFoundException($"参数 {key} 不存在");
        }
    }
}

// 支付宝参数适配器
public class AlipayAdapter : IPaymentParameters
{
    private readonly string _phoneNumber;

    public AlipayAdapter(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public T GetParameter<T>(string key)
    {
        if (key == "PhoneNumber") return (T)(object)_phoneNumber;
        throw new KeyNotFoundException($"参数 {key} 不存在");
    }
}
#endregion

#region 支付策略
// 具体策略：信用卡支付
public class CreditCardStrategy : IPaymentStrategy
{
    public void ProcessPayment(double amount, IPaymentParameters parameters)
    {
        string cardNumber = parameters.GetParameter<string>("CardNumber");
        string securityCode = parameters.GetParameter<string>("SecurityCode");
        Console.WriteLine($"信用卡支付：金额={amount}, 卡号={cardNumber}, 安全码={securityCode}");
    }
}

// 具体策略：支付宝支付
public class AlipayStrategy : IPaymentStrategy
{
    public void ProcessPayment(double amount, IPaymentParameters parameters)
    {
        string phone = parameters.GetParameter<string>("PhoneNumber");
        Console.WriteLine($"支付宝支付：金额={amount}, 手机号={phone}");
    }
}
#endregion

#region 上下文类
// 上下文类
public class PaymentContext
{
    private IPaymentStrategy _strategy;

    public void SetStrategy(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecutePayment(double amount, IPaymentParameters parameters)
    {
        _strategy?.ProcessPayment(amount, parameters);
    }
}
#endregion


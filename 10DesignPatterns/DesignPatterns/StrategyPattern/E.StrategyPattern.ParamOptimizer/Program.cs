using System;


#region Client Code
var parameters = new PaymentParameters
{
    CardNumber = "1234-5678-9012-3456",
    SecurityCode = "123",
    PhoneNumber = "13812345678"
};

var context = new PaymentContext();

// 使用信用卡支付
context.SetStrategy(new CreditCardStrategy());
context.ExecutePayment(100.0, parameters);

// 切换为支付宝支付
context.SetStrategy(new AlipayStrategy());
context.ExecutePayment(200.0, parameters);
#endregion

#region 参数封装类
public class PaymentParameters
{
    public string CardNumber { get; set; }
    public string SecurityCode { get; set; }
    public string PhoneNumber { get; set; }
}
#endregion

#region 策略接口
public interface IPaymentStrategy
{
    void ProcessPayment(double amount, PaymentParameters parameters);
}
#endregion

#region 具体策略实现
#region 信用卡支付策略
public class CreditCardStrategy : IPaymentStrategy
{
    public void ProcessPayment(double amount, PaymentParameters parameters)
    {
        if (string.IsNullOrEmpty(parameters.CardNumber) 
            || string.IsNullOrEmpty(parameters.SecurityCode))
        {
            throw new ArgumentException("信用卡信息缺失");
        }
        Console.WriteLine($"信用卡支付：金额={amount}, 卡号={parameters.CardNumber}, 安全码={parameters.SecurityCode}");
    }
}
#endregion

#region 支付宝支付策略
public class AlipayStrategy : IPaymentStrategy
{
    public void ProcessPayment(double amount, PaymentParameters parameters)
    {
        if (string.IsNullOrEmpty(parameters.PhoneNumber))
        {
            throw new ArgumentException("手机号缺失");
        }
        Console.WriteLine($"支付宝支付：金额={amount}, 手机号={parameters.PhoneNumber}");
    }
}
#endregion
#endregion

#region 上下文类
public class PaymentContext
{
    private IPaymentStrategy _strategy;

    public void SetStrategy(IPaymentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecutePayment(double amount, PaymentParameters parameters)
    {
        _strategy?.ProcessPayment(amount, parameters);
    }
}
#endregion

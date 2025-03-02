using System;

#region 客户端代码

var order = new OrderProcessor(100m);

// 使用会员折扣策略
order.SetDiscountStrategy(new MemberDiscount());
Console.WriteLine($"Member Discount: {order.ProcessOrder()}");

// 使用节日折扣策略
order.SetDiscountStrategy(new HolidayDiscount());
Console.WriteLine($"Holiday Discount: {order.ProcessOrder()}");
#endregion

#region 策略接口
// 定义策略接口
public interface IDiscountStrategy
{
    decimal ApplyDiscount(decimal orderAmount);
}
#endregion

#region 具体策略类
// 具体策略类：会员折扣
public class MemberDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal orderAmount)
    {
        return orderAmount * 0.9m; // 10% 折扣
    }
}

// 具体策略类：节日折扣
public class HolidayDiscount : IDiscountStrategy
{
    public decimal ApplyDiscount(decimal orderAmount)
    {
        return orderAmount * 0.8m; // 20% 折扣
    }
}
#endregion

#region 上下文类
// 上下文类
public class OrderProcessor
{
    private IDiscountStrategy _discountStrategy;
    private decimal _orderAmount;

    public OrderProcessor(decimal orderAmount)
    {
        _orderAmount = orderAmount;
    }

    public void SetDiscountStrategy(IDiscountStrategy discountStrategy)
    {
        _discountStrategy = discountStrategy;
    }

    public decimal ProcessOrder()
    {
        return _discountStrategy.ApplyDiscount(_orderAmount);
    }
}
#endregion

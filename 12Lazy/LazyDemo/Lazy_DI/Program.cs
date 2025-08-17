using System;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    static void Main(string[] args)
    {
        // 设置依赖注入容器
        var services = new ServiceCollection();
        
        // 注册服务（注意顺序很重要）
        services.AddScoped<PaymentService>();
        services.AddScoped<OrderService>();
        
        // 使用Lazy打破循环依赖
        /* ----------------------------------------------------------
         * 使用 Lazy<OrderService> 作为“延迟代理”：
         *   • 注册到容器中的类型是 Lazy<OrderService>。
         *   • 只有在真正访问 Lazy<OrderService>.Value 时，
         *     才会触发容器再次解析 OrderService。
         *   • 这样即使 OrderService 间接依赖 PaymentService，
         *     而 PaymentService 又反向依赖 OrderService，
         *     也不会在构造函数阶段立即触发循环解析。
         * -------------------------------------------------------- */
        services.AddScoped( sp => 
            new Lazy<OrderService>(() => sp.GetRequiredService<OrderService>()));
        
        using var serviceProvider = services.BuildServiceProvider();
        
        // 模拟请求范围
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProvider = scope.ServiceProvider;
            
            Console.WriteLine("解析OrderService...");
            var orderService = scopedProvider.GetRequiredService<OrderService>();
            
            Console.WriteLine("\n调用OrderService处理订单:");
            orderService.ProcessOrder(100.50m);
        }
        
        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }
}

public class OrderService
{
    private readonly PaymentService _paymentService;
    
    // 正常依赖PaymentService
    public OrderService(PaymentService paymentService)
    {
        Console.WriteLine(">>> OrderService 已创建");
        _paymentService = paymentService;
    }
    
    public void ProcessOrder(decimal amount)
    {
        Console.WriteLine($"处理订单: ${amount}");
        _paymentService.ProcessPayment(amount);
        
        // 模拟其他操作
        Console.WriteLine("订单处理完成!");
    }
    
    public Order GetCurrentOrder() => new Order(DateTime.Now, 100.50m);
}

public class PaymentService
{
    private readonly Lazy<OrderService> _lazyOrderService;
    
    // 通过Lazy间接依赖OrderService
    public PaymentService(Lazy<OrderService> lazyOrderService)
    {
        Console.WriteLine(">>> PaymentService 已创建");
        _lazyOrderService = lazyOrderService;
    }
    
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"处理支付: ${amount}");
        
        // 按需访问OrderService（实际使用时才解析）
        Console.WriteLine("\n需要订单信息，访问Lazy.Value...");
        var currentOrder = _lazyOrderService.Value.GetCurrentOrder();
        
        Console.WriteLine($"获取到当前订单: {currentOrder}");
    }
}

public record Order(DateTime CreatedTime, decimal Amount)
{
    public override string ToString() => 
        $"[{CreatedTime:HH:mm:ss}] ${Amount}";
}
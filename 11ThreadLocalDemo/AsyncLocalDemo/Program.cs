using System;
using System.Threading;
using System.Threading.Tasks;

#region 定义请求上下文
public static class RequestContext
{
    private static readonly AsyncLocal<string> _requestId = new AsyncLocal<string>();
    
    public static string RequestId 
    {
        get => _requestId.Value;
        set => _requestId.Value = value;
    }
}
#endregion

#region 演示
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("开始模拟HTTP请求处理...\n");
        
        // 模拟并行处理两个请求
        var request1 = SimulateHttpRequest(1);
        var request2 = SimulateHttpRequest(2);
        
        await Task.WhenAll(request1, request2);
        
        Console.WriteLine("\n所有请求处理完成！");
    }

    static async Task SimulateHttpRequest(int requestNum)
    {
        // 设置请求上下文
        RequestContext.RequestId = $"REQ-{requestNum}-{Guid.NewGuid().ToString("N").Substring(0, 4)}";
        
        Console.WriteLine($"[请求 {requestNum}] 设置上下文ID: {RequestContext.RequestId}");
        
        await ValidateUserAsync(requestNum);
        await SaveLogAsync(requestNum);
    }

    static async Task ValidateUserAsync(int requestNum)
    {
        // 模拟异步操作
        await Task.Delay(new Random().Next(100, 300));
        
        Console.WriteLine($"[请求 {requestNum}] 验证用户 - 当前上下文ID: {RequestContext.RequestId}");
    }

    
    static async Task SaveLogAsync(int requestNum)
    {
        // 模拟异步操作
        await Task.Delay(new Random().Next(50, 200));
        
        var log = $"[请求 {requestNum}] 保存日志 - 请求ID: {RequestContext.RequestId} 时间: {DateTime.Now:HH:mm:ss.fff}";
        Console.WriteLine(log);
    }
}

#endregion
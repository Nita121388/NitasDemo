using System;
using System.Threading;
using System.Threading.Tasks;

public static class EventBus
{
    public static void Publish(string eventName, string value)
    {
        Console.WriteLine($"[事件触发] {eventName}: 新值 = '{value}'");
    }
}

public static class DbConfig
{
    private static readonly AsyncLocal<string> _connectionString = new AsyncLocal<string>(
        args => {
            if (args.CurrentValue != args.PreviousValue)
            {
                Console.WriteLine($"\n--- 连接字符串变更 ({GetThreadInfo()}) ---");
                Console.WriteLine($"旧值: '{args.PreviousValue ?? "null"}'");
                Console.WriteLine($"新值: '{args.CurrentValue ?? "null"}'");
                EventBus.Publish("CONN_STRING_CHANGED", args.CurrentValue);
            }
        }
    );

    public static string ConnectionString
    {
        get => _connectionString.Value;
        set => _connectionString.Value = value;
    }

    private static string GetThreadInfo()
    {
        return $"线程 #{Thread.CurrentThread.ManagedThreadId}";
    }
}

class Program
{
    static async Task Main()
    {
        Console.WriteLine("连接字符串变更演示");
        Console.WriteLine("初始线程: " + Thread.CurrentThread.ManagedThreadId);
        
        // 初始设置
        UpdateConfiguration("Server=default;Database=AppDB");
        
        // 同步变更演示
        Console.WriteLine("\n==== 同步变更测试 ====");
        UpdateConfiguration("Server=prod;Database=ProductionDB");
        UpdateConfiguration("Server=prod;Database=ProductionDB"); // 相同值测试
        UpdateConfiguration(null);
        
        // 异步变更演示
        Console.WriteLine("\n==== 异步变更测试 ====");
        await Task.Run(() => UpdateConfiguration("Server=async;Database=AsyncDB"));
        
        // 多线程变更演示
        Console.WriteLine("\n==== 多线程测试 ====");
        var thread1 = new Thread(() => UpdateConfiguration("Server=thread1;Database=ThreadDB"));
        var thread2 = new Thread(() => UpdateConfiguration("Server=thread2;Database=ThreadDB"));
        
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
        
        // 恢复主线程上下文
        Console.WriteLine("\n==== 主线程恢复测试 ====");
        UpdateConfiguration("Server=main;Database=MainDB");
        
        Console.WriteLine("\n按任意键退出...");
        Console.ReadKey();
    }

    static void UpdateConfiguration(string newConnectionString)
    {
        Console.WriteLine($"\n[更新配置] ({Thread.CurrentThread.ManagedThreadId}) 设置值: '{newConnectionString}'");
        DbConfig.ConnectionString = newConnectionString;
    }
    
    
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#region 模拟数据库连接类
public class MockConnection : IDisposable
{
    public Guid ConnectionId { get; } = Guid.NewGuid();
    public string State { get; private set; } = "Closed";
    private bool _disposed = false;
    
    public void Open()
    {
        if (_disposed) throw new ObjectDisposedException("MockConnection");
        State = "Open";
        Console.WriteLine($"模拟连接已打开: {ConnectionId} (线程 {Thread.CurrentThread.ManagedThreadId})");
    }
    
    public MockCommand CreateCommand()
    {
        if (_disposed) throw new ObjectDisposedException("MockConnection");
        return new MockCommand();
    }
    
    public void Close()
    {
        if (!_disposed)
        {
            State = "Closed";
            Console.WriteLine($"模拟连接已关闭: {ConnectionId} (线程 {Thread.CurrentThread.ManagedThreadId})");
        }
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            Close();
            _disposed = true;
        }
    }
}
#endregion

#region 模拟数据库命令类
public class MockCommand : IDisposable
{
    public string CommandText { get; set; }
    private bool _disposed = false;
    
    public object ExecuteScalar()
    {
        if (_disposed) throw new ObjectDisposedException("MockCommand");
        return $"模拟结果: {DateTime.Now:HH:mm:ss.fff} (线程 {Thread.CurrentThread.ManagedThreadId})";
    }
    
    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine($"释放命令资源 (线程 {Thread.CurrentThread.ManagedThreadId})");
            _disposed = true;
        }
    }
}
#endregion

#region 数据库上下文类 (ThreadLocal<T>)
class DatabaseContext
{
    private static readonly ThreadLocal<MockConnection> _threadConnection = new ThreadLocal<MockConnection>(
        () => 
        {
            var conn = new MockConnection();
            conn.Open();
            Console.WriteLine($"为线程 {Thread.CurrentThread.ManagedThreadId} 创建连接");
            return conn;
        },
        trackAllValues: true); // 启用 Values 属性支持

    public static IEnumerable<MockConnection> AllConnections => _threadConnection.Values;
    public static MockConnection CurrentConnection => _threadConnection.Value;

    public static void ReleaseResources()
    {
        _threadConnection.Dispose();
    }
}
#endregion

class Program
{
    static void Main()
    {
        Console.WriteLine("=== ThreadLocal<T> 演示 ===");
        Console.WriteLine("1. 懒加载初始化");
        Console.WriteLine("2. 线程隔离数据");
        Console.WriteLine("3. 显式资源释放");
        Console.WriteLine("4. 枚举所有线程数据\n");
        
        var tasks = new Task[3];
        
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                // 首次访问时初始化连接（懒加载）
                var conn = DatabaseContext.CurrentConnection;
                
                // 模拟数据库查询
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT 模拟数据";
                    var result = cmd.ExecuteScalar();
                    Console.WriteLine($"线程 {Thread.CurrentThread.ManagedThreadId} 结果: {result}");
                }
            });
        }

        Task.WaitAll(tasks);
        
        // 输出所有线程的连接信息
        Console.WriteLine("\n所有活动连接:");
        foreach (var conn in DatabaseContext.AllConnections)
        {
            Console.WriteLine($"线程 {Thread.CurrentThread.ManagedThreadId} 的连接: {conn.ConnectionId} - 状态: {conn.State}");
        }

        // 显式释放资源
        Console.WriteLine("\n释放资源...");
        DatabaseContext.ReleaseResources();
        
        Console.WriteLine("\n释放后尝试访问:");
        try
        {
            Console.WriteLine(DatabaseContext.CurrentConnection.State);
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("已正确抛出 ObjectDisposedException");
        }
        
        Console.WriteLine("\n演示结束");
    }
}
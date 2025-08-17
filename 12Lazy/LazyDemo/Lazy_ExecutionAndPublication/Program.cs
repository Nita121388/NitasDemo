using System;
using System.Threading;
using System.Threading.Tasks;

#region 模拟配置类 Configuration

public class Configuration
{
    public string Environment { get; set; }
    public int MaxConnections { get; set; }
    public DateTime LoadTime { get; set; }

    public override string ToString() => 
        $"[{Environment}] MaxConnections={MaxConnections}, LoadTime={LoadTime:HH:mm:ss.fff}";
}


#endregion 模拟配置类

#region 配置服务使用线程安全的Lazy初始化
public class AppConfigService
{
    private static int _loadCounter = 0;  // 用于跟踪实际加载次数
    
    private static readonly Lazy<Configuration> _config = 
        new Lazy<Configuration>(() => 
        {
            Interlocked.Increment(ref _loadCounter);//记录实际初始化次数
            Console.WriteLine($">>> [线程 {Thread.CurrentThread.ManagedThreadId}] 开始加载配置...");
            Thread.Sleep(2000);  // 模拟数据库/IO延迟
            
            return new Configuration {
                Environment = "Production",
                MaxConnections = 100,
                LoadTime = DateTime.Now
            };
        }, LazyThreadSafetyMode.ExecutionAndPublication);

    public static Configuration Config => _config.Value; 
    public static int LoadCount => _loadCounter;
}

#endregion

#region Usage

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== 配置加载测试（线程安全模式）===");
        Console.WriteLine($"主线程ID: {Thread.CurrentThread.ManagedThreadId}\n");

        // 创建10个并发请求线程
        Parallel.For(0, 10, i => {
            Thread.Sleep(new Random().Next(50));  // 随机延迟增加并发冲突概率
            
            Console.WriteLine($"[线程 {Thread.CurrentThread.ManagedThreadId}] 请求配置...");
            var config = AppConfigService.Config;
            
            Console.WriteLine($"[线程 {Thread.CurrentThread.ManagedThreadId}] 获取配置: {config}");
        });

        Console.WriteLine($"\n实际加载次数: {AppConfigService.LoadCount}");
        Console.WriteLine("测试完成。按任意键退出...");
        Console.ReadKey();
    }
}

#endregion
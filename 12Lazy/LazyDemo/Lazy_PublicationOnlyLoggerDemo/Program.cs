using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PublicationOnlyLoggerDemo
{
    // 日志接口
    public interface ILogger
    {
        void Log(string message);
    }

    // 线程安全的文件日志记录器
    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        
        // 静态锁确保多实例写入时的线程安全
        private static readonly object _fileLock = new object();
        
        // 记录已创建实例数量（用于演示）
        public static int InstanceCount = 0;
        
        // 记录实际写入次数（用于演示）
        public static readonly ConcurrentBag<string> AllLogs = new ConcurrentBag<string>();

        public FileLogger(string filePath)
        {
            
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            _filePath = filePath;
            Interlocked.Increment(ref InstanceCount);

            lock (_fileLock)
            {
                // 初始化日志文件
                File.WriteAllText(filePath, $"Log initialized at {DateTime.Now:HH:mm:ss.fff}\n");
            }

        }

        public void Log(string message)
        {
            lock (_fileLock)
            {
                File.AppendAllText(_filePath, $"{DateTime.Now:HH:mm:ss.fff} - {message}\n");
            }
            AllLogs.Add(message);
        }
    }

    // 日志工厂（使用PublicationOnly模式）
    public static class LoggerFactory
    {
        private static readonly Lazy<ILogger> _logger = 
            new Lazy<ILogger>(
                () => new FileLogger("app.log"), 
                LazyThreadSafetyMode.PublicationOnly
            );

        public static ILogger GetLogger() => _logger.Value;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===开启并行日志记录测试===");
            
            // 并行日志记录测试
            Parallel.For(0, 10, i => 
            {
                var logger = LoggerFactory.GetLogger();
                logger.Log($"Task {i} 开启");
                Thread.Sleep(50); // 模拟工作负载
                logger.Log($"Task {i} 完成");
            });

            // 显示统计结果
            Console.WriteLine("\n测试结果:");
            Console.WriteLine($"日志实例创建个数: {FileLogger.InstanceCount}");
            Console.WriteLine($"总计写入日志条目：{FileLogger.AllLogs.Count}");
            Console.WriteLine($"首次使用的日志实例：{FileLogger.AllLogs.First()}");
            Console.WriteLine($"最后使用的日志实例：{FileLogger.AllLogs.Last()}");

            Console.WriteLine("\n日志文件内容：");
            Console.WriteLine("-----------------");
            Console.WriteLine(File.ReadAllText("app.log"));
            
            Console.WriteLine("\n按任意键退出...");
            Console.ReadKey();
        }
    }
}
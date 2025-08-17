using System;
using System.Collections.Generic;
using System.Threading;

class ThreadStaticDemo
{
    // 使用 [ThreadStatic] 声明线程独立的日志列表
    [ThreadStatic]
    private static List<string>? _perThreadLogs;

    // 普通静态变量（用于对比）
    private static int _sharedCounter = 0;

    public static void Main()
    {
        // 启动3个线程
        var threads = new List<Thread>();
        for (int i = 1; i <= 3; i++)
        {
            int threadId = i;
            var t = new Thread(() => WorkerThread(threadId));
            threads.Add(t);
            t.Start();
        }

        // 等待所有线程完成
        threads.ForEach(t => t.Join());
        
        Console.WriteLine("\n所有线程执行完毕");
        Console.WriteLine($"最终共享计数器值: {_sharedCounter}");
    }

    private static void WorkerThread(int threadId)
    {
        _perThreadLogs = new List<string>();
        
        // 添加线程启动日志
        AddLog($"线程 {threadId} 启动");

        // 模拟工作
        for (int i = 0; i < 3; i++)
        {
            // 操作共享资源（需要同步）
            Interlocked.Increment(ref _sharedCounter);
            AddLog($"执行操作{i + 1}, 共享计数器={_sharedCounter}");
            Thread.Sleep(100); // 模拟耗时
        }

        // 打印当前线程的专属日志
        Console.WriteLine($"\n线程 {threadId} 的日志:");
        foreach (var log in _perThreadLogs)
        {
            Console.WriteLine($"  {log}");
        }
    }

    private static void AddLog(string message)
    {
        // 安全访问线程专属列表
        if (_perThreadLogs == null)
            _perThreadLogs = new List<string>();
        
        _perThreadLogs.Add($"{DateTime.Now:HH:mm:ss.fff} {message}");
    }
}
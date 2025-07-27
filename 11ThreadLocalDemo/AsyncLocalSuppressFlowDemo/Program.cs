class Program
{
    static AsyncLocal<string> _authority = new AsyncLocal<string>();

    static async Task Main(string[] args)
    {
        _authority.Value = "Admin";
        Console.WriteLine("===== ExecutionContext.SuppressFlow() 演示 =====");
        // 场景1: 正常上下文流动
        Console.WriteLine("\n[场景1] 正常上下文流动:");
        await PrintAsync();

        
        // 场景2: 抑制上下文流动（使用using确保正确恢复）
        Console.WriteLine("\n[场景2] 抑制上下文流动:");
        using (ExecutionContext.SuppressFlow()) // 使用using自动管理恢复
        {
            // 同步等待确保抑制/恢复在同一线程
            PrintAsync().GetAwaiter().GetResult();
        }

        // 场景3: 恢复后的正常流动
        Console.WriteLine("\n[场景3] 恢复上下文流动:");
        await PrintAsync();

        Console.Read();

    }

    static async ValueTask PrintAsync()
    {
        new Thread(() =>
        {
            Console.WriteLine($"new Thread,Current Thread Id: {Thread.CurrentThread.ManagedThreadId},Current Authority: {_authority.Value}");
        })
        {
            IsBackground = true
        }.Start();

        Thread.Sleep(100); // 保证输出顺序

        ThreadPool.QueueUserWorkItem(_ =>
        {
            Console.WriteLine($"ThreadPool.QueueUserWorkItem,Current Thread Id: {Thread.CurrentThread.ManagedThreadId},Current Authority: {_authority.Value}");
        });

        Thread.Sleep(100);

        Task.Run(() =>
        {
            Console.WriteLine($"ask.Run,Current Thread Id: {Thread.CurrentThread.ManagedThreadId},Current Authority: {_authority.Value}");
        });

        await Task.Delay(100);
        
        Console.WriteLine($"after await,Current Thread Id: {Thread.CurrentThread.ManagedThreadId},Current Authority: {_authority.Value}");

        Console.WriteLine();
    }
    
}
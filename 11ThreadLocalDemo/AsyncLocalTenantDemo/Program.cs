using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== 租户上下文异步传递演示 ===");
        
        // 演示1：基础场景
        Console.WriteLine("\n[演示1] 主线程设置租户上下文");
        TenantContext.Current = "Tenant_A";
        Console.WriteLine($"主线程租户: {TenantContext.Current}");
        await StartBackgroundJob();

        // 演示2：切换租户场景
        Console.WriteLine("\n[演示2] 任务中切换租户上下文");
        TenantContext.Current = "Tenant_B";
        await StartBackgroundJob();
        
        // 演示3：嵌套任务场景
        Console.WriteLine("\n[演示3] 嵌套任务上下文传递");
        TenantContext.Current = "Tenant_C";
        await StartNestedTasks();
        
        // 演示4：未设置租户的场景
        Console.WriteLine("\n[演示4] 未设置租户上下文");
        TenantContext.Current = null;
        await StartBackgroundJob();
    }

    static async Task StartBackgroundJob()
    {
        Console.WriteLine($"启动后台任务 - 当前租户: {TenantContext.Current ?? "<null>"}");
        
        var task = Task.Run(() => {
            Console.WriteLine($"后台任务执行中 - 租户: {TenantContext.Current ?? "<null>"}");
            Thread.Sleep(200); // 模拟工作
        });

        await task;
    }

    static async Task StartNestedTasks()
    {
        Console.WriteLine($"主嵌套任务 - 租户: {TenantContext.Current}");
        
        var parentTask = Task.Run(async () => {
            Console.WriteLine($"父任务 - 租户: {TenantContext.Current}");
            
            await Task.Delay(100);
            var childTask = Task.Run(() => {
                Console.WriteLine($"子任务 - 租户: {TenantContext.Current}");
            });
            
            await childTask;
        });

        await parentTask;
    }
}

public static class TenantContext
{
    private static readonly AsyncLocal<string> _tenant = new AsyncLocal<string>();
    
    public static string Current {
        get => _tenant.Value;
        set => _tenant.Value = value;
    }
}
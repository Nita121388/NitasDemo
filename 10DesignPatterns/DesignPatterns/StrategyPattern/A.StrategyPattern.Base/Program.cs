
#region Client Code
// 创建具体策略
IStrategy addStrategy = new AddStrategy();
IStrategy subtractStrategy = new SubtractStrategy();
IStrategy multiplyStrategy = new MultiplyStrategy();

// 设置初始策略
Context context = new Context(addStrategy);
context.ExecuteStrategy(10, 5);

// 更改策略
context.SetStrategy(subtractStrategy);
context.ExecuteStrategy(10, 5);

// 更改策略
context.SetStrategy(multiplyStrategy);
context.ExecuteStrategy(10, 5);
#endregion

#region Interface 
public interface IStrategy
{
    void Execute(int a, int b);
}
#endregion

#region Concrete Strategies
public class AddStrategy : IStrategy
{
    public void Execute(int a, int b)
    {
        Console.WriteLine($"Add: {a} + {b} = {a + b}");
    }
}

public class SubtractStrategy : IStrategy
{
    public void Execute(int a, int b)
    {
        Console.WriteLine($"Subtract: {a} - {b} = {a - b}");
    }
}

public class MultiplyStrategy : IStrategy
{
    public void Execute(int a, int b)
    {
        Console.WriteLine($"Multiply: {a} * {b} = {a * b}");
    }
}
#endregion

#region  Context

public class Context
{
    private IStrategy _strategy;

    public Context(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecuteStrategy(int a, int b)
    {
        _strategy.Execute(a, b);
    }
}

#endregion


#region - Client 适用示例
// 使用示例
var portfolio = new Portfolio();
var manager = new PortfolioManager();

// 使用单一策略
manager.SetStrategy(new RiskAssessmentStrategy());
manager.ExecuteStrategy(portfolio);

// 使用复合策略
var composite = new CompositeStrategy();
composite.AddStrategy(new RiskAssessmentStrategy());
composite.AddStrategy(new TaxOptimizationStrategy());
composite.AddStrategy(new LiquidityAnalysisStrategy()); // 新增策略

manager.SetStrategy(composite);
manager.ExecuteStrategy(portfolio);
#endregion

#region - 策略接口（抽象策略）
public interface IInvestmentStrategy
{
    void Execute(Portfolio portfolio);
}
#endregion

#region - 抽象策略基类（继承复用）
public abstract class CommonStrategy : IInvestmentStrategy
{
    // 公共验证方法
    protected void ValidatePortfolio(Portfolio portfolio)
    {
        if (portfolio.TotalValue <= 0)
            throw new ArgumentException("Invalid portfolio value");
    }

    // 抽象方法要求子类必须实现
    public abstract void Execute(Portfolio portfolio);
}
#endregion

#region - 具体策略1：风险评估策略（继承复用公共逻辑）
public class RiskAssessmentStrategy : CommonStrategy
{
    public override void Execute(Portfolio portfolio)
    {
        ValidatePortfolio(portfolio); // 复用基类验证
        
        // 具体风险计算逻辑
        portfolio.RiskLevel = CalculateRisk(portfolio.Assets);
        Console.WriteLine($"风险评估完成，当前风险等级：{portfolio.RiskLevel}");
    }

    private RiskLevel CalculateRisk(IEnumerable<Asset> assets)
    {
        // 实际风险计算逻辑
        return RiskLevel.Moderate;
    }
}
#endregion

#region - 具体策略2：税务优化策略（继承复用公共逻辑）
public class TaxOptimizationStrategy : CommonStrategy
{
    public override void Execute(Portfolio portfolio)
    {
        ValidatePortfolio(portfolio); // 复用基类验证
        
        // 具体税务优化逻辑
        portfolio.TaxBurden = OptimizeTax(portfolio.Investments);
        Console.WriteLine($"税务优化完成，税务负担减少：{portfolio.TaxBurden}%");
    }

    private decimal OptimizeTax(IEnumerable<Investment> investments)
    {
        // 实际税务优化逻辑
        return 15.5m;
    }
}
#endregion

#region - 具体策略3：流动性分析策略（继承复用公共逻辑）
public class LiquidityAnalysisStrategy : CommonStrategy
{
    public override void Execute(Portfolio portfolio)
    {
        ValidatePortfolio(portfolio); // 复用基类验证
        
        // 具体流动性分析逻辑
        portfolio.LiquidityScore = CalculateLiquidity(portfolio.Assets);
        Console.WriteLine($"流动性分析完成，流动性得分：{portfolio.LiquidityScore}");
    }

    private decimal CalculateLiquidity(IEnumerable<Asset> assets)
    {
        // 示例流动性计算逻辑：假设流动性得分基于资产价值的加权平均
        decimal totalValue = assets.Sum(a => a.Value);
        decimal liquidityScore = assets.Sum(a => a.Value * a.LiquidityFactor) / totalValue;
        return liquidityScore;
    }
}
#endregion

#region - 复合策略（策略扩展）
public class CompositeStrategy : IInvestmentStrategy
{
    private readonly List<IInvestmentStrategy> _strategies = new();

    public void AddStrategy(IInvestmentStrategy strategy)
    {
        _strategies.Add(strategy);
    }

    public void Execute(Portfolio portfolio)
    {
        foreach (var strategy in _strategies)
        {
            strategy.Execute(portfolio);
        }
    }
}
#endregion

#region - 上下文类
public class PortfolioManager
{
    private IInvestmentStrategy _strategy;

    public void SetStrategy(IInvestmentStrategy strategy)
    {
        _strategy = strategy;
    }

    public void ExecuteStrategy(Portfolio portfolio)
    {
        _strategy?.Execute(portfolio);
    }
}
#endregion

#region - 辅助类
#region - 资产类
public class Asset
{
    public string Name { get; set; }
    public decimal Value { get; set; }
    public decimal LiquidityFactor { get; set; } // 流动性因子

    public Asset()
    {
        // 默认流动性因子
        LiquidityFactor = 1.0m;
    }
}
#endregion

// 投资类
public class Investment
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
}

// 风险等级枚举
public enum RiskLevel
{
    Low,
    Moderate,
    High
}
#endregion

#region - Portfolio类
public class Portfolio
{
    public List<Asset> Assets { get; set; } = new List<Asset>();
    public List<Investment> Investments { get; set; } = new List<Investment>();
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Moderate;
    public decimal TaxBurden { get; set; } = 0;
    public decimal LiquidityScore { get; set; } = 0; // 流动性得分
    public decimal TotalValue => Assets.Sum(a => a.Value);

    public Portfolio()
    {
        // 初始化示例数据
        Assets.Add(new Asset { Name = "Stock A", Value = 1000, LiquidityFactor = 0.9m });
        Assets.Add(new Asset { Name = "Bond B", Value = 500, LiquidityFactor = 0.8m });
        Investments.Add(new Investment { Name = "Investment 1", Amount = 200 });
        Investments.Add(new Investment { Name = "Investment 2", Amount = 300 });
    }
}
#endregion

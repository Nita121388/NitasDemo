
#region Client Code
var logger = new Logger();

// 使用单例的控制台输出策略
logger.SetLogStrategy(ConsoleLogStrategy.Instance);
logger.Log("This is a log message.");

// 使用对象池的文件输出策略
var fileLogStrategy = FileLogStrategyPool.Acquire();
logger.SetLogStrategy(fileLogStrategy);
logger.Log("This is a file log message.");
FileLogStrategyPool.Release(fileLogStrategy);

#endregion

    #region Strategy Interface and Implementations
    // 定义策略接口
    public interface ILogStrategy
    {
        void Log(string message);
    }

    // 具体策略类：控制台输出（轻量级）
    public class ConsoleLogStrategy : ILogStrategy
    {
        private static readonly ConsoleLogStrategy _instance = new ConsoleLogStrategy();

        private ConsoleLogStrategy() { }

        public static ConsoleLogStrategy Instance => _instance;

        public void Log(string message)
        {
            Console.WriteLine($"Console Log: {message}");
        }
    }

    // 具体策略类：文件输出（重型）
    public class FileLogStrategy : ILogStrategy
    {
        public void Log(string message)
        {
            Console.WriteLine($"File Log: {message}");
        }
    }
    #endregion

    #region Object Pool for FileLogStrategy
    // 对象池管理重型策略
    public class FileLogStrategyPool
    {
        private static readonly Queue<FileLogStrategy> _pool = new Queue<FileLogStrategy>();

        static FileLogStrategyPool()
        {
            for (int i = 0; i < 5; i++)
            {
                _pool.Enqueue(new FileLogStrategy());
            }
        }

        public static FileLogStrategy Acquire()
        {
            if (_pool.Count > 0)
            {
                return _pool.Dequeue();
            }
            return new FileLogStrategy();
        }

        public static void Release(FileLogStrategy strategy)
        {
            _pool.Enqueue(strategy);
        }
    }
    #endregion

    #region Context Class
    // 上下文类
    public class Logger
    {
        private ILogStrategy _logStrategy;

        public void SetLogStrategy(ILogStrategy logStrategy)
        {
            _logStrategy = logStrategy;
        }

        public void Log(string message)
        {
            _logStrategy.Log(message);
        }
    }
    #endregion

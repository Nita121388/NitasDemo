
	#region 客户端代码

     var processor = new ImageProcessor();

     // 使用共享的黑白滤镜策略对象
     processor.SetFilterStrategy(FilterFactory.GetFilter("BlackAndWhite"));
     processor.ProcessImage("image1.jpg");

     // 使用共享的模糊滤镜策略对象
     processor.SetFilterStrategy(FilterFactory.GetFilter("Blur"));
     processor.ProcessImage("image2.jpg");

    #endregion
	
    #region 策略接口和具体策略类

    // 定义策略接口
    public interface IFilterStrategy
    {
        void ApplyFilter(string image);
    }

    // 具体策略类：黑白滤镜
    public class BlackAndWhiteFilter : IFilterStrategy
    {
        public void ApplyFilter(string image)
        {
            Console.WriteLine($"Applying Black and White filter to {image}");
        }
    }

    // 具体策略类：模糊滤镜
    public class BlurFilter : IFilterStrategy
    {
        public void ApplyFilter(string image)
        {
            Console.WriteLine($"Applying Blur filter to {image}");
        }
    }

    #endregion

    #region 策略工厂类（Flyweight模式）

    // 策略工厂类（Flyweight 模式）
    public class FilterFactory
    {
        private static readonly Dictionary<string, IFilterStrategy> _filters = new Dictionary<string, IFilterStrategy>();

        public static IFilterStrategy GetFilter(string filterType)
        {
            if (!_filters.ContainsKey(filterType))
            {
                switch (filterType)
                {
                    case "BlackAndWhite":
                        _filters[filterType] = new BlackAndWhiteFilter();
                        break;
                    case "Blur":
                        _filters[filterType] = new BlurFilter();
                        break;
                    default:
                        throw new ArgumentException("Invalid filter type");
                }
            }
            return _filters[filterType];
        }
    }

    #endregion

    #region 上下文类

    // 上下文类
    public class ImageProcessor
    {
        private IFilterStrategy _filterStrategy;

        public void SetFilterStrategy(IFilterStrategy filterStrategy)
        {
            _filterStrategy = filterStrategy;
        }

        public void ProcessImage(string image)
        {
            _filterStrategy.ApplyFilter(image);
        }
    }

    #endregion


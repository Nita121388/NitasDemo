using Microsoft.EntityFrameworkCore;
using WPFArchitectureDemo.Data.DatabaseContexts;
using WPFArchitectureDemo.Data.IRepository;
using WPFArchitectureDemo.Data.Managers;

namespace WPFArchitectureDemo.Data.RepositoryFactory
{
    public class RepositoryFactory
    {
        private Action<DbContextOptionsBuilder> _configureDbContext;
        public RepositoryFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public IPromptRepository GetPromptManager()
        {
            return new PromptRepository(new DbContextFactory(_configureDbContext));
        }

        public IPromptUsageRepository GetPromptUsageManager()
        {
            return new PromptUsageRepository(new DbContextFactory(_configureDbContext));
        }
    }
}

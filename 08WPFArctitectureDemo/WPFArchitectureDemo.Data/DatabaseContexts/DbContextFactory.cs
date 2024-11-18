using Microsoft.EntityFrameworkCore;

namespace WPFArchitectureDemo.Data.DatabaseContexts
{
    public class DbContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDbContext;

        public DbContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
        {
            _configureDbContext = configureDbContext;
        }

        public PromptDbContext CreateDbContext()
        {
            DbContextOptionsBuilder<PromptDbContext> options = new DbContextOptionsBuilder<PromptDbContext>();

            _configureDbContext(options);

            return new PromptDbContext(options.Options);
        }
    }
}

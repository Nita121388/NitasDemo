using Microsoft.EntityFrameworkCore;
using WPFArchitectureDemo.Data.DatabaseContexts;
using WPFArchitectureDemo.Data.IRepository;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Data.Managers
{
    public class PromptUsageRepository : IPromptUsageRepository
    {
        private readonly DbContextFactory _contextFactory;

        public PromptUsageRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public PromptUsage Add(PromptUsage entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var promptUsageToAdd = new PromptUsage
                {
                    PromptID = entity.PromptID, 
                    CreateDateTime = DateTime.Now,
                };
                context.Add(promptUsageToAdd);
                context.SaveChanges();
                return context.PromptUsages.Find(promptUsageToAdd.ID); 
            }
        }

        public void Delete(long id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var promptUsage = context.PromptUsages.Find(id);
                if (promptUsage != null)
                {
                    context.PromptUsages.Remove(promptUsage);
                    context.SaveChanges();
                }
            }
        }

        public List<PromptUsage> Get()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.PromptUsages.ToList();
            }
        }

        public PromptUsage Get(long id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.PromptUsages.Find(id);
            }
        }

        public List<PromptUsage> GetByForeignKey(long id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.PromptUsages.Where(pu => pu.PromptID == id).ToList();
            }
        }

        public List<PromptUsage> Select(string filterSql, string orderBySql)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                // 构建基础SQL查询
                var sqlQuery = "SELECT * FROM PromptUsages";

                // 添加过滤条件
                if (!string.IsNullOrWhiteSpace(filterSql))
                {
                    sqlQuery += $" WHERE {filterSql}";
                }

                // 添加排序条件
                if (!string.IsNullOrWhiteSpace(orderBySql))
                {
                    sqlQuery += $" ORDER BY {orderBySql}";
                }

                // 使用FromSqlRaw方法执行自定义SQL查询
                var query = context.PromptUsages.FromSqlRaw(sqlQuery);
                return query.ToList();
            }
        }

        public void Update(PromptUsage entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var existingPromptUsage = context.PromptUsages.Find(entity.ID);
                if (existingPromptUsage != null)
                {
                    existingPromptUsage.PromptID = entity.PromptID;
                    context.SaveChanges();
                }
            }
        }
    }
}

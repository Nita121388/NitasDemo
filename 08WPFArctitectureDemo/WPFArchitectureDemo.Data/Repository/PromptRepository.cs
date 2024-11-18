using Microsoft.EntityFrameworkCore;
using WPFArchitectureDemo.Data.DatabaseContexts;
using WPFArchitectureDemo.Data.IRepository;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Data.Managers
{
    public class PromptRepository : IPromptRepository
    {
        private readonly DbContextFactory _contextFactory;

        public PromptRepository(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public Prompt Add(Prompt entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var prompt = new Prompt 
                { 
                    Title = entity.Title, 
                    Content = entity.Content,
                    CreateDateTime = DateTime.Now ,
                    UpdateDateTime = DateTime.Now 
                };
                prompt = context.Prompts.Add(prompt).Entity;
                context.SaveChanges();
                return prompt;
            }
        }

        public void Delete(long id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var prompt = context.Prompts.Find(id);
                if (prompt != null)
                {
                    prompt.IsDelete = true;
                    context.SaveChanges();
                }
            }
        }

        public List<Prompt> Get()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.Prompts.ToList();
            }
        }

        public Prompt Get(long id)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return context.Prompts.Find(id);
            }
        }

        public List<Prompt> Select(string filterSql, string orderBySql)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                // 构建基础SQL查询
                var sqlQuery = "SELECT * FROM Prompts";

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
                var query = context.Prompts.FromSqlRaw(sqlQuery);
                return query.ToList();
            }
        }

        public void Update(Prompt entity)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var prompt = context.Prompts.Find(entity.ID);
                if (prompt!= null)
                {
                    prompt.Title = entity.Title;
                    prompt.Content = entity.Content;
                    prompt.UpdateDateTime = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        List<Prompt> IRepository<Prompt>.GetByForeignKey(long id)
        {
            throw new NotImplementedException();
        }
    }
}

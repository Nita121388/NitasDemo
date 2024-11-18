using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WPFArchitectureDemo.Domain.Models;

namespace WPFArchitectureDemo.Data.DatabaseContexts
{
    public class PromptDbContext : DbContext
    {
        public DbSet<Prompt> Prompts { get; set; }
        public DbSet<PromptUsage> PromptUsages { get; set; }

        public PromptDbContext(DbContextOptions options) : base(options)
        { } //设计时不能包含此行内容,才能正确生成迁移文件
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             // 配置 Prompt 实体
             modelBuilder.Entity<Prompt>(entity =>
             {
                 entity.HasKey(e => e.ID);
                 entity.HasMany(e => e.PromptUsages)
                     .WithOne(u => u.Prompt)
                     .HasForeignKey(u => u.PromptID);
             });

             // 配置 PromptUsage 实体
             modelBuilder.Entity<PromptUsage>(entity =>
             {
                 entity.HasKey(e => e.ID);
             });
         }
    }
}

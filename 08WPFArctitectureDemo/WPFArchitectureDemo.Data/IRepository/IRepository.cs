namespace WPFArchitectureDemo.Data.IRepository
{
    public interface IRepository<T>
    {
        T Add(T entity);
        void Delete(long id);
        void Update(T entity);
        List<T> Get();
        List<T> Select(string filterSql,string orderBySql);
        T Get(long id);
        List<T> GetByForeignKey(long id);
    }
}

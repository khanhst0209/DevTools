namespace DevTools.Repositories.Interfaces
{
    public interface IBaseRepository<T, Tkey> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Tkey id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(Tkey id);
    }

}
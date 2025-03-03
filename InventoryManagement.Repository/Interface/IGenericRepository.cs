using System.Linq.Expressions;

namespace InventoryManagement.Repository.Interface
{
    public interface IGenericRepository<T> where T :class
    {
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> GetByIdAsync(int Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(int Id, params Expression<Func<T, object>>[] includes);
    }
}

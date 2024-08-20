using System.Linq.Expressions;

namespace B1TestTask.Domain.Repositories
{
    public interface IRepositoryBase<T>
    {
        #region default CRUD

        Task<T?> CreateAsync(T entity, CancellationToken token = default);
        Task<T?> GetAsync(int id, CancellationToken token = default);
        Task<T?> UpdateAsync(T entity, CancellationToken token = default);
        Task DeleteAsync(T entity, CancellationToken token = default);

        #endregion

        Task<IEnumerable<T>> GetByPredicateAsync(Expression<Func<T, bool>> predicate, CancellationToken token = default);
    }
}

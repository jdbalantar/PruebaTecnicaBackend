using Application.DTOs.Persistence;
using Domain.Interfaces;
using System.Linq.Expressions;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface IBaseRepository<T> where T : class, IBaseEntity
    {
        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PaginationResult<T>> GetAllWithPaginationAsync(PaginationRequestModel pagination, CancellationToken cancellationToken = default);
        Task<PaginationResult<T>> GetAllWithPaginationWithFilterAsync(PaginationRequestModel pagination, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T?> FindAsyncWithTracking(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        IQueryable<T> FindAll(Expression<Func<T, bool>> filter = null!, Expression<Func<T, object>> orderByDesc = null!, int? skip = null, int? take = null, IList<string> includes = null!);
        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> filter = null!, Expression<Func<T, object>> orderByDesc = null!, int? skip = null, int? take = null, IList<string> includes = null!, CancellationToken cancellationToken = default);
        IQueryable<T> FromSqlRaw(string query, params object[] parameters);
        Task<ICollection<T>> FromSqlRawAsync(string query, CancellationToken cancellationToken = default, params object[] parameters);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
        void DeleteRange(ICollection<T> entities);
        Task<bool> DeleteWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> Exists(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null!, IList<string> includes = null!, CancellationToken cancellationToken = default);
        void Attach(T entity);
        void AttachRange(ICollection<T> entities);
        void Detach(T entity);
        void SetUnchanged(T entity);

    }
}

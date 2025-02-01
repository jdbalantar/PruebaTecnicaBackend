using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Infrastructure.DbContext;
using Application.Interfaces.Infrastructure.Repositories;
using Application.DTOs.Persistence;

namespace Infrastructure.Repositories
{
    public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class, IBaseEntity
    {
        protected readonly DbSet<T> _DbSet = context!.Set<T>();
        protected readonly ApplicationDbContext _Context = context;

        #region GetAll
        public IQueryable<T> GetAll()
        {
            return _DbSet.AsQueryable().AsNoTracking().OrderBy(x => x.Id);
        }

        public virtual async Task<ICollection<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await GetAll().ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<PaginationResult<T>> GetAllWithPaginationAsync(PaginationRequestModel pagination, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _DbSet!;
            var result = await GetQueryWithPaginationWithFilterAsync(query, pagination, cancellationToken);
            return new PaginationResult<T>(result.PaginaActual, result.Paginas, result.CantidadRegistros, await result.Data.ToListAsync(cancellationToken: cancellationToken));
        }
        public async Task<PaginationResult<T>> GetAllWithPaginationWithFilterAsync(PaginationRequestModel pagination, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _DbSet!;
            var result = await GetPaginationAsync(query, pagination, cancellationToken);
            return new PaginationResult<T>(result.PaginaActual, result.Paginas, result.CantidadRegistros, await result.Data.ToListAsync(cancellationToken: cancellationToken));
        }

        public virtual IQueryable<T> FindAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderByDesc = null, int? skip = null, int? take = null, IList<string>? includes = null)
        {
            IQueryable<T> query = _DbSet;

            if (includes != null && includes.Count > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            if (orderByDesc != null)
                query = query.OrderByDescending(orderByDesc);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query.AsQueryable().AsNoTracking();
        }

        public virtual async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? orderByDesc = null, int? skip = null, int? take = null, IList<string>? includes = null, CancellationToken cancellationToken = default)
        {
            return await FindAll(filter, orderByDesc, skip, take, includes).ToListAsync(cancellationToken);
        }

        #endregion

        #region GetSingle

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _DbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
            if (entity != null)
                _DbSet.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public virtual async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _DbSet.AsQueryable().AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken)!;
        }

        public virtual async Task<T?> FindAsyncWithTracking(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _DbSet.AsQueryable().FirstOrDefaultAsync(predicate, cancellationToken: cancellationToken)!;
        }

        #endregion

        #region FromSql

        public virtual IQueryable<T> FromSqlRaw(string query, params object[] parameters)
        {
            return _DbSet.FromSqlRaw(query, parameters).AsQueryable().AsNoTracking();
        }

        public virtual async Task<ICollection<T>> FromSqlRawAsync(string query, CancellationToken cancellationToken = default, params object[] parameters)
        {
            return await _DbSet.FromSqlRaw(query, parameters).AsQueryable().AsNoTracking().ToListAsync(cancellationToken);
        }

        #endregion

        #region Add

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (_Context?.Entry(entity).State == EntityState.Added)
                Attach(entity);
            await _DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                if (_Context?.Entry(entity).State == EntityState.Added)
                {
                    _DbSet.Attach(entity);
                }
            }
            await _DbSet.AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        #region Update

        public virtual void Update(T entity)
        {
            _DbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _DbSet.UpdateRange(entities);
        }

        public virtual async Task<bool> UpdateRangeAsync(Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>? setPropertyCalls = null, Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            if (setPropertyCalls != null)
            {
                if (predicate != null)
                    return await _DbSet.Where(predicate).ExecuteUpdateAsync(setPropertyCalls, cancellationToken) > 0;
                return await _DbSet.ExecuteUpdateAsync(setPropertyCalls, cancellationToken: cancellationToken) > 0;
            }
            return false;
        }

        #endregion

        #region Delete

        public virtual void Delete(T entity)
        {
            _DbSet.Remove(entity);
        }

        public virtual async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            T? entity = await _DbSet.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

            if (entity != null)
                _DbSet.Remove(entity);
        }

        public virtual void DeleteRange(ICollection<T> entities)
        {
            _DbSet.RemoveRange(entities);
        }

        public virtual async Task<bool> DeleteWhereAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _DbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        #endregion

        #region Extras

        public virtual void SetUnchanged(T entity)
        {
            _DbSet.Entry(entity).State = EntityState.Unchanged;
        }

        public virtual void Attach(T entity)
        {
            _DbSet.Attach(entity);
        }

        public virtual void AttachRange(ICollection<T> entities)
        {
            _DbSet.AttachRange(entities);
        }

        public virtual void Detach(T entity)
        {
            _Context!.Entry(entity).State = EntityState.Detached;
        }

        public virtual async Task<bool> Exists(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _DbSet.AsQueryable().AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, IList<string>? includes = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _DbSet.AsQueryable().AsNoTracking();

            if (includes != null && includes.Count > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
                query = query.Where(filter);

            return await query.CountAsync(cancellationToken);
        }

        protected async Task<QueryPaginationResult<T>> GetQueryWithPaginationWithFilterAsync(IQueryable<T> query, PaginationRequestModel pagination, CancellationToken cancellationToken = default)
        {

            if (pagination.Filtros is not null)
            {
                foreach (var filtro in pagination.Filtros)
                {
                    if (filtro == null || string.IsNullOrEmpty(filtro.Campo) || string.IsNullOrEmpty(filtro.TipoFiltro) || string.IsNullOrEmpty(filtro.Valor))
                    {
                        continue;
                    }
                    query = BaseRepository<T>.ApplyFilter(query, filtro);

                }
            }

            return await GetPaginationAsync(query, pagination, cancellationToken);
        }

        protected async Task<QueryPaginationResult<T>> GetPaginationAsync(IQueryable<T> query, PaginationRequestModel pagination, CancellationToken cancellationToken = default)
        {

            if (pagination.Limite <= 0) pagination.Limite = 1;
            if (pagination.Pagina <= 0) pagination.Pagina = 1;

            var pageCount = Math.Ceiling(await CountAsync(cancellationToken: cancellationToken) / pagination.Limite);
            var cantidadRegistros = await query.CountAsync(cancellationToken: cancellationToken);
            var result = query
                .Skip((pagination.Pagina - 1) * (int)pagination.Limite)
                .Take((int)pagination.Limite);

            return new QueryPaginationResult<T>(pagination.Pagina, (int)pageCount, cantidadRegistros, result);
        }

        private static IQueryable<T> ApplyFilter(IQueryable<T> query, FiltroModel filtro)
        {
            if (filtro == null || string.IsNullOrEmpty(filtro.Campo) || string.IsNullOrEmpty(filtro.TipoFiltro) || string.IsNullOrEmpty(filtro.Valor))
            {
                // No hay filtro válido, devolver la consulta sin cambios
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "e");
            var property = Expression.Property(parameter, filtro.Campo);

            var constant = BaseRepository<T>.ConvertToConstantExpression(property.Type, filtro.Valor);
            var comparisonExpression = BaseRepository<T>.BuildComparisonExpression(property, constant, filtro.TipoFiltro);

            var lambda = Expression.Lambda<Func<T, bool>>(comparisonExpression, parameter);
            query = query.Where(lambda);

            return query;
        }

        private static ConstantExpression ConvertToConstantExpression(Type propertyType, string valor)
        {
            // Manejar la conversión según el tipo de propiedad
            return Expression.Constant(
                Convert.ChangeType(valor, Nullable.GetUnderlyingType(propertyType) ?? propertyType)
            );
        }

        private static Expression BuildComparisonExpression(MemberExpression property, ConstantExpression constant, string tipoFiltro)
        {
            MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;

            switch (tipoFiltro.ToLower())
            {
                case "startswith" when property.Type == typeof(string):
                    var toLowerCall = Expression.Call(property, toLowerMethod);
                    var constantLower = Expression.Call(constant, toLowerMethod);
                    return Expression.Call(toLowerCall, "StartsWith", null, constantLower);

                case "contains" when property.Type == typeof(string):
                    var toLowerCallContains = Expression.Call(property, toLowerMethod);
                    var constantLowerContains = Expression.Call(constant, toLowerMethod);
                    return Expression.Call(toLowerCallContains, "Contains", null, constantLowerContains);

                case "notcontains" when property.Type == typeof(string):
                    var toLowerCallNotContains = Expression.Call(property, toLowerMethod);
                    var constantLowerNotContains = Expression.Call(constant, toLowerMethod);
                    return Expression.Not(Expression.Call(toLowerCallNotContains, "Contains", null, constantLowerNotContains));

                case "endswith" when property.Type == typeof(string):
                    var toLowerCallEndsWith = Expression.Call(property, toLowerMethod);
                    var constantLowerEndsWith = Expression.Call(constant, toLowerMethod);
                    return Expression.Call(toLowerCallEndsWith, "EndsWith", null, constantLowerEndsWith);

                case "equals":
                    return Expression.Equal(property, constant);

                case "notequals":
                    return Expression.NotEqual(property, constant);

                case "greaterthan":
                    return Expression.GreaterThan(property, constant);

                case "lessthan":
                    return Expression.LessThan(property, constant);

                case "greaterthanequal":
                    return Expression.GreaterThanOrEqual(property, constant);

                case "lessthanequal":
                    return Expression.LessThanOrEqual(property, constant);


                default:
                    throw new InvalidOperationException($"Tipo de filtro no reconocido: {tipoFiltro}");
            }
        }
    }

    #endregion
}

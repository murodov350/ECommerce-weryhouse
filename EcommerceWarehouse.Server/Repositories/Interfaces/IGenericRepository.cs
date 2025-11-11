using System.Linq.Expressions;

namespace EcommerceWarehouse.Server.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // Basic operations
        Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<int> SaveChangesAsync();

        // Queryable access for advanced scenarios (paging, ThenInclude, ProjectTo)
        IQueryable<TEntity> AsQueryable();
        Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder);
        Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder);
    }
}

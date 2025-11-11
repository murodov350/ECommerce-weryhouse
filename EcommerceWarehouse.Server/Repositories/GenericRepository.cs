using System.Linq.Expressions;
using EcommerceWarehouse.Server.Data;
using EcommerceWarehouse.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceWarehouse.Server.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _db;
        protected readonly DbSet<TEntity> _set;
        public GenericRepository(ApplicationDbContext db)
        {
            _db = db;
            _set = db.Set<TEntity>();
        }
        public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set;
            foreach (var include in includes) query = query.Include(include);
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
        public async Task<List<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set;
            foreach (var include in includes) query = query.Include(include);
            return await query.ToListAsync();
        }
        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _set;
            foreach (var include in includes) query = query.Include(include);
            return await query.Where(predicate).ToListAsync();
        }
        public async Task AddAsync(TEntity entity) => await _set.AddAsync(entity);
        public void Update(TEntity entity) => _set.Update(entity);
        public void Remove(TEntity entity) => _set.Remove(entity);
        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
        public IQueryable<TEntity> AsQueryable() => _set.AsQueryable();
        public async Task<TEntity?> GetByIdAsync(Guid id, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder)
        {
            var query = includeBuilder == null ? _set.AsQueryable() : includeBuilder(_set.AsQueryable());
            return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
        }
        public async Task<List<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder)
        {
            var query = includeBuilder == null ? _set.AsQueryable() : includeBuilder(_set.AsQueryable());
            return await query.ToListAsync();
        }
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeBuilder)
        {
            var query = includeBuilder == null ? _set.AsQueryable() : includeBuilder(_set.AsQueryable());
            return await query.FirstOrDefaultAsync(predicate);
        }
    }
}

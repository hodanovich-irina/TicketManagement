using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Providing methods for managing entities.
    /// </summary>
    internal class Repository<T> : IRepository<T>, IEFRepository<T>
        where T : class
    {
        private readonly TicketManagementContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(TicketManagementContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<T>();
        }

        /// <summary>
        /// Method for add entity.
        /// </summary>
        /// <param name="entity">Object of entity.</param>
        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Method for delete entity.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            var areaForDelete = _dbSet.Find(id);
            var result = _dbSet.Remove(areaForDelete);
            await _dbContext.SaveChangesAsync();
            return result.State.HasFlag(EntityState.Deleted);
        }

        /// <summary>
        /// Method for edit entity.
        /// </summary>
        /// <param name="entity">Object of entity.</param>
        public async Task<bool> EditAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Method for get antity by id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        /// <returns>Object of entity.</returns>
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Method for get entity by id.
        /// </summary>
        /// <param name="predicate">Anonymous meyhod signature.</param>
        /// <returns>Collection of entities.</returns>
        public async Task<IQueryable<T>> GetAsync(Func<T, bool> predicate)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().Where(predicate).AsQueryable());
        }

        /// <summary>
        /// Method for get all entities.
        /// </summary>
        /// <returns>Collection of entities.</returns>
        public async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.Run(() => _dbSet.AsNoTracking().AsQueryable());
        }
    }
}

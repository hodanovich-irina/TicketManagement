using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Providing methods for managing event.
    /// </summary>
    internal class EFEventRepository : IRepository<Event>, IEFRepository<Event>
    {
        private readonly TicketManagementContext _dbContext;
        private readonly DbSet<Event> _dbSet;

        public EFEventRepository(TicketManagementContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Event;
        }

        /// <summary>
        /// Method for add event.
        /// </summary>
        /// <param name="entity">Object of event.</param>
        public async Task<Event> AddAsync(Event entity)
        {
            var output = new Microsoft.Data.SqlClient.SqlParameter();
            output.ParameterName = "@AddedId";
            output.SqlDbType = SqlDbType.Int;
            output.Size = int.MaxValue;
            output.Direction = ParameterDirection.Output;
            await _dbContext.Database.ExecuteSqlRawAsync("InsertEvent {0}, {1}, {2}, {3}, {4}, {5}, {6}, @AddedId output",
                entity.Name, entity.Description, entity.LayoutId, entity.DateStart, entity.DateEnd, entity.ImageURL, entity.ShowTime, output);
            entity.Id = (int)output.Value;
            return entity;
        }

        /// <summary>
        /// Method for delete event.
        /// </summary>
        /// <param name="id">Id of event.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _dbContext.Database.ExecuteSqlRawAsync("DeleteEvent {0}", id);
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Method for edit event.
        /// </summary>
        /// <param name="entity">Object of event.</param>
        public async Task<bool> EditAsync(Event entity)
        {
            var result = await _dbContext.Database.ExecuteSqlRawAsync("UpdateEvent {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}",
                entity.Id, entity.Name, entity.Description, entity.LayoutId, entity.DateStart, entity.DateEnd, entity.ImageURL, entity.ShowTime);
            await _dbContext.SaveChangesAsync();
            return Convert.ToBoolean(result);
        }

        /// <summary>
        /// Method for get area by id.
        /// </summary>
        /// <param name="id">Id of area.</param>
        /// <returns>Object of area.</returns>
        public async Task<Event> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Method for get event by id.
        /// </summary>
        /// <param name="predicate">Anonymous meyhod signature.</param>
        /// <returns>Collection of event.</returns>
        public async Task<IQueryable<Event>> GetAsync(Func<Event, bool> predicate)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().Where(predicate).AsQueryable());
        }

        /// <summary>
        /// Method for get all event.
        /// </summary>
        /// <returns>Collection of event.</returns>
        public async Task<IQueryable<Event>> GetAllAsync()
        {
            return await Task.Run(() => _dbSet.AsNoTracking().AsQueryable());
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    public interface IEFRepository<T>
        where T : class
    {
        /// <summary>
        /// Method for get all entity by parent Id.
        /// </summary>
        /// <param name="predicate">Anonymous meyhod signature.</param>
        /// <returns>Collection of objects.</returns>
        Task<IQueryable<T>> GetAsync(Func<T, bool> predicate);

        /// <summary>
        /// Method for get all entity.
        /// </summary>
        /// <returns>Collection of objects.</returns>
        Task<IQueryable<T>> GetAllAsync();
    }
}

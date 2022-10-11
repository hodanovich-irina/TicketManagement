using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for venue repository where method get all has param.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface IVenueRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Method for get all objects.
        /// </summary>
        /// <returns>Collection of objects.</returns>
        Task<IQueryable<T>> GetAllAsync();
    }
}

using System.Linq;
using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for repositories where method get all has param.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface IModelRepository<T> : IRepository<T>
    {
        /// <summary>
        /// Method for get all objects with id.
        /// </summary>
        /// <param name="id">Id of objects.</param>
        /// <returns>Collection of objects.</returns>
        Task<IQueryable<T>> GetAllByParentIdAsync(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketManagement.TicketAPI.Interfaces
{
    /// <summary>
    /// Interface for work with services.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Logic for add object.
        /// </summary>
        /// <param name="entity">Object of class.</param>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Logic for edit object.
        /// </summary>
        /// <param name="entity">Object of class.</param>
        Task<bool> EditAsync(T entity);

        /// <summary>
        /// Logic for delete object.
        /// </summary>
        /// <param name="id">Id of object.</param>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Logic for get object by id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Logic for get all object.
        /// </summary>
        /// <param name="id">Parent id of object.</param>
        /// <returns>Collection of object.</returns>
        Task<IEnumerable<T>> GetAsync(int id);

        /// <summary>
        /// Logic for get all object.
        /// </summary>
        /// <returns>Collection of object.</returns>
        Task<IEnumerable<T>> GetAllAsync();
    }
}

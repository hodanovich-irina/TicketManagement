using System.Threading.Tasks;

namespace TicketManagement.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for repositories.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Method for get entity by id.
        /// </summary>
        /// <param name="id">Id of object.</param>
        /// <returns>Object of entity.</returns>
        Task<T> GetByIdAsync(int id);

        /// <summary>
        /// Method for add entity.
        /// </summary>
        /// <param name="entity">Object of entity.</param>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Method for delete entity.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Method for edit entity.
        /// </summary>
        /// <param name="entity">Object of entity.</param>
        Task<bool> EditAsync(T entity);
    }
}

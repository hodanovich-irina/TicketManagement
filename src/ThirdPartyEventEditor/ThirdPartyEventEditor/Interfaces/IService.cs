using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Interfaces for CRUD operations.
    /// </summary>
    /// <typeparam name="T">type of entity.</typeparam>
    public interface IService<T>
    {
        /// <summary>
        /// Method for delete.
        /// </summary>
        /// <param name="id">entity id.</param>
        /// <returns>delete information.</returns>
        bool Delete(int id);

        /// <summary>
        /// Method for add.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns>entity</returns>
        Task<T> Add(T entity);

        /// <summary>
        /// Method for edit.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns>edit information.</returns>
        Task<bool> Edit(T entity);

        /// <summary>
        /// Method for get by id.
        /// </summary>
        /// <param name="id">entity id.</param>
        /// <returns>entity.</returns>
        T GetById(int id);

        /// <summary>
        /// Method for get all.
        /// </summary>
        /// <returns>collection of entities.</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Method for create import file.
        /// </summary>
        /// <param name="fileName">file name.</param>
        /// <param name="eventsId">collection of id.</param>
        void CreateImportFile(string fileName, IList<int> eventsId);
    }
}

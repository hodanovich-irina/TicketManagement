using System.Collections.Generic;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Interface for third party event repository.
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Method for write data.
        /// </summary>
        /// <param name="entity">entity.</param>
        void Write(IEnumerable<T> entity);

        /// <summary>
        /// Method for read data.
        /// </summary>
        /// <returns>collection of object.</returns>
        IEnumerable<T> Read();
    }
}

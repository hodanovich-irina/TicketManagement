using System.Collections.Generic;

namespace ThirdPartyEventEditor.Interfaces
{
    /// <summary>
    /// Interface for file work.
    /// </summary>
    public interface IThirdPartyEventFileToExportCreator<T>
    {
        /// <summary>
        /// Method for write data.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <param name="fileName">file name.</param>
        void Write(IEnumerable<T> entity, string fileName);

        /// <summary>
        /// Method for create file.
        /// </summary>
        /// <param name="fileName">file name</param>
        void Create(string fileName);
    }
}

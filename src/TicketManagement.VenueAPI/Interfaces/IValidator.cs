namespace TicketManagement.VenueAPI.Interfaces
{
    /// <summary>
    /// Interface for validation.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    internal interface IValidator<T>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Oblect of entity.</param>
        void ValidationBeforeAddAndEdit(T entity);

        /// <summary>
        /// Method for validity check id.
        /// </summary>
        /// <param name="id">Id of entity.</param>
        void ValidateId(int id);
    }
}

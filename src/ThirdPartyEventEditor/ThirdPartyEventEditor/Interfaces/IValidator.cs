namespace ThirdPartyEventEditor.Interfaces
{
    public interface IValidator<T>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="entity">Oblect of entity.</param>
        void IsValid(T entity);
    }
}

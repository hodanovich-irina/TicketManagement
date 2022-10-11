using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Services
{
    /// <summary>
    /// Providing methods for managing area with validation and some logic.
    /// </summary>
    public class ThirdPartyEventService : IService<ThirdPartyEvent>
    {
        private readonly IRepository<ThirdPartyEvent> _repository;
        private readonly IThirdPartyEventFileToExportCreator<ThirdPartyEvent> _importRepository;
        private readonly IValidator<ThirdPartyEvent> _validator;

        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="repository">third party event repository.</param>
        /// <param name="importRepository">import repository.</param>
        /// <param name="validator">validator.</param>
        public ThirdPartyEventService(IRepository<ThirdPartyEvent> repository, IThirdPartyEventFileToExportCreator<ThirdPartyEvent> importRepository, IValidator<ThirdPartyEvent> validator)
        {
            _repository = repository;
            _importRepository = importRepository;
            _validator = validator;
        }

        /// <summary>
        /// Method for add.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns>entity</returns>
        public async Task<ThirdPartyEvent> Add(ThirdPartyEvent entity)
        {
            _validator.IsValid(entity);
            var allEvents = _repository.Read();

            var eventsList = allEvents.ToList();
            if (eventsList.Any())
            { 
                var id = allEvents.Select(eventId => eventId.Id).Max();
                entity.Id = ++id;
            }            
            var img = await entity.PosterImage.UploadSampleImage();
            entity.PosterImage = img;
            eventsList.Add(entity);
            _repository.Write(eventsList);
            return entity;
        }

        /// <summary>
        /// Method for delete.
        /// </summary>
        /// <param name="id">third party event id.</param>
        /// <returns>delete information.</returns>
        public bool Delete(int id)
        {
            var allEvent = _repository.Read();
            var deleteEvent = allEvent.Where(isDeletedEvent => !isDeletedEvent.Id.Equals(id)).Select(events => events);
            _repository.Write(deleteEvent);
            var isDeleted = deleteEvent.All(isDeletedEvent => !isDeletedEvent.Id.Equals(id));
            return isDeleted;
        }

        /// <summary>
        /// Method for edit.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns>edit information.</returns>
        public async Task<bool> Edit(ThirdPartyEvent entity)
        {
            _validator.IsValid(entity);
            var allEvent = _repository.Read();
            var eventToEdit = allEvent.FirstOrDefault(isEditedEvent => isEditedEvent.Id.Equals(entity.Id));
            await ChangeEventAsync(eventToEdit, entity);
            _repository.Write(allEvent);
            var isEdited = allEvent.FirstOrDefault(isEdit => isEdit.Id.Equals(entity.Id));
            return isEdited.Equals(entity);
        }

        /// <summary>
        /// Method for get all.
        /// </summary>
        /// <returns>collection of third party event.</returns>
        public IEnumerable<ThirdPartyEvent> GetAll()
        {
            var allEvents = _repository.Read();
            return allEvents;
        }

        /// <summary>
        /// Method for get by id.
        /// </summary>
        /// <param name="id">third party event id.</param>
        /// <returns>third party event.</returns>
        public ThirdPartyEvent GetById(int id)
        {
            var allEvents = _repository.Read();
            var eventToGet = allEvents.FirstOrDefault(findEvent => findEvent.Id.Equals(id));
            return eventToGet;
        }

        /// <summary>
        /// Method for create import file.
        /// </summary>
        /// <param name="fileName">file name.</param>
        /// <param name="eventsId">collection of id.</param>
        public void CreateImportFile(string fileName, IList<int> eventsId)
        {
            _importRepository.Create(fileName);
            var events = new List<ThirdPartyEvent>();
            var allEvents = _repository.Read();
            events = allEvents.Where(eventToFile => eventsId.Contains(eventToFile.Id)).Select(eventToAdd => eventToAdd).ToList();
            _importRepository.Write(events, fileName);
        }

        private async Task<ThirdPartyEvent> ChangeEventAsync(ThirdPartyEvent eventToEdit, ThirdPartyEvent entity) 
        {
            eventToEdit.Name = entity.Name;
            eventToEdit.Description = entity.Description;
            eventToEdit.EndDate = entity.EndDate;
            eventToEdit.LayoutName = entity.LayoutName;
            eventToEdit.VenueName = entity.VenueName;
            var img = await entity.PosterImage.UploadSampleImage();
            eventToEdit.PosterImage = img;
            return eventToEdit;
        }
    }
}
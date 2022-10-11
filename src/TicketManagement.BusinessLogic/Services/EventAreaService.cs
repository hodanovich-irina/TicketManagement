using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.Automapper;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Services
{
    /// <summary>
    /// Providing methods for managing event area with validation and business logic.
    /// </summary>
    internal class EventAreaService : AutoMapperService, IService<EventAreaDto>
    {
        private readonly IEFRepository<EventArea> _eventAreaEFRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IEFRepository<EventSeat> _eventSeatEFRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IValidator<EventAreaDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventAreaService"/> class.
        /// Constructor with params.
        /// </summary>
        /// <param name="eventAreaEFRepository">Object of event area ef repository.</param>
        /// <param name="eventAreaRepository">Object of event area repository.</param>
        /// <param name="eventSeatEFRepository">Object of event seat ef repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="validator">Object of event area validator.</param>
        public EventAreaService(IEFRepository<EventArea> eventAreaEFRepository, IRepository<EventSeat> eventSeatRepository, IRepository<EventArea> eventAreaRepository,
            IEFRepository<EventSeat> eventSeatEFRepository, IValidator<EventAreaDto> validator)
        {
            _eventAreaEFRepository = eventAreaEFRepository;
            _eventSeatRepository = eventSeatRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatEFRepository = eventSeatEFRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add event area.
        /// </summary>
        /// <param name="entity">Object of class event area.</param>
        public async Task<EventAreaDto> AddAsync(EventAreaDto entity)
        {
            await Task.Run(() =>_validator.ValidationBeforeAddAndEdit(entity));
            throw new InvalidOperationException("You can't create area for event without event");
        }

        /// <summary>
        /// Logic for edit event area.
        /// </summary>
        /// <param name="entity">Object of class event area.</param>
        public async Task<bool> EditAsync(EventAreaDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            _validator.ValidateId(entity.Id);
            return await _eventAreaRepository.EditAsync(Mapper.Map<EventArea>(entity));
        }

        /// <summary>
        /// Logic for delete event area.
        /// </summary>
        /// <param name="id">Id of event area object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            var allEvenetAreaEventSeats = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(id));
            var isEventSeatsBooked = allEvenetAreaEventSeats.Any(seatState => seatState.State.Equals(EventSeatState.Booked));
            if (isEventSeatsBooked)
            {
                throw new InvalidOperationException("You can't delete event area. Any seats booked");
            }

            foreach (var eventAreaEvenetSeat in allEvenetAreaEventSeats.ToList())
            {
                await _eventSeatRepository.DeleteAsync(eventAreaEvenetSeat.Id);
            }

            return await _eventAreaRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for get event area by id.
        /// </summary>
        /// <param name="id">Id of event area object.</param>
        public async Task<EventAreaDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<EventAreaDto>(await _eventAreaRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all event area.
        /// </summary>
        /// <param name="id">Event Id of event area object.</param>
        /// <returns>Collection of event area object.</returns>
        public async Task<IEnumerable<EventAreaDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var areas = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(id));
            return areas.Select(area => Mapper.Map<EventAreaDto>(area)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get all event area.
        /// </summary>
        /// <returns>Collection of event area.</returns>
        public async Task<IEnumerable<EventAreaDto>> GetAllAsync()
        {
            var eventAreas = await _eventAreaEFRepository.GetAllAsync();
            return eventAreas.Select(eventArea => Mapper.Map<EventAreaDto>(eventArea)).AsEnumerable();
        }
    }
}

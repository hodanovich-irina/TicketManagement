using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventAPI.Automapper;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;

namespace TicketManagement.EventAPI.Services
{
    /// <summary>
    /// Providing methods for managing event seat with validation and business logic.
    /// </summary>
    internal class EventSeatService : AutoMapperService,  IService<EventSeatDto>
    {
        private readonly IEFRepository<EventSeat> _eventSeatEFRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IValidator<EventSeatDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSeatService"/> class.
        /// Constructor with params.
        /// </summary>
        /// <param name="eventSeatEFRepository">Object of event seat ef repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="validator">Object of event seat validator.</param>
        public EventSeatService(IEFRepository<EventSeat> eventSeatEFRepository, IRepository<EventSeat> eventSeatRepository, IValidator<EventSeatDto> validator)
        {
            _eventSeatEFRepository = eventSeatEFRepository;
            _eventSeatRepository = eventSeatRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add event seat.
        /// </summary>
        /// <param name="entity">Object of class event seat.</param>
        public async Task<EventSeatDto> AddAsync(EventSeatDto entity)
        {
            await Task.Run(() => _validator.ValidationBeforeAddAndEdit(entity));
            throw new InvalidOperationException("You can't create seat for event without event");
        }

        /// <summary>
        /// Logic for delete event seat.
        /// </summary>
        /// <param name="id">Id of event seat object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            var eventSeats = await _eventSeatRepository.GetByIdAsync(id);
            var isEventSeatsBooked = eventSeats.State.Equals(EventSeatState.Booked);
            if (isEventSeatsBooked)
            {
                throw new InvalidOperationException("You can't delete booked seat");
            }

            return await _eventSeatRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for edit event seat.
        /// </summary>
        /// <param name="entity">Object of class event seat.</param>
        public async Task<bool> EditAsync(EventSeatDto entity)
        {
            _validator.ValidateId(entity.Id);
            _validator.ValidationBeforeAddAndEdit(entity);
            var allEventAreaEventSeats = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(entity.EventAreaId));
            var isRowAndNumAndIdExists = allEventAreaEventSeats.Any(seatRowAndNum => seatRowAndNum.Row.Equals(entity.Row)
            && seatRowAndNum.Number.Equals(entity.Number) && seatRowAndNum.Id.Equals(entity.Id));
            var isRowAndNumExists = allEventAreaEventSeats.Any(seatRowAndNum => seatRowAndNum.Row.Equals(entity.Row) && seatRowAndNum.Number.Equals(entity.Number));
            if (isRowAndNumAndIdExists || !isRowAndNumExists)
            {
                return await _eventSeatRepository.EditAsync(Mapper.Map<EventSeat>(entity));
            }
            else
            {
                throw new InvalidOperationException("You can't edit this seat. Seat with this row and number alredy exist in this event area");
            }
        }

        /// <summary>
        /// Logic for get event seat by id.
        /// </summary>
        /// <param name="id">Id of event seat object.</param>
        public async Task<EventSeatDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<EventSeatDto>(await _eventSeatRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all area.
        /// </summary>
        /// <param name="id">Event area Id of event seat object.</param>
        /// <returns>Collection of event seat object.</returns>
        public async Task<IEnumerable<EventSeatDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var eventSeats = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(id));
            return eventSeats.Select(eventSeat => Mapper.Map<EventSeatDto>(eventSeat)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get all event seat.
        /// </summary>
        /// <returns>Collection of event seat.</returns>
        public async Task<IEnumerable<EventSeatDto>> GetAllAsync()
        {
            var eventSeats = await _eventSeatEFRepository.GetAllAsync();
            return eventSeats.Select(eventSeat => Mapper.Map<EventSeatDto>(eventSeat)).AsEnumerable();
        }
    }
}

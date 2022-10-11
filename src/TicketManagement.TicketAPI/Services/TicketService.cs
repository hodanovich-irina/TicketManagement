using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.TicketAPI.Automapper;
using TicketManagement.TicketAPI.Dto;
using TicketManagement.TicketAPI.Interfaces;

namespace TicketManagement.TicketAPI.Services
{
    /// <summary>
    /// Providing methods for managing ticket with validation and business logic.
    /// </summary>
    internal class TicketService : AutoMapperService,  ITicketService
    {
        private readonly IEFRepository<Ticket> _ticketEFRepository;
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IValidator<TicketDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketService"/> class.
        /// Constructor with params.
        /// </summary>
        /// <param name="ticketEFRepository">Object of ticket ef repository.</param>
        /// <param name="ticketRepository">Object of ticket repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="eventAreaRepository">Object of event area repository.</param>
        /// <param name="eventRepository">.</param>
        /// <param name="validator">Object of area validator.</param>
        public TicketService(IEFRepository<Ticket> ticketEFRepository, IRepository<Ticket> ticketRepository,
            IRepository<EventSeat> eventSeatRepository, IRepository<EventArea> eventAreaRepository, IRepository<Event> eventRepository,
            IValidator<TicketDto> validator)
        {
            _ticketEFRepository = ticketEFRepository;
            _ticketRepository = ticketRepository;
            _eventSeatRepository = eventSeatRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventRepository = eventRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add ticket.
        /// </summary>
        /// <param name="entity">Object of class ticket.</param>
        public async Task<TicketDto> AddAsync(TicketDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var seat = await _eventSeatRepository.GetByIdAsync(entity.EventSeatId);
            if (seat.State.Equals(EventSeatState.Booked))
            {
                throw new InvalidOperationException("You can't buy ticket for event. This seat is booked");
            }

            seat.State = EventSeatState.Booked;
            await _eventSeatRepository.EditAsync(seat);
            var ticket = await _ticketRepository.AddAsync(Mapper.Map<Ticket>(entity));
            return Mapper.Map<TicketDto>(ticket);
        }

        /// <summary>
        /// Logic for delete ticket.
        /// </summary>
        /// <param name="id">Id of ticket object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            var ticketForDelete = await _ticketRepository.GetByIdAsync(id);
            if (ticketForDelete is null)
            {
                throw new InvalidOperationException("Incorrect data");
            }

            var seat = await _eventSeatRepository.GetByIdAsync(ticketForDelete.EventSeatId);
            seat.State = EventSeatState.Free;
            await _eventSeatRepository.EditAsync(seat);
            return await _ticketRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for edit ticket.
        /// </summary>
        /// <param name="entity">Object of class ticket.</param>
        public Task<bool> EditAsync(TicketDto entity)
        {
            throw new InvalidOperationException("You can't edit ticket. If you want buy any ticket, delete this ticket and add another one.");
        }

        /// <summary>
        /// Logic for get all ticket.
        /// </summary>
        /// <param name="id">Event seat Id of ticket object.</param>
        /// <returns>Collection of ticket object.</returns>
        public async Task<IEnumerable<TicketDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var tickets = await _ticketEFRepository.GetAsync(ticket => ticket.EventSeatId.Equals(id));
            return tickets.Select(ticket => Mapper.Map<TicketDto>(ticket)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get all area.
        /// </summary>
        /// <param name="id">User Id of ticket object.</param>
        /// <returns>Collection of ticket object.</returns>
        public async Task<IEnumerable<TicketDto>> GetByParentStringIdAsync(string id)
        {
            var tickets = await _ticketEFRepository.GetAsync(ticket => ticket.UserId.Equals(id));
            return tickets.Select(ticket => Mapper.Map<TicketDto>(ticket)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get ticket by id.
        /// </summary>
        /// <param name="id">Id of ticket object.</param>
        public async Task<TicketDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<TicketDto>(await _ticketRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all ticket.
        /// </summary>
        /// <returns>Collection of ticket.</returns>
        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _ticketEFRepository.GetAllAsync();
            return tickets.Select(ticket => Mapper.Map<TicketDto>(ticket)).AsEnumerable();
        }

        /// <summary>
        /// Logic for get all ticket.
        /// </summary>
        /// <returns>Collection of ticket.</returns>
        public async Task<IEnumerable<TicketInfo>> GetUserTicketInfo(string id)
        {
            var tickets = await _ticketEFRepository.GetAsync(ticket => ticket.UserId.Equals(id));
            var ticketsModel = new List<TicketInfo>();
            foreach (var ticketModel in tickets)
            {
                var seat = await _eventSeatRepository.GetByIdAsync(ticketModel.EventSeatId);
                var area = await _eventAreaRepository.GetByIdAsync(seat.EventAreaId);
                var eventInArea = await _eventRepository.GetByIdAsync(area.EventId);
                ticketsModel.Add(new TicketInfo
                {
                    Id = ticketModel.Id,
                    Price = ticketModel.Price,
                    DateOfPurchase = ticketModel.DateOfPurchase,
                    Number = seat.Number,
                    Row = seat.Row,
                    DateEnd = eventInArea.DateEnd,
                    DateStart = eventInArea.DateStart,
                    EventName = eventInArea.Name,
                    EventSeatId = ticketModel.EventSeatId,
                });
            }

            return ticketsModel;
        }
    }
}

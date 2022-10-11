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
    /// Providing methods for managing event with validation and business logic.
    /// </summary>
    internal class EventService : AutoMapperService, IService<EventDto>
    {
        private readonly IEFRepository<Event> _eventEFRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Layout> _layoutRepository;
        private readonly IEFRepository<Layout> _layoutEFRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IEFRepository<Area> _areaEFRepository;
        private readonly IEFRepository<Seat> _seatEFRepository;
        private readonly IEFRepository<EventSeat> _eventSeatEFRepository;
        private readonly IEFRepository<EventArea> _eventAreaEFRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IValidator<EventDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventService"/> class.
        /// </summary>
        /// <param name="eventEFRepository">Object of event ef repository.</param>
        /// <param name="eventRepository">Object of event repository.</param>
        /// <param name="layuotRepository">Object of layout repository.</param>
        /// <param name="layuotEFRepository">Object of layout ef repository.</param>
        /// <param name="eventAreaEFRepository">Object of event area ef repository.</param>
        /// <param name="areaEFRepository">Object of area ef repository.</param>
        /// <param name="seatEFRepository">Object of seat ef repository.</param>
        /// <param name="eventSeatEFRepository">Object of event seat ef repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="eventAreaRepository">Object of event area repository.</param>
        /// <param name="validator">Object of event validator.</param>
        public EventService(IEFRepository<Event> eventEFRepository, IRepository<Event> eventRepository, IRepository<Layout> layuotRepository,
            IEFRepository<Layout> layuotEFRepository, IEFRepository<EventArea> eventAreaEFRepository,
            IEFRepository<Area> areaEFRepository, IEFRepository<Seat> seatEFRepository, IEFRepository<EventSeat> eventSeatEFRepository,
            IRepository<EventSeat> eventSeatRepository,
            IRepository<EventArea> eventAreaRepository, IValidator<EventDto> validator)
        {
            _eventEFRepository = eventEFRepository;
            _eventRepository = eventRepository;
            _layoutRepository = layuotRepository;
            _layoutEFRepository = layuotEFRepository;
            _eventSeatRepository = eventSeatRepository;
            _areaEFRepository = areaEFRepository;
            _seatEFRepository = seatEFRepository;
            _eventSeatEFRepository = eventSeatEFRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventAreaEFRepository = eventAreaEFRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add event.
        /// </summary>
        /// <param name="entity">Object of class event.</param>
        public async Task<EventDto> AddAsync(EventDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var layout = await _layoutRepository.GetByIdAsync(entity.LayoutId);
            var allVenueLayouts = await _layoutEFRepository.GetAsync(layoutInVenue => layoutInVenue.VenueId.Equals(layout.VenueId));
            foreach (var venueLayout in allVenueLayouts.ToList())
            {
                var layoutEvents = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(venueLayout.Id));
                var allLayoutEvents = layoutEvents.All(eventDate => (eventDate.DateStart > entity.DateStart && eventDate.DateStart > entity.DateEnd)
                    || (eventDate.DateEnd < entity.DateStart && eventDate.DateEnd < entity.DateEnd));
                if (!allLayoutEvents)
                {
                    throw new InvalidOperationException("You can't create event. Event on this period alredy exsist in venue!");
                }
            }

            if (entity.DateStart < DateTime.Now)
            {
                throw new InvalidOperationException("You can't create event in tha past!");
            }

            if (entity.DateStart >= entity.DateEnd)
            {
                throw new InvalidOperationException("You can't create event. Date of end should be more than date of start");
            }

            var addedEvent = await _eventRepository.AddAsync(Mapper.Map<Event>(entity));
            var addedEventDto = Mapper.Map<EventDto>(addedEvent);
            var events = await _eventEFRepository.GetAllAsync();
            var eventId = events.OrderByDescending(x => x.Id).FirstOrDefault().Id;
            var allLayoutAreas = await _areaEFRepository.GetAsync(area => area.LayoutId.Equals(entity.LayoutId));
            foreach (var layoutArea in allLayoutAreas.ToList())
            {
                await _eventAreaRepository.AddAsync(new EventArea
                {
                    CoordX = layoutArea.CoordX,
                    CoordY = layoutArea.CoordY,
                    Description = layoutArea.Description,
                    EventId = eventId,
                    Price = entity.BaseAreaPrice,
                });
                var eventAreas = await _eventAreaEFRepository.GetAllAsync();
                var eventAreaId = eventAreas.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                var allAreaSeats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(layoutArea.Id));
                foreach (var eventSeat in allAreaSeats.ToList())
                {
                    await _eventSeatRepository.AddAsync(new EventSeat
                    {
                        EventAreaId = eventAreaId,
                        Number = eventSeat.Number,
                        Row = eventSeat.Row,
                        State = EventSeatState.Free,
                    });
                }
            }

            return addedEventDto;
        }

        /// <summary>
        /// Logic for delete event.
        /// </summary>
        /// <param name="id">Id of area object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            await GetInformationForDeleteEvent(id);
            var eventAreasForDelete = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(id));
            foreach (var eventAreaForDelete in eventAreasForDelete.ToList())
            {
                var seatsForDelete = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(eventAreaForDelete.Id));
                foreach (var seatForDelete in seatsForDelete.ToList())
                {
                    await _eventSeatRepository.DeleteAsync(seatForDelete.Id);
                }

                await _eventAreaRepository.DeleteAsync(eventAreaForDelete.Id);
            }

            return await _eventRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for edit event.
        /// </summary>
        /// <param name="entity">Object of class event.</param>
        public async Task<bool> EditAsync(EventDto entity)
        {
            _validator.ValidateId(entity.Id);
            _validator.ValidationBeforeAddAndEdit(entity);
            var layout = await _layoutRepository.GetByIdAsync(entity.LayoutId);
            var allVenueLayouts = await _layoutEFRepository.GetAsync(layoutInVenue => layoutInVenue.VenueId.Equals(layout.VenueId));
            foreach (var venueLayout in allVenueLayouts.ToList())
            {
                var layoutEvents = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(venueLayout.Id));
                var layoutWithoutEditVenue = layoutEvents.Where(eventInLayout => !eventInLayout.Id.Equals(entity.Id)).Select(eventToSelect => eventToSelect);
                var allLayoutEvents = layoutWithoutEditVenue.All(eventDate => (eventDate.DateStart > entity.DateStart && eventDate.DateStart > entity.DateEnd)
                    || (eventDate.DateEnd < entity.DateStart && eventDate.DateEnd < entity.DateEnd));
                if (!allLayoutEvents)
                {
                    throw new InvalidOperationException("You can't edit event. Event on this period alredy exsist in venue!");
                }
            }

            var currentEvent = await _eventRepository.GetByIdAsync(entity.Id);
            if (currentEvent.DateStart.Equals(entity.DateStart) && currentEvent.DateEnd.Equals(entity.DateEnd))
            {
                await _eventRepository.EditAsync(Mapper.Map<Event>(entity));
            }

            if (entity.DateStart < DateTime.Now)
            {
                throw new InvalidOperationException("You can't edit event in tha past!");
            }

            if (entity.DateStart >= entity.DateEnd)
            {
                throw new InvalidOperationException("You can't edit event. Date of end should be more than date of start");
            }

            return await _eventRepository.EditAsync(Mapper.Map<Event>(entity));
        }

        /// <summary>
        /// Logic for get event by id.
        /// </summary>
        /// <param name="id">Id of event object.</param>
        public async Task<EventDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<EventDto>(await _eventRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all event.
        /// </summary>
        /// <param name="id">Layout Id of event object.</param>
        /// <returns>Collection of event object.</returns>
        public async Task<IEnumerable<EventDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var events = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(id));
            return events.Select(eventModel => Mapper.Map<EventDto>(eventModel)).AsEnumerable();
        }

        private async Task GetInformationForDeleteEvent(int eventId)
        {
            var allEventEventAreasForDelete = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(eventId));
            foreach (var eventEventAreaForDelete in allEventEventAreasForDelete.ToList())
            {
                var eventAreaEventSeatsForDelete = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(eventEventAreaForDelete.Id));

                if (eventAreaEventSeatsForDelete.Any(bookedSeat => bookedSeat.State.Equals(EventSeatState.Booked)))
                {
                    throw new InvalidOperationException("You can't delete event. Any seats booked");
                }
            }
        }

        /// <summary>
        /// Logic for get all event.
        /// </summary>
        /// <returns>Collection of event.</returns>
        public async Task<IEnumerable<EventDto>> GetAllAsync()
        {
            var events = await _eventEFRepository.GetAllAsync();
            return events.Select(eventToSelect => Mapper.Map<EventDto>(eventToSelect)).AsEnumerable();
        }
    }
}

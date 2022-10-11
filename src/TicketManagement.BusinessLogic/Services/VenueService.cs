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
    /// Providing methods for managing venue with validation and business logic.
    /// </summary>
    internal class VenueService : AutoMapperService,  IVenueService
    {
        private readonly IEFRepository<Venue> _venueEFRepository;
        private readonly IRepository<Venue> _venueRepository;
        private readonly IRepository<Layout> _layoutRepository;
        private readonly IRepository<Area> _areaRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IEFRepository<Layout> _layoutEFRepository;
        private readonly IEFRepository<Area> _areaEFRepository;
        private readonly IEFRepository<Seat> _seatEFRepository;
        private readonly IEFRepository<Event> _eventEFRepository;
        private readonly IEFRepository<EventSeat> _eventSeatEFRepository;
        private readonly IEFRepository<EventArea> _eventAreaEFRepository;
        private readonly IValidator<VenueDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="VenueService"/> class.
        /// </summary>
        /// <param name="venueEFRepository">Object of venue ef repository.</param>
        /// <param name="layoutRepository">Object of venue repository.</param>
        /// <param name="venueRepository">Object of layout repository.</param>
        /// <param name="areaRepository">Object of area repository.</param>
        /// <param name="seatRepository">Object of seat repository.</param>
        /// <param name="eventRepository">Object of event repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="eventAreaRepository">Object of event area repository.</param>
        /// <param name="layoutEFRepository">Object of layout ef repository.</param>
        /// <param name="areaEFRepository">Object of area ef repository.</param>
        /// <param name="seatEFRepository">Object of seat ef repository.</param>
        /// <param name="eventEFRepository">Object of event ef repository.</param>
        /// <param name="eventSeatEFRepository">Object of event seat ef repository.</param>
        /// <param name="eventAreaEFRepository">Object of event area ef repository.</param>
        /// <param name="validator">Object of layout validator.</param>
        public VenueService(IEFRepository<Venue> venueEFRepository, IRepository<Venue> venueRepository, IRepository<Layout> layoutRepository, IRepository<Area> areaRepository,
            IRepository<Seat> seatRepository, IRepository<Event> eventRepository, IRepository<EventSeat> eventSeatRepository, IRepository<EventArea> eventAreaRepository,
            IEFRepository<Layout> layoutEFRepository, IEFRepository<Area> areaEFRepository,
            IEFRepository<Seat> seatEFRepository, IEFRepository<Event> eventEFRepository, IEFRepository<EventSeat> eventSeatEFRepository, IEFRepository<EventArea> eventAreaEFRepository,
            IValidator<VenueDto> validator)
        {
            _venueEFRepository = venueEFRepository;
            _venueRepository = venueRepository;
            _layoutRepository = layoutRepository;
            _areaRepository = areaRepository;
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
            _seatRepository = seatRepository;
            _layoutEFRepository = layoutEFRepository;
            _areaEFRepository = areaEFRepository;
            _eventEFRepository = eventEFRepository;
            _eventAreaEFRepository = eventAreaEFRepository;
            _eventSeatEFRepository = eventSeatEFRepository;
            _seatEFRepository = seatEFRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add venue.
        /// </summary>
        /// <param name="entity">Object of class venue.</param>
        public async Task<VenueDto> AddAsync(VenueDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var allVenues = await _venueEFRepository.GetAllAsync();
            var isNameExists = allVenues.Any(name => name.Name.Equals(entity.Name));
            if (isNameExists)
            {
                throw new InvalidOperationException("You can't add a new venue. This venue name alredy exist");
            }

            var venue = await _venueRepository.AddAsync(Mapper.Map<Venue>(entity));
            return Mapper.Map<VenueDto>(venue);
        }

        /// <summary>
        /// Logic for edit venue.
        /// </summary>
        /// <param name="entity">Object of class venue.</param>
        public async Task<bool> EditAsync(VenueDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            _validator.ValidateId(entity.Id);
            var allVenues = await _venueEFRepository.GetAllAsync();
            var isNameExists = allVenues.Any(venueName => venueName.Name.Equals(entity.Name));
            var isNameAndIdExists = allVenues.Any(name => name.Name.Equals(entity.Name) && name.Id.Equals(entity.Id));
            if (!isNameExists || isNameAndIdExists)
            {
                return await _venueRepository.EditAsync(Mapper.Map<Venue>(entity));
            }
            else
            {
                throw new InvalidOperationException("You can't edit venue. This venue name alredy exist");
            }
        }

        /// <summary>
        /// Logic for delete venue.
        /// </summary>
        /// <param name="id">Id of venue object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            await DeleteAllForVenue(id);
            return await _venueRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for get venue by id.
        /// </summary>
        /// <param name="id">Id of venue object.</param>
        public async Task<VenueDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<VenueDto>(await _venueRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all venue.
        /// </summary>
        /// <returns>Collection of venue object.</returns>
        public async Task<IEnumerable<VenueDto>> GetAllAsync()
        {
            var venues = await _venueEFRepository.GetAllAsync();
            return venues.Select(venue => Mapper.Map<VenueDto>(venue)).AsEnumerable();
        }

        private async Task DeleteAllForVenue(int venueId)
        {
            await GetInformationForDeleteVenue(venueId);
            var layoutsInVenue = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(venueId));
            foreach (var layoutInVenue in layoutsInVenue.ToList())
            {
                await DeleteEventWithSeatsAndAreas(layoutInVenue.Id);
                await DeleteSeatsAndAreas(layoutInVenue.Id);
                await _layoutRepository.DeleteAsync(layoutInVenue.Id);
            }
        }

        private async Task DeleteSeatsAndAreas(int layoutId)
        {
            var areas = await _areaEFRepository.GetAsync(area => area.LayoutId.Equals(layoutId));
            foreach (var area in areas.ToList())
            {
                var seats = await _seatEFRepository.GetAsync(seat => seat.AreaId.Equals(area.Id));
                foreach (var seat in seats.ToList())
                {
                    await _seatRepository.DeleteAsync(seat.Id);
                }

                await _areaRepository.DeleteAsync(area.Id);
            }
        }

        private async Task DeleteEventWithSeatsAndAreas(int layoutId)
        {
            var eventsForDelete = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(layoutId));
            foreach (var eventForDelete in eventsForDelete.ToList())
            {
                var eventAreasForDelete = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(eventForDelete.Id));
                foreach (var eventAreaForDelete in eventAreasForDelete.ToList())
                {
                    var seatsForDelete = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(eventAreaForDelete.Id));
                    foreach (var seatForDelete in seatsForDelete.ToList())
                    {
                        await _eventSeatRepository.DeleteAsync(seatForDelete.Id);
                    }

                    await _eventAreaRepository.DeleteAsync(eventAreaForDelete.Id);
                }

                await _eventRepository.DeleteAsync(eventForDelete.Id);
            }
        }

        private async Task GetInformationForDeleteVenue(int venueId)
        {
            var layoutsInVenue = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(venueId));
            foreach (var layoutInVenue in layoutsInVenue.ToList())
            {
                var eventsForDelete = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(layoutInVenue.Id));
                foreach (var eventForDelete in eventsForDelete.ToList())
                {
                    await GetInformationForDeleteEvent(eventForDelete.Id);
                }
            }
        }

        private async Task GetInformationForDeleteEvent(int eventId)
        {
            var eventAreasForDelete = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(eventId));
            foreach (var eventAreaForDelete in eventAreasForDelete.ToList())
            {
                var seatsForDelete = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.EventAreaId.Equals(eventAreaForDelete.Id));

                if (seatsForDelete.Any(bookedSeat => bookedSeat.State.Equals(EventSeatState.Booked)))
                {
                    throw new InvalidOperationException("You can't delete venue. Any seats booked");
                }
            }
        }
    }
}

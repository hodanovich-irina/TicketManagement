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
    /// Providing methods for managing layout with validation and business logic.
    /// </summary>
    internal class LayoutService : AutoMapperService, IService<LayoutDto>
    {
        private readonly IEFRepository<Layout> _layoutEFRepository;
        private readonly IRepository<Layout> _layoutRepository;
        private readonly IRepository<Area> _areaRepository;
        private readonly IEFRepository<Area> _areaEFRepository;
        private readonly IRepository<Seat> _seatRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<EventSeat> _eventSeatRepository;
        private readonly IRepository<EventArea> _eventAreaRepository;
        private readonly IEFRepository<Seat> _seatEFRepository;
        private readonly IEFRepository<Event> _eventEFRepository;
        private readonly IEFRepository<EventSeat> _eventSeatEFRepository;
        private readonly IEFRepository<EventArea> _eventAreaEFRepository;
        private readonly IValidator<LayoutDto> _validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutService"/> class.
        /// </summary>
        /// <param name="layoutEFRepository">Object of layout ef repository.</param>
        /// <param name="layoutRepository">Object of layout repository.</param>
        /// <param name="areaRepository">Object of area repository.</param>
        /// <param name="seatRepository">Object of seat repository.</param>
        /// <param name="eventRepository">Object of event repository.</param>
        /// <param name="eventSeatRepository">Object of event seat repository.</param>
        /// <param name="eventAreaRepository">Object of event area repository.</param>
        /// <param name="seatEFRepository">Object of seat ef repository.</param>
        /// <param name="eventEFRepository">Object of event ef repository.</param>
        /// <param name="eventSeatEFRepository">Object of event seat ef repository.</param>
        /// <param name="eventAreaEFRepository">Object of  event area ef repository.</param>
        /// <param name="areaEFRepository">Object of area ef repository.</param>
        /// <param name="validator">Object of layout validator.</param>
#pragma warning disable S107 // Methods should not have too many parameters
        public LayoutService(IEFRepository<Layout> layoutEFRepository, IRepository<Layout> layoutRepository, IRepository<Area> areaRepository, IRepository<Seat> seatRepository,
            IRepository<Event> eventRepository, IRepository<EventSeat> eventSeatRepository, IRepository<EventArea> eventAreaRepository,  IEFRepository<Seat> seatEFRepository,
            IEFRepository<Event> eventEFRepository, IEFRepository<EventSeat> eventSeatEFRepository, IEFRepository<EventArea> eventAreaEFRepository, IEFRepository<Area> areaEFRepository,
            IValidator<LayoutDto> validator)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            _layoutEFRepository = layoutEFRepository;
            _layoutRepository = layoutRepository;
            _areaRepository = areaRepository;
            _eventRepository = eventRepository;
            _eventAreaRepository = eventAreaRepository;
            _eventSeatRepository = eventSeatRepository;
            _seatRepository = seatRepository;
            _areaEFRepository = areaEFRepository;
            _eventEFRepository = eventEFRepository;
            _eventAreaEFRepository = eventAreaEFRepository;
            _eventSeatEFRepository = eventSeatEFRepository;
            _seatEFRepository = seatEFRepository;
            _validator = validator;
        }

        /// <summary>
        /// Logic for add layout.
        /// </summary>
        /// <param name="entity">Object of class layout.</param>
        public async Task<LayoutDto> AddAsync(LayoutDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            var layouts = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(entity.VenueId));
            var isNameExists = layouts.Any(layoutN => layoutN.Name.Equals(entity.Name));
            if (isNameExists)
            {
                throw new InvalidOperationException("You can't add a new layuot. Layout with this name alredy exist in this venue");
            }

            var layout = await _layoutRepository.AddAsync(Mapper.Map<Layout>(entity));
            return Mapper.Map<LayoutDto>(layout);
        }

        /// <summary>
        /// Logic for edit layout.
        /// </summary>
        /// <param name="entity">Object of class layout.</param>
        public async Task<bool> EditAsync(LayoutDto entity)
        {
            _validator.ValidationBeforeAddAndEdit(entity);
            _validator.ValidateId(entity.Id);
            var layouts = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(entity.VenueId));
            var isNameExists = layouts.Any(layoutN => layoutN.Name.Equals(entity.Name));
            var layoutsInVenue = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(entity.VenueId));
            var isNameAndIdExists = layoutsInVenue.Any(layoutN => layoutN.Name.Equals(entity.Name) && layoutN.Id.Equals(entity.Id));
            if (!isNameExists || isNameAndIdExists)
            {
                return await _layoutRepository.EditAsync(Mapper.Map<Layout>(entity));
            }
            else
            {
                throw new InvalidOperationException("You can't edit a layuot. Layout with this name alredy exist in this venue");
            }
        }

        /// <summary>
        /// Logic for delete layout.
        /// </summary>
        /// <param name="id">Id of layout object.</param>
        public async Task<bool> DeleteAsync(int id)
        {
            _validator.ValidateId(id);
            await DeleteEventWithSeatsAndAreas(id);
            await DeleteSeatsAndAreas(id);
            return await _layoutRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Logic for get layout by id.
        /// </summary>
        /// <param name="id">Id of layout object.</param>
        public async Task<LayoutDto> GetByIdAsync(int id)
        {
            _validator.ValidateId(id);
            return Mapper.Map<LayoutDto>(await _layoutRepository.GetByIdAsync(id));
        }

        /// <summary>
        /// Logic for get all layout.
        /// </summary>
        /// <param name="id">Venue Id of layout object.</param>
        /// <returns>Collection of layout object.</returns>
        public async Task<IEnumerable<LayoutDto>> GetAsync(int id)
        {
            _validator.ValidateId(id);
            var layouts = await _layoutEFRepository.GetAsync(layout => layout.VenueId.Equals(id));
            return layouts.Select(layout => Mapper.Map<LayoutDto>(layout)).AsEnumerable();
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
            await GetInformationForDeleteLayout(layoutId);
            var eventsForDelete = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(layoutId));
            foreach (var eventForDelete in eventsForDelete.ToList())
            {
                var eventAreasForDelete = await _eventAreaEFRepository.GetAsync(eventArea => eventArea.EventId.Equals(eventForDelete.Id));
                foreach (var eventAreaForDelete in eventAreasForDelete.ToList())
                {
                    var seatsForDelete = await _eventSeatEFRepository.GetAsync(eventSeat => eventSeat.Equals(eventAreaForDelete.Id));
                    foreach (var seatForDelete in seatsForDelete.ToList())
                    {
                        await _eventSeatRepository.DeleteAsync(seatForDelete.Id);
                    }

                    await _eventAreaRepository.DeleteAsync(eventAreaForDelete.Id);
                }

                await _eventRepository.DeleteAsync(eventForDelete.Id);
            }
        }

        private async Task GetInformationForDeleteLayout(int layoutId)
        {
            var eventsForDelete = await _eventEFRepository.GetAsync(eventInLayout => eventInLayout.LayoutId.Equals(layoutId));
            foreach (var eventForDelete in eventsForDelete.ToList())
            {
                await GetInformationForDeleteEvent(eventForDelete.Id);
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
                    throw new InvalidOperationException("You can't delete layout. Any seats booked");
                }
            }
        }

        /// <summary>
        /// Logic for get all layout.
        /// </summary>
        /// <returns>Collection of event layout.</returns>
        public async Task<IEnumerable<LayoutDto>> GetAllAsync()
        {
            var layouts = await _layoutEFRepository.GetAllAsync();
            return layouts.Select(layout => Mapper.Map<LayoutDto>(layout)).AsEnumerable();
        }
    }
}

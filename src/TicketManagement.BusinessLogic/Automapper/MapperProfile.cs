using System;
using AutoMapper;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.BusinessLogic.Automapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Area, AreaDto>();
            CreateMap<EventArea, EventAreaDto>();
            CreateMap<Event, EventDto>();
            CreateMap<EventSeat, EventSeatDto>();
            CreateMap<Layout, LayoutDto>();
            CreateMap<Seat, SeatDto>();
            CreateMap<Ticket, TicketDto>();
            CreateMap<Venue, VenueDto>();
            CreateMap<AreaDto, Area>();
            CreateMap<EventAreaDto, EventArea>();
            CreateMap<EventDto, Event>();
            CreateMap<EventSeatDto, EventSeat>();
            CreateMap<LayoutDto, Layout>();
            CreateMap<SeatDto, Seat>();
            CreateMap<TicketDto, Ticket>();
            CreateMap<VenueDto, Venue>();
            CreateMap<EventSeatState, EventSeatStateDto>();
            CreateMap<EventSeatStateDto, EventSeatState>();
        }
    }
}

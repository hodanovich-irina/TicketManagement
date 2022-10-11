using System;
using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.EventAPI.Dto;

namespace TicketManagement.EventAPI.Automapper
{
    /// <summary>
    /// Class for mapper profile.
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<EventArea, EventAreaDto>();
            CreateMap<Event, EventDto>();
            CreateMap<EventSeat, EventSeatDto>();
            CreateMap<Venue, VenueDto>();
            CreateMap<Layout, LayoutDto>();
            CreateMap<EventAreaDto, EventArea>();
            CreateMap<EventDto, Event>();
            CreateMap<EventSeatDto, EventSeat>();
            CreateMap<VenueDto, Venue>();
            CreateMap<LayoutDto, Layout>();
            CreateMap<EventSeatState, EventSeatStateDto>();
            CreateMap<EventSeatStateDto, EventSeatState>();
        }
    }
}

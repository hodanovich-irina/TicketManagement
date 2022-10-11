using System;
using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.VenueAPI.Dto;

namespace TicketManagement.VenueAPI.Automapper
{
    /// <summary>
    /// Class for mapper profile.
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Area, AreaDto>();
            CreateMap<Layout, LayoutDto>();
            CreateMap<Seat, SeatDto>();
            CreateMap<Venue, VenueDto>();
            CreateMap<AreaDto, Area>();
            CreateMap<LayoutDto, Layout>();
            CreateMap<SeatDto, Seat>();
            CreateMap<VenueDto, Venue>();
        }
    }
}

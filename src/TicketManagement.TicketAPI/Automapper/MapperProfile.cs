using System;
using AutoMapper;
using TicketManagement.DataAccess.Models;
using TicketManagement.TicketAPI.Dto;

namespace TicketManagement.TicketAPI.Automapper
{
    /// <summary>
    /// Class for mapper profile.
    /// </summary>
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Ticket, TicketDto>();
            CreateMap<TicketDto, Ticket>();
        }
    }
}

using AutoMapper;

namespace TicketManagement.VenueAPI.Automapper
{
    /// <summary>
    /// Interface for mapper.
    /// </summary>
    internal interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}

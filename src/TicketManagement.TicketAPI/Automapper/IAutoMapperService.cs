using AutoMapper;

namespace TicketManagement.TicketAPI.Automapper
{
    /// <summary>
    /// Interface for mapper.
    /// </summary>
    internal interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}

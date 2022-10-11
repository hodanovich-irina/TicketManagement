using AutoMapper;

namespace TicketManagement.EventAPI.Automapper
{
    /// <summary>
    /// Interface for mapper.
    /// </summary>
    internal interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}

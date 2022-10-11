using AutoMapper;

namespace TicketManagement.BusinessLogic.Automapper
{
    internal interface IAutoMapperService
    {
        IMapper Mapper { get; }
    }
}

using AutoMapper;

namespace TicketManagement.EventAPI.Automapper
{
    /// <summary>
    /// Automapper.
    /// </summary>
    public class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper
        {
            get { return ObjectMapper.Mapper; }
        }
    }
}

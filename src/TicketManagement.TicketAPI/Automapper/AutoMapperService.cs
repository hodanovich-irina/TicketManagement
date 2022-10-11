using AutoMapper;

namespace TicketManagement.TicketAPI.Automapper
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

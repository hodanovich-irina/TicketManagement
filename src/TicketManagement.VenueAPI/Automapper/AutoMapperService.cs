using AutoMapper;

namespace TicketManagement.VenueAPI.Automapper
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

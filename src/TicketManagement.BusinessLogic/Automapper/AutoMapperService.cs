using AutoMapper;

namespace TicketManagement.BusinessLogic.Automapper
{
    public class AutoMapperService : IAutoMapperService
    {
        public IMapper Mapper
        {
            get { return ObjectMapper.Mapper; }
        }
    }
}

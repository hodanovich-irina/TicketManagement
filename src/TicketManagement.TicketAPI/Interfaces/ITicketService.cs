using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.TicketAPI.Dto;

namespace TicketManagement.TicketAPI.Interfaces
{
    /// <summary>
    /// Interface for work with ticket services.
    /// </summary>
    public interface ITicketService : IService<TicketDto>
    {
        Task<IEnumerable<TicketDto>> GetByParentStringIdAsync(string id);

        Task<IEnumerable<TicketInfo>> GetUserTicketInfo(string id);
    }
}

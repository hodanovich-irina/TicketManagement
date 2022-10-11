using System.Collections.Generic;
using System.Threading.Tasks;
using TicketManagement.BusinessLogic.ModelsDTO;

namespace TicketManagement.BusinessLogic.Interfaces
{
    /// <summary>
    /// Interface for work with ticket services.
    /// </summary>
    public interface ITicketService : IService<TicketDto>
    {
        Task<IEnumerable<TicketDto>> GetByParentStringIdAsync(string id);
    }
}

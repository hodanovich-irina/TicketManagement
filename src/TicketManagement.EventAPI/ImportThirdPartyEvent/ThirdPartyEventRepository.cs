using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TicketManagement.EventAPI.Dto;

namespace TicketManagement.EventAPI.ImportThirdPartyEvent
{
    /// <summary>
    /// Providing methods for managing third party event.
    /// </summary>
    public class ThirdPartyEventRepository : IThirdPartyEventRepository
    {
        /// <summary>
        /// Method for read json file with third party event.
        /// </summary>
        /// <param name="path">path of file.</param>
        /// <returns>collection of third party event.</returns>
        public IEnumerable<ThirdPartyEventViewModel> Read(string path)
        {
            if (!File.Exists(path))
            {
                throw new InvalidOperationException("File doesn't exsist!");
            }

            var fs = File.ReadAllText(path);
            return JsonSerializer.Deserialize<IEnumerable<ThirdPartyEventViewModel>>(fs);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Web.Configuration;
using System.Web.Hosting;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
    /// <summary>
    /// Class for third party event repository.
    /// </summary>
    internal class ThirdPartyEventRepository : IRepository<ThirdPartyEvent>
    {
        private readonly string _fileName;
        public static readonly object logsLock = new object();

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ThirdPartyEventRepository() 
        {
            _fileName = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["MyDatabase"]);
            if (!File.Exists(_fileName)) 
            {
                Create(); 
            }
        }

        /// <summary>
        /// Method for read data.
        /// </summary>
        /// <returns>collection of object.</returns>
        public IEnumerable<ThirdPartyEvent> Read()
        {
            IEnumerable<ThirdPartyEvent> _events = null;
            lock (logsLock)
            {
                var fs = File.ReadAllText(_fileName);
                _events = JsonSerializer.Deserialize<IEnumerable<ThirdPartyEvent>>(fs);
            }
            return _events;
        }

        /// <summary>
        /// Method for write data.
        /// </summary>
        /// <param name="entity">entity.</param>
        public void Write(IEnumerable<ThirdPartyEvent> entities)
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            lock (logsLock)
            {
                var jsonString = JsonSerializer.Serialize(entities, options);
                File.WriteAllText(_fileName, jsonString);
            }
        }

        private void Create()
        {
            var _events = new List<ThirdPartyEvent>(){};
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            
            lock (logsLock)
            {
                var jsonString = JsonSerializer.Serialize(_events, options);
                File.WriteAllText(_fileName, jsonString);
            }
        }
    }
}
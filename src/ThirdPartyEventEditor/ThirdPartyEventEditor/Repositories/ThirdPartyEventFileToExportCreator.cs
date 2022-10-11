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
    /// Class for file work.
    /// </summary>
    internal class ThirdPartyEventFileToExportCreator : IThirdPartyEventFileToExportCreator<ThirdPartyEvent>
    {
        private readonly string _path;
        public static readonly object logsLock = new object();
        /// <summary>
        /// Empty constructor.
        /// </summary>
        public ThirdPartyEventFileToExportCreator()
        {
            _path = HostingEnvironment.MapPath(WebConfigurationManager.AppSettings["MyImport"]);
        }

        /// <summary>
        /// Method for write data.
        /// </summary>
        /// <param name="entities">entity.</param>
        /// <param name="fileName">file name.</param>
        public void Write(IEnumerable<ThirdPartyEvent> entities, string fileName)
        {
            var fullPath = Path.Combine(_path, fileName + WebConfigurationManager.AppSettings["FileFormat"]);
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            lock (logsLock)
            {
                var jsonString = JsonSerializer.Serialize(entities, options);
                File.WriteAllText(fullPath, jsonString);
            }
        }

        /// <summary>
        /// Method for create file.
        /// </summary>
        /// <param name="fileName">file name</param>
        public void Create(string fileName)
        {
            var fullPath = Path.Combine(_path, fileName + WebConfigurationManager.AppSettings["FileFormat"]);
            if (!File.Exists(fullPath))
            {
                var _events = new List<ThirdPartyEvent>() { };
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };

                lock (logsLock)
                {
                    var jsonString = JsonSerializer.Serialize(_events, options);
                    File.WriteAllText(fullPath, jsonString);
                }
            }
        }
    }
}
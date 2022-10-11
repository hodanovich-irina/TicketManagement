using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with third party.
    /// </summary>
    [Authorize(Roles = Role.EventManager)]
    public class ThirdPartyImportController : Controller
    {
        private readonly IEventRestClient _eventRestClient;

        public ThirdPartyImportController(IEventRestClient eventRestClient)
        {
            _eventRestClient = eventRestClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Metjod for upload file.
        /// </summary>
        /// <param name="uploadedFile">uploaded file.</param>
        /// <returns>action result.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Index(IFormFile uploadedFile)
        {
            using (var reader = new System.IO.BinaryReader(uploadedFile.OpenReadStream()))
            {
                var data = reader.ReadBytes((int)uploadedFile.OpenReadStream().Length);

                using var content = new MultipartFormDataContent
                {
                    { new ByteArrayContent(data), "uploadedFile", uploadedFile.FileName },
                };
                await _eventRestClient.UploadFile(content, HttpContext.Request.Cookies["secret_jwt_key"]);
            }

            return Redirect("~/Home/Index");
        }
    }
}

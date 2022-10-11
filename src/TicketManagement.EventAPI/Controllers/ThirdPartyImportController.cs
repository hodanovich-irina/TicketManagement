using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.EventAPI.ImportThirdPartyEvent;
using TicketManagement.EventAPI.RoleData;

namespace TicketManagement.EventAPI.Controllers
{
    /// <summary>
    /// Controller for work with third party events.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = Role.EventManager)]
    public class ThirdPartyImportController : Controller
    {
        private readonly IThirdPartyEventService _thirdPartyEventService;

        public ThirdPartyImportController(IThirdPartyEventService thirdPartyEventService)
        {
            _thirdPartyEventService = thirdPartyEventService;
        }

        /// <summary>
        /// Method for upload file.
        /// </summary>
        /// <param name="uploadedFile">uploaded file.</param>
        /// <returns> action result.</returns>
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile uploadedFile)
        {
            try
            {
                await _thirdPartyEventService.AddEvent(uploadedFile);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Filters;
using TicketManagement.Presentation.Models;
using TicketManagement.Presentation.RoleData;

namespace TicketManagement.Presentation.Controllers
{
    /// <summary>
    /// Controller for work with seats.
    /// </summary>
    [Authorize(Roles = Role.Admin + ", " + Role.VenueManager)]
    public class SeatController : Controller
    {
        private readonly IVenueRestClient _venueClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeatController"/> class.
        /// Class constructor.
        /// </summary>
        /// <param name="venueClient"> venue client.</param>
        public SeatController(IVenueRestClient venueClient)
        {
            _venueClient = venueClient;
        }

        /// <summary>
        /// Method for select seat data.
        /// </summary>
        /// <param name="id">area id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Index(int id)
        {
            var seats = await _venueClient.GetSeatByParentIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var area = await _venueClient.GetAreaByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var layout = await _venueClient.GetLayoutByIdAsync(area.LayoutId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var seatsWithArea = new List<SeatViewModel>();
            foreach (var seat in seats)
            {
                seatsWithArea.Add(new SeatViewModel
                {
                    Id = seat.Id,
                    Number = seat.Number,
                    Row = seat.Row,
                    LayoutName = layout.Name,
                    AreaId = seat.AreaId,
                    AreaY = area.CoordY,
                    AreaX = area.CoordX,
                });
            }

            return View(seatsWithArea);
        }

        /// <summary>
        /// Method for create seat data.
        /// </summary>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Create()
        {
            var areas = await _venueClient.GetAllAreaAsync(HttpContext.Request.Cookies["secret_jwt_key"]);
            areas.Select(area => new AreaDto
            {
                Id = area.Id,
                Description = area.Description,
            });
            return View(new SeatViewModel { Areas = areas.ToList() });
        }

        /// <summary>
        /// Method for create seat data.
        /// </summary>
        /// <param name="seat">Object of seat.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Create(SeatViewModel seat)
        {
            var model = new SeatDto
            {
                Id = seat.Id,
                Number = seat.Number,
                Row = seat.Row,
                AreaId = seat.AreaId,
            };
            await _venueClient.AddSeatAsync(model, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Seat/Index?id={seat.AreaId}");
        }

        /// <summary>
        /// Method for edit seat data.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <returns>view result.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var areas = await _venueClient.GetAllAreaAsync(HttpContext.Request.Cookies["secret_jwt_key"]);

            var seatToEdit = await _venueClient.GetSeatByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);

            var model = new SeatViewModel
            {
                Id = seatToEdit.Id,
                Row = seatToEdit.Row,
                Number = seatToEdit.Number,
                Areas = areas.ToList(),
                AreaId = seatToEdit.AreaId,
            };
            return View(model);
        }

        /// <summary>
        /// Method for edit seat data.
        /// </summary>
        /// <param name="seatDto">Object of seat.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Edit(SeatDto seatDto)
        {
            await _venueClient.EditSeatAsync(seatDto, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Seat/Index?id={seatDto.AreaId}");
        }

        /// <summary>
        /// Method for delete seat data.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <returns>view result.</returns>
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var seat = await _venueClient.GetSeatByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            var area = await _venueClient.GetAreaByIdAsync(seat.AreaId, HttpContext.Request.Cookies["secret_jwt_key"]);
            var seatToDelete = new SeatViewModel
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                AreaId = seat.AreaId,
                AreaX = area.CoordX,
                AreaY = area.CoordY,
            };
            return View(seatToDelete);
        }

        /// <summary>
        /// Method for delete seat data.
        /// </summary>
        /// <param name="id">seat id.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        [ValidationExceptionFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var seat = await _venueClient.GetSeatByIdAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            await _venueClient.DeleteSeatAsync(id, HttpContext.Request.Cookies["secret_jwt_key"]);
            return Redirect($"~/Seat/Index?id={seat.AreaId}");
        }
    }
}

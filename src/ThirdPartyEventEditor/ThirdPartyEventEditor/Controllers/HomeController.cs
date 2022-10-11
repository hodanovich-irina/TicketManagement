using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Controllers
{
    [LogActionFilter]
    [ValidationExceptionFilterAttribute]
    /// <summary>
    /// Controller for work with third party events.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IService<ThirdPartyEvent> _service;
        /// <summary>
        /// Constructor with params.
        /// </summary>
        /// <param name="service">third party event service.</param>
        public HomeController(IService<ThirdPartyEvent> service)
        {
            _service = service;
        }

        /// <summary>
        /// Method for select third party event data.
        /// </summary>
        /// <returns>view result.</returns>
        public ActionResult Index()
        {
            var events = _service.GetAll();
            return View(events);
        }

        /// <summary>
        /// Method for create event data.
        /// </summary>
        /// <returns>view result.</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Method for create event data.
        /// </summary>
        /// <param name="thirdPartyEvent">object of third party event.</param>
        /// <returns>redirect to action</returns>
        [HttpPost]
        public async Task<ActionResult> Create(ThirdPartyEvent thirdPartyEvent)
        {
            await _service.Add(thirdPartyEvent);
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Method for edit third party event data.
        /// </summary>
        /// <param name="id">third party event id.</param>
        /// <returns>view result.</returns>
        public ActionResult Edit(int id)
        {
            var thirdPartyEvent = _service.GetById(id);
            return View(thirdPartyEvent);
        }

        /// <summary>
        /// Method for edit third party event data.
        /// </summary>
        /// <param name="thirdPartyEvent">Object of third party event.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        public async Task<ActionResult> Edit(ThirdPartyEvent thirdPartyEvent)
        {
            await _service.Edit(thirdPartyEvent);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for delete third party event data.
        /// </summary>
        /// <param name="id">third party event id.</param>
        /// <returns>view result.</returns>
        [HttpGet]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var _event = _service.GetById(id);
            return View(_event);
        }

        /// <summary>
        /// Method for delete select third party event data.
        /// </summary>
        /// <param name="id">select third party event id.</param>
        /// <returns>redirect to action.</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _service.Delete(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for get third party event details.
        /// </summary>
        /// <returns>view result.</returns>
        public ActionResult Details(int id)
        {
            var thirdPartyEvent = _service.GetById(id);
            return View(thirdPartyEvent);
        }

        /// <summary>
        /// Method for import file.
        /// </summary>
        /// <returns>action result</returns>
        public ActionResult FileImport() 
        {
            var events = _service.GetAll();
            return View(events);
        }

        /// <summary>
        /// Method for choose import files.
        /// </summary>
        /// <param name="selectId">collection of selected id.</param>
        /// <param name="fileName">name of file import.</param>
        /// <returns>redirect to action</returns>
        [HttpPost]
        public ActionResult FileImport(IList<int> selectId, string fileName) 
        {
            if (selectId != null)
            {
                _service.CreateImportFile(fileName, selectId);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Method for catch exception.
        /// </summary>
        /// <param name="message">error message.</param>
        /// <returns>view result.</returns>
        public ViewResult ShowError(string message)
        {
            ViewData["Message"] = message;
            return View();
        }
    }
}
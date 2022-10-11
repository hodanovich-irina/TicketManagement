using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TicketManagement.Presentation.Client;
using TicketManagement.Presentation.Dto;
using TicketManagement.Presentation.Models;

namespace TicketManagement.Presentation.ImportThirdPartyEvent
{
    /// <summary>
    /// Providing methods for managing area with validation and some logic.
    /// </summary>
    public class ThirdPartyEventService : IThirdPartyEventService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IEventRestClient _eventRestClient;
        private readonly IThirdPartyEventRepository _thirdPartyEvenetRepository;
        private readonly IHttpContextAccessor _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventService"/> class.
        /// </summary>
        /// <param name="hostEnvironment">web host enviroment.</param>
        /// <param name="configuration">configuration.</param>
        /// <param name="eventRestClient">event rest client.</param>
        /// <param name="thirdPartyEvenetRepository">third party evenet repository.</param>
        /// <param name="context">http context. </param>
        public ThirdPartyEventService(IWebHostEnvironment hostEnvironment, IConfiguration configuration, IEventRestClient eventRestClient,
            IThirdPartyEventRepository thirdPartyEvenetRepository, IHttpContextAccessor context)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _eventRestClient = eventRestClient;
            _thirdPartyEvenetRepository = thirdPartyEvenetRepository;
            _context = context;
        }

        /// <summary>
        /// Method for add event.
        /// </summary>
        /// <param name="uploadedFile">upload third party events file.</param>
        public async Task AddEvent(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = Path.Combine(_hostEnvironment.WebRootPath, _configuration["FileFolder"], uploadedFile.FileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var events = await GetEvent(path);
                var flag = false;
                foreach (var addEvent in events)
                {
                    if (!(addEvent.Value is null))
                    {
                        await _eventRestClient.AddEventAsync(ConvertToEventModel(addEvent.Value), _context.HttpContext.Request.Cookies["secret_jwt_key"]);
                    }
                    else
                    {
                        flag = true;
                    }
                }

                if (flag)
                {
                    throw new InvalidOperationException("Warning! Events from the file are not added or not all are added...");
                }
            }
        }

        private async Task<Dictionary<string, EventDto>> GetEvent(string path)
        {
            var importedDictionary = new Dictionary<string, EventDto>();
            var venues = await _eventRestClient.GetAllVenueAsync(_context.HttpContext.Request.Cookies["secret_jwt_key"]);
            var layouts = await _eventRestClient.GetAllLayoutAsync(_context.HttpContext.Request.Cookies["secret_jwt_key"]);
            var importedEvent = new List<EventDto>();
            var eventsToConvert = _thirdPartyEvenetRepository.Read(path);
            var x = 0;
            foreach (var eventToConvert in eventsToConvert)
            {
                x += 1;
                var trueVenue = venues.FirstOrDefault(x => x.Name.Equals(eventToConvert.VenueName));
                var trueLayout = layouts.FirstOrDefault(x => x.Name.Equals(eventToConvert.LayoutName));
                var trueEvent = ConvertIsValid(trueVenue, trueLayout);
                if (trueEvent.TrueEvent)
                {
                    var eventConverted = ConvertEvent(eventToConvert, trueEvent, trueLayout.Id);
                    importedEvent.Add(eventConverted);
                    importedDictionary.Add(trueEvent.TrueEvent.ToString() + x, eventConverted);
                }
                else
                {
                    importedDictionary.Add(trueEvent.TrueEvent.ToString() + x, null);
                }
            }

            return importedDictionary;
        }

        private EventDto ConvertEvent(ThirdPartyEventViewModel jsonEvent, ConvertDataViewModel data, int layoutId)
        {
            var convertEvent = new EventDto
            {
                BaseAreaPrice = data.BaseAreaPrice,
                DateEnd = jsonEvent.EndDate,
                DateStart = jsonEvent.StartDate,
                Description = jsonEvent.Description,
                ImageURL = jsonEvent.PosterImage.UploadSampleImage(_hostEnvironment, _configuration),
                LayoutId = layoutId,
                Name = jsonEvent.Name,
                ShowTime = jsonEvent.StartDate.TimeOfDay,
            };

            return convertEvent;
        }

        private ConvertDataViewModel ConvertIsValid(VenueDto trueVenue, LayoutDto trueLayout)
        {
            var trueEvent = true;
            if ((trueVenue is null) || (trueLayout is null))
            {
                trueEvent = false;
            }

            var data = new ConvertDataViewModel
            {
                TrueEvent = trueEvent,
                BaseAreaPrice = decimal.Zero,
            };
            return data;
        }

        private EventModel ConvertToEventModel(EventDto eventDto)
        {
            var convertEvent = new EventModel
            {
                BaseAreaPrice = eventDto.BaseAreaPrice,
                DateEnd = eventDto.DateEnd,
                DateStart = eventDto.DateStart,
                Description = eventDto.Description,
                ImageURL = eventDto.ImageURL,
                LayoutId = eventDto.LayoutId,
                Name = eventDto.Name,
                Hours = eventDto.ShowTime.Hours,
                Minutes = eventDto.ShowTime.Minutes,
                Seconds = eventDto.ShowTime.Seconds,
            };

            return convertEvent;
        }
    }
}
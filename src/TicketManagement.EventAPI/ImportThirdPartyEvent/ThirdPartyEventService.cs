using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.Interfaces;

namespace TicketManagement.EventAPI.ImportThirdPartyEvent
{
    /// <summary>
    /// Providing methods for managing area with validation and some logic.
    /// </summary>
    public class ThirdPartyEventService : IThirdPartyEventService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IService<LayoutDto> _layoutService;
        private readonly IService<EventDto> _eventService;
        private readonly IVenueService _venueService;
        private readonly IThirdPartyEventRepository _thirdPartyEvenetRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThirdPartyEventService"/> class.
        /// </summary>
        /// <param name="hostEnvironment">web host enviroment.</param>
        /// <param name="configuration">configuration.</param>
        /// <param name="layoutService">layout service.</param>
        /// <param name="venueService">venue service.</param>
        /// <param name="thirdPartyEvenetRepository">third party evenet repository.</param>
        /// <param name="eventService">event service.</param>
        public ThirdPartyEventService(IWebHostEnvironment hostEnvironment, IConfiguration configuration, IService<LayoutDto> layoutService,
            IVenueService venueService, IThirdPartyEventRepository thirdPartyEvenetRepository, IService<EventDto> eventService)
        {
            _hostEnvironment = hostEnvironment;
            _configuration = configuration;
            _layoutService = layoutService;
            _venueService = venueService;
            _thirdPartyEvenetRepository = thirdPartyEvenetRepository;
            _eventService = eventService;
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
                        await _eventService.AddAsync(addEvent.Value);
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
            var venues = await _venueService.GetAllAsync();
            var layouts = await _layoutService.GetAllAsync();
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
    }
}

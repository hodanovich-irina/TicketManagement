using System;
using ThirdPartyEventEditor.Exceptions;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Validators
{
    /// <summary>
    /// Validate properties for third party event.
    /// </summary>
    public class ThirdPartyEventValidator : IValidator<ThirdPartyEvent>
    {
        /// <summary>
        /// Method for validity check object before add and edit.
        /// </summary>
        /// <param name="eventForWork">object of third party event.</param>
        public void IsValid(ThirdPartyEvent eventForWork)
        {
            if (eventForWork is null)
            {
                throw new ValidationException("Third party event was null");
            }

            if (eventForWork.LayoutName is null)
            {
                throw new ValidationException("Layout name of third party event must be not null");
            }
            if (eventForWork.VenueName is null)
            {
                throw new ValidationException("Venue name of third party event must be not null");
            }

            if (eventForWork.Name is null || eventForWork.Name.Length > 120)
            {
                throw new ValidationException("Name of third party event must be less than 120 sumbols and must be not null");
            }

            if (eventForWork.Description is null)
            {
                throw new ValidationException("Description of third party event must be not null");
            }

            if (eventForWork.PosterImage is null)
            {
                throw new ValidationException("Image url of third party event must be not null");
            }

            if (eventForWork.EndDate < eventForWork.StartDate || eventForWork.StartDate < DateTime.Now)
            {
                throw new ValidationException("You can't create event in the past and when start date more than end date");
            }
        }
    }
}
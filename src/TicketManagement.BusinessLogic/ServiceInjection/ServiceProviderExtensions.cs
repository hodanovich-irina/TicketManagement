using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.BusinessLogic.Validations;
using TicketManagement.DataAccess.RepositoryInjection;

namespace TicketManagement.BusinessLogic.ServiceInjection
{
    /// <summary>
    /// Class for DI.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Method for registered service.
        /// </summary>
        /// <param name="services">Service.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Object that used to access the registered service.</returns>
        public static IServiceCollection AddService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRepository(configuration);

            services.AddScoped<IValidator<AreaDto>, AreaValidation>();
            services.AddScoped<IValidator<EventAreaDto>, EventAreaValidation>();
            services.AddScoped<IValidator<EventSeatDto>, EventSeatValidation>();
            services.AddScoped<IValidator<EventDto>, EventValidation>();
            services.AddScoped<IValidator<LayoutDto>, LayoutValidation>();
            services.AddScoped<IValidator<SeatDto>, SeatValidation>();
            services.AddScoped<IValidator<TicketDto>, TicketValidation>();
            services.AddScoped<IValidator<VenueDto>, VenueValidation>();

            services.AddScoped<IService<AreaDto>, AreaService>();
            services.AddScoped<IService<EventAreaDto>, EventAreaService>();
            services.AddScoped<IService<EventSeatDto>, EventSeatService>();
            services.AddScoped<IService<EventDto>, EventService>();
            services.AddScoped<IService<LayoutDto>, LayoutService>();
            services.AddScoped<IService<SeatDto>, SeatService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IVenueService, VenueService>();

            return services;
        }
    }
}

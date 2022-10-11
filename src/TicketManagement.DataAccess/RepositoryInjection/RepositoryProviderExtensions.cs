using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.DataAccess.RepositoryInjection
{
    /// <summary>
    /// Class for DI.
    /// </summary>
    public static class RepositoryProviderExtensions
    {
        /// <summary>
        /// Method for registered repository.
        /// </summary>
        /// <param name="services">Service.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Object that used to access the registered service.</returns>
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TicketManagementContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped(typeof(IEFRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEFRepository<Event>, EFEventRepository>();
            services.AddScoped<IRepository<Event>, EFEventRepository>();
            return services;
        }
    }
}

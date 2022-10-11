using System;
using AutoMapper;

namespace TicketManagement.TicketAPI.Automapper
{
    /// <summary>
    /// Class for mapper object.
    /// </summary>
    public class ObjectMapper : Profile
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var mapper = new Mapper(Configuration);
            return mapper;
        });

        private static Lazy<IConfigurationProvider> _config = new Lazy<IConfigurationProvider>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
            });

            return config;
        });

        public static IMapper Mapper
        {
            get { return _mapper.Value; }
        }

        public static IConfigurationProvider Configuration
        {
            get { return _config.Value; }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.BusinessLogic.Validations;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests.BusinessLogic.Services.IntegrationTests
{
    /// <summary>
    /// Testing of venue service.
    /// </summary>
    [TestFixture]
    public class VenueServiceTest
    {
        private string _connectionString;
        private AreaRepository _areaRepository;
        private Repository<Layout> _layoutEFRepository;
        private LayoutRepository _layoutRepository;
        private Repository<Area> _areaEFRepository;
        private SeatRepository _seatRepository;
        private Repository<Seat> _seatEFRepository;
        private EventAreaRepository _eventAreaRepository;
        private Repository<EventArea> _eventAreaEFRepository;
        private EventRepository _eventRepository;
        private Repository<Event> _eventEFRepository;
        private EventSeatRepository _eventSeatRepository;
        private Repository<EventSeat> _eventSeatEFRepository;
        private VenueRepository _venueRepository;
        private Repository<Venue> _venueEFRepository;
        private VenueValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _areaRepository = new AreaRepository(_connectionString);
            _areaEFRepository = new Repository<Area>(_context);
            _eventAreaRepository = new EventAreaRepository(_connectionString);
            _eventAreaEFRepository = new Repository<EventArea>(_context);
            _eventRepository = new EventRepository(_connectionString);
            _eventEFRepository = new Repository<Event>(_context);
            _layoutRepository = new LayoutRepository(_connectionString);
            _layoutEFRepository = new Repository<Layout>(_context);
            _venueRepository = new VenueRepository(_connectionString);
            _venueEFRepository = new Repository<Venue>(_context);
            _seatRepository = new SeatRepository(_connectionString);
            _seatEFRepository = new Repository<Seat>(_context);
            _eventSeatRepository = new EventSeatRepository(_connectionString);
            _eventSeatEFRepository = new Repository<EventSeat>(_context);
            _validator = new VenueValidation();
        }

        [Test]
        public async Task GetAllAsync_WhenVenueGet_ShouldReturnVenuesList()
        {
            // Arrange
            var service = new VenueService(_venueEFRepository, _venueRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository,
                _eventSeatRepository, _eventAreaRepository, _layoutEFRepository, _areaEFRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _validator);

            // Act
            var venues = await service.GetAllAsync();

            // Assert
            venues.Should().BeEquivalentTo(new List<VenueDto>
            {
                new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
            });
        }

        [Test]
        public async Task GetById_WhenVenueWithFirsId_ShouldReturnVenue()
        {
            // Arrange
            var venueId = 1;
            var service = new VenueService(_venueEFRepository, _venueRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository,
                _eventSeatRepository, _eventAreaRepository, _layoutEFRepository, _areaEFRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _validator);

            // Act
            var venue = await service.GetByIdAsync(venueId);

            // Assert
            venue.Should().BeEquivalentTo(new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" });
        }

        [Test]
        public async Task Add_WhenAddNewVenue_ShouldReturnVenuesListWithNewVenue()
        {
            // Arrange
            var venue = new VenueDto { Name = "Ttttt", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" };
            var service = new VenueService(_venueEFRepository, _venueRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository,
                _eventSeatRepository, _eventAreaRepository, _layoutEFRepository, _areaEFRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _validator);

            // Act
            var lastId = await service.AddAsync(venue);
            var venues = (await service.GetAllAsync()).ToList();
            await service.DeleteAsync(lastId.Id);

            // Assert
            venues.Should().BeEquivalentTo(new List<VenueDto>
            {
                new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
                new VenueDto { Id = lastId.Id, Name = "Ttttt", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" },
            });
        }

        [Test]
        public async Task Edit_WhenEditVenue_ShouldReturnVenuesListWithEditedVenue()
        {
            // Arrange
            var venue = new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "111 45 678 90 12" };
            var venueWas = new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" };
            var service = new VenueService(_venueEFRepository, _venueRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository,
                _eventSeatRepository, _eventAreaRepository, _layoutEFRepository, _areaEFRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _validator);

            // Act
            await service.EditAsync(venue);
            var venues = (await service.GetAllAsync()).ToList();
            await service.EditAsync(venueWas);

            // Assert
            venues.Should().BeEquivalentTo(new List<VenueDto>
            {
                new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "111 45 678 90 12" },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteVenue_ShouldReturnVenuesListWithoutLastElement()
        {
            // Arrange
            var venue = new VenueDto { Name = "Name1 first venue", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" };
            var service = new VenueService(_venueEFRepository, _venueRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository,
                _eventSeatRepository, _eventAreaRepository, _layoutEFRepository, _areaEFRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _validator);

            // Act
            var last = await service.AddAsync(venue);
            await service.DeleteAsync(last.Id);
            var venuesWithoutLast = await service.GetAllAsync();

            // Assert
            venuesWithoutLast.Should().BeEquivalentTo(new List<VenueDto>
            {
                new VenueDto { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
            });
        }
    }
}

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
    /// Testing of layout service.
    /// </summary>
    [TestFixture]
    public class LayoutServiceTest
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
        private LayoutValidation _validator;
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
            _seatRepository = new SeatRepository(_connectionString);
            _seatEFRepository = new Repository<Seat>(_context);
            _eventSeatRepository = new EventSeatRepository(_connectionString);
            _eventSeatEFRepository = new Repository<EventSeat>(_context);
            _validator = new LayoutValidation();
        }

        [Test]
        public async Task GetAllByParentId_WhenLayoutWithFirstVenueId_ShouldReturnLayotsList()
        {
            // Arrange
            var venueId = 1;
            var service = new LayoutService(_layoutEFRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository, _eventSeatRepository,
                _eventAreaRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _areaEFRepository, _validator);

            // Act
            var layouts = await service.GetAsync(venueId);

            // Assert
            layouts.Should().BeEquivalentTo(new List<LayoutDto>
            {
                new LayoutDto { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new LayoutDto { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task GetById_WhenLayoutWithFirsId_ShouldReturnLayout()
        {
            // Arrange
            var layoutId = 1;
            var service = new LayoutService(_layoutEFRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository, _eventSeatRepository,
                _eventAreaRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _areaEFRepository, _validator);

            // Act
            var layout = await service.GetByIdAsync(layoutId);

            // Assert
            layout.Should().BeEquivalentTo(new LayoutDto { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 });
        }

        [Test]
        public async Task Add_WhenAddNewLayout_ShouldReturnLayoutsListWithNewLayout()
        {
            // Arrange
            var layout = new LayoutDto { Name = "Name1 firsttt layout", Description = "First1 layout", VenueId = 1 };
            var service = new LayoutService(_layoutEFRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository, _eventSeatRepository,
                _eventAreaRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _areaEFRepository, _validator);

            // Act
            var lastId = await service.AddAsync(layout);
            var layouts = (await service.GetAsync(layout.VenueId)).ToList();
            await service.DeleteAsync(lastId.Id);

            // Assert
            layouts.Should().BeEquivalentTo(new List<LayoutDto>
            {
                new LayoutDto { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new LayoutDto { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
                new LayoutDto { Id = lastId.Id, Name = "Name1 firsttt layout", Description = "First1 layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task Edit_WhenEditLayout_ShouldReturnLayoutsListWithEditedLayout()
        {
            // Arrange
            var layout = new LayoutDto { Id = 1, Name = "Name first layout1", Description = "First layout", VenueId = 1 };
            var layoutWas = new LayoutDto { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 };
            var service = new LayoutService(_layoutEFRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository, _eventSeatRepository,
                _eventAreaRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _areaEFRepository, _validator);

            // Act
            await service.EditAsync(layout);
            var layouts = (await service.GetAsync(layout.VenueId)).ToList();
            await service.EditAsync(layoutWas);

            // Assert
            layouts.Should().BeEquivalentTo(new List<LayoutDto>
            {
                new LayoutDto { Id = 1, Name = "Name first layout1", Description = "First layout", VenueId = 1 },
                new LayoutDto { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteLayout_ShouldReturnLayoutsListWithoutLastElement()
        {
            // Arrange
            var layout = new LayoutDto { Name = "Name first layout1", Description = "First layout1", VenueId = 1 };
            var service = new LayoutService(_layoutEFRepository, _layoutRepository, _areaRepository, _seatRepository, _eventRepository, _eventSeatRepository,
                _eventAreaRepository, _seatEFRepository, _eventEFRepository, _eventSeatEFRepository, _eventAreaEFRepository, _areaEFRepository, _validator);

            // Act
            var lastId = await service.AddAsync(layout);
            await service.DeleteAsync(lastId.Id);
            var layoutsWithoutLast = await service.GetAsync(layout.VenueId);

            // Assert
            layoutsWithoutLast.Should().BeEquivalentTo(new List<LayoutDto>
            {
                new LayoutDto { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new LayoutDto { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }
    }
}

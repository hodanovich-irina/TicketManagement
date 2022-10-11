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
    /// Testing of event area service.
    /// </summary>
    [TestFixture]
    public class EventAreaServiceTest
    {
        private string _connectionString;
        private EventAreaRepository _areaRepository;
        private Repository<EventArea> _areaEFRepository;
        private EventSeatRepository _seatRepository;
        private Repository<EventSeat> _seatEFRepository;
        private EventAreaValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _areaRepository = new EventAreaRepository(_connectionString);
            _areaEFRepository = new Repository<EventArea>(_context);
            _seatRepository = new EventSeatRepository(_connectionString);
            _seatEFRepository = new Repository<EventSeat>(_context);
            _validator = new EventAreaValidation();
        }

        [Test]
        public async Task GetAllByParentId_WhenEventAreaWithElevenEventId_ShouldReturnEventAreasList()
        {
            // Arrange
            var eventId = 1;
            var service = new EventAreaService(_areaEFRepository, _seatRepository, _areaRepository, _seatEFRepository, _validator);

            // Act
            var events = await service.GetAsync(eventId);

            // Assert
            events.Should().BeEquivalentTo(new List<EventAreaDto>
            {
                new EventAreaDto { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First event area", Price = 1 },
            });
        }

        [Test]
        public async Task GetById_WhenEventAreaWithSecondId_ShouldReturnEventArea()
        {
            // Arrange
            var eventAreaId = 1;
            var service = new EventAreaService(_areaEFRepository, _seatRepository, _areaRepository, _seatEFRepository, _validator);

            // Act
            var eventSeat = await service.GetByIdAsync(eventAreaId);

            // Assert
            eventSeat.Should().BeEquivalentTo(new EventAreaDto { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First event area", Price = 1 });
        }

        [Test]
        public async Task Edit_WhenEditEventArea_ShouldReturnEventAreasListWithEditedArea()
        {
            // Arrange
            var eventAreaToEdit = new EventAreaDto { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First1 event area", Price = 1 };
            var eventAreaWas = new EventAreaDto { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First event area", Price = 1 };
            var service = new EventAreaService(_areaEFRepository, _seatRepository, _areaRepository, _seatEFRepository, _validator);

            // Act
            await service.EditAsync(eventAreaToEdit);
            var events = (await service.GetAsync(eventAreaToEdit.EventId)).ToList();
            await service.EditAsync(eventAreaWas);

            // Assert
            events.Should().BeEquivalentTo(new List<EventAreaDto>
            {
                new EventAreaDto { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First1 event area", Price = 1 },
            });
        }
    }
}

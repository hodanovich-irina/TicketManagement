using System.Collections.Generic;
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
    /// Testing of event seat service.
    /// </summary>
    [TestFixture]
    public class EventSeatServiceTest
    {
        private string _connectionString;
        private EventSeatRepository _seatRepository;
        private Repository<EventSeat> _seatEFRepository;
        private EventSeatValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _seatRepository = new EventSeatRepository(_connectionString);
            _seatEFRepository = new Repository<EventSeat>(_context);
            _validator = new EventSeatValidation();
        }

        [Test]
        public async Task GetAllByParentId_WhenEventSeatWithSecondEventAreaId_ShouldReturnEventSeatsList()
        {
            // Arrange
            var eventAreaId = 2;
            var service = new EventSeatService(_seatEFRepository, _seatRepository, _validator);

            // Act
            var events = await service.GetAsync(eventAreaId);

            // Assert
            events.Should().BeEquivalentTo(new List<EventSeatDto>
            {
                new EventSeatDto { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatStateDto.Free },
            });
        }

        [Test]
        public async Task GetById_WhenEventSeatWithFirsId_ShouldReturnEventSeat()
        {
            // Arrange
            var eventSeatId = 1;
            var service = new EventSeatService(_seatEFRepository, _seatRepository, _validator);

            // Act
            var eventSeat = await service.GetByIdAsync(eventSeatId);

            // Assert
            eventSeat.Should().BeEquivalentTo(new EventSeatDto { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatStateDto.Free });
        }
    }
}

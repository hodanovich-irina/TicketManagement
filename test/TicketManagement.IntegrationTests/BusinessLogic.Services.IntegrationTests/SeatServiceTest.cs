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
    /// Testing of seat service.
    /// </summary>
    [TestFixture]
    public class SeatServiceTest
    {
        private string _connectionString;
        private SeatRepository _seatRepository;
        private Repository<Seat> _seatEFRepository;
        private SeatValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _seatRepository = new SeatRepository(_connectionString);
            _seatEFRepository = new Repository<Seat>(_context);
            _validator = new SeatValidation();
        }

        [Test]
        public async Task GetById_WhenSeattWithFirsId_ShouldReturnSeat()
        {
            // Arrange
            var seatId = 1;
            var service = new SeatService(_seatEFRepository, _seatRepository, _validator);

            // Act
            var seat = await service.GetByIdAsync(seatId);

            // Assert
            seat.Should().BeEquivalentTo(new SeatDto { Id = 1, AreaId = 1, Number = 1, Row = 1 });
        }
    }
}

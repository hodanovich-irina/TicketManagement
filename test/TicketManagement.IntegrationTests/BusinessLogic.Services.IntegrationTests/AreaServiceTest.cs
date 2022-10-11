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
    /// Testing of area service.
    /// </summary>
    [TestFixture]
    public class AreaServiceTest
    {
        private string _connectionString;
        private AreaService _service;
        private AreaRepository _areaRepository;
        private Repository<Area> _areaEFRepository;
        private SeatRepository _seatRepository;
        private Repository<Seat> _seatEFRepository;
        private AreaValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _areaRepository = new AreaRepository(_connectionString);
            _areaEFRepository = new Repository<Area>(_context);
            _seatRepository = new SeatRepository(_connectionString);
            _seatEFRepository = new Repository<Seat>(_context);
            _validator = new AreaValidation();

            _service = new AreaService(_areaRepository, _areaEFRepository, _seatRepository, _seatEFRepository, _validator);
        }

        [Test]
        public async Task GetAllByParentId_WhenSeatWithFirstAreaId_ShouldReturnSeatsList()
        {
            // Arrange
            var areaId = 1;

            // Act
            var areas = await _service.GetAsync(areaId);

            // Assert
            areas.Should().BeEquivalentTo(new List<AreaDto>
            {
                new AreaDto { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new AreaDto { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }

        [Test]
        public async Task Add_WhenAddNewArea_ShouldReturnAreasListWithNewArea()
        {
            // Arrange
            var area = new AreaDto { CoordX = 3, CoordY = 3, Description = "First area of first layout3", LayoutId = 1 };

            // Act
            var lastId = await _service.AddAsync(area);
            var areas = (await _service.GetAsync(area.LayoutId)).ToList();
            await _service.DeleteAsync(lastId.Id);

            // Assert
            areas.Should().BeEquivalentTo(new List<AreaDto>
            {
                new AreaDto { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new AreaDto { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
                new AreaDto { Id = lastId.Id, CoordX = 3, CoordY = 3, Description = "First area of first layout3", LayoutId = 1 },
            });
        }

        [Test]
        public async Task Edit_WhenEditArea_ShouldReturnAreasListWithEditedArea()
        {
            // Arrange
            var area = new AreaDto { Id = 1, CoordX = 2, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };
            var areaWas = new AreaDto { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };

            // Act
            await _service.EditAsync(area);
            var areas = (await _service.GetAsync(area.LayoutId)).ToList();
            await _service.EditAsync(areaWas);

            // Assert
            areas.Should().BeEquivalentTo(new List<AreaDto>
            {
                new AreaDto { Id = 1, CoordX = 2, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new AreaDto { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteArea_ShouldReturnAreasListWithoutLastElement()
        {
            // Arrange
            var area = new AreaDto { CoordX = 3, CoordY = 3, Description = "First area of first layout3", LayoutId = 1 };

            // Act
            var lastId = await _service.AddAsync(area);
            await _service.DeleteAsync(lastId.Id);
            var areasWithoutLast = await _service.GetAsync(area.LayoutId);

            // Assert
            areasWithoutLast.Should().BeEquivalentTo(new List<AreaDto>
            {
                new AreaDto { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new AreaDto { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.IntegrationTests
{
    /// <summary>
    /// Testing of area repository.
    /// </summary>
    [TestFixture]
    public class AreaRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task GetAllByParentId_WhenAreaInFirsLayout_ShouldReturnAreasList()
        {
            // Arrange
            var layoutId = 1;
            var repository = new AreaRepository(_connectionString);

            // Act
            var areas = await repository.GetAllByParentIdAsync(layoutId);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }

        [Test]
        public async Task GetById_WhenAreaWithFirsId_ShouldReturnArea()
        {
            // Arrange
            var areaId = 1;
            var repository = new AreaRepository(_connectionString);

            // Act
            var areas = await repository.GetByIdAsync(areaId);

            // Assert
            areas.Should().BeEquivalentTo(new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 });
        }

        [Test]
        public async Task Add_WhenAddNewArea_ShouldReturnAreasListWithNewArea()
        {
            // Arrange
            var area = new Area { CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };
            var repository = new AreaRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(area);
            var areas = await repository.GetAllByParentIdAsync(area.LayoutId);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
                new Area { Id = lastId.Id, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
            });
        }

        [Test]
        public async Task Edit_WhenEditArea_ShouldReturnAreasListWithEditedArea()
        {
            // Arrange
            var area = new Area { Id = 1, CoordX = 2, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };
            var areaWas = new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };
            var repository = new AreaRepository(_connectionString);

            // Act
            await repository.EditAsync(area);
            var areas = await repository.GetAllByParentIdAsync(area.LayoutId);
            await repository.EditAsync(areaWas);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                new Area { Id = 1, CoordX = 2, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteArea_ShouldReturnAreasListWithoutLastElement()
        {
            // Arrange
            var area = new Area { CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 };
            var repository = new AreaRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(area);
            await repository.DeleteAsync(lastId.Id);
            var areasWithoutLast = await repository.GetAllByParentIdAsync(area.LayoutId);

            // Assert
            areasWithoutLast.Should().BeEquivalentTo(new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area of first layout", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "Second area of first layout", LayoutId = 1 },
            });
        }
    }
}

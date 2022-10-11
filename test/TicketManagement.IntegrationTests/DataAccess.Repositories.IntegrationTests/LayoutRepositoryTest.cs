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
    /// Testing of layout repository.
    /// </summary>
    [TestFixture]
    public class LayoutRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task GetAllByParentId_WhenLayoutWithFirstVenueId_ShouldReturnLayotsList()
        {
            // Arrange
            var venueId = 1;
            var repository = new LayoutRepository(_connectionString);

            // Act
            var layouts = await repository.GetAllByParentIdAsync(venueId);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new Layout { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task GetById_WhenLayoutWithFirsId_ShouldReturnLayout()
        {
            // Arrange
            var layoutId = 1;
            var repository = new LayoutRepository(_connectionString);

            // Act
            var layout = await repository.GetByIdAsync(layoutId);

            // Assert
            layout.Should().BeEquivalentTo(new Layout { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 });
        }

        [Test]
        public async Task Add_WhenAddNewLayout_ShouldReturnLayoutsListWithNewLayout()
        {
            // Arrange
            var layout = new Layout { Name = "Name1 first layout", Description = "First1 layout", VenueId = 1 };
            var repository = new LayoutRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(layout);
            var layouts = await repository.GetAllByParentIdAsync(layout.VenueId);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new Layout { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
                new Layout { Id = lastId.Id, Name = "Name1 first layout", Description = "First1 layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task Edit_WhenEditLayout_ShouldReturnLayoutsListWithEditedLayout()
        {
            // Arrange
            var layout = new Layout { Id = 1, Name = "Name first layout1", Description = "First layout", VenueId = 1 };
            var layoutWas = new Layout { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 };
            var repository = new LayoutRepository(_connectionString);

            // Act
            await repository.EditAsync(layout);
            var layouts = await repository.GetAllByParentIdAsync(layout.VenueId);
            await repository.EditAsync(layoutWas);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 1, Name = "Name first layout1", Description = "First layout", VenueId = 1 },
                new Layout { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteLayout_ShouldReturnLayoutsListWithoutLastElement()
        {
            // Arrange
            var layout = new Layout { Name = "Name first layout1", Description = "First layout1", VenueId = 1 };
            var repository = new LayoutRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(layout);
            await repository.DeleteAsync(lastId.Id);
            var layoutsWithoutLast = await repository.GetAllByParentIdAsync(layout.VenueId);

            // Assert
            layoutsWithoutLast.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 1, Name = "Name first layout", Description = "First layout", VenueId = 1 },
                new Layout { Id = 2, Name = "Name second layout", Description = "Second layout", VenueId = 1 },
            });
        }
    }
}

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
    /// Testing of event area repository.
    /// </summary>
    [TestFixture]
    public class EventAreaRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task Add_WhenAddNewEventArea_ShouldReturnEventAreaListWithNewEventArea()
        {
            // Arrange
            var eventAreaToAdd = new EventArea { CoordX = 1, CoordY = 1, EventId = 1, Description = "Next event area", Price = 1 };
            var repository = new EventAreaRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(eventAreaToAdd);
            var eventAreas = await repository.GetAllByParentIdAsync(eventAreaToAdd.EventId);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            eventAreas.Should().BeEquivalentTo(new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First event area", Price = 1 },
                new EventArea { Id = lastId.Id, CoordX = 1, CoordY = 1,  EventId = 1, Description = "Next event area", Price = 1 },
            });
        }

        [Test]
        public async Task GetAllByParentId_WhenEventAreaWithElevenEventId_ShouldReturnEventAreasList()
        {
            // Arrange
            var eventId = 1;
            var repository = new EventAreaRepository(_connectionString);

            // Act
            var events = await repository.GetAllByParentIdAsync(eventId);

            // Assert
            events.Should().BeEquivalentTo(new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First event area", Price = 1 },
            });
        }

        [Test]
        public async Task GetById_WhenEventAreaWithSecondId_ShouldReturnEventArea()
        {
            // Arrange
            var eventAreaId = 1;
            var repository = new EventAreaRepository(_connectionString);

            // Act
            var eventSeat = await repository.GetByIdAsync(eventAreaId);

            // Assert
            eventSeat.Should().BeEquivalentTo(new EventArea { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First event area", Price = 1 });
        }

        [Test]
        public async Task Edit_WhenEditEventArea_ShouldReturnEventAreasListWithEditedArea()
        {
            // Arrange
            var eventAreaToEdit = new EventArea { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First1 event area", Price = 1 };
            var eventAreaWas = new EventArea { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First event area", Price = 1 };
            var repository = new EventAreaRepository(_connectionString);

            // Act
            await repository.EditAsync(eventAreaToEdit);
            var events = await repository.GetAllByParentIdAsync(eventAreaToEdit.EventId);
            await repository.EditAsync(eventAreaWas);

            // Assert
            events.Should().BeEquivalentTo(new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First1 event area", Price = 1 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteEvent_ShouldReturnEventsListWithoutLastElement()
        {
            // Arrange
            var eventAreaToDelete = new EventArea { Id = 1, CoordX = 1, CoordY = 1, EventId = 1, Description = "First1 event area", Price = 1 };
            var repository = new EventAreaRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(eventAreaToDelete);
            await repository.DeleteAsync(lastId.Id);
            var eventsWithoutLast = await repository.GetAllByParentIdAsync(eventAreaToDelete.EventId);

            // Assert
            eventsWithoutLast.Should().BeEquivalentTo(new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 1,  EventId = 1, Description = "First event area", Price = 1 },
            });
        }
    }
}

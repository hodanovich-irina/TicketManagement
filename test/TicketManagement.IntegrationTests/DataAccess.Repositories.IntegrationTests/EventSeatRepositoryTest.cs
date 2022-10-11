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
    /// Testing of event seat repository.
    /// </summary>
    [TestFixture]
    public class EventSeatRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task Add_WhenAddNewEventSeat_ShouldReturnEventSeatListWithNewEventSeat()
        {
            // Arrange
            var eventSeatToAdd = new EventSeat { EventAreaId = 2, Number = 2, Row = 4, State = EventSeatState.Booked };
            var repository = new EventSeatRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(eventSeatToAdd);
            var eventSeats = await repository.GetAllByParentIdAsync(eventSeatToAdd.EventAreaId);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            eventSeats.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Free },
                new EventSeat { Id = lastId.Id, EventAreaId = 2, Number = 2, Row = 4, State = EventSeatState.Booked },
            });
        }

        [Test]
        public async Task GetAllByParentId_WhenEventSeatWithSecondEventAreaId_ShouldReturnEventSeatsList()
        {
            // Arrange
            var eventAreaId = 2;
            var repository = new EventSeatRepository(_connectionString);

            // Act
            var events = await repository.GetAllByParentIdAsync(eventAreaId);

            // Assert
            events.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Free },
            });
        }

        [Test]
        public async Task GetById_WhenEventSeatWithFirsId_ShouldReturnEventSeat()
        {
            // Arrange
            var eventSeatId = 1;
            var repository = new EventSeatRepository(_connectionString);

            // Act
            var eventSeat = await repository.GetByIdAsync(eventSeatId);

            // Assert
            eventSeat.Should().BeEquivalentTo(new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Free });
        }

        [Test]
        public async Task Edit_WhenEditEventSeat_ShouldReturnEventSeatsListWithEditedEvent()
        {
            // Arrange
            var eventSeatToEdit = new EventSeat { Id = 2, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var eventSeatWas = new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Free };
            var repository = new EventSeatRepository(_connectionString);

            // Act
            await repository.EditAsync(eventSeatToEdit);
            var events = await repository.GetAllByParentIdAsync(eventSeatToEdit.EventAreaId);
            await repository.EditAsync(eventSeatWas);

            // Assert
            events.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Free },
                new EventSeat { Id = 2, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteEvent_ShouldReturnEventsListWithoutLastElement()
        {
            // Arrange
            var eventToDelete = new EventSeat { Id = 2, EventAreaId = 2, Number = 4, Row = 2, State = EventSeatState.Free };
            var repository = new EventSeatRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(eventToDelete);
            await repository.DeleteAsync(lastId.Id);
            var eventsWithoutLast = await repository.GetAllByParentIdAsync(eventToDelete.EventAreaId);

            // Assert
            eventsWithoutLast.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Free },
            });
        }
    }
}

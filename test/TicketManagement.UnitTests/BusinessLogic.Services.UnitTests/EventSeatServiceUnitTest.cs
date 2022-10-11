using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.BusinessLogic.ModelsDTO;
using TicketManagement.BusinessLogic.Services;
using TicketManagement.BusinessLogic.Validations;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Models;

namespace TicketManagement.UnitTests.BusinessLogic.Services.UnitTests
{
    /// <summary>
    /// Testing of event seat service.
    /// </summary>
    [TestFixture]
    public class EventSeatServiceUnitTest
    {
        private Mock<IEFRepository<EventSeat>> _eventSeatEFMock;
        private Mock<IRepository<EventSeat>> _eventSeatMock;
        private IValidator<EventSeatDto> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new EventSeatValidation();
            _eventSeatEFMock = new Mock<IEFRepository<EventSeat>>();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
        }

        [Test]
        public async Task ReturnEventSeatFromFirstEventArea_WhenReturnEventSeatFromFirstEventArea_ShouldReturnEventSeatWithFirstEventAreaId()
        {
            // Arrange
            var eventAreaId = 1;
            var eventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
                new EventSeat { Id = 1, EventAreaId = 2, Number = 2, Row = 1, State = EventSeatState.Booked },
            };

            _eventSeatEFMock.Setup(eventSeat => eventSeat.GetAsync(It.IsAny<Func<EventSeat, bool>>()))
                .Returns(Task.FromResult(eventSeats.Where(eventSeatForId => eventSeatForId.EventAreaId == eventAreaId).AsQueryable()));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            var result = await eventSeatService.GetAsync(eventAreaId);

            // Assert
            result.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
            });
        }

        [Test]
        public async Task ReturnEventSeatById_WhenReturnEventSeatWithFirstId_ShouldReturnEventSeatWithFirstId()
        {
            // Arrange
            var eventSeatId = 1;
            var eventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 1, State = EventSeatState.Booked },
            };

            _eventSeatMock.Setup(eventArea => eventArea.GetByIdAsync(eventSeatId)).Returns(Task.FromResult(eventSeats.FirstOrDefault(eventSeat => eventSeat.Id == eventSeatId)));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            var result = await eventSeatService.GetByIdAsync(eventSeatId);

            // Assert
            result.Should().BeEquivalentTo(eventSeats.FirstOrDefault(eventArea => eventArea.Id == eventSeatId));
        }

        [Test]
        public void ReturnEventSeatById_WhenReturnEventSeatWithZeroId_ShouldThrowException()
        {
            // Arrange
            var eventSeatId = 0;
            var eventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 1, State = EventSeatState.Booked },
            };

            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(eventSeatId)).Returns(Task.FromResult(eventSeats.FirstOrDefault(eventSeat => eventSeat.Id == eventSeatId)));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.GetByIdAsync(eventSeatId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void AddNewEventSeat_WhenDataAdded_ShouldThrowException()
        {
            // Arrange
            var eventSeats = new List<EventSeat>();
            var eventSeatToAdd = new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked };
            var dtoEventSeatToAdd = new EventSeatDto
            {
                Id = eventSeatToAdd.Id,
                EventAreaId = eventSeatToAdd.EventAreaId,
                Number = eventSeatToAdd.Number,
                Row = eventSeatToAdd.Row,
                State = (EventSeatStateDto)eventSeatToAdd.State,
            };

            _eventSeatMock.Setup(area => area.AddAsync(eventSeatToAdd)).Callback<EventSeat>(eventSeat => eventSeats.Add(eventSeat));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.AddAsync(dtoEventSeatToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create seat for event without event"));
        }

        [Test]
        public async Task EditEventSeat_WhenValidData_ShouldEditedEventSeat()
        {
            // Arrange
            var seat = new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Free };
            var eventSeats = new List<EventSeat> { seat };
            var eventSeatToEdit = new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked };
            var dtoEventSeatToEdit = new EventSeatDto { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatStateDto.Booked };
            seat.State = eventSeatToEdit.State;
            _eventSeatEFMock.Setup(eventSeat => eventSeat.GetAsync(It.IsAny<Func<EventSeat, bool>>())).Returns(Task.FromResult(eventSeats.AsQueryable()));
            _eventSeatMock.Setup(eventSeat => eventSeat.EditAsync(eventSeatToEdit));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            await eventSeatService.EditAsync(dtoEventSeatToEdit);

            // Assert
            eventSeats.Should().BeEquivalentTo(new List<EventSeat>
            {
                eventSeatToEdit,
            });
        }

        [Test]
        public void EditEventSeat_WhenRowAndNumberNotUniq_ShouldThrowException()
        {
            // Arrange
            var eventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Free },
                new EventSeat { Id = 2, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Booked },
            };
            var eventSeatToEdit = new EventSeat { Id = 1, EventAreaId = 2, Number = 2, Row = 2, State = EventSeatState.Booked };
            var dtoEventSeatToEdit = new EventSeatDto
            {
                Id = eventSeatToEdit.Id,
                EventAreaId = eventSeatToEdit.EventAreaId,
                Number = eventSeatToEdit.Number,
                Row = eventSeatToEdit.Row,
                State = (EventSeatStateDto)eventSeatToEdit.State,
            };
            _eventSeatEFMock.Setup(eventSeat => eventSeat.GetAsync(It.IsAny<Func<EventSeat, bool>>())).Returns(Task.FromResult(eventSeats.AsQueryable()));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.EditAsync(dtoEventSeatToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit this seat. Seat with this row and number alredy exist in this event area"));
        }

        [Test]
        public void DeleteEventSeat_WhenDeleteEventSeatBooked_ShouldThrowException()
        {
            // Arrange
            var eventSeatId = 1;
            var eventSeats = new List<EventSeat> { new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked } };

            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(eventSeatId)).Returns(Task.FromResult(eventSeats.FirstOrDefault(eventSeatForId => eventSeatForId.Id == eventSeatId)));
            _eventSeatMock.Setup(eventSeat => eventSeat.DeleteAsync(eventSeatId)).Callback<int>(id => eventSeats.Remove(eventSeats.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.DeleteAsync(eventSeatId);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't delete booked seat"));
        }

        [Test]
        public async Task DeleteEventSeat_WhenDeleteEventSeatFree_ShouldDeleteEventSeat()
        {
            // Arrange
            var eventSeatId = 1;
            var eventSeats = new List<EventSeat>
            {
                new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Free },
                new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
            };

            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(eventSeatId)).Returns(Task.FromResult(eventSeats.FirstOrDefault(eventSeatForId => eventSeatForId.Id == eventSeatId)));
            _eventSeatMock.Setup(eventSeat => eventSeat.DeleteAsync(eventSeatId)).Callback<int>(id => eventSeats.Remove(eventSeats.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            await eventSeatService.DeleteAsync(eventSeatId);

            // Assert
            eventSeats.Should().BeEquivalentTo(new List<EventSeat>
            {
                new EventSeat { Id = 2, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked },
            });
        }

        [Test]
        public void EventSeatAdd_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0, Number = 1, Row = 1, State = EventSeatState.Booked, EventAreaId = 0 };
            var dtoEventSeat = new EventSeatDto
            {
                Id = eventSeat.Id,
                EventAreaId = eventSeat.EventAreaId,
                Number = eventSeat.Number,
                Row = eventSeat.Row,
                State = (EventSeatStateDto)eventSeat.State,
            };
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.AddAsync(dtoEventSeat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void EventSeatAdd_WhenUseValidatorRowAndNumbersLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 1, Number = 0, Row = 1, State = EventSeatState.Booked, EventAreaId = 2 };
            var dtoEventSeat = new EventSeatDto
            {
                Id = eventSeat.Id,
                EventAreaId = eventSeat.EventAreaId,
                Number = eventSeat.Number,
                Row = eventSeat.Row,
                State = (EventSeatStateDto)eventSeat.State,
            };
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.AddAsync(dtoEventSeat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Number and row of seat must be more than zero"));
        }

        [Test]
        public void EventSeatDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventSeat eventSeat = new EventSeat { Id = 0, Number = 1, Row = 1, State = EventSeatState.Booked, EventAreaId = 2 };
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.DeleteAsync(eventSeat.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void EventSeatEdit_WhenUseValidatorAndEventSeatWasNull_ShouldThrowException()
        {
            // Arrange
            EventSeatDto eventSeat = new EventSeatDto { Id = 0 };
            var eventSeatService = new EventSeatService(_eventSeatEFMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventSeatService.EditAsync(eventSeat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }
    }
}
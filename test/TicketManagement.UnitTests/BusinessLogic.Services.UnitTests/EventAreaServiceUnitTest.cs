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
    /// Testing of event area service.
    /// </summary>
    [TestFixture]
    public class EventAreaServiceUnitTest
    {
        private Mock<IRepository<EventArea>> _eventAreaMock;
        private Mock<IRepository<EventSeat>> _eventSeatMock;
        private Mock<IEFRepository<EventArea>> _eventAreaEFMock;
        private Mock<IEFRepository<EventSeat>> _eventSeatEFMock;
        private IValidator<EventAreaDto> _validator;

        [SetUp]
        public void Setup()
        {
            _eventAreaMock = new Mock<IRepository<EventArea>>();
            _eventAreaEFMock = new Mock<IEFRepository<EventArea>>();
            _validator = new EventAreaValidation();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
            _eventSeatEFMock = new Mock<IEFRepository<EventSeat>>();
        }

        [Test]
        public async Task ReturnEventAreaFromFirstEvent_WhenReturnEventAreaFromFirstEvent_ShouldReturnEventAreaFromFirstEvent()
        {
            // Arrange
            var eventId = 1;
            var eventAreas = new List<EventArea>
            {
                new EventArea { CoordX = 1, CoordY = 2, Description = "First event area",  EventId = 1, Price = 2 },
                new EventArea { CoordX = 2, CoordY = 2, Description = "Second event area",  EventId = 1, Price = 2 },
            };

            _eventAreaEFMock.Setup(eventArea => eventArea.GetAsync(It.IsAny<Func<EventArea, bool>>()))
                .Returns(Task.FromResult(eventAreas.Where(eventAreaForId=> eventAreaForId.EventId == eventId).AsQueryable()));
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            var result = await eventAreaService.GetAsync(eventId);

            // Assert
            result.Should().BeEquivalentTo(eventAreas);
        }

        [Test]
        public async Task ReturnEventAreaById_WhenReturnEventAreaWithFirstId_ShouldReturnAreaWithFirstId()
        {
            // Arrange
            var eventAreaId = 1;
            var eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 2, CoordY = 2, Description = "Second event area",  EventId = 2, Price = 2 },
                new EventArea { Id = 2, CoordX = 1, CoordY = 1, Description = "First event area",  EventId = 1, Price = 1 },
            };

            _eventAreaMock.Setup(eventArea => eventArea.GetByIdAsync(eventAreaId)).Returns(Task.FromResult(eventAreas.FirstOrDefault(area => area.Id == eventAreaId)));
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            var result = await eventAreaService.GetByIdAsync(eventAreaId);

            // Assert
            result.Should().BeEquivalentTo(eventAreas.FirstOrDefault(eventArea => eventArea.Id == eventAreaId));
        }

        [Test]
        public void ReturnEventAreaById_WhenReturnEventAreaWithZeroId_ShouldThrowException()
        {
            // Arrange
            var eventAreaId = 0;
            var eventAreas = new List<EventArea>
            {
                new EventArea { CoordX = 1, CoordY = 2, Description = "First event area",  EventId = 1, Price = 2 },
                new EventArea { CoordX = 2, CoordY = 2, Description = "Second event area",  EventId = 1, Price = 2 },
            };

            _eventAreaMock.Setup(eventArea => eventArea.GetByIdAsync(eventAreaId)).Returns(Task.FromResult(eventAreas.FirstOrDefault(eventArea => eventArea.Id == eventAreaId)));
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.GetByIdAsync(eventAreaId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void AddNewEventArea_WhenDataAdded_ShouldThrowException()
        {
            // Arrange
            var eventAreas = new List<EventArea>();
            var eventAreaToAdd = new EventArea { CoordX = 1, CoordY = 2, Description = "First event area", EventId = 1, Price = 2 };

            _eventAreaMock.Setup(area => area.AddAsync(eventAreaToAdd)).Callback<EventArea>(eventArea => eventAreas.Add(eventArea));
            var dtoEventAreaToAdd = new EventAreaDto
            {
                CoordX = eventAreaToAdd.CoordX,
                CoordY = eventAreaToAdd.CoordY,
                Description = eventAreaToAdd.Description,
                EventId = eventAreaToAdd.EventId,
                Price = eventAreaToAdd.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await eventAreaService.AddAsync(dtoEventAreaToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create area for event without event"));
        }

        [Test]
        public async Task EditEventArea_WhenValidData_ShouldEditedEventArea()
        {
            // Arrange
            var eventArea = new EventArea { Id = 1, CoordX = 1, CoordY = 2, Description = "First event area", EventId = 1, Price = 2 };
            var eventAreas = new List<EventArea> { eventArea };
            var eventAreaToEdit = new EventArea { Id = 1, CoordX = 1, CoordY = 3, Description = "First event area", EventId = 1, Price = 2 };
            eventArea.CoordY = eventAreaToEdit.CoordY;
            _eventAreaEFMock.Setup(eventArea => eventArea.GetAsync(It.IsAny<Func<EventArea, bool>>())).Returns(Task.FromResult(eventAreas.AsQueryable()));
            _eventAreaMock.Setup(eventArea => eventArea.EditAsync(eventAreaToEdit));

            var dtoEventAreaToEdit = new EventAreaDto
            {
                Id = eventAreaToEdit.Id,
                CoordX = eventAreaToEdit.CoordX,
                CoordY = eventAreaToEdit.CoordY,
                Description = eventAreaToEdit.Description,
                EventId = eventAreaToEdit.EventId,
                Price = eventAreaToEdit.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            await eventAreaService.EditAsync(dtoEventAreaToEdit);

            // Assert
            eventAreas.Should().BeEquivalentTo(new List<EventArea>
            {
                eventAreaToEdit,
            });
        }

        [Test]
        public void DeleteEventArea_WhenDeleteEventAreaWithBookedSeat_ShouldThrowException()
        {
            // Arrange
            var eventAreaId = 1;
            var eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 2, Description = "First event area", EventId = 1, Price = 2 },
                new EventArea { Id = 2, CoordX = 1, CoordY = 2, Description = "First event area", EventId = 1, Price = 2 },
            };
            var eventSeats = new List<EventSeat> { new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked } };

            _eventSeatEFMock.Setup(eventSeats => eventSeats.GetAsync(It.IsAny<Func<EventSeat, bool>>())).Returns(Task.FromResult(eventSeats.Where(x => x.EventAreaId == eventAreaId).AsQueryable()));

            _eventAreaMock.Setup(eventArea => eventArea.GetByIdAsync(eventAreaId)).Returns(Task.FromResult(eventAreas.FirstOrDefault(eventAreaForId => eventAreaForId.Id == eventAreaId)));
            _eventAreaMock.Setup(eventArea => eventArea.DeleteAsync(eventAreaId)).Callback<int>(id => eventAreas.Remove(eventAreas.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.DeleteAsync(eventAreaId);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't delete event area. Any seats booked"));
        }

        [Test]
        public void EventAreaEdit_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 1, CoordX = 1, CoordY = 1, Description = "eventArea description", EventId = -2, Price = 2 };
            var dtoEventArea = new EventAreaDto
            {
                Id = eventArea.Id,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Description = eventArea.Description,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.EditAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void EventAreaAdd_WhenUseValidatorAndCoordinatesLessThenNull_ShouldThrowException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 1, CoordX = -1, CoordY = 1, Description = "eventArea description", EventId = 2, Price = 2 };
            var dtoEventArea = new EventAreaDto
            {
                Id = eventArea.Id,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Description = eventArea.Description,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.AddAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Coordinates must be more than zero"));
        }

        [Test]
        public void EventAreaAdd_WhenUseValidatorAndDescriptionisNull_ShouldThrowException()
        {
            // Arrange
            string stringDescription =
                "eventArea dgvyhtf zygh cux iuchxknvkjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj jcnnnnnnnnnnnnnnkjlxsj xncdddddddddd" +
                "ddddddddddddddddddddd dhxicuhxohxuiuyx uihsn jhzbjbxz xzhjxnbzyu xgx xgxxulujnjk x huixlnkjkvskj,kxjdvn hg isg vdhjkxfjh v";

            EventArea eventArea = new EventArea { Id = 1, CoordX = 1, CoordY = 1, Description = stringDescription, EventId = 2, Price = 2 };
            var dtoEventArea = new EventAreaDto
            {
                Id = eventArea.Id,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Description = eventArea.Description,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.AddAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of area must be less than 200 and must be not null"));
        }

        [Test]
        public void EventAreaEdit_WhenUseValidatorAndDescriptionMoreThenNeed_ShouldThrowException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 1, CoordX = 1, CoordY = 1, Description = null, EventId = 2, Price = 2 };
            var dtoEventArea = new EventAreaDto
            {
                Id = eventArea.Id,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Description = eventArea.Description,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.EditAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of area must be less than 200 and must be not null"));
        }

        [Test]
        public void EventAreaEdit_WhenUseValidatorWhenPriceLessThenZero_ShouldThrowException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 1, CoordX = 1, CoordY = 1, Description = "description", EventId = 2, Price = -1 };
            var dtoEventArea = new EventAreaDto
            {
                Id = eventArea.Id,
                CoordX = eventArea.CoordX,
                CoordY = eventArea.CoordY,
                Description = eventArea.Description,
                EventId = eventArea.EventId,
                Price = eventArea.Price,
            };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.EditAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Price must be more than zero"));
        }

        [Test]
        public void EventAreaDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventArea eventArea = new EventArea { Id = 0, CoordX = 1, CoordY = 1, Description = "EventArea description", EventId = 2, Price = 2 };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.DeleteAsync(eventArea.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void EventAreaEdit_WhenUseValidatorAndEventAreaWasNull_ShouldThrowException()
        {
            // Arrange
            EventAreaDto dtoEventArea = new EventAreaDto { Id = 0 };
            var eventAreaService = new EventAreaService(_eventAreaEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _eventSeatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventAreaService.EditAsync(dtoEventArea);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }
    }
}

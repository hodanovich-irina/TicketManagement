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
    /// Testing of event service.
    /// </summary>
    [TestFixture]
    public class EventServiceUnitTest
    {
        private Mock<IRepository<Venue>> _venueMock;
        private Mock<IRepository<Layout>> _layoutMock;
        private Mock<IRepository<Seat>> _seatMock;
        private Mock<IRepository<Area>> _areaMock;
        private Mock<IRepository<Event>> _eventMock;
        private Mock<IRepository<EventArea>> _eventAreaMock;
        private Mock<IRepository<EventSeat>> _eventSeatMock;
        private Mock<IEFRepository<Venue>> _venueEFMock;
        private Mock<IEFRepository<Layout>> _layoutEFMock;
        private Mock<IEFRepository<Seat>> _seatEFMock;
        private Mock<IEFRepository<Area>> _areaEFMock;
        private Mock<IEFRepository<Event>> _eventEFMock;
        private Mock<IEFRepository<EventArea>> _eventAreaEFMock;
        private Mock<IEFRepository<EventSeat>> _eventSeatEFMock;
        private IValidator<EventDto> _validator;

        [SetUp]
        public void Setup()
        {
            _layoutMock = new Mock<IRepository<Layout>>();
            _seatMock = new Mock<IRepository<Seat>>();
            _areaMock = new Mock<IRepository<Area>>();
            _eventMock = new Mock<IRepository<Event>>();
            _eventAreaMock = new Mock<IRepository<EventArea>>();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
            _venueMock = new Mock<IRepository<Venue>>();
            _layoutEFMock = new Mock<IEFRepository<Layout>>();
            _seatEFMock = new Mock<IEFRepository<Seat>>();
            _areaEFMock = new Mock<IEFRepository<Area>>();
            _eventEFMock = new Mock<IEFRepository<Event>>();
            _eventAreaEFMock = new Mock<IEFRepository<EventArea>>();
            _eventSeatEFMock = new Mock<IEFRepository<EventSeat>>();
            _venueEFMock = new Mock<IEFRepository<Venue>>();
            _validator = new EventValidation();
        }

        [Test]
        public async Task AddNewEvent_WhenAddEventWithValidData_ShouldAddedNewEvent()
        {
            // Arrange
            var eventToAdd = new Event
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };

            var events = new List<Event> { eventToAdd };

            var dtoEventToAdd = new EventDto
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };

            _venueEFMock.Setup(allEvent => allEvent.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToAdd.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToAdd.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(new List<Event>().AsQueryable()));
            _eventMock.Setup(addEvent => addEvent.AddAsync(eventToAdd));
            _eventEFMock.Setup(method => method.GetAllAsync()).Returns(Task.FromResult(events.AsQueryable()));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            await eventService.AddAsync(dtoEventToAdd);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                eventToAdd,
            });
        }

        [Test]
        public void AddNewEvent_WhenAddEventWithNoValidDate_ShouldThrowException()
        {
            // Arrange
            var events = new List<Event>();
            var eventToAdd = new Event
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2022, 11, 1), DateStart = new DateTime(2021, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToAdd = new EventDto
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2022, 11, 1), DateStart = new DateTime(2021, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _venueEFMock.Setup(allEvent => allEvent.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToAdd.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToAdd.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(addEvent => addEvent.AddAsync(eventToAdd)).Callback<Event>(addEvent => events.Add(addEvent));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.AddAsync(dtoEventToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create event in tha past!"));
        }

        [Test]
        public void AddNewEvent_WhenAddEventWithDateStartMoreThenDateEnd_ShouldThrowException()
        {
            // Arrange
            var events = new List<Event>();
            var eventToAdd = new Event
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2026, 11, 1), DateStart = new DateTime(2027, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToAdd = new EventDto
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2026, 11, 1), DateStart = new DateTime(2027, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _venueEFMock.Setup(allEvent => allEvent.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToAdd.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToAdd.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(addEvent => addEvent.AddAsync(eventToAdd)).Callback<Event>(addEvent => events.Add(addEvent));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.AddAsync(dtoEventToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create event. Date of end should be more than date of start"));
        }

        [Test]
        public void AddNewEvent_WhenAddEventWithDateIntersect_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg", ShowTime = new TimeSpan(01, 30, 00),
                },
            };
            var eventToAdd = new Event
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg", ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToAdd = new EventDto
            {
                Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _layoutEFMock.Setup(layouts => layouts.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToAdd.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToAdd.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(addEvent => addEvent.AddAsync(eventToAdd)).Callback<Event>(addEvent => events.Add(addEvent));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.AddAsync(dtoEventToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create event. Event on this period alredy exsist in venue!"));
        }

        [Test]
        public void EditEvent_WhenEditEventWithDateIntersect_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
                new Event
                {
                    Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
            };
            var eventToEdit = new Event
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2029, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToEdit = new EventDto
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2027, 11, 1), DateStart = new DateTime(2029, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _layoutEFMock.Setup(layouts => layouts.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToEdit.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToEdit.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(editEvent => editEvent.EditAsync(eventToEdit)).Callback<Event>(editEvent =>
            {
                var editedEvent = events.FirstOrDefault(eventId => eventId.Id == editEvent.Id);
                editedEvent.Id = editEvent.Id;
                editedEvent.LayoutId = editEvent.LayoutId;
                editedEvent.DateEnd = editEvent.DateEnd;
                editedEvent.DateStart = editEvent.DateStart;
                editedEvent.Description = editEvent.Description;
                editedEvent.BaseAreaPrice = editEvent.BaseAreaPrice;
                editedEvent.Name = editEvent.Name;
            });
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.EditAsync(dtoEventToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit event. Event on this period alredy exsist in venue!"));
        }

        [Test]
        public void EditEvent_WhenEditEventWithDateStartMoreThanEnd_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
                new Event
                {
                    Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
            };
            var eventToEdit = new Event
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2033, 11, 1), DateStart = new DateTime(2034, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToEdit = new EventDto
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2033, 11, 1), DateStart = new DateTime(2034, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _layoutEFMock.Setup(layouts => layouts.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToEdit.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToEdit.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(editEvent => editEvent.EditAsync(eventToEdit)).Callback<Event>(editEvent =>
            {
                var editedEvent = events.FirstOrDefault(eventId => eventId.Id == editEvent.Id);
                editedEvent.Id = editEvent.Id;
                editedEvent.LayoutId = editEvent.LayoutId;
                editedEvent.DateEnd = editEvent.DateEnd;
                editedEvent.DateStart = editEvent.DateStart;
                editedEvent.Description = editEvent.Description;
                editedEvent.BaseAreaPrice = editEvent.BaseAreaPrice;
                editedEvent.Name = editEvent.Name;
            });
            _eventMock.Setup(editEvent => editEvent.GetByIdAsync(eventToEdit.Id)).Returns(Task.FromResult(events.FirstOrDefault(eventId => eventId.Id == eventToEdit.Id)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.EditAsync(dtoEventToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit event. Date of end should be more than date of start"));
        }

        [Test]
        public void EditEvent_WhenEditEventWithDateLessThanNow_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var events = new List<Event>
            {
                new Event
                {
                    Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
                new Event
                {
                    Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                    ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                    ShowTime = new TimeSpan(01, 30, 00),
                },
            };
            var eventToEdit = new Event
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2018, 11, 1), DateStart = new DateTime(2017, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var dtoEventToEdit = new EventDto
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2018, 11, 1), DateStart = new DateTime(2017, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _layoutEFMock.Setup(layouts => layouts.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToEdit.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToEdit.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(editEvent => editEvent.EditAsync(eventToEdit)).Callback<Event>(editEvent =>
            {
                var editedEvent = events.FirstOrDefault(eventId => eventId.Id == editEvent.Id);
                editedEvent.Id = editEvent.Id;
                editedEvent.LayoutId = editEvent.LayoutId;
                editedEvent.DateEnd = editEvent.DateEnd;
                editedEvent.DateStart = editEvent.DateStart;
                editedEvent.Description = editEvent.Description;
                editedEvent.BaseAreaPrice = editEvent.BaseAreaPrice;
                editedEvent.Name = editEvent.Name;
            });
            _eventMock.Setup(editEvent => editEvent.GetByIdAsync(eventToEdit.Id)).Returns(Task.FromResult(events.FirstOrDefault(eventId => eventId.Id == eventToEdit.Id)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.EditAsync(dtoEventToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit event in tha past!"));
        }

        [Test]
        public async Task EditEvent_WhenEditEventWithValidData_ShouldEditedEvent()
        {
            // Arrange
            int id = 1;
            var eventNew = new Event
            {
                Id = 2,
                BaseAreaPrice = 5,
                DateEnd = new DateTime(2029, 11, 1),
                DateStart = new DateTime(2030, 12, 2),
                Description = "Event",
                LayoutId = 1,
                Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };

            var events = new List<Event> { eventNew };
            var eventToEdit = new Event
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2033, 11, 1), DateStart = new DateTime(2032, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            eventNew.DateEnd = eventToEdit.DateEnd;
            eventNew.DateStart = eventToEdit.DateStart;
            var dtoEventToEdit = new EventDto
            {
                Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2033, 11, 1), DateStart = new DateTime(2032, 12, 2), Description = "Event", LayoutId = 1, Name = "Event",
                ImageURL = "https://www.megacritic.ru/media/reviews/photos/thumbnail/640x640s/12/c8/8b/kingsman-nachalo-5-1597677411.jpg",
                ShowTime = new TimeSpan(01, 30, 00),
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            _layoutEFMock.Setup(layouts => layouts.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _layoutMock.Setup(layouts => layouts.GetByIdAsync(eventToEdit.LayoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == eventToEdit.LayoutId)));
            _eventEFMock.Setup(allEvent => allEvent.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.AsQueryable()));
            _eventMock.Setup(editEvent => editEvent.EditAsync(eventToEdit));
            _eventMock.Setup(editEvent => editEvent.GetByIdAsync(eventToEdit.Id)).Returns(Task.FromResult(events.FirstOrDefault(eventId => eventId.Id == eventToEdit.Id)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            await eventService.EditAsync(dtoEventToEdit);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                eventToEdit,
            });
        }

        [Test]
        public async Task ReturnEventFromFirstLayout_WhenReturnEventFromFirstLayout_ShouldReturnedEventFromFirstLayout()
        {
            // Arrange
            var layotId = 1;
            var events = new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            };

            _eventEFMock.Setup(eventt => eventt.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.Where(eventForId => eventForId.LayoutId == layotId).AsQueryable()));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            var result = await eventService.GetAsync(layotId);

            // Assert
            result.Should().BeEquivalentTo(new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            });
        }

        [Test]
        public async Task ReturnEventById_WhenReturnEventWithFirstId_ShouldReturnEventWithFirstId()
        {
            // Arrange
            var eventId = 1;
            var events = new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            };

            _eventMock.Setup(eventById => eventById.GetByIdAsync(eventId)).Returns(Task.FromResult(events.FirstOrDefault(eventForId => eventForId.Id == eventId)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            var result = await eventService.GetByIdAsync(eventId);

            // Assert
            result.Should().BeEquivalentTo(events.FirstOrDefault(seat => seat.Id == eventId));
        }

        [Test]
        public void ReturnEvents_WhenReturnEventsWithZerosId_ShouldThrowException()
        {
            // Arrange
            var layotId = 0;
            var events = new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            };

            _eventEFMock.Setup(eventt => eventt.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.Where(eventForId => eventForId.LayoutId == layotId).AsQueryable()));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.GetAsync(layotId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void ReturnEventById__WhenReturnEventWithZerosId_ShouldThrowException()
        {
            // Arrange
            var eventId = 0;
            var events = new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            };

            _eventMock.Setup(eventById => eventById.GetByIdAsync(eventId)).Returns(Task.FromResult(events.FirstOrDefault(eventForId => eventForId.Id == eventId)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.GetByIdAsync(eventId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public async Task DeleteEvent_WhenDeleteEvent_ShouldDeleteEvent()
        {
            // Arrange
            var eventId = 1;
            var events = new List<Event>
            {
                new Event { Id = 1, BaseAreaPrice = 5, DateEnd = new DateTime(2028, 11, 1), DateStart = new DateTime(2026, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            };

            _eventMock.Setup(eventForDelete => eventForDelete.DeleteAsync(eventId)).Callback<int>(id => events.Remove(events.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            await eventService.DeleteAsync(eventId);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                new Event { Id = 2, BaseAreaPrice = 5, DateEnd = new DateTime(2029, 11, 1), DateStart = new DateTime(2030, 12, 2), Description = "Event", LayoutId = 1, Name = "Event" },
            });
        }

        [Test]
        public void EventEdit_WhenNameLengthMoreThenThirty_ShouldThrowException()
        {
            // Arrange
            EventDto eventToEdit = new EventDto
            {
                Id = 1,
                Name = "Concert halllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll" +
                "llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll",
                Description = "Description of first layout",
                LayoutId = 2,
                BaseAreaPrice = 2,
                DateEnd = new DateTime(2033, 10, 10),
                DateStart = new DateTime(2031, 10, 10),
            };
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.EditAsync(eventToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Name of event must be less than 120 sumbols and must be not null"));
        }

        [Test]
        public void EventAdd_WhenPriceLessThenOne_ShouldThrowException()
        {
            // Arrange
            EventDto eventToAdd = new EventDto
            {
                Id = 0,
                Description = "First venue descriptionl",
                Name = "Concert hall",
                LayoutId = 2,
                BaseAreaPrice = 2,
                DateEnd = new DateTime(2033, 10, 10),
                DateStart = new DateTime(2031, 10, 10),
            };
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.AddAsync(eventToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Image url of event must be not null"));
        }

        [Test]
        public void EventDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            Event eventToAdd = new Event
            {
                Id = 0,
                BaseAreaPrice = 2,
                DateEnd = new DateTime(2033, 10, 10),
                DateStart = new DateTime(2031, 10, 10),
                Description = "Description of event",
                LayoutId = 2,
                Name = "First event",
            };
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.DeleteAsync(eventToAdd.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void EventEdit_WhenUseValidatorAndEventWasNull_ShouldThrowException()
        {
            // Arrange
            EventDto eventToEdit = new EventDto { Id = 0 };
            var eventService = new EventService(_eventEFMock.Object, _eventMock.Object, _layoutMock.Object, _layoutEFMock.Object, _eventAreaEFMock.Object,
                _areaEFMock.Object, _seatEFMock.Object, _eventSeatEFMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => eventService.EditAsync(eventToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }
    }
}

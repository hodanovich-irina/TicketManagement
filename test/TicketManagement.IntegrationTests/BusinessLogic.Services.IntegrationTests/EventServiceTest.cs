using System;
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
    /// Testing of event service.
    /// </summary>
    public class EventServiceTest
    {
        private string _connectionString;
        private Repository<Layout> _layoutEFRepository;
        private LayoutRepository _layoutRepository;
        private Repository<Area> _areaEFRepository;
        private Repository<Seat> _seatEFRepository;
        private EventAreaRepository _eventAreaRepository;
        private Repository<EventArea> _eventAreaEFRepository;
        private EventRepository _eventRepository;
        private Repository<Event> _eventEFRepository;
        private EventSeatRepository _eventSeatRepository;
        private Repository<EventSeat> _eventSeatEFRepository;
        private EventValidation _validator;
        private TicketManagementContext _context;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
            _areaEFRepository = new Repository<Area>(_context);
            _eventAreaRepository = new EventAreaRepository(_connectionString);
            _eventAreaEFRepository = new Repository<EventArea>(_context);
            _eventRepository = new EventRepository(_connectionString);
            _eventEFRepository = new Repository<Event>(_context);
            _layoutRepository = new LayoutRepository(_connectionString);
            _layoutEFRepository = new Repository<Layout>(_context);
            _seatEFRepository = new Repository<Seat>(_context);
            _eventSeatRepository = new EventSeatRepository(_connectionString);
            _eventSeatEFRepository = new Repository<EventSeat>(_context);
            _validator = new EventValidation();
        }

        [Test]
        public async Task Add_WhenAddNewEvent_ShouldReturnEventListWithNewEvent()
        {
            // Arrange
            var eventToAdd = new EventDto
            {
                LayoutId = 1,
                Name = "Name",
                Description = "First",
                DateStart = new DateTime(2085, 10, 10),
                DateEnd = new DateTime(2087, 10, 10),
                BaseAreaPrice = 2,
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var service = new EventService(_eventEFRepository, _eventRepository, _layoutRepository, _layoutEFRepository, _eventAreaEFRepository, _areaEFRepository,
                _seatEFRepository, _eventSeatEFRepository, _eventSeatRepository, _eventAreaRepository, _validator);

            // Act
            var last = await service.AddAsync(eventToAdd);
            var events = (await service.GetAsync(eventToAdd.LayoutId)).ToList();
            await service.DeleteAsync(last.Id);

            // Assert
            events.Should().BeEquivalentTo(new List<EventDto>
            {
                new EventDto
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                    ShowTime = new TimeSpan(11, 30, 00),
                },
                new EventDto
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = "https://static.kinoafisha.info/k/movie_posters/canvas/800x1200/upload/movie_posters/3/4/9/8362943/1f273c077a116fc68dd219e88ca47079.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
                new EventDto
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = "https://plaqat.ru/images/47675.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
                new EventDto
                {
                    Id = last.Id, LayoutId = 1, Name = "Name", Description = "First", DateStart = new DateTime(2085, 10, 10), DateEnd = new DateTime(2087, 10, 10),
                    ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                    ShowTime = new TimeSpan(11, 30, 00),
                },
            });
        }

        [Test]
        public async Task GetAllByParentId_WhenEventtWithFirstLayoutId_ShouldReturnEventsList()
        {
            // Arrange
            var layoutId = 1;
            var service = new EventService(_eventEFRepository, _eventRepository, _layoutRepository, _layoutEFRepository, _eventAreaEFRepository, _areaEFRepository,
                            _seatEFRepository, _eventSeatEFRepository, _eventSeatRepository, _eventAreaRepository, _validator);

            // Act
            var events = await service.GetAsync(layoutId);

            // Assert
            events.Should().BeEquivalentTo(new List<EventDto>
            {
                new EventDto
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                    ShowTime = new TimeSpan(11, 30, 00),
                },
                new EventDto
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = "https://static.kinoafisha.info/k/movie_posters/canvas/800x1200/upload/movie_posters/3/4/9/8362943/1f273c077a116fc68dd219e88ca47079.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
                new EventDto
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = "https://plaqat.ru/images/47675.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
            });
        }

        [Test]
        public async Task GetById_WhenEventWithFirsId_ShouldReturnEvent()
        {
            // Arrange
            var eventId = 1;
            var service = new EventService(_eventEFRepository, _eventRepository, _layoutRepository, _layoutEFRepository, _eventAreaEFRepository, _areaEFRepository,
                            _seatEFRepository, _eventSeatEFRepository, _eventSeatRepository, _eventAreaRepository, _validator);

            // Act
            var evenT = await service.GetByIdAsync(eventId);

            // Assert
            evenT.Should().BeEquivalentTo(new EventDto
            {
                Id = 1,
                LayoutId = 1,
                Name = "First event",
                Description = "Event",
                DateStart = new DateTime(2030, 01, 01),
                DateEnd = new DateTime(2033, 01, 01),
                ImageURL = null,
                ShowTime = default,
            });
        }

        [Test]
        public async Task Edit_WhenEditEvent_ShouldReturnEventsListWithEditedEvent()
        {
            // Arrange
            var eventToEdit = new EventDto
            {
                Id = 1,
                Name = "First event1",
                Description = "Event1",
                LayoutId = 1,
                DateStart = new DateTime(2030, 01, 01),
                DateEnd = new DateTime(2033, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var eventtWas = new EventDto
            {
                Id = 1,
                LayoutId = 1,
                Name = "First event",
                Description = "Event",
                DateStart = new DateTime(2030, 01, 01),
                DateEnd = new DateTime(2033, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var service = new EventService(_eventEFRepository, _eventRepository, _layoutRepository, _layoutEFRepository, _eventAreaEFRepository, _areaEFRepository,
                            _seatEFRepository, _eventSeatEFRepository, _eventSeatRepository, _eventAreaRepository, _validator);

            // Act
            await service.EditAsync(eventToEdit);
            var events = (await service.GetAsync(eventToEdit.LayoutId)).ToList();
            await service.EditAsync(eventtWas);

            // Assert
            events.Should().BeEquivalentTo(new List<EventDto>
            {
                new EventDto
                {
                    Id = 1,
                    Name = "First event1",
                    Description = "Event1",
                    LayoutId = 1,
                    DateStart = new DateTime(2030, 01, 01),
                    DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                    ShowTime = new TimeSpan(11, 30, 00),
                },
                new EventDto
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = "https://static.kinoafisha.info/k/movie_posters/canvas/800x1200/upload/movie_posters/3/4/9/8362943/1f273c077a116fc68dd219e88ca47079.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
                new EventDto
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = "https://plaqat.ru/images/47675.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteEvent_ShouldReturnEventsListWithoutLastElement()
        {
            // Arrange
            var eventToDelete = new EventDto
            {
                LayoutId = 1,
                Name = "event",
                Description = "event",
                DateStart = new DateTime(2100, 01, 01),
                DateEnd = new DateTime(2101, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var service = new EventService(_eventEFRepository, _eventRepository, _layoutRepository, _layoutEFRepository, _eventAreaEFRepository, _areaEFRepository,
                            _seatEFRepository, _eventSeatEFRepository, _eventSeatRepository, _eventAreaRepository, _validator);

            // Act
            var last = await service.AddAsync(eventToDelete);
            await service.DeleteAsync(last.Id);
            var eventsWithoutLast = await service.GetAsync(eventToDelete.LayoutId);

            // Assert
            eventsWithoutLast.Should().BeEquivalentTo(new List<EventDto>
            {
                new EventDto
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                    ShowTime = new TimeSpan(11, 30, 00),
                },
                new EventDto
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = "https://static.kinoafisha.info/k/movie_posters/canvas/800x1200/upload/movie_posters/3/4/9/8362943/1f273c077a116fc68dd219e88ca47079.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
                new EventDto
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = "https://plaqat.ru/images/47675.jpg",
                    ShowTime = new TimeSpan(19, 30, 00),
                },
            });
        }
    }
}

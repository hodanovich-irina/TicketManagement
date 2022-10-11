using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.IntegrationTests
{
    /// <summary>
    /// Testing of event repository.
    /// </summary>
    [TestFixture]
    public class EventRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task Add_WhenAddNewEvent_ShouldReturnEventListWithNewEvent()
        {
            // Arrange
            var eventToAdd = new Event
            {
                Id = 1, LayoutId = 1, Name = "Name", Description = "First", DateStart = new DateTime(2035, 10, 10), DateEnd = new DateTime(2037, 10, 10), BaseAreaPrice = 2,
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var repository = new EventRepository(_connectionString);

            // Act
            await repository.AddAsync(eventToAdd);
            var events = await repository.GetAllByParentIdAsync(eventToAdd.LayoutId);
            var lastId = events.LastOrDefault().Id;
            await repository.DeleteAsync(lastId);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                new Event
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = lastId, LayoutId = 1, Name = "Name", Description = "First", DateStart = new DateTime(2035, 10, 10), DateEnd = new DateTime(2037, 10, 10),
                    ImageURL = null,
                    ShowTime = default,
                },
            });
        }

        [Test]
        public async Task GetAllByParentId_WhenEventtWithFirstLayoutId_ShouldReturnEventsList()
        {
            // Arrange
            var layoutId = 1;
            var repository = new EventRepository(_connectionString);

            // Act
            var events = await repository.GetAllByParentIdAsync(layoutId);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                new Event
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
            });
        }

        [Test]
        public async Task GetById_WhenEventWithFirsId_ShouldReturnEvent()
        {
            // Arrange
            var eventId = 1;
            var repository = new EventRepository(_connectionString);

            // Act
            var evenT = await repository.GetByIdAsync(eventId);

            // Assert
            evenT.Should().BeEquivalentTo(new Event
            {
                Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                ImageURL = null,
                ShowTime = default,
            });
        }

        [Test]
        public async Task Edit_WhenEditEvent_ShouldReturnEventsListWithEditedEvent()
        {
            // Arrange
            var eventToEdit = new Event
            {
                Id = 1, Name = "First event1", Description = "Event1", LayoutId = 1, DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var eventtWas = new Event
            {
                Id = 1, Name = "First event", Description = "Event",  LayoutId = 1, DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var repository = new EventRepository(_connectionString);

            // Act
            await repository.EditAsync(eventToEdit);
            var events = await repository.GetAllByParentIdAsync(eventToEdit.LayoutId);
            await repository.EditAsync(eventtWas);

            // Assert
            events.Should().BeEquivalentTo(new List<Event>
            {
                new Event
                {
                    Id = 1, LayoutId = 1, Name = "First event1", Description = "Event1", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteEvent_ShouldReturnEventsListWithoutLastElement()
        {
            // Arrange
            var eventToDelete = new Event
            {
                LayoutId = 1, Name = "event", Description = "event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                ImageURL = "https://avatars.mds.yandex.net/get-kinopoisk-image/4774061/a07b2623-1c2c-4e80-b14b-76193b6bfcae/600x900",
                ShowTime = new TimeSpan(11, 30, 00),
            };
            var repository = new EventRepository(_connectionString);

            // Act
            await repository.AddAsync(eventToDelete);
            var events = await repository.GetAllByParentIdAsync(eventToDelete.LayoutId);
            var lastId = events.LastOrDefault().Id;
            await repository.DeleteAsync(lastId);
            var eventsWithoutLast = await repository.GetAllByParentIdAsync(eventToDelete.LayoutId);

            // Assert
            eventsWithoutLast.Should().BeEquivalentTo(new List<Event>
            {
                new Event
                {
                    Id = 1, LayoutId = 1, Name = "First event", Description = "Event", DateStart = new DateTime(2030, 01, 01), DateEnd = new DateTime(2033, 01, 01),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 2, LayoutId = 1, Name = "Second event", Description = "Event1", DateStart = new DateTime(2034, 01, 01), DateEnd = new DateTime(2034, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
                new Event
                {
                    Id = 3, LayoutId = 1, Name = "Third event", Description = "Event3", DateStart = new DateTime(2035, 02, 02), DateEnd = new DateTime(2035, 06, 06),
                    ImageURL = null,
                    ShowTime = default,
                },
            });
        }
    }
}

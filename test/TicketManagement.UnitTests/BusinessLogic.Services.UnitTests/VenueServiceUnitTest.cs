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
    /// Testing of venue service.
    /// </summary>
    [TestFixture]
    public class VenueServiceUnitTest
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
        private IValidator<VenueDto> _validator;

        [SetUp]
        public void Setup()
        {
            _venueMock = new Mock<IRepository<Venue>>();
            _layoutMock = new Mock<IRepository<Layout>>();
            _seatMock = new Mock<IRepository<Seat>>();
            _areaMock = new Mock<IRepository<Area>>();
            _eventMock = new Mock<IRepository<Event>>();
            _eventAreaMock = new Mock<IRepository<EventArea>>();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
            _venueEFMock = new Mock<IEFRepository<Venue>>();
            _layoutEFMock = new Mock<IEFRepository<Layout>>();
            _seatEFMock = new Mock<IEFRepository<Seat>>();
            _areaEFMock = new Mock<IEFRepository<Area>>();
            _eventEFMock = new Mock<IEFRepository<Event>>();
            _eventAreaEFMock = new Mock<IEFRepository<EventArea>>();
            _eventSeatEFMock = new Mock<IEFRepository<EventSeat>>();
            _validator = new VenueValidation();
        }

        [Test]
        public async Task AddNewVenue_WhenAddVenue_ShouldAddedNewVenue()
        {
            // Arrange
            var venueToAdd = new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            var venues = new List<Venue> { venueToAdd };
            var dtoVenueToAdd = new VenueDto { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(new List<Venue>().AsQueryable()));
            _venueMock.Setup(venue => venue.AddAsync(venueToAdd)).Callback<Venue>(venue => venues.Add(venue));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            await venueService.AddAsync(dtoVenueToAdd);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
            });
        }

        [Test]
        public void AddNewVenueWithNotUniqueName_WhenAddVenue_ShouldThrowException()
        {
            // Arrange
            var venues = new List<Venue> { new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" } };
            var venueToAdd = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            var dtoVenueToAdd = new VenueDto { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _venueMock.Setup(venue => venue.AddAsync(venueToAdd)).Callback<Venue>(venue => venues.Add(venue));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.AddAsync(dtoVenueToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't add a new venue. This venue name alredy exist"));
        }

        [Test]
        public void EditVenue_WhenNameNotUnique_ShouldThrowException()
        {
            // Arrange
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            var venueToEdit = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            var dtoVenueToEdit = new VenueDto { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" };
            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _venueMock.Setup(venue => venue.EditAsync(venueToEdit)).Callback<Venue>(venue =>
            {
                var venueEdit = venues.FirstOrDefault(venueId => venueId.Id == venue.Id);
                venueEdit.Id = venue.Id;
                venueEdit.Address = venue.Address;
                venueEdit.Description = venue.Description;
                venueEdit.Name = venue.Name;
                venueEdit.Phone = venue.Phone;
            });
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.EditAsync(dtoVenueToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit venue. This venue name alredy exist"));
        }

        [Test]
        public async Task EditVenue_WhenNameUnique_ShouldEditVenue()
        {
            // Arrange
            var venue = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" };
            var venues = new List<Venue> { venue };
            var venueToEdit = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue2", Phone = "13 45 678 90 12" };
            var dtoVenueToEdit = new VenueDto { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue2", Phone = "13 45 678 90 12" };
            venue.Name = venueToEdit.Name;
            venue.Phone = venueToEdit.Phone;
            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _venueMock.Setup(venue => venue.EditAsync(venueToEdit));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            await venueService.EditAsync(dtoVenueToEdit);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue2", Phone = "13 45 678 90 12" },
            });
        }

        [Test]
        public async Task EditVenue_WhenNameByIdUnique_ShouldEditVenue()
        {
            // Arrange
            var venue = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" };
            var venues = new List<Venue> { venue };
            var venueToEdit = new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "13 45 678 90 12" };
            var dtoVenueToEdit = new VenueDto { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "13 45 678 90 12" };
            venue.Phone = venueToEdit.Phone;
            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            _venueMock.Setup(venue => venue.EditAsync(venueToEdit));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            await venueService.EditAsync(dtoVenueToEdit);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "13 45 678 90 12" },
            });
        }

        [Test]
        public void DeleteVenue_WhenDeleteVenue_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            };
            var events = new List<Event>
            {
                new Event { Id = 1, LayoutId = 1, BaseAreaPrice = 2, DateEnd = new DateTime(2021, 10, 10), DateStart = new DateTime(2021, 9, 9), Description ="Event", Name = "First event" },
            };
            var eventAreas = new List<EventArea>
            {
                new EventArea { Id = 1, CoordX = 1, CoordY = 2, Description = "First event area", EventId = 1, Price = 2 },
            };
            var eventSeats = new List<EventSeat> { new EventSeat { Id = 1, EventAreaId = 1, Number = 2, Row = 1, State = EventSeatState.Booked } };
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId == id).AsQueryable()));
            _eventEFMock.Setup(eventAll => eventAll.GetAsync(It.IsAny<Func<Event, bool>>())).Returns(Task.FromResult(events.Where(eventId => eventId.LayoutId == id).AsQueryable()));
            _eventAreaEFMock.Setup(eventArea => eventArea.GetAsync(It.IsAny<Func<EventArea, bool>>())).Returns(Task.FromResult(eventAreas.Where(eventArea => eventArea.EventId == id).AsQueryable()));
            _eventSeatEFMock.Setup(eventSeat => eventSeat.GetAsync(It.IsAny<Func<EventSeat, bool>>())).Returns(Task.FromResult(eventSeats.Where(seat => seat.EventAreaId == id).AsQueryable()));
            _venueMock.Setup(venue => venue.DeleteAsync(id)).Callback<int>(id => venues.Remove(venues.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.DeleteAsync(id);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't delete venue. Any seats booked"));
        }

        [Test]
        public async Task DeleteSeat_WhenDeleteSeat_ShouldDeleteSeat()
        {
            // Arrange
            int id = 1;
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };
            _venueMock.Setup(venue => venue.DeleteAsync(id)).Callback<int>(id => venues.Remove(venues.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            await venueService.DeleteAsync(id);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            });
        }

        [Test]
        public async Task ReturnVenueById_WhenReturnVenueByFirstId_ShouldReturnVenueWithFirstId()
        {
            // Arrange
            int venueId = 1;
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };

            _venueMock.Setup(venue => venue.GetByIdAsync(venueId)).Returns(Task.FromResult(venues.FirstOrDefault(venue => venue.Id.Equals(venueId))));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            var result = await venueService.GetByIdAsync(venueId);

            // Assert
            result.Should().BeEquivalentTo(venues.FirstOrDefault(venue => venue.Id == venueId));
        }

        [Test]
        public async Task ReturnAllVenue_WhenReturnVenues_ShouldReturnAllVenues()
        {
            // Arrange
            var venues = new List<Venue>
            {
                new Venue { Id = 1, Address = "First venue address", Description = "First venue", Name = "Name first venue", Phone = "123 45 678 90 12" },
                new Venue { Id = 2, Address = "First venue address", Description = "First venue", Name = "Name second venue", Phone = "123 45 678 90 12" },
            };

            _venueEFMock.Setup(venue => venue.GetAllAsync()).Returns(Task.FromResult(venues.AsQueryable()));
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            var result = await venueService.GetAllAsync();

            // Assert
            result.Should().BeEquivalentTo(venues);
        }

        [Test]
        public void VenueValidatorEdit_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto { Id = 0, Address = "Lenina street 12", Description = "First venue description", Name = "Concert hall", Phone = "+273628192" };

            // Act
            TestDelegate testAction = () => _validator.ValidateId(venue.Id);

            // Assert
            var ex = Assert.Throws<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void VenueEdit_WhenPhoneLengthMoreThenThirty_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto { Id = 1, Address = "Lenina street 12", Description = "First venue description", Name = "Concert hall", Phone = "+27362999999999999999999999999999998192" };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.EditAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Phone of venue must be less than 30"));
        }

        [Test]
        public void VenueAdd_WhenPhoneLengthMoreThenNeed_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto
            {
                Id = 1,
                Address = "Lenina street 12",
                Description = "First venue description",
                Name = "Concert halllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll" +
                "llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll",
                Phone = "+273628192",
            };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.AddAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Name of venue must be less than 120 and must be not null"));
        }

        [Test]
        public void VenueAdd_WhenDescriptionLengthMoreThenThirty_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto
            {
                Id = 1,
                Address = "Lenina street 12",
                Description = "First venue descriptionnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn",
                Name = "Concert hall",
                Phone = "+273628192",
            };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.AddAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of venue must be less than 120 and must be not null"));
        }

        [Test]
        public void VenueAdd_WhenAdressLengthMoreThenThirty_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto
            {
                Id = 1,
                Address = "Lenina street 12nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn" +
                "nnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnn",
                Description = "First venue description",
                Name = "Concert hall",
                Phone = "+273628192",
            };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.AddAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Address of venue must be less than 200 and must be not null"));
        }

        [Test]
        public void VenueDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto { Id = 0, Address = "Lenina street 12", Description = "First venue description", Name = "Concert hall", Phone = "+273628192" };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.DeleteAsync(venue.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void VenueEdit_WhenUseValidatorAndNameIsNull_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = new VenueDto { Id = 1, Address = "Lenina street 12", Description = null, Name = "Concert hall", Phone = "+273628192" };
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.EditAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of venue must be less than 120 and must be not null"));
        }

        [Test]
        public void VenueEdit_WhenUseValidatorAndVenueWasNull_ShouldThrowException()
        {
            // Arrange
            VenueDto venue = null;
            var venueService = new VenueService(_venueEFMock.Object, _venueMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object,
            _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object, _layoutEFMock.Object, _areaEFMock.Object, _seatEFMock.Object,
            _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => venueService.EditAsync(venue);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Venue was null"));
        }
    }
}

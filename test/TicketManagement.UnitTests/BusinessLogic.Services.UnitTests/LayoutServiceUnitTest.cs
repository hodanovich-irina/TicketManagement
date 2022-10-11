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
    /// Testing of layout service.
    /// </summary>
    [TestFixture]
    public class LayoutServiceUnitTest
    {
        private Mock<IRepository<Layout>> _layoutMock;
        private Mock<IRepository<Seat>> _seatMock;
        private Mock<IRepository<Area>> _areaMock;
        private Mock<IRepository<Event>> _eventMock;
        private Mock<IRepository<EventArea>> _eventAreaMock;
        private Mock<IRepository<EventSeat>> _eventSeatMock;
        private Mock<IEFRepository<Layout>> _layoutEFMock;
        private Mock<IEFRepository<Seat>> _seatEFMock;
        private Mock<IEFRepository<Area>> _areaEFMock;
        private Mock<IEFRepository<Event>> _eventEFMock;
        private Mock<IEFRepository<EventArea>> _eventAreaEFMock;
        private Mock<IEFRepository<EventSeat>> _eventSeatEFMock;
        private IValidator<LayoutDto> _validator;

        [SetUp]
        public void Setup()
        {
            _layoutMock = new Mock<IRepository<Layout>>();
            _seatMock = new Mock<IRepository<Seat>>();
            _areaMock = new Mock<IRepository<Area>>();
            _eventMock = new Mock<IRepository<Event>>();
            _eventAreaMock = new Mock<IRepository<EventArea>>();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
            _layoutEFMock = new Mock<IEFRepository<Layout>>();
            _seatEFMock = new Mock<IEFRepository<Seat>>();
            _areaEFMock = new Mock<IEFRepository<Area>>();
            _eventEFMock = new Mock<IEFRepository<Event>>();
            _eventAreaEFMock = new Mock<IEFRepository<EventArea>>();
            _eventSeatEFMock = new Mock<IEFRepository<EventSeat>>();
            _validator = new LayoutValidation();
        }

        [Test]
        public async Task AddNewLayout_WhenAddLayoutWithUniqueNameInVenue_ShouldAddedNewLayout()
        {
            // Arrange
            var layoutToAdd = new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 };
            var layouts = new List<Layout> { layoutToAdd };

            var dtoLayoutToAdd = new LayoutDto { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 };
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(new List<Layout>().AsQueryable()));
            _layoutMock.Setup(layout => layout.AddAsync(layoutToAdd)).Callback<Layout>(layout => layouts.Add(layout));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            await layoutService.AddAsync(dtoLayoutToAdd);

            // Assert
            layouts.Should().BeEquivalentTo(new List<LayoutDto>
            {
                new LayoutDto { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
            });
        }

        [Test]
        public void AddNewLayout_WhenAddLayoutWithNotUniqueNameInVenue_ShouldThrowException()
        {
            // Arrange
            var layouts = new List<Layout> { new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 } };
            var layoutToAdd = new Layout { Id = 2, Name = "First layout", Description = "Layout description", VenueId = 1 };
            var dtoLayoutToAdd = new LayoutDto { Id = 2, Name = "First layout", Description = "Layout description", VenueId = 1 };
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.AsQueryable()));
            _layoutMock.Setup(layout => layout.AddAsync(layoutToAdd)).Callback<Layout>(layout => layouts.Add(layout));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.AddAsync(dtoLayoutToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't add a new layuot. Layout with this name alredy exist in this venue"));
        }

        [Test]
        public void EditLayout_WhenLoyoutNameNotUnique_ShouldThrowException()
        {
            // Arrange
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 },
            };

            var layoutToEdit = new Layout { Id = 2, Name = "First layout", Description = "Layout description", VenueId = 1 };
            var dtoLayoutToEdit = new LayoutDto { Id = 2, Name = "First layout", Description = "Layout description", VenueId = 1 };
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.AsQueryable()));
            _layoutMock.Setup(layout => layout.EditAsync(layoutToEdit)).Callback<Layout>(layout =>
            {
                var layoutEdit = layouts.FirstOrDefault(layoutId => layoutId.Id == layout.Id);
                layoutEdit.Id = layout.Id;
                layoutEdit.Description = layout.Description;
                layoutEdit.Name = layout.Name;
            });
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.EditAsync(dtoLayoutToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit a layuot. Layout with this name alredy exist in this venue"));
        }

        [Test]
        public async Task EditLayout_WhenLayoutNameUnique_ShouldEditLayout()
        {
            // Arrange
            var layout = new Layout { Id = 2, Name = "Secon layout", Description = "Layout description", VenueId = 1 };
            var layouts = new List<Layout> { layout };
            var layoutToEdit = new Layout { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 };
            var dtoLayoutToEdit = new LayoutDto { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 };
            layout.Name = layoutToEdit.Name;
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.AsQueryable()));
            _layoutMock.Setup(layout => layout.EditAsync(layoutToEdit));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            await layoutService.EditAsync(dtoLayoutToEdit);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 },
            });
        }

        [Test]
        public async Task EditLayout_WhenLayoutNameByIdUniqueInVenue_ShouldEditLayout()
        {
            // Arrange
            var layout = new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 };
            var layouts = new List<Layout> { layout };
            var layoutToEdit = new Layout { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 };
            var dtoLayoutToEdit = new LayoutDto { Id = 2, Name = "Second layout", Description = "Layout description", VenueId = 1 };
            layout.Description = layoutToEdit.Description;
            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.AsQueryable()));
            _layoutMock.Setup(layout => layout.EditAsync(layoutToEdit));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            await layoutService.EditAsync(dtoLayoutToEdit);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                layoutToEdit,
            });
        }

        [Test]
        public void DeleteLayout_WhenDeleteLayout_ShouldThrowException()
        {
            // Arrange
            int id = 1;
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 },
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
            _layoutMock.Setup(layout => layout.DeleteAsync(id)).Callback<int>(id => layouts.Remove(layouts.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.DeleteAsync(id);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't delete layout. Any seats booked"));
        }

        [Test]
        public async Task DeleteLayout_WhenDeleteLayout_ShouldDeleteLayout()
        {
            // Arrange
            var id = 1;
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 },
            };
            _layoutMock.Setup(layout => layout.DeleteAsync(id)).Callback<int>(id => layouts.Remove(layouts.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            await layoutService.DeleteAsync(id);

            // Assert
            layouts.Should().BeEquivalentTo(new List<Layout>
            {
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 },
            });
        }

        [Test]
        public async Task ReturnLayoutById_WhenReturnLayoutByFirstId_ShouldReturnLayoutWithFirstId()
        {
            // Arrange
            var layoutId = 1;
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 },
            };

            _layoutMock.Setup(layout => layout.GetByIdAsync(layoutId)).Returns(Task.FromResult(layouts.FirstOrDefault(layout => layout.Id == layoutId)));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            var result = await layoutService.GetByIdAsync(layoutId);

            // Assert
            result.Should().BeEquivalentTo(layouts.FirstOrDefault(layout => layout.Id == layoutId));
        }

        [Test]
        public async Task ReturnAllLayoutByVenueId_WhenReturnLayoutWithFirstVenueId_ShouldReturnLayoutWithFirstVenueId()
        {
            // Arrange
            var venueId = 1;
            var layouts = new List<Layout>
            {
                new Layout { Id = 1, Name = "First layout", Description = "Layout description", VenueId = 1 },
                new Layout { Id = 2, Name = "Second layout", Description = "Layout description2", VenueId = 1 },
            };

            _layoutEFMock.Setup(layout => layout.GetAsync(It.IsAny<Func<Layout, bool>>())).Returns(Task.FromResult(layouts.Where(layout => layout.VenueId.Equals(venueId)).AsQueryable()));
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            var result = await layoutService.GetAsync(venueId);

            // Assert
            result.Should().BeEquivalentTo(layouts.Where(layout => layout.VenueId == venueId));
        }

        [Test]
        public void LayoutEdit_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = new LayoutDto { Id = 0, Name = "Name of first layout", Description = "Description of first layout", VenueId = 0 };
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.EditAsync(layout);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void LayoutEdit_WhenNameLengthMoreThenThirty_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = new LayoutDto
            {
                Id = 1,
                Name = "Concert halllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll" +
                "llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll",
                Description = "Description of first layout",
                VenueId = 2,
            };
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.EditAsync(layout);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Name of layout must be less than 120 sumbols and must be not null"));
        }

        [Test]
        public void LayoutAdd_WhenDescriptionLengthMoreThenNeed_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = new LayoutDto
            {
                Id = 1,
                Description = @"First cgvhn descriptionlllllkklllllkjjjjjjjjjjjjjjjjjhgfdsllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll
                llllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllllll",
                Name = "Concert hall",
                VenueId = 2,
            };
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.AddAsync(layout);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of layout must be less than 120 sumbols and must be not null"));
        }

        [Test]
        public void LayoutDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = new LayoutDto { Id = 0, Name = "Name of first layout", Description = "Description of first layout", VenueId = 2 };
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.DeleteAsync(layout.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void LayoutEdit_WhenUseValidatorAndNameIsNull_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = new LayoutDto { Id = 0, Name = "Name of first layout", Description = null, VenueId = 0 };
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.EditAsync(layout);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void LayoutEdit_WhenUseValidatorAndLayoutWasNull_ShouldThrowException()
        {
            // Arrange
            LayoutDto layout = null;
            var layoutService = new LayoutService(_layoutEFMock.Object, _layoutMock.Object, _areaMock.Object, _seatMock.Object, _eventMock.Object, _eventSeatMock.Object, _eventAreaMock.Object,
                 _seatEFMock.Object, _eventEFMock.Object, _eventSeatEFMock.Object, _eventAreaEFMock.Object, _areaEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => layoutService.EditAsync(layout);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Layout was null"));
        }
    }
}

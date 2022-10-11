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
    /// Testing of ticket service.
    /// </summary>
    [TestFixture]
    public class TicketServiceUnitTest
    {
        private Mock<IRepository<Ticket>> _ticketMock;
        private Mock<IRepository<EventSeat>> _eventSeatMock;
        private Mock<IEFRepository<Ticket>> _ticketEFMock;
        private IValidator<TicketDto> _validator;
        private ITicketService _ticketService;

        [SetUp]
        public void Setup()
        {
            _ticketMock = new Mock<IRepository<Ticket>>();
            _eventSeatMock = new Mock<IRepository<EventSeat>>();
            _ticketEFMock = new Mock<IEFRepository<Ticket>>();
            _validator = new TicketValidation();
        }

        [Test]
        public async Task AddNewTicket_WhenAddTicketForFreeSeat_ShouldAddedNewTicket()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Free };
            var ticketToAdd = new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            var tickets = new List<Ticket> { ticketToAdd };

            var dtoTicketToAdd = new TicketDto { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToAdd.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.AddAsync(ticketToAdd)).Callback<Ticket>(ticket => tickets.Add(ticket));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            await _ticketService.AddAsync(dtoTicketToAdd);

            // Assert
            tickets.Should().BeEquivalentTo(new List<TicketDto>
            {
                dtoTicketToAdd,
            });
        }

        [Test]
        public void AddNewTicket_WhenAddTicketForBookedSeat_ShouldThrowException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var ticketToAdd = new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            var tickets = new List<Ticket> { ticketToAdd };

            var dtoTicketToAdd = new TicketDto { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToAdd.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.AddAsync(ticketToAdd)).Callback<Ticket>(ticket => tickets.Add(ticket));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.AddAsync(dtoTicketToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't buy ticket for event. This seat is booked"));
        }

        [Test]
        public async Task DeleteTicket_WhenDeleteTicket_ShouldDeleteTicket()
        {
            // Arrange
            var id = 1;
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var ticketToDelete = new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" },
            };
            _ticketMock.Setup(ticket => ticket.GetByIdAsync(ticketToDelete.Id)).Returns(Task.FromResult(ticketToDelete));
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToDelete.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.DeleteAsync(id)).Callback<int>(id => tickets.Remove(tickets.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            await _ticketService.DeleteAsync(id);

            // Assert
            tickets.Should().BeEquivalentTo(new List<Ticket>());
        }

        [Test]
        public void DeleteTicket_WhenDeleteTicket_ShouldThrowException()
        {
            // Arrange
            var id = 1;
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var ticketToDelete = new Ticket();
            var tickets = new List<Ticket>
            {
                ticketToDelete,
            };
            _ticketMock.Setup(ticket => ticket.GetByIdAsync(ticketToDelete.Id)).Returns(Task.FromResult(ticketToDelete));
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToDelete.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.DeleteAsync(id)).Callback<int>(id => tickets.Remove(tickets.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.DeleteAsync(id);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Incorrect data"));
        }

        [Test]
        public void DeleteTicket_WhenDeleteTicketWithNoValidId_ShouldThrowException()
        {
            // Arrange
            var id = -1;
            var eventSeat = new EventSeat { Id = -1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var ticketToDelete = new Ticket { Id = -1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            var tickets = new List<Ticket>
            {
                ticketToDelete,
            };
            _ticketMock.Setup(ticket => ticket.GetByIdAsync(ticketToDelete.Id)).Returns(Task.FromResult(ticketToDelete));
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToDelete.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.DeleteAsync(id)).Callback<int>(id => tickets.Remove(tickets.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.DeleteAsync(id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void AddTicket_WhenDeleteTicketWithNoValidPrice_ShouldThrowException()
        {
            // Arrange
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Free };
            var ticketToAdd = new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = -1, UserId = "1" };
            var tickets = new List<Ticket> { ticketToAdd };

            var dtoTicketToAdd = new TicketDto { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = -1, UserId = "1" };
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToAdd.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.AddAsync(ticketToAdd)).Callback<Ticket>(ticket => tickets.Add(ticket));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.AddAsync(dtoTicketToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Price must be more than zero"));
        }

        [Test]
        public void EditTicket_WhenEditTicket_ShouldThrowException()
        {
            // Arrange
            var ticketToEdit = new TicketDto { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" };
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.EditAsync(ticketToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit ticket. If you want buy any ticket, delete this ticket and add another one."));
        }

        [Test]
        public async Task ReturnTicketById_WhenReturnTicketByFirstId_ShouldReturnTickettWithFirstId()
        {
            // Arrange
            var ticketId = 1;
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" },
            };

            _ticketMock.Setup(layout => layout.GetByIdAsync(ticketId)).Returns(Task.FromResult(tickets.FirstOrDefault(ticket => ticket.Id == ticketId)));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            var result = await _ticketService.GetByIdAsync(ticketId);

            // Assert
            result.Should().BeEquivalentTo(tickets.FirstOrDefault(ticket => ticket.Id == ticketId));
        }

        [Test]
        public async Task ReturnAllTicketByEventSeatId_WhenReturnTicketWithFirstEventSeatId_ShouldReturnTicketWithFirstEventSeatId()
        {
            // Arrange
            var eventSeatId = 1;
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" },
            };

            _ticketEFMock.Setup(ticket => ticket.GetAsync(It.IsAny<Func<Ticket, bool>>())).Returns(Task.FromResult(tickets.Where(ticket => ticket.EventSeatId.Equals(eventSeatId)).AsQueryable()));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            var result = await _ticketService.GetAsync(eventSeatId);

            // Assert
            result.Should().BeEquivalentTo(tickets.Where(ticket => ticket.EventSeatId == eventSeatId));
        }

        [Test]
        public async Task ReturnAllTicketByUserId_WhenReturnTicketWithFirstUserId_ShouldReturnTicketWithFirstUserId()
        {
            // Arrange
            var userId = "1";
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1, UserId = "1" },
            };

            _ticketEFMock.Setup(ticket => ticket.GetAsync(It.IsAny<Func<Ticket, bool>>())).Returns(Task.FromResult(tickets.Where(ticket => ticket.UserId.Equals(userId)).AsQueryable()));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            var result = await _ticketService.GetByParentStringIdAsync(userId);

            // Assert
            result.Should().BeEquivalentTo(tickets.Where(ticket => ticket.UserId == userId));
        }

        [Test]
        public void ReturnAllTicketByUserId_WhenReturnTicketWithFirstUserId_ShouldThrowException()
        {
            // Arrange
            var userId = "";
            var eventSeat = new EventSeat { Id = 1, EventAreaId = 1, Number = 1, Row = 1, State = EventSeatState.Booked };
            var ticketToAdd = new Ticket { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1 };
            var tickets = new List<Ticket> { };

            var dtoTicketToAdd = new TicketDto { Id = 1, DateOfPurchase = new DateTime(2022, 01, 25), EventSeatId = 1, Price = 1 };
            _eventSeatMock.Setup(eventSeat => eventSeat.GetByIdAsync(ticketToAdd.EventSeatId)).Returns(Task.FromResult(eventSeat));
            _eventSeatMock.Setup(eventSeatToEdit => eventSeatToEdit.EditAsync(eventSeat));
            _ticketMock.Setup(ticket => ticket.AddAsync(ticketToAdd)).Callback<Ticket>(ticket => tickets.Add(ticket));
            _ticketEFMock.Setup(ticket => ticket.GetAsync(It.IsAny<Func<Ticket, bool>>())).Returns(Task.FromResult(tickets.Where(ticket => ticket.UserId.Equals(userId)).AsQueryable()));
            _ticketService = new TicketService(_ticketEFMock.Object, _ticketMock.Object, _eventSeatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => _ticketService.AddAsync(dtoTicketToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("User was null"));
        }
    }
}

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
    /// Testing of seat service.
    /// </summary>
    [TestFixture]
    public class SeatServiceUnitTest
    {
        private Mock<IRepository<Seat>> _seatMock;
        private Mock<IEFRepository<Seat>> _seatEFMock;
        private IValidator<SeatDto> _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new SeatValidation();
            _seatEFMock = new Mock<IEFRepository<Seat>>();
            _seatMock = new Mock<IRepository<Seat>>();
        }

        [Test]
        public async Task AddNewSeat_WhenAddNewSeatWithValidData_AddedNewSeat()
        {
            // Arrange
            var seatToAdd = new Seat { Id = 1, AreaId = 1, Number = 1, Row = 2 };
            var seats = new List<Seat> { seatToAdd };
            var dtoSeatToAdd = new SeatDto { Id = 1, AreaId = 1, Number = 1, Row = 2 };

            _seatEFMock.Setup(seat => seat.GetAsync(It.IsAny<Func<Seat, bool>>())).Returns(Task.FromResult(new List<Seat>().AsQueryable()));
            _seatMock.Setup(seat => seat.AddAsync(seatToAdd)).Callback<Seat>(seat => seats.Add(seat));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            await seatService.AddAsync(dtoSeatToAdd);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 2 },
            });
        }

        [Test]
        public void AddNewSeat_WhenAddNewSeatWithNoValidData_ShouldThrowException()
        {
            // Arrange
            var areaId = 1;
            var seats = new List<Seat> { new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 } };
            var seatToAdd = new Seat { Id = 2, AreaId = 1, Number = 1, Row = 1 };
            var dtoSeatToAdd = new SeatDto { Id = 2, AreaId = 1, Number = 1, Row = 1 };

            _seatEFMock.Setup(seat => seat.GetAsync(It.IsAny<Func<Seat, bool>>())).Returns(Task.FromResult(seats.Where(seatForId => seatForId.AreaId == areaId).AsQueryable()));
            _seatMock.Setup(seat => seat.AddAsync(seatToAdd)).Callback<Seat>(seat => seats.Add(seat));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.AddAsync(dtoSeatToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't add a new seat. Seat with this row and number alredy exsist in this area"));
        }

        [Test]
        public void DeleteSeat_WhenDeleteSeat_ShouldThrowException()
        {
            // Arrange
            var seatId = 0;
            var seats = new List<Seat> { new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 } };

            _seatMock.Setup(seat => seat.GetByIdAsync(seatId)).Returns(Task.FromResult(seats.FirstOrDefault(seatForId => seatForId.Id == seatId)));
            _seatMock.Setup(seat => seat.DeleteAsync(seatId)).Callback<int>(id => seats.Remove(seats.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.DeleteAsync(seatId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public async Task DeleteSeat_WhenDeleteSeat_ShouldDeleteSeat()
        {
            // Arrange
            var seatId = 1;
            var seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 },
                new Seat { Id = 2, AreaId = 1, Number = 1, Row = 1 },
            };

            _seatMock.Setup(seat => seat.GetByIdAsync(seatId)).Returns(Task.FromResult(seats.FirstOrDefault(seatForId => seatForId.Id == seatId)));
            _seatMock.Setup(seat => seat.DeleteAsync(seatId)).Callback<int>(id => seats.Remove(seats.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            await seatService.DeleteAsync(seatId);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 2, AreaId = 1, Number = 1, Row = 1 },
            });
        }

        [Test]
        public void EditSeat_WhenRowAndNumberNotUnique_ShouldThrowException()
        {
            // Arrange
            var seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 },
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
            };
            var dtoSeatToEdit = new SeatDto { Id = 1, AreaId = 1, Number = 2, Row = 1 };

            _seatEFMock.Setup(seat => seat.GetAsync(It.IsAny<Func<Seat, bool>>())).Returns(Task.FromResult(seats.AsQueryable()));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.EditAsync(dtoSeatToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit this seat. Seat with this row and number alredy exist in this area"));
        }

        [Test]
        public async Task EditSeat_WhenRowAndNumberUnique_ShouldThrowException()
        {
            // Arrange
            var seat = new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 };
            var seats = new List<Seat> { seat };
            var seatToEdit = new Seat { Id = 1, AreaId = 1, Number = 3, Row = 1 };
            var dtoSeatToEdit = new SeatDto { Id = 1, AreaId = 1, Number = 3, Row = 1 };
            seat.Number = seatToEdit.Number;

            _seatEFMock.Setup(seat => seat.GetAsync(It.IsAny<Func<Seat, bool>>())).Returns(Task.FromResult(seats.AsQueryable()));
            _seatMock.Setup(seat => seat.EditAsync(seatToEdit));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            await seatService.EditAsync(dtoSeatToEdit);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                seatToEdit,
            });
        }

        [Test]
        public async Task ReturnSeatFromFirstArea_WhenReturnSeatFromFirstArea_ShouldReturnSeatWithFirstAreaId()
        {
            // Arrange
            var areaId = 1;
            var seats = new List<Seat>
            {
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 1, AreaId = 2, Number = 2, Row = 1 },
            };

            _seatEFMock.Setup(seat => seat.GetAsync(It.IsAny<Func<Seat, bool>>())).Returns(Task.FromResult(seats.Where(seatForId => seatForId.AreaId == areaId).AsQueryable()));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            var result = await seatService.GetAsync(areaId);

            // Assert
            result.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
            });
        }

        [Test]
        public async Task ReturnEventSeatById_WhenReturnEventSeatWithFirstId_ShouldReturnEventSeatWithFirstId()
        {
            // Arrange
            var seatId = 1;
            var seats = new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 2, AreaId = 2, Number = 2, Row = 1 },
            };

            _seatMock.Setup(seat => seat.GetByIdAsync(seatId)).Returns(Task.FromResult(seats.FirstOrDefault(seat => seat.Id == seatId)));
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            var result = await seatService.GetByIdAsync(seatId);

            // Assert
            result.Should().BeEquivalentTo(seats.FirstOrDefault(seat => seat.Id == seatId));
        }

        [Test]
        public void SeatAdd_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            SeatDto seat = new SeatDto { Id = 0, Number = 1, Row = 1, AreaId = 0 };
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.AddAsync(seat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void SeatEdit_WhenUseValidatorRowAndNumbersLessThenOne_ShouldThrowException()
        {
            // Arrange
            SeatDto seat = new SeatDto { Id = 1, Number = 1, Row = 0, AreaId = 2 };
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.EditAsync(seat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Number and row of seat must be more than zero"));
        }

        [Test]
        public void EventSeatDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            SeatDto seat = new SeatDto { Id = -1, Number = 1, Row = 1, AreaId = 2 };
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.DeleteAsync(seat.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void SeatEdit_WhenUseValidatorAndSeatWasNull_ShouldThrowException()
        {
            // Arrange
            SeatDto seat = null;
            var seatService = new SeatService(_seatEFMock.Object, _seatMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => seatService.EditAsync(seat);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Seat was null"));
        }
    }
}

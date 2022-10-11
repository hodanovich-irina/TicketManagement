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
    /// Testing of seat repository.
    /// </summary>
    [TestFixture]
    public class SeatRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task GetAllByParentId_WhenSeatWithFirstAreaId_ShouldReturnSeatsList()
        {
            // Arrange
            var areaId = 1;
            var repository = new SeatRepository(_connectionString);

            // Act
            var seats = await repository.GetAllByParentIdAsync(areaId);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 },
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1 },
                new Seat { Id = 4, AreaId = 1, Number = 2, Row = 2 },
                new Seat { Id = 6, AreaId = 1, Number = 1, Row = 2 },
            });
        }

        [Test]
        public async Task GetById_WhenSeattWithFirsId_ShouldReturnSeat()
        {
            // Arrange
            var seatId = 1;
            var repository = new SeatRepository(_connectionString);

            // Act
            var seat = await repository.GetByIdAsync(seatId);

            // Assert
            seat.Should().BeEquivalentTo(new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 });
        }

        [Test]
        public async Task Add_WhenAddNewSeat_ShouldReturnSeatsListWithNewSeat()
        {
            // Arrange
            var seat = new Seat { AreaId = 1, Number = 4, Row = 4 };
            var repository = new SeatRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(seat);
            var seats = await repository.GetAllByParentIdAsync(seat.AreaId);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 },
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1 },
                new Seat { Id = 4, AreaId = 1, Number = 2, Row = 2 },
                new Seat { Id = 6, AreaId = 1, Number = 1, Row = 2 },
                new Seat { Id = lastId.Id, AreaId = 1, Number = 4, Row = 4 },
            });
        }

        [Test]
        public async Task Edit_WhenEditSeat_ShouldReturnSeatsListWithEditedSeat()
        {
            // Arrange
            var seat = new Seat { Id = 1, AreaId = 1, Number = 4, Row = 4 };
            var seatWas = new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 };
            var repository = new SeatRepository(_connectionString);

            // Act
            await repository.EditAsync(seat);
            var seats = await repository.GetAllByParentIdAsync(seat.AreaId);
            await repository.EditAsync(seatWas);

            // Assert
            seats.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 4, Row = 4 },
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1 },
                new Seat { Id = 4, AreaId = 1, Number = 2, Row = 2 },
                new Seat { Id = 6, AreaId = 1, Number = 1, Row = 2 },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteSeat_ShouldReturnSeatsListWithoutLastElement()
        {
            // Arrange
            var seat = new Seat { AreaId = 1, Number = 4, Row = 4 };
            var repository = new SeatRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(seat);
            await repository.DeleteAsync(lastId.Id);
            var seatsWithoutLast = await repository.GetAllByParentIdAsync(seat.AreaId);

            // Assert
            seatsWithoutLast.Should().BeEquivalentTo(new List<Seat>
            {
                new Seat { Id = 1, AreaId = 1, Number = 1, Row = 1 },
                new Seat { Id = 2, AreaId = 1, Number = 2, Row = 1 },
                new Seat { Id = 3, AreaId = 1, Number = 3, Row = 1 },
                new Seat { Id = 4, AreaId = 1, Number = 2, Row = 2 },
                new Seat { Id = 6, AreaId = 1, Number = 1, Row = 2 },
            });
        }
    }
}

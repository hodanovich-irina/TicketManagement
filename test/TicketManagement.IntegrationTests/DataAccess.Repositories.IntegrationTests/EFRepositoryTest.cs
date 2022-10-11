using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using TicketManagement.DataAccess.Models;
using TicketManagement.DataAccess.Repositories;

namespace TicketManagement.IntegrationTests.DataAccess.Repositories.IntegrationTests
{
    [TestFixture]
    public class EFRepositoryTest
    {
        private TicketManagementContext _context;
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
            _context = new TicketManagementContext(new DbContextOptionsBuilder<TicketManagementContext>().UseSqlServer(_connectionString).Options);
        }

        [Test]
        public async Task Add_WhenAddNewVenue_ShouldReturnVenuesListWithNewVenue2()
        {
            // Arrange
            var venue = new Area { LayoutId = 1, CoordX = 231, CoordY = 123, Description = "lalallala" };
            var repository = new Repository<Area>(_context);

            // Act
            var lastId = await repository.AddAsync(venue);
            await repository.DeleteAsync(lastId.Id);

            // Assert
            lastId.Id.Should().NotBe(0);
        }
    }
}

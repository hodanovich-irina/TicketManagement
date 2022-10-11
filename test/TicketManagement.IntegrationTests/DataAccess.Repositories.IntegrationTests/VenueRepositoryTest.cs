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
    /// Testing of venue repository.
    /// </summary>
    [TestFixture]
    public class VenueRepositoryTest
    {
        private string _connectionString;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            _connectionString = configuration.GetConnectionString("TestDatabase");
        }

        [Test]
        public async Task GetAllAsync_WhenVenueGet_ShouldReturnVenuesList()
        {
            // Arrange
            var repository = new VenueRepository(_connectionString);

            // Act
            var venues = await repository.GetAllAsync();

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
            });
        }

        [Test]
        public async Task GetById_WhenVenueWithFirsId_ShouldReturnVenue()
        {
            // Arrange
            var venueId = 1;
            var repository = new VenueRepository(_connectionString);

            // Act
            var venue = await repository.GetByIdAsync(venueId);

            // Assert
            venue.Should().BeEquivalentTo(new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" });
        }

        [Test]
        public async Task Add_WhenAddNewVenue_ShouldReturnVenuesListWithNewVenue()
        {
            // Arrange
            var venue = new Venue { Name = "Name1 first venue", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" };
            var repository = new VenueRepository(_connectionString);

            // Act
            var lastId = await repository.AddAsync(venue);
            var venues = await repository.GetAllAsync();
            await repository.DeleteAsync(lastId.Id);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
                new Venue { Id = lastId.Id, Name = "Name1 first venue", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" },
            }.AsQueryable());
        }

        [Test]
        public async Task Edit_WhenEditVenue_ShouldReturnVenuesListWithEditedVenue()
        {
            // Arrange
            var venue = new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "111 45 678 90 12" };
            var venueWas = new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" };
            var repository = new VenueRepository(_connectionString);

            // Act
            await repository.EditAsync(venue);
            var venues = await repository.GetAllAsync();
            await repository.EditAsync(venueWas);

            // Assert
            venues.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "111 45 678 90 12" },
            });
        }

        [Test]
        public async Task Delete_WhenDeleteVenue_ShouldReturnVenuesListWithoutLastElement()
        {
            // Arrange
            var venue = new Venue { Name = "Name1 first venue", Address = "First venue address", Description = "First1 venue", Phone = "123 45 678 90 12" };
            var repository = new VenueRepository(_connectionString);

            // Act
            await repository.AddAsync(venue);
            var venues = await repository.GetAllAsync();
            var lastId = venues.LastOrDefault().Id;
            await repository.DeleteAsync(lastId);
            var venuesWithoutLast = await repository.GetAllAsync();

            // Assert
            venuesWithoutLast.Should().BeEquivalentTo(new List<Venue>
            {
                new Venue { Id = 1, Name = "Name first venue", Address = "First venue address", Description = "First venue", Phone = "123 45 678 90 12" },
            });
        }
    }
}

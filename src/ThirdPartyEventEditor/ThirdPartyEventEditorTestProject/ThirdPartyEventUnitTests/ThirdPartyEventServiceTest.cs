using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ThirdPartyEventEditor.Interfaces;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Services;
using ThirdPartyEventEditor.Validators;
using ThirdPartyEventEditor.Exceptions;

namespace ThirdPartyEventEditorTestProject.ThirdPartyEventUnitTests
{
    /// <summary>
    /// Testing of third party event service.
    /// </summary>
    [TestFixture]
    class ThirdPartyEventServiceTest
    {
        private Mock<IRepository<ThirdPartyEvent>> _repositoryMock;
        private Mock<IThirdPartyEventFileToExportCreator<ThirdPartyEvent>> _importRepositoryMock;
        private IValidator<ThirdPartyEvent> _validator;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository<ThirdPartyEvent>>();
            _importRepositoryMock = new Mock<IThirdPartyEventFileToExportCreator<ThirdPartyEvent>>();
            _validator = new ThirdPartyEventValidator();
        }

        [Test]
        public void ReturnAllThirdPartyEvent_WhenReturnThirdPartyEvent_ShouldReturnThirdPartyEvent()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x=>x));
            var areaService = new ThirdPartyEventService( _repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            var result = areaService.GetAll();

            // Assert
            result.Should().BeEquivalentTo(thirdPartyEvents);
        }

        [Test]
        public void  DeleteThirdPartyEvent_WhenDeleteThirdPartyEvent_ShouldDeleteThirdPartyEvent()
        {
            // Arrange
            var id = 1;
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x=>x));
            var deleteEvent = thirdPartyEvents.Where(_event => !_event.Id.Equals(id)).Select(events => events);
            _repositoryMock.Setup(repository => repository.Write(deleteEvent)).Callback<IEnumerable<ThirdPartyEvent>>(del => thirdPartyEvents.Remove(thirdPartyEvents.FirstOrDefault(idForDelete => idForDelete.Id == id)));

            var thirdPartyEventService = new ThirdPartyEventService( _repositoryMock.Object, _importRepositoryMock.Object, _validator);
            
            // Act
            var result = thirdPartyEventService.Delete(id);

            // Assert
            result.Should().Be(true);
        }
        
        [Test]
        public void ReturnThirdPartyEventByFirstId_WhenReturnThirdPartyEventByFirstId_ShouldReturnThirdPartyEventByFirstId()
        {
            // Arrange
            var id = 1;
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x=>x));
            var thirdPartyEventService = new ThirdPartyEventService( _repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            var result = thirdPartyEventService.GetById(id);

            // Assert
            result.Should().BeEquivalentTo(thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(id)));
        }


        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidDate_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "img",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 3,
                LayoutName = "layout",
                VenueName = "venue",
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create event in the past and when start date more than end date"));
        }

        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidLayoutName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "img",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 3,
                LayoutName = null,
                VenueName = "venue",
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Layout name of third party event must be not null"));
        }

        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidVenueName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "img",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 3,
                LayoutName = "layout",
                VenueName = null,
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Venue name of third party event must be not null"));
        }


        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWasNull_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            ThirdPartyEvent newEvent = null;
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Third party event was null"));
        }


        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin on
                        Tsvetnoy Boulevard presents a new circus program  Almoust serious , dedicated to the 100th anniversary of the birth of Yuri Nikulin!
                        The program includes trained horses, bears, goats, a Brazilian wheel of courage, a moto ball, equilibrists on a tightrope,  acrobats on a mast,
                        aerialists,jugglers and clowns! Hurry!",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "img",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 3,
                LayoutName = "layout",
                VenueName = "venue",
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Name of third party event must be less than 120 sumbols and must be not null"));
        }

        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidDescription_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = "15 мая",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "img",
                Description = null,
                Id = 3,
                LayoutName = "layout",
                VenueName = "venue",
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of third party event must be not null"));
        }

        [Test]
        public void AddThirdPartyEvent_WhenAddThirdPartyEventWithInvalidImage_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "img",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var newEvent = new ThirdPartyEvent
            {
                Name = "15 мая",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = null,
                Description = "...",
                Id = 3,
                LayoutName = "layout",
                VenueName = "venue",
            };
            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.Add(newEvent);
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Add(newEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Image url of third party event must be not null"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidLayoutName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "film.png",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 2,
                LayoutName = null,
                VenueName = "venue",
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).LayoutName = editEvent.LayoutName;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Layout name of third party event must be not null"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidVenueName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "film.png",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 2,
                LayoutName = "layout",
                VenueName = null,
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).VenueName = editEvent.VenueName;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Venue name of third party event must be not null"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidDate_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2020, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "film.png",
                Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                Id = 2,
                LayoutName = "...",
                VenueName = "venue",
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).EndDate = editEvent.EndDate;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't create event in the past and when start date more than end date"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidDescription_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "film.png",
                Description = null,
                Id = 2,
                LayoutName = "...",
                VenueName = "venue",
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).LayoutName = editEvent.LayoutName;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of third party event must be not null"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidImage_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = "Almoust serious",
                EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = null,
                Description = "...",
                Id = 2,
                LayoutName = "...",
                VenueName = "venue",
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).LayoutName = editEvent.LayoutName;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Image url of third party event must be not null"));
        }

        [Test]
        public void EditThirdPartyEvent_WhenEditThirdPartyEventWithInvalidName_ShouldThrowException()
        {
            // Arrange
            var thirdPartyEvents = new List<ThirdPartyEvent>
            {
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 1,
                    LayoutName = "layout",
                    VenueName = "venue",
                },
                new ThirdPartyEvent
                {
                    Name = "Almoust serious",
                    EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                    StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                    PosterImage = "film.png",
                    Description = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin!",
                    Id = 2,
                    LayoutName = "layout",
                    VenueName = "venue",
                }
            };
            var editEvent = new ThirdPartyEvent
            {
                Name = @"From May 15 to August 1, the Belarusian State Circus and the Moscow Circus of Y. Nikulin on
                        Tsvetnoy Boulevard presents a new circus program  Almoust serious , dedicated to the 100th anniversary of the birth of Yuri Nikulin!
                        The program includes trained horses, bears, goats, a Brazilian wheel of courage, a moto ball, equilibrists on a tightrope,  acrobats on a mast,
                        aerialists,jugglers and clowns! Hurry!",
                EndDate = new DateTime(2022, 06, 30, 21, 00, 00),
                StartDate = new DateTime(2022, 05, 30, 15, 00, 00),
                PosterImage = "film.png",
                Description = "...",
                Id = 2,
                LayoutName = "...",
                VenueName = "venue",
            };

            _repositoryMock.Setup(repository => repository.Read()).Returns(thirdPartyEvents.Select(x => x));
            thirdPartyEvents.FirstOrDefault(x => x.Id.Equals(editEvent.Id)).LayoutName = editEvent.LayoutName;
            _repositoryMock.Setup(repository => repository.Write(thirdPartyEvents));

            var thirdPartyEventService = new ThirdPartyEventService(_repositoryMock.Object, _importRepositoryMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = () => thirdPartyEventService.Edit(editEvent);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Name of third party event must be less than 120 sumbols and must be not null"));
        }
    }
}

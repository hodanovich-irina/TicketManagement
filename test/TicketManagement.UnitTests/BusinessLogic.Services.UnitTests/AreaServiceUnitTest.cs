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

namespace TicketManagement.UnitTests.BusinessLogic.Services.UnitTest
{
    /// <summary>
    /// Testing of area service.
    /// </summary>
    [TestFixture]
    public class AreaServiceUnitTest
    {
        private Mock<IRepository<Seat>> _seatMock;
        private Mock<IRepository<Area>> _areaMock;
        private Mock<IEFRepository<Seat>> _seatEFMock;
        private Mock<IEFRepository<Area>> _areaEFMock;
        private IValidator<AreaDto> _validator;

        [SetUp]
        public void Setup()
        {
            _seatMock = new Mock<IRepository<Seat>>();
            _areaMock = new Mock<IRepository<Area>>();
            _seatEFMock = new Mock<IEFRepository<Seat>>();
            _areaEFMock = new Mock<IEFRepository<Area>>();
            _validator = new AreaValidation();
        }

        [Test]
        public async Task ReturnAreaFromFirstLayout_WhenReturnAreaFromFirstLayout_ShouldReturnAllAreaFromFirstLayout()
        {
            // Arrange
            var layoutId = 1;
            var areas = new List<Area>
                    {
                        new Area { CoordX = 1, CoordY = 2, Description = "First area description", LayoutId= 1 },
                        new Area { CoordX = 4, CoordY = 1, Description = "Second area description", LayoutId= 1 },
                    };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            var result = await areaService.GetAsync(layoutId);

            // Assert
            result.Should().BeEquivalentTo(areas);
        }

        [Test]
        public async Task ReturnAreaById_WhenReturnAreaWithFirstId_ShouldReturnAreaWithFirstId()
        {
            // Arrange
            var areaId = 1;
            var areas = new List<Area>
            {
                new Area { Id = 2, CoordX = 1, CoordY = 2, Description = "First area description", LayoutId = 1 },
                new Area { Id = 1, CoordX = 4, CoordY = 1, Description = "Second area description", LayoutId = 1 },
            };

            _areaMock.Setup(area => area.GetByIdAsync(areaId)).Returns(Task.FromResult(areas.FirstOrDefault(area => area.Id == areaId)));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            var result = await areaService.GetByIdAsync(areaId);

            // Assert
            result.Should().BeEquivalentTo(areas.FirstOrDefault(area => area.Id == areaId));
        }

        [Test]
        public void ReturnAreaById_WhenReturnAreaWithZeroId_ShouldThrowException()
        {
            // Arrange
            var areaId = 0;
            var areas = new List<Area>
            {
                new Area { Id = 2, CoordX = 1, CoordY = 2, Description = "First area description", LayoutId = 1 },
                new Area { Id = 1, CoordX = 4, CoordY = 1, Description = "Second area description", LayoutId = 1 },
            };

            _areaMock.Setup(area => area.GetByIdAsync(areaId)).Returns(Task.FromResult(areas.FirstOrDefault(area => area.Id == areaId)));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.GetByIdAsync(areaId);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public async Task AddNewArea_WhenAddNewArea_AddedNewArea()
        {
            // Arrange
            var areaToAdd = new Area { CoordX = 1, CoordY = 1, Description = "Description for area", LayoutId = 1 };
            var areas = new List<Area> { areaToAdd };
            var dtoAreaToAdd = new AreaDto { CoordX = 1, CoordY = 1, Description = "Description for area", LayoutId = 1 };
            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(new List<Area>().AsQueryable()));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            await areaService.AddAsync(dtoAreaToAdd);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                areaToAdd,
            });
        }

        [Test]
        public void AddNewArea_WhenAddNewArea_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area> { new Area { CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 } };
            var areaToAdd = new Area { CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.AddAsync(areaToAdd)).Callback<Area>(area => areas.Add(area));

            var dtoAreaToAdd = new AreaDto { CoordX = areaToAdd.CoordX, CoordY = areaToAdd.CoordY, Description = areaToAdd.Description, LayoutId = areaToAdd.LayoutId };
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.AddAsync(dtoAreaToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't add a new area. This area description alredy exist in this layout"));
        }

        [Test]
        public void EditArea_WhenValidDesciptionAndId_ShouldEditedArea()
        {
            // Arrange
            var area = new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 };
            var areas = new List<Area> { area };
            var areaToEdit = new Area { Id = 1, CoordX = 1, CoordY = 2, Description = "First area description", LayoutId = 1 };
            var dtoAreaToEdit = new AreaDto { Id = areaToEdit.Id, CoordX = areaToEdit.CoordX, CoordY = areaToEdit.CoordY, Description = areaToEdit.Description, LayoutId = areaToEdit.LayoutId };
            area.CoordY = areaToEdit.CoordY;

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            var result = areaService.EditAsync(dtoAreaToEdit).IsCompleted;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task EditArea_WhenValidDescription_ShouldEditedArea()
        {
            // Arrange
            var area = new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 };
            var areas = new List<Area> { area };
            var areaToEdit = new Area { Id = 1, CoordX = 1, CoordY = 2, Description = "First area description2", LayoutId = 1 };
            area.CoordY = areaToEdit.CoordY;
            area.Description = areaToEdit.Description;

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit));

            var dtoAreaToEdit = new AreaDto { Id = areaToEdit.Id, CoordX = areaToEdit.CoordX, CoordY = areaToEdit.CoordY, Description = areaToEdit.Description, LayoutId = areaToEdit.LayoutId };
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            await areaService.EditAsync(dtoAreaToEdit);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                areaToEdit,
            });
        }

        [Test]
        public void EditArea_WhenNotValidArea_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
            };
            var areaToEdit = new Area { Id = 2, CoordX = 1, CoordY = 2, Description = "First area description", LayoutId = 1 };
            var dtoAreaToEdit = new AreaDto { Id = areaToEdit.Id, CoordX = areaToEdit.CoordX, CoordY = areaToEdit.CoordY, Description = areaToEdit.Description, LayoutId = areaToEdit.LayoutId };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit)).Callback<Area>(area => areas.Add(area));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.EditAsync(dtoAreaToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("You can't edit this area. This area description alredy exist in this layout"));
        }

        [Test]
        public async Task DeleteArea_WhenDeleteArea_ShouldDeleteAreaAndSeats()
        {
            // Arrange
            var areaId = 1;
            var areas = new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
            };

            _areaMock.Setup(area => area.GetByIdAsync(areaId)).Returns(Task.FromResult(areas.FirstOrDefault(x=>x.Id==areaId)));
            _areaMock.Setup(area => area.DeleteAsync(areaId)).Callback<int>(id => areas.Remove(areas.FirstOrDefault(idForDelete => idForDelete.Id == id)));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            await areaService.DeleteAsync(areaId);

            // Assert
            areas.Should().BeEquivalentTo(new List<Area>
            {
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
            });
        }

        [Test]
        public void AreaEdit_WhenUseValidatorAndLayoutIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
            };
            var areaToEdit = new Area { Id = 2, CoordX = 1, CoordY = 2, Description = "First area description", LayoutId = -1 };
            var dtoAreaToEdit = new AreaDto { Id = areaToEdit.Id, CoordX = areaToEdit.CoordX, CoordY = areaToEdit.CoordY, Description = areaToEdit.Description, LayoutId = areaToEdit.LayoutId };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit)).Callback<Area>(area => areas.Add(area));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.EditAsync(dtoAreaToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void AreaAdd_WhenUseValidatorAndCoordinatesLessThenNull_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area> { new Area { CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 } };
            var areaToAdd = new Area { CoordX = -1, CoordY = 1, Description = "First area description", LayoutId = 1 };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.AddAsync(areaToAdd)).Callback<Area>(area => areas.Add(area));
            var dtoAreaToAdd = new AreaDto { CoordX = areaToAdd.CoordX, CoordY = areaToAdd.CoordY, Description = areaToAdd.Description, LayoutId = areaToAdd.LayoutId };
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act, Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await areaService.AddAsync(dtoAreaToAdd));
            Assert.That(ex.Message, Is.EqualTo("Coordinates must be more than zero"));
        }

        [Test]
        public void AreaAdd_WhenUseValidatorAndDescriptionisNull_ShouldThrowException()
        {
            // Arrange
            string stringDescription =
                @"area dgvyhtf zygh cux iuchxknvkjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjjj jcnnnnnnnnnnnnnnkjlxsj xncdddddddddd
                ddddddddddddddddddddd dhxicuhxohxuiuyx uihsn jhzbjbxz xzhjxnbzyu xgx xgxxulujnjk x huixlnkjkvskj,kxjdvn hg isg vdhjkxfjh v";

            var areas = new List<Area> { new Area { CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 } };
            var areaToAdd = new Area { CoordX = 1, CoordY = 1, Description = stringDescription, LayoutId = 1 };
            var dtoAreaToAdd = new AreaDto { CoordX = areaToAdd.CoordX, CoordY = areaToAdd.CoordY, Description = areaToAdd.Description, LayoutId = areaToAdd.LayoutId };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.AddAsync(areaToAdd)).Callback<Area>(area => areas.Add(area));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.AddAsync(dtoAreaToAdd);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of area must be less than 200 and must be not null"));
        }

        [Test]
        public void AreaEdit_WhenUseValidatorAndDescriptionMoreThenNeed_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area>
            {
                new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 },
                new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
            };
            var areaToEdit = new Area { Id = 2, CoordX = 1, CoordY = 2, Description = null, LayoutId = 1 };

            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit)).Callback<Area>(area => areas.Add(area));
            var dtoAreaToEdit = new AreaDto { Id = areaToEdit.Id, CoordX = areaToEdit.CoordX, CoordY = areaToEdit.CoordY, Description = areaToEdit.Description, LayoutId = areaToEdit.LayoutId };
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.EditAsync(dtoAreaToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Description of area must be less than 200 and must be not null"));
        }

        [Test]
        public void AreaDelete_WhenUseValidatorAndIdLessThenOne_ShouldThrowException()
        {
            // Arrange
            Area area = new Area { Id = 0, CoordX = 1, CoordY = 1, Description = "Area description", LayoutId = 2 };
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.DeleteAsync(area.Id);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }

        [Test]
        public void AreaEdit_WhenUseValidatorAndAreaWasNull_ShouldThrowException()
        {
            // Arrange
            var areas = new List<Area>
                {
                    new Area { Id = 1, CoordX = 1, CoordY = 1, Description = "First area description", LayoutId = 1 },
                    new Area { Id = 2, CoordX = 1, CoordY = 1, Description = "First area description1", LayoutId = 1 },
                };

            Area areaToEdit = new Area { Id = 0 };
            AreaDto dtoAreaToEdit = new AreaDto { Id = 0 };
            _areaEFMock.Setup(area => area.GetAsync(It.IsAny<Func<Area, bool>>())).Returns(Task.FromResult(areas.AsQueryable()));
            _areaMock.Setup(area => area.EditAsync(areaToEdit)).Callback<Area>(area => areas.Add(area));
            var areaService = new AreaService(_areaMock.Object, _areaEFMock.Object, _seatMock.Object, _seatEFMock.Object, _validator);

            // Act
            AsyncTestDelegate testAction = async () => await areaService.EditAsync(dtoAreaToEdit);

            // Assert
            var ex = Assert.ThrowsAsync<ValidationException>(testAction);
            Assert.That(ex.Message, Is.EqualTo("Id must be more than zero"));
        }
    }
}

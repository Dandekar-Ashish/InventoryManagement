using InventoryManagement.Controllers;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagement.WebApi.Tests.Controller
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryService> _inventoryServiceMock;
        private readonly InventoryController _controller;

        public InventoryControllerTests()
        {
            _inventoryServiceMock = new Mock<IInventoryService>();
            _controller = new InventoryController(_inventoryServiceMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfInventoryDtos()
        {
            // Arrange
            var inventoryDtos = new List<InventoryDto>
            {
                new InventoryDto {Id = 1,Title="London", Description="Quisque ut eleifend turpis",RemainingCount=3 , ExpirationDate =DateTime.Now},
                new InventoryDto {Id = 1,Title="France", Description="Ut at euismod massa",RemainingCount=2 , ExpirationDate =DateTime.Now},
            };

            _inventoryServiceMock.Setup(service => service.GetAllInventory())
                .ReturnsAsync(inventoryDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<InventoryDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_WithInventoryId_ReturnsOkResult_WithInventoryDto()
        {
            // Arrange
            var inventoryId = 1;
            var inventoryDto = new InventoryDto { Id = 1, Title = "London", Description = "Quisque ut eleifend turpis", RemainingCount = 3, ExpirationDate = DateTime.Now };

            _inventoryServiceMock.Setup(service => service.GetInventoryById(inventoryId))
                .ReturnsAsync(inventoryDto);

            // Act
            var result = await _controller.Get(inventoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<InventoryDto>(okResult.Value);
            Assert.Equal(inventoryId, returnValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedInventoryDto()
        {
            // Arrange
            var inventoryDto = new InventoryDto { Id = 1, Title = "London", Description = "Quisque ut eleifend turpis", RemainingCount = 3, ExpirationDate = DateTime.Now };

            _inventoryServiceMock.Setup(service => service.AddInventory(It.IsAny<InventoryDto>()))
                .ReturnsAsync(inventoryDto);

            // Act
            var result = await _controller.Create(inventoryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<InventoryDto>(okResult.Value);
            Assert.Equal("London", returnValue.Title);
            Assert.Equal(3, returnValue.RemainingCount);
        }
    }
}
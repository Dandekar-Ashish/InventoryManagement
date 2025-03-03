using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Service.Services;
using Moq;

namespace InventoryManagement.Service.Tests
{
    public class InventoryServiceTests
    {
        private readonly Mock<IGenericRepository<Inventory>> _mockInventoryRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly InventoryService _inventoryService;

        public InventoryServiceTests()
        {
            _mockInventoryRepository = new Mock<IGenericRepository<Inventory>>();
            _mockMapper = new Mock<IMapper>();
            _inventoryService = new InventoryService(_mockInventoryRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddInventory_ShouldReturnInventoryDto_WhenValidInventoryDto()
        {
            // Arrange
            var inventoryDto = new InventoryDto { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now };
            var inventory = new Inventory { Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now };
            var inventoryOutput = new Inventory { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now };

            _mockMapper.Setup(m => m.Map<Inventory>(inventoryDto)).Returns(inventory);
            _mockMapper.Setup(m => m.Map<InventoryDto>(It.IsAny<Inventory>())).Returns(inventoryDto);
            _mockInventoryRepository.Setup(r => r.AddAsync(inventory)).ReturnsAsync(inventoryOutput);

            // Act
            var result = await _inventoryService.AddInventory(inventoryDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _mockInventoryRepository.Verify(r => r.AddAsync(It.IsAny<Inventory>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Inventory>(It.IsAny<InventoryDto>()), Times.Once);
        }

        [Fact]
        public async Task AddInventories_ShouldReturnInventoryDtos_WhenValidInventoryDtos()
        {
            // Arrange
            var inventoryDtos = new List<InventoryDto>
            {
                new InventoryDto { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now },
                new InventoryDto { Id = 2, Title = "Item2", Description = "Item Description", RemainingCount = 50, ExpirationDate = System.DateTime.Now }
            };
            var inventories = new List<Inventory>
            {
                new Inventory { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now },
                new Inventory { Id = 2, Title = "Item2", Description = "Item Description", RemainingCount = 50, ExpirationDate = System.DateTime.Now }
            };

            _mockMapper.Setup(m => m.Map<IEnumerable<Inventory>>(inventoryDtos)).Returns(inventories);
            _mockMapper.Setup(m => m.Map<IEnumerable<InventoryDto>>(inventories)).Returns(inventoryDtos);
            _mockInventoryRepository.Setup(r => r.AddRangeAsync(inventories)).ReturnsAsync(inventories);

            // Act
            var result = await _inventoryService.AddInventories(inventoryDtos);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockInventoryRepository.Verify(r => r.AddRangeAsync(It.IsAny<IEnumerable<Inventory>>()), Times.Once);
            _mockMapper.Verify(m => m.Map<IEnumerable<Inventory>>(It.IsAny<IEnumerable<InventoryDto>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllInventory_ShouldReturnInventoryDtos_WhenInventoryExists()
        {
            // Arrange
            var inventories = new List<Inventory>
            {
                new Inventory { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now },
                new Inventory { Id = 2, Title = "Item2", Description = "Item Description", RemainingCount = 50, ExpirationDate = System.DateTime.Now }
            };
            var inventoryDtos = new List<InventoryDto>
            {
                new InventoryDto { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now },
                new InventoryDto { Id = 2, Title = "Item2", Description = "Item Description", RemainingCount = 50, ExpirationDate = System.DateTime.Now }
            };

            _mockInventoryRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(inventories);
            _mockMapper.Setup(m => m.Map<IEnumerable<InventoryDto>>(inventories)).Returns(inventoryDtos);

            // Act
            var result = await _inventoryService.GetAllInventory();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _mockInventoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetInventoryById_ShouldReturnInventoryDto_WhenInventoryExists()
        {
            // Arrange
            var inventory = new Inventory { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now };
            var inventoryDto = new InventoryDto { Id = 1, Title = "Item1", Description = "Item Description", RemainingCount = 100, ExpirationDate = System.DateTime.Now };

            _mockInventoryRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(inventory);
            _mockMapper.Setup(m => m.Map<InventoryDto>(inventory)).Returns(inventoryDto);

            // Act
            var result = await _inventoryService.GetInventoryById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inventoryDto.Id, result.Id);
            _mockInventoryRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateInventory_ShouldReturnUpdatedInventoryDto_WhenValidInventoryDto()
        {
            // Arrange
            var inventoryDto = new InventoryDto { Id = 1, Title = "Updated Item", Description = "Updated Description", RemainingCount = 90, ExpirationDate = System.DateTime.Now };
            var inventory = new Inventory { Id = 1, Title = "Updated Item", Description = "Updated Description", RemainingCount = 90, ExpirationDate = System.DateTime.Now };

            _mockMapper.Setup(m => m.Map<Inventory>(inventoryDto)).Returns(inventory);
            _mockMapper.Setup(m => m.Map<InventoryDto>(inventory)).Returns(inventoryDto);
            _mockInventoryRepository.Setup(r => r.UpdateAsync(inventory)).ReturnsAsync(inventory);

            // Act
            var result = await _inventoryService.UpdateInventory(inventoryDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inventoryDto.Id, result.Id);
            _mockInventoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Inventory>()), Times.Once);
            _mockMapper.Verify(m => m.Map<Inventory>(It.IsAny<InventoryDto>()), Times.Once);
        }
    }
}

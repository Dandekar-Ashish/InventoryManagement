using InventoryManagement.Controllers;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagement.WebApi.Tests.Controller
{
    public class BookControllerTests
    {
        private readonly Mock<IBookingService> _mockBookingService;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            _mockBookingService = new Mock<IBookingService>();
            _controller = new BookController(_mockBookingService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfBookings()
        {
            // Arrange
            var bookingDtos = new List<BookingDto>
            {
                new BookingDto { Id=1,MemberId=1,InventoryId=1,Status="Active" },
                new BookingDto { Id=1,MemberId=1,InventoryId=1,Status="Active" },
            };

            _mockBookingService.Setup(service => service.GetAllBookings())
                .ReturnsAsync(bookingDtos);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<BookingDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_WithBookingId_ReturnsOkResult_WithBookingDto()
        {
            // Arrange
            var bookingId = 1;
            var bookingDto = new BookingDto { Id = 1, MemberId = 1, InventoryId = 1, Status = "Active" };

            _mockBookingService.Setup(service => service.GetBookingById(bookingId))
                .ReturnsAsync(bookingDto);

            // Act
            var result = await _controller.Get(bookingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookingDto>(okResult.Value);
            Assert.Equal(bookingId, returnValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedBookingDto()
        {
            // Arrange
            var bookingRequest = new BookingRequest { MemberId = 1, InventoryId = 2 };
            var bookingDto = new BookingDto { Id = 1, MemberId = 2, InventoryId = 1, Status = "Active" };

            _mockBookingService.Setup(service => service.AddBooking(bookingRequest))
                .ReturnsAsync((true, bookingDto, ""));

            // Act
            var result = await _controller.Create(bookingRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookingDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Active", returnValue.Status);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenBookingCreationFails()
        {
            // Arrange
            var bookingRequest = new BookingRequest { MemberId = 1, InventoryId = 2 };

            _mockBookingService.Setup(service => service.AddBooking(bookingRequest))
                .ReturnsAsync((false, null, "Member with Id 1 not found."));

            // Act
            var result = await _controller.Create(bookingRequest);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Member with Id 1 not found.", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task Cancel_ReturnsOkResult_WithCancelledBookingDto()
        {
            // Arrange
            var bookingId = 1;
            var bookingDto = new BookingDto { Id = 1, MemberId = 2, InventoryId = 1, Status = "Cancelled" };

            _mockBookingService.Setup(service => service.CancelBooking(bookingId))
                .ReturnsAsync((true, bookingDto, ""));

            // Act
            var result = await _controller.Cancel(bookingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<BookingDto>(okResult.Value);
            Assert.Equal("Cancelled", returnValue.Status);
        }
    }
}

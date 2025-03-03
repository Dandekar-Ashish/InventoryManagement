using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Repository.Models.Enums;
using InventoryManagement.Service.Interface;
using InventoryManagement.Service.Services;
using Moq;

namespace InventoryManagement.Service.Tests
{
    public class BookingServiceTests
    {
        private readonly Mock<IGenericRepository<Booking>> _mockBookingRepository;
        private readonly Mock<IMemberService> _mockMemberService;
        private readonly Mock<IInventoryService> _mockInventoryService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BookingService _bookingService;

        public BookingServiceTests()
        {
            _mockBookingRepository = new Mock<IGenericRepository<Booking>>();
            _mockMemberService = new Mock<IMemberService>();
            _mockInventoryService = new Mock<IInventoryService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();

            _bookingService = new BookingService(
                _mockBookingRepository.Object,
                _mockMemberService.Object,
                _mockInventoryService.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object
            );
        }

        [Fact]
        public async Task AddBooking_ShouldReturnSuccess_WhenValidBooking()
        {
            // Arrange
            var bookingRequest = new BookingRequest { MemberId = 1, InventoryId = 2 };
            var memberDto = new MemberDto { Id = 1, BookingCount = 0 };
            var inventoryDto = new InventoryDto { Id = 2, RemainingCount = 5 };
            var booking = new Booking { Id = 1, MemberId = 1, InventoryId = 2, Status = BookingStatus.Active };
            var bookingDto = new BookingDto { Id = 1, MemberId = 1, InventoryId = 2, Status = BookingStatus.Active.ToString() };

            _mockMemberService.Setup(m => m.GetMemberById(It.IsAny<int>())).ReturnsAsync(memberDto);
            _mockInventoryService.Setup(i => i.GetInventoryById(It.IsAny<int>())).ReturnsAsync(inventoryDto);
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.MemberRepository.UpdateAsync(It.IsAny<Member>())).ReturnsAsync(new Member());
            _mockUnitOfWork.Setup(u => u.InventoryRepository.UpdateAsync(It.IsAny<Inventory>())).ReturnsAsync(new Inventory());
            _mockUnitOfWork.Setup(u => u.BookingRepository.AddAsync(It.IsAny<Booking>())).ReturnsAsync(booking);
            _mockMapper.Setup(m => m.Map<BookingDto>(It.IsAny<Booking>())).Returns(bookingDto);

            // Act
            var result = await _bookingService.AddBooking(bookingRequest);

            // Assert
            Assert.True(result.isSuccess);
            Assert.NotNull(result.bookingDto);
            Assert.Equal(bookingDto.Id, result.bookingDto?.Id);
        }

        [Fact]
        public async Task AddBooking_ShouldReturnFailure_WhenInvalidBooking()
        {
            // Arrange
            var bookingRequest = new BookingRequest { MemberId = 1, InventoryId = 2 };
            var memberDto = new MemberDto { Id = 1, BookingCount = 0 };
            var inventoryDto = new InventoryDto { Id = 2, RemainingCount = 0 };
            var bookingDto = new BookingDto { Id = 1, MemberId = 1, InventoryId = 2, Status = BookingStatus.Active.ToString() };

            _mockMemberService.Setup(m => m.GetMemberById(It.IsAny<int>())).ReturnsAsync(memberDto);
            _mockInventoryService.Setup(i => i.GetInventoryById(It.IsAny<int>())).ReturnsAsync(inventoryDto);

            // Act
            var result = await _bookingService.AddBooking(bookingRequest);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Null(result.bookingDto);
            Assert.Equal("Inventory 2 not found.", result.errorMessage);
        }

        [Fact]
        public async Task CancelBooking_ShouldReturnSuccess_WhenValidBookingId()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { Id = bookingId, Status = BookingStatus.Active };
            var bookingUpdated = new Booking { Id = bookingId, Status = BookingStatus.Cancelled };
            var bookingDto = new BookingDto { Id = bookingId, Status = BookingStatus.Cancelled.ToString() };

            _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync(booking);
            _mockBookingRepository.Setup(r => r.UpdateAsync(It.IsAny<Booking>())).ReturnsAsync(bookingUpdated);
            _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, o => o.Member, o => o.Inventory)).ReturnsAsync(booking);
            _mockMapper.Setup(m => m.Map<BookingDto>(It.IsAny<Booking>())).Returns(bookingDto);

            // Act
            var result = await _bookingService.CancelBooking(bookingId);

            // Assert
            Assert.True(result.isSuccess);
            Assert.NotNull(result.bookingDto);
            Assert.Equal(BookingStatus.Cancelled.ToString(), result.bookingDto?.Status);
        }

        [Fact]
        public async Task CancelBooking_ShouldReturnFailure_WhenAlreadyCancelled()
        {
            // Arrange
            var bookingId = 1;
            var booking = new Booking { Id = bookingId, Status = BookingStatus.Cancelled };

            _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId)).ReturnsAsync(booking);

            // Act
            var result = await _bookingService.CancelBooking(bookingId);

            // Assert
            Assert.False(result.isSuccess);
            Assert.Null(result.bookingDto);
            Assert.Equal("No active booking. Booking already cancelled.", result.errorMessage);
        }
    }
}

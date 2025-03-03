using InventoryManagement.Controllers;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InventoryManagement.WebApi.Tests.Controller
{
    public class MemberControllerTests
    {
        private readonly Mock<IMemberService> _mockMemberService;
        private readonly MemberController _controller;

        public MemberControllerTests()
        {
            _mockMemberService = new Mock<IMemberService>();
            _controller = new MemberController(_mockMemberService.Object);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithListOfMembers()
        {
            // Arrange
            var members = new List<MemberDto>
            {
                new MemberDto { Id = 1, Name = "John", Surname="Doe",BookingCount = 1, DateJoined = DateTime.Now },
                new MemberDto { Id = 2, Name = "Emily", Surname="Johnson",BookingCount = 2, DateJoined = DateTime.Now  }
            };

            _mockMemberService.Setup(service => service.GetAllMember())
                .ReturnsAsync(members);

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<MemberDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Get_WithBookingId_ReturnsOkResult_WithMemberDto()
        {
            // Arrange
            var bookingId = 1;
            var member = new MemberDto { Id = 1, Name = "Emily", Surname = "Johnson", BookingCount = 2, DateJoined = DateTime.Now };

            _mockMemberService.Setup(service => service.GetMemberById(bookingId))
                .ReturnsAsync(member);

            // Act
            var result = await _controller.Get(bookingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MemberDto>(okResult.Value);
            Assert.Equal(bookingId, returnValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsOkResult_WithCreatedMemberDto()
        {
            // Arrange
            var memberDto = new MemberDto { Id = 1, Name = "Emily", Surname = "Johnson", BookingCount = 2, DateJoined = DateTime.Now };

            _mockMemberService.Setup(service => service.AddMember(It.IsAny<MemberDto>()))
                .ReturnsAsync(memberDto);

            // Act
            var result = await _controller.Create(memberDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<MemberDto>(okResult.Value);
            Assert.Equal("Emily", returnValue.Name);
            Assert.Equal("Johnson", returnValue.Surname);
        }

        [Fact]
        public async Task UploadMembers_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            IFormFile file = null;

            // Act
            var result = await _controller.UploadMembers(file);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No file uploaded.", badRequestResult.Value);
        }

        [Fact]
        public async Task UploadMembers_ReturnsBadRequest_WhenCSVHeadersAreIncorrect()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var csvContent = "BookingId,Name\n1,John Doe\n"; // Example of incorrect headers

            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write(csvContent);
            writer.Flush();
            fileStream.Position = 0;
            fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);
            fileMock.Setup(f => f.Length).Returns(fileStream.Length);

            // Act
            var result = await _controller.UploadMembers(fileMock.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("CSV headers are incorrect", badRequestResult.Value.ToString());
        }

        [Fact]
        public async Task UploadMembers_ReturnsOkResult_WhenMembersAreSuccessfullyUploaded()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var csvContent = "name,surname,booking_count,date_joined\nSophie,Davis,1,2024-01-02T12:10:11\n";

            var fileStream = new MemoryStream();
            var writer = new StreamWriter(fileStream);
            writer.Write(csvContent);
            writer.Flush();
            fileStream.Position = 0;
            fileMock.Setup(f => f.OpenReadStream()).Returns(fileStream);
            fileMock.Setup(f => f.Length).Returns(fileStream.Length);

            var memberDto = new MemberDto { Id = 1, Name = "Emily", Surname = "Johnson", BookingCount = 2, DateJoined = DateTime.Now };

            _mockMemberService.Setup(service => service.AddMember(It.IsAny<MemberDto>()))
                .ReturnsAsync(memberDto);

            // Act
            var result = await _controller.UploadMembers(fileMock.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Contains("1 Members successfully uploaded", okResult.Value.ToString());
        }
    }
}

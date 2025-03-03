using AutoMapper;
using InventoryManagement.DataTransferModel;
using InventoryManagement.Repository.Interface;
using InventoryManagement.Repository.Models;
using InventoryManagement.Service.Services;
using Moq;

namespace InventoryManagement.Service.Tests
{
    public class MemberServiceTests
    {
        private readonly Mock<IGenericRepository<Member>> _mockMemberRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly MemberService _memberService;

        public MemberServiceTests()
        {
            _mockMemberRepository = new Mock<IGenericRepository<Member>>();
            _mockMapper = new Mock<IMapper>();
            _memberService = new MemberService(_mockMemberRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddMember_ShouldReturnMemberDto_WhenValidMemberDtoIsPassed()
        {
            // Arrange
            var memberDto = new MemberDto
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 5,
                DateJoined = new DateTime(2022, 1, 1)
            };

            var member = new Member
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 5,
                DateJoined = new DateTime(2022, 1, 1)
            };

            _mockMapper.Setup(m => m.Map<Member>(It.IsAny<MemberDto>())).Returns(member);
            _mockMemberRepository.Setup(m => m.AddAsync(It.IsAny<Member>())).ReturnsAsync(member);
            _mockMapper.Setup(m => m.Map<MemberDto>(It.IsAny<Member>())).Returns(memberDto);

            // Act
            var result = await _memberService.AddMember(memberDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberDto.Id, result.Id);
            Assert.Equal(memberDto.Name, result.Name);
            Assert.Equal(memberDto.Surname, result.Surname);
            Assert.Equal(memberDto.BookingCount, result.BookingCount);
            Assert.Equal(memberDto.DateJoined, result.DateJoined);
        }

        [Fact]
        public async Task AddMembers_ShouldReturnMemberDtos_WhenValidMemberDtosArePassed()
        {
            // Arrange
            var memberDtos = new List<MemberDto>
            {
                new MemberDto
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    BookingCount = 5,
                    DateJoined = new DateTime(2022, 1, 1)
                }
            };

            var members = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    BookingCount = 5,
                    DateJoined = new DateTime(2022, 1, 1)
                }
            };

            _mockMapper.Setup(m => m.Map<IEnumerable<Member>>(It.IsAny<IEnumerable<MemberDto>>())).Returns(members);
            _mockMemberRepository.Setup(m => m.AddRangeAsync(It.IsAny<IEnumerable<Member>>())).ReturnsAsync(members);
            _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<Member>>())).Returns(memberDtos);

            // Act
            var result = await _memberService.AddMembers(memberDtos);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(memberDtos[0].Id, result.First().Id);
            Assert.Equal(memberDtos[0].Name, result.First().Name);
            Assert.Equal(memberDtos[0].Surname, result.First().Surname);
            Assert.Equal(memberDtos[0].BookingCount, result.First().BookingCount);
            Assert.Equal(memberDtos[0].DateJoined, result.First().DateJoined);
        }

        [Fact]
        public async Task GetAllMember_ShouldReturnMemberDtos_WhenMembersExist()
        {
            // Arrange
            var members = new List<Member>
            {
                new Member
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    BookingCount = 5,
                    DateJoined = new DateTime(2022, 1, 1)
                }
            };

            var memberDtos = new List<MemberDto>
            {
                new MemberDto
                {
                    Id = 1,
                    Name = "John",
                    Surname = "Doe",
                    BookingCount = 5,
                    DateJoined = new DateTime(2022, 1, 1)
                }
            };

            _mockMemberRepository.Setup(m => m.GetAllAsync()).ReturnsAsync(members);
            _mockMapper.Setup(m => m.Map<IEnumerable<MemberDto>>(It.IsAny<IEnumerable<Member>>())).Returns(memberDtos);

            // Act
            var result = await _memberService.GetAllMember();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(memberDtos[0].Id, result.First().Id);
            Assert.Equal(memberDtos[0].Name, result.First().Name);
            Assert.Equal(memberDtos[0].Surname, result.First().Surname);
            Assert.Equal(memberDtos[0].BookingCount, result.First().BookingCount);
            Assert.Equal(memberDtos[0].DateJoined, result.First().DateJoined);
        }

        [Fact]
        public async Task GetMemberById_ShouldReturnMemberDto_WhenValidIdIsPassed()
        {
            // Arrange
            var member = new Member
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 5,
                DateJoined = new DateTime(2022, 1, 1)
            };

            var memberDto = new MemberDto
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 5,
                DateJoined = new DateTime(2022, 1, 1)
            };

            _mockMemberRepository.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(member);
            _mockMapper.Setup(m => m.Map<MemberDto>(It.IsAny<Member>())).Returns(memberDto);

            // Act
            var result = await _memberService.GetMemberById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(memberDto.Id, result.Id);
            Assert.Equal(memberDto.Name, result.Name);
            Assert.Equal(memberDto.Surname, result.Surname);
            Assert.Equal(memberDto.BookingCount, result.BookingCount);
            Assert.Equal(memberDto.DateJoined, result.DateJoined);
        }

        [Fact]
        public async Task UpdateMember_ShouldReturnUpdatedMemberDto_WhenValidMemberDtoIsPassed()
        {
            // Arrange
            var memberDto = new MemberDto
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 5,
                DateJoined = new DateTime(2022, 1, 1)
            };

            var updatedMember = new Member
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 6,  
                DateJoined = new DateTime(2022, 1, 1)
            };

            var updatedMemberDto = new MemberDto
            {
                Id = 1,
                Name = "John",
                Surname = "Doe",
                BookingCount = 6,
                DateJoined = new DateTime(2022, 1, 1)
            };

            _mockMapper.Setup(m => m.Map<Member>(It.IsAny<MemberDto>())).Returns(updatedMember);
            _mockMemberRepository.Setup(m => m.UpdateAsync(It.IsAny<Member>())).ReturnsAsync(updatedMember);
            _mockMapper.Setup(m => m.Map<MemberDto>(It.IsAny<Member>())).Returns(updatedMemberDto);

            // Act
            var result = await _memberService.UpdateMember(memberDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedMemberDto.Id, result.Id);
            Assert.Equal(updatedMemberDto.Name, result.Name);
            Assert.Equal(updatedMemberDto.Surname, result.Surname);
            Assert.Equal(updatedMemberDto.BookingCount, result.BookingCount);
            Assert.Equal(updatedMemberDto.DateJoined, result.DateJoined);
        }
    }
}

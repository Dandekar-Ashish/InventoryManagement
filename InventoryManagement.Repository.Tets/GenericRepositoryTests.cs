using InventoryManagement.Repository.Database;
using InventoryManagement.Repository.Models;
using InventoryManagement.Repository.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Repository.Tests
{
    public class GenericRepositoryTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly GenericRepository<Member> _repository;

        public GenericRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase") // In-memory database
                .Options;

            _dbContext = new ApplicationDbContext(options);

            _repository = new GenericRepository<Member>(_dbContext);

        }

        [Fact]
        public async Task AddAsync_Should_Add_Entity_And_Return_Entity()
        {
            // Arrange
            var Member = new Member { Id = 1, Name = "Fname", Surname = "Lname", BookingCount = 1 };

            // Act
            var result = await _repository.AddAsync(Member);

            // Assert
            Assert.Equal(Member, result); 
            var dbEntity = await _dbContext.Set<Member>().FindAsync(Member.Id);
            Assert.NotNull(dbEntity);
        }

        [Fact]
        public async Task AddRangeAsync_Should_Add_Multiple_Entities()
        {
            // Arrange
            var testEntities = new List<Member>
            {
                new Member { Id = 2, Name = "Fname", Surname = "Lname", BookingCount = 1 },
                new Member { Id = 3, Name = "Fname1", Surname = "Lname1", BookingCount = 1 }
            };

            _dbContext.Members.RemoveRange(_dbContext.Members);
            _dbContext.SaveChanges();

            // Act
            var result = await _repository.AddRangeAsync(testEntities);

            // Assert
            Assert.Equal(testEntities.Count, result.Count()); 
            var dbEntities = await _dbContext.Set<Member>().ToListAsync();
            Assert.Equal(testEntities.Count, dbEntities.Count); 
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Entities()
        {
            // Arrange
            var testEntities = new List<Member>
            {
                new Member { Id = 4, Name = "Fname", Surname = "Lname", BookingCount = 1 },
                new Member { Id = 5, Name = "Fname1", Surname = "Lname1", BookingCount = 1 }
            };
            _dbContext.Members.RemoveRange(_dbContext.Members);
            _dbContext.SaveChanges();

            await _dbContext.AddRangeAsync(testEntities);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count()); 
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Entity_If_Exists()
        {
            // Arrange
            var Member = new Member { Name = "Fname", Surname = "Lname", BookingCount = 1 };
            _dbContext.Members.RemoveRange(_dbContext.Members);
            _dbContext.SaveChanges();

            await _dbContext.AddAsync(Member);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Equal(Member.Name, result.Name);
            Assert.Equal(Member.Surname, result.Surname); 
            Assert.Equal(1, result.Id); 
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_If_Entity_Does_Not_Exist()
        {
            // Act
            var result = await _repository.GetByIdAsync(999); 

            // Assert
            Assert.Null(result); 
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Entity_And_Return_Updated_Entity()
        {
            // Arrange
            var Member = new Member { Id = 7, Name = "Fname", Surname = "Lname", BookingCount = 1 };
            await _dbContext.AddAsync(Member);
            await _dbContext.SaveChangesAsync();

            // Modify the entity
            Member.Name = "Updated Test";

            // Act
            var result = await _repository.UpdateAsync(Member);

            // Assert
            Assert.Equal("Updated Test", result.Name); 
            var dbEntity = await _dbContext.Set<Member>().FindAsync(Member.Id);
            Assert.Equal("Updated Test", dbEntity.Name); 
        }
    }
}

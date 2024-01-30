using SibersProject.DataAL.Repository.Implemintations;
using SibersProject.DataAL.SqlServer;
using SibersProject.Tests.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace SibersProject.Tests.Repositories
{
    [TestFixture]
    public class BaseAsyncRepositoryTests
    {
        private Mock<AppDbContext>? _mockDbContext;
        private Mock<DbSet<TestEntity>>? _mockDbSet;
        private BaseAsyncRepository<TestEntity>? _repository;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = new Mock<AppDbContext>();
            _mockDbSet = new Mock<DbSet<TestEntity>>();

            _mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(_mockDbSet.Object);
            _repository = new BaseAsyncRepository<TestEntity>(_mockDbContext.Object);
        }

        //TODO: End this
        [Test]
        public void Create_ShouldAddEntityAndSaveChanges()
        {
            // Arrange
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

            _mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(_mockDbSet.Object);

            // Act
            _repository.Create(entity);

            // Assert
            _mockDbSet.Verify(x => x.AddAsync(entity, default), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Test]
        public void ReadAll_ShouldReturnAllEntities()
        {
            // Arrange
            var entities = new List<TestEntity>
            {
                new TestEntity { Id = Guid.NewGuid(), Name = "EntityFirst" },
                new TestEntity { Id = Guid.NewGuid(), Name = "EntitySecond" }
            };
            var mockDbSet = new Mock<DbSet<TestEntity>>();

            mockDbSet.As<IQueryable<TestEntity>>().Setup(x => x.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(x => x.Expression).Returns(entities.AsQueryable().Expression);


            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(mockDbSet.Object);

            var repository = new BaseAsyncRepository<TestEntity>(mockDbContext.Object);
            // Act
            var result = repository.ReadAll();

            // Assert
            Assert.IsTrue(result.SequenceEqual(entities));
        }

        [Test]
        public void ReadById_ShouldReturnEntityWithMatchingId()
        {
            // Arrange
            var entityId = Guid.NewGuid();
            var entity = new TestEntity { Id = entityId, Name = "Test" };
            var entities = new List<TestEntity> { entity };
            var mockDbSet = new Mock<DbSet<TestEntity>>();

            mockDbSet.As<IQueryable<TestEntity>>().Setup(x => x.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<TestEntity>>().Setup(x => x.Expression).Returns(entities.AsQueryable().Expression);


            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(mockDbSet.Object);

            var repository = new BaseAsyncRepository<TestEntity>(mockDbContext.Object);
            // Act
            var result = repository.ReadById(entityId);

            // Assert
            Assert.AreEqual(entity, result);
        }


        //TODO: End this
        [Test]
        public void UpdateAsync_ShouldUpdateEntityAndSaveChanges()
        {
            // Arrange
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

            _mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(_mockDbSet.Object);
            // Act
            _repository.UpdateAsync(entity);

            // Assert
            _mockDbSet.Verify(x => x.Update(entity), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        //TODO: End this
        [Test]
        public void DeleteAsync_ShouldDeleteEntityAndSaveChanges()
        {
            // Arrange
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test" };

            _mockDbContext.Setup(x => x.Set<TestEntity>()).Returns(_mockDbSet.Object);

            // Act
            _repository.DeleteAsync(entity);

            // Assert
            _mockDbSet.Verify(x => x.Remove(entity), Times.Once);
            _mockDbContext.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }
    }
}

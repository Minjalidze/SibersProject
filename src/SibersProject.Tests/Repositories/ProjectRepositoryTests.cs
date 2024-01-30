//using SibersProject.DataAL.Repository.Implemintations;
//using SibersProject.DataAL.Repository.Interfaces;
//using SibersProject.DataAL.SqlServer;
//using SibersProject.MainDomain.Models.Entities;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System.Linq.Expressions;

//namespace SibersProject.Tests.Repositories
//{
//    [TestFixture]
//    public class ProjectRepositoryTests
//    {
//        private Mock<AppDbContext> _mockDbContext;
//        private IProjectRepository _projectRepository;

//        [SetUp]
//        public void Setup()
//        {
//            var options = new DbContextOptionsBuilder<AppDbContext>()
//                .UseInMemoryDatabase(databaseName: "TestDatabase")
//                .Options;

//            _mockDbContext = new Mock<AppDbContext>(options);
//            _projectRepository = new ProjectRepository(_mockDbContext.Object);
//        }

//        [Test]
//        public async Task GetFilteredProjectAsync_ReturnsFilteredProjects()
//        {
//            // Arrange
//            var projects = new List<Project>
//            {
//                new Project { Id = Guid.NewGuid(), Name = "Project 1", Priority = 1 },
//                new Project { Id = Guid.NewGuid(), Name = "Project 2", Priority = 2 },
//                new Project { Id = Guid.NewGuid(), Name = "Project 3", Priority = 3 }
//            };

//            _mockDbContext.Setup(c => c.Set<Project>()).Returns(CreateMockDbSet(projects));

//            Expression<Func<Project, bool>> filter = p => p.Priority > 1;

//            // Act
//            var result = await _projectRepository.GetFilteredProjectAsync(filter);

//            // Assert
//            Assert.NotNull(result);
//            Assert.AreEqual(2, result.Count());
//            Assert.AreEqual("Project 2", result.First().Name);
//            Assert.AreEqual("Project 3", result.Last().Name);
//        }

//        [Test]
//        public async Task GetFilteredProjectByIdAsync_ReturnsProjectById()
//        {
//            // Arrange
//            var projectId = Guid.NewGuid();
//            var project = new Project { Id = projectId, Name = "Test Project", Priority = 1 };
//            var projects = new List<Project> { project };

//            _mockDbContext.Setup(c => c.Set<Project>()).Returns(CreateMockDbSet(projects));

//            // Act
//            var result = await _projectRepository.GetFilteredProjectByIdAsync(projectId);

//            // Assert
//            Assert.NotNull(result);
//            Assert.AreEqual(projectId, result.Id);
//            Assert.AreEqual("Test Project", result.Name);
//            Assert.AreEqual(1, result.Priority);
//        }

//        private DbSet<T> CreateMockDbSet<T>(List<T> data) where T : class
//        {
//            var queryableData = data.AsQueryable();
//            var mockDbSet = new Mock<DbSet<T>>();
//            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
//            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
//            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
//            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
//            return mockDbSet.Object;
//        }
//    }
//}

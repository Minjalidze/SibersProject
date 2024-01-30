using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.Services.Services.Implementations;
using SibersProject.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace SibersProject.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private Mock<IProjectRepository>? _mockProjectRepository;
        private Mock<UserManager<Employee>>? _mockUserManager;
        private IProjectService? _projectService;

        [SetUp]
        public void Setup()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockUserManager = MockUserManager<Employee>();
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockUserManager.Object);
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var userManager = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Test]
        public async Task CreateAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var createProjectDto = new CreateProjectDTO
            {
                Name = "Test Project",
                Priority = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                ClientCompanyName = "Test Client",
                ExecutiveCompanyName = "Test Executive",
                ProjectManagerId = "managerId",
                EmployeesIds = new List<string> { "employeeId1", "employeeId2" }
            };

            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = createProjectDto.Name,
                Priority = createProjectDto.Priority,
                StartDate = createProjectDto.StartDate,
                EndDate = createProjectDto.EndDate,
                ClientCompanyName = createProjectDto.ClientCompanyName,
                ExecutiveCompanyName = createProjectDto.ExecutiveCompanyName,
                ProjectManagerId = createProjectDto.ProjectManagerId,
                Employees = createProjectDto.EmployeesIds.Select(x => new ProjectEmployee { EmployeeId = x }).ToList()
            };

            _mockUserManager.Setup(m => m.FindByIdAsync(createProjectDto.ProjectManagerId))
                .ReturnsAsync(new Employee { Id = createProjectDto.ProjectManagerId });

            _mockProjectRepository.Setup(m => m.Create(It.IsAny<Project>()))
                .Callback<Project>(p => project.Id = p.Id)
                .Returns(Task.CompletedTask);


            // Act
            var response = await _projectService.CreateAsync(createProjectDto);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<Guid>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(project.Id, response.Data);
        }

        [Test]
        public async Task DeleteAsync_WithValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            _mockProjectRepository.Setup(m => m.DeleteByIdAsync(projectId))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _projectService.DeleteAsync(projectId);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<bool>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(true, response.Data);
        }

        [Test]
        public async Task GetAsync_WithValidFilter_ReturnsSuccessResponse()
        {
            // Arrange
            var projectFilterDto = new FilterProjectDTO
            {
            };

            var projects = new List<Project>
            {
                new Project { Id = Guid.NewGuid(), Name = "Project 1" },
                new Project { Id = Guid.NewGuid(), Name = "Project 2" }
            };

            _mockProjectRepository.Setup(m => m.GetFilteredProjectAsync(It.IsAny<Expression<Func<Project, bool>>>()))
                .ReturnsAsync(projects);

            // Act
            var response = await _projectService.GetAsync(projectFilterDto, null);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<IEnumerable<Project>>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(projects, response.Data);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var projectId = Guid.NewGuid();

            var project = new Project { Id = projectId, Name = "Test Project" };

            _mockProjectRepository.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync(project);

            // Act
            var response = await _projectService.GetByIdAsync(projectId);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<Project>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(project, response.Data);
        }

        [Test]
        public async Task UpdateAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var updateProjectDto = new UpdateProjectDTO
            {
                Name = "Updated Project",
                Priority = 2,
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(10),
                ClientCompanyName = "Updated Client",
                ExecutiveCompanyName = "Updated Executive"
            };

            var project = new Project
            {
                Id = projectId,
                Name = "Original Project",
                Priority = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                ClientCompanyName = "Original Client",
                ExecutiveCompanyName = "Original Executive"
            };

            _mockProjectRepository.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync(project);

            _mockProjectRepository.Setup(m => m.UpdateAsync(It.IsAny<Project>()))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _projectService.UpdateAsync(projectId, updateProjectDto);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<bool>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(true, response.Data);
            Assert.AreEqual(updateProjectDto.Name, project.Name);
            Assert.AreEqual(updateProjectDto.Priority, project.Priority);
            Assert.AreEqual(updateProjectDto.StartDate, project.StartDate);
            Assert.AreEqual(updateProjectDto.EndDate, project.EndDate);
            Assert.AreEqual(updateProjectDto.ClientCompanyName, project.ClientCompanyName);
            Assert.AreEqual(updateProjectDto.ExecutiveCompanyName, project.ExecutiveCompanyName);
        }
    }
}

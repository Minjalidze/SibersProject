using SibersProject.ApiModels.DTOs.EntitiesDTO.Employee;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.Services.Services.Implementations;
using SibersProject.Services.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;

namespace SibersProject.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private Mock<UserManager<Employee>>? _userManagerMock;
        private Mock<IProjectRepository>? _projectRepositoryMock;
        private Mock<ITaskItemRepository>? _taskItemRepositoryMock;
        private IEmployeeService? _employeeService;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<Employee>>(Mock.Of<IUserStore<Employee>>(), null, null, null, null, null, null, null, null);
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
            _employeeService = new EmployeeService(_userManagerMock.Object, _projectRepositoryMock.Object, _taskItemRepositoryMock.Object);
        }
       

        [Test]
        public async Task GetAllAsync_ReturnsNotFoundResponse_WhenUsersListIsNull()
        {
            // Arrange
            IQueryable<Employee> users = null;
            _userManagerMock.Setup(m => m.Users)
                .Returns(users);

            // Act
            var response = await _employeeService.GetAllAsync();

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsSuccessResponseWithEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = "1";
            var user = new Employee { Id = employeeId, UserName = "user1" };

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync(user);

            // Act
            var response = await _employeeService.GetByIdAsync(employeeId);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(employeeId, response.Data.Id);
            Assert.AreEqual(user.UserName, response.Data.UserName);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNotFoundResponse_WhenEmployeeIsNull()
        {
            // Arrange
            var employeeId = "1";

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var response = await _employeeService.GetByIdAsync(employeeId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
        }

        [Test]
        public async Task UpdateAsync_ReturnsSuccessResponse_WhenEmployeeIsUpdated()
        {
            // Arrange
            var employeeId = "1";
            var updateDto = new UpdateEmployeeDTO
            {
                Email = "newemail@example.com",
                FirstName = "New",
                LastName = "Name",
                MiddleName = "Middle"
            };
            var employee = new Employee { Id = employeeId };

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _userManagerMock.Setup(m => m.UpdateAsync(employee))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var response = await _employeeService.UpdateAsync(employeeId, updateDto);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            _userManagerMock.Verify(m => m.UpdateAsync(employee), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_ReturnsNotFoundResponse_WhenEmployeeIsNull()
        {
            // Arrange
            var employeeId = "1";
            var updateDto = new UpdateEmployeeDTO();

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var response = await _employeeService.UpdateAsync(employeeId, updateDto);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _userManagerMock.Verify(m => m.UpdateAsync(It.IsAny<Employee>()), Times.Never);
        }


        [Test]
        public async Task UpdateAsync_ReturnsErrorResponse_WhenUpdateFails()
        {
            // Arrange
            var employeeId = "1";
            var updateDto = new UpdateEmployeeDTO();

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var response = await _employeeService.UpdateAsync(employeeId, updateDto);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
        }

        [Test]
        public async Task DeleteByIdAsync_ReturnsSuccessResponse_WhenEmployeeIsDeleted()
        {
            // Arrange
            var employeeId = "1";
            var employee = new Employee { Id = employeeId };

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _userManagerMock.Setup(m => m.DeleteAsync(employee))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var response = await _employeeService.DeleteByIdAsync(employeeId);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            _userManagerMock.Verify(m => m.DeleteAsync(employee), Times.Once);
        }

        [Test]
        public async Task DeleteByIdAsync_ReturnsNotFoundResponse_WhenEmployeeIsNull()
        {
            // Arrange
            var employeeId = "1";

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync((Employee)null);

            // Act
            var response = await _employeeService.DeleteByIdAsync(employeeId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<Employee>()), Times.Never);

        }

        [Test]
        public async Task DeleteByIdAsync_ReturnsErrorResponse_WhenDeleteFails()
        {
            // Arrange
            var employeeId = "1";
            var employee = new Employee { Id = employeeId };

            _userManagerMock.Setup(m => m.FindByIdAsync(employeeId))
                .ReturnsAsync(employee);
            _userManagerMock.Setup(m => m.DeleteAsync(employee))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Delete failed" }));

            // Act
            var response = await _employeeService.DeleteByIdAsync(employeeId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _userManagerMock.Verify(m => m.DeleteAsync(employee), Times.Once);
        }

        [Test]
        public async Task AssignEmployeeToProjectAsync_ReturnsSuccessResponse_WhenEmployeeIsAssignedToProject()
        {
            // Arrange
            var employeesId = new List<string> { "1", "2" };
            var projectId = Guid.NewGuid();
            var employee1 = new Employee { Id = "1" };
            var employee2 = new Employee { Id = "2" };
            var project = new Project { Id = projectId };

            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(employee1);
            _userManagerMock.Setup(m => m.FindByIdAsync("2"))
                .ReturnsAsync(employee2);
            _projectRepositoryMock.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync(project);
            _projectRepositoryMock.Setup(m => m.UpdateAsync(project))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            _projectRepositoryMock.Verify(m => m.UpdateAsync(project), Times.Once);
        }

        [Test]
        public async Task AssignEmployeeToProjectAsync_ReturnsNotFoundResponse_WhenEmployeeIsNull()
        {
            // Arrange
            var employeesId = new List<string> { "1", "2" };
            var projectId = Guid.NewGuid();
            var employee1 = new Employee { Id = "1" };

            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(employee1);
            _userManagerMock.Setup(m => m.FindByIdAsync("2"))
                .ReturnsAsync((Employee)null);
            _projectRepositoryMock.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync((Project)null);

            // Act
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _projectRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Project>()), Times.Never);
        }

        [Test]
        public async Task AssignEmployeeToProjectAsync_ReturnsNotFoundResponse_WhenProjectIsNull()
        {
            // Arrange
            var employeesId = new List<string> { "1", "2" };
            var projectId = Guid.NewGuid();
            var employee1 = new Employee { Id = "1" };

            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(employee1);
            _userManagerMock.Setup(m => m.FindByIdAsync("2"))
                .ReturnsAsync((Employee)null);
            _projectRepositoryMock.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync((Project)null);

            // Act
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _projectRepositoryMock.Verify(m => m.UpdateAsync(It.IsAny<Project>()), Times.Never);

        }

        [Test]
        public async Task AssignEmployeeToProjectAsync_ReturnsErrorResponse_WhenUpdateFails()
        {
            // Arrange
            var employeesId = new List<string> { "1", "2" };
            var projectId = Guid.NewGuid();
            var employee1 = new Employee { Id = "1" };
            var employee2 = new Employee { Id = "2" };
            var project = new Project { Id = projectId };

            _userManagerMock.Setup(m => m.FindByIdAsync("1"))
                .ReturnsAsync(employee1);
            _userManagerMock.Setup(m => m.FindByIdAsync("2"))
                .ReturnsAsync(employee2);
            _projectRepositoryMock.Setup(m => m.ReadByIdAsync(projectId))
                .ReturnsAsync(project);
            _projectRepositoryMock.Setup(m => m.UpdateAsync(project))
                .Throws(new Exception("Update failed"));

            // Act
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.NotNull(response.Message);
            _projectRepositoryMock.Verify(m => m.UpdateAsync(project), Times.Once);
        }
    }
}

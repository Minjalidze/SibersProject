using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Implementations;
using SibersProject.Services.Services.Interfaces;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Linq.Expressions;

namespace SibersProject.Tests.Services
{
    [TestFixture]
    public class TaskItemServiceTests
    {
        private Mock<ITaskItemRepository>? _mockTaskItemRepository;
        private Mock<UserManager<Employee>>? _mockUserManager;
        private ITaskItemService? _taskItemService;

        [SetUp]
        public void Setup()
        {
            _mockTaskItemRepository = new Mock<ITaskItemRepository>();
            _mockUserManager = MockUserManager<Employee>();
            _taskItemService = new TaskItemService(_mockTaskItemRepository.Object, _mockUserManager.Object);
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
            var createTaskItemDto = new CreateTaskItemDTO
            {
                Name = "Test Task",
                Comment = "Test Comment",
                Status = Status.InProgress,
                Priority = 10,
                AuthorId = "authorId",
                ExecutorId = "executorId"
            };

            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Name = createTaskItemDto.Name,
                Comment = createTaskItemDto.Comment,
                Status = createTaskItemDto.Status,
                Priority = createTaskItemDto.Priority,
                AuthorId = createTaskItemDto.AuthorId,
                ExecutorId = createTaskItemDto.ExecutorId
            };

            _mockUserManager.Setup(m => m.FindByIdAsync(createTaskItemDto.AuthorId))
                .ReturnsAsync(new Employee { Id = createTaskItemDto.AuthorId });

            _mockUserManager.Setup(m => m.FindByIdAsync(createTaskItemDto.ExecutorId))
                .ReturnsAsync(new Employee { Id = createTaskItemDto.ExecutorId });

            _mockTaskItemRepository.Setup(m => m.Create(It.IsAny<TaskItem>()))
                .Callback<TaskItem>(t => taskItem.Id = t.Id)
                .Returns(Task.CompletedTask);

            // Act
            var response = await _taskItemService.CreateAsync(createTaskItemDto);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<Guid>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(taskItem.Id, response.Data);
        }

        [Test]
        public async Task DeleteAsync_WithValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockTaskItemRepository.Setup(m => m.DeleteByIdAsync(taskId))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _taskItemService.DeleteAsync(taskId);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<bool>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(true, response.Data);
        }      

        [Test]
        public async Task GetAsync_WithFilterAndSortOrder_ReturnsFilteredAndSortedTaskItems()
        {
            // Arrange
            var taskItemFilterDto = new FilterTaskItemDTO { Status = Status.Done };
            var sortOrder = Sort.PriorityDesc;

            var filteredTaskItems = new List<TaskItem>
            {
                new TaskItem { Id = Guid.NewGuid(), Name = "Task 1", Status = Status.Done, Priority = 5 },
                new TaskItem { Id = Guid.NewGuid(), Name = "Task 2", Status = Status.Done, Priority = 10 },
                new TaskItem { Id = Guid.NewGuid(), Name = "Task 3", Status = Status.Done, Priority = 0 }
            };

            _mockTaskItemRepository.Setup(m => m.GetFilteredTaskItemsAsync(It.IsAny<Expression<Func<TaskItem, bool>>>()))
                .ReturnsAsync(filteredTaskItems);

            // Act
            var response = await _taskItemService.GetAsync(taskItemFilterDto, sortOrder);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<IEnumerable<TaskItem>>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(filteredTaskItems.OrderByDescending(t => t.Priority), response.Data);
        }

        [Test]
        public async Task GetByIdAsync_WithValidId_ReturnsSuccessResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskItem = new TaskItem { Id = taskId, Name = "Test Task" };

            _mockTaskItemRepository.Setup(m => m.ReadByIdAsync(taskId))
                .ReturnsAsync(taskItem);

            // Act
            var response = await _taskItemService.GetByIdAsync(taskId);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<TaskItem>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(taskItem, response.Data);
        }

        [Test]
        public async Task UpdateAuthorAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateTaskItemDto = new UpdateTaskItemWithAuthorDTO
            {
                Name = "Updated Task",
                Comment = "Updated Comment",
                Status = Status.InProgress,
                Priority = 0,
                AuthorId = "authorId"
            };

            var taskItem = new TaskItem { Id = taskId };

            _mockTaskItemRepository.Setup(m => m.GetFilteredTaskItemByIdAsync(taskId))
                .ReturnsAsync(taskItem);

            _mockTaskItemRepository.Setup(m => m.UpdateAsync(taskItem))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _taskItemService.UpdateAuthorAsync(taskId, updateTaskItemDto);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<bool>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(true, response.Data);
            Assert.AreEqual(updateTaskItemDto.Name, taskItem.Name);
            Assert.AreEqual(updateTaskItemDto.Comment, taskItem.Comment);
            Assert.AreEqual(updateTaskItemDto.Status, taskItem.Status);
            Assert.AreEqual(updateTaskItemDto.Priority, taskItem.Priority);
            Assert.AreEqual(updateTaskItemDto.AuthorId, taskItem.AuthorId);
        }

        [Test]
        public async Task UpdateExecutorAsync_WithValidData_ReturnsSuccessResponse()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updateTaskItemDto = new UpdateTaskItemWithExecutorDTO
            {
                Name = "Updated Task",
                Comment = "Updated Comment",
                Status = Status.InProgress,
                Priority = 0,
                ExecutorId = "executorId"
            };

            var taskItem = new TaskItem { Id = taskId };

            _mockTaskItemRepository.Setup(m => m.GetFilteredTaskItemByIdAsync(taskId))
                .ReturnsAsync(taskItem);

            _mockTaskItemRepository.Setup(m => m.UpdateAsync(taskItem))
                .Returns(Task.CompletedTask);

            // Act
            var response = await _taskItemService.UpdateExecutorAsync(taskId, updateTaskItemDto);

            // Assert
            Assert.IsInstanceOf<IBaseResponse<bool>>(response);
            Assert.AreEqual(true, response.IsSuccess);
            Assert.AreEqual(true, response.Data);
            Assert.AreEqual(updateTaskItemDto.Name, taskItem.Name);
            Assert.AreEqual(updateTaskItemDto.Comment, taskItem.Comment);
            Assert.AreEqual(updateTaskItemDto.Status, taskItem.Status);
            Assert.AreEqual(updateTaskItemDto.Priority, taskItem.Priority);
            Assert.AreEqual(updateTaskItemDto.ExecutorId, taskItem.ExecutorId);
        }
    }
}


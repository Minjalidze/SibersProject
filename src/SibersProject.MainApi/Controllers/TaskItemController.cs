using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Interfaces;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SibersProject.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterTaskItemDTO taskItemFilterDto, [FromQuery] Sort? sortOrder)
        {
            var response = await _taskItemService.GetAsync(taskItemFilterDto, sortOrder);
            return Ok(response.Data);
        }
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _taskItemService.GetByIdAsync(id);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemDTO createTaskItemDto)
        {
            var response = await _taskItemService.CreateAsync(createTaskItemDto);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPut("update-executor/{taskId}")]
        public async Task<IActionResult> UpdateExecutor(Guid taskId, UpdateTaskItemWithExecutorDTO taskDto)
        {
            var response = await _taskItemService.UpdateExecutorAsync(taskId, taskDto);
            return Ok(response.Data);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPut("update-author/{taskId}")]
        public async Task<IActionResult> UpdateAuthor(Guid taskId, UpdateTaskItemWithAuthorDTO taskDto)
        {
            var response = await _taskItemService.UpdateAuthorAsync(taskId, taskDto);
            return Ok(response.Data);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _taskItemService.DeleteAsync(id);
            return Ok(response);
        }
    }
}

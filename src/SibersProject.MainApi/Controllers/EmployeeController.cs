using SibersProject.ApiModels.Auth.Models;
using SibersProject.ApiModels.DTOs.EntitiesDTO.Employee;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Interfaces;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text.Json;

namespace SibersProject.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthManager<Employee> _authService;

        public EmployeeController(IEmployeeService employeeService, IAuthManager<Employee> authManager)
        {
            _employeeService = employeeService;
            _authService = authManager;
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _employeeService.GetAllAsync();


            var employeeDtos = response.Data.Select(employee => new EmployeeDTO
            {
                Id = employee.Id,
                UserName = employee.UserName,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                MiddleName = employee.MiddleName,
                Email = employee.Email,
                UserType = employee.UserType
            });

            return Ok(employeeDtos);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _employeeService.GetByIdAsync(id);
            return Ok(response.Data);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager, Employee")]
        [HttpGet]
        [Route("get-projects/{employeeId}")]
        public async Task<IActionResult> GetProjects(string employeeId)
        {
            var response = await _employeeService.GetEmployeeProjectsAsync(employeeId);
            return Ok(response.Data);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager, Employee")]
        [HttpGet]
        [Route("get-tasks/{employeeId}")]
        public async Task<IActionResult> GetTasks(string employeeId)
        {
            var response = await _employeeService.GetEmployeeTasksAsync(employeeId);

            var taskDtos = response.Data.Select(task => new TaskItemDTO
            {
                TaskItemId = task.Id,
                Name = task.Name,
                Comment = task.Comment,
                Status = task.Status,
                Priority = task.Priority,
                AuthorId = task.AuthorId,
                ExecutorId = task.ExecutorId
            });
            var tasksJson = JsonSerializer.Serialize(taskDtos);


            return Content(tasksJson, "application/json");
        }



        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateEmployeeDTO employeeDto)
        {
            var response = await _employeeService.UpdateAsync(id, employeeDto);
            return Ok(response.Data);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpGet("checkUserRole/{userId}/{roleType}")]
        public async Task<IActionResult> CheckUserRole(string userId, Roles roleType)
        {
            var response = await _employeeService.CheckUserRole(userId, roleType);

            if (response.IsSuccess)
            {
                return Ok(response.Data);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPut]
        [Route("put-role/{userId}/{roleType}")]
        public async Task<IActionResult> PutRoleById(string userId, Roles roleType)
        {
            var response = await _employeeService.SetEmployeeNewRoleByIdAsync(userId, roleType);
            return Ok(response);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _employeeService.DeleteByIdAsync(id);
            return Ok(response);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPost]
        [Route("assign-to-project/{projectId}")]
        public async Task<IActionResult> AssignToProject(List<string> employeesId, Guid projectId)
        {
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);
            return Ok(response);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPost]
        [Route("remove-from-project/{projectId}")]
        public async Task<IActionResult> RemoveFromProject(List<string> employeesId, Guid projectId)
        {
            var response = await _employeeService.RemoveEmployeeFromProjectAsync(employeesId, projectId);
            return Ok(response);
        }


        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager")]
        [HttpPost]
        [Route("assign-executor-to-task/{taskId}/{employeeId}")]
        public async Task<IActionResult> AssignExecutorToTask(Guid taskId, string employeeId)
        {
            var response = await _employeeService.AssignExecutorToTaskAsync(taskId, employeeId);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor, Manager, Employee")]
        [HttpPut]
        [Route("change-task-status/{taskId}/{newStatus}")]
        public async Task<IActionResult> ChangeTaskStatus(Guid taskId, Status newStatus)
        {
            var response = await _employeeService.ChangeTaskStatusAsync(taskId, newStatus);
            return Ok(response);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.Login(model);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return Unauthorized(response.Message);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);
            await _employeeService.SetEmployeeNewRoleByIdAsync(result.Data, Roles.Employee);

            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var result = await _authService.RefreshToken(tokenModel);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            try
            {
                await _authService.RevokeRefreshTokenByUsernameAsync(username);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            await _authService.RevokeAllRefreshTokensAsync();
            return NoContent();
        }

    }
}

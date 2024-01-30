using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace SibersProject.MainApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterProjectDTO projectFilterDto, [FromQuery] Sort? sortOrder)
        {
            var response = await _projectService.GetAsync(projectFilterDto, sortOrder);
            return Ok(response.Data);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _projectService.GetByIdAsync(id);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDTO createProjectDto)
        {
            var response = await _projectService.CreateAsync(createProjectDto);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpPut("{projectid}")]
        public async Task<IActionResult> Update(Guid projectid, UpdateProjectDTO projectDto)
        {
            var response = await _projectService.UpdateAsync(projectid, projectDto);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Supervisor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _projectService.DeleteAsync(id);
            return Ok(response);
        }
    }
}

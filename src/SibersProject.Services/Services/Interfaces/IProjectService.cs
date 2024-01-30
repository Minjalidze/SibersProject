using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.Services.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IBaseResponse<IEnumerable<Project>>> GetAsync(FilterProjectDTO? projectFilterDto, Sort? sortOrder);
        Task<IBaseResponse<Project>> GetByIdAsync(Guid id);
        Task<IBaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectDTO project);
        Task<IBaseResponse<bool>> DeleteAsync(Guid id);
        Task<IBaseResponse<Guid>> CreateAsync(CreateProjectDTO createProjectDto);
    }
}

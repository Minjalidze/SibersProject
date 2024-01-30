using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;

namespace SibersProject.Services.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<IBaseResponse<IEnumerable<TaskItem>>> GetAsync(FilterTaskItemDTO? projectFilterDto, Sort? sortOrder);
        Task<IBaseResponse<TaskItem>> GetByIdAsync(Guid id);
        Task<IBaseResponse<bool>> UpdateExecutorAsync(Guid id, UpdateTaskItemWithExecutorDTO taskItemWithExecutorDto);
        Task<IBaseResponse<bool>> UpdateAuthorAsync(Guid id, UpdateTaskItemWithAuthorDTO taskItemWithAuthorDto);
        Task<IBaseResponse<bool>> DeleteAsync(Guid id);
        Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createProjectDto);
    }
}

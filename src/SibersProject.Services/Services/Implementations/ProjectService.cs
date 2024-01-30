using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.ApiModels.Response.Features;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Helpers;
using SibersProject.Services.Services.Interfaces;
using SibersProject.Validator;
using Microsoft.AspNetCore.Identity;

namespace SibersProject.Services.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly UserManager<Employee> _userManager;

        public ProjectService(IProjectRepository projectRepository,
            UserManager<Employee> userManager)
        {
            _projectRepository = projectRepository;
            _userManager = userManager;
        }

        public async Task<IBaseResponse<Guid>> CreateAsync(CreateProjectDTO createProjectDto)
        {
            try
            {
                ObjectValidator<CreateProjectDTO>.CheckIsNotNullObject(createProjectDto);
                var project = new Project
                {
                    Name = createProjectDto.Name,
                    Priority = createProjectDto.Priority,
                    StartDate = createProjectDto.StartDate,
                    EndDate = createProjectDto.EndDate,
                    ClientCompanyName = createProjectDto.ClientCompanyName,
                    ExecutiveCompanyName = createProjectDto.ExecutiveCompanyName,
                };

                if (await _userManager.FindByIdAsync(createProjectDto.ProjectManagerId) == null)
                    throw new ArgumentNullException($"Employee with id  {createProjectDto.ProjectManagerId} not found");

                project.ProjectManagerId = createProjectDto.ProjectManagerId;
                var employees = createProjectDto.EmployeesIds.Select(x => new ProjectEmployee { EmployeeId = x.ToString() }).ToList();
                project.Employees = employees;

                await _projectRepository.Create(project);

                return ResponseFactory<Guid>.CreateSuccessResponse(project.Id);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<Guid>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<Guid>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                await _projectRepository.DeleteByIdAsync(id);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<IEnumerable<Project>>> GetAsync(FilterProjectDTO? projectFilterDto, Sort? sortOrder)
        {
            try
            {
                ObjectValidator<FilterProjectDTO>.CheckIsNotNullObject(projectFilterDto);

                var filter = FilterHelper.CreateProjectFilter(projectFilterDto);

                var projects = await _projectRepository.GetFilteredProjectAsync(filter);

                if (sortOrder.HasValue)
                {
                    projects = sortOrder switch
                    {
                        Sort.NameDesc => projects.OrderByDescending(x => x.Name).ToList(),
                        Sort.NameAsc => projects.OrderBy(x => x.Name).ToList(),
                        Sort.PriorityDesc => projects.OrderByDescending(x => x.Priority).ToList(),
                        Sort.PriorityAsc => projects.OrderBy(x => x.Priority).ToList(),
                        _ => projects,
                    };
                }

                return ResponseFactory<IEnumerable<Project>>.CreateSuccessResponse(projects);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<Project>> GetByIdAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                var project = await _projectRepository.ReadByIdAsync(id);

                return ResponseFactory<Project>.CreateSuccessResponse(project);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<Project>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<Project>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAsync(Guid id, UpdateProjectDTO updateProjectDto)
        {
            try
            {
                ObjectValidator<UpdateProjectDTO>.CheckIsNotNullObject(updateProjectDto);

                var project = await _projectRepository.ReadByIdAsync(id);

                project.Name = updateProjectDto.Name;
                project.Priority = updateProjectDto.Priority;
                project.StartDate = updateProjectDto.StartDate;
                project.EndDate = updateProjectDto.EndDate;
                project.ClientCompanyName = updateProjectDto.ClientCompanyName;
                project.ExecutiveCompanyName = updateProjectDto.ExecutiveCompanyName;

                await _projectRepository.UpdateAsync(project);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }
    }
}

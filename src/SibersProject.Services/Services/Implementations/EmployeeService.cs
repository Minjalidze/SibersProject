using SibersProject.ApiModels.DTOs.EntitiesDTO.Employee;
using SibersProject.ApiModels.Response.Features;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;
using SibersProject.Services.Services.Interfaces;
using SibersProject.Validator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SibersProject.Services.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskItemRepository _taskItemRepository;

        public EmployeeService(UserManager<Employee> userManager,
            IProjectRepository repository,
            ITaskItemRepository taskItemRepository)
        {
            _userManager = userManager;
            _projectRepository = repository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Employee>>> GetAllAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                ObjectValidator<IEnumerable<Employee>>.CheckIsNotNullObject(users);

                return ResponseFactory<IEnumerable<Employee>>.CreateSuccessResponse(users);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<Employee>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Employee>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<Employee>> GetByIdAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var user = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(user);

                return ResponseFactory<Employee>.CreateSuccessResponse(user);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<Employee>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Employee>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAsync(string id, UpdateEmployeeDTO employeeDto)
        {
            try
            {
                ObjectValidator<UpdateEmployeeDTO>.CheckIsNotNullObject(employeeDto);

                var employee = await _userManager.FindByIdAsync(id);
                if (employee is null)
                    throw new ArgumentNullException("User Not found");

                employee.Email = employeeDto.Email;
                employee.FirstName = employeeDto.FirstName;
                employee.LastName = employeeDto.LastName;
                employee.MiddleName = employeeDto.MiddleName;

                await _userManager.UpdateAsync(employee);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> DeleteByIdAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var result = await _userManager.DeleteAsync(employee);
                if (result.Succeeded)
                {
                    return ResponseFactory<bool>.CreateSuccessResponse(true);
                }
                else
                {
                    return ResponseFactory<bool>.CreateErrorResponse(new Exception("Failed to delete user."));
                }
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> AssignEmployeeToProjectAsync(List<string> employeesId, Guid projectId)
        {
            try
            {
                ObjectValidator<List<string>>.CheckIsNotNullObject(employeesId);
                ObjectValidator<Guid>.CheckIsNotNullObject(projectId);

                foreach (var emp in employeesId)
                {
                    var employee = await _userManager.FindByIdAsync(emp);
                    ObjectValidator<Employee>.CheckIsNotNullObject(employee);
                }

                var project = await _projectRepository.ReadByIdAsync(projectId);
                project.Employees = employeesId.Select(x => new ProjectEmployee { EmployeeId = x.ToString() }).ToList();

                await _projectRepository.UpdateAsync(project);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> RemoveEmployeeFromProjectAsync(List<string> employeesId, Guid projectId)
        {
            try
            {
                ObjectValidator<List<string>>.CheckIsNotNullObject(employeesId);

                ObjectValidator<Guid>.CheckIsNotNullObject(projectId);

                foreach (var emp in employeesId)
                {
                    var employee = await _userManager.FindByIdAsync(emp);
                    ObjectValidator<Employee>.CheckIsNotNullObject(employee);
                }

                var project = await _projectRepository.GetFilteredProjectByIdAsync(projectId);

                var projectEmployeesToRemove = project.Employees.Where(pe => employeesId.Contains(pe.EmployeeId)).ToList();

                foreach (var projectEmployee in projectEmployeesToRemove)
                {
                    project.Employees.Remove(projectEmployee);
                }

                await _projectRepository.UpdateAsync(project);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> AssignExecutorToTaskAsync(Guid taskId, string employeeId)
        {

            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(taskId);
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var taskItem = await _taskItemRepository.GetFilteredTaskItemByIdAsync(taskId);

                taskItem.Executor = employee;

                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> ChangeTaskStatusAsync(Guid taskId, Status newStatus)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(taskId);


                var taskItem = await _taskItemRepository.ReadByIdAsync(taskId);

                taskItem.Status = newStatus;
                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<IEnumerable<Project>>> GetEmployeeProjectsAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var projects = await _projectRepository.ReadAllAsync().Result
                    .Where(p => p.Employees
                    .Any(pe => pe.EmployeeId == employeeId))
                    .ToListAsync();

                ObjectValidator<IEnumerable<Project>>.CheckIsNotNullObject(projects);

                return ResponseFactory<IEnumerable<Project>>.CreateSuccessResponse(projects);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskItem>>> GetEmployeeTasksAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var manager = await _userManager.FindByIdAsync(employeeId);

                var tasks = _taskItemRepository.ReadAllAsync().Result
                    .Where(t => t.ExecutorId == employeeId)
                    .ToList();




                ObjectValidator<IEnumerable<TaskItem>>.CheckIsNotNullObject(tasks);

                return ResponseFactory<IEnumerable<TaskItem>>.CreateSuccessResponse(tasks);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<string>> CheckUserRole(string userId, Roles role)
        {
            try
            {
                StringValidator.CheckIsNotNull(userId);

                var user = await _userManager.FindByIdAsync(userId);
                ObjectValidator<Employee>.CheckIsNotNullObject(user);
                
                var roleName = role.ToString();

                bool isInRole = await _userManager.IsInRoleAsync(user, roleName);

                if (isInRole)
                {
                    return ResponseFactory<string>.CreateSuccessResponse($"User role is: {roleName}");
                }
                else
                {
                    throw new ArgumentNullException("its role not found");
                }
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<string>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception ex)
            {
                return ResponseFactory<string>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> SetEmployeeNewRoleByIdAsync(string employeeId, Roles roleType)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);
                List<string> roles = new List<string>()
                {
                    Roles.Employee.ToString(),
                    Roles.Manager.ToString(),
                    Roles.Supervisor.ToString(),
                    Roles.Admin.ToString(),
                };

                await _userManager.RemoveFromRolesAsync(employee, roles);

                var result = await _userManager.AddToRoleAsync(employee, roleType.ToString());


                if (result.Succeeded)
                {
                    return ResponseFactory<bool>.CreateSuccessResponse(true);
                }
                else
                {
                    return ResponseFactory<bool>.CreateErrorResponse(new Exception("Failed to set user as role."));
                }

            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }


    }
}

using SibersProject.ApiModels.DTOs.EntitiesDTO.Employee;
using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.ApiModels.Response.Interfaces;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.Services.Services.Interfaces
{
    /// <summary>
    /// Service for managing employee entities and their roles, projects, and tasks.
    /// </summary>
    public interface IEmployeeService
    {
        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result contains the response with the list of employees or an error if the operation fails.</returns>
        Task<IBaseResponse<IEnumerable<Employee>>> GetAllAsync();

        /// <summary>
        /// Retrieves an employee by their ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response with the employee object or an error if the operation fails.</returns>
        Task<IBaseResponse<Employee>> GetByIdAsync(string employeeId);

        /// <summary>
        /// Updates the details of the specified employee.
        /// </summary>
        /// <param name="employeeDto">The employee object containing the updated details.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> UpdateAsync(string id, UpdateEmployeeDTO employeeDto);

        /// <summary>
        /// Deletes the specified employee by their ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to delete.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> DeleteByIdAsync(string employeeId);

        /// <summary>
        /// Sets the specified employee as an Role.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <param name="roleType">The type of role to assign.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> SetEmployeeNewRoleByIdAsync(string employeeId, Roles roleType);
        /// <summary>
        /// Checks the specified employee as an Role.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="role">The type of role to assign</param>
        /// <returns></returns>
        Task<IBaseResponse<string>> CheckUserRole(string userId, Roles role);
        /// <summary>
        /// Retrieves the list of projects associated with an employee.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response with the list of projects or an error if the operation fails.</returns>
        Task<IBaseResponse<IEnumerable<Project>>> GetEmployeeProjectsAsync(string employeeId);

        /// <summary>
        /// Assigns an employee to a project.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to assign.</param>
        /// <param name="projectId">The ID of the project to assign.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> AssignEmployeeToProjectAsync(List<string> employeeId, Guid projectId);

        /// <summary>
        /// Removes an employee from a project.
        /// </summary>
        /// <param name="employeeId">The ID of the employee to remove.</param>
        /// <param name="projectId">The ID of the project to remove the employee from.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> RemoveEmployeeFromProjectAsync(List<string> employeesId, Guid projectId);

        /// <summary>
        /// Retrieves the list of tasks associated with a project.
        /// </summary>
        /// <param name="projectId">The ID of the project.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response with the list of tasks or an error if the operation fails.</returns>
        Task<IBaseResponse<IEnumerable<TaskItem>>> GetEmployeeTasksAsync(string employeeId);

        /// <summary>
        /// Assigns an employee as the executor of a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to assign.</param>
        /// <param name="employeeId">The ID of the employee to assign as the executor.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> AssignExecutorToTaskAsync(Guid taskId, string employeeId);

        /// <summary>
        /// Changes the status of a task.
        /// </summary>
        /// <param name="taskId">The ID of the task to update.</param>
        /// <param name="newStatus">The new status to set for the task.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the response indicating the success or failure of the operation.</returns>
        Task<IBaseResponse<bool>> ChangeTaskStatusAsync(Guid taskId, Status newStatus);
    }
}

using SibersProject.ApiModels.DTOs.BaseDTOs;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.Employee
{
    /// <summary>
    /// Data Transfer Object (DTO) class for representing an employee.
    /// Inherits from BaseUserDTO.
    /// </summary>
    public class CreateEmployeeDTO : BaseUserDTO
    {

        /// <summary>
        /// Gets or sets the first name of the employee.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the employee.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the middle name of the employee.
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the employee.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the role of the employee.
        /// </summary>
        public Roles UserType { get; set; }
    }
}

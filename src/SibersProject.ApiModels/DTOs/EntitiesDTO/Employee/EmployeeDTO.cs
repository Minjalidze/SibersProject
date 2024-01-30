using SibersProject.MainDomain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.Employee
{
    public class EmployeeDTO
    {
        /// <summary>
        /// Employee Id
        /// </summary>
        public string? Id { get; set; }
        /// <summary>
        /// Employee UserName
        /// </summary>
        public string? UserName { get; set; }
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

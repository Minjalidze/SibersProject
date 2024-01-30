using SibersProject.ApiModels.DTOs.BaseDTOs;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.Project
{
    /// <summary>
    /// Data Transfer Object (DTO) class for create a project.
    /// Inherits from BaseEntityDTO.
    /// </summary>
    public class CreateProjectDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the client company.
        /// </summary>
        public string? ClientCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the executing company.
        /// </summary>
        public string? ExecutiveCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the start date of the project.
        /// The default value is set to the current UTC date and time.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end date of the project.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the priority of the project.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets ProjectManagerId
        /// </summary>
        public string? ProjectManagerId { get; set; }

        public List<string>? EmployeesIds { get; set; }
    }
}

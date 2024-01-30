using SibersProject.ApiModels.DTOs.BaseDTOs;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem
{
    public class UpdateTaskItemWithAuthorDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with the task.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the priority of the task.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets AuthorId
        /// </summary>
        public string? AuthorId { get; set; }
    }
}

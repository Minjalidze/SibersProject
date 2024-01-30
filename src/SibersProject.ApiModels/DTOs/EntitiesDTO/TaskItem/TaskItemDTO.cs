using SibersProject.MainDomain.Models.Enums;
using System.Text.Json.Serialization;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem
{
    public class TaskItemDTO
    {
        /// <summary>
        /// Task Item id
        /// </summary>
        [JsonIgnore]
        public Guid TaskItemId { get; set; }
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
        /// Gets or sets the ID of the author associated with the task.
        /// Nullable property.
        /// </summary>
        public string? AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the assignee associated with the task.
        /// Nullable property.
        /// </summary>
        public string? ExecutorId { get; set; }
    }
}

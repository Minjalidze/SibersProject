using SibersProject.MainDomain.Models.Abstractions.BaseEntities;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.MainDomain.Models.Entities
{
    public class TaskItem : BaseEntity
    {
        public string? Name { get; set; }
        public string? Comment { get; set; }
        public Status Status { get; set; }
        public int Priority { get; set; }

        public Employee? Author { get; set; }
        public string? AuthorId { get; set; }
        public Employee? Executor { get; set; }
        public string? ExecutorId { get; set; }
    }
}

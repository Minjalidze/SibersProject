using SibersProject.MainDomain.Models.Abstractions.BaseUsers;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.MainDomain.Models.Entities
{
    public class Employee : ApplicationUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public Roles UserType { get; set; }

        public string FullName() => $"{FirstName} {LastName} {MiddleName}";

        public ICollection<ProjectEmployee>? Projects { get; set; } 
        public ICollection<TaskItem>? ExecutorTasks { get; set; } 
        public ICollection<TaskItem>? AuthoredTasks { get; set; } 
    }
}

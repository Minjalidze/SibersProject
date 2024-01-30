namespace SibersProject.MainDomain.Models.Entities
{
    public class ProjectEmployee
    {
        public Guid? ProjectId { get; set; }
        public string? EmployeeId { get; set; }

        public Employee? Employee { get; set; }
        public Project? Project { get; set; }
    }
}

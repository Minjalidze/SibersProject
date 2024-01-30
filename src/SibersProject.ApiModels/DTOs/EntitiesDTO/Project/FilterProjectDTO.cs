using SibersProject.ApiModels.DTOs.BaseDTOs;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.Project
{
    public class FilterProjectDTO : BaseEntityDTO
    {
        public string? Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }

        public bool ShouldFilterByName() => !string.IsNullOrWhiteSpace(Name);
    }
}

using SibersProject.ApiModels.DTOs.BaseDTOs;
using SibersProject.MainDomain.Models.Enums;

namespace SibersProject.ApiModels.DTOs.EntitiesDTO.Project
{
    public class SortProjectDTO : BaseEntityDTO
    {
        public Sort NameSort { get; set; }
        public Sort PrioritySort { get; set; }
    }
}

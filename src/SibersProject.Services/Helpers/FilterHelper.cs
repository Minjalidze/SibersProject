using SibersProject.ApiModels.DTOs.EntitiesDTO.TaskItem;
using SibersProject.ApiModels.DTOs.EntitiesDTO.Project;
using SibersProject.MainDomain.Models.Entities;
using LinqKit;

namespace SibersProject.Services.Helpers
{
    public static class FilterHelper
    {
        public static ExpressionStarter<Project> CreateProjectFilter(FilterProjectDTO? projectFilterDto)
        {
            var filter = PredicateBuilder.New<Project>(true);

            if (projectFilterDto is null) return filter;

            if (projectFilterDto.ShouldFilterByName())
                filter = filter.And(x => x.Name.Contains(projectFilterDto.Name));

            if (projectFilterDto.StartDate.HasValue)
                filter = filter.And(x => x.StartDate >= projectFilterDto.StartDate);


            if (projectFilterDto.EndDate.HasValue)
                filter = filter.And(x => x.EndDate <= projectFilterDto.EndDate);

            if (projectFilterDto.Priority.HasValue)
                filter = filter.And(x => x.Priority == projectFilterDto.Priority);

            return filter;
        }

        public static ExpressionStarter<TaskItem> CreateTaskItemFilter(FilterTaskItemDTO? taskItemFilterDto)
        {
            var filter = PredicateBuilder.New<TaskItem>(true);

            if (taskItemFilterDto is null) return filter;

            if (taskItemFilterDto.ShouldFilterByName())
                filter = filter.And(x => x.Name.Contains(taskItemFilterDto.Name));

            if (taskItemFilterDto.Status.HasValue)
                filter = filter.And(x => x.Status == taskItemFilterDto.Status);

            if (taskItemFilterDto.Priority.HasValue)
                filter = filter.And(x => x.Priority == taskItemFilterDto.Priority);

            return filter;
        }
    }
}

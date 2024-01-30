using SibersProject.MainDomain.Models.Entities;
using System.Linq.Expressions;

namespace SibersProject.DataAL.Repository.Interfaces
{
    public interface ITaskItemRepository : IBaseAsyncRepository<TaskItem>
    {
        public Task<IEnumerable<TaskItem>> GetFilteredTaskItemsAsync(Expression<Func<TaskItem, bool>> filter);
        public Task<TaskItem> GetFilteredTaskItemByIdAsync(Guid Id);
    }
}

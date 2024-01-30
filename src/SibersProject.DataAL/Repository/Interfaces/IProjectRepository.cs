using SibersProject.DataAL.SqlServer;
using SibersProject.MainDomain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SibersProject.DataAL.Repository.Interfaces
{
    public interface IProjectRepository : IBaseAsyncRepository<Project>
    {
        public Task<IEnumerable<Project>> GetFilteredProjectAsync(Expression<Func<Project, bool>> filter);

        public Task<Project> GetFilteredProjectByIdAsync( Guid Id);

    }
}

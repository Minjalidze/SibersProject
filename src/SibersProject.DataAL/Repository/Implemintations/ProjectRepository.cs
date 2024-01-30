using SibersProject.DataAL.Repository.Interfaces;
using SibersProject.DataAL.SqlServer;
using SibersProject.MainDomain.Models.Entities;
using SibersProject.Validator;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace SibersProject.DataAL.Repository.Implemintations
{
    public class ProjectRepository : BaseAsyncRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetFilteredProjectAsync(Expression<Func<Project, bool>> filter)
        {
            ObjectValidator<Expression<Func<Project, bool>>>.CheckIsNotNullObject(filter);

            return await ReadAll()
                .Where(filter)
                .ToListAsync();

        }

        public async Task<Project> GetFilteredProjectByIdAsync(Guid id)
        {
            var project = await ReadAll()
                  .Include(x => x.Employees)
                  .ThenInclude(x => x.Employee)
                  .Include(x => x.ProjectManager)
                  .FirstOrDefaultAsync(x => x.Id == id);

            return project == null
           ? throw new ArgumentNullException(nameof(id), $"Entity not found by id {id} in Repository")
           : project;
        }
    }
}

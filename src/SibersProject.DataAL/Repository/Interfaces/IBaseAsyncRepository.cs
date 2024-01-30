using SibersProject.MainDomain.Models.Abstractions.BaseEntities;

namespace SibersProject.DataAL.Repository.Interfaces
{
    /// <summary>
    /// Represents a repository for managing entities.
    /// </summary>
    /// <typeparam name="T">managed entity</typeparam>
    public interface IBaseAsyncRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Creates a record in the database and adds the entity.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <returns></returns>
        public Task Create(T entity);

        /// <summary>
        /// Retrieves all records from the database.
        /// </summary>
        /// <returns>A query that returns all records.</returns>
        public IQueryable<T> ReadAll();

        /// <summary>
        /// Asynchronously retrieves all records from the database.
        /// </summary>
        /// <returns>A task that returns a query that returns all records.</returns>
        public Task<IQueryable<T>> ReadAllAsync();

        /// <summary>
        /// Retrieves an entity by the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the specified identifier.</returns>
        public T ReadById(Guid id);

        /// <summary>
        /// Asynchronously retrieves an entity by the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>A task that returns the entity with the specified identifier.</returns>
        public Task<T> ReadByIdAsync(Guid id);

        /// <summary>
        /// Updates the specified entity in the database.
        /// </summary>
        /// <param name="item">The entity to be updated.</param>
        /// <returns></returns>
        public Task UpdateAsync(T item);

        /// <summary>
        /// Deletes the specified entity from the database.
        /// </summary>
        /// <param name="item">The entity to be deleted.</param>
        /// <returns></returns>
        public Task DeleteAsync(T item);

        /// <summary>
        /// Deletes the entity with the specified identifier from the database.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns></returns>
        public Task DeleteByIdAsync(Guid id);


    }
}

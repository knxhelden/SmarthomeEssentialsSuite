using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services
{
    /// <summary>
    /// Defines a generic service interface for managing entities with observable counterparts.
    /// </summary>
    /// <typeparam name="TObservable">The observable representation of the entity.</typeparam>
    /// <typeparam name="TEntity">The underlying entity type.</typeparam>
    public interface IEntityService<TObservable, TEntity> : IService
        where TEntity : EntityBase
        where TObservable : ObservableBase<TEntity>
    {
        /// <summary>
        /// Retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the requested observable entity.</returns>
        Task<TObservable> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all entities as an observable collection.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all observable entities.</returns>
        Task<ObservableCollection<TObservable>> GetAllAsync();

        /// <summary>
        /// Creates a new entity from an observable representation.
        /// </summary>
        /// <param name="observableProject">The observable entity to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the creation was successful.</returns>
        Task<bool> CreateAsync(TObservable observableProject);

        /// <summary>
        /// Updates an existing entity using an observable representation.
        /// </summary>
        /// <param name="observableProject">The observable entity with updated data.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
        Task<bool> UpdateAsync(TObservable observableProject);

        /// <summary>
        /// Deletes an entity using its observable representation.
        /// </summary>
        /// <param name="observableProject">The observable entity to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(TObservable observableProject);
    }
}

using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services
{
    /// <summary>
    /// Defines a generic contract for services that manage entity models and their observable counterparts.
    /// Provides CRUD operations that operate on observables but persist changes to the underlying entity layer.
    /// </summary>
    /// <typeparam name="TObservable">
    /// The type of the observable wrapper, derived from <see cref="ObservableBase{TEntity}"/>.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The type of the underlying entity, derived from <see cref="EntityBase"/>.
    /// </typeparam>
    public interface IEntityService<TObservable, TEntity> : IService
        where TEntity : EntityBase
        where TObservable : ObservableBase<TEntity>
    {
        /// <summary>
        /// Retrieves an observable entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>
        /// An instance of <typeparamref name="TObservable"/> wrapping the retrieved entity, or <c>null</c> if an error occurs.
        /// </returns>
        Task<TObservable> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves all observable entities.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The result is a collection of all observable entities.
        /// </returns>
        Task<ObservableCollection<TObservable>> GetAllAsync();

        /// <summary>
        /// Creates a new entity based on the given observable model.
        /// </summary>
        /// <param name="observableProject">The observable model to create.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        Task<bool> CreateAsync(TObservable observableProject);

        /// <summary>
        /// Updates an existing entity based on the given observable model.
        /// </summary>
        /// <param name="observableProject">The observable model with updated data.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        Task<bool> UpdateAsync(TObservable observableProject);

        /// <summary>
        /// Deletes an existing entity based on the given observable model.
        /// </summary>
        /// <param name="observableProject">The observable model to delete.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        Task<bool> DeleteAsync(TObservable observableProject);
    }
}

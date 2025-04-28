using KnxHelden.SHES.Data.Repositories;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services
{
    /// <summary>
    /// Abstract base class for services that handle entities and their associated observables.
    /// Provides common functionality for data access through a generic repository.
    /// </summary>
    /// <typeparam name="TObservable">
    /// The type of the observable model, inheriting from <see cref="ObservableBase{TEntity}"/>.
    /// </typeparam>
    /// <typeparam name="TEntity">
    /// The type of the entity, inheriting from <see cref="EntityBase"/>.
    /// </typeparam>
    public abstract class EntityServiceBase<TObservable, TEntity> : ServiceBase, IEntityService<TObservable, TEntity>
        where TEntity : EntityBase, new()
        where TObservable : ObservableBase<TEntity>, new()
    {
        private readonly IRepository<TEntity> _repository;

        #region --- Constructor ---

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityServiceBase{TObservable, TEntity}"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader used for localization and resource access.</param>
        /// <param name="logger">The logger instance used for logging diagnostic messages.</param>
        /// <param name="repository">The repository instance used for accessing entity data.</param>
        protected EntityServiceBase(ResourceLoader resourceLoader, ILogger logger, IRepository<TEntity> repository)
            : base(resourceLoader, logger)
        {
            _repository = repository;
        }

        #endregion

        /// <summary>
        /// Retrieves an observable entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>
        /// An instance of <typeparamref name="TObservable"/> wrapping the retrieved entity, or <c>null</c> if an error occurs.
        /// </returns>
        public async Task<TObservable> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await this._repository.GetByIdAsync(id);
                return (TObservable)Activator.CreateInstance(typeof(TObservable), entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load entity in GetByIdAsync.");
                return null;
            }
        }

        /// <summary>
        /// Retrieves all observable entities.
        /// </summary>
        /// <returns>
        /// A task representing the asynchronous operation.
        /// The result is a collection of all observable entities.
        /// </returns>
        public async Task<ObservableCollection<TObservable>> GetAllAsync()
        {
            try
            {
                var entities = await _repository.GetAllAsync();
                var observables = new ObservableCollection<TObservable>();

                foreach (var entity in entities)
                {
                    var observable = (TObservable)Activator.CreateInstance(typeof(TObservable), entity);
                    observables.Add(observable);
                }

                return observables;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load entities in GetAllAsync.");
                return new ObservableCollection<TObservable>();
            }
        }

        /// <summary>
        /// Creates a new entity based on the given observable model.
        /// </summary>
        /// <param name="observable">The observable model to create.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        public async Task<bool> CreateAsync(TObservable observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            try
            {
                await _repository.CreateAsync(observable.entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create entity in CreateAsync.");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing entity based on the given observable model.
        /// </summary>
        /// <param name="observable">The observable model with updated data.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        public async Task<bool> UpdateAsync(TObservable observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            try
            {
                await _repository.UpdateAsync(observable.entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update entity in UpdateAsync.");
                return false;
            }
        }

        /// <summary>
        /// Deletes an existing entity based on the given observable model.
        /// </summary>
        /// <param name="observable">The observable model to delete.</param>
        /// <returns>A task representing the asynchronous operation. The result indicates success or failure.</returns>
        public async Task<bool> DeleteAsync(TObservable observable)
        {
            if (observable == null)
            {
                throw new ArgumentNullException(nameof(observable));
            }

            try
            {
                await _repository.DeleteAsync(observable.entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete entity in DeleteAsync.");
                return false;
            }
        }
    }
}

using KnxHelden.SHES.Data.Repositories;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services
{
    public abstract class EntityServiceBase<TObservable, TEntity> : ServiceBase, IEntityService<TObservable, TEntity>
        where TObservable : class, new()
        where TEntity : EntityBase, new()
    {
        private readonly IRepository<TEntity> _repository;

        #region --- Constructor ---

        public EntityServiceBase(ResourceLoader resourceLoader, ILogger logger, IRepository<TEntity> repository)
            : base(resourceLoader, logger)
        {
            _repository = repository;
        }

        #endregion

        public async Task<TObservable> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await this._repository.GetByIdAsync(id);

                return (TObservable)Activator.CreateInstance(typeof(TObservable), entity);
            }
            catch
            {
                return null;
            }
        }

        public async Task<ObservableCollection<TObservable>> GetAllAsync(string orderBy = "")
        {
            try
            {
                var entities = await this._repository.GetAllAsync(orderBy);

                var observables = new ObservableCollection<TObservable>(
                    entities.Select(entity => (TObservable)Activator.CreateInstance(typeof(TObservable), entity))
                );

                return observables;
            }
            catch
            {
                return new ObservableCollection<TObservable>();
            }
        }
    }
}

using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services
{
    public interface IEntityService<TObservable, TEntity> : IService
        where TObservable : class, new()
        where TEntity : EntityBase, new()
    {
        Task<TObservable> GetByIdAsync(Guid id);

        Task<ObservableCollection<TObservable>> GetAllAsync(string orderBy = "");
    }
}

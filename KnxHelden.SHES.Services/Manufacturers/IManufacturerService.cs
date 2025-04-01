using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Manufacturers
{
    public interface IManufacturerService : IService
    {
        Task<List<ObservableManufacturer>> GetAllAsync();

        Task UpdateAsync(ObservableManufacturer observableManufacturer);
    }
}

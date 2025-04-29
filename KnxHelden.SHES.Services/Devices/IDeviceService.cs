using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Devices
{
    public interface IDeviceService : IEntityService<ObservableDevice, Device>
    {
        Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem);
    }
}

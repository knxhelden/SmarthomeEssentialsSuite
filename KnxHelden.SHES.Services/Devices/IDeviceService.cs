using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Devices
{
    public interface IDeviceService : IService
    {
        Task<ObservableDevice> GetDeviceAsync(Guid id);

        Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem);
        Task UpdateAsync(ObservableDevice observableDevice);
        //Task UpdateRangeAsync(ObservableCollection<ObservableDevice> observableDevices);
    }
}

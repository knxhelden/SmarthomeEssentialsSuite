using KnxHelden.SHES.Data.Repositories.Devices;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Devices
{
    public class DeviceService : ServiceBase, IDeviceService
    {
        private readonly IDeviceRepository _deviceRepository;

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="DeviceService" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="deviceRepository">The device repository.</param>
        public DeviceService(ResourceLoader resourceLoader, ILogger<DeviceService> logger, IDeviceRepository deviceRepository)
            : base(resourceLoader, logger)
        {
            this._deviceRepository = deviceRepository;
        }

        #endregion

        #region --- IDeviceService ---

        public async Task<ObservableDevice> GetDeviceAsync(Guid id)
        {
            try
            {
                var device = await this._deviceRepository.GetByIdAsync(id);

                return new ObservableDevice(device);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Gets the devices for location asynchronous.</summary>
        /// <param name="observableProjectItem">The observable project item.</param>
        /// <returns>Returns devices.</returns>
        public async Task<List<ObservableDevice>> GetDevicesForLocationAsync(ObservableProjectItem observableProjectItem)
        {
            try
            {
                var devices = await this._deviceRepository.GetDevicesForLocationAsync(observableProjectItem.entity);

                return devices.Select(d => new ObservableDevice(d)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task UpdateAsync(ObservableDevice observableDevice)
        {
            await this._deviceRepository.UpdateAsync(observableDevice.entity as Device);
        }

        #endregion
    }
}

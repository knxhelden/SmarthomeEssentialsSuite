using KnxHelden.SHES.Data.Repositories.Manufacturers;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Manufacturers
{
    public class ManufacturerService : ServiceBase, IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ManufacturerService" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="deviceRepository">The manufacturer repository.</param>
        public ManufacturerService(ResourceLoader resourceLoader, ILogger<DeviceService> logger, IManufacturerRepository manufacturerRepository)
            : base(resourceLoader, logger)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        #endregion

        public async Task<List<ObservableManufacturer>> GetManufacturersAsync()
        {
            try
            {
                var manufacturers = await this._manufacturerRepository.GetAllAsync();

                return manufacturers.Select(m => new ObservableManufacturer(m)).ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}

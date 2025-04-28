using KnxHelden.SHES.Data.Repositories;
using KnxHelden.SHES.Data.Repositories.Manufacturers;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;

namespace KnxHelden.SHES.Services.Manufacturers
{
    public class ManufacturerService : EntityServiceBase<ObservableManufacturer, Manufacturer>, IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ManufacturerService" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="deviceRepository">The manufacturer repository.</param>
        public ManufacturerService(ResourceLoader resourceLoader, ILogger<DeviceService> logger, IRepository<Manufacturer> repository, IManufacturerRepository manufacturerRepository)
            : base(resourceLoader, logger, repository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        #endregion
    }
}

using KnxHelden.SHES.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Data.Repositories.Devices
{
    public interface IDeviceRepository : IRepository<Device>
    {
        Task<List<Device>> GetDevicesForLocationAsync(ProjectItem projectItem);
    }
}
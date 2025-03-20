using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Settings
{
    public interface ISettingService : IService
    {
        Task<T> ReadSettingAsync<T>(string key);

        Task SaveSettingAsync<T>(string key, T value);
    }
}

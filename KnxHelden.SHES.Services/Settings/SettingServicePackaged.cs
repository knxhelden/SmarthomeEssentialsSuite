using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Shared.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Threading.Tasks;
using Windows.Storage;

namespace KnxHelden.SHES.Services.Settings
{
    public class SettingServicePackaged : ServiceBase, ISettingService
    {
        #region --- Constructor ---

        public SettingServicePackaged(ResourceLoader resourceLoader, ILogger<DeviceService> logger)
            : base(resourceLoader, logger)
        {
            
        }

        #endregion

        public async Task<T> ReadSettingAsync<T>(string key)
        {
            object obj = null;

            if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out obj))
            {
                return await JsonHelper.ToObjectAsync<T>((string)obj);
            }

            return default;
        }

        public async Task SaveSettingAsync<T>(string key, T value)
        {
            ApplicationData.Current.LocalSettings.Values[key] = await JsonHelper.StringifyAsync(value);
        }
    }
}

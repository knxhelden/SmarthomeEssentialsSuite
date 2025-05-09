using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Services.Files;
using KnxHelden.SHES.Shared.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Settings
{
    public class SettingServiceUnpackaged : ServiceBase, ISettingService
    {
        private readonly IFileService _fileService;
        private readonly LocalSettingsOptions _options;
        private readonly string _localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        private IDictionary<string, object> _settings;

        #region --- Constructor ---

        public SettingServiceUnpackaged(ResourceLoader resourceLoader, ILogger<DeviceService> logger, IFileService fileService, IOptions<LocalSettingsOptions> options)
            : base(resourceLoader, logger)
        {
            _fileService = fileService;
            _options = options.Value;
        }

        #endregion

        public async Task<T> ReadSettingAsync<T>(string key)
        {
            await InitializeAsync();

            object obj = null;

            if (_settings.TryGetValue(key, out obj))
            {
                return await JsonHelper.ToObjectAsync<T>((string)obj);
            }

            return default;
        }

        public async Task SaveSettingAsync<T>(string key, T value)
        {
            await InitializeAsync();

            _settings[key] = await JsonHelper.StringifyAsync(value);

            var folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);
            var fileName = _options.LocalSettingsFile;
            await Task.Run(() => _fileService.Save(folderPath, fileName, _settings));
        }

        private async Task InitializeAsync()
        {
            if (_settings is null)
            {
                var folderPath = Path.Combine(_localAppData, _options.ApplicationDataFolder);
                var fileName = _options.LocalSettingsFile;
                _settings = await Task.Run(() => _fileService.Read<IDictionary<string, object>>(folderPath, fileName)) ?? new Dictionary<string, object>();
            }
        }
    }
}

using KnxHelden.SHES.Services.Settings;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.Services.ThemeService
{
    public class ThemeService : IThemeService
    {
        private readonly ISettingService _settingsService;
        private const string SettingsKey = "AppBackgroundRequestedTheme";

        public ElementTheme Theme { get; set; } = ElementTheme.Default;

        #region --- Constructor ---

        public ThemeService(ISettingService settingsService)
        {
            _settingsService = settingsService;
        }

        #endregion

        #region --- IThemeSelectorService ---

        /// <summary>
        /// Gets the theme currently saved in the settings.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task GetThemeAsync()
        {
            Theme = await LoadThemeFromSettingsAsync();
            await SetRequestedThemeAsync();
        }

        /// <summary>
        /// Sets the theme and saves it in the settings.
        /// </summary>
        /// <param name="theme">The theme.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetThemeAsync(ElementTheme theme)
        {
            Theme = theme;

            await SetRequestedThemeAsync();
            await SaveThemeInSettingsAsync(Theme);
        }

        #endregion

        #region --- Methods ---

        private async Task SetRequestedThemeAsync()
        {
            if (App.MainWindow.Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = Theme;
            }

            await Task.CompletedTask;
        }

        private async Task<ElementTheme> LoadThemeFromSettingsAsync()
        {
            ElementTheme cacheTheme = ElementTheme.Default;
            string themeName = await _settingsService.ReadSettingAsync<string>(SettingsKey);

            if (!string.IsNullOrEmpty(themeName))
            {
                Enum.TryParse(themeName, out cacheTheme);
            }

            return cacheTheme;
        }

        private async Task SaveThemeInSettingsAsync(ElementTheme theme)
        {
            await _settingsService.SaveSettingAsync(SettingsKey, theme.ToString());
        }

        #endregion
    }
}

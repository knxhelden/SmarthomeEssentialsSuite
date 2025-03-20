using Microsoft.UI.Xaml;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.Services.ThemeService
{
    public interface IThemeService
    {
        ElementTheme Theme { get; set; }

        /// <summary>
        /// Gets the theme currently saved in the settings.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task GetThemeAsync();

        /// <summary>
        /// Sets the theme and saves it in the settings.
        /// </summary>
        /// <param name="theme">The theme.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetThemeAsync(ElementTheme theme);
    }
}

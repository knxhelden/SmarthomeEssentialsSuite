using CommunityToolkit.Mvvm.ComponentModel;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Manufacturers;
using KnxHelden.SHES.Shared.Extensions;
using Microsoft.UI.Xaml;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ViewModels
{
    public sealed class ManufacturerViewModel : ObservableRecipient
    {
        private IManufacturerService _manufacturerService;

        public ObservableCollection<ObservableManufacturer> Manufacturers { get; set; } = [];

        #region --- Constructor ---

        public ManufacturerViewModel(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        #endregion

        #region --- Events ---

        public async Task Page_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadManufacturersAsync();
        }

        #endregion

        #region --- Methods ---

        private async Task LoadManufacturersAsync()
        {
            var manufacturers = await this._manufacturerService.GetManufacturersAsync();
            Manufacturers.AddRange(manufacturers);

            this.OnPropertyChanged(nameof(Manufacturers));
        }

        #endregion
    }
}

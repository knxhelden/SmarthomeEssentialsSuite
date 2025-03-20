using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ComponentModels
{
    public sealed class ProjectItemDevicesComponentModel : ObservableRecipient
    {
        private readonly IDeviceService _deviceService;

        #region --- Properties ---

        public ObservableCollection<ObservableDevice> Devices { get; private set; } = new ObservableCollection<ObservableDevice>();

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentProjectItem, value);

                    this.LoadDevicesForProjectItemAsync();
                }
            }
        }

        #endregion

        #region --- Constructor ---

        public ProjectItemDevicesComponentModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;

            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemDevicesComponentModel, CurrentProjectItemSenderMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);
        }

        #endregion

        #region --- Methods ---

        private async void LoadDevicesForProjectItemAsync()
        {
            var devices = await this._deviceService.GetDevicesForLocationAsync(this.CurrentProjectItem);
            this.Devices = new ObservableCollection<ObservableDevice>(devices);
            this.OnPropertyChanged(nameof(this.Devices));

            //// Apply existing filter
            //this.OnTimedEvent(this._dataGridFilterTimer, null);

            //// Apply existing sorting
            //this.SortDevices();
        }

        #endregion
    }
}

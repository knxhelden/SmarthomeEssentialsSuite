using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Controls;
using KnxHelden.SHES.Controls.FormFieldService;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Enumerations;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Services.Manufacturers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;

namespace KnxHelden.SHES.App.ComponentModels
{
    public sealed class ProjectItemDetailsComponentModel : ObservableRecipient
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly IManufacturerService _manufacturerService;
        private readonly IDeviceService _deviceService;
        private readonly IFormFieldService _formFieldService;

        #region --- Properties ---

        private ObservableDevice _currentDevice;
        public ObservableDevice CurrentDevice
        {
            get => _currentDevice;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentDevice, value);

                    this.LoadFormFields();
                }
            }
        }

        public ObservableCollection<FormField> FormFields { get; } = new();

        #endregion

        #region --- Constructor ---

        public ProjectItemDetailsComponentModel(ResourceLoader resourceLoader, IManufacturerService manufacturerService, IDeviceService deviceService, IFormFieldService formFieldService)
        {
            _resourceLoader = resourceLoader;
            _manufacturerService = manufacturerService;
            _deviceService = deviceService;
            _formFieldService = formFieldService;

            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemDetailsComponentModel, CurrentProjectItemSenderMessage>(this, (r, m) =>
            {
                if (m.Value.entity is Device device)
                {
                    CurrentDevice = _deviceService.GetDeviceAsync(m.Value.Id).Result;
                }
            });
        }

        #endregion

        #region --- Events ---

        public async void FormField_SaveChanges(object sender, object e)
        {
            await this._deviceService.UpdateAsync(this.CurrentDevice);
        }

        #endregion

        #region --- Methods ---

        private async void LoadFormFields()
        {
            var manufacturers = await _manufacturerService.GetAllAsync();

            FormFields.Clear();

            // General device fields
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Identifier"),
    _formFieldService.GetTextBox(CurrentDevice, nameof(CurrentDevice.Identifier), FormField_SaveChanges)));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DeviceType"),
                _formFieldService.GetEnumComboBox<DeviceType>(CurrentDevice, nameof(CurrentDevice.Type), FormField_SaveChanges)));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_BusType"),
                _formFieldService.GetEnumComboBox<BusType>(CurrentDevice, nameof(CurrentDevice.BusType), FormField_SaveChanges)));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Manufacturer"),
                _formFieldService.GetComboBox(CurrentDevice, "Manufacturer", FormField_SaveChanges, manufacturers, "Name", "Id")));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_OrderNumber"),
                _formFieldService.GetTextBox(CurrentDevice, nameof(CurrentDevice.OrderNumber), FormField_SaveChanges)));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_PhysicalKnxAddress"),
                new TextBox()));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_RailMount"),
                _formFieldService.GetCheckBox(CurrentDevice, nameof(CurrentDevice.IsRailMounted), FormField_SaveChanges)));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DivisionUnits"),
                new ComboBox()));
        }

        #endregion
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Controls.ControlFactory;
using KnxHelden.SHES.Controls.FormFieldService;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Enumerations;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Services.Manufacturers;
using KnxHelden.SHES.Shared.Helpers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

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
                if (_currentDevice != value)
                {
                    _currentDevice = value;
                    this.GenerateFormFields();
                }
            }
        }

        public ObservableCollection<FormField> FormFields { get; } = new();

        public IAsyncRelayCommand UpdateProjectItemCommand { get; }

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
                    CurrentDevice = _deviceService.GetByIdAsync(m.Value.Id).Result;
                }
            });

            // Commands
            UpdateProjectItemCommand = new AsyncRelayCommand(async (dialog) => await UpdateProjectItem());
        }

        #endregion

        #region --- Methods ---

        private async void GenerateFormFields()
        {
            var manufacturers = await _manufacturerService.GetAllAsync();

            FormFields.Clear();

            var configName = new FormFieldConfig
            {
                Label = _resourceLoader.GetString("StructureView_ProjectItemDetails_Name"),
                PropertyName = "ProjectItem.Name",
                PropertyPath = PropertyPathHelper.GetPropertyPath(() => CurrentDevice.ProjectItem.Name),
                FieldType = FormFieldType.TextBox
            };

            FormFields.Add(new FormField(configName, CurrentDevice));

            var configIdentifier = new FormFieldConfig
            {
                Label = _resourceLoader.GetString("StructureView_ProjectItemDetails_Identifier"),
                PropertyName = nameof(CurrentDevice.Identifier),
                PropertyPath = PropertyPathHelper.GetPropertyPath(() => CurrentDevice.Identifier),
                FieldType = FormFieldType.TextBox
            };

            FormFields.Add(new FormField(configIdentifier, CurrentDevice));

            var configType = new FormFieldConfig
            {
                Label = _resourceLoader.GetString("StructureView_ProjectItemDetails_DeviceType"),
                PropertyName = nameof(CurrentDevice.Type),
                PropertyPath = PropertyPathHelper.GetPropertyPath(() => CurrentDevice.Type),
                FieldType = FormFieldType.EnumComboBox,
                EnumType = typeof(DeviceType)
            };

            FormFields.Add(new FormField(configType, CurrentDevice));


            // General device fields
            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Identifier"),
            //    _formFieldService.GetTextBox(CurrentDevice, nameof(CurrentDevice.Identifier)),
            //    "CurrentDevice.Identifier"));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DeviceType"),
            //    _formFieldService.GetEnumComboBox<DeviceType>(CurrentDevice, nameof(CurrentDevice.Type))));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_BusType"),
            //    _formFieldService.GetEnumComboBox<BusType>(CurrentDevice, nameof(CurrentDevice.BusType))));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Manufacturer"),
            //    _formFieldService.GetComboBox(CurrentDevice, "Manufacturer", manufacturers, "Name", "Id")));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_OrderNumber"),
            //    _formFieldService.GetTextBox(CurrentDevice, nameof(CurrentDevice.OrderNumber))));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_PhysicalKnxAddress"),
            //    _formFieldService.GetTextBox(CurrentDevice, nameof(CurrentDevice.PhysicalAddress))));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_RailMount"),
            //    _formFieldService.GetCheckBox(CurrentDevice, nameof(CurrentDevice.IsRailMounted))));

            //FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DivisionUnits"),
            //    _formFieldService.GetNumberBox(CurrentDevice, nameof(CurrentDevice.DivisionUnits))));
        }

        #endregion

        #region --- Commands ---

        private async Task UpdateProjectItem()
        {
            await _deviceService.UpdateAsync(CurrentDevice);
        }

        #endregion
    }
}

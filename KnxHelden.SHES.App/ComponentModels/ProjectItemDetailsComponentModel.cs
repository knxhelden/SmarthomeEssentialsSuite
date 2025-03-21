using CommunityToolkit.Mvvm.ComponentModel;
using KnxHelden.SHES.Controls;
using KnxHelden.SHES.Models.Observables;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Controls.Converters;
using Microsoft.UI.Xaml.Markup;

namespace KnxHelden.SHES.App.ComponentModels
{
    public sealed class ProjectItemDetailsComponentModel : ObservableRecipient
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly IDeviceService _deviceService;

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

        public ProjectItemDetailsComponentModel(ResourceLoader resourceLoader, IDeviceService deviceService)
        {
            _resourceLoader = resourceLoader;
            _deviceService = deviceService;

            // Messages
            //WeakReferenceMessenger.Default.Register<ProjectItemDetailsComponentModel, CurrentProjectItemSenderMessage>(this, (r, m) => r.CurrentDevice = m.Value);
            WeakReferenceMessenger.Default.Register<ProjectItemDetailsComponentModel, CurrentProjectItemSenderMessage>(this, (r, m) =>
            {
                if (m.Value.entity is Device device)
                {
                    CurrentDevice = _deviceService.GetDeviceAsync(m.Value.Id).Result;
                }
            });
        }

        #endregion

        #region --- Methods ---

        private void LoadFormFields()
        {
            FormFields.Clear();

            // General device fields
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Identifier"),
        GetBoundTextBox(CurrentDevice, nameof(CurrentDevice.Identifier))));

            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Identifier"), new TextBox()));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DeviceType"), GetEnumComboBox<DeviceType>(CurrentDevice, nameof(CurrentDevice.Type))));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_BusType"), GetEnumComboBox<BusType>(CurrentDevice, nameof(CurrentDevice.BusType))));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_Manufacturer"), new TextBox()));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_OrderNumber"), new TextBox()));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_PhysicalKnxAddress"), new TextBox()));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_RailMount"), new TextBox()));
            FormFields.Add(new FormField(_resourceLoader.GetString("StructureView_ProjectItemDetails_DivisionUnits"), new ComboBox()));
        }

        private TextBox GetBoundTextBox(object source, string propertyName)
        {
            var textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            return textBox;
        }

        private ComboBox GetEnumComboBox<T>(object source, string propertyName) where T : struct, Enum
        {
            

            var comboBox = new ComboBox
            {
                ItemsSource = Enum.GetValues(typeof(T)).Cast<T>().ToList()
            };

            string xamlTemplate =
                "<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
                    "<TextBlock Text='{Binding Converter={StaticResource EnumDisplayNameConverter}}'/>" +
                "</DataTemplate>";

            comboBox.ItemTemplate = (DataTemplate)XamlReader.Load(xamlTemplate);

            // Binding for SelectedItem
            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

            return comboBox;
        }

        #endregion
    }
}

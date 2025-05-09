using KnxHelden.SHES.Models.Attributes;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Enumerations;
using KnxHelden.SHES.Models.Strings;
using KnxHelden.SHES.Models.Validations;
using KnxHelden.SHES.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KnxHelden.SHES.Models.Observables
{
    public class ObservableDevice : ObservableBase<Device>
    {
        private readonly ObservableProjectItem _projectItem;

        #region --- Properties ---

        public ObservableProjectItem ProjectItem
        {
            get => _projectItem;
        }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public string Identifier
        {
            get => ((Device)entity).Identifier;
            set => SetProperty(((Device)entity).Identifier, value, (Device)entity, (u, n) => u.Identifier = n, true);
        }

        public string OrderNumber
        {
            get => ((Device)entity).OrderNumber;
            set => SetProperty(((Device)entity).OrderNumber, value, (Device)entity, (u, n) => u.OrderNumber = n);
        }

        public DeviceType Type
        {
            get => ((Device)entity).Type;
            set => SetProperty(((Device)entity).Type, value, (Device)entity, (u, n) => u.Type = n);
        }

        public BusType BusType
        {
            get => ((Device)entity).BusType;
            set
            {
                SetProperty(((Device)entity).BusType, value, (Device)entity, (u, n) => u.BusType = n);
                OnPropertyChanged(nameof(TypeIcon));
            }
        }

        [KnxPhysicalAddress(ErrorMessage = "Ungültige KNX-Adresse.")]
        public string PhysicalAddress
        {
            get => string.Format("{0}.{1}.{2}", ((Device)entity).KnxTopologyArea, ((Device)entity).KnxTopologyLine, ((Device)entity).KnxTopologyAddress);
            set
            {
                // Must be explicitly validated here, since there is no 1-to-1 mapping with a property
                this.ValidateProperty(value, nameof(PhysicalAddress));

                if (!string.IsNullOrWhiteSpace(value))
                {
                    var parts = value.Split('.');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out int area) &&
                        int.TryParse(parts[1], out int line) &&
                        int.TryParse(parts[2], out int address))
                    {
                        var device = (Device)entity;

                        SetProperty(device.KnxTopologyArea, area, device, (d, v) => d.KnxTopologyArea = v);
                        SetProperty(device.KnxTopologyLine, line, device, (d, v) => d.KnxTopologyLine = v);
                        SetProperty(device.KnxTopologyAddress, address, device, (d, v) => d.KnxTopologyAddress = v);
                    }
                }
            }
        }

        public bool IsRailMounted
        {
            get => ((Device)entity).IsRailMounted;
            set => SetProperty(((Device)entity).IsRailMounted, value, (Device)entity, (u, n) => u.IsRailMounted = n);
        }

        public float DivisionUnits
        {
            get => ((Device)entity).DivisionUnits;
            set
            {
                SetProperty(((Device)entity).DivisionUnits, value, (Device)entity, (u, n) => u.DivisionUnits = n);
                OnPropertyChanged(nameof(TypeIcon));
            }
        }

        public string TypeIcon
        {
            get => this.BusType.GetEnumAttribute<BusTypeInfoAttribute>()?.Icon;
        }

        public string Location
        {
            get => ProjectItem.Parent.Name;
        }

        public List<DeviceType> DeviceTypes
        {
            get => Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>().ToList();
        }

        public ObservableManufacturer Manufacturer
        {
            get => ((Device)entity).Manufacturer != null ? new ObservableManufacturer(((Device)entity).Manufacturer) : null;
            set
            {
                if (value != null)
                {
                    SetProperty(((Device)entity).Manufacturer, value.entity, (Device)entity, (d, v) => d.Manufacturer = v);
                }
            }
        }

        #endregion

        #region --- Constructors ---

        public ObservableDevice()
            : this(new Device())
        { }

        public ObservableDevice(Device device)
            : base(device)
        {
            _projectItem = new ObservableProjectItem(device);
        }

        #endregion
    }
}

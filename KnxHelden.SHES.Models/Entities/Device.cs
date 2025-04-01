using KnxHelden.SHES.Models.Attributes;
using KnxHelden.SHES.Models.Enumerations;
using System;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Gerät", "\U000F0C9D")]
    public class Device : ProjectItem
    {
        public string Identifier { get; set; }

        public string OrderNumber { get; set; }

        public DeviceType Type { get; set; }

        public BusType BusType { get; set; }

        public int? KnxTopologyArea { get; set; }

        public int? KnxTopologyLine { get; set; }

        public int? KnxTopologyAddress { get; set; }

        public bool IsRailMounted { get; set; }

        public float DivisionUnits { get; set; }

        public Guid? ManufacturerId { get; set; }

        public Manufacturer Manufacturer { get; set; }
    }
}

using KnxHelden.SHES.Models.Attributes;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Busspannungsversorgung", "\U000F0C9D")]
    public class BusPowerSupply : Device
    {
        public ushort Power { get; set; }
    }
}

using KnxHelden.SHES.Models.Attributes;
using KnxHelden.SHES.Models.Enumerations;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Heizungsaktor", "\U000F0C9D")]
    public class HeatingActuator : Device
    {
        public int Channels { get; set; }

        public ChannelNames ChannelNames { get; set; }
    }
}

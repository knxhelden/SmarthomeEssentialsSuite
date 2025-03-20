using KnxHelden.SHES.Models.Attributes;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Raum", "\U000F081A")]
    [RestrictChildren(typeof(Cabinet), typeof(Device))]
    public class Room : ProjectItem
    {
    }
}

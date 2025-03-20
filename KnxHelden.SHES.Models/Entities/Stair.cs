using KnxHelden.SHES.Models.Attributes;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Treppe", "\U000F04CD")]
    [RestrictChildren(typeof(Cabinet), typeof(Device))]
    public class Stair : ProjectItem
    {
    }
}

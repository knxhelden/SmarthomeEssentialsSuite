using KnxHelden.SHES.Models.Attributes;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Gebäudeteil", "\U000F0991")]
    [RestrictChildren(typeof(BuildingPart), typeof(Floor), typeof(Corridor), typeof(Stair), typeof(Room))]
    public class BuildingPart : ProjectItem
    {
    }
}

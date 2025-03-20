using KnxHelden.SHES.Models.Attributes;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Etage", "\U000F0821")]
    [RestrictChildren(typeof(Corridor), typeof(Room))]
    public class Floor : ProjectItem
    {
    }
}

using KnxHelden.SHES.Models.Attributes;
using System;

namespace KnxHelden.SHES.Models.Entities
{
    [ProjectItemInfo("Gebäude", "\U000F01D7")]
    [RestrictChildren(typeof(BuildingPart), typeof(Floor), typeof(Corridor), typeof(Stair), typeof(Room))]

    public class Building : ProjectItem
    {
        public Guid? ProjectId { get; set; }

        public Project Project { get; set; }
    }
}

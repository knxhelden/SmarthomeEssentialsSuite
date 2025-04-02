using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace KnxHelden.SHES.Models.Entities
{
    public class Manufacturer : EntityBase
    {
        [Required]
        public string Name { get; set; }

        public virtual ICollection<Device> Devices { get; set; } = new Collection<Device>();

        public override string ToString()
        {
            return $"Manufacturer ({this.Id}): {this.Name}";
        }
    }
}

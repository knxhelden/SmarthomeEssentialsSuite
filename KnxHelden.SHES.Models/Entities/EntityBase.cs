using System;
using System.ComponentModel.DataAnnotations;

namespace KnxHelden.SHES.Models.Entities
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public EntityBase()
        {
            this.Id = Guid.NewGuid();
        }
    }
}

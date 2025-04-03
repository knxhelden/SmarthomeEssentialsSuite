using KnxHelden.SHES.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace KnxHelden.SHES.Models.Observables
{
    public class ObservableManufacturer : ObservableBase<Manufacturer>
    {
        #region --- Properties ---

        [Required]
        public string Name
        {
            get => entity?.Name;
            set => SetProperty(entity.Name, value, entity, (u, n) => u.Name = n, true);
        }

        #endregion

        #region --- Constructors ---

        public ObservableManufacturer()
            : this(new Manufacturer())
        { }

        public ObservableManufacturer(Manufacturer manufacturer)
            : base(manufacturer)
        { }

        #endregion
    }
}

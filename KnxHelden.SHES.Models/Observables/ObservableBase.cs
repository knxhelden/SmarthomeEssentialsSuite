using KnxHelden.SHES.Models.Entities;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KnxHelden.SHES.Models.Observables
{
    public abstract class ObservableBase<TEntity> : ObservableValidator where TEntity : EntityBase
    {
        public readonly TEntity entity;

        #region --- Properties ---

        public Guid Id
        {
            get => entity.Id;
            set => SetProperty(entity.Id, value, entity, (u, n) => u.Id = n);
        }

        public string Errors => string.Join(Environment.NewLine, from ValidationResult e in GetErrors(null) select e.ErrorMessage);

        #endregion

        #region --- Constructor ---

        public ObservableBase(TEntity entity)
        {
            this.entity = entity;

            this.PropertyChanged += ObservableBase_PropertyChanged;
            this.ErrorsChanged += ObservableBase_ErrorsChanged;

            this.ValidateAllProperties();
        }

        #endregion

        #region --- Events ---

        private void ObservableBase_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(HasErrors))
            {
                OnPropertyChanged(nameof(HasErrors));
            }
        }

        private void ObservableBase_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Errors));
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is null) return false; // Return false if obj is null
            if (ReferenceEquals(this, obj)) return true; // Return true if both references point to the same object
            if (obj.GetType() != GetType()) return false; // Return false if the types do not match

            var other = (ObservableBase<TEntity>)obj;
            return Id == other.Id; // Compare by Id
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

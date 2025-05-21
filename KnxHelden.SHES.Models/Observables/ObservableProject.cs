using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Enumerations;
using KnxHelden.SHES.Models.Strings;
using System.ComponentModel.DataAnnotations;

namespace KnxHelden.SHES.Models.Observables
{
    public class ObservableProject : ObservableBase<Project>
    {
        #region --- Properties ---

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [StringLength(50, MinimumLength = 3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLength))]
        public string Name
        {
            get => entity.Name;
            set => SetProperty(entity.Name, value, entity, (u, n) => u.Name = n, true);
        }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [StringLength(12, MinimumLength = 3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLength))]
        public string Number
        {
            get => entity.Number;
            set => SetProperty(entity.Number, value, entity, (u, n) => u.Number = n, true);
        }

        public string ClientFirstName
        {
            get => entity.ClientFirstName;
            set => SetProperty(entity.ClientFirstName, value, entity, (u, n) => u.ClientFirstName = n);
        }

        public string ClientSurname
        {
            get => entity.ClientSurname;
            set => SetProperty(entity.ClientSurname, value, entity, (u, n) => u.ClientSurname = n);
        }

        [RegularExpression(@"^(?:$|\+?[1-9]\d{1,14})$", ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Phone))]
        public string ClientPhone
        {
            get => entity.ClientPhone;
            set => SetProperty(entity.ClientPhone, value, entity, (u, n) => u.ClientPhone = n, true);
        }

        public string ClientEmail
        {
            get => entity.ClientEmail;
            set => SetProperty(entity.ClientEmail, value, entity, (u, n) => u.ClientEmail = n);
        }

        public string ConstructionStreet
        {
            get => entity.ConstructionStreet;
            set => SetProperty(entity.ConstructionStreet, value, entity, (u, n) => u.ConstructionStreet = n);
        }

        public string ConstructionPostalCode
        {
            get => entity.ConstructionPostalCode;
            set => SetProperty(entity.ConstructionPostalCode, value, entity, (u, n) => u.ConstructionPostalCode = n);
        }

        public string ConstructionCity
        {
            get => entity.ConstructionCity;
            set => SetProperty(entity.ConstructionCity, value, entity, (u, n) => u.ConstructionCity = n);
        }

        public ItemState State
        {
            get => entity.State;
            set => SetProperty(entity.State, value, entity, (u, n) => u.State = n);
        }

        #endregion

        #region --- Constructors ---

        public ObservableProject()
            : this(new Project())
        { }

        public ObservableProject(Project project)
            : base(project)
        { }

        #endregion
    }
}

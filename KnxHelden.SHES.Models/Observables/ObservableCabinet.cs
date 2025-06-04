using KnxHelden.SHES.Models.Entities;

namespace KnxHelden.SHES.Models.Observables
{
    public class ObservableCabinet : ObservableBase<Cabinet>
    {
        private readonly ObservableProjectItem _projectItem;

        #region --- Properties ---

        public ObservableProjectItem ProjectItem
        {
            get => _projectItem;
        }

        public string OrderNumber
        {
            get => entity.OrderNumber;
            set => SetProperty(entity.OrderNumber, value, entity, (u, n) => u.OrderNumber = n);
        }

        #endregion

        #region --- Constructors ---

        public ObservableCabinet()
            : this(new Cabinet())
        { }

        public ObservableCabinet(Cabinet cabinet)
            : base(cabinet)
        {
            _projectItem = new ObservableProjectItem(cabinet);
        }

        #endregion
    }
}

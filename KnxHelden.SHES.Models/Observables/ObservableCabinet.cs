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

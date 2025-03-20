using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Services.ProjectItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ComponentModels
{
    public sealed class ProjectItemMetadataComponentModel : ObservableRecipient
    {
        private readonly IProjectItemService _projectItemService;

        #region --- Properties ---

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    ObservableProjectItem previousItem = _currentProjectItem;
                    SetProperty(ref _currentProjectItem, value);

                }
            }
        }

        #endregion

        #region --- Constructor ---

        public ProjectItemMetadataComponentModel(IProjectItemService projectItemService)
        {
            _projectItemService = projectItemService;

            // Messages
            WeakReferenceMessenger.Default.Register<ProjectItemMetadataComponentModel, CurrentProjectItemSenderMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);
        }

        #endregion

        #region --- Events ---

        public async void InputField_LosingFocus(object sender, object e)
        {
            await this._projectItemService.UpdateAsync(this._currentProjectItem);
        }

        #endregion
    }
}

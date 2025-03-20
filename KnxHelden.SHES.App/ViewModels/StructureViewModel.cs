using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.ComponentModels;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.ProjectItems;
using KnxHelden.SHES.Shared.Extensions;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ViewModels
{
    public sealed class StructureViewModel : ObservableRecipient
    {
        #region --- Properties ---

        private ObservableProjectItem _currentProjectItem;
        public ObservableProjectItem CurrentProjectItem
        {
            get => _currentProjectItem;
            private set
            {
                if (value != null)
                {
                    SetProperty(ref _currentProjectItem, value);
                    this.TabsVisibility(value.entity.GetType());

                }
            }
        }

        private Visibility _locationTabVisibility = Visibility.Visible;
        public Visibility LocationTabVisibility
        {
            get => _locationTabVisibility;
            private set
            {
                SetProperty(ref _locationTabVisibility, value);
                OnPropertyChanged(nameof(LocationTabSelected));
            }
        }

        private Visibility _cabinetTabVisibility = Visibility.Collapsed;
        public Visibility CabinetTabVisibility
        {
            get => _cabinetTabVisibility;
            private set
            {
                SetProperty(ref _cabinetTabVisibility, value);
            }
        }

        private Visibility _detailsTabVisibility = Visibility.Collapsed;
        public Visibility DetailsTabVisibility
        {
            get => _detailsTabVisibility;
            private set
            {
                SetProperty(ref _detailsTabVisibility, value);
            }
        }

        public bool LocationTabSelected
        {
            get => _locationTabVisibility == Visibility.Visible;
        }

        #endregion

        #region --- Constructor ---

        public StructureViewModel()
        {
            // Messages
            WeakReferenceMessenger.Default.Register<StructureViewModel, CurrentProjectItemSenderMessage>(this, (r, m) => r.CurrentProjectItem = m.Value);
        }

        #endregion

        #region --- Methods ---

        private void TabsVisibility(Type locationType)
        {
            this.LocationTabVisibility = Visibility.Collapsed;
            this.CabinetTabVisibility = Visibility.Collapsed;
            this.DetailsTabVisibility = Visibility.Collapsed;

            switch (locationType.Name)
            {
                case nameof(Device):
                    this.DetailsTabVisibility = Visibility.Visible;
                    break;
                case nameof(Cabinet):
                    this.LocationTabVisibility = Visibility.Visible;
                    this.CabinetTabVisibility = Visibility.Visible;
                    break;
                default:
                    this.LocationTabVisibility = Visibility.Visible;
                    break;
            }
        }

        #endregion
    }
}

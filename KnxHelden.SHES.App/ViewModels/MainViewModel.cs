using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Enumerations;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Observables;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ViewModels
{
    public sealed class MainViewModel : ObservableRecipient
    {
        #region --- Properties ---

        private ObservableProject _currentProject;
        public ObservableProject CurrentProject
        {
            get => _currentProject;
            private set
            {
                SetProperty(ref _currentProject, value);
                OnPropertyChanged(nameof(IsCurrentProject));
            }
        }

        public bool IsCurrentProject => _currentProject != null;

        private AppInfoBarViewModel _applicationInfoBarOptions = new AppInfoBarViewModel();
        public AppInfoBarViewModel ApplicationInfoBarOptions
        {
            get => _applicationInfoBarOptions;
            private set
            {
                SetProperty(ref _applicationInfoBarOptions, value);
            }
        }

        private NavigationViewPaneDisplayMode _navigationViewPaneDisplayMode;
        public NavigationViewPaneDisplayMode NavigationViewPaneDisplayMode
        {
            get => _navigationViewPaneDisplayMode;
            set
            {
                SetProperty(ref _navigationViewPaneDisplayMode, value);
            }
        }

        #endregion

        #region --- Constructor ---

        public MainViewModel()
        {
            WeakReferenceMessenger.Default.Register<MainViewModel, AppBarSenderMessage>(this, (r, m) =>
            {
                r.ApplicationInfoBarOptions = m.Value;
                r.StartInfoBarTimer();
            });

            // Current Project Sender
            WeakReferenceMessenger.Default.Register<MainViewModel, CurrentProjectSenderMessage>(this, (r, m) => r.CurrentProject = m.Value);
            WeakReferenceMessenger.Default.Register<MainViewModel, NavigationViewPaneDisplayModeSenderMessage>(this, (r, m) => r.NavigationViewPaneDisplayMode = m.Value);

            // Current Project Receiver
            WeakReferenceMessenger.Default.Register<MainViewModel, CurrentProjectRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.CurrentProject);
            });
        }

        #endregion

        #region --- Methoden ---

        private async void StartInfoBarTimer()
        {
            if (ApplicationInfoBarOptions.IsOpen)
            {
                await Task.Delay(10000);
                ApplicationInfoBarOptions.IsOpen = false;
                OnPropertyChanged(nameof(ApplicationInfoBarOptions));
            }
        }

        #endregion
    }
}

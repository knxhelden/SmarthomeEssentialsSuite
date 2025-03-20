using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.WinUI;
using KnxHelden.SHES.App.Enumerations;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.App.Services.ThemeService;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;

namespace KnxHelden.SHES.App.ViewModels
{
    public sealed class SettingsViewModel : ObservableRecipient
    {
        private readonly IThemeService _themeService;

        #region --- Properties ---

        private Theme _selectedTheme;
        public Theme SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                SetProperty(ref _selectedTheme, value);
            }
        }

        private Navigation _selectedNavigation;
        public Navigation SelectedNavigation
        {
            get => _selectedNavigation;
            set
            {
                SetProperty(ref _selectedNavigation, value);
            }
        }

        public string AppName
        {
            get => "AppDisplayName".GetLocalized();
        }

        public string AppCopyright
        {
            get => "AppCopyright".GetLocalized();
        }

        public string AppVersion
        {
            get
            {
                var version = Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        #endregion

        #region --- Constructor ---

        public SettingsViewModel(IThemeService themeService)
        {
            _themeService = themeService;

            switch(themeService.Theme)
            {
                case ElementTheme.Dark:
                    _selectedTheme = Theme.Dark;
                    break;
                case ElementTheme.Light:
                    _selectedTheme = Theme.Light;
                    break;
                default:
                    _selectedTheme = Theme.System;
                    break;
            }
        }

        #endregion

        #region --- Events ---

        public async void Theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectedTheme)
            {
                case Theme.Dark:
                    await _themeService.SetThemeAsync(ElementTheme.Dark);
                    break;
                case Theme.Light:
                    await _themeService.SetThemeAsync(ElementTheme.Light);
                    break;
                default:
                    await _themeService.SetThemeAsync(ElementTheme.Default);
                    break;
            }
        }

        public void NavigationOrientation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectedNavigation)
            {
                case Navigation.Top:
                    WeakReferenceMessenger.Default.Send(new NavigationViewPaneDisplayModeSenderMessage(NavigationViewPaneDisplayMode.Top));
                    break;
                default:
                    WeakReferenceMessenger.Default.Send(new NavigationViewPaneDisplayModeSenderMessage(NavigationViewPaneDisplayMode.Left));
                    break;
            }
        }

        #endregion
    }
}

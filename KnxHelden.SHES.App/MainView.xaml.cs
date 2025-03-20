using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using KnxHelden.SHES.App.Views;
using Microsoft.Windows.ApplicationModel.Resources;
using Microsoft.Extensions.DependencyInjection;
using KnxHelden.SHES.App.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.App
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Window
    {
        private ResourceLoader _resourceLoader;

        public MainViewModel ViewModel { get; }

        public MainView()
        {
            ViewModel = App.Services.GetService<MainViewModel>();
            _resourceLoader = App.Services.GetService<ResourceLoader>();
            this.InitializeComponent();

            AppNavigation.SelectedItem = AppNavigation.MenuItems[0]; // Standardauswahl setzen
            contentFrame.Navigate(typeof(ProjectsView)); // Erste Seite beim Start laden
        }

        private void AppNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type? pageType = null;

            if (args.IsSettingsSelected) // Special handling for settings
            {
                pageType = typeof(SettingsView);
                AppNavigation.Header = _resourceLoader.GetString("AppNavigation_Settings");
            }
            else if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string pageName = selectedItem.Tag.ToString();
                AppNavigation.Header = selectedItem.Content?.ToString();

                pageType = pageName switch
                {
                    nameof(ProjectsView) => typeof(ProjectsView),
                    nameof(StructureView) => typeof(StructureView),
                    _ => null
                };
            }

            if (pageType != null)
            {
                contentFrame.Navigate(pageType);
            }
        }
    }
}

using CommunityToolkit.WinUI;
using KnxHelden.SHES.App.ComponentModels;
using KnxHelden.SHES.App.Services.ThemeService;
using KnxHelden.SHES.App.ViewModels;
using KnxHelden.SHES.Controls.FormFieldService;
using KnxHelden.SHES.Data;
using KnxHelden.SHES.Data.Repositories.Devices;
using KnxHelden.SHES.Data.Repositories.ProjectItems;
using KnxHelden.SHES.Data.Repositories.Projects;
using KnxHelden.SHES.Services.Devices;
using KnxHelden.SHES.Services.Knx;
using KnxHelden.SHES.Services.ProjectItems;
using KnxHelden.SHES.Services.Projects;
using KnxHelden.SHES.Services.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.IO;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KnxHelden.SHES.App
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        public static Window? MainWindow { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            Services = ConfigureServices();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainView() { Title = "AppDisplayName".GetLocalized() };
            MainWindow.Activate();

            // Load current theme from settings
            var themeService = App.Services.GetService<IThemeService>();
            await themeService.GetThemeAsync();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Logging
            services.AddLogging(logging =>
            {
                logging.AddDebug();
            });

            // Database
            services.AddDbContext<ShesDbContext>(options =>
            {
                // Create database directory
                string userDocumentPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
                string shesDatabasePath = System.IO.Path.Combine(userDocumentPath, "SHES");
                Directory.CreateDirectory(shesDatabasePath);

                options.UseSqlite($"Data Source={System.IO.Path.Combine(shesDatabasePath, "shes.db")};");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            // Repositories
            services.AddSingleton<IProjectRepository, ProjectRepository>();
            services.AddSingleton<IProjectItemRepository, ProjectItemRepository>();
            services.AddSingleton<IDeviceRepository, DeviceRepository>();

            // Services
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<IProjectItemService, ProjectItemService>();
            services.AddSingleton<IDeviceService, DeviceService>();
            services.AddSingleton<IKnxImportService, KnxImportService>();

            // View Models
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ProjectsViewModel>();
            services.AddTransient<MasterDataViewModel>();
            services.AddTransient<CompanyViewModel>();
            services.AddTransient<ManufacturerViewModel>();
            services.AddTransient<StructureViewModel>();
            services.AddTransient<ProjectTreeComponentModel>();
            services.AddTransient<ProjectItemDevicesComponentModel>();
            services.AddTransient<ProjectItemCabinetComponentModel>();
            services.AddTransient<ProjectItemDetailsComponentModel>();
            services.AddTransient<ProjectItemMetadataComponentModel>();
            services.AddTransient<ProjectItemMetadataComponentModel>();
            services.AddTransient<ProjectItemMetadataComponentModel>();


            services.AddSingleton<ResourceLoader>();
            services.AddSingleton<ISettingService, SettingServicePackaged>();
            services.AddSingleton<IThemeService, ThemeService>();
            services.AddSingleton<IFormFieldService, FormFieldService>();

            return services.BuildServiceProvider();
        }
    }
}

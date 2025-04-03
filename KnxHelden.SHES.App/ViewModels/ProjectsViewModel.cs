using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Knx;
using KnxHelden.SHES.Services.Projects;
using KnxHelden.SHES.Shared.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace KnxHelden.SHES.App.ViewModels
{
    public sealed class ProjectsViewModel : ObservableRecipient
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly IProjectService _projectService;
        private readonly IKnxImportService _knxImportService;

        #region --- Properties: Project List ---

        public ObservableCollection<ObservableProject> ProjectList { get; set; } = new ObservableCollection<ObservableProject>();

        private ObservableProject newProject = new ObservableProject(new Project());
        public ObservableProject NewProject
        {
            get => newProject;
            set => SetProperty(ref newProject, value);
        }

        private ObservableProject selectedProject;
        public ObservableProject SelectedProject
        {
            get => selectedProject;
            set
            {
                SetProperty(ref selectedProject, value);
                OnPropertyChanged(nameof(IsProjectSelected));
                OpenProjectCommand.NotifyCanExecuteChanged();
                DeleteProjectCommand.NotifyCanExecuteChanged();
            }
        }

        public bool IsProjectSelected => selectedProject != null;

        public IAsyncRelayCommand OpenAddProjectDialogCommand { get; }
        public IAsyncRelayCommand AddProjectCommand { get; }
        public IRelayCommand OpenProjectCommand { get; }
        public IAsyncRelayCommand DeleteProjectCommand { get; }
        

        #endregion

        #region --- Properties: Knx Import ---

        private StorageFile _knxProject;
        public StorageFile KnxProject
        {
            get => _knxProject;
            private set
            {
                SetProperty(ref _knxProject, value);
            }
        }

        private bool _isKnxProjectProtected;
        public bool IsKnxProjectProtected
        {
            get => _isKnxProjectProtected;
            private set
            {
                SetProperty(ref _isKnxProjectProtected, value);
                OnPropertyChanged(nameof(ImportProjectDialogHasErrors));
            }
        }

        private string _knxProjectPassword;
        public string KnxProjectPassword
        {
            get => _knxProjectPassword;
            set
            {
                SetProperty(ref _knxProjectPassword, value);
                OnPropertyChanged(nameof(ImportProjectDialogHasErrors));
            }
        }

        private bool _importKnxStructure = true;
        public bool ImportKnxStructure
        {
            get => _importKnxStructure;
            set => SetProperty(ref _importKnxStructure, value);
        }

        private bool _importKnxDevices = true;
        public bool ImportKnxDevices
        {
            get => _importKnxDevices;
            set => SetProperty(ref _importKnxDevices, value);
        }

        public IAsyncRelayCommand ImportKnxProjectDialogCommand { get; }
        public IAsyncRelayCommand ImportKnxProjectCommand { get; }

        public bool ImportProjectDialogHasErrors
        {
            get => IsKnxProjectProtected && string.IsNullOrEmpty(KnxProjectPassword);
        }

        #endregion

        #region --- Constructor ---

        public ProjectsViewModel(ResourceLoader resourceLoader, IProjectService projectService, IKnxImportService knxImportService)
        {
            // Services
            _resourceLoader = resourceLoader;
            _projectService = projectService;
            _knxImportService = knxImportService;

            // Commands
            OpenAddProjectDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await OpenAddProjectDialog(dialog));
            AddProjectCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await AddProject(dialog));
            OpenProjectCommand = new RelayCommand(OpenProject, CanOpenProject);
            DeleteProjectCommand = new AsyncRelayCommand(async (dialog) => await DeleteProject(), CanDeleteProject);
            ImportKnxProjectDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await this.ImportKnxProjectDialog(dialog));
            ImportKnxProjectCommand = new AsyncRelayCommand(ImportKnxProject);
        }

        #endregion

        #region --- Events ---

        public async void OnLoaded(object sender, RoutedEventArgs e)
        {
            ProjectList.AddRange(await _projectService.GetAllAsync());
        }

        public async void InputField_LostFocus(object sender, object e)
        {
            await _projectService.UpdateAsync(SelectedProject);
        }

        #endregion

        #region --- Commands ---

        private async Task OpenAddProjectDialog(ContentDialog dialog)
        {
            NewProject = new ObservableProject(new Project());
            await dialog.ShowAsync();
        }

        private async Task AddProject(ContentDialog dialog)
        {
            // Add project
            await _projectService.CreateAsync(NewProject);
            ProjectList.Add(NewProject);

            WeakReferenceMessenger.Default.Send(new AppBarSenderMessage(new AppInfoBarViewModel
            {
                IsOpen = true,
                Severity = InfoBarSeverity.Success,
                Title = _resourceLoader.GetString("MainView_AppBar_Success"),
                Message = _resourceLoader.GetString("ProjectView_AppBar_ProjectAdded")
            }));
        }

        private bool CanOpenProject()
        {
            return SelectedProject != null;
        }

        private void OpenProject()
        {
            // Set current project
            WeakReferenceMessenger.Default.Send(new CurrentProjectSenderMessage(this.SelectedProject));
        }

        private bool CanDeleteProject()
        {
            return selectedProject != null;
        }

        private async Task DeleteProject()
        {
            await _projectService.DeleteAsync(selectedProject);
            ProjectList.Remove(selectedProject);
            WeakReferenceMessenger.Default.Send(new CurrentProjectSenderMessage(null));

            WeakReferenceMessenger.Default.Send(new AppBarSenderMessage(new AppInfoBarViewModel
            {
                IsOpen = true,
                Severity = InfoBarSeverity.Success,
                Title = _resourceLoader.GetString("MainView_AppBar_Success"),
                Message = _resourceLoader.GetString("ProjectView_AppBar_ProjectDeleted")
            }));
        }

        private async Task ImportKnxProjectDialog(ContentDialog dialog)
        {
            var filePicker = new FileOpenPicker();

            // Get the current window's HWND by passing in the Window object
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            filePicker.ViewMode = PickerViewMode.List;
            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            filePicker.FileTypeFilter.Add(".knxproj");
            filePicker.CommitButtonText = _resourceLoader.GetString("ProjectView_KnxImport_FilePicker_ImportButtonText");
            KnxProject = await filePicker.PickSingleFileAsync();

            if (this.KnxProject != null)
            {
                this.IsKnxProjectProtected = await this._knxImportService.ProtectionCheckAsync(this.KnxProject.Path);
                await dialog.ShowAsync();
            }
        }

        private async Task ImportKnxProject()
        {
            var options = new KnxImportOptions {
                ImportStructure = ImportKnxStructure,
                ImportDevices = ImportKnxDevices
            };

            var result = await this._knxImportService.ImportProjectAsync(this.KnxProject.Path, options, KnxProjectPassword);

            if (result.Successful)
            {
                this.ProjectList.Add(result.Data.Project);
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new AppBarSenderMessage(new AppInfoBarViewModel
                {
                    IsOpen = true,
                    Severity = InfoBarSeverity.Error,
                    Title = this._resourceLoader.GetString("Shell_AppInfoBar_Error"),
                    Message = result.ErrorMessage
                }));
            }
        }

        #endregion
    }
}

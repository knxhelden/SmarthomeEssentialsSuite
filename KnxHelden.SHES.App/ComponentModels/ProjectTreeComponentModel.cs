using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using KnxHelden.SHES.App.Messages;
using KnxHelden.SHES.Models;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Extensions;
using KnxHelden.SHES.Models.Helpers;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.ProjectItems;
using KnxHelden.SHES.Shared.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.App.ComponentModels
{
    public sealed class ProjectTreeComponentModel : ObservableRecipient
    {
        private readonly IProjectItemService _projectItemService;

        #region --- Properties ---

        private ObservableProject _currentProject;
        public ObservableProject CurrentProject
        {
            get => _currentProject;
            private set
            {
                SetProperty(ref _currentProject, value);
            }
        }

        public ObservableCollection<ObservableProjectItem> ProjectItems { get; } = new ObservableCollection<ObservableProjectItem>();

        public ObservableCollection<ProjectItemTypeInfo> RestrictedProjectItemInfos { get; } = new ObservableCollection<ProjectItemTypeInfo>();

        private ObservableProjectItem _selectedProjectItem;
        public ObservableProjectItem SelectedProjectItem
        {
            get => _selectedProjectItem;
            set
            {
                SetProperty(ref _selectedProjectItem, value);
                this.UpdateProjectItemTypes();

                AddProjectItemDialogCommand.NotifyCanExecuteChanged();
                DeleteProjectItemCommand.NotifyCanExecuteChanged();

                // Set current project item
                WeakReferenceMessenger.Default.Send(new CurrentProjectItemSenderMessage(value));
            }
        }

        private ProjectItemTypeInfo _newProjectItemType;
        public ProjectItemTypeInfo NewProjectItemType
        {
            get => _newProjectItemType;
            set
            {
                SetProperty(ref _newProjectItemType, value);
                OnPropertyChanged(nameof(this.NewProjectItemDialogHasErrors));
            }
        }

        private string _newProjectItemName;
        public string NewProjectItemName
        {
            get => _newProjectItemName;
            set
            {
                SetProperty(ref _newProjectItemName, value);
                OnPropertyChanged(nameof(this.NewProjectItemDialogHasErrors));
            }
        }

        public bool NewProjectItemDialogHasErrors
        {
            get => this.NewProjectItemType == null || string.IsNullOrEmpty(this.NewProjectItemName);
        }

        private bool _isTreeLoading = true;
        public bool IsTreeLoading
        {
            get => _isTreeLoading;
            set => SetProperty(ref _isTreeLoading, value);
        }

        public IAsyncRelayCommand AddProjectItemDialogCommand { get; }
        public IAsyncRelayCommand AddProjectItemCommand { get; }
        public IAsyncRelayCommand DeleteProjectItemCommand { get; }

        #endregion

        #region --- Constructor ---

        public ProjectTreeComponentModel(IProjectItemService projectItemService)
        {
            this._projectItemService = projectItemService;

            // Commands
            AddProjectItemDialogCommand = new AsyncRelayCommand<ContentDialog>(async (dialog) => await AddProjectItemDialog(dialog), CanAddProjectItemDialog);
            AddProjectItemCommand = new AsyncRelayCommand(AddProjectItem);
            DeleteProjectItemCommand = new AsyncRelayCommand(async () => await DeleteProjectItem(), CanDeleteProjectItem);

            // Messages
            CurrentProject = WeakReferenceMessenger.Default.Send<CurrentProjectRequestMessage>();
        }

        #endregion

        #region --- Events ---

        public async void OnLoaded(object sender, RoutedEventArgs e)
        {
            var projectItems = await this._projectItemService.GetItemsForProjectAsync(this.CurrentProject, true);
            this.ProjectItems.AddRange(projectItems);
            this.SelectedProjectItem = projectItems.FirstOrDefault();
            this.IsTreeLoading = false;
        }

        #endregion

        #region --- Commands ---

        /// <summary>Opens the dialog to add a project item.</summary>
        /// <param name="dialog">The dialog.</param>
        private async Task AddProjectItemDialog(ContentDialog dialog)
        {
            await dialog.ShowAsync();
        }

        private bool CanAddProjectItemDialog(ContentDialog dialog)
        {
            return this.RestrictedProjectItemInfos.Count > 0;
        }

        /// <summary>
        /// Adds a new project item as child of the selected project item.
        /// </summary>
        private async Task AddProjectItem()
        {
            ObservableProjectItem item = new ObservableProjectItem(ReflectionHelper.GetInstance<ProjectItem>(this.NewProjectItemType.FullName));
            item.entity.ParentId = this.SelectedProjectItem.Id;
            item.Name = this.NewProjectItemName;

            // Insert new project item
            await this._projectItemService.CreateAsync(item);

            // Loads the item again from the database to ensure that all properties are filled correctly
            item = await this._projectItemService.GetByIdAsync(item.Id);

            // Add new project item to project tree
            this.SelectedProjectItem.Children.Add(item);

            // Causes the DataGrid to be reloaded with the new item
            WeakReferenceMessenger.Default.Send(new CurrentProjectItemSenderMessage(this.SelectedProjectItem));

            // Clear form
            this.NewProjectItemType = null;
            this.NewProjectItemName = string.Empty;
        }

        /// <summary>
        /// Deletes the selected project item.
        /// </summary>
        private async Task DeleteProjectItem()
        {
            ObservableProjectItem parent = this.ProjectItems.Traverse(pi => pi.Children)
                .FirstOrDefault(pi => pi.Id == this.SelectedProjectItem.Parent.Id);

            await this._projectItemService.DeleteAsync(this.SelectedProjectItem);
            parent.Children.Remove(this.SelectedProjectItem);
            this.SelectedProjectItem = parent;
        }

        private bool CanDeleteProjectItem()
        {
            return this.SelectedProjectItem != null && this.SelectedProjectItem.Id != this.ProjectItems.First().Id;
        }

        #endregion

        private void UpdateProjectItemTypes()
        {
            this.RestrictedProjectItemInfos.Clear();

            if (this.SelectedProjectItem != null)
            {
                this.RestrictedProjectItemInfos.AddRange(this.SelectedProjectItem?.entity.GetRestrictChildrenInfos());
            }
        }
    }
}

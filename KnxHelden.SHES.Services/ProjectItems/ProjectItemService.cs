using KnxHelden.SHES.Data.Repositories;
using KnxHelden.SHES.Data.Repositories.ProjectItems;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.ProjectItems
{
    /// <summary>
    /// Service class for managing project items, providing CRUD operations and project-specific queries.
    /// </summary>
    public class ProjectItemService : EntityServiceBase<ObservableProjectItem, ProjectItem>, IProjectItemService
    {
        private readonly IProjectItemRepository _projectItemRepository;

        #region --- Constructor ---

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectItemService"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader used for localization and resource access.</param>
        /// <param name="logger">The logger instance used for logging diagnostic messages.</param>
        /// <param name="repository">The generic repository instance for project item entities.</param>
        /// <param name="projectItemRepository">The specific project item repository for advanced queries.</param>
        public ProjectItemService(ResourceLoader resourceLoader, ILogger<ProjectItemService> logger, IRepository<ProjectItem> repository, IProjectItemRepository projectItemRepository)
            : base(resourceLoader, logger, repository)
        {
            this._projectItemRepository = projectItemRepository;
        }

        #endregion

        #region --- IProjectItemService ---

        /// <summary>
        /// Creates multiple project items asynchronously.
        /// </summary>
        /// <param name="observableProjectItems">A collection of observable project items to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateManyAsync(ObservableCollection<ObservableProjectItem> observableProjectItems)
        {
            // Insert new project items
            foreach(var item in observableProjectItems)
            {
                await this._projectItemRepository.CreateAsync(item.entity);
            }
        }

        /// <summary>
        /// Updates multiple project items asynchronously.
        /// </summary>
        /// <param name="observableProjectItems">A collection of observable project items to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateManyAsync(ObservableCollection<ObservableProjectItem> observableProjectItems)
        {
            // Update project items
            foreach(var item in observableProjectItems)
            {
                await this._projectItemRepository.UpdateAsync(item.entity);
            }
        }

        /// <summary>
        /// Retrieves all project items associated with a specific project.
        /// </summary>
        /// <param name="observableProject">The observable project whose items should be retrieved.</param>
        /// <param name="includeDevices">Optional flag indicating whether to include associated devices in the result.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is an observable collection of project items,
        /// or <c>null</c> if an error occurs.
        /// </returns>
        public async Task<ObservableCollection<ObservableProjectItem>> GetItemsForProjectAsync(ObservableProject observableProject, bool includeDevices = false)
        {
            try
            {
                var projectItemTree = await this._projectItemRepository.GetProjectItemTreeAsync(observableProject.Id, includeDevices);
                return new ObservableCollection<ObservableProjectItem>(projectItemTree.Select(pi => new ObservableProjectItem(pi)));
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}

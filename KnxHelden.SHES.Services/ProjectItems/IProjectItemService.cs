using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.ProjectItems
{
    public interface IProjectItemService : IEntityService<ObservableProjectItem, ProjectItem>
    {
        /// <summary>
        /// Creates multiple project items asynchronously.
        /// </summary>
        /// <param name="observableProjectItems">A collection of observable project items to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateManyAsync(ObservableCollection<ObservableProjectItem> observableProjectItems);

        /// <summary>
        /// Updates multiple project items asynchronously.
        /// </summary>
        /// <param name="observableProjectItems">A collection of observable project items to update.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateManyAsync(ObservableCollection<ObservableProjectItem> observableProjectItems);

        /// <summary>
        /// Retrieves all project items associated with a specific project.
        /// </summary>
        /// <param name="observableProject">The observable project whose items should be retrieved.</param>
        /// <param name="includeDevices">Optional flag indicating whether to include associated devices in the result.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result is an observable collection of project items,
        /// or <c>null</c> if an error occurs.
        /// </returns>
        Task<ObservableCollection<ObservableProjectItem>> GetItemsForProjectAsync(ObservableProject observableProject, bool includeDevices = false);
    }
}
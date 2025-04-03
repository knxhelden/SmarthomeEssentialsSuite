using KnxHelden.SHES.Models.Observables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Projects
{
    public interface IProjectService : IService
    {
        /// <summary>
        /// Checks if a project with the specified name exists.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <returns>True if the project exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(string name);

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A collection of all projects.</returns>
        Task<ObservableCollection<ObservableProject>> GetAllAsync();

        /// <summary>
        /// Adds a new project.
        /// </summary>
        /// <param name="observableProject">The project to be added.</param>
        /// <returns>The newly created project with an auto-generated ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the project name is null or empty.</exception>
        /// <exception cref="EntityAlreadyExistsException">Thrown when a project with the same name already exists.</exception>
        Task<ObservableProject> AddAsync(ObservableProject observableProject);

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="observableProject">The project to be updated.</param>
        /// <returns>The updated project if successful; otherwise, null.</returns>
        Task<ObservableProject> UpdateAsync(ObservableProject observableProject);

        /// <summary>
        /// Deletes an existing project.
        /// </summary>
        /// <param name="observableProject">The project to be deleted.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        Task<bool> DeleteAsync(ObservableProject observableProject);
    }
}
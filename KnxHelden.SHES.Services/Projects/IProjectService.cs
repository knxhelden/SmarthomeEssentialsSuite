using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Projects
{
    public interface IProjectService : IEntityService<ObservableProject, Project>
    {
        /// <summary>
        /// Checks if a project with the specified name exists.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <returns>True if the project exists; otherwise, false.</returns>
        Task<bool> ExistsAsync(string name);
    }
}
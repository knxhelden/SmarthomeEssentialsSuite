using KnxHelden.SHES.Data.Repositories;
using KnxHelden.SHES.Data.Repositories.Projects;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Projects
{
    /// <summary>
    /// Service class for managing projects.
    /// </summary>
    public class ProjectService : EntityServiceBase<ObservableProject, Project>, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        #region --- Constructors ---

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader for localization.</param>
        /// <param name="logger">The logger for logging messages.</param>
        /// <param name="projectRepository">The project repository for data access.</param>
        public ProjectService(ResourceLoader resourceLoader, ILogger<ProjectService> logger, IRepository<Project> repository, IProjectRepository projectRepository)
            : base(resourceLoader, logger, repository)
        {
            this._projectRepository = projectRepository;
        }

        #endregion

        /// <summary>
        /// Creates a new project entity with additional validation and initialization.
        /// </summary>
        /// <param name="observableProject">The observable project model containing the project data.</param>
        /// <returns>
        /// A task representing the asynchronous operation. The result indicates success (<c>true</c>) or failure (<c>false</c>).
        /// </returns>
        public new async Task<bool> CreateAsync(ObservableProject observableProject)
        {
            if (string.IsNullOrWhiteSpace(observableProject.Name))
            {
                throw new ArgumentNullException();
            }

            var project = new Project
            {
                Name = observableProject.Name,
                Number = observableProject.Number,
            };

            try
            {
                // Check if project with same name exists
                if (await this._projectRepository.ExistsAsync(project))
                {
                    throw new EntityAlreadyExistsException($"A project with name '{observableProject.Name}' already exists.");
                }

                // Add default building
                project.Buildings = new List<Building>(new[]
                {
                    new Building { Name = observableProject.Name }
                });

                // Insert new project
                await this._projectRepository.CreateAsync(project);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if a project with the specified name exists.
        /// </summary>
        /// <param name="name">The name of the project.</param>
        /// <returns>True if the project exists; otherwise, false.</returns>
        public async Task<bool> ExistsAsync(string name)
        {
            return await this._projectRepository.ExistsAsync(name);
        }
    }
}

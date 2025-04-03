using KnxHelden.SHES.Data.Repositories.Projects;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Services.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Projects
{
    /// <summary>
    /// Service class for managing projects.
    /// </summary>
    public class ProjectService : ServiceBase, IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        #region --- Constructors ---

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectService"/> class.
        /// </summary>
        /// <param name="resourceLoader">The resource loader for localization.</param>
        /// <param name="logger">The logger for logging messages.</param>
        /// <param name="projectRepository">The project repository for data access.</param>
        public ProjectService(ResourceLoader resourceLoader, ILogger<ProjectService> logger, IProjectRepository projectRepository)
            : base(resourceLoader, logger)
        {
            this._projectRepository = projectRepository;
        }

        #endregion

        /// <summary>
        /// Retrieves all projects.
        /// </summary>
        /// <returns>A collection of all projects.</returns>
        public async Task<ObservableCollection<ObservableProject>> GetAllAsync()
        {
            try
            {
                var projects = await this._projectRepository.GetAllAsync();
                return new ObservableCollection<ObservableProject>(projects.Select(p => new ObservableProject(p)));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Adds a new project.
        /// </summary>
        /// <param name="observableProject">The project to be added.</param>
        /// <returns>The newly created project with an auto-generated ID.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the project name is null or empty.</exception>
        /// <exception cref="EntityAlreadyExistsException">Thrown when a project with the same name already exists.</exception>
        public async Task<ObservableProject> AddAsync(ObservableProject observableProject)
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
                await this._projectRepository.AddAsync(project);
                return new ObservableProject(project);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Updates an existing project.
        /// </summary>
        /// <param name="observableProject">The project to be updated.</param>
        /// <returns>The updated project if successful; otherwise, null.</returns>
        public async Task<ObservableProject> UpdateAsync(ObservableProject observableProject)
        {
            try
            {
                // Update new project
                await this._projectRepository.UpdateAsync(observableProject.entity);
                return observableProject;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Deletes an existing project.
        /// </summary>
        /// <param name="observableProject">The project to be deleted.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public async Task<bool> DeleteAsync(ObservableProject observableProject)
        {
            try
            {
                await this._projectRepository.DeleteAsync(observableProject.entity);
            }
            catch
            {
                return false;
            }

            return true;
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

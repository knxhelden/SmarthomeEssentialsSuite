using KnxHelden.SHES.Data.Repositories.Projects;
using KnxHelden.SHES.Models.Observables;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.ApplicationModel.Resources;

namespace KnxHelden.SHES.Services.Projects
{
    public class ProjectServiceMock : ServiceBase, IProjectService
    {
        public ProjectServiceMock(ResourceLoader resourceLoader, ILogger<ProjectService> logger)
            : base(resourceLoader, logger)
        {
        }

        public Task<bool> DeleteAsync(ObservableProject observableProject)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<ObservableProject>> GetAllAsync()
        {
            var projects = new ObservableCollection<ObservableProject>
            {
                new ObservableProject { Name = "Projekt 1" },
                new ObservableProject { Name = "Projekt 2" },
                new ObservableProject { Name = "Projekt 3" },
                new ObservableProject { Name = "Projekt 4" },
                new ObservableProject { Name = "Projekt 5" }
            };

            return Task.FromResult(projects);
        }

        public Task<ObservableProject> InsertAsync(ObservableProject observableProject)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableProject> UpdateAsync(ObservableProject observableProject)
        {
            throw new NotImplementedException();
        }
    }
}

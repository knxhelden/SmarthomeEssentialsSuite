using System.Threading.Tasks;

namespace KnxHelden.SHES.Services.Knx
{
    public interface IKnxImportService : IService
    {
        /// <summary>
        /// Imports a KNX project file asynchronously.
        /// </summary>
        /// <param name="path">Path to the KNX project file.</param>
        /// <param name="options">Import options for processing the project.</param>
        /// <param name="password">Optional password for encrypted project files.</param>
        /// <returns>Result of the import operation containing project data.</returns>
        Task<KnxImportResult> ImportProjectAsync(string path, KnxImportOptions options, string password = "");

        /// <summary>
        /// Checks asynchronously if the KNX project file is password-protected.
        /// </summary>
        /// <param name="path">Path to the KNX project file.</param>
        /// <returns>True if the project is password-protected, otherwise false.</returns>
        Task<bool> ProtectionCheckAsync(string path);
    }
}
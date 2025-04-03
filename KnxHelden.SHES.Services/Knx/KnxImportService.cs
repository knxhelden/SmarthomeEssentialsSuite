using ICSharpCode.SharpZipLib.Zip;
using KnxHelden.SHES.Data.Repositories.Projects;
using KnxHelden.SHES.Models.Entities;
using KnxHelden.SHES.Models.Enumerations;
using KnxHelden.SHES.Models.Observables;
using KnxHelden.SHES.Shared.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KnxHelden.SHES.Services.Knx
{
    /// <summary>
    /// Service for importing ETS (KNX) project files.
    /// </summary>
    public class KnxImportService : ServiceBase, IKnxImportService
    {
        #region --- Fields ---

        private KnxImportOptions _options;
        private KnxImportResult _result;
        private Project _project = new();
        private List<ProjectItem> _projectItems;

        private int _schemaVersion;
        private string _schemaNamespace => $"http://knx.org/xml/project/{_schemaVersion}";
        private XElement _projectXml;
        private XElement _topologyXml;
        private XElement _tradesXml;
        private XElement _locationsXml;

        private readonly IProjectRepository _projectRepository;

        /// <summary>
        /// Dictionary for mapping ETS project types to SHES project types.
        /// </summary>
        private static readonly Dictionary<string, string> projectTypeMapping = new()
        {
            { "Building", "Building" },
            { "", "BuildingPart" },
            { "Floor", "Floor" },
            { "Room", "Room" },
            { "Corridor", "Corridor" },
            { "DistributionBoard", "Cabinet" }
        };

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="KnxImportService"/> class.
        /// </summary>
        /// <param name="resourceLoader">Resource loader for localized strings.</param>
        /// <param name="logger">Logger for logging events.</param>
        /// <param name="projectRepository">Repository for accessing project data.</param>
        public KnxImportService(ResourceLoader resourceLoader, ILogger<KnxImportService> logger, IProjectRepository projectRepository)
            : base(resourceLoader, logger) => _projectRepository = projectRepository;

        #region --- IKnxImportService ---

        /// <summary>
        /// Checks asynchronously if the KNX project file is password-protected.
        /// </summary>
        /// <param name="path">Path to the KNX project file.</param>
        /// <returns>True if the project is password-protected, otherwise false.</returns>
        public async Task<bool> ProtectionCheckAsync(string path) => await Task.Run(() =>
        {
            using var file = File.OpenRead(path);
            using var zip = new ZipFile(file);
            return zip.Cast<ZipEntry>().Any(entry => Regex.IsMatch(entry.Name, @"P-\w{4}.zip$"));
        });

        /// <summary>
        /// Imports a KNX project file asynchronously.
        /// </summary>
        /// <param name="path">Path to the KNX project file.</param>
        /// <param name="options">Import options for processing the project.</param>
        /// <param name="password">Optional password for encrypted project files.</param>
        /// <returns>Result of the import operation containing project data.</returns>
        public async Task<KnxImportResult> ImportProjectAsync(string path, KnxImportOptions options, string password = "")
        {
            this._options = options;
            this._result = new KnxImportResult();

            // Import project file
            using var file = File.OpenRead(path);
            await this.UnzipAsync(file, password);

            if (this._result.Successful)
            {
                // Add new project
                if (!await this._projectRepository.ExistsAsync(this._project.Name))
                {
                    await this._projectRepository.CreateAsync(this._project);
                    this._result.Data.Project = new ObservableProject(this._project);
                }
                else
                {
                    this._result.ErrorMessage = $"A project with name '{this._result.Data.Project.Name}' already exists. Please delete the existing project in SHES.";
                    this._result.Successful = false;
                }
            }

            return this._result;
        }

        #endregion

        /// <summary>
        /// Extracts and processes files from an ETS project archive.
        /// </summary>
        /// <param name="file">Stream representing the ETS project archive.</param>
        /// <param name="password">Password for encrypted project files.</param>
        private async Task UnzipAsync(Stream file, string password)
        {
            try
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Position = 0;

                using var zip = new ZipFile(ms);
                zip.Password = password;

                // ETS Schema Version
                foreach (ZipEntry zipEntry in zip)
                {
                    if (zipEntry.Name == "knx_master.xml" && zipEntry.IsFile)
                    {
                        await this.ReadEtsSchemaVersionAsync(zipEntry, zip);
                    }
                }

                // Products
                foreach (ZipEntry zipEntry in zip)
                {
                    // Product Catalogs
                    if (Regex.IsMatch(zipEntry.Name, @"^M-\w{4}/Hardware.xml") && zipEntry.IsFile)
                    {
                        await this.ReadHardwareAsync(zipEntry, zip);
                    }
                }

                // Project
                foreach (ZipEntry zipEntry in zip)
                {
                    // Password protected project
                    if (Regex.IsMatch(zipEntry.Name, @"P-\w{4}.zip$") && zipEntry.IsFile)
                    {
                        if (this._schemaVersion >= 21)
                        {
                            await this.UnzipAsync(zip.GetInputStream(zipEntry), this.EncryptEtsPassword(password));
                        }
                        else
                        {
                            await this.UnzipAsync(zip.GetInputStream(zipEntry), password);
                        }
                    }

                    // Project XML
                    if ((Regex.IsMatch(zipEntry.Name, @"^project.xml$") || Regex.IsMatch(zipEntry.Name, @"^P-\w{4}/project.xml$")) && zipEntry.IsFile)
                    {
                        await this.ReadProjectAsync(zipEntry, zip);
                    }

                    // Structure XML
                    if ((Regex.IsMatch(zipEntry.Name, @"^0.xml$") || Regex.IsMatch(zipEntry.Name, @"^P-\w{4}/0.xml$")) && zipEntry.IsFile)
                    {
                        await this.ReadStructureFileAsync(zipEntry, zip);
                    }
                }
            }
            catch (ZipException)
            {
                this._result.Successful = false;
                this._result.ErrorMessage = this._resourceLoader.GetString("Main_ProjectList_EtsImport_PasswordError");
            }
            catch (Exception ex)
            {
                this._result.Successful = false;
                this._result.ErrorMessage = ex.Message;
            }
        }

        /// <summary>
        /// Reads the ETS XML schema version from the master file.
        /// </summary>
        /// <param name="zipEntry">The zip entry containing the schema file.</param>
        /// <param name="zip">Reference to the ZIP archive.</param>
        private async Task ReadEtsSchemaVersionAsync(ZipEntry zipEntry, ZipFile zip)
        {
            await Task.Run(() =>
            {
                var stream = zip.GetInputStream(zipEntry);
                var ns = XElement.Load(stream).GetDefaultNamespace().NamespaceName;
                this._schemaVersion = Convert.ToInt32(ns.Split('/').Last());
            });
        }

        /// <summary>
        /// Asynchronously reads the hardware information (Hardware.xml) from the KNX project archive and extracts product details.
        /// </summary>
        /// <param name="zipEntry">The zip entry corresponding to the hardware file (Hardware.xml).</param>
        /// <param name="zip">The zip file containing the hardware file.</param>
        private async Task ReadHardwareAsync(ZipEntry zipEntry, ZipFile zip)
        {
            var stream = zip.GetInputStream(zipEntry);
            StreamReader reader = new(stream);
            XDocument doc = XDocument.Parse(await reader.ReadToEndAsync());

            var element = doc.Descendants(XName.Get("Hardware", _schemaNamespace)).FirstOrDefault();

            foreach (XElement hardware in element.Elements(XName.Get("Hardware", _schemaNamespace)))
            {
                KnxProduct product = new KnxProduct
                {
                    Name = hardware.Attribute(XName.Get("Name")).Value
                };

                var details = hardware.Element(XName.Get("Products", _schemaNamespace))
                    .Element(XName.Get("Product", _schemaNamespace));

                product.Id = details.Attribute(XName.Get("Id")).Value;
                product.OrderNumber = details.Attribute(XName.Get("OrderNumber")).Value;

                this._result.Data.Products.Add(product);
            }
        }

        /// <summary>
        /// Asynchronously reads the project information (project.xml) from the KNX project archive and extracts key project details.
        /// </summary>
        /// <param name="zipEntry">The zip entry corresponding to the project file (project.xml).</param>
        /// <param name="zip">The zip file containing the project file.</param>
        private async Task ReadProjectAsync(ZipEntry zipEntry, ZipFile zip)
        {
            using var stream = zip.GetInputStream(zipEntry);
            using var reader = new StreamReader(stream);

            XDocument doc = XDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));
            this._projectXml = doc.Element(XName.Get("KNX", _schemaNamespace));

            var projectElement = this._projectXml?.Element(XName.Get("Project", _schemaNamespace));
            var projectInfo = projectElement?.Element(XName.Get("ProjectInformation", _schemaNamespace));

            this._project.Number = projectElement?.Attribute(XName.Get("Id"))?.Value ?? "UNKNOWN";
            this._project.Name = projectInfo?.Attribute(XName.Get("Name"))?.Value ?? "UNKNOWN";
        }

        /// <summary>
        /// Asynchronously reads the structure file (0.xml) from the KNX project archive and parses its content into XML elements.
        /// </summary>
        /// <param name="zipEntry">The zip entry corresponding to the structure file (0.xml).</param>
        /// <param name="zip">The zip file containing the structure file.</param>
        private async Task ReadStructureFileAsync(ZipEntry zipEntry, ZipFile zip)
        {
            using var stream = zip.GetInputStream(zipEntry);
            using var reader = new StreamReader(stream);

            var xml = XDocument.Parse(await reader.ReadToEndAsync().ConfigureAwait(false));

            // Verwende FirstOrDefault() direkt für eindeutige Elemente
            this._topologyXml = xml.Descendants(XName.Get("Topology", _schemaNamespace)).FirstOrDefault();
            this._locationsXml = xml.Descendants(XName.Get("Locations", _schemaNamespace)).FirstOrDefault();
            this._tradesXml = xml.Descendants(XName.Get("Trades", _schemaNamespace)).FirstOrDefault();

            // Project Items nur initialisieren, wenn Import aktiviert ist
            if (this._options.ImportStructure && this._locationsXml != null)
            {
                var spaces = this._locationsXml.Elements(XName.Get("Space", _schemaNamespace)) ?? Enumerable.Empty<XElement>();
                this._projectItems = this.ReadProjectItems(spaces);

                this._project.Buildings.AddRange(this._projectItems.Cast<Building>());
            }
        }



        /// <summary>
        /// Parses all project items (e.g., rooms, floors, buildings) from the provided collection of space elements and returns a hierarchical list of <see cref="ProjectItem"/> objects.
        /// </summary>
        /// <param name="spaces">A collection of XML elements representing spaces in the KNX project.</param>
        /// <returns>A list of parsed <see cref="ProjectItem"/> objects representing spaces and their hierarchical structure.</returns>
        private List<ProjectItem> ReadProjectItems(IEnumerable<XElement> spaces)
        {
            return spaces.Select(space =>
            {
                if (!projectTypeMapping.TryGetValue(space.Attribute("Type")?.Value, out string mappedType) || string.IsNullOrEmpty(mappedType))
                {
                    return null; // Falls der Typ nicht gemappt werden kann, ignorieren
                }

                Assembly assembly = typeof(EntityBase).Assembly;
                Type type = assembly.GetType($"KnxHelden.SHES.Models.Entities.{mappedType}");
                if (type == null)
                {
                    return null; // Falls der Typ nicht existiert, ignorieren
                }

                ProjectItem entity = (ProjectItem)Activator.CreateInstance(type);
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .ToDictionary(p => p.Name); // Dictionary für schnelleren Zugriff

                // Name setzen
                properties["Name"].SetValue(entity, space.Attribute("Name")?.Value ?? "UNKNOWN");

                // Kinder (rekursive Räume & Geräte)
                var projectItems = ReadProjectItems(space.Elements(XName.Get("Space", _schemaNamespace)));
                if (this._options.ImportDevices)
                {
                    projectItems.AddRange(ReadDevices(space.Elements(XName.Get("DeviceInstanceRef", _schemaNamespace))));
                }
                properties["Children"].SetValue(entity, projectItems);

                return entity;
            })
            .Where(e => e != null)
            .ToList();
        }


        /// <summary>
        /// Parses all device instances within a given space (container) and returns a list of device objects.
        /// </summary>
        /// <param name="deviceInstanceReferences">A collection of XML elements referencing device instances.</param>
        /// <returns>A list of parsed <see cref="Device"/> objects.</returns>
        private List<Device> ReadDevices(IEnumerable<XElement> deviceInstanceReferences)
        {
            return deviceInstanceReferences.Select(deviceInstanceReference =>
            {
                var refId = deviceInstanceReference.Attribute("RefId")?.Value;
                var deviceInstance = _topologyXml.Descendants(XName.Get("DeviceInstance", _schemaNamespace))
                                                 .FirstOrDefault(e => e.Attribute("Id")?.Value == refId);
                if (deviceInstance == null)
                    return null;

                var productRefId = deviceInstance.Attribute("ProductRefId")?.Value;
                var product = _result.Data.Products.FirstOrDefault(p => p.Id == productRefId);

                return new Device
                {
                    BusType = BusType.Knx,
                    Name = string.IsNullOrEmpty(deviceInstance.Attribute("Name")?.Value) ? product?.Name : deviceInstance.Attribute("Name")?.Value,
                    Comment = deviceInstance.Attribute("Comment")?.Value,
                    Description = deviceInstance.Attribute("Description")?.Value,
                    KnxTopologyArea = TryParseInt(deviceInstance.Parent?.Parent?.Parent?.Attribute("Address")?.Value),
                    KnxTopologyLine = TryParseInt(deviceInstance.Parent?.Parent?.Attribute("Address")?.Value),
                    KnxTopologyAddress = TryParseInt(deviceInstance.Attribute("Address")?.Value)
                };
            }).Where(device => device != null).ToList();
        }

        /// <summary>
        /// Encrypts a password required for unpacking ETS6 project files.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <returns>Encrypted password.</returns>
        private string EncryptEtsPassword(string password)
        {
            int iterations = 65536;
            byte[] salt = Encoding.ASCII.GetBytes("21.project.ets.knx.org");

            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] bytes = deriveBytes.GetBytes(32);
                return Convert.ToBase64String(bytes);
            }
        }

        private int? TryParseInt(string value)
        {
            return int.TryParse(value, out var result) ? result : null;
        }
    }
}

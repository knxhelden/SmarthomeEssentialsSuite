using KnxHelden.SHES.Models.Entities;
using Microsoft.Extensions.Logging;

namespace KnxHelden.SHES.Data.Repositories.Manufacturers
{
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="ManufacturerRepository" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public ManufacturerRepository(ILogger<ManufacturerRepository> logger, ShesDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        #endregion
    }
}

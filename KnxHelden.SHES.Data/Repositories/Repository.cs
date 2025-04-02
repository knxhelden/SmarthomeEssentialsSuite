using KnxHelden.SHES.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace KnxHelden.SHES.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase, new()
    {
        #region --- Fields ---

        protected readonly ILogger _logger;
        protected readonly ShesDbContext _dbContext;

        #endregion

        #region --- Constructor ---

        /// <summary>Initializes a new instance of the <see cref="Repository{TEntity}" /> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <param name="dbContext">The database context.</param>
        public Repository(ILogger logger, ShesDbContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        #endregion

        #region --- IRepository ---

        /// <summary>Gets all entities asynchronous.</summary>
        /// <param name="orderBy">The property to order by.</param>
        /// <returns>Returns a list of all entities.</returns>
        public async Task<List<TEntity>> GetAllAsync(string orderBy = "")
        {
            try
            {
                var query = this._dbContext.Set<TEntity>().AsQueryable();

                // Order by
                if (!string.IsNullOrWhiteSpace(orderBy))
                {
                    query = query.OrderBy(orderBy);
                }

                return await this._dbContext.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        /// <summary>Gets an entity by identifier asynchronous.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an entity.</returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            try
            {
                var query = this._dbContext.Set<TEntity>().Where(e => e.Id == id);

                return await query.SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        /// <summary>Gets an entity by expression asynchronous.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="orderBy">The property to order by.</param>
        /// <returns>Returns an entity.</returns>
        public async Task<List<TEntity>> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, string orderBy = "")
        {
            try
            {
                var query = this._dbContext.Set<TEntity>().Where(expression);

                // Order by
                if(!string.IsNullOrWhiteSpace(orderBy))
                {
                    query = query.OrderBy(orderBy);
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        /// <summary>Adds an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity - Entity must not be null.</exception>
        public async Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                // Set creation date and last modification time
                entity.CreationTime = DateTime.Now;
                entity.LastModificationTime = DateTime.Now;

                await this._dbContext.AddAsync(entity);
                await this._dbContext.SaveChangesAsync();
                this._logger.LogInformation($"A new entity (Id: {entity.Id}) was added.");
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        /// <summary>Updates an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        /// <exception cref="System.ArgumentNullException">entity - Entity must not be null.</exception>
        public async Task UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity must not be null");
            }

            try
            {
                this.DetachEntity(entity);

                // Set last modification time
                entity.LastModificationTime = DateTime.Now;

                // set Modified flag in your entry
                this._dbContext.Entry(entity).State = EntityState.Modified;
                await this._dbContext.SaveChangesAsync();
                this._logger.LogInformation($"An existing entity (Id: {entity.Id}) was updated.");
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        /// <summary>Deletes an entity asynchronous.</summary>
        /// <param name="entity">The entity.</param>
        public async Task DeleteAsync(TEntity entity)
        {
            try
            {
                DetachEntity(entity);
                this._dbContext.Remove(entity);
                await this._dbContext.SaveChangesAsync();
                this._dbContext.ChangeTracker.Clear();

                this._logger.LogInformation($"An existing entity (Id: {entity.Id}) was deleted.");
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion

        /// <summary>Detaches an entity.</summary>
        /// <param name="entity">The entity.</param>
        private void DetachEntity(TEntity entity)
        {
            var localEntity = this._dbContext.Set<TEntity>().Local.FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            // check if local is not null 
            if (localEntity != null)
            {
                // detach
                this._dbContext.Entry(localEntity).State = EntityState.Detached;
            }
        }
    }
}

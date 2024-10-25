using BenzForum.Data.EntityInterfaceModel;
using BenzForum.Helpers;
using ForumApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ForumApp.Repositories
{
    public class Repository<DB> : IRepository<DB> where DB : class, IEntity
    {
        private readonly ForumContext _context;
        private readonly DbSet<DB> _dbSet;
        private readonly ILogger<Repository<DB>> _logger;

        public Repository(ForumContext context, ILogger<Repository<DB>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<DB>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<DB>> GetAll()
        {
            try
            {
                return await _dbSet.IncludeAll().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all entities.");
                throw;
            }
        }

        public async Task<IEnumerable<DB>> GetSome(Expression<Func<DB, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).IncludeAll().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting some entities.");
                throw;
            }
        }

        public async Task<DB> Get(Expression<Func<DB, bool>> predicate)
        {
            try
            {
                return await _dbSet.Where(predicate).IncludeAll().FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting an entity.");
                throw;
            }
        }

        public async Task<DB> Add(DB entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an entity.");
                throw;
            }
        }

        public async Task<DB> Update(DB entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating an entity.");
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var entity = await Get(e => e.Id == id);
                if (entity == null)
                {
                    throw new InvalidOperationException($"Entity with id {id} not found.");
                }
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting an entity.");
                throw;
            }
        }
    }
}



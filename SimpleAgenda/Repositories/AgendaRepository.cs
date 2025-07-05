/*
 The meaning of this code snippet is to implement a generic repository pattern for managing entities in a SQLite database using Entity Framework Core. The `AgendaRepository<T>` class provides methods for CRUD operations (Create, Read, Update, Delete) on entities of type `T`, where `T` is constrained to be a class. The repository interacts with a `SqliteContext` to perform database operations asynchronously.
 */
using Microsoft.EntityFrameworkCore;
using SimpleAgenda.Context;
using SimpleAgenda.DTOS.Internals;
using SimpleAgenda.DTOS.Publics;
using SimpleAgenda.Entities;
using SimpleAgenda.Interfaces;

namespace SimpleAgenda.Repositories
{
    internal class AgendaRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        internal AgendaRepository(string? connectionString=null)
        {
            _context = connectionString is null 
                ? new SqliteContext()
                : new SqliteContext(connectionString);

            ContextStarter(out this._dbSet!, _context);
        }

        internal AgendaRepository(DbContext context)
        {
            _context = context;
            ContextStarter(out this._dbSet!, _context);
        }

        private void ContextStarter(out DbSet<T>? dbSet, DbContext context)
        {
            dbSet = _context.Set<T>();

            if (context is IContext iContext)
                iContext.EnsureCreated();
            else
                throw new ArgumentException("Context must implement IContext interface.");
        }

        public void DeleteDb()
                {
            if (_context is IContext iContext)
                iContext.EnsureDeleted();
            else
                throw new ArgumentException("Context must implement IContext interface.");
        }
        
        // Get by ID
        public async Task<T?> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        // Get list of all entities
        public async Task<List<T>> GetList()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetList(QueryDto param)
        {
            // Grantee that the provided context is a DbContext implementation
            if (_context is not DbContext dbContext)
                throw new InvalidOperationException("O IContext precisa herdar de DbContext para permitir queries avançadas.");

            // Bring the Events and Location properties into the query - Otherwise, they will return null
            IQueryable<AppointmentDto> query = dbContext.Set<AppointmentDto>()
                .Include(a => a.Event)
                .ThenInclude(e => e.Location)
                .AsQueryable();

        }

        // Create new entity
        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, T entity)
        {
            var existingEntity = await Get(id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Delete entity by ID
        public async Task Delete(int id)
        {
            var entity = await Get(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }


        private void SetQueryParam(IQueryable<T> )
    
    }
}

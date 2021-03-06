using ChargingStations.Domain.Shared;
using ChargingStations.Domain.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStations.Infrastructure.Repositories
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly ApplicationDbContext Context;

        public virtual DbSet<TEntity> DbSet => Context.Set<TEntity>();

        public Repository(ApplicationDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected virtual IQueryable<TEntity> Set()
        {
            return Context.Set<TEntity>();
        }

        public virtual async Task<List<TEntity>> GetAsync()
        {
            var result = await Set().AsTracking().ToListAsync();
            return result.Any() ? result : null;
        }
        
        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await Set().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }

        public virtual void Remove(TPrimaryKey id)
        {
            var entity = Context.Set<TEntity>().Find(id);

            Context.Set<TEntity>().Remove(entity);
        }

        public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
    }
}

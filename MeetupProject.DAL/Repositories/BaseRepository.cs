using MeetupProject.DAL.Contextes;
using Microsoft.EntityFrameworkCore;

namespace MeetupProject.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly EventDbContext Context;
        protected DbSet<T> DbSet;

        protected BaseRepository(EventDbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await DbSet.AddAsync(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> DeleteAsync(T entity)
        {
            DbSet.Remove(entity);

            await Context.SaveChangesAsync();

            return entity;
        }

        public IQueryable<T> GetAllAsync()
        {
            var entities = DbSet.AsQueryable();

            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);

            return entity;
        }

        public async Task<T> UpdateAsync(Guid id, T entity)
        {
            DbSet.Update(entity);

            await Context.SaveChangesAsync();

            return entity;
        }
    }
}

using DataLayer.DatabaseEntities;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Implementations
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly DatabaseContext DBContext;
        protected DbSet<T> DbSet { get; set; }

        public BaseRepository(DatabaseContext dbContext)
        {
            DBContext = dbContext;
            DbSet = DBContext.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> conditions)
        {
            return DbSet.Where(conditions);
        }

        public bool Any(Expression<Func<T, bool>> conditions)
        {
            return DbSet.Any(conditions);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public T Add(T entity)
        {
            return DbSet.Add(entity).Entity;
        }

        public T Update(T entity)
        {
            return DbSet.Update(entity).Entity;
        }

        public void Remove(T entity)
        {
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        public async Task CommitAsync()
        {
            await DBContext.SaveChangesAsync();
        }
    }
}

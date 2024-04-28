using DataLayer.DatabaseEntities;
using System.Linq.Expressions;

namespace DataLayer.Repositories.Interfaces
{
    public interface IBaseRepository<T>
        where T : BaseEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> conditions);
        bool Any(Expression<Func<T, bool>> conditions);
        Task<T> GetByIdAsync(int id);
        T Add(T entity);
        T Update(T entity);
        void Remove(T entity);
        Task CommitAsync();
    }
}

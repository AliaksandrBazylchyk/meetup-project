namespace MeetupProject.DAL.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        IQueryable<T> GetAllAsync();
        Task<T> DeleteAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> CreateAsync(T entity);
    }
}

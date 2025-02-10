namespace HM.Domain.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(List<T> entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(List<T> entities, CancellationToken cancellationToken);
        Task AddAsync(T entity, CancellationToken cancellationToken);
        Task AddAsync(List<T> entity, CancellationToken cancellationToken);
    }
}

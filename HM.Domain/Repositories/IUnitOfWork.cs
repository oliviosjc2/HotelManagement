namespace HM.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task<int> CommitAsync(CancellationToken cancellationToken);
        void Detach<TEntity>(TEntity entity) where TEntity : class;
    }
}

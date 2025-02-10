using HM.Domain.Repositories;
using HM.Infra.Context;
using HM.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HM.Infra.UOW
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly HMContext _context;
        private bool _disposed = false;

        public UnitOfWork(HMContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(_context);
        }

        public void SetEntityState<TEntity>(TEntity entity, EntityState state) where TEntity : class
        {
            _context.Entry(entity).State = state;
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Attach(entity);
        }

        public void Detach<TEntity>(TEntity entity) where TEntity : class
        {
            var entry = _context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Detached;
            }
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
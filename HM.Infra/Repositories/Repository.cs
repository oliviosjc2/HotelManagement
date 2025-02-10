using HM.Domain.Repositories;
using HM.Infra.Context;

namespace HM.Infra.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly HMContext _context;

        public Repository(HMContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public async Task AddAsync(List<T> entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddRangeAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(T entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Remove(entity));
        }

        public Task DeleteAsync(List<T> entities, CancellationToken cancellationToken)
        {
            _context.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>().AsQueryable();
        }

        public Task UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            return Task.FromResult(_context.Update(entity));
        }

        public Task UpdateAsync(List<T> entity, CancellationToken cancellationToken)
        {
            _context.UpdateRange(entity);
            return Task.CompletedTask;
        }
    }
}

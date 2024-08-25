using B1TestTask.DataAccess.Context;
using B1TestTask.DataAccess.Repositories.Interfaces;
using B1TestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1TestTask.DataAccess.Repositories.Implementations
{
    public class ExelRowRepository : IExelRowRepository
    {
        private readonly TaskDbContext _dbContext;

        public ExelRowRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExelRow?> CreateAsync(ExelRow entity, CancellationToken token = default)
        {
            var created = await _dbContext.ExelRows.AddAsync(entity, token);

            if (created != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return created?.Entity;
        }

        public async Task DeleteAsync(ExelRow entity, CancellationToken token = default)
        {
            var deleted = _dbContext.ExelRows.Remove(entity);

            if (deleted != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<ExelRow?> GetAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.ExelRows.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<IEnumerable<ExelRow>> GetByPredicateAsync(Expression<Func<ExelRow, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.ExelRows.Where(predicate).ToListAsync(token);
        }

        public async Task<ExelRow?> UpdateAsync(ExelRow entity, CancellationToken token = default)
        {
            var updated = _dbContext.ExelRows.Update(entity);

            if (updated != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return updated?.Entity;
        }
    }
}

using B1TestTask.DataAccess.Context;
using B1TestTask.DataAccess.Repositories.Interfaces;
using B1TestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1TestTask.DataAccess.Repositories.Implementations
{
    public class ExelReportRepository : IExelReportRepository
    {
        private readonly TaskDbContext _dbContext;

        public ExelReportRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ExelReport?> CreateAsync(ExelReport entity, CancellationToken token = default)
        {
            var created = await _dbContext.Reports.AddAsync(entity, token);

            if (created != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return created?.Entity;
        }

        public async Task DeleteAsync(ExelReport entity, CancellationToken token = default)
        {
            var deleted = _dbContext.Reports.Remove(entity);

            if (deleted != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<ExelReport?> GetAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Reports.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<IEnumerable<ExelReport>> GetByPredicateAsync(Expression<Func<ExelReport, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.Reports.Where(predicate).ToListAsync(token);
        }

        public async Task<ExelReport?> UpdateAsync(ExelReport entity, CancellationToken token = default)
        {
            var updated = _dbContext.Reports.Update(entity);

            if (updated != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return updated?.Entity;
        }
    }
}

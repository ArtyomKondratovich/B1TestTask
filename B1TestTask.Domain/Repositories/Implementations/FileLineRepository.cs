using B1TestTask.DataAccess.Context;
using B1TestTask.Domain.Entities;
using B1TestTask.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1TestTask.Domain.Repositories.Implementations
{
    public class FileLineRepository : IFileLineRepository
    {
        private readonly TaskDbContext _dbContext;

        public FileLineRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FileLine?> CreateAsync(FileLine entity, CancellationToken token = default)
        {
            var created = await _dbContext.FileLines.AddAsync(entity, token);   

            if (created != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return created?.Entity;
        }

        public async Task DeleteAsync(FileLine entity, CancellationToken token = default)
        {
            var deleted = _dbContext.FileLines.Remove(entity);

            if (deleted != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<FileLine?> GetAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.FileLines.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<IEnumerable<FileLine>> GetByPredicateAsync(Expression<Func<FileLine, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.FileLines.Where(predicate).ToListAsync(token);
        }

        public async Task SaveBatchOfLinesAsync(IEnumerable<FileLine> lines)
        {
            await _dbContext.AddRangeAsync(lines);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<FileLine?> UpdateAsync(FileLine entity, CancellationToken token = default)
        {
            var updated = _dbContext.FileLines.Update(entity);

            if (updated != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return updated?.Entity;
        }
    }
}

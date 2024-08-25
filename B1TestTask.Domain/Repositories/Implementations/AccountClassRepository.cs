using B1TestTask.DataAccess.Context;
using B1TestTask.DataAccess.Repositories.Interfaces;
using B1TestTask.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace B1TestTask.DataAccess.Repositories.Implementations
{
    public class AccountClassRepository : IAccountClassRepository
    {
        private readonly TaskDbContext _dbContext;

        public AccountClassRepository(TaskDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BankAccountClass?> CreateAsync(BankAccountClass entity, CancellationToken token = default)
        {
            var created = await _dbContext.AccountClasses.AddAsync(entity, token);

            if (created != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return created?.Entity;
        }

        public async Task DeleteAsync(BankAccountClass entity, CancellationToken token = default)
        {
            var deleted = _dbContext.AccountClasses.Remove(entity);

            if (deleted != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }
        }

        public async Task<BankAccountClass?> GetAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.AccountClasses.FirstOrDefaultAsync(x => x.Id == id, token);
        }

        public async Task<IEnumerable<BankAccountClass>> GetByPredicateAsync(Expression<Func<BankAccountClass, bool>> predicate, CancellationToken token = default)
        {
            return await _dbContext.AccountClasses.Where(predicate).ToListAsync(token);
        }

        public async Task<BankAccountClass?> UpdateAsync(BankAccountClass entity, CancellationToken token = default)
        {
            var updated = _dbContext.AccountClasses.Update(entity);

            if (updated != null)
            {
                await _dbContext.SaveChangesAsync(token);
            }

            return updated?.Entity;
        }
    }
}

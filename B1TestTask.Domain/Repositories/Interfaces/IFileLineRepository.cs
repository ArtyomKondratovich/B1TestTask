using B1TestTask.Domain.Entities;

namespace B1TestTask.Domain.Repositories.Interfaces
{
    public interface IFileLineRepository : IRepositoryBase<FileLine>
    {
        Task SaveBatchOfLinesAsync(IEnumerable<FileLine> lines);
    }
}

using B1TestTask.Domain.Entities;

namespace B1TestTask.Main.Services
{
    interface IExelService
    {
        Task<ExelData?> SaveFileAsync(string filePath);
        Task<ExelData?> DownloadFileAsync(string filePath);
        Task<IEnumerable<ExelReport>> GetReportsAsync();
    }
}

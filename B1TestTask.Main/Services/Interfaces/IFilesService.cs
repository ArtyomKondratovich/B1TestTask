using B1TestTask.Main.Utilities.Events;

namespace B1TestTask.Main.Services
{
    internal interface IFilesService
    {
        event EventHandler<ProgressUpdatedEventArgs> ProgressUpdated;
        event EventHandler<LoadedLinesEventArgs> LoadedLinesUpdated;
        Task<string> GenerateFilesAsync(string directoryPath);
        Task<int> MergeFiles(string directoryPath, Func<string, bool> predicate);
        Task SaveFile(string fileName);
    }
}

namespace B1TestTask.Main.Services
{
    internal interface IFilesService
    {
        Task<string> GenerateFilesAsync(string directoryPath);
        Task<int> MergeFiles(string directoryPath, Func<string, bool> predicate);
    }
}

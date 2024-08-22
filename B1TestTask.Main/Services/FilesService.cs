using B1TestTask.Domain.Entities;
using B1TestTask.Domain.Repositories.Impllementations;
using B1TestTask.Domain.Repositories.Interfaces;
using B1TestTask.Main.Utilities.Events;
using B1TestTask.Main.Utilities.Extensions;
using System.IO;

namespace B1TestTask.Main.Services
{
    internal class FilesService : IFilesService
    {
        private readonly IFileLineRepository _lineRepository;
        private readonly IFileRepository _fileRepository;

        public event EventHandler<ProgressUpdatedEventArgs> ProgressUpdated;
        public event EventHandler<LoadedLinesEventArgs> LoadedLinesUpdated;

        private string _currentDestinationDirectorypath = string.Empty;
        private const string _latinChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _russianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        

        private static readonly Random _random = new();
        private static readonly DateTime _startDate = new(2019, 1, 1);
        private static readonly int _daysRange = (DateTime.Today - _startDate).Days;
        private static int _linesSaved = 0;

        public FilesService(IFileLineRepository lineRepository, IFileRepository fileRepository) 
        {
            _lineRepository = lineRepository;
            _fileRepository = fileRepository;
        }

        public async Task<string> GenerateFilesAsync(string sourceDirectoryPath)
        {
            var timeNow = DateTime.UtcNow.ToString("yyyy-MM-dddd-HH-mm-ss");

            _currentDestinationDirectorypath = Path.Combine(sourceDirectoryPath,timeNow);

            if (!Directory.Exists(_currentDestinationDirectorypath)) 
            {
                Directory.CreateDirectory(_currentDestinationDirectorypath);           
            }

            for (var i = 1; i <= 100; i++)
            {
                var fileName = Path.Combine(_currentDestinationDirectorypath, $"{i}.txt");

                using var fileStream = File.Open(fileName, FileMode.Create);
                using var bufferedStream = new BufferedStream(fileStream);
                using var streamWriter = new StreamWriter(bufferedStream);

                await foreach (var line in GenerateLinesAsync())
                {
                    await streamWriter.WriteLineAsync(line);
                }
            }

            return new string(_currentDestinationDirectorypath);
        }

        public async Task<int> MergeFiles(string directoryPath, Func<string, bool> predicate)
        {
            var files = Directory
                .GetFiles(directoryPath)
                .Where(x => x.EndsWith(".txt")
                    && int.TryParse(Path.GetFileName(x)[..^4], out _))
                .ToList();

            var mergedFileName = Path.Combine(directoryPath, $"merged-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.txt");

            var validLinesSum = 0;

            foreach (var file in files) 
            {
                using var fileStream = File.Open(mergedFileName, FileMode.Append);
                using var bufferedStream = new BufferedStream(fileStream);
                using var fileStreamWriter = new StreamWriter(bufferedStream);

                await foreach (var line in GetFileLinesAsync(file, predicate)) 
                {
                    await fileStreamWriter.WriteLineAsync(line);
                    validLinesSum++;
                }
            }

            return validLinesSum;
        }

        private static async IAsyncEnumerable<string> GetFileLinesAsync(string fileName, Func<string, bool> predicate) 
        {
            using var fileStream = File.OpenRead(fileName);
            using var fileStreamReader = new StreamReader(fileStream);

            while (!fileStreamReader.EndOfStream)
            {
                var line = await fileStreamReader.ReadLineAsync();

                if (string.IsNullOrEmpty(line) || !predicate(line)) 
                {
                    continue;
                }

                yield return line;
            }
        }

        private static IAsyncEnumerable<string> GenerateLinesAsync(int count = 100_000)
        {
            return AsyncEnumerable.Range(0, count).Select(_ => GenerateLine());
        }

        private static string GenerateLine()
        {
            var randomDate = _startDate.AddDays(_random.Next(_daysRange));

            var randomDateStr = randomDate.ToString("dd.MM.yyyy");

            var randomLatinChars = new string(Enumerable.Repeat(_latinChars, 10)
                                    .Select(s => s[_random.Next(s.Length)])
                                    .ToArray());

            var randomRussianChars = new string(Enumerable.Repeat(_russianLetters, 10)
                                    .Select(s => s[_random.Next(s.Length)])
                                    .ToArray());

            var randomEvenNumber = (_random.Next(1, 50_000_000) * 2).ToString();

            var randomDoubleNumber = Math.Round(_random.NextDouble() * 19 + 1, 8);

            return $"{randomDateStr}||{randomLatinChars}||{randomRussianChars}||{randomEvenNumber}||{randomDoubleNumber}";
        }

        protected virtual void OnProgressUpdated(ProgressUpdatedEventArgs e)
        {
            ProgressUpdated?.Invoke(this, e);
        }

        protected virtual void OnLoadedLinesUpdated(LoadedLinesEventArgs e)
        {
            LoadedLinesUpdated?.Invoke(this, e);
        }

        public async Task SaveFile(string fileName)
        {
            _linesSaved = 0;

            if (!fileName.IsValidMergedFile()) 
            {
                return;
            }

            var mergedFile = new MergedFile
            {
                FileName = fileName
            };

            var fileResult = await _fileRepository.CreateAsync(mergedFile);

            if (fileResult != null)
            {
                await foreach (var line in GetFileLinesAsync(fileName, (s) => true))
                {
                    var fileLine = line.Parse();
                    fileLine.MergedFile = fileResult;

                    if (await _lineRepository.CreateAsync(fileLine) != null)
                    {
                        OnLoadedLinesUpdated(new LoadedLinesEventArgs(++_linesSaved));
                    }
                }
            }
        }
    }
}

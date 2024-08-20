using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace B1TestTask.Main.Services
{
    internal class FilesService : IFilesService
    {
        private string _currentDestinationDirectorypath = string.Empty;
        private const string _latinChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string _russianLetters = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";

        public FilesService() {}

        public async Task<string> GenerateFilesAsync(string sourceDirectoryPath)
        {
            var timeNow = DateTime.UtcNow.ToString("yyyy-MM-dddd-HH-mm-ss");

            _currentDestinationDirectorypath = Path.Combine(sourceDirectoryPath,timeNow);

            if (!Directory.Exists(_currentDestinationDirectorypath)) 
            {
                Directory.CreateDirectory(_currentDestinationDirectorypath);           
            }

            await Task.WhenAll(Enumerable.Range(0, 4).Select(GenerateBatchFiles));

            return new string(_currentDestinationDirectorypath);
        }

        public async Task<int> MergeFiles(string directoryPath, Func<string, bool> predicate)
        {
            var files = Directory.GetFiles(directoryPath).Where(x => x.EndsWith(".txt")).ToList();

            var mergedFileName = Path.Combine(directoryPath, $"merged-{DateTime.UtcNow:yyyy-MM-dd-HH-mm-ss}.txt");

            var validLinesSum = 0;

            foreach (var file in files) 
            {
                var lines = GetFileLines(file, predicate);

                using var fileStream = File.Open(mergedFileName, FileMode.Append);
                using var fileStreamWriter = new StreamWriter(fileStream);

                await foreach (var line in lines) 
                {
                    await fileStreamWriter.WriteLineAsync(line);
                    validLinesSum++;
                }
            }

            return validLinesSum;
        }

        private static async IAsyncEnumerable<string> GetFileLines(string fileName, Func<string, bool> predicate) 
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

        private async Task GenerateBatchFiles(int batchIndex) 
        {
            var builder = new StringBuilder();

            for (var i = 1; i <= 25; i++) 
            {
                var fileName = Path.Combine(_currentDestinationDirectorypath, (batchIndex * 25 + i).ToString() + ".txt");

                using (var writer = new StreamWriter(fileName, false, Encoding.UTF8)) 
                {

                    for (var j = 0; j < 100_000; j++)
                    {
                        builder.AppendLine(GenerateLine());
                    }

                    await writer.WriteAsync(builder.ToString());

                }

                builder.Clear();
            }
        }

        private static string GenerateLine() 
        {
            var random = new Random();

            var startDate = new DateTime(2019, 1, 1);
            var daysRange = (DateTime.Today - startDate).Days;
            var randomDate = startDate.AddDays(random.Next(daysRange));

            var randomDateStr = randomDate.ToString("dd.MM.yyyy");

            var randomLatinChars = new string(Enumerable.Repeat(_latinChars, 10)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());

            var randomRussianChars = new string(Enumerable.Repeat(_russianLetters, 10)
                                        .Select(s => s[random.Next(s.Length)])
                                        .ToArray());

            var randomEvenNumber = (random.Next(1,50_000_000) * 2).ToString();

            var randomDoubleNumber = Math.Round(random.NextDouble() * 19 + 1, 8);

            return $"{randomDateStr}||{randomLatinChars}||{randomRussianChars}||{randomEvenNumber}||{randomDoubleNumber}";
        }
    }
}

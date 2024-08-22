using B1TestTask.Domain.Entities;
using System.IO;

namespace B1TestTask.Main.Utilities.Extensions
{
    internal static class HelpExtensions
    {
        public static Func<string, bool> GetPredicate(this string? str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return (x) => true;
            }

            return (x) => !x.Contains(str);
        }

        public static bool IsValidMergedFile(this string? fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) 
            {
                return false;
            }

            var onlyName = Path.GetFileNameWithoutExtension(fileName);
            var fileInfo = new FileInfo(fileName);

            return onlyName.Contains("merged") && fileInfo.Length > 6e+8;
        }

        public static FileLine Parse(this string line)
        {
            var fields = line.Split("||");

            var dateParts = fields[0].Split('.').Select(x => int.Parse(x)).ToList();

            var date = DateTime.SpecifyKind(new DateTime(dateParts[2], dateParts[1], dateParts[0]), DateTimeKind.Utc);

            return new FileLine
            {
                Date = date,
                LatinStr = fields[1],
                RussianStr = fields[2],
                IntNumber = int.Parse(fields[3]),
                DoubleNumber = double.Parse(fields[4])
            };
        }
    }
}

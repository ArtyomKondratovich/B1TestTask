using Aspose.Cells;
using B1TestTask.DataAccess.Repositories.Interfaces;
using B1TestTask.Domain.Entities;
using B1TestTask.Main.Utilities.Extensions;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace B1TestTask.Main.Services
{
    class ExelService : IExelService
    {
        private readonly IExelReportRepository _reportRepository;
        private readonly IExelRowRepository _rowsRepository;
        private readonly IAccountClassRepository _accountClassRepository;

        public ExelService(IExelReportRepository reportRepository, IExelRowRepository rowRepository, IAccountClassRepository accountClassRepository)
        {
            _reportRepository = reportRepository;
            _rowsRepository = rowRepository;
            _accountClassRepository = accountClassRepository;
        }

        public async Task<ExelData?> DownloadFileAsync(string filePath)
        {
            var exelReport = await _reportRepository.GetByPredicateAsync((x) => x.FilePath == filePath);

            if (exelReport == null || !exelReport.Any())
            {
                return null;
            }

            var report = exelReport.First();
            var classes = await _accountClassRepository.GetByPredicateAsync((x) => true);
            var rows = await _rowsRepository.GetByPredicateAsync((x) => x.Report.Id == report.Id);

            return new ExelData
            {
                Report = report,
                Classes = classes.ToList(),
                Rows = rows.ToList(),
            };
        }

        public async Task<IEnumerable<ExelReport>> GetReportsAsync()
        {
            return await _reportRepository.GetByPredicateAsync((x) => true);
        }

        public async Task<ExelData?> SaveFileAsync(string filePath)
        {
            var result = new ExelData();

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            var exel = new Workbook(filePath);

            var workSheet = exel.Worksheets[0];

            var rows = workSheet.Cells.MaxDataRow;
            var dates = workSheet.Cells[2, 0].StringValue.Parse();

            var report = new ExelReport
            {
                FilePath = filePath,
                BankName = workSheet.Cells[0, 0].StringValue,
                Description = workSheet.Cells[1, 0].StringValue,
                StartDate = dates.Item1,
                EndDate = dates.Item2,
                ReportDate = DateTime.SpecifyKind(workSheet.Cells[5, 0].DateTimeValue, DateTimeKind.Utc)
            };

            if ((await _reportRepository.GetByPredicateAsync((x) => x.FilePath == filePath)).Any())
            {
                return null;
            }

            var reportEntity = await _reportRepository.CreateAsync(report);
            
            var classes = new List<BankAccountClass>();
            var exelRows = new List<ExelRow>();

            for (var i = 8; i < rows; i++)
            {
                if (workSheet.Cells[i, 0].StringValue.StartsWith("КЛАСС"))
                {
                    var parts = workSheet.Cells[i, 0].StringValue.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    var name = string.Join(" ", parts[..2]);
                    var desciprtion = string.Join(" ", parts[2..]);

                    classes.Add(new BankAccountClass
                    {
                        Description = desciprtion,
                        Name = name
                    });

                    continue;
                }
                else if (workSheet.Cells[i, 0].StringValue.StartsWith("ПО"))
                {
                    continue;
                }
                else if (workSheet.Cells[i, 0].StringValue.StartsWith("БАЛАНС"))
                {
                    continue;
                }


                exelRows.Add(new ExelRow
                {
                    Report = report,
                    BankAccount = int.Parse(workSheet.Cells[i, 0].StringValue),
                    Class = classes.Last(),
                    OpeningBalanceAsset = workSheet.Cells[i,1].DoubleValue,
                    OpeningBalanceLiability = workSheet.Cells[i,2].DoubleValue,
                    TurnoverDebit = workSheet.Cells[i, 3].DoubleValue,
                    TurnoverCredit = workSheet.Cells[i, 4].DoubleValue,
                    ClosedBalanceAsset = workSheet.Cells[i,5].DoubleValue,
                    ClosedBalanceLiability = workSheet.Cells[i,6].DoubleValue,
                });
            }

            await foreach (var accountClass in SaveAccountClasses(classes))
            {
                result.Classes.Add(accountClass);
            }

            foreach (var row in exelRows)
            {
                row.Class = result.Classes.Find((x) => x.Name ==  row.Class.Name);
                await _rowsRepository.CreateAsync(row);
            }

            return new ExelData 
            {
                Report = report,
                Rows = exelRows
            };
        }

        private async IAsyncEnumerable<BankAccountClass> SaveAccountClasses(IEnumerable<BankAccountClass> classes)
        {
            foreach (var accountClass in classes) 
            {
                var existingClass = await _accountClassRepository.GetByPredicateAsync((x) => x.Name == accountClass.Name);

                if (existingClass != null && existingClass.Any())
                {
                    yield return existingClass.First();
                }

                yield return await _accountClassRepository.CreateAsync(accountClass);
            }
        }
    }
}

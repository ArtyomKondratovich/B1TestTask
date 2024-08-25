using B1TestTask.Domain.Entities;

namespace B1TestTask.Main.Services
{
    class ExelData
    {
        public ExelReport Report { get; set; }
        public List<BankAccountClass> Classes { get; set; } = new();
        public List<ExelRow> Rows { get; set; } = new();
    }
}

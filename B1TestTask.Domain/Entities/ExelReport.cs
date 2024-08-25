namespace B1TestTask.Domain.Entities
{
    public class ExelReport
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string BankName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReportDate { get; set; }

        public override string ToString()
        {
            return Path.GetFileName(FilePath);
        }
    }
}

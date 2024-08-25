namespace B1TestTask.Domain.Entities
{
    public class ExelRow
    {
        public int Id { get; set; }

        public ExelFile ExelFile { get; set; }

        public int BankAccount {  get; set; }

        public BankAccountClass Class { get; set; }

        public OpeningBalance OpeningBalance { get; set; }

        public ClosingBalance ClosedBalance { get; set; }

        public Turnover Turnover { get; set; }
    }

    public struct OpeningBalance 
    {
        public decimal Asset { get; set; }
        public decimal Liability { get; set; }
    }

    public struct ClosingBalance 
    {
        public decimal Asset { get; set; }
        public decimal Liability { get; set; }
    }

    public struct Turnover
    {
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}

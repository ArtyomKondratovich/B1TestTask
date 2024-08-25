namespace B1TestTask.Domain.Entities
{
    public class ExelRow
    {
        public int Id { get; set; }
        public ExelReport Report { get; set; }
        public int BankAccount { get; set; }
        public BankAccountClass Class { get; set; }
        public double OpeningBalanceAsset { get; set; }
        public double OpeningBalanceLiability { get; set; }
        public double ClosedBalanceAsset { get; set; }
        public double ClosedBalanceLiability { get; set; }
        public double TurnoverDebit { get; set; }
        public double TurnoverCredit { get; set; }
    }
}

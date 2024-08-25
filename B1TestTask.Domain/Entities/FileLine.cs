namespace B1TestTask.Domain.Entities
{
    public class FileLine
    {
        public int Id { get; set; }
        public MergedFile MergedFile { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string LatinStr { get; set; }
        public string RussianStr { get; set; }
        public int IntNumber { get; set; }
        public double DoubleNumber { get; set; }
    }
}

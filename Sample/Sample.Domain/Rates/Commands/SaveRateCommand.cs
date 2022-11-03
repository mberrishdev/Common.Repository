namespace Sample.Domain.Rates.Commands
{
    public class SaveRateCommand
    {
        public string Iso1 { get; set; }
        public string Iso2 { get; set; }
        public int CurrentRate { get; set; }
        public int Quantity { get; set; }
        public int Diff { get; set; }
        public DateTime ValidFromDate { get; set; }
    }
}

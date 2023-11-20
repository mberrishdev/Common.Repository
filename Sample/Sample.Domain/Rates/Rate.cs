using Sample.Domain.Rates.Commands;
using System.ComponentModel.DataAnnotations;

namespace Sample.Domain.Rates
{
    public class Rate
    {
        [Key, Required]
        public int Id { get; set; }
        [Required]
        public string Iso1 { get; set; }
        [Required]
        public string Iso2 { get; set; }
        [Required]
        public int CurrentRate { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int Diff { get; set; }
        [Required, DataType(DataType.DateTime)]
        public DateTime ValidFromDate { get; set; }

        public Rate() { }
        public Rate(SaveRateCommand rate)
        {
            Iso1 = rate.Iso1;
            Iso2 = rate.Iso2;
            CurrentRate = rate.CurrentRate;
            Quantity = rate.Quantity;
            Diff = rate.Diff;
            ValidFromDate = rate.ValidFromDate;
        }

        public void Update(SaveRateCommand command)
        {
            Iso1 = command.Iso1;
            Iso2 = command.Iso2;
            CurrentRate = command.CurrentRate;
            Quantity = command.Quantity;
            Diff = command.Diff;
            ValidFromDate = command.ValidFromDate;
        }
    }
}

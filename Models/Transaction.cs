using System;
using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class Transaction
    {
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Directions Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        public string Currency { get; set; }
        public MCC? Mcc { get; set; }
        [Required]
        public TransactionKind? Kind { get; set; }
        public string CatCode { get; set; }
    }
}
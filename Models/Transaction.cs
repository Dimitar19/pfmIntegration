using System;
using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class Transaction
    {
        [Required]
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public Directions Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        [MinLength(3), MaxLength(3)]
        public string Currency { get; set; }
        public MCC? Mcc { get; set; }
        [Required]
        public TransactionKind? Kind { get; set; }
        public string Catcode { get; set; }
    }
}
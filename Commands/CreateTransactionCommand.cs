using System;
using System.ComponentModel.DataAnnotations;
using pfm.Models;

namespace pfm.Commands
{
    public class CreateTransactionCommand
    {
        [Required]
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public Directions? Direction { get; set; }
        [Required]
        public double? Amount { get; set; }
        public string Description { get; set; }
        [Required]
        public string Currency { get; set; }
        public MCC? mcc { get; set; }
        [Required] 
        public TransactionKind? Kind { get; set; }
    }
}
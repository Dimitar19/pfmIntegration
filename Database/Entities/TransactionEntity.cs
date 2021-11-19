using System;
using System.Collections.Generic;
using pfm.Models;

namespace pfm.Database.Entities
{
    public class TransactionEntity
    {
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        public Directions Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public MCC? Mcc { get; set; }
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; }
        public CategoryEntity Category { get; set; }
        public List<TransactionSplitEntity> Splits { get; set; }
    }
}
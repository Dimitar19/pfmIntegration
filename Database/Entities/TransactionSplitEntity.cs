using System.Transactions;

namespace pfm.Database.Entities
{
    public class TransactionSplitEntity
    {
        public string CatCode { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
        public TransactionEntity ParentTransaction { get; set; }
        public CategoryEntity Category  { get; set; }
    }
}
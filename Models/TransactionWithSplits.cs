using System;
using System.Text.Json.Serialization;

namespace pfm.Models
{
    public class TransactionWithSplits
    {
        public string Id { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime Date { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Directions Direction { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public MCC? Mcc { get; set; }
        //[JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionKind Kind { get; set; }
        public string CatCode { get; set; }
        public SingleCategorySplit[] Splits { get; set; }
    }
}
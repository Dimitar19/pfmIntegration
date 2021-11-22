using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfm.Models
{
    public class TransactionWithSplits
    {
        [Required]
        public string Id { get; set; }
        [JsonPropertyName("beneficiary-name")]
        public string BeneficiaryName { get; set; }
        [Required]
        public string Date { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public Directions Direction { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Description { get; set; }
        [Required]
        [MaxLength(3), MinLength(3)]
        public string Currency { get; set; }
        public MCC? Mcc { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public TransactionKind Kind { get; set; }
        public string Catcode { get; set; }
        public SingleCategorySplit[] Splits { get; set; }
    }
}
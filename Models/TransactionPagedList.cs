using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pfm.Models
{
    public class TransactionPagedList<T>
    {
        [JsonPropertyName("total-count")]
        [Range(minimum:0, maximum: Int64.MaxValue)]
        public int TotalCount { get; set; }
        [Range(1,100)]
        [JsonPropertyName("page-size")]
        public int PageSize { get; set; }
        [Range(minimum: 1, maximum: Int64.MaxValue)]
        public int Page { get; set; }
        [JsonIgnore]
        [Range(minimum: 0, maximum: Int64.MaxValue)]
        public int TotalPages { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("sort-order")]
        public SortOrder SortOrder { get; set; }
        [JsonPropertyName("sory-by")]
        public string SortBy { get; set; }
        public TransactionWithSplits[] Items { get; set; }
    }
}
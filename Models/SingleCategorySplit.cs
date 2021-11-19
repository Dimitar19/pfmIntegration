using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class SingleCategorySplit
    {
        [Required]
        public string Catcode { get; set; }
        [Required]
        public double Amount { get; set; }
    }
}
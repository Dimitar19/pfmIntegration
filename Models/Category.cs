using System.ComponentModel.DataAnnotations;

namespace pfm.Models
{
    public class Category
    {
        [Required]
        public string Code { get; set; }
        public string ParentCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
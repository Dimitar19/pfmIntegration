using System.ComponentModel.DataAnnotations;

namespace pfm.Commands
{
    public class TransactionCategorizeCommand
    {
        [Required]
        public string Catcode { get; set; }
    }
}
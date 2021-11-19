using System.ComponentModel.DataAnnotations;

namespace pfm.Commands
{
    public class CreateCategoryCommand
    {
        [Required]
        public string Code;
        [Required]
        public string Name;
        public string ParentCode;
    }
}
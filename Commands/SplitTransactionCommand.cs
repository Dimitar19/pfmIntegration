using pfm.Models;

namespace pfm.Commands
{
    public class SplitTransactionCommand
    {
        public SingleCategorySplit [] Splits { get; set; }
    }
}
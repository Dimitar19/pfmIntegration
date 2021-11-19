using System.Collections.Generic;

namespace pfm.Database.Entities
{
    public class CategoryEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ParentCode { get; set; }
        public CategoryEntity ParentCategory { get; set; }
        public List<CategoryEntity> ChildCategories { get; set; }
    }
}
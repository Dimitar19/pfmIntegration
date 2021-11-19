using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pfm.Database.Entities;

namespace pfm.Database.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(x => x.Code);
            builder.Property(x => x.Code).IsRequired();
            builder.Property(x => x.Name).IsRequired();

            builder.Property(x => x.ParentCode);

            builder.HasOne<CategoryEntity>(x => x.ParentCategory).WithMany(x => x.ChildCategories).HasForeignKey(s => s.ParentCode);

        }
    }
}
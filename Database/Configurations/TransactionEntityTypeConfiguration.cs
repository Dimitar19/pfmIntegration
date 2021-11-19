using Microsoft.EntityFrameworkCore;
using pfm.Database.Entities;

namespace pfm.Database.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.BeneficiaryName);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Direction).IsRequired().HasConversion<string>();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.Description);
            builder.Property(x => x.Currency).IsRequired();
            builder.Property(x => x.Mcc);
            builder.Property(x => x.Kind).IsRequired().HasConversion<string>();
            builder.Property(x => x.CatCode);

            builder.HasOne<CategoryEntity>(x => x.Category).WithMany().HasForeignKey(s => s.CatCode);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using pfm.Database.Entities;

namespace pfm.Database.Configurations
{
    public class SplitTransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionSplitEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<TransactionSplitEntity> builder)
        {
            builder.ToTable("transactionSplits");

            builder.HasKey(x => new {x.CatCode, x.TransactionId});

            builder.Property(x => x.CatCode).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
            builder.Property(x => x.TransactionId);

            builder.HasOne<TransactionEntity>(x => x.ParentTransaction).WithMany(x => x.Splits).HasForeignKey(x => x.TransactionId);
            builder.HasOne<CategoryEntity>(x => x.Category).WithMany().HasForeignKey(x => x.CatCode);
        }
    }
}
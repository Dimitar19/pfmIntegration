using System.Reflection;
using Microsoft.EntityFrameworkCore;
using pfm.Database.Entities;

namespace pfm.Database
{
    public class PfmDbContext : DbContext
    {
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<TransactionSplitEntity> SplitTransactions { get; set; }
        public PfmDbContext(){}
        public PfmDbContext(DbContextOptions options):base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}